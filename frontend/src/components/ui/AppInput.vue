<script setup lang="ts">
defineProps<{
  label?: string
  error?: string
  hint?: string
  modelValue?: string
}>()
defineEmits<{ 'update:modelValue': [value: string] }>()
</script>

<template>
  <div class="flex flex-col gap-1">
    <label v-if="label" class="text-sm font-medium text-gray-700">{{ label }}</label>
    <input
      v-bind="$attrs"
      :value="modelValue"
      @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
      :class="[
        'w-full px-3 py-2 text-sm border rounded-lg outline-none transition-colors',
        error
          ? 'border-red-400 focus:ring-2 focus:ring-red-300'
          : 'border-gray-300 focus:ring-2 focus:ring-indigo-500 focus:border-transparent',
      ]"
    />
    <p v-if="error" class="text-xs text-red-500">{{ error }}</p>
    <p v-else-if="hint" class="text-xs text-gray-400">{{ hint }}</p>
  </div>
</template>
