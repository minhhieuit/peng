<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { permissionsApi } from '@/api/permissions.api'
import type { PermissionDto } from '@/types/identity.types'
import AppBadge from '@/components/ui/AppBadge.vue'

const allPermissions = ref<PermissionDto[]>([])
const loading = ref(false)

const byModule = computed(() => {
  const groups: Record<string, PermissionDto[]> = {}
  for (const p of allPermissions.value) {
    ;(groups[p.module] ??= []).push(p)
  }
  return groups
})

onMounted(async () => {
  loading.value = true
  try { allPermissions.value = await permissionsApi.getAll() }
  finally { loading.value = false }
})
</script>

<template>
  <div>
    <div class="mb-6">
      <h1 class="text-xl font-bold text-gray-900">Permissions</h1>
      <p class="text-sm text-gray-500 mt-0.5">All system permissions (defined at compile time per module)</p>
    </div>

    <div v-if="loading" class="text-center py-12 text-sm text-gray-400">Loading...</div>

    <div v-else class="space-y-6">
      <div v-for="(perms, module) in byModule" :key="module" class="bg-white rounded-xl border border-gray-200 overflow-hidden">
        <div class="px-5 py-3 bg-gray-50 border-b border-gray-200 flex items-center gap-3">
          <AppBadge variant="indigo">{{ module }}</AppBadge>
          <span class="text-xs text-gray-400">{{ perms.length }} permissions</span>
        </div>
        <table class="w-full">
          <thead>
            <tr class="border-b border-gray-100 text-left text-xs font-medium text-gray-500 uppercase tracking-wide">
              <th class="px-5 py-2.5">Code</th>
              <th class="px-5 py-2.5">Name</th>
              <th class="px-5 py-2.5">Description</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr v-for="perm in perms" :key="perm.code" class="hover:bg-gray-50">
              <td class="px-5 py-3 font-mono text-xs text-indigo-600">{{ perm.code }}</td>
              <td class="px-5 py-3 text-sm font-medium text-gray-800">{{ perm.name }}</td>
              <td class="px-5 py-3 text-sm text-gray-500">{{ perm.description }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
