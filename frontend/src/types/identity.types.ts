export interface RoleDto {
  id: string
  name: string
  description: string
  isSystem: boolean
  createdAt: string
  permissions: PermissionDto[]
}

export interface PermissionDto {
  id: string
  code: string
  name: string
  description: string
  module: string
}
