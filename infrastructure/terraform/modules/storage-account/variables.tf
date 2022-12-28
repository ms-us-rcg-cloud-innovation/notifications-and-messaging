variable "name" {
  type = string
}

variable "tables" {
  type = list(string)
  default = null
}

variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}