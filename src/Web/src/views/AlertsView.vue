<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../lib/api'

interface Alert {
  severity: string
  code: string
  message: string
  caseId?: string
  medicalRecordNumber?: string
}

const items = ref<Alert[]>([])

async function load() {
  const { data } = await api.get('/api/v1/clinicalalerts')
  items.value = data
}

onMounted(load)
</script>

<template>
  <div class="page">
    <h2>臨床提醒</h2>
    <button type="button" class="refresh" @click="load">重新整理</button>
    <ul>
      <li v-for="(a, i) in items" :key="i" :class="a.severity">
        <strong>{{ a.severity }}</strong> · {{ a.code }} — {{ a.message }}
        <span v-if="a.medicalRecordNumber">（{{ a.medicalRecordNumber }}）</span>
      </li>
    </ul>
    <p v-if="!items.length" class="empty">目前無提醒</p>
  </div>
</template>

<style scoped>
.page {
  max-width: 720px;
}
.refresh {
  margin-bottom: 1rem;
  padding: 0.35rem 0.75rem;
  border: none;
  border-radius: 6px;
  background: #475569;
  color: #fff;
  cursor: pointer;
}
ul {
  list-style: none;
  padding: 0;
  margin: 0;
}
li {
  padding: 0.65rem 0.85rem;
  border-radius: 6px;
  margin-bottom: 0.5rem;
  background: #fff;
  border: 1px solid #e2e8f0;
}
li.critical {
  border-color: #fecaca;
  background: #fef2f2;
}
li.warning {
  border-color: #fde68a;
  background: #fffbeb;
}
.empty {
  color: #94a3b8;
}
</style>
