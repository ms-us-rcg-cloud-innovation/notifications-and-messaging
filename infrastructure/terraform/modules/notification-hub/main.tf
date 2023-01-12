resource "azurerm_notification_hub_namespace" "nh_namespace" {
  name                  = var.nh_namespace_name
  resource_group_name   = var.resource_group_name
  location              = var.location
  namespace_type        = "NotificationHub"
  sku_name              = var.nh_namespace_sku_name
}

resource "azurerm_notification_hub" "notification_hub" {
  name                = var.nh_name
  namespace_name      = azurerm_notification_hub_namespace.nh_namespace.name
  resource_group_name = var.resource_group_name
  location            = var.location

  dynamic "gcm_credential" {
    for_each  = var.gcm_api_key[*]
    content {
      api_key = var.gcm_api_key
    }
  }
}

# create a new access policy you can provide to your functions for 
# registration management
resource "azurerm_notification_hub_authorization_rule" "nh_shared_listener" {
  name                  = "ManagementApiAccessSignature"
  notification_hub_name = azurerm_notification_hub.notification_hub.name
  namespace_name        = azurerm_notification_hub_namespace.nh_namespace.name
  resource_group_name   = var.resource_group_name

  manage  = true
  send    = true
  listen  = true
}