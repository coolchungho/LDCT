import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'

// 測試機 API 與前端同主機時維持預設即可；API 在別台時設 VITE_API_PROXY_TARGET（見 .env）
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')
  const apiTarget = env.VITE_API_PROXY_TARGET || 'http://127.0.0.1:5003'
  return {
    plugins: [vue()],
    server: {
      port: 5173,
      host: true,
      proxy: {
        '/api': {
          target: apiTarget,
          changeOrigin: true,
        },
      },
    },
  }
})
