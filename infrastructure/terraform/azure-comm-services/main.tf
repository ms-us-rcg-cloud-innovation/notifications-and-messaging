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

locals {
    acs_name    = "acs-demo"
}

resource "azurerm_resource_group" "acs_resource_group" {
  name     = "acs-demo-rg"
  location = "East US 2"
}

resource "azurerm_communication_service" "acs_demo" {
  name                = local.acs_name
  resource_group_name = azurerm_resource_group.acs_resource_group.name
  data_location       = "United States"
}

resource "azurerm_service_plan" "nh_funcs_service_plan" {
  name = "acs-functions-sp"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  os_type = "Linux"
  sku_name = "Y1"
}

module "notification_hub_function_app" {
  source = "./modules/function"
  app_name = "acs-funcs-app"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = azurerm_resource_group.resource_group.location
  storage_account_name = "acsfuncssa" #customer messaging func sa
  host_sku = "Y1"
  func_app_settings = {}
}