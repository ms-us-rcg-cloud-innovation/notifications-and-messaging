variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "namespace_name" {
  type = string
}

variable "queue_names" {
  type = list(string)
  default = null
}