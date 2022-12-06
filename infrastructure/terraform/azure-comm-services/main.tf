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

resource "azurerm_communication_service" "example" {
  name                = local.acs_name
  resource_group_name = azurerm_resource_group.acs_resource_group.name
  data_location       = "United States"
}