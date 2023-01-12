terraform {
    required_providers {
      azapi = {
        source = "azure/azapi"
        version = ">=1.1.0"
      }
    }
}

resource "azapi_resource" "acs_email" {
  type = "Microsoft.Communication/emailServices@2022-07-01-preview"
  name = "${var.name}-email"
  location = "global"
  parent_id = var.resource_group_id
  body = jsonencode({
    properties = {
      dataLocation = var.data_location
    }
  })
}

resource "azapi_resource" "acs_email_domain" {
  type = "Microsoft.Communication/emailServices/domains@2022-07-01-preview"
  name = "AzureManagedDomain" # This name is required by the API
  location = "global"
  parent_id = azapi_resource.acs_email.id
  body = jsonencode({
    properties = {
      domainManagement = var.domain_management
      userEngagementTracking = "Enabled"
      validSenderUsernames = {
        (var.from_email) : "Do Not Reply Email"
      }
    }
  })
}

resource "azapi_resource" "acs" {
  type = "Microsoft.Communication/communicationServices@2022-07-01-preview"
  name = var.name
  location = "global"  
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

# use the external data resource to query Azure for information about our
# ACS instance and related resource. This will pull connection strings
# to be used by our functions and the from email we can use
data "external" "pwsh_acs_details" {
  program = [
    "pwsh", 
    "${path.module}/get-acs-details.ps1"]

  query = {
    "acs_name" = resource.azapi_resource.acs.name
    "resource_group" = var.resource_group_name
    "from_email" = var.from_email
    "domainEmailResourceId" = resource.azapi_resource.acs_email_domain.id
  }

  depends_on = [
    azapi_resource.acs,
    azapi_resource.acs_email,
    azapi_resource.acs_email_domain
  ]
}