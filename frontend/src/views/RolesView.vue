<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { rolesApi } from '@/api/roles.api'
import { permissionsApi } from '@/api/permissions.api'
import type { RoleDto, PermissionDto } from '@/types/identity.types'
import { useToast } from '@/composables/useToast'
import { useConfirm } from '@/composables/useConfirm'
import AppButton from '@/components/ui/AppButton.vue'
import AppModal from '@/components/ui/AppModal.vue'
import AppInput from '@/components/ui/AppInput.vue'
import AppBadge from '@/components/ui/AppBadge.vue'
import { useAuthStore } from '@/stores/auth.store'

const toast = useToast()
const { confirm } = useConfirm()
const auth = useAuthStore()

const roles = ref<RoleDto[]>([])
const allPermissions = ref<PermissionDto[]>([])
const loading = ref(false)

// Create/Edit modal
const formModal = ref(false)
const isEditing = ref(false)
const formTarget = ref<RoleDto | null>(null)
const form = ref({ name: '', description: '' })
const formLoading = ref(false)

// Permissions modal
const permModal = ref(false)
const permTarget = ref<RoleDto | null>(null)
const selectedPerms = ref<string[]>([])
const permLoading = ref(false)

const permissionsByModule = computed(() => {
  const groups: Record<string, PermissionDto[]> = {}
  for (const p of allPermissions.value) {
    ;(groups[p.module] ??= []).push(p)
  }
  return groups
})

async function fetchAll() {
  loading.value = true
  try {
    [roles.value, allPermissions.value] = await Promise.all([rolesApi.getAll(), permissionsApi.getAll()])
  } finally {
    loading.value = false
  }
}

function openCreate() {
  isEditing.value = false
  formTarget.value = null
  form.value = { name: '', description: '' }
  formModal.value = true
}

function openEdit(role: RoleDto) {
  isEditing.value = true
  formTarget.value = role
  form.value = { name: role.name, description: role.description }
  formModal.value = true
}

async function saveForm() {
  formLoading.value = true
  try {
    if (isEditing.value && formTarget.value) {
      await rolesApi.update(formTarget.value.id, form.value)
      toast.success('Role updated.')
    } else {
      await rolesApi.create(form.value)
      toast.success('Role created.')
    }
    formModal.value = false
    fetchAll()
  } catch {
    toast.error('Failed to save role.')
  } finally {
    formLoading.value = false
  }
}

async function deleteRole(role: RoleDto) {
  const ok = await confirm({
    title: 'Delete Role',
    message: `Delete role "${role.name}"? This cannot be undone.`,
    confirmText: 'Delete',
    danger: true,
  })
  if (!ok) return
  try {
    await rolesApi.delete(role.id)
    toast.success('Role deleted.')
    fetchAll()
  } catch {
    toast.error('Cannot delete this role.')
  }
}

function openPermissions(role: RoleDto) {
  permTarget.value = role
  selectedPerms.value = role.permissions.map(p => p.code)
  permModal.value = true
}

async function savePermissions() {
  if (!permTarget.value) return
  permLoading.value = true
  try {
    await rolesApi.assignPermissions(permTarget.value.id, selectedPerms.value)
    toast.success('Permissions updated.')
    permModal.value = false
    fetchAll()
  } catch {
    toast.error('Failed to update permissions.')
  } finally {
    permLoading.value = false
  }
}

onMounted(fetchAll)
</script>

<template>
  <div>
    <div class="mb-6 flex items-center justify-between">
      <div>
        <h1 class="text-xl font-bold text-gray-900">Roles</h1>
        <p class="text-sm text-gray-500 mt-0.5">Manage roles and their permissions</p>
      </div>
      <AppButton v-if="auth.hasPermission('identity:roles:write')" @click="openCreate">
        <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15"/>
        </svg>
        New Role
      </AppButton>
    </div>

    <div v-if="loading" class="text-center py-12 text-sm text-gray-400">Loading...</div>

    <div v-else class="grid gap-4">
      <div v-for="role in roles" :key="role.id" class="bg-white rounded-xl border border-gray-200 p-5">
        <div class="flex items-start justify-between">
          <div class="flex items-center gap-3">
            <div class="w-9 h-9 rounded-lg bg-indigo-100 text-indigo-600 flex items-center justify-center">
              <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.8">
                <path stroke-linecap="round" stroke-linejoin="round" d="M9 12.75L11.25 15 15 9.75m-3-7.036A11.959 11.959 0 013.598 6 11.99 11.99 0 003 9.749c0 5.592 3.824 10.29 9 11.623 5.176-1.332 9-6.03 9-11.622 0-1.31-.21-2.571-.598-3.751h-.152c-3.196 0-6.1-1.248-8.25-3.285z"/>
              </svg>
            </div>
            <div>
              <div class="flex items-center gap-2">
                <h3 class="text-sm font-semibold text-gray-900">{{ role.name }}</h3>
                <AppBadge v-if="role.isSystem" variant="warning">System</AppBadge>
              </div>
              <p class="text-xs text-gray-400 mt-0.5">{{ role.description }}</p>
            </div>
          </div>
          <div class="flex items-center gap-2">
            <AppButton v-if="auth.hasPermission('identity:roles:write')" variant="ghost" size="sm" @click="openPermissions(role)">Permissions</AppButton>
            <AppButton v-if="auth.hasPermission('identity:roles:write') && !role.isSystem" variant="ghost" size="sm" @click="openEdit(role)">Edit</AppButton>
            <AppButton v-if="auth.hasPermission('identity:roles:write') && !role.isSystem" variant="ghost" size="sm" class="!text-red-500 hover:!bg-red-50" @click="deleteRole(role)">Delete</AppButton>
          </div>
        </div>

        <div class="mt-4 flex flex-wrap gap-1.5">
          <span v-for="perm in role.permissions" :key="perm.code" class="px-2 py-0.5 text-xs font-mono bg-gray-100 text-gray-600 rounded">
            {{ perm.code }}
          </span>
          <span v-if="!role.permissions.length" class="text-xs text-gray-400">No permissions assigned</span>
        </div>
      </div>
    </div>

    <!-- Create/Edit modal -->
    <AppModal :open="formModal" :title="isEditing ? 'Edit Role' : 'Create Role'" @close="formModal = false">
      <div class="space-y-4">
        <AppInput label="Name" v-model="form.name" placeholder="e.g. Manager" />
        <AppInput label="Description" v-model="form.description" placeholder="Brief description of this role" />
      </div>
      <template #footer>
        <AppButton variant="secondary" @click="formModal = false">Cancel</AppButton>
        <AppButton :loading="formLoading" @click="saveForm">{{ isEditing ? 'Save changes' : 'Create role' }}</AppButton>
      </template>
    </AppModal>

    <!-- Permissions modal -->
    <AppModal :open="permModal" :title="`Permissions — ${permTarget?.name}`" size="lg" @close="permModal = false">
      <div v-for="(perms, module) in permissionsByModule" :key="module" class="mb-5">
        <p class="text-xs font-semibold text-gray-400 uppercase tracking-wider mb-2">{{ module }}</p>
        <div class="space-y-1.5">
          <label
            v-for="perm in perms" :key="perm.code"
            class="flex items-center gap-3 p-3 rounded-lg border cursor-pointer transition-colors"
            :class="selectedPerms.includes(perm.code) ? 'border-indigo-400 bg-indigo-50' : 'border-gray-200 hover:border-indigo-200'"
          >
            <input type="checkbox" :value="perm.code" v-model="selectedPerms" class="h-4 w-4 text-indigo-600 rounded" />
            <div class="flex-1 min-w-0">
              <p class="text-sm font-medium text-gray-800">{{ perm.name }}</p>
              <p class="text-xs font-mono text-gray-400">{{ perm.code }}</p>
            </div>
          </label>
        </div>
      </div>
      <template #footer>
        <AppButton variant="secondary" @click="permModal = false">Cancel</AppButton>
        <AppButton :loading="permLoading" @click="savePermissions">Save permissions</AppButton>
      </template>
    </AppModal>
  </div>
</template>
