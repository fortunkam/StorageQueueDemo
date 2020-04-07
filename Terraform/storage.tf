resource "azurerm_storage_account" "storage" {
  name                = "tfsta${lower(random_id.storageaccountname.hex)}"
  resource_group_name = azurerm_resource_group.storagequeue.name
  location                 = azurerm_resource_group.storagequeue.location
  account_tier             = "Standard"
  account_replication_type = "GRS"
}

resource "azurerm_storage_queue" "queue" {
  name                 = local.queueName
  storage_account_name = azurerm_storage_account.storage.name
}