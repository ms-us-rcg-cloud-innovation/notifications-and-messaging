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
  name     = "comm-demo-rg"
  location = "East US 2"
}

# notification hub

resource "azurerm_notification_hub_namespace" "nh_namespace" {
  name = "demonnamespace"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  namespace_type = "NotificationHub"
  sku_name = "Basic"
}

resource "azurerm_notification_hub" "notification_hub" {
  name = "demonotificationhub"
  namespace_name = azurerm_notification_hub_namespace.nh_namespace.name
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  
  dynamic "gcm_credential" {
    for_each = var.gcm_api_key[*]
    content {
      api_key = var.gcm_api_key
    }
  }
}

# create a new access policy you can provide to your functions for 
# registration management
resource "azurerm_notification_hub_authorization_rule" "nh_shared_listener" {
  name = "apis-full-access-auth-rule"
  notification_hub_name = azurerm_notification_hub.notification_hub.name
  namespace_name = azurerm_notification_hub_namespace.nh_namespace.name
  resource_group_name = azurerm_resource_group.resource_group.name
  manage = true
  send = true
  listen = true
}

# function apps
resource "azurerm_storage_account" "function_storage_account" {
  name = "commfuncssa"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  account_tier = "Standard"
  account_replication_type = "LRS"
  enable_https_traffic_only = true
  min_tls_version = "TLS1_2"  
}

resource "azurerm_service_plan" "nh_funcs_service_plan" {
  name = "nh-functions-sp"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  os_type = "Linux"
  sku_name = "Y1"
}

resource "azurerm_linux_function_app" "notification_hub_funcs" {
  name = "notification-hub-functions"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location

  storage_account_name = azurerm_storage_account.function_storage_account.name
  storage_account_access_key = azurerm_storage_account.function_storage_account.primary_access_key
  service_plan_id = azurerm_service_plan.nh_funcs_service_plan.id  
  
  https_only = true
  functions_extension_version = "~4"

  app_settings = {    
    # set notification hub name as an environment variable for confugration at startup time
    "NOTIFICATION_HUB_NAME" = azurerm_notification_hub.notification_hub.name
  }

  # connection string for configuration notificaiton hub client at startup time
  connection_string {
    name = "NOTIFICATION_HUB_CS"
    type = "NotificationHub"
    value = azurerm_notification_hub_authorization_rule.nh_shared_listener.primary_access_key
  }  
  
  site_config {   
       
    minimum_tls_version = "1.2"
    http2_enabled = true
    
    application_stack {
      dotnet_version = "6.0"
      use_dotnet_isolated_runtime = true
    }
  }
} 