<script setup lang="ts">
import type { CourseDto, PagedResult } from '~/types/courses.types'

useSeoMeta({
  title: 'Courses — Peng',
  description: 'Browse all available courses',
  ogTitle: 'Courses — Peng',
})

const page = ref(1)

const { data, pending } = await useApiFetch<PagedResult<CourseDto>>('/courses', {
  query: { page, pageSize: 12 },
  watch: [page],
})

function formatPrice(price: number) {
  return price === 0 ? 'Free' : `$${price.toFixed(2)}`
}
</script>

<template>
  <div class="max-w-6xl mx-auto px-6 py-12">
    <div class="mb-8">
      <h1 class="text-3xl font-bold text-gray-900">Courses</h1>
      <p class="text-gray-500 mt-2">{{ data?.totalCount ?? 0 }} courses available</p>
    </div>

    <div v-if="pending" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div v-for="i in 6" :key="i" class="bg-gray-100 animate-pulse rounded-2xl h-56" />
    </div>

    <div v-else-if="data?.items?.length" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <NuxtLink
        v-for="course in data.items"
        :key="course.id"
        :to="`/courses/${course.id}`"
        class="group bg-white border border-gray-200 rounded-2xl overflow-hidden hover:shadow-md transition-shadow"
      >
        <div class="aspect-video bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center">
          <img v-if="course.thumbnailUrl" :src="course.thumbnailUrl" :alt="course.title" class="w-full h-full object-cover" />
          <span v-else class="text-4xl">📚</span>
        </div>
        <div class="p-5">
          <h2 class="font-semibold text-gray-900 group-hover:text-blue-600 transition-colors line-clamp-2">{{ course.title }}</h2>
          <p class="text-sm text-gray-500 mt-1.5 line-clamp-2">{{ course.description }}</p>
          <div class="flex items-center justify-between mt-4">
            <span class="text-sm font-bold text-blue-600">{{ formatPrice(course.price) }}</span>
            <span class="text-xs text-gray-400">{{ course.enrollmentCount }} enrolled</span>
          </div>
        </div>
      </NuxtLink>
    </div>

    <div v-else class="text-center py-20 text-gray-400">
      <p class="text-lg">No courses available yet</p>
    </div>

    <!-- Pagination -->
    <div v-if="data && data.totalPages > 1" class="flex justify-center gap-2 mt-10">
      <button
        v-for="p in data.totalPages"
        :key="p"
        class="w-9 h-9 rounded-lg text-sm font-medium transition-colors"
        :class="p === page ? 'bg-blue-600 text-white' : 'bg-white border border-gray-200 text-gray-600 hover:bg-gray-50'"
        @click="page = p"
      >
        {{ p }}
      </button>
    </div>
  </div>
</template>
