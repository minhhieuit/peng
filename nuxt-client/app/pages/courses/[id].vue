<script setup lang="ts">
import type { CourseDto, EnrollmentDto } from '~/types/courses.types'

const route = useRoute()
const auth = useAuthStore()
const $api = useApiClient()

const { data: course, error } = await useApiFetch<CourseDto>(`/courses/${route.params.id}`)

if (error.value || !course.value) {
  throw createError({ statusCode: 404, message: 'Course not found' })
}

useSeoMeta({
  title: `${course.value.title} — Peng`,
  description: course.value.description,
  ogTitle: course.value.title,
  ogDescription: course.value.description,
  ogImage: course.value.thumbnailUrl ?? undefined,
})

const enrolling = ref(false)
const unenrolling = ref(false)
const enrollment = ref<EnrollmentDto | null>(null)
const enrollError = ref('')

// Check if already enrolled
if (auth.isAuthenticated) {
  try {
    const enrollments = await $api<EnrollmentDto[]>('/courses/my-enrollments')
    enrollment.value = enrollments.find(e => e.courseId === route.params.id && e.status === 'Active') ?? null
  } catch {}
}

async function enroll() {
  if (!auth.isAuthenticated) return navigateTo('/auth/login')
  enrolling.value = true
  enrollError.value = ''
  try {
    enrollment.value = await $api<EnrollmentDto>(`/courses/${route.params.id}/enroll`, { method: 'POST' })
  } catch (e: any) {
    enrollError.value = e?.data?.message ?? 'Enrollment failed'
  } finally {
    enrolling.value = false
  }
}

async function unenroll() {
  unenrolling.value = true
  try {
    await $api(`/courses/${route.params.id}/enroll`, { method: 'DELETE' })
    enrollment.value = null
  } catch (e: any) {
    enrollError.value = e?.data?.message ?? 'Failed to unenroll'
  } finally {
    unenrolling.value = false
  }
}

function formatPrice(price: number) {
  return price === 0 ? 'Free' : `$${price.toFixed(2)}`
}
</script>

<template>
  <div class="max-w-4xl mx-auto px-6 py-12" v-if="course">
    <NuxtLink to="/courses" class="text-sm text-blue-600 hover:underline mb-6 inline-block">← Back to courses</NuxtLink>

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <!-- Main content -->
      <div class="lg:col-span-2">
        <div class="aspect-video bg-gradient-to-br from-blue-50 to-indigo-100 rounded-2xl overflow-hidden mb-6 flex items-center justify-center">
          <img v-if="course.thumbnailUrl" :src="course.thumbnailUrl" :alt="course.title" class="w-full h-full object-cover" />
          <span v-else class="text-6xl">📚</span>
        </div>
        <h1 class="text-3xl font-bold text-gray-900">{{ course.title }}</h1>
        <p class="text-gray-600 mt-4 leading-relaxed whitespace-pre-line">{{ course.description }}</p>
      </div>

      <!-- Sidebar card -->
      <div class="lg:col-span-1">
        <div class="bg-white border border-gray-200 rounded-2xl p-6 sticky top-6">
          <p class="text-3xl font-bold text-blue-600 mb-4">{{ formatPrice(course.price) }}</p>
          <div class="space-y-2 text-sm text-gray-500 mb-6">
            <p>{{ course.enrollmentCount }} students enrolled</p>
          </div>

          <div v-if="enrollment">
            <div class="bg-green-50 text-green-700 text-sm font-medium px-4 py-2.5 rounded-xl text-center mb-3">
              ✓ You're enrolled
            </div>
            <UiButton variant="ghost" class="w-full text-red-500" :loading="unenrolling" @click="unenroll">
              Unenroll
            </UiButton>
          </div>
          <div v-else>
            <UiButton class="w-full" :loading="enrolling" @click="enroll">
              {{ auth.isAuthenticated ? 'Enroll now' : 'Sign in to enroll' }}
            </UiButton>
          </div>

          <p v-if="enrollError" class="text-xs text-red-600 text-center mt-2">{{ enrollError }}</p>
        </div>
      </div>
    </div>
  </div>
</template>
