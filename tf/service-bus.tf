resource "azurerm_servicebus_namespace" "funfair-sb-namespace" {
  name                = "${var.azure_servicebus_name}-namespace"
  location            = var.azure_region_location
  resource_group_name = var.azure_rg_name
  sku                 = var.azure_servicebus_sku
  tags                = var.tags
  
}

resource "azurerm_servicebus_queue" "example" {
  name         = "${var.azure_servicebus_name}-queue"
  namespace_id = azurerm_servicebus_namespace.funfair-sb-namespace.id


  enable_partitioning = true
}