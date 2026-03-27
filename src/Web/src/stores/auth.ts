import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '../lib/api'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('ldct_token'))
  const userName = ref<string | null>(localStorage.getItem('ldct_user'))
  const role = ref<string | null>(localStorage.getItem('ldct_role'))

  const isAuthenticated = computed(() => !!token.value)

  function setSession(t: string, name: string, r: string) {
    token.value = t
    userName.value = name
    role.value = r
    localStorage.setItem('ldct_token', t)
    localStorage.setItem('ldct_user', name)
    localStorage.setItem('ldct_role', r)
  }

  function logout() {
    token.value = null
    userName.value = null
    role.value = null
    localStorage.removeItem('ldct_token')
    localStorage.removeItem('ldct_user')
    localStorage.removeItem('ldct_role')
  }

  async function login(userNameIn: string, password: string) {
    const { data } = await api.post('/api/v1/auth/login', {
      userName: userNameIn,
      password,
    })
    setSession(data.token, data.userName, data.role)
  }

  return { token, userName, role, isAuthenticated, login, logout }
})
