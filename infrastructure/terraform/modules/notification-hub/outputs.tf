output "auth_rule_name" {
  value = azurerm_notification_hub_authorization_rule.nh_shared_listener.name
}

output "auth_rule_primary_access_key" {
  value = azurerm_notification_hub_authorization_rule.nh_shared_listener.primary_access_key
}

output "auth_rule_secondary_access_key" {
  value = azurerm_notification_hub_authorization_rule.nh_shared_listener.secondary_access_key
}