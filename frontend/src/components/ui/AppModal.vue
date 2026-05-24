<script setup lang="ts">
import { computed, onMounted, onUnmounted, watch } from 'vue'
import { useModalStack } from '@/composables/useModalStack'

const props = defineProps<{
  open: boolean
  title: string
  size?: 'sm' | 'md' | 'lg' | 'xl'
}>()
const emit = defineEmits<{ close: [] }>()

const id = Symbol()
const { push, pop, zIndexFor, isTop } = useModalStack()

const zIndex = computed(() => zIndexFor(id))

watch(
  () => props.open,
  (val) => { if (val) push(id); else pop(id) },
  { immediate: true },
)

onUnmounted(() => pop(id))

function onKeydown(e: KeyboardEvent) {
  if (e.key === 'Escape' && props.open && isTop(id)) emit('close')
}
onMounted(() => window.addEventListener('keydown', onKeydown))
onUnmounted(() => window.removeEventListener('keydown', onKeydown))
</script>

<template>
  <Teleport to="body">
    <Transition name="modal">
      <div
        v-if="open"
        class="fixed inset-0 flex items-center justify-center p-4"
        :style="{ zIndex }"
      >
        <div class="absolute inset-0 bg-black/40 backdrop-blur-sm" @click="$emit('close')" />
        <div
          :class="[
            'relative bg-white rounded-2xl shadow-xl flex flex-col max-h-[90vh] w-full',
            size === 'sm' ? 'max-w-sm' :
            size === 'lg' ? 'max-w-2xl' :
            size === 'xl' ? 'max-w-4xl' : 'max-w-lg',
          ]"
        >
          <div class="flex items-center justify-between px-6 py-4 border-b border-gray-200">
            <h2 class="text-base font-semibold text-gray-900">{{ title }}</h2>
            <button @click="$emit('close')" class="text-gray-400 hover:text-gray-600 transition-colors">
              <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"/>
              </svg>
            </button>
          </div>
          <div class="flex-1 overflow-y-auto px-6 py-5">
            <slot />
          </div>
          <div v-if="$slots.footer" class="px-6 py-4 border-t border-gray-200 flex justify-end gap-3">
            <slot name="footer" />
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.modal-enter-active, .modal-leave-active { transition: opacity 0.2s ease; }
.modal-enter-from, .modal-leave-to { opacity: 0; }
.modal-enter-active .relative, .modal-leave-active .relative { transition: transform 0.2s ease; }
.modal-enter-from .relative { transform: scale(0.95); }
</style>
