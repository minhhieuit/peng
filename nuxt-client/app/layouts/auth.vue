<template>
  <div class="min-h-screen bg-gray-50 flex items-center justify-center p-4">
    <div class="w-full max-w-sm">
      <div class="text-center mb-8">
        <h1 class="text-2xl font-bold text-gray-900">Peng</h1>
        <p class="text-sm text-gray-500 mt-1">Base System</p>
      </div>
      <div class="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
        <slot />
      </div>
    </div>
  </div>
</template>
