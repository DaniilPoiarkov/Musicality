variable "resource_group" {
  description = "RG"
  type        = string
}

variable "location" {
  description = "Azure location for all resources"
  type        = string
}

variable "sku" {
  description = "SKU"
  type        = string
}

variable "product_name" {
  description = "Application name"
  type        = string
}

variable "environment" {
  description = "Environment"
  type        = string
}

variable "docker_username" {
  description = "Docker username"
  type        = string
  sensitive   = true
}

variable "docker_password" {
  description = "Docker password"
  type        = string
  sensitive   = true
}

variable "docker_image_name" {
  description = "Docker image"
  type        = string
}

variable "app_insights_connection_string" {
  description = "Application Insights connection string"
  type        = string
  sensitive   = true

  validation {
    error_message = "Application Insights connection string must be provided"
    condition = (var.app_insights_connection_string != null
      && var.app_insights_connection_string != ""
    )
  }
}

variable "telegram_secret_token" {
  type        = string
  description = "Authentication for telegram api calls"
  sensitive   = true
  validation {
    error_message = "Security token can not be empty"
    condition = (var.telegram_secret_token != null
      && var.telegram_secret_token != ""
    )
  }
}

variable "telegram_bot_token" {
  type        = string
  description = "Telegram bot token"
  sensitive   = true
  validation {
    error_message = "Bot token can not be empty"
    condition = (var.telegram_bot_token != null
      && var.telegram_bot_token != ""
    )
  }
}
