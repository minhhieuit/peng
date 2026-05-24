<script setup lang="ts">
import { useAuthStore } from '@/stores/auth.store'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

function logout() {
  authStore.logout()
  router.push({ name: 'login' })
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <header class="bg-white shadow-sm">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <div class="flex items-center gap-8">
          <span class="text-xl font-bold text-indigo-600">Peng</span>
          <nav class="flex items-center gap-6 text-sm font-medium text-gray-600">
            <RouterLink to="/" class="hover:text-indigo-600 transition-colors">Dashboard</RouterLink>
            <RouterLink v-if="authStore.hasPermission('identity:users:read')" to="/users" class="hover:text-indigo-600 transition-colors">Users</RouterLink>
          </nav>
        </div>
        <div class="flex items-center gap-4">
          <span class="text-sm text-gray-600">{{ authStore.user?.fullName }}</span>
          <button @click="logout" class="text-sm text-red-500 hover:text-red-700 transition-colors">
            Logout
          </button>
        </div>
      </div>
    </header>

    <main class="flex-1 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <RouterView />
    </main>
  </div>
</template>
