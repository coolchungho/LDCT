<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../lib/api'

const route = useRoute()
const router = useRouter()
const caseId = ref((route.query.caseId as string) || '')
const phone = ref('')
const message = ref('【LDCT 追蹤提醒】請依約回診或聯繫個管師。')
const autoLog = ref(true)
const status = ref('')

onMounted(async () => {
  if (!caseId.value) return
  try {
    const { data } = await api.get(`/api/v1/cases/${caseId.value}`)
    phone.value = (data as { mobile?: string }).mobile || ''
  } catch {
    /* ignore */
  }
})

async function send() {
  status.value = ''
  const { data } = await api.post(
    `/api/v1/sms/send?caseId=${encodeURIComponent(caseId.value)}`,
    { phone: phone.value, message: message.value, autoCreateFollowUpLog: autoLog.value }
  )
  status.value = `已紀錄：${data.status}（ID ${data.smsId}）`
}
</script>

<template>
  <div class="page">
    <button type="button" class="back" @click="router.push('/cases')">← 回清單</button>
    <h2>簡訊發送（雛型）</h2>
    <p class="hint">對齊 sms-fetnet：實際發送需院方憑證；此處模擬寫入與可選追蹤紀錄連動。</p>
    <div class="panel">
      <label>caseId <input v-model="caseId" /></label>
      <label>手機 <input v-model="phone" /></label>
      <label>內容 <textarea v-model="message" rows="4" /></label>
      <label><input v-model="autoLog" type="checkbox" /> 同步寫入追蹤紀錄（簡訊）</label>
      <button type="button" :disabled="!caseId || !phone" @click="send">送出</button>
      <p v-if="status" class="ok">{{ status }}</p>
    </div>
  </div>
</template>

<style scoped>
.page {
  max-width: 520px;
}
.back {
  background: none;
  border: none;
  color: #2563eb;
  cursor: pointer;
  margin-bottom: 0.5rem;
}
.hint {
  color: #64748b;
  font-size: 0.9rem;
}
.panel {
  background: #fff;
  padding: 1.25rem;
  border-radius: 8px;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}
label {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  font-size: 0.9rem;
}
input,
textarea {
  padding: 0.45rem;
  border-radius: 6px;
  border: 1px solid #cbd5e1;
}
button {
  padding: 0.5rem;
  background: #2563eb;
  color: #fff;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  margin-top: 0.5rem;
}
.ok {
  color: #15803d;
}
</style>
