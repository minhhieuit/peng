import type { FetchOptions } from 'ofetch'

export function useApiClient() {
  const config = useRuntimeConfig()
  const token = useCookie<string | null>('peng_token')

  return $fetch.create({
    baseURL: `${config.public.apiBase}/api`,
    onRequest({ options }: { options: FetchOptions }) {
      if (token.value) {
        options.headers = new Headers(options.headers as HeadersInit)
        options.headers.set('Authorization', `Bearer ${token.value}`)
      }
    },
    onResponseError({ response }: { response: Response }) {
      if (response.status === 401 && import.meta.client) {
        token.value = null
        navigateTo('/auth/login')
      }
    },
  })
}
