import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { authApi } from '@/api/auth.api'
import type { UserDto, LoginRequest, RegisterRequest } from '@/types/auth.types'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<UserDto | null>(null)
  const token = ref<string | null>(localStorage.getItem('access_token'))
  const loading = ref(false)

  const isAuthenticated = computed(() => !!token.value && !!user.value)
  const permissions = computed(() => user.value?.permissions ?? [])
  const roles = computed(() => user.value?.roles ?? [])

  function hasPermission(permission: string) {
    return permissions.value.includes(permission)
  }

  function hasRole(role: string) {
    return roles.value.includes(role)
  }

  async function login(data: LoginRequest) {
    loading.value = true
    try {
      const response = await authApi.login(data)
      token.value = response.accessToken
      user.value = response.user
      localStorage.setItem('access_token', response.accessToken)
    } finally {
      loading.value = false
    }
  }

  async function register(data: RegisterRequest) {
    loading.value = true
    try {
      const response = await authApi.register(data)
      token.value = response.accessToken
      user.value = response.user
      localStorage.setItem('access_token', response.accessToken)
    } finally {
      loading.value = false
    }
  }

  async function fetchMe() {
    if (!token.value) return
    try {
      user.value = await authApi.getMe()
    } catch {
      logout()
    }
  }

  function logout() {
    user.value = null
    token.value = null
    localStorage.removeItem('access_token')
  }

  return { user, token, loading, isAuthenticated, permissions, roles, hasPermission, hasRole, login, register, fetchMe, logout }
})
