output "acs_id" {
    value = azapi_resource.acs.id
}

output "acs_name" {
  value = azapi_resource.acs.name
}

output "primary_connectionstring" {
  value = data.external.pwsh_acs_details.result.primaryConnectionString
  sensitive = true
}

output "primary_key" {
  value = data.external.pwsh_acs_details.result.primaryKey
  sensitive = true
}

output "secondary_connectionstring" {
  value = data.external.pwsh_acs_details.result.primaryConnectionString
  sensitive = true
}

output "secondary_key" {
  value = data.external.pwsh_acs_details.result.primaryKey
  sensitive = true
}

output "email_from" {
  value = data.external.pwsh_acs_details.result.fromEmail
}

output "mail_domain" {
  value = data.external.pwsh_acs_details.result.fromDomain
}