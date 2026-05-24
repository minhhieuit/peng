import { ref } from 'vue'

interface ConfirmOptions {
  title: string
  message: string
  confirmText?: string
  cancelText?: string
  danger?: boolean
}

const visible = ref(false)
const options = ref<ConfirmOptions>({ title: '', message: '' })
let resolveFn: ((value: boolean) => void) | null = null

export function useConfirm() {
  function confirm(opts: ConfirmOptions): Promise<boolean> {
    options.value = opts
    visible.value = true
    return new Promise(resolve => {
      resolveFn = resolve
    })
  }

  function handleConfirm() {
    visible.value = false
    resolveFn?.(true)
  }

  function handleCancel() {
    visible.value = false
    resolveFn?.(false)
  }

  return { visible, options, confirm, handleConfirm, handleCancel }
}
