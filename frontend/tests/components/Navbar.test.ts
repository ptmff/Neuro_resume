import { render, screen } from '@testing-library/vue'
import Navbar from '@/components/NavBar.vue'
import { createTestingPinia } from '@pinia/testing'
import { createRouter, createWebHistory } from 'vue-router'
import { describe, it, expect, vi, beforeEach } from 'vitest'
import userEvent from '@testing-library/user-event'
import flushPromises from 'flush-promises'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', name: 'Home', component: { template: '<div>Home</div>' } },
    { path: '/resume', name: 'Resume', component: { template: '<div>Resume</div>' } },
    { path: '/analyse', name: 'Analyse', component: { template: '<div>Analyse</div>' } },
    { path: '/profile', name: 'Profile', component: { template: '<div>Profile</div>' } },
  ],
})

vi.spyOn(router, 'push')

beforeEach(async () => {
  router.push('/')
  await router.isReady()
})

describe('Navbar.vue', () => {
  it('renders all nav links', () => {
    render(Navbar, {
      global: {
        plugins: [createTestingPinia(), router],
      },
    })

    expect(screen.getByText('Главная')).toBeInTheDocument()
    expect(screen.getByText('Создать резюме')).toBeInTheDocument()
    expect(screen.getByText('Анализ вакансий')).toBeInTheDocument()
    expect(screen.getByText('Профиль')).toBeInTheDocument()
  })

  it(
    'navigates to correct route on click',
    async () => {
      render(Navbar, {
        global: {
          plugins: [createTestingPinia(), router],
        },
      })

      await userEvent.click(screen.getByText('Профиль'))
      await flushPromises() // дожидаемся навигации

      expect(router.push).toHaveBeenCalledWith('/profile')
    },
    7000 // увеличим тайм-аут для этого теста
  )
})
