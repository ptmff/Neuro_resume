import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: ['./tests/setup/vitest.setup.ts'],
  },
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5116',
        changeOrigin: true,
        secure: false,
      },
    },
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
})

//eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0IiwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwibmJmIjoxNzQyMjI0NjE4LCJleHAiOjE3NDI4Mjk0MTgsImlhdCI6MTc0MjIyNDYxOCwiaXNzIjoi0LLQsNGIX2lzc3VlciIsImF1ZCI6ItCy0LDRiF9hdWRpZW5jZSJ9.ydK-oZWg6zzvXQ10cIRNwctl0TvYvuK5CKA5gsTxpLeFf2I4aunrkFqkFyIcN0t1ta86mzwf67B_8sHqBJWalw