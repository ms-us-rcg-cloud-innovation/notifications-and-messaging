resource "azurerm_service_plan" "logic" {
  name                = var.service_plan_name
  resource_group_name = var.resource_group_name
  location            = var.location
  os_type             = "Windows"
  sku_name            = "WS1"
}


resource "azurerm_application_insights" "insights" {
  name                = var.app_name
  location            = var.location
  resource_group_name = var.resource_group_name
  application_type    = "web"
}


resource "azurerm_logic_app_standard" "logic" {
  name                       = var.app_name
  location                   = var.location
  resource_group_name        = var.resource_group_name
  
  app_service_plan_id        = azurerm_service_plan.logic.id
  storage_account_name       = var.sa_name
  storage_account_access_key = var.sa_key

  https_only                 = true
  
  app_settings                = merge(var.app_settings, { 
    "APPINSIGHTS_INSTRUMENTATIONKEY": azurerm_application_insights.insights.instrumentation_key,
    "APPLICATIONINSIGHTS_CONNECTION_STRING": azurerm_application_insights.insights.connection_string,
    "FUNCTIONS_WORKER_RUNTIME": "node"
  })
  
  identity {
    type = "SystemAssigned"
  }

  site_config {
      
    min_tls_version = "1.2" 
    elastic_instance_minimum = 1    
  }
  
  lifecycle {
    ignore_changes = [
      app_settings
    ]
  }
}