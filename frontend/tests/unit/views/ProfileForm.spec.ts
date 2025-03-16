import { describe, it, expect, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { useProfileStore } from '@/store/profile'
import ProfileView from '@/pages/ProfileView.vue'

describe('ProfileView', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })

  it('updates profile store when form is filled', async () => {
    const wrapper = mount(ProfileView)
    
    await wrapper.find('#firstName').setValue('John')
    await wrapper.find('#lastName').setValue('Doe')
    await wrapper.find('#email').setValue('john@example.com')
    
    const store = useProfileStore()
    expect(store.firstName).toBe('John')
    expect(store.lastName).toBe('Doe')
    expect(store.email).toBe('john@example.com')
    expect(store.fullName).toBe('John Doe')
  })
})