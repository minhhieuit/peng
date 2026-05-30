import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: "2025-05-24",

  future: {
    compatibilityVersion: 4,
  },

  devtools: { enabled: true },

  modules: ["@pinia/nuxt"],

  css: ["~/assets/css/main.css"],

  vite: {
    plugins: [tailwindcss()],
  },

  routeRules: {
    "/": { prerender: true },
    "/auth/**": { ssr: false },
    "/dashboard/**": { ssr: true },
    "/courses": { isr: 60 },
    "/courses/**": { ssr: true },
  },

  runtimeConfig: {
    public: {
      // Absolute backend URL so both client-side and SSR fetches reach the API.
      // Override per environment via NUXT_PUBLIC_API_BASE.
      apiBase: process.env.NUXT_PUBLIC_API_BASE,
    },
  },

  app: {
    head: {
      charset: "utf-8",
      viewport: "width=device-width, initial-scale=1",
      htmlAttrs: { lang: "en" },
    },
  },
});
