  import { setActivePinia, createPinia } from 'pinia'
  import { useProfileStore } from '@/stores/profile'
  import { vi, describe, it, expect, beforeEach } from 'vitest'
  import api from '@/api'  
  
  vi.mock('@/api', () => {
    const get = vi.fn()
    const patch = vi.fn()
  
    return {
      __esModule: true,
      default: {
        get,
        patch
      }
    }
  })
  
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
  
    it('fetchProfile should handle API error gracefully', async () => {
      api.get.mockRejectedValueOnce(new Error('Profile fetch failed'))
      api.get.mockRejectedValueOnce(new Error('Resumes fetch failed'))
  
      const store = useProfileStore()
      store.profile = { name: 'Old User' }
      store.allResumes = [{ id: 123, title: 'Old Resume' }]
  
      const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {})
  
      await store.fetchProfile()
  
      expect(store.profile).toEqual({ name: 'Old User' })
      expect(store.allResumes).toEqual([{ id: 123, title: 'Old Resume' }])
      expect(consoleSpy).toHaveBeenCalled()
  
      consoleSpy.mockRestore()
    })
  
    it('setMainResume should update mainResumeId', async () => {
      const store = useProfileStore()
      store.profile = { name: 'John', mainResumeId: 1 }
  
      api.patch.mockResolvedValueOnce({})
  
      await store.setMainResume(2)
  
      expect(store.profile.mainResumeId).toBe(2)
    })
  })
  