<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../lib/api'

const raw = ref<string>('')

async function load() {
  const { data } = await api.get('/api/v1/reporting/dataset')
  raw.value = JSON.stringify(data, null, 2)
}

onMounted(load)
</script>

<template>
  <div class="page">
    <h2>報表資料集（JSON）</h2>
    <p class="hint">
      語意與清單一致，供 Power BI／閘道或直接匯出銜接；正式環境應改為受控端點與 RLS。
    </p>
    <pre>{{ raw }}</pre>
    <button type="button" @click="load">重新整理</button>
  </div>
</template>

<style scoped>
.page {
  max-width: 960px;
}
.hint {
  color: #64748b;
  font-size: 0.9rem;
}
pre {
  background: #0f172a;
  color: #e2e8f0;
  padding: 1rem;
  border-radius: 8px;
  overflow: auto;
  max-height: 60vh;
  font-size: 0.8rem;
}
button {
  margin-top: 0.75rem;
  padding: 0.45rem 1rem;
  background: #2563eb;
  color: #fff;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}
</style>
