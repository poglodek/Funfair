#Provides the configuration for the Azure provider
terraform {
  required_providers {
    azurerm = {
        source = "hashicorp/azurerm"
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
  name     = "funfair_rg"
  location = var.azure_region
}