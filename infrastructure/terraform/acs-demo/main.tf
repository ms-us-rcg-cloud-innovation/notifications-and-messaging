terraform {
    required_providers {
      azurerm = {
        source = "hashicorp/azurerm"
        version = "=3.0.0"
      }
      azapi = {
        source = "azure/azapi"
      }
    }
}

provider "azapi" {
}

provider "azurerm" {
  features {}
}

locals {
    acs_name    = "acs-demo"
}

resource "azurerm_resource_group" "acs_resource_group" {
  name     = "${local.acs_name}-rg"
  location = "East US 2"
}

module "azure_communication_services_email" {
  source = "../modules/azure-communication-services-email"
  name = local.acs_name
  location = azurerm_resource_group.acs_resource_group.location
  resource_group_id = azurerm_resource_group.acs_resource_group.id
  data_location = "United States"
  domain_management = "AzureManaged"
  user_engagement_tracking_enabled = true
}

module "acs_email_event_handler_funcs" {
  source = "../modules/function"
  app_name = "acs-funcs-app"
  resource_group_name = azurerm_resource_group.acs_resource_group.name
  location = azurerm_resource_group.acs_resource_group.location
  storage_account_name = "acsfuncssa" #customer messaging func sa
  host_sku = "Y1"
  app_settings = {}
}