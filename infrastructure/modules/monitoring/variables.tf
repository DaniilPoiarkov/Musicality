variable "resource_group" {
  description = "RG"
  type        = string
}

variable "location" {
  description = "Azure location for all resources"
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
