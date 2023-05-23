variable "azure_region_location" {
  type    = string
  default = "West Europe"
}

variable "azure_rg_name" {
  type    = string
  default = "funfair-rg"
}

variable "azure_vn_name" {
  type    = string
  default = "funfair-vn"
}


variable "azure_key_vault_name" {
  type    = string
  default = "funfair-key-vault"
}

variable "azure_postgres_user" {
  type    = string
  default = "postgresadmin"
}

variable "azure_postgres_version" {
  type    = string
  default = "12"
}

variable "azure_postgres_skuname" {
  type    = string
  default = "Standard_B2s"
}


variable "azure_postgres_ssl_enforcement_enabled" {
  type    = bool
  default = true
}

variable "azure_servicebus_name" {
  type    = string
  default = "funfair-service-bus"
}

variable "azure_servicebus_sku" {
  type    = string
  default = "Standard"
}

variable "tags" {
  type = map(string)
  default = {
    environment = "dev"
    costcenter  = "it"
    application = "funfair"
  }
}



