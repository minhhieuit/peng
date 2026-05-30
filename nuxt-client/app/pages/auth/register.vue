<script setup lang="ts">
definePageMeta({ layout: 'auth', middleware: 'guest' })

useSeoMeta({ title: 'Register — Peng', robots: 'noindex' })

const auth = useAuthStore()
const form = reactive({ email: '', password: '', firstName: '', lastName: '' })
const error = ref('')
const loading = ref(false)

async function submit() {
  error.value = ''
  loading.value = true
  try {
    await auth.register(form)
    await navigateTo('/dashboard')
  } catch (e: any) {
    error.value = e?.data?.message ?? 'Registration failed'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <form @submit.prevent="submit" class="space-y-4">
    <div class="mb-6">
      <h2 class="text-xl font-semibold text-gray-900">Create account</h2>
      <p class="text-sm text-gray-500 mt-1">Get started for free</p>
    </div>

    <div class="grid grid-cols-2 gap-3">
      <UiInput v-model="form.firstName" label="First name" />
      <UiInput v-model="form.lastName" label="Last name" />
    </div>
    <UiInput v-model="form.email" label="Email" type="email" />
    <UiInput v-model="form.password" label="Password" type="password" hint="At least 8 characters" />

    <p v-if="error" class="text-sm text-red-600">{{ error }}</p>

    <UiButton type="submit" :loading="loading" class="w-full mt-2">
      Create account
    </UiButton>

    <p class="text-center text-sm text-gray-500 mt-4">
      Already have an account?
      <NuxtLink to="/auth/login" class="text-blue-600 hover:underline">Sign in</NuxtLink>
    </p>
  </form>
</template>
