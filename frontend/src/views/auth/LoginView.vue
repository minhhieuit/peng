<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import type { AxiosError } from 'axios'

const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()

const email = ref('')
const password = ref('')
const error = ref('')

async function handleSubmit() {
  error.value = ''
  try {
    await authStore.login({ email: email.value, password: password.value })
    const redirect = route.query.redirect as string | undefined
    router.push(redirect || { name: 'dashboard' })
  } catch (e) {
    const axiosError = e as AxiosError<{ description: string }>
    error.value = axiosError.response?.data?.description || 'Invalid email or password.'
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex items-center justify-center px-4">
    <div class="w-full max-w-md">
      <div class="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
        <div class="mb-8 text-center">
          <h1 class="text-2xl font-bold text-gray-900">Welcome back</h1>
          <p class="mt-1 text-sm text-gray-500">Sign in to your account</p>
        </div>

        <form @submit.prevent="handleSubmit" class="space-y-5">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Email</label>
            <input
              v-model="email"
              type="email"
              required
              autocomplete="email"
              placeholder="you@example.com"
              class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
            />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Password</label>
            <input
              v-model="password"
              type="password"
              required
              autocomplete="current-password"
              placeholder="••••••••"
              class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
            />
          </div>

          <div v-if="error" class="text-sm text-red-500 bg-red-50 rounded-lg px-3 py-2">
            {{ error }}
          </div>

          <button
            type="submit"
            :disabled="authStore.loading"
            class="w-full bg-indigo-600 text-white py-2 px-4 rounded-lg text-sm font-medium hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          >
            {{ authStore.loading ? 'Signing in...' : 'Sign in' }}
          </button>
        </form>
      </div>
    </div>
  </div>
</template>
