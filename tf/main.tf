#Provides the configuration for the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.56.0"
    }
  }
}

# Configure the Microsoft Azure Provider
provider "azurerm" {
  features {}
}

data "azurerm_client_config" "current" {}

# Create a resource group
resource "azurerm_resource_group" "rg" {
  name     = var.azure_rg_name
  location = var.azure_region_location
  tags     = var.tags
}

resource "azurerm_network_security_group" "network_sg" {
  name                = var.azure_nsg_name
  location            = var.azure_region_location
  resource_group_name = azurerm_resource_group.rg.name
}

resource "random_password" "sql-password" {
  length           = 16
  special          = true
  min_special      = 4
  override_special = "_%@"
}



resource "azurerm_key_vault" "key_vault" {
  name                = var.azure_key_vault_name
  resource_group_name = azurerm_resource_group.rg.name
  tags                = var.tags
  sku_name            = "standard"
  location            = var.azure_region_location
  tenant_id           = data.azurerm_client_config.current.tenant_id

}

resource "azurerm_key_vault_secret" "sql-password" {
  name         = "sql-password"
  value        = random_password.sql-password.result
  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_access_policy" "key_vault_policy" {

  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  key_permissions = [
      "Get",
    ]

    secret_permissions = [
      "Get",
    ]
}

resource "azurerm_mssql_server" "sql_server" {
  name                          = "funfair-mssqlserver"
  resource_group_name           = azurerm_resource_group.rg.name
  location                      = azurerm_resource_group.rg.location
  version                       = "12.0"
  administrator_login           = var.azure_sql_user
  administrator_login_password  = random_password.sql-password.result
  minimum_tls_version           = "1.2"
  public_network_access_enabled = false
  tags                          = var.tags
}

resource "azurerm_mssql_database" "sql_database" {
  name                        = "funfair-database"
  server_id                   = azurerm_mssql_server.sql_server.id
  auto_pause_delay_in_minutes = 60
  max_size_gb                 = 5
  min_capacity                = 0.5
  read_replica_count          = 0
  read_scale                  = false
  sku_name                    = "GP_S_Gen5_1"
  zone_redundant              = false
  collation                   = "SQL_Latin1_General_CP1_CI_AS"
  threat_detection_policy {
    disabled_alerts      = []
    email_account_admins = "Disabled"
    email_addresses      = []
    retention_days       = 0
    state                = "Disabled"
  }
}

resource "azurerm_servicebus_namespace" "funfair-sb-namespace" {
  name                = "${var.azure_servicebus_name}-namespace"
  location            = var.azure_region_location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = var.azure_servicebus_sku
  tags                = var.tags

}

resource "azurerm_servicebus_queue" "example" {
  name         = "${var.azure_servicebus_name}-queue"
  namespace_id = azurerm_servicebus_namespace.funfair-sb-namespace.id


  enable_partitioning = true
}