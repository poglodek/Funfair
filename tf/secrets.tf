resource "azurerm_key_vault_secret" "postgres-password" {
  name         = "postgres-password"
  value        = random_password.postgres-password.result
  key_vault_id = azurerm_key_vault.key_vault.id
}