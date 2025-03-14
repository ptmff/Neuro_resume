import { setActivePinia, createPinia } from 'pinia'
import { useProfileStore } from '@/stores/profile'
import { vi, describe, it, expect, beforeEach } from 'vitest'

vi.mock('@/api', () => ({
  default: {
    get: vi.fn()
  }
}))

import api from '@/api'

describe('Profile Store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })

  it('fetchProfile should set profile and allResumes', async () => {
    api.get.mockResolvedValueOnce({ data: { name: 'John', mainResumeId: 1, resumes: [1, 2] } })
    api.get.mockResolvedValueOnce({ data: [{ id: 1, title: 'Frontend' }, { id: 2, title: 'Backend' }] })

    const store = useProfileStore()
    await store.fetchProfile()

    expect(store.profile).toEqual({ name: 'John', mainResumeId: 1, resumes: [1, 2] })
    expect(store.allResumes).toHaveLength(2)
  })
})
