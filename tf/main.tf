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


resource "azurerm_key_vault" "key_vault" {
  name                = var.azure_key_vault_name
  resource_group_name = var.azure_rg_name
  tags                = var.tags
  sku_name            = "standard"
  location            = var.azure_region_location
  tenant_id           = data.azurerm_client_config.current.tenant_id

}


