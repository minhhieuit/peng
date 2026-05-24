<script setup lang="ts">
defineProps<{
  variant?: 'default' | 'success' | 'warning' | 'danger' | 'indigo'
}>()
</script>

<template>
  <span :class="[
    'inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium',
    variant === 'success' ? 'bg-green-100 text-green-700' :
    variant === 'warning' ? 'bg-yellow-100 text-yellow-700' :
    variant === 'danger'  ? 'bg-red-100 text-red-700' :
    variant === 'indigo'  ? 'bg-indigo-100 text-indigo-700' :
                            'bg-gray-100 text-gray-700',
  ]">
    <slot />
  </span>
</template>
