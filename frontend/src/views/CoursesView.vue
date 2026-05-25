<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { coursesApi } from '@/api/courses.api'
import type { CourseDto, EnrollmentDto } from '@/types/courses.types'
import { useToast } from '@/composables/useToast'
import { useConfirm } from '@/composables/useConfirm'
import { useAuthStore } from '@/stores/auth.store'
import AppButton from '@/components/ui/AppButton.vue'
import AppModal from '@/components/ui/AppModal.vue'
import AppInput from '@/components/ui/AppInput.vue'
import AppBadge from '@/components/ui/AppBadge.vue'

const toast = useToast()
const { confirm } = useConfirm()
const auth = useAuthStore()

const courses = ref<CourseDto[]>([])
const loading = ref(false)
const totalCount = ref(0)

// Create/Edit modal
const formModal = ref(false)
const isEditing = ref(false)
const editTarget = ref<CourseDto | null>(null)
const form = ref({ title: '', description: '', price: 0, thumbnailUrl: '' })
const formLoading = ref(false)
const formErrors = ref<Record<string, string>>({})

// Enrollments modal
const enrollModal = ref(false)
const enrollTarget = ref<CourseDto | null>(null)
const enrollments = ref<EnrollmentDto[]>([])
const enrollLoading = ref(false)

async function fetchCourses() {
  loading.value = true
  try {
    const result = await coursesApi.getAll()
    courses.value = result.items
    totalCount.value = result.totalCount
  } catch {
    toast.error('Failed to load courses')
  } finally {
    loading.value = false
  }
}

onMounted(fetchCourses)

function openCreate() {
  isEditing.value = false
  editTarget.value = null
  form.value = { title: '', description: '', price: 0, thumbnailUrl: '' }
  formErrors.value = {}
  formModal.value = true
}

function openEdit(course: CourseDto) {
  isEditing.value = true
  editTarget.value = course
  form.value = { title: course.title, description: course.description, price: course.price, thumbnailUrl: course.thumbnailUrl ?? '' }
  formErrors.value = {}
  formModal.value = true
}

async function submitForm() {
  formLoading.value = true
  formErrors.value = {}
  try {
    const payload = {
      title: form.value.title,
      description: form.value.description,
      price: Number(form.value.price),
      thumbnailUrl: form.value.thumbnailUrl || undefined,
    }
    if (isEditing.value && editTarget.value) {
      const updated = await coursesApi.update(editTarget.value.id, payload)
      const idx = courses.value.findIndex(c => c.id === editTarget.value!.id)
      if (idx !== -1) courses.value[idx] = updated
      toast.success('Course updated')
    } else {
      const created = await coursesApi.create(payload)
      courses.value.unshift(created)
      toast.success('Course created')
    }
    formModal.value = false
  } catch (e: unknown) {
    const err = e as { response?: { data?: { details?: Record<string, string[]>; message?: string } } }
    const detail = err?.response?.data
    if (detail?.details) {
      formErrors.value = Object.fromEntries(
        Object.entries(detail.details).map(([k, v]) => [k.toLowerCase(), v.join(', ')])
      )
    } else {
      toast.error(detail?.message ?? 'Save failed')
    }
  } finally {
    formLoading.value = false
  }
}

async function togglePublish(course: CourseDto) {
  try {
    const updated = await coursesApi.togglePublish(course.id)
    const idx = courses.value.findIndex(c => c.id === course.id)
    if (idx !== -1) courses.value[idx] = updated
    toast.success(updated.isPublished ? 'Course published' : 'Course unpublished')
  } catch {
    toast.error('Failed to update publish state')
  }
}

async function deleteCourse(course: CourseDto) {
  const ok = await confirm({
    title: 'Delete Course',
    message: `Delete "${course.title}"? This cannot be undone.`,
    confirmText: 'Delete',
    danger: true,
  })
  if (!ok) return
  try {
    await coursesApi.remove(course.id)
    courses.value = courses.value.filter(c => c.id !== course.id)
    toast.success('Course deleted')
  } catch {
    toast.error('Failed to delete course')
  }
}

async function openEnrollments(course: CourseDto) {
  enrollTarget.value = course
  enrollments.value = []
  enrollModal.value = true
  enrollLoading.value = true
  try {
    enrollments.value = await coursesApi.getEnrollments(course.id)
  } catch {
    toast.error('Failed to load enrollments')
  } finally {
    enrollLoading.value = false
  }
}

function formatPrice(price: number) {
  return price === 0 ? 'Free' : `$${price.toFixed(2)}`
}

function formatDate(d: string) {
  return new Date(d).toLocaleDateString()
}
</script>

<template>
  <div>
    <!-- Header -->
    <div class="flex items-center justify-between mb-6">
      <div>
        <h1 class="text-xl font-bold text-gray-900">Courses</h1>
        <p class="text-sm text-gray-500 mt-0.5">{{ totalCount }} total courses</p>
      </div>
      <AppButton v-if="auth.hasPermission('courses:courses:write')" @click="openCreate">
        <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15"/>
        </svg>
        New Course
      </AppButton>
    </div>

    <!-- Table -->
    <div class="bg-white rounded-xl border border-gray-200 overflow-hidden">
      <div v-if="loading" class="p-12 text-center text-gray-400 text-sm">Loading...</div>
      <table v-else class="w-full text-sm">
        <thead class="bg-gray-50 border-b border-gray-200">
          <tr>
            <th class="text-left px-4 py-3 text-xs font-medium text-gray-500 uppercase tracking-wide">Course</th>
            <th class="text-left px-4 py-3 text-xs font-medium text-gray-500 uppercase tracking-wide">Price</th>
            <th class="text-left px-4 py-3 text-xs font-medium text-gray-500 uppercase tracking-wide">Status</th>
            <th class="text-left px-4 py-3 text-xs font-medium text-gray-500 uppercase tracking-wide">Enrollments</th>
            <th class="text-left px-4 py-3 text-xs font-medium text-gray-500 uppercase tracking-wide">Created</th>
            <th class="px-4 py-3" />
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100">
          <tr v-for="course in courses" :key="course.id" class="hover:bg-gray-50 transition-colors">
            <td class="px-4 py-3">
              <div class="font-medium text-gray-900">{{ course.title }}</div>
              <div class="text-xs text-gray-400 mt-0.5 line-clamp-1">{{ course.description }}</div>
            </td>
            <td class="px-4 py-3 text-gray-700">{{ formatPrice(course.price) }}</td>
            <td class="px-4 py-3">
              <AppBadge :variant="course.isPublished ? 'success' : undefined">
                {{ course.isPublished ? 'Published' : 'Draft' }}
              </AppBadge>
            </td>
            <td class="px-4 py-3 text-gray-600">{{ course.enrollmentCount }}</td>
            <td class="px-4 py-3 text-gray-500">{{ formatDate(course.createdAt) }}</td>
            <td class="px-4 py-3">
              <div class="flex items-center justify-end gap-2">
                <AppButton v-if="auth.hasPermission('courses:enrollments:read')" size="sm" variant="ghost" @click="openEnrollments(course)">
                  Enrollments
                </AppButton>
                <AppButton v-if="auth.hasPermission('courses:courses:write')" size="sm" variant="ghost" @click="openEdit(course)">
                  Edit
                </AppButton>
                <AppButton
                  v-if="auth.hasPermission('courses:courses:write')"
                  size="sm"
                  variant="ghost"
                  @click="togglePublish(course)"
                >
                  {{ course.isPublished ? 'Unpublish' : 'Publish' }}
                </AppButton>
                <AppButton v-if="auth.hasPermission('courses:courses:delete')" size="sm" variant="ghost" class="!text-red-500 hover:!bg-red-50" @click="deleteCourse(course)">
                  Delete
                </AppButton>
              </div>
            </td>
          </tr>
          <tr v-if="courses.length === 0">
            <td colspan="6" class="px-4 py-12 text-center text-gray-400">No courses yet</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Create/Edit Modal -->
    <AppModal :open="formModal" :title="isEditing ? 'Edit Course' : 'New Course'" size="lg" @close="formModal = false">
      <form class="space-y-4" @submit.prevent="submitForm">
        <AppInput v-model="form.title" label="Title" :error="formErrors.title" />
        <div class="flex flex-col gap-1.5">
          <label class="text-sm font-medium text-gray-700">Description</label>
          <textarea
            v-model="form.description"
            rows="4"
            class="w-full px-3 py-2 text-sm rounded-lg border border-gray-300 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none transition-colors resize-none"
          />
          <p v-if="formErrors.description" class="text-xs text-red-600">{{ formErrors.description }}</p>
        </div>
        <div class="grid grid-cols-2 gap-4">
          <div class="flex flex-col gap-1.5">
            <label class="text-sm font-medium text-gray-700">Price (0 = Free)</label>
            <input
              v-model.number="form.price"
              type="number"
              min="0"
              step="0.01"
              class="w-full px-3 py-2 text-sm rounded-lg border border-gray-300 focus:border-blue-500 focus:ring-2 focus:ring-blue-100 outline-none transition-colors"
            />
          </div>
          <AppInput v-model="form.thumbnailUrl" label="Thumbnail URL (optional)" />
        </div>
      </form>
      <template #footer>
        <AppButton variant="secondary" @click="formModal = false">Cancel</AppButton>
        <AppButton :loading="formLoading" @click="submitForm">
          {{ isEditing ? 'Save Changes' : 'Create Course' }}
        </AppButton>
      </template>
    </AppModal>

    <!-- Enrollments Modal -->
    <AppModal :open="enrollModal" :title="`Enrollments — ${enrollTarget?.title}`" size="xl" @close="enrollModal = false">
      <div v-if="enrollLoading" class="py-8 text-center text-gray-400 text-sm">Loading...</div>
      <div v-else-if="enrollments.length === 0" class="py-8 text-center text-gray-400 text-sm">No enrollments yet</div>
      <table v-else class="w-full text-sm">
        <thead class="bg-gray-50">
          <tr>
            <th class="text-left px-3 py-2 text-xs font-medium text-gray-500 uppercase">User ID</th>
            <th class="text-left px-3 py-2 text-xs font-medium text-gray-500 uppercase">Status</th>
            <th class="text-left px-3 py-2 text-xs font-medium text-gray-500 uppercase">Enrolled</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100">
          <tr v-for="e in enrollments" :key="e.id">
            <td class="px-3 py-2 font-mono text-xs text-gray-600">{{ e.userId }}</td>
            <td class="px-3 py-2">
              <AppBadge :variant="e.status === 'Active' ? 'success' : undefined">{{ e.status }}</AppBadge>
            </td>
            <td class="px-3 py-2 text-gray-500">{{ formatDate(e.enrolledAt) }}</td>
          </tr>
        </tbody>
      </table>
    </AppModal>
  </div>
</template>
