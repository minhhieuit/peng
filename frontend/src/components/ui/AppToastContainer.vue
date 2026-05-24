<script setup lang="ts">
import { useToastStore } from '@/stores/toast.store'
const toast = useToastStore()
</script>

<template>
  <Teleport to="body">
    <div class="fixed top-4 right-4 z-[100] flex flex-col gap-2 w-80">
      <TransitionGroup name="toast">
        <div
          v-for="t in toast.toasts"
          :key="t.id"
          :class="[
            'flex items-start gap-3 px-4 py-3 rounded-xl shadow-lg text-sm font-medium cursor-pointer',
            t.type === 'success' ? 'bg-green-600 text-white' :
            t.type === 'error'   ? 'bg-red-600 text-white' :
            t.type === 'warning' ? 'bg-yellow-500 text-white' :
                                   'bg-gray-800 text-white',
          ]"
          @click="toast.remove(t.id)"
        >
          <span class="flex-1">{{ t.message }}</span>
          <svg class="w-4 h-4 shrink-0 mt-0.5 opacity-70" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
          </svg>
        </div>
      </TransitionGroup>
    </div>
  </Teleport>
</template>

<style scoped>
.toast-enter-active { transition: all 0.25s ease; }
.toast-leave-active { transition: all 0.2s ease; }
.toast-enter-from  { opacity: 0; transform: translateX(100%); }
.toast-leave-to    { opacity: 0; transform: translateX(100%); }
</style>
