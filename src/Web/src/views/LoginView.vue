<script setup lang="ts">
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const auth = useAuthStore()
const route = useRoute()
const router = useRouter()
const userName = ref('case_mgr')
const password = ref('ChangeMe!')
const error = ref('')
const busy = ref(false)

async function submit() {
  error.value = ''
  busy.value = true
  try {
    await auth.login(userName.value, password.value)
    const r = route.query.redirect as string | undefined
    router.replace(r && r.startsWith('/') ? r : '/cases')
  } catch {
    error.value = '登入失敗，請確認帳號密碼。'
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <div class="card">
    <h1>LDCT 個案管理平台</h1>
    <p class="hint">雛型帳號：<code>case_mgr</code>／<code>admin</code> 等，密碼 <code>ChangeMe!</code></p>
    <form @submit.prevent="submit">
      <label>帳號 <input v-model="userName" autocomplete="username" /></label>
      <label>密碼 <input v-model="password" type="password" autocomplete="current-password" /></label>
      <p v-if="error" class="err">{{ error }}</p>
      <button type="submit" :disabled="busy">{{ busy ? '登入中…' : '登入' }}</button>
    </form>
  </div>
</template>

<style scoped>
.card {
  max-width: 380px;
  margin: 4rem auto;
  padding: 2rem;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.08);
}
h1 {
  font-size: 1.35rem;
  margin: 0 0 0.5rem;
  color: #1e3a5f;
}
.hint {
  font-size: 0.85rem;
  color: #666;
  margin-bottom: 1.25rem;
}
form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
label {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
  font-size: 0.9rem;
  color: #333;
}
input {
  padding: 0.5rem 0.6rem;
  border: 1px solid #ccd4dd;
  border-radius: 6px;
}
.err {
  color: #b00020;
  margin: 0;
  font-size: 0.9rem;
}
button {
  padding: 0.6rem;
  background: #2563eb;
  color: #fff;
  border: none;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
}
button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
