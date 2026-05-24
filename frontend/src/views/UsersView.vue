<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { usersApi } from '@/api/users.api'
import type { UserDto } from '@/types/auth.types'
import type { PagedList } from '@/types/common.types'

const data = ref<PagedList<UserDto> | null>(null)
const loading = ref(false)
const search = ref('')
const currentPage = ref(1)

async function fetchUsers() {
  loading.value = true
  try {
    data.value = await usersApi.getUsers({ page: currentPage.value, pageSize: 20, search: search.value || undefined })
  } finally {
    loading.value = false
  }
}

function onSearch() {
  currentPage.value = 1
  fetchUsers()
}

onMounted(fetchUsers)
</script>

<template>
  <div>
    <div class="mb-6 flex items-center justify-between">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Users</h1>
        <p class="mt-1 text-sm text-gray-500">Manage system users</p>
      </div>
    </div>

    <div class="bg-white rounded-xl border border-gray-200">
      <div class="p-4 border-b border-gray-200">
        <input
          v-model="search"
          @keyup.enter="onSearch"
          type="search"
          placeholder="Search users..."
          class="w-full max-w-xs px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
        />
      </div>

      <div v-if="loading" class="p-8 text-center text-sm text-gray-500">Loading...</div>

      <table v-else-if="data && data.items.length > 0" class="w-full">
        <thead>
          <tr class="border-b border-gray-200 bg-gray-50">
            <th class="text-left text-xs font-medium text-gray-500 px-4 py-3">Name</th>
            <th class="text-left text-xs font-medium text-gray-500 px-4 py-3">Email</th>
            <th class="text-left text-xs font-medium text-gray-500 px-4 py-3">Roles</th>
            <th class="text-left text-xs font-medium text-gray-500 px-4 py-3">Status</th>
            <th class="text-left text-xs font-medium text-gray-500 px-4 py-3">Joined</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100">
          <tr v-for="user in data.items" :key="user.id" class="hover:bg-gray-50 transition-colors">
            <td class="px-4 py-3 text-sm font-medium text-gray-900">{{ user.fullName }}</td>
            <td class="px-4 py-3 text-sm text-gray-600">{{ user.email }}</td>
            <td class="px-4 py-3">
              <div class="flex flex-wrap gap-1">
                <span v-for="role in user.roles" :key="role" class="px-2 py-0.5 text-xs bg-indigo-50 text-indigo-700 rounded-full">
                  {{ role }}
                </span>
              </div>
            </td>
            <td class="px-4 py-3">
              <span :class="user.isActive ? 'bg-green-50 text-green-700' : 'bg-red-50 text-red-700'" class="px-2 py-0.5 text-xs rounded-full">
                {{ user.isActive ? 'Active' : 'Inactive' }}
              </span>
            </td>
            <td class="px-4 py-3 text-sm text-gray-500">
              {{ new Date(user.createdAt).toLocaleDateString() }}
            </td>
          </tr>
        </tbody>
      </table>

      <div v-else class="p-8 text-center text-sm text-gray-500">No users found.</div>

      <div v-if="data && data.totalPages > 1" class="p-4 border-t border-gray-200 flex items-center justify-between">
        <p class="text-xs text-gray-500">{{ data.totalCount }} total users</p>
        <div class="flex items-center gap-2">
          <button
            :disabled="!data.hasPreviousPage"
            @click="() => { currentPage--; fetchUsers() }"
            class="px-3 py-1 text-sm border border-gray-300 rounded-lg disabled:opacity-50 hover:bg-gray-50 transition-colors"
          >Previous</button>
          <span class="text-xs text-gray-500">{{ currentPage }} / {{ data.totalPages }}</span>
          <button
            :disabled="!data.hasNextPage"
            @click="() => { currentPage++; fetchUsers() }"
            class="px-3 py-1 text-sm border border-gray-300 rounded-lg disabled:opacity-50 hover:bg-gray-50 transition-colors"
          >Next</button>
        </div>
      </div>
    </div>
  </div>
</template>
