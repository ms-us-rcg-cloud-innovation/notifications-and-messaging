variable "location" {
  type = string
}

variable "resource_group_name" {
  type = string
}

# notification hub namesapce
variable "nh_namespace_name" {
  type = string
}

variable "nh_namespace_sku_name" {
  type    = string
  default = "Basic"
}

# notificaiton hub
variable "nh_name" {
  type = string
}

variable "gcm_api_key" {
  type    = string
  default = null
}