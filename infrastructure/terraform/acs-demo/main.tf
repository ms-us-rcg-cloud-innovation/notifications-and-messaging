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
    acs_name                = "acs-demo"
    send_queue              = "send-mail"
    failed_delivery_queue   = "email-failed-delivery-events"
    delivered_email_queue   = "email-delivered-events"
    engagement_update_queue = "email-engagement-update-events"
    from_email              = "acs_demo_msft"
    emails_table         = "acsEmails"
}

resource "azurerm_resource_group" "acs_resource_group" {
  name     = "${local.acs_name}-rg"
  location = "East US 2"
}

module "acs_storage_account" {
  source = "../modules/storage-account"
  name = "acsdemomsftsa"
  resource_group_name = azurerm_resource_group.acs_resource_group.name
  location = azurerm_resource_group.acs_resource_group.location
  tables = [
    local.emails_table
  ]
}

module "acs_processing_queues" {
  source               = "../modules/service-bus"  
  namespace_name       = "acs-work-request-bus"
  resource_group_name  = azurerm_resource_group.acs_resource_group.name
  location             = azurerm_resource_group.acs_resource_group.location
  queue_names          = [
    local.send_queue,
    local.failed_delivery_queue,
    local.engagement_update_queue,
    local.delivered_email_queue
  ]
}

module "azure_communication_services_email" {
  source              = "../modules/azure-communication-services-email"
  name                = local.acs_name
  from_email          = local.from_email 
  domain_management   = "AzureManaged"
  providers           = {
    azapi = azapi
  }
  location            = azurerm_resource_group.acs_resource_group.location
  resource_group_id   = azurerm_resource_group.acs_resource_group.id
  resource_group_name = azurerm_resource_group.acs_resource_group.name 
}

module "acs_email_input_event_handler_funcs" {
  source               = "../modules/function"
  app_name             = "rcg-acs-demo-funcs-app"
  resource_group_name  = azurerm_resource_group.acs_resource_group.name
  location             = azurerm_resource_group.acs_resource_group.location
  sa_name              = module.acs_storage_account.name
  sa_key               = module.acs_storage_account.primary_access_key
  host_sku             = "Y1"
  app_settings         = {
    "SA_CONNECT_STRING"     = module.acs_storage_account.primary_connection_string
    "SA_TABLE"              = local.emails_table
    "SB_CONNECTION_STRING"  = module.acs_processing_queues.primary_connectionstrnig
    "SB_QUEUE"              = local.send_queue
    "ACS_CONNECTION_STRING" = module.azure_communication_services_email.primary_connectionstring
    "EMAIL_SENDER"          = module.azure_communication_services_email.email_from
  }
}

module "acs_email_output_event_handler_logic_app" {
  source               = "../modules/logic-app"
  app_name             = "acs-demo-rcg-event-handler"
  resource_group_name  = azurerm_resource_group.acs_resource_group.name
  location             = azurerm_resource_group.acs_resource_group.location
  sa_name              = module.acs_storage_account.name
  sa_key               = module.acs_storage_account.primary_access_key
  host_sku             = "WS1"
  app_settings         = {}
}

module "acs_event_grid_topic_subscription" {
  source = "../modules/event-grid-system-topic"
  topic_name = "acs-events-topic"
  resource_group_name = azurerm_resource_group.acs_resource_group.name
  topic_type = "Microsoft.Communication.CommunicationServices"
  location =  "global"
  event_source_id = module.azure_communication_services_email.acs_id
  subscriptions_with_queue_endpoint = {
    "acs-failed-email-events-subscription" = {
      begins_with_filters = [
        {
          key = "data.Status"
          values =  [ "Failed" ]
        }
      ]
      event_types = [ "Microsoft.Communication.EmailDeliveryReportReceived" ]
      queue_id = module.acs_processing_queues.queues[local.failed_delivery_queue].id
    },
    "acs-delivered-email-events-subscription" = {
      begins_with_filters = [
        {
          key = "data.Status"
          values =  [ "Succeeded" ]
        }
      ]
      event_types = [ "Microsoft.Communication.EmailDeliveryReportReceived" ]
      queue_id = module.acs_processing_queues.queues[local.delivered_email_queue].id
    },
    "acs-engagement-events-subscription" = {
      event_types = [ "Microsoft.Communication.EmailEngagementTrackingReportReceived" ]
      queue_id = module.acs_processing_queues.queues[local.engagement_update_queue].id      
    }
  }
}