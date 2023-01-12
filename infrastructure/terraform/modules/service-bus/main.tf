resource "azurerm_servicebus_namespace" "ns" {
  name                = var.namespace_name
  location            = var.location
  resource_group_name = var.resource_group_name
  sku                 = var.sku
}

resource "azurerm_servicebus_queue" "queue" {
  for_each                             = var.queues
  name                                 = each.key
  namespace_id                         = azurerm_servicebus_namespace.ns.id
  max_size_in_megabytes                = each.value.max_size_in_megabytes
  max_delivery_count                   = each.value.max_delivery_count
  requires_duplicate_detection         = each.value.requires_duplicate_detection
  dead_lettering_on_message_expiration = each.value.dead_lettering_on_message_expiration
}
