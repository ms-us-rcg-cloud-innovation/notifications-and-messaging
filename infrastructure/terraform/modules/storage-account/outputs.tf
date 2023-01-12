output "name" {
    value = azurerm_storage_account.sa.name
}

output "primary_access_key" {
    value = azurerm_storage_account.sa.primary_access_key
}

output "primary_connection_string" {
    value = azurerm_storage_account.sa.primary_connection_string
}

output "primary_table_endpoint" {
    value = azurerm_storage_account.sa.primary_table_endpoint
}