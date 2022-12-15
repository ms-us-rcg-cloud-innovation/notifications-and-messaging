resource "azurerm_eventgrid_system_topic" "acs_eg_system_topic" {
  name                   = var.topic_name
  topic_type             = var.topic_type
  resource_group_name    = var.resource_group_name
  location               = var.location
  source_arm_resource_id = "Microsoft.Communication.CommunicationServices"
}

resource "azurerm_eventgrid_system_topic_event_subscription" "acs_eg_system_topic" {
  name                = "acs-failed-email-delivery-events-subscription"
  system_topic        = azurerm_eventgrid_system_topic.eg_system_topic.name
  resource_group_name = var.resource_group_name

  service_bus_queue_endpoint_id = var.failed_email_queue_id

  # Allows advanced filters to be evaluated against an array 
  # of values instead of expecting a singular value. This property 
  # exists for backwards compatibility purposes and it is recommended 
  # to keep it enabled for new event subscriptions.
  advanced_filtering_on_arrays_enabled = true

  advanced_filter {
    string_begins_with {
      key = "data.Status"
      values = ["Failed"]
    }
  }

}