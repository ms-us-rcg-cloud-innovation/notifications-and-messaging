resource "azurerm_service_plan" "logic_app_host" {
  name                = "${var.app_name}-sp"
  resource_group_name =  var.resource_group_name
  location            = var.location
  os_type             = "Windows"
  sku_name            = "WS1"
}

resource "azurerm_logic_app_standard" "lapp_standard" {
  name                       = var.app_name
  location                   = var.location
  resource_group_name        = var.resource_group_name
  
  app_service_plan_id        = azurerm_service_plan.logic_app_host.id
  storage_account_name       = var.sa_name
  storage_account_access_key = var.sa_key

  https_only                 = true

  app_settings               = var.app_settings
  
  site_config {
    min_tls_version = "1.2" 
    elastic_instance_minimum = 1
  }
}