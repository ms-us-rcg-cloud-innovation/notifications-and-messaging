variable "topic_name" {
  type = string
}

variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "topic_type" {
 type = string
}

variable "subscriptions_with_queue_endpoint" {
  type = map(
    object({
      queue_id            = string
      event_types         = list(string)
      begins_with_filters = optional(set(
        object({
          key    = string
          values = list(string)
        })
      ))
      delivery_properties = optional(set(
        object({   
          header_name  = string       
          type         = string
          value        = optional(string)
          source_field = optional(string)
          is_secret    = bool
        })
      ))
    })
  )
}

variable "event_source_id" {
  type = string
}
