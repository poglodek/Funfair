variable "azure_region_location" {
  type    = string
  default = "West Europe"
}

variable "azure_rg_name" {
  type    = string
  default = "funfair-rg"
}

variable "azure_nsg_name" {
  type    = string
  default = "funfair-nsg"
}

variable "azure_vn_name" {
  type    = string
  default = "funfair-vn"
}


variable "azure_key_vault_name" {
  type    = string
  default = "funfair-key-vault"
}

variable "azure_sql_user" {
  type    = string
  default = "sqladmin"
}

variable "azure_sql_version" {
  type    = string
  default = "12.0"
}

variable "azure_sql_skuname" {
  type    = string
  default = "Standard_B2s"
}


variable "azure_sql_ssl_enforcement_enabled" {
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



