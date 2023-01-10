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
  resource_group_name     = "acs-demo-rg-${random_string.random.result}"
  location                = var.location
  acs_name                = "acs-demo-${random_string.random.result}"  
  from_email              = "acs_demo_dnr"
  function_app_name       = "acs-send-emails-fa-${random_string.random.result}"
  logic_app_name          = "acs-email-events-la-${random_string.random.result}"
  storage_account_name    = "acsdemomsftsa${random_string.random.result}"
  service_bus_namespace   = "acs-sb-demo-${random_string.random.result}"
  event_grid_topic_name   = "acs-events-topic-${random_string.random.result}"
  
  send_email_queue        = "send-email"    
  email_status_queue      = "email-status-events"
  engagement_event_queue  = "email-engagement-update-events"

  emails_table            = "sendEmails"
  email_status_table      = "emailStatus"
  engagement_events_table = "engagementEvents"
}

resource "azurerm_resource_group" "acs_resource_group" {
  name     = local.resource_group_name
  location = local.location
}

resource "random_string" "random" {
  length  = 4
  special = false
  upper   = false
  numeric = false
}

module "acs_storage_account" {
  source              = "../modules/storage-account"
  name                = local.storage_account_name
  resource_group_name = azurerm_resource_group.acs_resource_group.name
  location            = azurerm_resource_group.acs_resource_group.location
  tables = [
    local.emails_table,
    local.email_status_table,
    local.engagement_events_table
  ]
}

module "acs_processing_queues" {
  source               = "../modules/service-bus"  
  namespace_name       = local.service_bus_namespace
  resource_group_name  = azurerm_resource_group.acs_resource_group.name
  location             = azurerm_resource_group.acs_resource_group.location
  queues               = {
    (local.send_email_queue) = {
      max_delivery_count                   = 5
      requires_duplicate_detection         = true
      dead_lettering_on_message_expiration = true
    }
    (local.email_status_queue) = {
      max_delivery_count                   = 5
      requires_duplicate_detection         = true
      dead_lettering_on_message_expiration = true
    }
    (local.engagement_event_queue) = {
      max_delivery_count                   = 5
      requires_duplicate_detection         = true
      dead_lettering_on_message_expiration = true
    }
  }  
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
  source               = "../modules/linux-function-app"
  service_plan_name    = local.function_app_name
  app_name             = local.function_app_name
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
  source               = "../modules/logic-app-standard"
  service_plan_name    = local.logic_app_name
  app_name             = local.logic_app_name
  resource_group_name  = azurerm_resource_group.acs_resource_group.name
  location             = azurerm_resource_group.acs_resource_group.location
  sa_name              = module.acs_storage_account.name
  sa_key               = module.acs_storage_account.primary_access_key
  host_sku             = "WS1"
  app_settings         = {
    
    // service bus settings
    "serviceBus_connectionString"  = module.acs_processing_queues.primary_connectionstrnig
    "azureTables_connectionString" = module.acs_storage_account.primary_connection_string
  }
}

module "acs_event_grid_topic_subscription" {
  source = "../modules/event-grid-system-topic"
  topic_name = local.event_grid_topic_name
  resource_group_name = azurerm_resource_group.acs_resource_group.name
  topic_type = "Microsoft.Communication.CommunicationServices"
  location =  "global"
  event_source_id = module.azure_communication_services_email.acs_id
  subscriptions_with_queue_endpoint = {
    acs-email-status-events-subscription = {
      event_types         = [ 
        "Microsoft.Communication.EmailDeliveryReportReceived" 
      ]
      queue_id            = module.acs_processing_queues.queues[local.email_status_queue].id,
      delivery_properties = [
        {
          header_name  = "acs-status"
          type         = "Dynamic"
          source_field = "data.Status"
          is_secret    = false
        },
        {
          header_name  = "acs-messageId"
          type         = "Dynamic"
          source_field = "data.MessageId"
          is_secret    = false
        },
        {
          header_name  = "acs-event"
          type         = "Dynamic"
          source_field = "data.EventType"
          is_secret    = false
        }
      ]
    },
    acs-engagement-events-subscription = {
      event_types = [ 
        "Microsoft.Communication.EmailEngagementTrackingReportReceived" 
      ]
      queue_id    = module.acs_processing_queues.queues[local.engagement_event_queue].id
      delivery_properties = [
        {
          header_name  = "acs-messageId"
          type         = "Dynamic"
          source_field = "data.MessageId"
          is_secret    = false
        },
        {
          header_name  = "acs-engagement-type"
          type         = "Dynamic"
          source_field = "data.EngagementType"
          is_secret    = false
        },
      ]
    }
  }
}