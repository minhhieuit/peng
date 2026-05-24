<script setup lang="ts">
const auth = useAuthStore()
</script>

<template>
  <header class="bg-white border-b border-gray-200">
    <div class="max-w-6xl mx-auto px-6 h-16 flex items-center justify-between">
      <NuxtLink to="/" class="text-lg font-bold text-gray-900">Peng</NuxtLink>
      <nav class="flex items-center gap-6">
        <template v-if="auth.isAuthenticated">
          <NuxtLink to="/dashboard" class="text-sm text-gray-600 hover:text-gray-900">Dashboard</NuxtLink>
          <button class="text-sm text-gray-600 hover:text-gray-900" @click="auth.logout">Sign out</button>
        </template>
        <template v-else>
          <NuxtLink to="/auth/login" class="text-sm text-gray-600 hover:text-gray-900">Sign in</NuxtLink>
          <NuxtLink
            to="/auth/register"
            class="text-sm bg-blue-600 hover:bg-blue-700 text-white px-4 py-1.5 rounded-lg transition-colors"
          >
            Get started
          </NuxtLink>
        </template>
      </nav>
    </div>
  </header>
</template>
