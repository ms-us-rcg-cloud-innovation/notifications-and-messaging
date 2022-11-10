terraform {
    required_providers {
      azurerm = {
        source = "hashicorp/azurerm"
        version = "=3.0.0"
      }
    }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "resource_group" {
  name     = "messaging-demo-rg"
  location = "East US 2"
}

locals {
  notification_hub_name = "customer-messaging"
  notification_hub_namespace = "notification-hub-user-messaging-namespace"
}

# notification hub
module "notification_hub" {
  source = "./modules/notification-hub"
  location = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  nh_namespace_name = local.notification_hub_namespace
  nh_name = local.notification_hub_name
}

# app service plan host for functions
resource "azurerm_service_plan" "nh_funcs_service_plan" {
  name = "nh-functions-sp"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  os_type = "Linux"
  sku_name = "Y1"
}

module "notification_hub_function_app" {
  source = "./modules/function"
  function_app_name = "notification-hub-funcs-app"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  storage_account_name = "customermessagingfuncsa" #customer messaging func sa
  app_service_plan_id = azurerm_service_plan.nh_funcs_service_plan.id
  func_app_settings = {
     # set notification hub name as an environment variable for confugration at startup time
    "NOTIFICATION_HUB_NAME" = local.notification_hub_name
    "NOTIFICATION_HUB_CS" = "Endpoint=sb://${local.notification_hub_namespace}.servicebus.windows.net/;SharedAccessKeyName=${module.notification_hub.auth_rule_name};SharedAccessKey=${module.notification_hub.auth_rule_primary_access_key}" #
  }
}
