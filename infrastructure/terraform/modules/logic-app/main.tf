resource "azurerm_app_service_plan" "logic_app_host" {
  name                = "${var.app_name}-sp"
  resource_group_name = var.resource_group_name
  location            = var.location
  kind                = "Elastic"
  
  sku {
    tier = "WorkflowStandard"
    size = "WS1"
  }
}

resource "azurerm_storage_account" "logic_app_storage_account" {
  name                      = var.storage_account_name
  resource_group_name       = var.resource_group_name
  location                  = var.location
  account_tier              = "Standard"
  account_replication_type  = "LRS"
  enable_https_traffic_only = true
  min_tls_version           = "TLS1_2"  
}

resource "azurerm_logic_app_standard" "example" {
  name                       = var.app_name
  location                   = var.location
  resource_group_name        = var.resource_group_name
  
  app_service_plan_id        = azurerm_app_service_plan.logic_app_host.id
  storage_account_name       = azurerm_storage_account.logic_app_storage_account.name
  storage_account_access_key = azurerm_storage_account.logic_app_storage_account.primary_access_key

  https_only                 = true

  app_settings               = var.app_settings

  site_config {
    min_tls_version = "1.2" 
  }
}