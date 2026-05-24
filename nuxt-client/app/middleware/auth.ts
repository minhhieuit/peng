export default defineNuxtRouteMiddleware(async () => {
  const auth = useAuthStore()
  if (!auth.token) return navigateTo('/auth/login')
  if (!auth.user) await auth.fetchMe()
  if (!auth.isAuthenticated) return navigateTo('/auth/login')
})
