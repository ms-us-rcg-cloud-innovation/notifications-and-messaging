# app service plan host for functions
resource "azurerm_service_plan" "func" {
  name                = var.service_plan_name
  resource_group_name = var.resource_group_name
  location            = var.location
  os_type             = "Linux"
  sku_name            = var.host_sku
}

resource "azurerm_linux_function_app" "func" {
  name                = var.app_name
  resource_group_name = var.resource_group_name
  location            = var.location

  storage_account_name        = var.sa_name
  storage_account_access_key  = var.sa_key
  service_plan_id             = azurerm_service_plan.func.id
  
  https_only                  = true
  functions_extension_version = "~4"

  app_settings                = var.app_settings

  site_config {   
    
    minimum_tls_version       = "1.2"
    http2_enabled             = true
    
    application_stack {
      dotnet_version              = "6.0"
      use_dotnet_isolated_runtime = true
    }
  }

  lifecycle {
    ignore_changes = [
      app_settings
    ]
  }
} 