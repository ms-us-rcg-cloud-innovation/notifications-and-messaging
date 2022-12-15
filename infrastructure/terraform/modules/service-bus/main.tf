resource "azurerm_servicebus_namespace" "sb_namespace" {
  name                = var.namespace_name
  location            = var.location
  resource_group_name = var.resource_group_name
  sku                 = "Standard"
  minimum_tls_version = "1.2"
}

resource "azurerm_servicebus_queue" "sb_queue" {
  for_each                             = toset(var.queues)
  name                                 = each.key
  namespace_id                         = azurerm_servicebus_namespace.sb_namespace.id
  
  dead_lettering_on_message_expiration = true
}

