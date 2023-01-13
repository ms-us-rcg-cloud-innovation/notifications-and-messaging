terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">=3.0.0"
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
  unique_suffix             = var.uniquify ? "-${random_string.random.result}" : ""
  resource_group_name       = "${var.resource_group_name}${local.unique_suffix}"
  location                  = var.location
  acs_name                  = "${var.acs_name}${local.unique_suffix}"
  from_email                = var.from_email
  function_app_name         = "${var.function_app_name}${local.unique_suffix}"
  logic_app_name            = "${var.logic_app_name}${local.unique_suffix}"
  data_storage_account_name = replace("${var.storage_account_name}${local.unique_suffix}", "-", "")
  apps_storage_account_name = replace("appsstatesa${local.unique_suffix}", "-", "")
  service_bus_namespace     = "${var.service_bus_namespace}${local.unique_suffix}"
  event_grid_topic_name     = "${var.event_grid_topic_name}${local.unique_suffix}"

  send_email_queue       = var.send_email_queue
  email_status_queue     = var.email_status_queue
  engagement_event_queue = var.engagement_event_queue

  emails_table            = var.emails_table
  email_status_table      = var.email_status_table
  engagement_events_table = var.engagement_events_table
}

resource "azurerm_resource_group" "acs" {
  name     = local.resource_group_name
  location = local.location
}

resource "random_string" "random" {
  length  = 5
  special = false
  upper   = false
  numeric = false
}

module "data_storage" {
  source                   = "../modules/storage-account"
  name                     = local.data_storage_account_name
  resource_group_name      = azurerm_resource_group.acs.name
  location                 = azurerm_resource_group.acs.location
  account_replication_type = "ZRS"
  tables = [
    local.emails_table,
    local.email_status_table,
    local.engagement_events_table
  ]
}

module "apps_state_sa" {
  source                   = "../modules/storage-account"
  name                     = local.apps_storage_account_name
  resource_group_name      = azurerm_resource_group.acs.name
  location                 = azurerm_resource_group.acs.location
  account_replication_type = "ZRS"
}

module "acs_processing_queues" {
  source              = "../modules/service-bus"
  namespace_name      = local.service_bus_namespace
  resource_group_name = azurerm_resource_group.acs.name
  location            = azurerm_resource_group.acs.location
  queues = {
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
  source            = "../modules/azure-communication-services-email"
  name              = local.acs_name
  from_email        = local.from_email
  domain_management = "AzureManaged"
  providers = {
    azapi = azapi
  }
  location            = azurerm_resource_group.acs.location
  resource_group_id   = azurerm_resource_group.acs.id
  resource_group_name = azurerm_resource_group.acs.name
}

module "acs_email_input_event_handler_funcs" {
  source              = "../modules/linux-function-app"
  service_plan_name   = local.function_app_name
  app_name            = local.function_app_name
  resource_group_name = azurerm_resource_group.acs.name
  location            = azurerm_resource_group.acs.location
  sa_name             = module.apps_state_sa.name
  sa_key              = module.apps_state_sa.primary_access_key
  host_sku            = "Y1"
  app_settings = {
    // storage account settings
    "SA_CONNECTION_STRING" = module.data_storage.primary_connection_string
    "SA_EMAIL_TABLE"       = local.emails_table
    "SA_EVENTS_TABLE"      = local.engagement_events_table

    // service bus settings
    "SB_CONNECTION_STRING"      = module.acs_processing_queues.primary_connectionstrnig
    "SB_SEND_EMAIL_QUEUE"       = local.send_email_queue
    "SB_ENGAGEMENT_EVENT_QUEUE" = local.engagement_event_queue

    // azure communication services email settings
    "ACS_CONNECTION_STRING" = module.azure_communication_services_email.primary_connectionstring
    "EMAIL_SENDER"          = module.azure_communication_services_email.email_from

    // site configs
    "SCM_DO_BUILD_DURING_DEPLOYMENT" = false
  }

  depends_on = [
    module.azure_communication_services_email
  ]
}

module "acs_email_output_event_handler_logic_app" {
  source              = "../modules/logic-app-standard"
  service_plan_name   = local.logic_app_name
  app_name            = local.logic_app_name
  resource_group_name = azurerm_resource_group.acs.name
  location            = azurerm_resource_group.acs.location
  sa_name             = module.apps_state_sa.name
  sa_key              = module.apps_state_sa.primary_access_key
  host_sku            = "WS1"
  app_settings = {

    // service bus settings
    "serviceBus_connectionString"  = module.acs_processing_queues.primary_connectionstrnig
    "azureTables_connectionString" = module.data_storage.primary_connection_string
  }
}

module "acs_event_grid_topic_subscription" {
  source              = "../modules/event-grid-system-topic"
  topic_name          = local.event_grid_topic_name
  resource_group_name = azurerm_resource_group.acs.name
  topic_type          = "Microsoft.Communication.CommunicationServices"
  location            = "global"
  event_source_id     = module.azure_communication_services_email.acs_id
  subscriptions_with_queue_endpoint = {
    acs-email-status-events-subscription = {
      event_types = [
        "Microsoft.Communication.EmailDeliveryReportReceived"
      ]
      queue_id = module.acs_processing_queues.queues[local.email_status_queue].id,
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
      queue_id = module.acs_processing_queues.queues[local.engagement_event_queue].id
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
