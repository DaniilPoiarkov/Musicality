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
