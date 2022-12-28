output "ns_id" {
  value = azurerm_servicebus_namespace.servicebus_ns.id
}

output "primary_connectionstrnig" {
  value = azurerm_servicebus_namespace.servicebus_ns.default_primary_connection_string
}

output "primary_key" {
  value = azurerm_servicebus_namespace.servicebus_ns.default_primary_key
}

output "secondary_connectionstrnig" {
  value = azurerm_servicebus_namespace.servicebus_ns.default_secondary_connection_string
}

output "secondary_key" {
  value = azurerm_servicebus_namespace.servicebus_ns.default_secondary_key
}

output "queues" {
  value = tomap({
    for k, q in azurerm_servicebus_queue.sb_queue : k => q
  })
}