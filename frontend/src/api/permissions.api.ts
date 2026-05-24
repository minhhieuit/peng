import apiClient from '@/lib/axios'
import type { PermissionDto } from '@/types/identity.types'

export const permissionsApi = {
  getAll: () => apiClient.get<PermissionDto[]>('/permissions').then(r => r.data),
}
