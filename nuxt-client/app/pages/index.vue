<script setup lang="ts">
useSeoMeta({
  title: 'Peng — Core Base System',
  description: 'A modern base system built with .NET 10 and Nuxt for rapid project development.',
  ogTitle: 'Peng — Core Base System',
  ogDescription: 'A modern base system built with .NET 10 and Nuxt for rapid project development.',
  ogType: 'website',
})

const features = [
  { icon: '🔐', title: 'Authentication', description: 'JWT-based auth with register, login, and secure token management.' },
  { icon: '🛡️', title: 'RBAC Permissions', description: 'Fine-grained role-based access control with per-module permission constants.' },
  { icon: '⚡', title: 'Modular Monolith', description: '.NET 10 backend with clean architecture and auto-discovering modules.' },
]
</script>

<template>
  <div>
    <section class="max-w-6xl mx-auto px-6 py-24 text-center">
      <h1 class="text-5xl font-bold text-gray-900 leading-tight">
        Build faster with<br />
        <span class="text-blue-600">Peng</span>
      </h1>
      <p class="mt-6 text-xl text-gray-500 max-w-2xl mx-auto">
        A production-ready base system with authentication, role-based permissions,
        and a clean modular architecture.
      </p>
      <div class="mt-10 flex justify-center gap-4">
        <NuxtLink to="/auth/register" class="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-xl font-medium transition-colors">
          Get started
        </NuxtLink>
        <NuxtLink to="/auth/login" class="bg-gray-100 hover:bg-gray-200 text-gray-800 px-6 py-3 rounded-xl font-medium transition-colors">
          Sign in
        </NuxtLink>
      </div>
    </section>
    <section class="bg-gray-50 py-20">
      <div class="max-w-6xl mx-auto px-6">
        <h2 class="text-2xl font-bold text-gray-900 text-center mb-12">Everything you need</h2>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div v-for="feature in features" :key="feature.title" class="bg-white rounded-2xl p-6 shadow-sm border border-gray-100">
            <div class="text-2xl mb-3">{{ feature.icon }}</div>
            <h3 class="font-semibold text-gray-900 mb-2">{{ feature.title }}</h3>
            <p class="text-sm text-gray-500">{{ feature.description }}</p>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>
