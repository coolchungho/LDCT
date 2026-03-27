<script setup lang="ts">
import { RouterView, useRouter } from 'vue-router'
import { useAuthStore } from './stores/auth'

const auth = useAuthStore()
const router = useRouter()

function navLogout() {
  auth.logout()
  router.push('/login')
}
</script>

<template>
  <div class="layout">
    <header v-if="auth.isAuthenticated" class="topbar">
      <nav class="nav">
        <router-link to="/cases">個案清單</router-link>
        <router-link to="/alerts">臨床提醒</router-link>
        <router-link v-if="auth.role === 'Administrator' || auth.role === 'CaseManager'" to="/reporting">
          報表資料集
        </router-link>
      </nav>
      <div class="user">
        <span>{{ auth.userName }}（{{ auth.role }}）</span>
        <button type="button" class="link" @click="navLogout">登出</button>
      </div>
    </header>
    <main class="main">
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
.layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background: #f4f6f9;
}
.topbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.75rem 1.25rem;
  background: #1e3a5f;
  color: #e8eef5;
}
.nav {
  display: flex;
  gap: 1.25rem;
}
.nav a {
  color: #b8d4f0;
  text-decoration: none;
  font-weight: 500;
}
.nav a.router-link-active {
  color: #fff;
}
.user {
  display: flex;
  gap: 1rem;
  align-items: center;
  font-size: 0.9rem;
}
.link {
  background: none;
  border: none;
  color: #7ec8ff;
  cursor: pointer;
  text-decoration: underline;
}
.main {
  flex: 1;
  padding: 1.25rem;
  max-width: 1400px;
  margin: 0 auto;
  width: 100%;
  box-sizing: border-box;
}
</style>
