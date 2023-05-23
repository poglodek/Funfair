
resource "azurerm_postgresql_flexible_server" "postgresql-server" {
  name                   = "funfair-postgres-server"
  resource_group_name    = var.azure_rg_name
  location               = var.azure_region_location
  version                = var.azure_postgres_version
  administrator_login    = var.azure_postgres_user
  administrator_password = random_password.postgres-password.result
  zone                   = "1"
  tags = var.tags
  sku_name = "GP_Standard_D4s_v3"
  storage_mb = 32768
}

resource "azurerm_postgresql_flexible_server" "postgres-database-flexible" {
  name                = "funfair-postgres-server"
  resource_group_name = var.azure_rg_name
  location            = var.azure_region_location
  sku_name            = "GP_Standard_D4s_v3"
  storage_mb          = 32768
  administrator_login = "adminuser"
  administrator_password = "MyP@ssword123!"
  version             = "14"
  tags = var.tags
}