<script setup lang="ts">
definePageMeta({ layout: 'dashboard', title: 'Dashboard', middleware: 'auth' })

useSeoMeta({ title: 'Dashboard — Peng', robots: 'noindex' })

const auth = useAuthStore()

if (!auth.user) await auth.fetchMe()
</script>

<template>
  <div class="space-y-6">
    <div>
      <h2 class="text-2xl font-bold text-gray-900">Welcome, {{ auth.user?.firstName }}!</h2>
      <p class="text-gray-500 mt-1">Here's an overview of your account.</p>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <div class="bg-white rounded-xl border border-gray-200 p-5">
        <p class="text-xs font-medium text-gray-500 uppercase tracking-wide">Account type</p>
        <p class="text-2xl font-bold text-gray-900 mt-1">Member</p>
      </div>
      <div class="bg-white rounded-xl border border-gray-200 p-5">
        <p class="text-xs font-medium text-gray-500 uppercase tracking-wide">Member since</p>
        <p class="text-2xl font-bold text-gray-900 mt-1">
          {{ auth.user?.createdAt ? new Date(auth.user.createdAt).toLocaleDateString() : '—' }}
        </p>
      </div>
      <div class="bg-white rounded-xl border border-gray-200 p-5">
        <p class="text-xs font-medium text-gray-500 uppercase tracking-wide">Status</p>
        <p class="text-2xl font-bold mt-1" :class="auth.user?.isActive ? 'text-green-600' : 'text-red-500'">
          {{ auth.user?.isActive ? 'Active' : 'Inactive' }}
        </p>
      </div>
    </div>

    <div class="bg-white rounded-xl border border-gray-200 p-5">
      <h3 class="text-sm font-semibold text-gray-900 mb-3">Account details</h3>
      <dl class="space-y-2 text-sm">
        <div class="flex gap-4">
          <dt class="text-gray-500 w-24">Email</dt>
          <dd class="text-gray-900">{{ auth.user?.email }}</dd>
        </div>
        <div class="flex gap-4">
          <dt class="text-gray-500 w-24">Name</dt>
          <dd class="text-gray-900">{{ auth.user?.fullName }}</dd>
        </div>
        <div class="flex gap-4">
          <dt class="text-gray-500 w-24">Member since</dt>
          <dd class="text-gray-900">{{ auth.user?.createdAt ? new Date(auth.user.createdAt).toLocaleDateString() : '—' }}</dd>
        </div>
      </dl>
    </div>
  </div>
</template>
