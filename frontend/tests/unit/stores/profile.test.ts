import { setActivePinia, createPinia } from 'pinia'
import { useProfileStore } from '@/stores/profile'
import { vi, describe, it, expect, beforeEach } from 'vitest'
import type { Profile } from '@/types/profile'
import api from '@/api'

// Типизация mock'а API
vi.mock('@/api', () => {
  const get = vi.fn()
  const patch = vi.fn()

  return {
    __esModule: true,
    default: {
      get,
      patch,
    },
  }
})

// Приводим api к типу с моками
const mockedApi = api as unknown as {
  get: ReturnType<typeof vi.fn>
  patch: ReturnType<typeof vi.fn>
}

describe('Profile Store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })

  it('fetchProfile should set profile and allResumes', async () => {
    mockedApi.get.mockResolvedValueOnce({
      data: { name: 'John', mainResumeId: 1, resumes: [1, 2] },
    })
    mockedApi.get.mockResolvedValueOnce({
      data: [
        { id: 1, title: 'Frontend' },
        { id: 2, title: 'Backend' },
      ],
    })

    const store = useProfileStore()
    await store.fetchProfile()

    expect(store.profile).toEqual({
      name: 'John',
      mainResumeId: 1,
      resumes: [1, 2],
    })
    expect(store.allResumes).toHaveLength(2)
  })

  it('fetchProfile should handle API error gracefully', async () => {
    mockedApi.get.mockRejectedValueOnce(new Error('Profile fetch failed'))
    mockedApi.get.mockRejectedValueOnce(new Error('Resumes fetch failed'))

    const store = useProfileStore()
    store.profile = { name: 'Old User' } as any
    store.allResumes = [{ id: 123, title: 'Old Resume' }] as any

    const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {})

    await store.fetchProfile()

    expect(store.profile).toEqual({ name: 'Old User' })
    expect(store.allResumes).toEqual([{ id: 123, title: 'Old Resume' }])
    expect(consoleSpy).toHaveBeenCalled()

    consoleSpy.mockRestore()
  })

  it('setMainResume should update mainResumeId', async () => {
    const store = useProfileStore()

    store.profile = {
      name: 'John',
      email: 'john@example.com',
      mainResumeId: 1,
      resumes: [1, 2],
    } satisfies Profile

    mockedApi.patch.mockResolvedValueOnce({})

    await store.setMainResume(2)

    expect(store.profile!.mainResumeId).toBe(2)
  })
})
