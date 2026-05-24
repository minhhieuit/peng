<script setup lang="ts">
defineProps<{
  options: { value: string; label: string; description?: string }[]
  modelValue: string[]
  label?: string
}>()
const emit = defineEmits<{ 'update:modelValue': [v: string[]] }>()

function toggle(value: string, current: string[]) {
  const next = current.includes(value) ? current.filter(v => v !== value) : [...current, value]
  emit('update:modelValue', next)
}
</script>

<template>
  <div>
    <p v-if="label" class="text-sm font-medium text-gray-700 mb-2">{{ label }}</p>
    <div class="space-y-2">
      <label
        v-for="opt in options"
        :key="opt.value"
        class="flex items-start gap-3 p-3 rounded-lg border border-gray-200 hover:border-indigo-300 hover:bg-indigo-50 cursor-pointer transition-colors"
        :class="modelValue.includes(opt.value) ? 'border-indigo-400 bg-indigo-50' : ''"
      >
        <input
          type="checkbox"
          :value="opt.value"
          :checked="modelValue.includes(opt.value)"
          @change="toggle(opt.value, modelValue)"
          class="mt-0.5 h-4 w-4 text-indigo-600 rounded border-gray-300"
        />
        <div>
          <p class="text-sm font-medium text-gray-800">{{ opt.label }}</p>
          <p v-if="opt.description" class="text-xs text-gray-500">{{ opt.description }}</p>
        </div>
      </label>
    </div>
  </div>
</template>
