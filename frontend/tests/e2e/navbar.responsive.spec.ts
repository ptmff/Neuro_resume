import { test, expect } from '@playwright/test'

test.describe('Navbar responsiveness', () => {
  const viewports = [
    { name: 'Desktop', width: 1280, height: 800 },
    { name: 'Tablet', width: 768, height: 1024 },
    { name: 'Mobile', width: 375, height: 667 }
  ]

  for (const viewport of viewports) {
    test(`should display navbar correctly on ${viewport.name}`, async ({ page }) => {
      await page.setViewportSize({ width: viewport.width, height: viewport.height })
      await page.goto('/')

      const navbar = await page.locator('nav')
      await expect(navbar).toBeVisible()

      await expect(navbar.locator('text=Главная')).toBeVisible()
      await expect(navbar.locator('text=Профиль')).toBeVisible()

    })
  }
})
