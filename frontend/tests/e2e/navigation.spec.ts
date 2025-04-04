import { test, expect } from '@playwright/test'

test('navigation works correctly', async ({ page }) => {
  await page.goto('/')
  
  await expect(page.getByText('Welcome to NeuroResume')).toBeVisible()
  
  await page.click('text=Resume')
  await expect(page.getByText('Resume Builder')).toBeVisible()
  
  await page.click('text=Profile')
  await expect(page.getByText('Profile')).toBeVisible()
  
  await page.click('text=Analysis')
  await expect(page.getByText('Job Analysis')).toBeVisible()
})