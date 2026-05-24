export function useApiFetch<T>(url: string | (() => string), options: Record<string, unknown> = {}) {
  const config = useRuntimeConfig()
  const token = useCookie<string | null>('peng_token')

  return useFetch<T>(url, {
    ...options,
    baseURL: `${config.public.apiBase}/api`,
    headers: {
      ...(token.value ? { Authorization: `Bearer ${token.value}` } : {}),
      ...((options.headers as Record<string, string>) ?? {}),
    },
  })
}
