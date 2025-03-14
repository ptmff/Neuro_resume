import { render } from '@testing-library/vue'
import { createTestingPinia } from '@pinia/testing'
import { createRouter, createWebHistory } from 'vue-router'

export function renderWithStore(component: any, options: any = {}) {
  const pinia = createTestingPinia({ createSpy: vi.fn })
  const router = createRouter({
    history: createWebHistory(),
    routes: [],
  })

  return render(component, {
    global: {
      plugins: [pinia, router],
    },
    ...options,
  })
}
