<script setup lang="ts">
import { useAuthStore } from '@/stores/auth.store'

const authStore = useAuthStore()
</script>

<template>
  <div>
    <div class="mb-8">
      <h1 class="text-2xl font-bold text-gray-900">Dashboard</h1>
      <p class="mt-1 text-sm text-gray-500">Welcome back, {{ authStore.user?.firstName }}!</p>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      <div class="bg-white rounded-xl border border-gray-200 p-6">
        <p class="text-sm font-medium text-gray-500">Roles</p>
        <div class="mt-2 flex flex-wrap gap-2">
          <span
            v-for="role in authStore.roles"
            :key="role"
            class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-indigo-100 text-indigo-800"
          >
            {{ role }}
          </span>
        </div>
      </div>

      <div class="bg-white rounded-xl border border-gray-200 p-6">
        <p class="text-sm font-medium text-gray-500">Permissions</p>
        <p class="mt-1 text-2xl font-bold text-gray-900">{{ authStore.permissions.length }}</p>
      </div>

      <div class="bg-white rounded-xl border border-gray-200 p-6">
        <p class="text-sm font-medium text-gray-500">Account Status</p>
        <div class="mt-2">
          <span class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
            Active
          </span>
        </div>
      </div>
    </div>

    <div v-if="authStore.permissions.length > 0" class="bg-white rounded-xl border border-gray-200 p-6">
      <h2 class="text-sm font-semibold text-gray-700 mb-3">Your Permissions</h2>
      <div class="flex flex-wrap gap-2">
        <span
          v-for="perm in authStore.permissions"
          :key="perm"
          class="inline-flex items-center px-2.5 py-0.5 rounded-md text-xs font-mono bg-gray-100 text-gray-700"
        >
          {{ perm }}
        </span>
      </div>
    </div>
  </div>
</template>
