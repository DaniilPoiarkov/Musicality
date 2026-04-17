variable "subscription_id" {
  description = "subscription id"
  type        = string
  sensitive   = true
}

variable "location" {
  description = "Azure location for all resources"
  type        = string
}

variable "environment" {
  description = "Environment"
  type        = string
}

variable "product_name" {
  description = "Application name"
  type        = string
}

variable "service_plan_sku_name" {
  description = "App service plan"
  type        = string
}

variable "bot_token" {
  description = "Bot token"
  type        = string
  sensitive   = true
  default     = "SHOULD_BE_SET"
}

variable "chat_id" {
  description = "Administrator chat id"
  type        = number
  sensitive   = true
}

variable "secret_token" {
  description = "Secret token"
  type        = string
  sensitive   = true
  default     = "SHOULD_BE_SET"
}

variable "docker_username" {
  description = "Docker username"
  type        = string
  sensitive   = true
  default     = "SHOULD_BE_SET"
}

variable "docker_password" {
  description = "Docker password"
  type        = string
  sensitive   = true
  default     = "SHOULD_BE_SET"
}

variable "image_name" {
  description = "Image name"
  type        = string
  default     = "SHOULD_BE_SET"
}
