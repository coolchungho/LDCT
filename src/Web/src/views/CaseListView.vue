<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import api from '../lib/api'

interface CaseRow {
  id: string
  medicalRecordNumber: string
  patientName: string
  examDate: string
  customerName?: string
  trackType?: string
  track1?: string
  track2?: string
  track3?: string
  track4?: string
  chestClinicOneMonth: boolean
  track3Months: boolean
  track6Months: boolean
  track12Months: boolean
  ldctStatus?: string
  isClosed: boolean
  closureCode?: number
  reportLanded: boolean
}

const router = useRouter()
const auth = useAuthStore()
const rows = ref<CaseRow[]>([])
const loading = ref(false)
const defaultDueThisMonth = ref(true)
const openOnly = ref(false)
const examFrom = ref('')
const examTo = ref('')
const urlCache = ref<Record<string, { reportQuery?: string; basicProfile?: string; smsPortal?: string }>>({})

const closureCodes: Record<number, string> = {
  0: '失聯',
  1: '外院追蹤',
  2: '本院追蹤',
  3: '複檢 LDCT',
  4: '確診程序',
  5: '提醒追蹤／衛教',
  6: '肺轉移或其他癌症',
}

async function load() {
  loading.value = true
  try {
    const params: Record<string, string | boolean> = {
      defaultDueThisMonth: defaultDueThisMonth.value,
    }
    if (openOnly.value) params.openOnly = true
    if (examFrom.value) params.examFrom = examFrom.value
    if (examTo.value) params.examTo = examTo.value
    const { data } = await api.get<CaseRow[]>('/api/v1/cases', { params })
    rows.value = data
  } finally {
    loading.value = false
  }
}

async function urlsFor(caseId: string) {
  if (!urlCache.value[caseId]) {
    const { data } = await api.get('/api/v1/integrations/urls', { params: { caseId } })
    urlCache.value[caseId] = data as typeof urlCache.value[string]
  }
  return urlCache.value[caseId]
}

async function openReportQuery(caseId: string) {
  const u = await urlsFor(caseId)
  if (u.reportQuery) window.open(u.reportQuery, '_blank', 'noopener')
}

async function openBasic(caseId: string) {
  const u = await urlsFor(caseId)
  if (u.basicProfile) window.open(u.basicProfile, '_blank', 'noopener')
}

function goFollowUp(caseId: string) {
  router.push(`/cases/${caseId}/follow-up`)
}

async function goSms(caseId: string) {
  const u = await urlsFor(caseId)
  const path = u.smsPortal?.startsWith('http') ? u.smsPortal : `/sms?caseId=${caseId}`
  if (u.smsPortal?.startsWith('http')) window.open(u.smsPortal, '_blank', 'noopener')
  else router.push(path)
}

/** 以今日為基準加減月份，回傳 YYYY-MM-DD（供示範檢查日） */
function examMonthsFromToday(deltaMonths: number): string {
  const x = new Date()
  x.setMonth(x.getMonth() + deltaMonths)
  return x.toISOString().slice(0, 10)
}

type RosterRow = {
  medicalRecordNumber: string
  patientName: string
  gender: string
  age: number
  examDate: string
  customerName: string
  phone: string
  mobile: string
}

type EveningRow = {
  medicalRecordNumber: string
  hasNodule: boolean
  noduleCount: number | null
  maxNoduleLengthMm: number | null
  needOutpatientFollowUp: boolean
  track3Months: boolean
  track6Months: boolean
  track12Months: boolean
  narrativeSummary: string
  issuedReport: boolean
}

async function seedDemo() {
  const e1 = examMonthsFromToday(-1) // 胸腔門診 +1 個月 → 本月應追蹤
  const e2 = examMonthsFromToday(-2)
  const e3 = examMonthsFromToday(-3) // +3 個月 → 本月
  const e4 = examMonthsFromToday(-4) // +3 個月 → 非本月（僅 track3 時）
  const e6 = examMonthsFromToday(-6) // +6 個月 → 本月
  const e12 = examMonthsFromToday(-12) // +12 個月 → 本月
  const e5 = examMonthsFromToday(-5) // +3 個月 → 非本月

  const roster: RosterRow[] = [
    {
      medicalRecordNumber: 'MRN-DEMO-001',
      patientName: '王小明',
      gender: 'M',
      age: 58,
      examDate: e3,
      customerName: '示範企業 A',
      phone: '02-12345678',
      mobile: '0912000111',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-002',
      patientName: '李美華',
      gender: 'F',
      age: 62,
      examDate: e6,
      customerName: '示範企業 B',
      phone: '02-22334455',
      mobile: '0922000222',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-003',
      patientName: '張志偉',
      gender: 'M',
      age: 55,
      examDate: e1,
      customerName: '科技園區健檢',
      phone: '03-44556677',
      mobile: '0933000333',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-004',
      patientName: '陳淑芬',
      gender: 'F',
      age: 64,
      examDate: e12,
      customerName: '社區篩檢第 2 梯',
      phone: '04-77889900',
      mobile: '0944000444',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-005',
      patientName: '林國棟',
      gender: 'M',
      age: 59,
      examDate: e3,
      customerName: '公務體檢',
      phone: '02-99887766',
      mobile: '0955000555',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-006',
      patientName: '黃雅婷',
      gender: 'F',
      age: 61,
      examDate: e4,
      customerName: '示範企業 A',
      phone: '02-11223344',
      mobile: '0966000666',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-007',
      patientName: '吳建宏',
      gender: 'M',
      age: 57,
      examDate: e2,
      customerName: '自費健檢',
      phone: '07-55667788',
      mobile: '0977000777',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-008',
      patientName: '鄭秀英',
      gender: 'F',
      age: 66,
      examDate: e5,
      customerName: '長照合作單位',
      phone: '08-33445566',
      mobile: '0988000888',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-009',
      patientName: '許家豪',
      gender: 'M',
      age: 54,
      examDate: e3,
      customerName: '金融業聯合篩檢',
      phone: '02-66778899',
      mobile: '0999000999',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-010',
      patientName: '劉怡君',
      gender: 'F',
      age: 60,
      examDate: e6,
      customerName: '示範企業 C',
      phone: '03-22110099',
      mobile: '0910110123',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-011',
      patientName: '蔡宗憲',
      gender: 'M',
      age: 63,
      examDate: e1,
      customerName: '工廠團檢',
      phone: '06-88776655',
      mobile: '0922334455',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-012',
      patientName: '楊佩蓉',
      gender: 'F',
      age: 56,
      examDate: e12,
      customerName: '里民篩檢',
      phone: '02-55443322',
      mobile: '0933445566',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-013',
      patientName: '謝文龍',
      gender: 'M',
      age: 65,
      examDate: e3,
      customerName: '醫院員工健檢',
      phone: '02-77889900',
      mobile: '0944556677',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-014',
      patientName: '郭美玲',
      gender: 'F',
      age: 59,
      examDate: e4,
      customerName: '示範企業 B',
      phone: '04-99001122',
      mobile: '0955667788',
    },
    {
      medicalRecordNumber: 'MRN-DEMO-015',
      patientName: '江承翰',
      gender: 'M',
      age: 52,
      examDate: e2,
      customerName: '新進員工體檢',
      phone: '02-33445566',
      mobile: '0966778899',
    },
  ]

  const eveningBatch: EveningRow[] = [
    {
      medicalRecordNumber: 'MRN-DEMO-001',
      hasNodule: true,
      noduleCount: 1,
      maxNoduleLengthMm: 6,
      needOutpatientFollowUp: true,
      track3Months: true,
      track6Months: false,
      track12Months: true,
      narrativeSummary: '示範報告：結節 6mm，建議追蹤；含 3／12 個月類型。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-002',
      hasNodule: false,
      noduleCount: 0,
      maxNoduleLengthMm: null,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: true,
      track12Months: false,
      narrativeSummary: '示範：3／6 個月追蹤（+6 個月應到於本月）。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-003',
      hasNodule: true,
      noduleCount: 2,
      maxNoduleLengthMm: 4.5,
      needOutpatientFollowUp: true,
      track3Months: false,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '胸腔門診建議一個月內回診；結節 4.5mm。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-004',
      hasNodule: false,
      noduleCount: 0,
      maxNoduleLengthMm: null,
      needOutpatientFollowUp: false,
      track3Months: false,
      track6Months: false,
      track12Months: true,
      narrativeSummary: '一年期 LDCT 追蹤；無結節。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-005',
      hasNodule: true,
      noduleCount: 3,
      maxNoduleLengthMm: 8,
      needOutpatientFollowUp: true,
      track3Months: true,
      track6Months: true,
      track12Months: false,
      narrativeSummary: '多發結節，最大 8mm，建議密集追蹤。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-006',
      hasNodule: false,
      noduleCount: 0,
      maxNoduleLengthMm: null,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '僅 3 個月追蹤；+3 應到日非本月（供關閉「本月應追蹤」時仍見）。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-007',
      hasNodule: true,
      noduleCount: 1,
      maxNoduleLengthMm: 5,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '單一結節 5mm，建議 3 個月複查。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-008',
      hasNodule: false,
      noduleCount: 0,
      maxNoduleLengthMm: null,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '低風險；3 個月電訪提醒。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-009',
      hasNodule: true,
      noduleCount: 1,
      maxNoduleLengthMm: 7,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: true,
      track12Months: true,
      narrativeSummary: '結節 7mm，多時程追蹤建議。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-010',
      hasNodule: false,
      noduleCount: 0,
      maxNoduleLengthMm: null,
      needOutpatientFollowUp: false,
      track3Months: false,
      track6Months: true,
      track12Months: false,
      narrativeSummary: '半年追蹤；應到於本月。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-011',
      hasNodule: true,
      noduleCount: 1,
      maxNoduleLengthMm: 9,
      needOutpatientFollowUp: true,
      track3Months: false,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '疑似需胸腔科評估；結節 9mm。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-012',
      hasNodule: false,
      noduleCount: 0,
      maxNoduleLengthMm: null,
      needOutpatientFollowUp: false,
      track3Months: false,
      track6Months: false,
      track12Months: true,
      narrativeSummary: '年度追蹤個案；無急性發現。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-013',
      hasNodule: true,
      noduleCount: 2,
      maxNoduleLengthMm: 5.5,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '雙側小結節，3 個月影像追蹤。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-014',
      hasNodule: false,
      noduleCount: 0,
      maxNoduleLengthMm: null,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '文書示範：建議電訪確認回診意願。',
      issuedReport: true,
    },
    {
      medicalRecordNumber: 'MRN-DEMO-015',
      hasNodule: false,
      noduleCount: 0,
      maxNoduleLengthMm: null,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '3 個月追蹤；+3 應到約為下個月（供對照預設篩選）。',
      issuedReport: true,
    },
    // 無對應個案：測試 orphan 報告列（ImportsController 會寫入 OrphanLdctReports）
    {
      medicalRecordNumber: 'MRN-ORPHAN-NOMATCH',
      hasNodule: true,
      noduleCount: 1,
      maxNoduleLengthMm: 4,
      needOutpatientFollowUp: false,
      track3Months: true,
      track6Months: false,
      track12Months: false,
      narrativeSummary: '示範：EHR 有報告但名單無此病歷號（晚間批次 orphan）。',
      issuedReport: true,
    },
  ]

  await api.post('/api/v1/imports/roster', roster)
  await api.post('/api/v1/imports/evening-batch', eveningBatch)
  await load()
}

async function viewReport(id: string) {
  const { data } = await api.get(`/api/v1/cases/${id}/report`)
  alert(
    `報告摘要：${data.summary ?? '—'}\n結節：${data.hasNodule}\n外部連結提示：${data.externalUrlHint ?? '—'}`
  )
}

async function closeCase(id: string) {
  const codeStr = prompt('結案代碼 0–6（見 spec）：')
  if (codeStr == null) return
  const code = parseInt(codeStr, 10)
  if (Number.isNaN(code) || code < 0 || code > 6) {
    alert('代碼無效')
    return
  }
  await api.post(`/api/v1/cases/${id}/closure`, { closureCode: code })
  await load()
}

onMounted(load)
</script>

<template>
  <div class="page">
    <div class="toolbar">
      <h2>個案總清單</h2>
      <div class="filters">
        <label><input v-model="defaultDueThisMonth" type="checkbox" @change="load" /> 預設：本月應追蹤</label>
        <label><input v-model="openOnly" type="checkbox" @change="load" /> 僅未結案</label>
        <label>檢查起 <input v-model="examFrom" type="date" @change="load" /></label>
        <label>檢查迄 <input v-model="examTo" type="date" @change="load" /></label>
        <button type="button" @click="load">重新載入</button>
        <button
          v-if="auth.role === 'CaseManager' || auth.role === 'Administrator'"
          type="button"
          class="secondary"
          @click="seedDemo"
        >
          載入示範名單與晚間批次
        </button>
      </div>
    </div>

    <p v-if="loading">載入中…</p>
    <div v-else class="table-wrap">
      <table class="grid">
        <thead>
          <tr>
            <th>病歷號</th>
            <th>姓名</th>
            <th>檢查日</th>
            <th>客戶</th>
            <th>胸腔門診</th>
            <th>3m</th>
            <th>6m</th>
            <th>12m</th>
            <th>LDCT</th>
            <th>結案</th>
            <th>報告</th>
            <th>列尾操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="r in rows" :key="r.id">
            <td>{{ r.medicalRecordNumber }}</td>
            <td>{{ r.patientName }}</td>
            <td>{{ r.examDate }}</td>
            <td>{{ r.customerName ?? '—' }}</td>
            <td>{{ r.chestClinicOneMonth ? '✓' : '' }}</td>
            <td>{{ r.track3Months ? '✓' : '' }}</td>
            <td>{{ r.track6Months ? '✓' : '' }}</td>
            <td>{{ r.track12Months ? '✓' : '' }}</td>
            <td>{{ r.ldctStatus ?? '—' }}</td>
            <td>
              {{
                r.isClosed
                  ? r.closureCode != null
                    ? closureCodes[r.closureCode] ?? `#${r.closureCode}`
                    : '—'
                  : '未結案'
              }}
            </td>
            <td>
              <button
                v-if="auth.role === 'CaseManager' || auth.role === 'Physician' || auth.role === 'Administrator'"
                type="button"
                class="small"
                @click="viewReport(r.id)"
              >
                檢視報告
              </button>
            </td>
            <td class="actions">
              <button type="button" class="small" @click="goFollowUp(r.id)">追蹤記錄</button>
              <button type="button" class="small" @click="openReportQuery(r.id)">報告查詢</button>
              <button type="button" class="small" @click="openBasic(r.id)">基本資料</button>
              <button
                v-if="auth.role === 'CaseManager' || auth.role === 'Administrator'"
                type="button"
                class="small"
                @click="goSms(r.id)"
              >
                簡訊發送
              </button>
              <button
                v-if="(auth.role === 'CaseManager' || auth.role === 'Administrator') && !r.isClosed"
                type="button"
                class="small warn"
                @click="closeCase(r.id)"
              >
                Quick 結案
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <p v-if="!rows.length" class="empty">無符合條件之個案（可先按「載入示範」或確認晚間批次已完成落地）。</p>
    </div>
  </div>
</template>

<style scoped>
.page h2 {
  margin: 0 0 1rem;
  color: #1e3a5f;
}
.toolbar {
  margin-bottom: 1rem;
}
.filters {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem 1rem;
  align-items: center;
  font-size: 0.9rem;
}
.filters label {
  display: flex;
  align-items: center;
  gap: 0.35rem;
}
button {
  padding: 0.35rem 0.75rem;
  border-radius: 6px;
  border: none;
  background: #2563eb;
  color: #fff;
  cursor: pointer;
  font-size: 0.85rem;
}
button.secondary {
  background: #475569;
}
.table-wrap {
  overflow: auto;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.06);
}
.grid {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.85rem;
}
.grid th,
.grid td {
  border: 1px solid #e2e8f0;
  padding: 0.45rem 0.5rem;
  text-align: left;
}
.grid th {
  background: #eef2f7;
  white-space: nowrap;
}
.actions {
  white-space: normal;
  min-width: 200px;
}
.actions .small {
  margin: 0.15rem 0.15rem 0.15rem 0;
}
button.small {
  padding: 0.2rem 0.45rem;
  font-size: 0.75rem;
}
button.warn {
  background: #b45309;
}
.empty {
  padding: 1rem;
  color: #64748b;
}
</style>
