output "name" {
    value = azurerm_storage_account.storage.name
}

output "primary_access_key" {
    value = azurerm_storage_account.storage.primary_access_key
    sensitive = true
}

output "primary_connection_string" {
    value = azurerm_storage_account.storage.primary_connection_string
    sensitive = true
}

output "primary_table_endpoint" {
    value = azurerm_storage_account.storage.primary_table_endpoint    
}