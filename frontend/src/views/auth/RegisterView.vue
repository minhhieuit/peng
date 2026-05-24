<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import type { AxiosError } from 'axios'

const authStore = useAuthStore()
const router = useRouter()

const form = ref({
  email: '',
  password: '',
  firstName: '',
  lastName: '',
})
const error = ref('')

async function handleSubmit() {
  error.value = ''
  try {
    await authStore.register(form.value)
    router.push({ name: 'dashboard' })
  } catch (e) {
    const axiosError = e as AxiosError<{ description: string }>
    error.value = axiosError.response?.data?.description || 'Registration failed. Please try again.'
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex items-center justify-center px-4">
    <div class="w-full max-w-md">
      <div class="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
        <div class="mb-8 text-center">
          <h1 class="text-2xl font-bold text-gray-900">Create an account</h1>
          <p class="mt-1 text-sm text-gray-500">Get started for free</p>
        </div>

        <form @submit.prevent="handleSubmit" class="space-y-5">
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">First name</label>
              <input
                v-model="form.firstName"
                type="text"
                required
                placeholder="John"
                class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Last name</label>
              <input
                v-model="form.lastName"
                type="text"
                required
                placeholder="Doe"
                class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
              />
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Email</label>
            <input
              v-model="form.email"
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
              v-model="form.password"
              type="password"
              required
              placeholder="Min. 8 characters"
              class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
            />
            <p class="mt-1 text-xs text-gray-400">Must contain uppercase, lowercase, and a digit</p>
          </div>

          <div v-if="error" class="text-sm text-red-500 bg-red-50 rounded-lg px-3 py-2">
            {{ error }}
          </div>

          <button
            type="submit"
            :disabled="authStore.loading"
            class="w-full bg-indigo-600 text-white py-2 px-4 rounded-lg text-sm font-medium hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          >
            {{ authStore.loading ? 'Creating account...' : 'Create account' }}
          </button>
        </form>

        <p class="mt-6 text-center text-sm text-gray-500">
          Already have an account?
          <RouterLink to="/login" class="text-indigo-600 hover:text-indigo-700 font-medium">Sign in</RouterLink>
        </p>
      </div>
    </div>
  </div>
</template>
