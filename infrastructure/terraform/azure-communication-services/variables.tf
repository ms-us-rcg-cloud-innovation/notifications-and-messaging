variable "uniquify" {
    type = bool
    description = "Adds 5 random characters to the end of required resources to ensure uniquess"
}

variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "acs_name" {
  type = string
}

variable "from_email" {
  type = string
}

variable "function_app_name" {
  type = string
}

variable "logic_app_name" {
  type = string
}

variable "storage_account_name" {
  type = string
}

variable "service_bus_namespace" {
  type = string
}

variable "event_grid_topic_name" {
  type = string
}

variable "send_email_queue" {
  type = string
}

variable "email_status_queue" {
  type = string
}

variable "engagement_event_queue" {
  type = string
}

variable "emails_table" {
  type = string
}

variable "email_status_table" {
  type = string
}

variable "engagement_events_table" {
  type = string
}
