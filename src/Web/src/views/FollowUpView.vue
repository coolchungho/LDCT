<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../lib/api'

const route = useRoute()
const router = useRouter()
const caseId = computed(() => route.params.caseId as string)
const header = ref<{ medicalRecordNumber: string; patientName: string; examDate: string } | null>(null)
const logs = ref<
  {
    id: string
    trackCorridor: string
    contactType: string
    contactDate: string
    contactResult?: string
    content: string
    hisQuerySummary?: string
    createdAt: string
  }[]
>([])
const form = ref({
  trackCorridor: '3個月',
  contactType: '電訪',
  contactDate: new Date().toISOString().slice(0, 10),
  contactResult: '',
  content: '',
  hisQuerySummary: '',
})

async function load() {
  const [{ data: h }, { data: l }] = await Promise.all([
    api.get(`/api/v1/cases/${caseId.value}/FollowUp`),
    api.get(`/api/v1/cases/${caseId.value}/FollowUp/logs`),
  ])
  header.value = h
  logs.value = l
}

async function addLog() {
  await api.post(`/api/v1/cases/${caseId.value}/FollowUp/logs`, {
    trackCorridor: form.value.trackCorridor,
    contactType: form.value.contactType,
    contactDate: form.value.contactDate,
    contactResult: form.value.contactResult || null,
    content: form.value.content,
    hisQuerySummary: form.value.hisQuerySummary || null,
  })
  form.value.content = ''
  await load()
}

onMounted(load)
</script>

<template>
  <div class="page">
    <button type="button" class="back" @click="router.push('/cases')">← 回清單</button>
    <template v-if="header">
      <h2>追蹤紀錄</h2>
      <p class="meta">
        病歷號 {{ header.medicalRecordNumber }}／{{ header.patientName }}／檢查日 {{ header.examDate }}
      </p>
    </template>

    <section class="panel">
      <h3>新增紀錄</h3>
      <div class="form-grid">
        <label>軌道 <input v-model="form.trackCorridor" /></label>
        <label>聯繫類型 <input v-model="form.contactType" /></label>
        <label>日期 <input v-model="form.contactDate" type="date" /></label>
        <label>結果 <input v-model="form.contactResult" /></label>
        <label class="full">內容 <textarea v-model="form.content" rows="3" /></label>
        <label class="full">HIS 摘要 <input v-model="form.hisQuerySummary" /></label>
      </div>
      <button type="button" @click="addLog" :disabled="!form.content.trim()">儲存</button>
    </section>

    <section class="panel">
      <h3>歷次紀錄</h3>
      <ul class="timeline">
        <li v-for="e in logs" :key="e.id">
          <div class="when">{{ e.contactDate }} · {{ e.contactType }} · {{ e.trackCorridor }}</div>
          <div class="what">{{ e.content }}</div>
          <div v-if="e.hisQuerySummary" class="his">HIS：{{ e.hisQuerySummary }}</div>
        </li>
      </ul>
      <p v-if="!logs.length" class="empty">尚無紀錄</p>
    </section>
  </div>
</template>

<style scoped>
.page {
  max-width: 800px;
}
.back {
  background: none;
  border: none;
  color: #2563eb;
  cursor: pointer;
  margin-bottom: 0.5rem;
}
.meta {
  color: #475569;
  margin-bottom: 1.25rem;
}
.panel {
  background: #fff;
  padding: 1.25rem;
  border-radius: 8px;
  margin-bottom: 1rem;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.06);
}
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.75rem;
  margin-bottom: 0.75rem;
}
.full {
  grid-column: 1 / -1;
}
label {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  font-size: 0.85rem;
}
input,
textarea {
  padding: 0.4rem;
  border: 1px solid #cbd5e1;
  border-radius: 6px;
}
.timeline {
  list-style: none;
  padding: 0;
  margin: 0;
}
.timeline li {
  border-left: 3px solid #93c5fd;
  padding: 0.5rem 0 0.75rem 1rem;
  margin-bottom: 0.5rem;
}
.when {
  font-size: 0.8rem;
  color: #64748b;
}
.what {
  margin-top: 0.25rem;
}
.his {
  font-size: 0.85rem;
  color: #0369a1;
  margin-top: 0.35rem;
}
.empty {
  color: #94a3b8;
}
button {
  padding: 0.45rem 1rem;
  background: #2563eb;
  color: #fff;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}
</style>
