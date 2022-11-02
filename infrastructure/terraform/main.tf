terraform {
    required_providers {
      azurerm = {
        source = "hashicorp/azurerm"
        version = "=3.0.0"
      }
    }
}

locals {
  nh_ns = "nhdemonamespace"
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "resource_group" {
  name     = var.resource_group_name
  location = var.region
}

resource "azurerm_notification_hub_namespace" "namespace" {
  name = var.namespace_name
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  namespace_type = "NotificationHub"
  sku_name = var.namespace_sku
}

resource "azurerm_notification_hub" "notification_hub" {
  name = var.notification_hub_name
  namespace_name = azurerm_notification_hub_namespace.namespace.name
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  gcm_credential {
    api_key = var.gcm_api_key
  }
}