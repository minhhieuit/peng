<script setup lang="ts">
definePageMeta({ middleware: 'auth' })

const auth = useAuthStore()
const route = useRoute()

const nav = [
  { label: 'Dashboard', to: '/dashboard', icon: 'home' },
  { label: 'My Courses', to: '/dashboard/my-courses', icon: 'book' },
  { label: 'Profile', to: '/dashboard/profile', icon: 'user' },
]
</script>

<template>
  <div class="min-h-screen flex bg-gray-50">
    <!-- Sidebar -->
    <aside class="w-60 bg-white border-r border-gray-200 flex flex-col">
      <div class="px-6 py-5 border-b border-gray-100">
        <span class="text-lg font-bold text-gray-900">Peng</span>
      </div>
      <nav class="flex-1 px-3 py-4 space-y-0.5">
        <NuxtLink
          v-for="item in nav"
          :key="item.to"
          :to="item.to"
          class="flex items-center gap-3 px-3 py-2 rounded-lg text-sm transition-colors"
          :class="route.path === item.to
            ? 'bg-blue-50 text-blue-700 font-medium'
            : 'text-gray-600 hover:bg-gray-100'"
        >
          {{ item.label }}
        </NuxtLink>
      </nav>
      <div class="px-4 py-4 border-t border-gray-100">
        <div class="flex items-center gap-3 mb-3">
          <div class="w-8 h-8 rounded-full bg-blue-600 flex items-center justify-center text-white text-sm font-medium">
            {{ auth.user?.firstName?.[0] ?? '?' }}
          </div>
          <div class="flex-1 min-w-0">
            <p class="text-sm font-medium text-gray-900 truncate">{{ auth.user?.fullName }}</p>
            <p class="text-xs text-gray-500 truncate">{{ auth.user?.email }}</p>
          </div>
        </div>
        <button
          class="w-full text-sm text-gray-500 hover:text-gray-900 text-left transition-colors"
          @click="auth.logout"
        >
          Sign out
        </button>
      </div>
    </aside>

    <!-- Main -->
    <div class="flex-1 flex flex-col min-w-0">
      <header class="bg-white border-b border-gray-200 px-8 py-4">
        <h1 class="text-sm font-medium text-gray-700">{{ route.meta.title ?? 'Dashboard' }}</h1>
      </header>
      <main class="flex-1 p-8">
        <slot />
      </main>
    </div>
  </div>
</template>
