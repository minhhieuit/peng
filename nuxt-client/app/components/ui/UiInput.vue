<script setup lang="ts">
defineProps<{
  label?: string
  error?: string
  hint?: string
  type?: string
}>()
const model = defineModel<string>()
</script>

<template>
  <div class="flex flex-col gap-1.5">
    <label v-if="label" class="text-sm font-medium text-gray-700">{{ label }}</label>
    <input
      v-model="model"
      :type="type ?? 'text'"
      :class="[
        'w-full px-3 py-2 text-sm rounded-lg border outline-none transition-colors',
        error
          ? 'border-red-300 bg-red-50 focus:ring-2 focus:ring-red-200'
          : 'border-gray-300 focus:border-blue-500 focus:ring-2 focus:ring-blue-100',
      ]"
    />
    <p v-if="error" class="text-xs text-red-600">{{ error }}</p>
    <p v-else-if="hint" class="text-xs text-gray-500">{{ hint }}</p>
  </div>
</template>
