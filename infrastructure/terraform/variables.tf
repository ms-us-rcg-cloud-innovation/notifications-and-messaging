variable "resource_group_name" {
  type = string
  default = "nh_demo_rg"
}

variable "region" {
  type = string
  default = "East US 2"
}

variable "namespace_sku" {
  type = string
  default = "Basic"
}

variable "namespace_name" {
  type = string
  default = "nhdemonamespace" 
}

variable "notification_hub_name" {
  type = string
  default = "demonotificationhub"
}

variable "gcm_api_key" {
    type = string
    default = ""
}