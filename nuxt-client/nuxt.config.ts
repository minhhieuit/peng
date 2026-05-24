import tailwindcss from '@tailwindcss/vite'

export default defineNuxtConfig({
  compatibilityDate: '2025-05-24',

  future: {
    compatibilityVersion: 4,
  },

  devtools: { enabled: true },

  modules: [
    '@pinia/nuxt',
    '@nuxtjs/seo',
  ],

  css: ['~/assets/css/main.css'],

  vite: {
    plugins: [tailwindcss()],
  },

  routeRules: {
    '/': { prerender: true },
    '/auth/**': { ssr: false },
    '/dashboard/**': { ssr: true },
  },

  runtimeConfig: {
    public: {
      apiBase: '',
    },
  },

  site: {
    url: 'https://example.com',
    name: 'Peng',
    description: 'Powered by Peng base system',
    defaultLocale: 'en',
  },
})
