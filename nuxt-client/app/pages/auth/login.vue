<script setup lang="ts">
definePageMeta({ layout: 'auth', middleware: 'guest' })

useSeoMeta({ title: 'Sign in — Peng', robots: 'noindex' })

const auth = useAuthStore()
const form = reactive({ email: '', password: '' })
const error = ref('')
const loading = ref(false)

async function submit() {
  error.value = ''
  loading.value = true
  try {
    await auth.login(form)
    await navigateTo('/dashboard')
  } catch (e: any) {
    error.value = e?.data?.message ?? 'Invalid email or password'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <form @submit.prevent="submit" class="space-y-4">
    <div class="mb-6">
      <h2 class="text-xl font-semibold text-gray-900">Welcome back</h2>
      <p class="text-sm text-gray-500 mt-1">Sign in to your account</p>
    </div>

    <UiInput v-model="form.email" label="Email" type="email" />
    <UiInput v-model="form.password" label="Password" type="password" />

    <p v-if="error" class="text-sm text-red-600">{{ error }}</p>

    <UiButton type="submit" :loading="loading" class="w-full mt-2">
      Sign in
    </UiButton>

    <p class="text-center text-sm text-gray-500 mt-4">
      Don't have an account?
      <NuxtLink to="/auth/register" class="text-blue-600 hover:underline">Register</NuxtLink>
    </p>
  </form>
</template>
