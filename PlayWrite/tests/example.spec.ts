import { test, expect } from '@playwright/test';
import { setMaxIdleHTTPParsers } from 'http';

test('has title', async ({ page }) => {
  await page.goto('https://playwright.dev/');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Playwright/);
});

test('get started link', async ({ page }) => {
  await page.goto('https://playwright.dev/');

  // Click the get started link.
  await page.getByRole('link', { name: 'Get started' }).click();

  // Expects page to have a heading with the name of Installation.
  await expect(page.getByRole('heading', { name: 'Installation' })).toBeVisible();
});

test('currency table', async ({ page })=> {
  await page.goto('http://localhost:4000');
  await page.waitForSelector('#CurrencyListTable');
  const table = page.locator('#CurrencyListTable');
  await expect(await table.isVisible()).toBe(true);
});