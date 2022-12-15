# app service plan host for functions
resource "azurerm_service_plan" "func_host" {
  name = "${var.app_name}-sp"
  resource_group_name =  var.resource_group_name
  location = var.location
  os_type = "Linux"
  sku_name = var.host_sku
}

# function apps
resource "azurerm_storage_account" "func_storage_account" {
  name = var.storage_account_name
  resource_group_name = var.resource_group_name
  location = var.location
  account_tier = "Standard"
  account_replication_type = "LRS"
  enable_https_traffic_only = true
  min_tls_version = "TLS1_2"  
}

resource "azurerm_linux_function_app" "func_app" {
  name = var.app_name
  resource_group_name = var.resource_group_name
  location = var.location

  storage_account_name = azurerm_storage_account.func_storage_account.name
  storage_account_access_key = azurerm_storage_account.func_storage_account.primary_access_key
  service_plan_id = azurerm_service_plan.func_host.id
  
  https_only = true
  functions_extension_version = "~4"

  app_settings = var.app_settings

  site_config {   
       
    minimum_tls_version = "1.2"
    http2_enabled = true
    
    application_stack {
      dotnet_version = "6.0"
      use_dotnet_isolated_runtime = true
    }
  }
} 