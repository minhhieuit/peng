import apiClient from '@/lib/axios'
import type { AuthResponse, LoginRequest, UserDto } from '@/types/auth.types'

export const authApi = {
  login: (data: LoginRequest) =>
    apiClient.post<AuthResponse>('/auth/login', data).then((r) => r.data),

  getMe: () =>
    apiClient.get<UserDto>('/users/me').then((r) => r.data),
}
