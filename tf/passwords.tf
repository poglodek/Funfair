resource "random_password" "postgres-password" {
  length           = 16
  special          = true
  min_special      = 4
  override_special = "_%@"
}
