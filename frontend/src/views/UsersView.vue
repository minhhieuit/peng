<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { usersApi } from '@/api/users.api'
import { rolesApi } from '@/api/roles.api'
import type { UserDto } from '@/types/auth.types'
import type { RoleDto } from '@/types/identity.types'
import type { PagedList } from '@/types/common.types'
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

const data = ref<PagedList<UserDto> | null>(null)
const roles = ref<RoleDto[]>([])
const loading = ref(false)
const search = ref('')
const currentPage = ref(1)

// Edit modal
const editModal = ref(false)
const editTarget = ref<UserDto | null>(null)
const editForm = ref({ firstName: '', lastName: '' })
const editLoading = ref(false)

// Role assignment modal
const roleModal = ref(false)
const roleTarget = ref<UserDto | null>(null)
const selectedRoleIds = ref<string[]>([])
const roleLoading = ref(false)

async function fetchUsers() {
  loading.value = true
  try {
    data.value = await usersApi.getAll({ page: currentPage.value, pageSize: 20, search: search.value || undefined })
  } finally {
    loading.value = false
  }
}

async function fetchRoles() {
  roles.value = await rolesApi.getAll()
}

function onSearch() { currentPage.value = 1; fetchUsers() }

function openEdit(user: UserDto) {
  editTarget.value = user
  editForm.value = { firstName: user.firstName, lastName: user.lastName }
  editModal.value = true
}

async function saveEdit() {
  if (!editTarget.value) return
  editLoading.value = true
  try {
    await usersApi.update(editTarget.value.id, editForm.value)
    toast.success('User updated successfully.')
    editModal.value = false
    fetchUsers()
  } catch {
    toast.error('Failed to update user.')
  } finally {
    editLoading.value = false
  }
}

function openRoleModal(user: UserDto) {
  roleTarget.value = user
  selectedRoleIds.value = user.roles.map(r => roles.value.find(ro => ro.name === r)?.id ?? '').filter(Boolean)
  roleModal.value = true
}

async function saveRoles() {
  if (!roleTarget.value) return
  roleLoading.value = true
  try {
    await usersApi.assignRoles(roleTarget.value.id, selectedRoleIds.value)
    toast.success('Roles updated.')
    roleModal.value = false
    fetchUsers()
  } catch {
    toast.error('Failed to update roles.')
  } finally {
    roleLoading.value = false
  }
}

async function deactivate(user: UserDto) {
  const ok = await confirm({
    title: 'Deactivate User',
    message: `Deactivate "${user.fullName}"? They will no longer be able to login.`,
    confirmText: 'Deactivate',
    danger: true,
  })
  if (!ok) return
  try {
    await usersApi.deactivate(user.id)
    toast.success('User deactivated.')
    fetchUsers()
  } catch {
    toast.error('Failed to deactivate user.')
  }
}

onMounted(() => { fetchUsers(); fetchRoles() })
</script>

<template>
  <div>
    <div class="mb-6 flex items-center justify-between">
      <div>
        <h1 class="text-xl font-bold text-gray-900">Users</h1>
        <p class="text-sm text-gray-500 mt-0.5">Manage system users and their roles</p>
      </div>
    </div>

    <div class="bg-white rounded-xl border border-gray-200">
      <div class="p-4 border-b border-gray-200 flex gap-3">
        <input
          v-model="search" @keyup.enter="onSearch" type="search" placeholder="Search by name or email..."
          class="flex-1 max-w-xs px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
        />
        <AppButton variant="secondary" size="sm" @click="onSearch">Search</AppButton>
      </div>

      <div v-if="loading" class="p-10 text-center text-sm text-gray-400">Loading...</div>

      <table v-else-if="data?.items.length" class="w-full">
        <thead>
          <tr class="border-b border-gray-200 bg-gray-50 text-left text-xs font-medium text-gray-500 uppercase tracking-wide">
            <th class="px-4 py-3">User</th>
            <th class="px-4 py-3">Roles</th>
            <th class="px-4 py-3">Status</th>
            <th class="px-4 py-3">Joined</th>
            <th class="px-4 py-3 text-right">Actions</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100">
          <tr v-for="user in data.items" :key="user.id" class="hover:bg-gray-50 transition-colors">
            <td class="px-4 py-3">
              <div class="flex items-center gap-3">
                <div class="w-8 h-8 rounded-full bg-indigo-100 text-indigo-700 flex items-center justify-center text-xs font-semibold shrink-0">
                  {{ user.firstName.charAt(0) }}{{ user.lastName.charAt(0) }}
                </div>
                <div>
                  <p class="text-sm font-medium text-gray-900">{{ user.fullName }}</p>
                  <p class="text-xs text-gray-400">{{ user.email }}</p>
                </div>
              </div>
            </td>
            <td class="px-4 py-3">
              <div class="flex flex-wrap gap-1">
                <AppBadge v-for="role in user.roles" :key="role" variant="indigo">{{ role }}</AppBadge>
                <span v-if="!user.roles.length" class="text-xs text-gray-400">No roles</span>
              </div>
            </td>
            <td class="px-4 py-3">
              <AppBadge :variant="user.isActive ? 'success' : 'danger'">
                {{ user.isActive ? 'Active' : 'Inactive' }}
              </AppBadge>
            </td>
            <td class="px-4 py-3 text-sm text-gray-500">{{ new Date(user.createdAt).toLocaleDateString() }}</td>
            <td class="px-4 py-3">
              <div class="flex items-center justify-end gap-2">
                <AppButton v-if="auth.hasPermission('identity:users:write')" variant="ghost" size="sm" @click="openEdit(user)">Edit</AppButton>
                <AppButton v-if="auth.hasPermission('identity:users:write')" variant="ghost" size="sm" @click="openRoleModal(user)">Roles</AppButton>
                <AppButton
                  v-if="auth.hasPermission('identity:users:delete') && user.isActive"
                  variant="ghost" size="sm"
                  class="!text-red-500 hover:!bg-red-50"
                  @click="deactivate(user)"
                >Deactivate</AppButton>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <div v-else class="p-10 text-center text-sm text-gray-400">No users found.</div>

      <div v-if="data && data.totalPages > 1" class="p-4 border-t border-gray-200 flex items-center justify-between">
        <p class="text-xs text-gray-400">{{ data.totalCount }} users total</p>
        <div class="flex items-center gap-2">
          <AppButton variant="secondary" size="sm" :disabled="!data.hasPreviousPage" @click="() => { currentPage--; fetchUsers() }">Prev</AppButton>
          <span class="text-xs text-gray-500">{{ currentPage }} / {{ data.totalPages }}</span>
          <AppButton variant="secondary" size="sm" :disabled="!data.hasNextPage" @click="() => { currentPage++; fetchUsers() }">Next</AppButton>
        </div>
      </div>
    </div>

    <!-- Edit modal -->
    <AppModal :open="editModal" title="Edit User" @close="editModal = false">
      <div class="space-y-4">
        <AppInput label="First name" v-model="editForm.firstName" />
        <AppInput label="Last name" v-model="editForm.lastName" />
      </div>
      <template #footer>
        <AppButton variant="secondary" @click="editModal = false">Cancel</AppButton>
        <AppButton :loading="editLoading" @click="saveEdit">Save changes</AppButton>
      </template>
    </AppModal>

    <!-- Role assignment modal -->
    <AppModal :open="roleModal" title="Assign Roles" @close="roleModal = false">
      <p class="text-sm text-gray-500 mb-4">Select roles for <strong>{{ roleTarget?.fullName }}</strong></p>
      <div class="space-y-2">
        <label
          v-for="role in roles" :key="role.id"
          class="flex items-center gap-3 p-3 rounded-lg border cursor-pointer transition-colors"
          :class="selectedRoleIds.includes(role.id) ? 'border-indigo-400 bg-indigo-50' : 'border-gray-200 hover:border-indigo-200'"
        >
          <input type="checkbox" :value="role.id" v-model="selectedRoleIds" class="h-4 w-4 text-indigo-600 rounded" />
          <div>
            <p class="text-sm font-medium text-gray-800">{{ role.name }}</p>
            <p class="text-xs text-gray-400">{{ role.description }}</p>
          </div>
        </label>
      </div>
      <template #footer>
        <AppButton variant="secondary" @click="roleModal = false">Cancel</AppButton>
        <AppButton :loading="roleLoading" @click="saveRoles">Save roles</AppButton>
      </template>
    </AppModal>
  </div>
</template>
