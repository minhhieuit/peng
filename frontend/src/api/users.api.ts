import apiClient from '@/lib/axios'
import type { UserDto } from '@/types/auth.types'
import type { PagedList } from '@/types/common.types'

export const usersApi = {
  getUsers: (params: { page?: number; pageSize?: number; search?: string }) =>
    apiClient.get<PagedList<UserDto>>('/users', { params }).then((r) => r.data),
}
