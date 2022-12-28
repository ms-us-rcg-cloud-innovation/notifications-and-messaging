output "acs_id" {
    value = azapi_resource.acs.id
}

output "primary_connectionstring" {
  value = data.external.pwsh_acs_details.result.primaryConnectionString
}

output "primary_key" {
  value = data.external.pwsh_acs_details.result.primaryKey
}

output "secondary_connectionstring" {
  value = data.external.pwsh_acs_details.result.primaryConnectionString
}

output "secondary_key" {
  value = data.external.pwsh_acs_details.result.primaryKey
}

output "email_from" {
  value = data.external.pwsh_acs_details.result.fromEmail
}