import apiClient from '@/lib/axios'
import type { RoleDto } from '@/types/identity.types'

export interface CreateRolePayload { name: string; description: string }
export interface UpdateRolePayload { name: string; description: string }

export const rolesApi = {
  getAll: () => apiClient.get<RoleDto[]>('/roles').then(r => r.data),
  getById: (id: string) => apiClient.get<RoleDto>(`/roles/${id}`).then(r => r.data),
  create: (payload: CreateRolePayload) => apiClient.post<RoleDto>('/roles', payload).then(r => r.data),
  update: (id: string, payload: UpdateRolePayload) => apiClient.put<RoleDto>(`/roles/${id}`, payload).then(r => r.data),
  delete: (id: string) => apiClient.delete(`/roles/${id}`),
  assignPermissions: (id: string, permissionCodes: string[]) =>
    apiClient.put<RoleDto>(`/roles/${id}/permissions`, { permissionCodes }).then(r => r.data),
}
