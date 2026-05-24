export interface PagedList<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export interface ModuleDescriptor {
  moduleName: string
  description: string
  version: string
  permissions: PermissionDescriptor[]
  features: FeatureDescriptor[]
}

export interface PermissionDescriptor {
  code: string
  name: string
  description: string
  category: string
}

export interface FeatureDescriptor {
  name: string
  description: string
  businessRules: string[]
}
