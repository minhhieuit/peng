import apiClient from '@/lib/axios'
import type { UserDto } from '@/types/auth.types'
import type { PagedList } from '@/types/common.types'

export const usersApi = {
  getAll: (params: { page?: number; pageSize?: number; search?: string }) =>
    apiClient.get<PagedList<UserDto>>('/users', { params }).then(r => r.data),
  getById: (id: string) => apiClient.get<UserDto>(`/users/${id}`).then(r => r.data),
  update: (id: string, payload: { firstName: string; lastName: string }) =>
    apiClient.put<UserDto>(`/users/${id}`, payload).then(r => r.data),
  assignRoles: (id: string, roleIds: string[]) =>
    apiClient.put<UserDto>(`/users/${id}/roles`, { roleIds }).then(r => r.data),
  deactivate: (id: string) => apiClient.patch(`/users/${id}/deactivate`),
}
