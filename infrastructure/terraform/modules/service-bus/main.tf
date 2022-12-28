resource "azurerm_servicebus_namespace" "servicebus_ns" {
  name                = var.namespace_name
  location            = var.location
  resource_group_name = var.resource_group_name
  sku                 = "Standard"
}

resource "azurerm_servicebus_queue" "sb_queue" {
  for_each                             = toset(var.queue_names)
  name                                 = each.key
  namespace_id                         = azurerm_servicebus_namespace.servicebus_ns.id
  max_size_in_megabytes                = 1024
  max_delivery_count                   = 5
  requires_duplicate_detection         = true 
  dead_lettering_on_message_expiration = true
}

