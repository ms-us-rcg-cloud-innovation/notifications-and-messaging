terraform {
    required_providers {
      azapi = {
        source = "azure/azapi"
      }
    }
}

resource "azapi_resource" "acs_email" {
  type = "Microsoft.Communication/emailServices@2022-07-01-preview"
  name = "${var.name}_email"
  location = var.location
  parent_id = var.resource_group_id
  body = jsonencode({
    properties = {
      dataLocation = var.data_location
    }
  })
}

resource "azapi_resource" "acs_email_domain" {
  type = "Microsoft.Communication/emailServices/domains@2022-07-01-preview"
  name = "${var.name}_domain"
  location = var.location
  parent_id = azapi_resource.acs_email.id
  body = jsonencode({
    properties = {
      domainManagement = var.domain_management
      userEngagementTracking = var.user_engagement_tracking_enabled ? "Enabled" : "Disabled"
      validSenderUsernames = {}
    }
  })
}

resource "azapi_resource" "acs" {
  type = "Microsoft.Communication/communicationServices@2022-07-01-preview"
  name = var.name
  location = var.location
  parent_id = var.resource_group_id
  body = jsonencode({
    properties = {
      dataLocation = var.data_location
      linkedDomains = [
        azapi_resource.acs_email_domain.id
      ]
    }
  })
}