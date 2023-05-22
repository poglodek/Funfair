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

# Create a resource group
resource "azurerm_resource_group" "rg" {
  name     = var.azure_rg_name
  location = var.azure_region_location
}

resource "azurerm_postgresql_database" "database" {
  name                = "${var.azure_postgres_server_name}-users"
  resource_group_name = var.azure_rg_name

  server_name = "${var.azure_postgres_server_name}-users"
  charset     = "UTF8"
  collation   = "English_United States.1252"


}