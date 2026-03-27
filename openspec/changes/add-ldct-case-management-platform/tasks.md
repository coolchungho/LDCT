# 實作與驗收任務 — `add-ldct-case-management-platform`

## OpenSpec 工具（本機）

- [x] 若已安裝 OpenSpec CLI：執行 `openspec validate add-ldct-case-management-platform --strict`
- [ ] 功能上線並確認行為符合 spec 後：執行 `openspec archive add-ldct-case-management-platform --yes`，合併至 `openspec/specs/`

## 初始化與文件

- [x] 建立 `openspec/project.md`、`openspec/AGENTS.md`
- [x] 建立本 change 之 `proposal.md`、`design.md`、各 capability 之 `specs/.../spec.md`
- [x] 實作程式後更新 [計畫書.md](../../../計畫書.md) 對應章節（與本變更一致）

## 依 capability 與 Requirement 對照（驗收勾選）

### data-sources（[`specs/data-sources/spec.md`](specs/data-sources/spec.md)）

- [x] **健檢名單匯入（Health Screening Roster Import）** — 欄位、匯入、錯誤處理 Scenario（詳細格式錯誤／全檔回滾策略仍待院方定案）
- [x] **LDCT 影像報告匯入（LDCT Report Import）** — 關聯、無對應個案、**已發報告當日晚間納入個案清單** Scenario（以 `POST /api/v1/imports/evening-batch` 模擬晚間批次；真 EHR 排程與狀態碼另定案）
- [x] **LDCT 報告欄位 LLM 擷取（LLM Field Extraction from LDCT Report）** — 非結構化報告、人工覆核、低信心 Scenario（`POST .../integrations/llm/extract` 為雛型；未接真端點、PHI 須資安核定）
- [ ] **個管師手動輸入（Case Manager Manual Entry）** — 儲存與稽核 Scenario（部分欄位已於個案／追蹤承載；專責 API 可再補）
- [x] **問卷資料整合（Questionnaire Data）** — 關聯 Scenario（`survey/sync` stub）
- [ ] **確診資料登錄（Diagnosis Data Entry）** — 欄位與驗證 Scenario（實體欄位雛型；完整驗證規則待補）

### case-list（[`specs/case-list/spec.md`](specs/case-list/spec.md)）

- [x] **LDCT 個案總清單（Master Case List）** — 欄位顯示 Scenario
- [x] **列尾對外連結操作（Row Outbound Action Buttons）** — 四枚按鈕；**追蹤記錄**→`follow-up-logging`；其餘導向、權限 Scenario
- [x] **查詢與日期篩選（Search and Filter）** — 篩選 Scenario
- [x] **預設篩選—本月應追蹤（Default Filter Due This Month）** — 預設開啟、自訂篩選 Scenario
- [x] **檢視報告（View Report）** — 開啟報告 Scenario
- [x] **結案操作（Initiate Closure from List）** — 結案後狀態 Scenario

### follow-up-lists（[`specs/follow-up-lists/spec.md`](specs/follow-up-lists/spec.md)）

- [x] **本變更不驗收** — 見該檔 **REMOVED Requirements**（與 [計畫書.md](../../../計畫書.md) 第三節一致）

### follow-up-workflow（[`specs/follow-up-workflow/spec.md`](specs/follow-up-workflow/spec.md)）

- [x] **本變更不驗收** — 見該檔 **REMOVED Requirements**（與計畫書一致）

### follow-up-logging（[`specs/follow-up-logging/spec.md`](specs/follow-up-logging/spec.md)）

- [x] **個案脈絡載入（Case Context Load）** — 自清單進入、無權限 Scenario
- [x] **追蹤事項紀錄（Follow-up Activity Log）** — 新增、必填驗證 Scenario
- [x] **紀錄序與列表（Ordered Log and Table）** — 歷次列表、空狀態 Scenario
- [x] **紀錄之修正與稽核（Log Amendments）** — 補正後可追溯 Scenario
- [x] **與簡訊發送紀錄之對應（SMS Log Cross-Reference）** — 簡訊與追蹤紀錄對應 Scenario
- [x] **HIS 就醫紀錄查詢關聯（HIS Visit Reference）** — 註記、與 HIS 連結銜接 Scenario（HIS 為出站 URL 雛型）

### sms-fetnet（[`specs/sms-fetnet/spec.md`](specs/sms-fetnet/spec.md)）

- [x] **遠傳公務機簡訊平台整合（FET Enterprise SMS Integration）** — 成功／失敗 Scenario（模擬成功寫入；真實 `umc.fetnet.net` 憑證後啟用）
- [ ] **批次發送簡訊（Batch SMS）** — 批次選取 Scenario
- [x] **發送紀錄（SMS Audit Log）** — 稽核 Scenario
- [ ] **簡訊內容範例（SMS Content Template）** — 範本 Scenario

### case-closure（[`specs/case-closure/spec.md`](specs/case-closure/spec.md)）

- [x] **結案分類代碼（Closure Type Codes）** — 0–6 必選 Scenario
- [x] **結案狀態與清單一致性（Closure State Consistency）** — 統計更新 Scenario

### clinical-alerts（[`specs/clinical-alerts/spec.md`](specs/clinical-alerts/spec.md)）

- [x] **病理報告關鍵字提醒（Pathology Keyword Alert）** — 關鍵字觸發 Scenario（規則雛型）
- [ ] **肺部切片檢體結果提醒（Biopsy Result Alert）** — 待追蹤 Scenario
- [x] **尚未追蹤個案提醒（Overdue Follow-up Alert）** — 逾期列示 Scenario（規則雛型）

### reporting（[`specs/reporting/spec.md`](specs/reporting/spec.md)）

- [x] **報表資料集供應（Reporting Dataset Provisioning）** — 授權、語意一致 Scenario（JSON 資料集雛型；RLS／閘道後定案）
- [x] **資料集涵蓋之分析維度（Dataset Coverage for Analytics）** — 月報／管理維度、與清單一致 Scenario（可擴充欄位）
- [ ] **Power BI 連線與資料重新整理（Power BI Connectivity and Refresh）** — 連線文件、失敗追查 Scenario
- [x] **本平台內建報表之定位（Optional In-App Reporting）** — 與 Power BI 不矛盾 Scenario

### rbac（[`specs/rbac/spec.md`](specs/rbac/spec.md)）

- [x] **個案管理師角色（Case Manager Role）** — 操作、連結 HIS Scenario
- [x] **護理師角色（Nurse Role）** — 問卷 Scenario
- [x] **醫師角色（Physician Role）** — 判讀 Scenario
- [x] **管理者角色（Administrator Role）** — 資料集／Power BI 報表 Scenario
- [x] **身分驗證與授權（Authentication and Authorization）** — 未授權 Scenario（JWT 雛型＋`DevAuthUsers`）

### system-integrations（[`specs/system-integrations/spec.md`](specs/system-integrations/spec.md)）

- [x] **HIS 整合（HIS Integration）** — 個管自個案連結 HIS、無 SSO／失敗提示、非強制寫回 Scenario（URL 模板雛型）
- [x] **EHR LDCT 報告整合（EHR LDCT Report Integration）** — 可追溯、欄位對應 Scenario（以匯入批次模擬）
- [x] **LLM 服務整合（LLM Service Integration）** — 模型版本、失敗重試、資安 Scenario（雛型端點；正式須核定）
- [x] **問卷系統整合（Survey System Integration）** — 鍵一致 Scenario（stub）
- [x] **簡訊平台整合（SMS Platform Integration）** — 與 sms-fetnet 一致 Scenario（模擬）
- [ ] **Power BI 報表與視覺化（Power BI Analytics）** — 建模、權限與閘道 Scenario

## 端到端驗收（跨多 Scenario）

- [x] 匯入健檢＋報告後（含 LLM 擷取非結構化報告若適用），個案出現於總清單且追蹤欄位／預設「本月應追蹤」篩選正確（data-sources、case-list）（前端「載入示範」＋清單篩選可驗證）
- [x] 總清單未結案視角與結案狀態一致，報表資料集口徑與 `case-closure` 對齊（case-list、case-closure、reporting）
- [ ] 完成簡訊／電訪紀錄後，資料集與 Power BI 報表可反映相同語意（follow-up-logging、sms-fetnet、reporting、system-integrations）（Power BI 側待院方）
- [x] 個管師可自個案連結 HIS 查詢並於追蹤紀錄註記（system-integrations、follow-up-logging、rbac）（HIS 為模板連結＋註記欄位）
- [x] 結案代碼與統計口徑一致（case-closure、reporting）
