variable "name" {
  type = string
}

variable "location" {
  type = string
}

variable "resource_group_id" {
  type = string
}

variable "resource_group_name" {
  type = string
}

variable "from_email" {
  type = string
}

variable "data_location" {
  type = string
  default = "United States"
  validation {
    condition = contains(["Africa"
                        , "Asia"
                        , "Pacific"
                        , "Australia"
                        , "Brazil"
                        , "Canada"
                        , "Europe"
                        , "France"
                        , "Germany"
                        , "India"
                        , "Japan"
                        , "Korea"
                        , "Norway"
                        , "Switzerland"
                        , "UAE"
                        , "UK"
                        , "United States"], var.data_location)
    
    error_message = "Invalid data_location value. Must be a valid location. Examples: United States, UK, Africa, Asia"

  }
}

variable "domain_management" {
  type = string
  default = "AzureManagedDomain"
  validation {
    condition = contains(["AzureManaged"
                        , "CustomerManaged"
                        , "CustomerManagedInExchangeOnline"], var.domain_management)

    error_message = "Must be a valid value. Valid values: AzureManaged, CustomerManaged, CustomerManagedInExchangeOnline"
  }
}

variable "user_engagement_tracking_enabled" {
  type = bool

  default = true
}