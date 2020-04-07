
resource "azurerm_key_vault" "keyvault" {
  name                        = local.keyvault_name
  location                    = azurerm_resource_group.storagequeue.location
  resource_group_name         = azurerm_resource_group.storagequeue.name
  tenant_id                   = data.azurerm_client_config.current.tenant_id

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "get",
    ]

    secret_permissions = [
      "get",
      "set",
      "list",
      "delete"
    ]

    storage_permissions = [
      "get",
    ]
  }

}

resource "azurerm_key_vault_secret" "storage_connection_string" {
  name         = "StorageConnectionString"
  value        = azurerm_storage_account.storage.primary_connection_string 
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "queueName" {
  name         = "QueueName"
  value        = local.queueName
  key_vault_id = azurerm_key_vault.keyvault.id
}