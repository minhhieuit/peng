import { useAuthStore } from '@/stores/auth.store'

export function usePermission() {
  const authStore = useAuthStore()

  return {
    can: (permission: string) => authStore.hasPermission(permission),
    hasRole: (role: string) => authStore.hasRole(role),
    isAdmin: () => authStore.hasRole('Admin'),
  }
}
