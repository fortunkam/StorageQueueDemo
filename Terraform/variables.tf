variable location {
    default="centralus"
}
variable prefix {
    default="storagedemo"
}

resource "random_id" "storageaccountname" {
  keepers = {
    resource_group = azurerm_resource_group.storagequeue.name
  }
  byte_length = 8
}

locals {
    rg_name = "${var.prefix}-rg"
    keyvault_name = "${var.prefix}-kv"
    queueName="demo"
}

data "azurerm_client_config" "current" {
}
