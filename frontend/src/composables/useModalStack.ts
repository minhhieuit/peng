import { reactive } from 'vue'

const stack = reactive<symbol[]>([])
const BASE_Z = 50

function push(id: symbol) {
  if (!stack.includes(id)) stack.push(id)
}

function pop(id: symbol) {
  const idx = stack.indexOf(id)
  if (idx !== -1) stack.splice(idx, 1)
}

function zIndexFor(id: symbol): number {
  const pos = stack.indexOf(id)
  return pos === -1 ? BASE_Z : BASE_Z + pos * 10
}

function isTop(id: symbol): boolean {
  return stack.length > 0 && stack[stack.length - 1] === id
}

export function useModalStack() {
  return { push, pop, zIndexFor, isTop }
}
