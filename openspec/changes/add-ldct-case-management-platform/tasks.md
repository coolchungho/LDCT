# 實作與驗收任務 — `add-ldct-case-management-platform`

## OpenSpec 工具（本機）

- [ ] 若已安裝 OpenSpec CLI：執行 `openspec validate add-ldct-case-management-platform --strict`
- [ ] 功能上線並確認行為符合 spec 後：執行 `openspec archive add-ldct-case-management-platform --yes`，合併至 `openspec/specs/`

## 初始化與文件

- [x] 建立 `openspec/project.md`、`openspec/AGENTS.md`
- [x] 建立本 change 之 `proposal.md`、`design.md`、各 capability 之 `specs/.../spec.md`
- [ ] 實作程式後更新 [計畫書.md](../../../計畫書.md) 對應章節（與本變更一致）

## 依 capability 與 Requirement 對照（驗收勾選）

### data-sources（[`specs/data-sources/spec.md`](specs/data-sources/spec.md)）

- [ ] **健檢名單匯入（Health Screening Roster Import）** — 欄位、匯入、錯誤處理 Scenario
- [ ] **LDCT 影像報告匯入（LDCT Report Import）** — 關聯、無對應個案 Scenario
- [ ] **LDCT 報告欄位 LLM 擷取（LLM Field Extraction from LDCT Report）** — 非結構化報告、人工覆核、低信心 Scenario
- [ ] **個管師手動輸入（Case Manager Manual Entry）** — 儲存與稽核 Scenario
- [ ] **問卷資料整合（Questionnaire Data）** — 關聯 Scenario
- [ ] **確診資料登錄（Diagnosis Data Entry）** — 欄位與驗證 Scenario

### case-list（[`specs/case-list/spec.md`](specs/case-list/spec.md)）

- [ ] **LDCT 個案總清單（Master Case List）** — 欄位顯示 Scenario
- [ ] **查詢與日期篩選（Search and Filter）** — 篩選 Scenario
- [ ] **檢視報告（View Report）** — 開啟報告 Scenario
- [ ] **加入追蹤（Add to Follow-up）** — 清單更新 Scenario
- [ ] **結案操作（Initiate Closure from List）** — 結案後狀態 Scenario

### follow-up-lists（[`specs/follow-up-lists/spec.md`](specs/follow-up-lists/spec.md)）

- [ ] **追蹤名單自動分類（Automated Follow-up Buckets）** — 分類 Scenario
- [ ] **依追蹤日期排序（Sort by Follow-up Date）** — 排序 Scenario
- [ ] **清單與未結案筆數（Counts）** — 筆數一致性 Scenario

### follow-up-workflow（[`specs/follow-up-workflow/spec.md`](specs/follow-up-workflow/spec.md)）

- [ ] **胸腔門診與 3 個月追蹤流程（Chest OP and 3-Month Track）** — 簡訊／電訪／結案 Scenario
- [ ] **6 個月追蹤流程（6-Month Track）** — 簡訊後電訪 Scenario
- [ ] **1 年追蹤與結案（12-Month Track）** — 無異常結案 Scenario
- [ ] **流程階段可視化（Workflow Progress）** — 進度 Scenario

### follow-up-logging（[`specs/follow-up-logging/spec.md`](specs/follow-up-logging/spec.md)）

- [ ] **追蹤事項紀錄（Follow-up Activity Log）** — 新增紀錄 Scenario
- [ ] **紀錄序與列表（Ordered Log and Table）** — 次數／類型／日期／內容 Scenario
- [ ] **HIS 就醫紀錄查詢關聯（HIS Visit Reference）** — 註記、與 HIS 連結銜接 Scenario

### sms-fetnet（[`specs/sms-fetnet/spec.md`](specs/sms-fetnet/spec.md)）

- [ ] **遠傳公務機簡訊平台整合（FET Enterprise SMS Integration）** — 成功／失敗 Scenario
- [ ] **批次發送簡訊（Batch SMS）** — 批次選取 Scenario
- [ ] **發送紀錄（SMS Audit Log）** — 稽核 Scenario
- [ ] **簡訊內容範例（SMS Content Template）** — 範本 Scenario

### case-closure（[`specs/case-closure/spec.md`](specs/case-closure/spec.md)）

- [ ] **結案分類代碼（Closure Type Codes）** — 0–6 必選 Scenario
- [ ] **結案狀態與清單一致性（Closure State Consistency）** — 統計更新 Scenario

### clinical-alerts（[`specs/clinical-alerts/spec.md`](specs/clinical-alerts/spec.md)）

- [ ] **病理報告關鍵字提醒（Pathology Keyword Alert）** — 關鍵字觸發 Scenario
- [ ] **肺部切片檢體結果提醒（Biopsy Result Alert）** — 待追蹤 Scenario
- [ ] **尚未追蹤個案提醒（Overdue Follow-up Alert）** — 逾期列示 Scenario

### reporting（[`specs/reporting/spec.md`](specs/reporting/spec.md)）

- [ ] **報表資料集供應（Reporting Dataset Provisioning）** — 授權、語意一致 Scenario
- [ ] **資料集涵蓋之分析維度（Dataset Coverage for Analytics）** — 月報／管理維度、與清單一致 Scenario
- [ ] **Power BI 連線與資料重新整理（Power BI Connectivity and Refresh）** — 連線文件、失敗追查 Scenario
- [ ] **本平台內建報表之定位（Optional In-App Reporting）** — 與 Power BI 不矛盾 Scenario

### rbac（[`specs/rbac/spec.md`](specs/rbac/spec.md)）

- [ ] **個案管理師角色（Case Manager Role）** — 操作、連結 HIS Scenario
- [ ] **護理師角色（Nurse Role）** — 問卷 Scenario
- [ ] **醫師角色（Physician Role）** — 判讀 Scenario
- [ ] **管理者角色（Administrator Role）** — 資料集／Power BI 報表 Scenario
- [ ] **身分驗證與授權（Authentication and Authorization）** — 未授權 Scenario

### system-integrations（[`specs/system-integrations/spec.md`](specs/system-integrations/spec.md)）

- [ ] **HIS 整合（HIS Integration）** — 個管自個案連結 HIS、無 SSO／失敗提示、非強制寫回 Scenario
- [ ] **EHR LDCT 報告整合（EHR LDCT Report Integration）** — 可追溯、欄位對應 Scenario
- [ ] **LLM 服務整合（LLM Service Integration）** — 模型版本、失敗重試、資安 Scenario
- [ ] **問卷系統整合（Survey System Integration）** — 鍵一致 Scenario
- [ ] **簡訊平台整合（SMS Platform Integration）** — 與 sms-fetnet 一致 Scenario
- [ ] **Power BI 報表與視覺化（Power BI Analytics）** — 建模、權限與閘道 Scenario

## 端到端驗收（跨多 Scenario）

- [ ] 匯入健檢＋報告後（含 LLM 擷取非結構化報告若適用），個案出現於總清單且追蹤分類正確（data-sources、case-list、follow-up-lists）
- [ ] 追蹤清單筆數與未結案數與實際一致（follow-up-lists、case-closure）
- [ ] 完成一輪簡訊／電訪紀錄後，資料集與 Power BI 報表可反映相同語意（follow-up-workflow、follow-up-logging、sms-fetnet、reporting、system-integrations）
- [ ] 個管師可自個案連結 HIS 查詢並於追蹤紀錄註記（system-integrations、follow-up-logging、rbac）
- [ ] 結案代碼與統計口徑一致（case-closure、reporting）
