<script setup lang="ts">
import { useConfirm } from '@/composables/useConfirm'
import AppButton from './AppButton.vue'
import AppModal from './AppModal.vue'

const { visible, options, handleConfirm, handleCancel } = useConfirm()
</script>

<template>
  <AppModal :open="visible" :title="options.title" size="sm" @close="handleCancel">
    <p class="text-sm text-gray-600">{{ options.message }}</p>
    <template #footer>
      <AppButton variant="secondary" @click="handleCancel">
        {{ options.cancelText ?? 'Cancel' }}
      </AppButton>
      <AppButton :variant="options.danger ? 'danger' : 'primary'" @click="handleConfirm">
        {{ options.confirmText ?? 'Confirm' }}
      </AppButton>
    </template>
  </AppModal>
</template>
