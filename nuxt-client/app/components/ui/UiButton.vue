<script setup lang="ts">
defineProps<{
  variant?: 'primary' | 'secondary' | 'ghost' | 'danger'
  size?: 'sm' | 'md' | 'lg'
  loading?: boolean
  disabled?: boolean
  type?: 'button' | 'submit' | 'reset'
}>()
</script>

<template>
  <button
    :type="type ?? 'button'"
    :disabled="disabled || loading"
    :class="[
      'inline-flex items-center justify-center font-medium rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed',
      variant === 'secondary' ? 'bg-gray-100 hover:bg-gray-200 text-gray-800' :
      variant === 'ghost' ? 'hover:bg-gray-100 text-gray-700' :
      variant === 'danger' ? 'bg-red-600 hover:bg-red-700 text-white' :
      'bg-blue-600 hover:bg-blue-700 text-white',
      size === 'sm' ? 'text-xs px-3 py-1.5 gap-1.5' :
      size === 'lg' ? 'text-base px-6 py-3 gap-2.5' :
      'text-sm px-4 py-2 gap-2',
    ]"
  >
    <svg v-if="loading" class="animate-spin w-4 h-4" fill="none" viewBox="0 0 24 24">
      <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
      <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
    </svg>
    <slot />
  </button>
</template>
