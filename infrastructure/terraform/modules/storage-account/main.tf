# function apps
resource "azurerm_storage_account" "storage" {
  name                            = var.name
  resource_group_name             = var.resource_group_name
  location                        = var.location
  account_tier                    = var.account_tier
  account_replication_type        = var.account_replication_type
  enable_https_traffic_only       = true
  min_tls_version                 = "TLS1_2"
  #allow_nested_items_to_be_public = false
  access_tier                     = var.access_tier
}

resource "azurerm_storage_table" "table" {
  for_each             = var.tables == null ? [] : toset(var.tables)
  name                 = each.key
  storage_account_name = azurerm_storage_account.storage.name
}
