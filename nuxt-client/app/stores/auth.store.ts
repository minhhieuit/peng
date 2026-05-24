import { defineStore } from 'pinia'
import type { UserDto, AuthResponse, LoginRequest, RegisterRequest } from '~/types/api.types'

export const useAuthStore = defineStore('auth', () => {
  const token = useCookie<string | null>('peng_token', {
    default: () => null,
    maxAge: 60 * 60 * 24,
    secure: false,
    sameSite: 'lax',
  })

  const user = useState<UserDto | null>('auth_user', () => null)
  const isAuthenticated = computed(() => !!token.value)

  async function login(credentials: LoginRequest) {
    const $api = useApiClient()
    const data = await $api<AuthResponse>('/auth/login', {
      method: 'POST',
      body: credentials,
    })
    token.value = data.accessToken
    user.value = data.user
    return data
  }

  async function register(payload: RegisterRequest) {
    const $api = useApiClient()
    await $api('/auth/register', {
      method: 'POST',
      body: payload,
    })
  }

  async function fetchMe() {
    if (!token.value) return
    const $api = useApiClient()
    try {
      user.value = await $api<UserDto>('/users/me')
    } catch {
      token.value = null
      user.value = null
    }
  }

  function logout() {
    token.value = null
    user.value = null
    return navigateTo('/auth/login')
  }

  function hasPermission(permission: string): boolean {
    return user.value?.permissions.includes(permission) ?? false
  }

  return { token, user, isAuthenticated, login, register, fetchMe, logout, hasPermission }
})
