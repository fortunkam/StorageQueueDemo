resource "azurerm_resource_group" "storagequeue" {
    name     = local.rg_name
    location = var.location
}