import { defineStore } from 'pinia'
import type { MemberDto, MemberAuthResponse, LoginRequest, RegisterRequest } from '~/types/api.types'

export const useAuthStore = defineStore('auth', () => {
  const token = useCookie<string | null>('peng_token', {
    default: () => null,
    maxAge: 60 * 60 * 24,
    secure: false,
    sameSite: 'lax',
  })

  const user = useState<MemberDto | null>('auth_member', () => null)
  const isAuthenticated = computed(() => !!token.value)

  async function login(credentials: LoginRequest) {
    const $api = useApiClient()
    const data = await $api<MemberAuthResponse>('/auth/member/login', {
      method: 'POST',
      body: credentials,
    })
    token.value = data.accessToken
    user.value = data.member
    return data
  }

  async function register(payload: RegisterRequest) {
    const $api = useApiClient()
    const data = await $api<MemberAuthResponse>('/auth/member/register', {
      method: 'POST',
      body: payload,
    })
    token.value = data.accessToken
    user.value = data.member
    return data
  }

  async function fetchMe() {
    if (!token.value) return
    const $api = useApiClient()
    try {
      user.value = await $api<MemberDto>('/members/me')
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

  return { token, user, isAuthenticated, login, register, fetchMe, logout }
})
