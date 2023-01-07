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
    acs_name                = var.comm_services_name
    resource_group_name     = var.resource_group_name
    from_email              = var.from_email

    send_email_queue        = "send-email"    
    email_status_queue      = "email-status-events"
    engagement_event_queue  = "email-engagement-update-events"

    emails_table            = "sendEmails"
    email_status_table      = "emailStatus"
    engagement_events_table = "engagementEvents"
}

resource "azurerm_resource_group" "acs_resource_group" {
  name     = local.resource_group_name
  location = "East US 2"
}

module "acs_storage_account" {
  source = "../modules/storage-account"
  name = "acsdemomsftsa"
  resource_group_name = azurerm_resource_group.acs_resource_group.name
  location = azurerm_resource_group.acs_resource_group.location
  tables = [
    local.emails_table,
    local.email_status_table,
    local.engagement_events_table
  ]
}

module "acs_processing_queues" {
  source               = "../modules/service-bus"  
  namespace_name       = "acs-service-bus"
  resource_group_name  = azurerm_resource_group.acs_resource_group.name
  location             = azurerm_resource_group.acs_resource_group.location
  queue_names          = [
    local.send_email_queue,
    local.email_status_queue,
    local.engagement_event_queue,
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
    // storage account settings
    "SA_CONNECTION_STRING"      = module.acs_storage_account.primary_connection_string
    "SA_EMAIL_TABLE"            = local.emails_table
    "SA_EVENTS_TABLE"           = local.engagement_events_table

    // service bus settings
    "SB_CONNECTION_STRING"      = module.acs_processing_queues.primary_connectionstrnig
    "SB_SEND_EMAIL_QUEUE"       = local.send_email_queue
    "SB_ENGAGEMENT_EVENT_QUEUE" = local.engagement_event_queue 

    // azure communication services email settings
    "ACS_CONNECTION_STRING"     = module.azure_communication_services_email.primary_connectionstring
    "EMAIL_SENDER"              = module.azure_communication_services_email.email_from
    
    // site configs
    "SCM_DO_BUILD_DURING_DEPLOYMENT" = false
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
    "acs-email-status-events-subscription" = {
      event_types = [ 
        "Microsoft.Communication.EmailDeliveryReportReceived" 
      ]
      queue_id = module.acs_processing_queues.queues[local.email_status_queue].id
    },
    "acs-engagement-events-subscription" = {
      event_types = [ 
        "Microsoft.Communication.EmailEngagementTrackingReportReceived" 
      ]
      queue_id = module.acs_processing_queues.queues[local.engagement_event_queue ].id      
    }
  }
}