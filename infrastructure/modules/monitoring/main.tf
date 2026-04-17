locals {
  tags = {
    management  = "terraform"
    product     = var.product_name
    environment = var.environment
  }
}

resource "azurerm_log_analytics_workspace" "log_analytics_workspace" {
  location            = var.location
  resource_group_name = var.resource_group
  name                = "${var.product_name}-${var.environment}-log"
  tags                = local.tags
}


resource "azurerm_application_insights" "app_insights" {
  workspace_id        = azurerm_log_analytics_workspace.log_analytics_workspace.id
  name                = "${var.product_name}-${var.environment}-appi"
  location            = var.location
  resource_group_name = var.resource_group
  application_type    = "web"
  tags                = local.tags

  depends_on = [
    azurerm_log_analytics_workspace.log_analytics_workspace
  ]
}
