<script setup lang="ts">
import type { EnrollmentDto } from '~/types/courses.types'

definePageMeta({ layout: 'dashboard', middleware: 'auth', title: 'My Courses' })

useSeoMeta({ title: 'My Courses — Peng', robots: 'noindex' })

const $api = useApiClient()
const enrollments = ref<EnrollmentDto[]>([])
const loading = ref(false)

try {
  loading.value = true
  enrollments.value = await $api<EnrollmentDto[]>('/courses/my-enrollments')
} catch {
  enrollments.value = []
} finally {
  loading.value = false
}

const active = computed(() => enrollments.value.filter(e => e.status === 'Active'))
const cancelled = computed(() => enrollments.value.filter(e => e.status === 'Cancelled'))
</script>

<template>
  <div class="space-y-6">
    <div>
      <h2 class="text-2xl font-bold text-gray-900">My Courses</h2>
      <p class="text-gray-500 mt-1">{{ active.length }} active enrollment{{ active.length !== 1 ? 's' : '' }}</p>
    </div>

    <div v-if="loading" class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div v-for="i in 4" :key="i" class="bg-gray-100 animate-pulse rounded-xl h-24" />
    </div>

    <div v-else-if="active.length" class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <NuxtLink
        v-for="e in active"
        :key="e.id"
        :to="`/courses/${e.courseId}`"
        class="bg-white border border-gray-200 rounded-xl p-4 hover:shadow-sm transition-shadow"
      >
        <p class="font-medium text-gray-900">{{ e.courseTitle }}</p>
        <p class="text-xs text-gray-400 mt-1">Enrolled {{ new Date(e.enrolledAt).toLocaleDateString() }}</p>
      </NuxtLink>
    </div>

    <div v-else class="bg-white rounded-xl border border-gray-200 p-12 text-center">
      <p class="text-gray-400 mb-4">You haven't enrolled in any courses yet</p>
      <NuxtLink to="/courses" class="text-blue-600 hover:underline text-sm font-medium">Browse courses →</NuxtLink>
    </div>
  </div>
</template>
