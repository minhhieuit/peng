<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { membersApi } from '@/api/members.api'
import type { MemberDto, CreateMemberResponse } from '@/types/member.types'
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

const data = ref<PagedList<MemberDto> | null>(null)
const loading = ref(false)
const search = ref('')
const currentPage = ref(1)

// Create modal
const createModal = ref(false)
const createForm = ref({ email: '', firstName: '', lastName: '' })
const createLoading = ref(false)

// Result modal (shows the generated temporary password once)
const resultModal = ref(false)
const created = ref<CreateMemberResponse | null>(null)

// Edit modal
const editModal = ref(false)
const editTarget = ref<MemberDto | null>(null)
const editForm = ref({ firstName: '', lastName: '' })
const editLoading = ref(false)

// Change password modal
const pwModal = ref(false)
const pwTarget = ref<MemberDto | null>(null)
const pwForm = ref({ newPassword: '', confirmPassword: '' })
const pwLoading = ref(false)

async function fetchMembers() {
  loading.value = true
  try {
    data.value = await membersApi.getAll({ page: currentPage.value, pageSize: 20, search: search.value || undefined })
  } finally {
    loading.value = false
  }
}

function onSearch() { currentPage.value = 1; fetchMembers() }

function openCreate() {
  createForm.value = { email: '', firstName: '', lastName: '' }
  createModal.value = true
}

async function saveCreate() {
  createLoading.value = true
  try {
    const res = await membersApi.create(createForm.value)
    createModal.value = false
    created.value = res
    resultModal.value = true
    toast.success('Member created.')
    fetchMembers()
  } catch {
    toast.error('Failed to create member. The email may already be in use.')
  } finally {
    createLoading.value = false
  }
}

async function copyPassword() {
  if (!created.value) return
  await navigator.clipboard.writeText(created.value.temporaryPassword)
  toast.success('Temporary password copied.')
}

function openEdit(member: MemberDto) {
  editTarget.value = member
  editForm.value = { firstName: member.firstName, lastName: member.lastName }
  editModal.value = true
}

async function saveEdit() {
  if (!editTarget.value) return
  editLoading.value = true
  try {
    await membersApi.update(editTarget.value.id, editForm.value)
    toast.success('Member updated.')
    editModal.value = false
    fetchMembers()
  } catch {
    toast.error('Failed to update member.')
  } finally {
    editLoading.value = false
  }
}

function openPassword(member: MemberDto) {
  pwTarget.value = member
  pwForm.value = { newPassword: '', confirmPassword: '' }
  pwModal.value = true
}

async function savePassword() {
  if (!pwTarget.value) return
  if (pwForm.value.newPassword.length < 8) {
    toast.error('Password must be at least 8 characters.')
    return
  }
  if (pwForm.value.newPassword !== pwForm.value.confirmPassword) {
    toast.error('Passwords do not match.')
    return
  }
  pwLoading.value = true
  try {
    await membersApi.changePassword(pwTarget.value.id, pwForm.value.newPassword)
    toast.success('Password changed.')
    pwModal.value = false
  } catch {
    toast.error('Failed to change password.')
  } finally {
    pwLoading.value = false
  }
}

async function deactivate(member: MemberDto) {
  const ok = await confirm({
    title: 'Deactivate Member',
    message: `Deactivate "${member.fullName}"? They will no longer be able to login.`,
    confirmText: 'Deactivate',
    danger: true,
  })
  if (!ok) return
  try {
    await membersApi.deactivate(member.id)
    toast.success('Member deactivated.')
    fetchMembers()
  } catch {
    toast.error('Failed to deactivate member.')
  }
}

onMounted(() => { fetchMembers() })
</script>

<template>
  <div>
    <div class="mb-6 flex items-center justify-between">
      <div>
        <h1 class="text-xl font-bold text-gray-900">Members</h1>
        <p class="text-sm text-gray-500 mt-0.5">Members who registered through the client, plus manually created accounts</p>
      </div>
      <AppButton v-if="auth.hasPermission('members:members:write')" @click="openCreate">Create member</AppButton>
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
            <th class="px-4 py-3">Member</th>
            <th class="px-4 py-3">Status</th>
            <th class="px-4 py-3">Last login</th>
            <th class="px-4 py-3">Joined</th>
            <th class="px-4 py-3 text-right">Actions</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100">
          <tr v-for="member in data.items" :key="member.id" class="hover:bg-gray-50 transition-colors">
            <td class="px-4 py-3">
              <div class="flex items-center gap-3">
                <div class="w-8 h-8 rounded-full bg-emerald-100 text-emerald-700 flex items-center justify-center text-xs font-semibold shrink-0">
                  {{ member.firstName.charAt(0) }}{{ member.lastName.charAt(0) }}
                </div>
                <div>
                  <p class="text-sm font-medium text-gray-900">{{ member.fullName }}</p>
                  <p class="text-xs text-gray-400">{{ member.email }}</p>
                </div>
              </div>
            </td>
            <td class="px-4 py-3">
              <div class="flex flex-wrap gap-1">
                <AppBadge :variant="member.isActive ? 'success' : 'danger'">
                  {{ member.isActive ? 'Active' : 'Inactive' }}
                </AppBadge>
                <AppBadge v-if="member.mustChangePassword" variant="indigo">Temp password</AppBadge>
              </div>
            </td>
            <td class="px-4 py-3 text-sm text-gray-500">
              {{ member.lastLoginAt ? new Date(member.lastLoginAt).toLocaleDateString() : '—' }}
            </td>
            <td class="px-4 py-3 text-sm text-gray-500">{{ new Date(member.createdAt).toLocaleDateString() }}</td>
            <td class="px-4 py-3">
              <div class="flex items-center justify-end gap-2">
                <AppButton v-if="auth.hasPermission('members:members:write')" variant="ghost" size="sm" @click="openEdit(member)">Edit</AppButton>
                <AppButton v-if="auth.hasPermission('members:members:write')" variant="ghost" size="sm" @click="openPassword(member)">Password</AppButton>
                <AppButton
                  v-if="auth.hasPermission('members:members:write') && member.isActive"
                  variant="ghost" size="sm"
                  class="!text-red-500 hover:!bg-red-50"
                  @click="deactivate(member)"
                >Deactivate</AppButton>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <div v-else class="p-10 text-center text-sm text-gray-400">No members found.</div>

      <div v-if="data && data.totalPages > 1" class="p-4 border-t border-gray-200 flex items-center justify-between">
        <p class="text-xs text-gray-400">{{ data.totalCount }} members total</p>
        <div class="flex items-center gap-2">
          <AppButton variant="secondary" size="sm" :disabled="!data.hasPreviousPage" @click="() => { currentPage--; fetchMembers() }">Prev</AppButton>
          <span class="text-xs text-gray-500">{{ currentPage }} / {{ data.totalPages }}</span>
          <AppButton variant="secondary" size="sm" :disabled="!data.hasNextPage" @click="() => { currentPage++; fetchMembers() }">Next</AppButton>
        </div>
      </div>
    </div>

    <!-- Create modal -->
    <AppModal :open="createModal" title="Create Member" @close="createModal = false">
      <div class="space-y-4">
        <AppInput label="Email" type="email" v-model="createForm.email" />
        <AppInput label="First name" v-model="createForm.firstName" />
        <AppInput label="Last name" v-model="createForm.lastName" />
        <p class="text-xs text-gray-400">A temporary password will be generated and shown once. The member must change it on first login.</p>
      </div>
      <template #footer>
        <AppButton variant="secondary" @click="createModal = false">Cancel</AppButton>
        <AppButton :loading="createLoading" @click="saveCreate">Create</AppButton>
      </template>
    </AppModal>

    <!-- Edit modal -->
    <AppModal :open="editModal" title="Edit Member" @close="editModal = false">
      <div class="space-y-4">
        <AppInput label="First name" v-model="editForm.firstName" />
        <AppInput label="Last name" v-model="editForm.lastName" />
      </div>
      <template #footer>
        <AppButton variant="secondary" @click="editModal = false">Cancel</AppButton>
        <AppButton :loading="editLoading" @click="saveEdit">Save changes</AppButton>
      </template>
    </AppModal>

    <!-- Change password modal -->
    <AppModal :open="pwModal" title="Change Password" @close="pwModal = false">
      <div class="space-y-4">
        <p class="text-sm text-gray-500">Set a new password for <strong>{{ pwTarget?.fullName }}</strong>.</p>
        <AppInput label="New password" type="password" v-model="pwForm.newPassword" />
        <AppInput label="Confirm password" type="password" v-model="pwForm.confirmPassword" />
      </div>
      <template #footer>
        <AppButton variant="secondary" @click="pwModal = false">Cancel</AppButton>
        <AppButton :loading="pwLoading" @click="savePassword">Change password</AppButton>
      </template>
    </AppModal>

    <!-- Temporary password result modal -->
    <AppModal :open="resultModal" title="Member created" @close="resultModal = false">
      <div class="space-y-4">
        <p class="text-sm text-gray-600">
          Account created for <strong>{{ created?.fullName }}</strong> ({{ created?.email }}).
          Share this temporary password securely — it will not be shown again.
        </p>
        <div class="flex items-center gap-2">
          <code class="flex-1 px-3 py-2 bg-gray-100 rounded-lg text-sm font-mono select-all">{{ created?.temporaryPassword }}</code>
          <AppButton variant="secondary" size="sm" @click="copyPassword">Copy</AppButton>
        </div>
      </div>
      <template #footer>
        <AppButton @click="resultModal = false">Done</AppButton>
      </template>
    </AppModal>
  </div>
</template>
