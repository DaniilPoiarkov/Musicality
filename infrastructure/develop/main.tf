data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "resource_group_main" {
  location = var.location
  name     = "rg-${var.product_name}-${var.environment}"

  tags = {
    environment = var.environment
    management  = "terraform"
    product     = var.product_name
  }
}

module "monitoring" {
  source         = "../modules/monitoring"
  resource_group = azurerm_resource_group.resource_group_main.name
  environment    = var.environment
  location       = var.location
  product_name   = var.product_name
}

module "webapp" {
  source                         = "../modules/webapp"
  resource_group                 = azurerm_resource_group.resource_group_main.name
  location                       = var.location
  environment                    = var.environment
  sku                            = var.service_plan_sku_name
  product_name                   = var.product_name
  docker_username                = var.docker_username
  docker_password                = var.docker_password
  app_insights_connection_string = module.monitoring.appi_connection_string
}
