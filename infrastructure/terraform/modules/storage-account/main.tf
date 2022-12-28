# function apps
resource "azurerm_storage_account" "acs_storage_account" {
  name                      = var.name
  resource_group_name       = var.resource_group_name
  location                  = var.location
  account_tier              = "Standard"
  account_replication_type  = "LRS"
  enable_https_traffic_only = true
  min_tls_version           = "TLS1_2"  
}

resource "azurerm_storage_table" "sa_table" {
  for_each             = toset(var.tables)
  name                 = each.key
  storage_account_name = azurerm_storage_account.acs_storage_account.name
}