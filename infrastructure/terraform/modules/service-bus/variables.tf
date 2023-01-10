variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "namespace_name" {
  type = string
}

variable "queues" {
  type = map(object({    
    max_delivery_count                   = optional(number)
    max_size_in_megabytes                = optional(number)
    requires_duplicate_detection         = optional(bool)
    dead_lettering_on_message_expiration = optional(bool)
  }
  ))

  default = null
}

variable "sku" {
  type = string
  default = "Standard"
}