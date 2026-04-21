locals {
  tags = {
    product     = var.product_name
    environment = var.environment
    management  = "terraform"
  }
}

resource "azurerm_service_plan" "asp" {
  resource_group_name = var.resource_group
  location            = var.location
  os_type             = "Linux"
  sku_name            = var.sku
  name                = "asp-${var.product_name}-${var.environment}"
  tags                = local.tags
}

resource "azurerm_linux_web_app" "app" {
  service_plan_id                                = azurerm_service_plan.asp.id
  resource_group_name                            = var.resource_group
  name                                           = "app-${var.product_name}-${var.environment}"
  location                                       = var.location
  https_only                                     = true
  tags                                           = local.tags
  ftp_publish_basic_authentication_enabled       = false
  webdeploy_publish_basic_authentication_enabled = false

  site_config {
    always_on           = (var.sku != "F1" && var.sku != "Free")
    api_definition_url  = "https://${var.product_name}-${var.environment}-app.azurewebsites.net/swagger/index.html"
    ftps_state          = "FtpsOnly"
    http2_enabled       = true
    minimum_tls_version = "1.3"

    application_stack {
      docker_registry_url      = "https://ghcr.io"
      docker_registry_username = var.docker_username
      docker_registry_password = var.docker_password
      docker_image_name        = var.docker_image_name
    }
  }

  logs {
    http_logs {
      file_system {
        retention_in_days = 7
        retention_in_mb   = 35
      }
    }
    application_logs {
      file_system_level = "Warning"
    }
    detailed_error_messages = true
    failed_request_tracing  = true
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT                     = var.environment
    APPLICATIONINSIGHTS_CONNECTION_STRING      = var.app_insights_connection_string
    ApplicationInsightsAgent_EXTENSION_VERSION = "~3"
    XDT_MicrosoftApplicationInsights_Mode      = "Recommended"
    Telegram__WebhookUrl                       = "https://${var.product_name}-${var.environment}-app.azurewebsites.net/api/telegram/_handle"
    Telegram__SecretToken                      = var.telegram_secret_token
    Telegram__Token                            = var.telegram_bot_token
  }

  sticky_settings {
    app_setting_names = [
      "ASPNETCORE_ENVIRONMENT",
      "APPLICATIONINSIGHTS_CONNECTION_STRING",
      "ApplicationInsightsAgent_EXTENSION_VERSION",
      "XDT_MicrosoftApplicationInsights_Mode",
      "Telegram__WebhookUrl"
    ]
  }
}
