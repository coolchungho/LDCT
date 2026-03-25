# system-integrations — 系統整合邊界

## ADDED Requirements

### Requirement: HIS 整合（HIS Integration）

系統應（SHALL）界定與 HIS 之整合範圍，並使**個案管理師**得於本系統內**連結或開啟 HIS**（或院方約定之整合方式，例如單一登入 SSO、深層連結、新視窗開啟院內 HIS 網址），以**查詢並確認**與個案相關之資訊（含門診紀錄、手術紀錄等院方開放範圍）。查詢結果之**摘要或註記**得登錄於本平台（與 `follow-up-logging`）；HIS 內之完整內容以 HIS 為權威來源。

#### Scenario: 個管師自個案連結 HIS

- **GIVEN** 使用者具個案管理師權限且正在檢視某筆個案
- **WHEN** 選擇「連結 HIS」「開啟 HIS 查詢」或同等操作
- **THEN** 系統應（SHALL）依院方設定導向 HIS 或開啟授權之 HIS 連結，並於可行範圍內帶入病歷號或院方約定之病人識別參數，供個管師確認相關就醫資訊

#### Scenario: 無 SSO 或連結不可用

- **GIVEN** 院方尚未提供 SSO 或連結暫時失效
- **WHEN** 個管師嘗試連結 HIS
- **THEN** 系統應（SHALL）顯示明確提示或替代流程（例如手動註記已於 HIS 查詢），且不洩漏其他病人資料

#### Scenario: 不強制雙向寫回

- **WHEN** 個管師於本平台更新追蹤或結案
- **THEN** 不強制自動寫回 HIS（除非院方另有介面專案，以 MODIFIED 補充）

---

### Requirement: EHR LDCT 報告整合（EHR LDCT Report Integration）

系統應（SHALL）以 **EHR** 作為 LDCT 報告欄位之**主取得來源**（與 `data-sources` 一致）；介面型態可為 API、批次匯出檔或院方約定之排程同步。影像原始資料若存於 RIS／PACS，由院方經 EHR 彙整或介接後提供予本平台，**本平台不強制**另行直接串接 RIS／PACS 或 Power BI（除非院方另以變更提案要求直接連線）。

#### Scenario: 報告來源可追溯

- **WHEN** LDCT 報告資料自 EHR 進入本平台
- **THEN** 應可辨識資料批次、介面版本或來源類型，供稽核

#### Scenario: 欄位對應

- **WHEN** EHR 介面或匯出格式更新
- **THEN** 欄位對應表可版本化，避免欄位變更導致錯誤對應

---

### Requirement: LLM 服務整合（LLM Service Integration）

系統應（SHALL）整合院方核准之 **LLM 推理服務**（地端部署、私有雲或經資安評估之外部 API），用於自 LDCT 報告內容擷取結構化欄位（見 `data-sources` 之「LDCT 報告欄位 LLM 擷取」）。傳輸內容、日誌留存、是否允許上雲、以及是否禁止用於模型訓練等，必須（MUST）符合院方資安與個資政策（細節見 `design.md`）。

#### Scenario: 服務可替換與版本紀錄

- **WHEN** 更換 LLM 供應商或模型版本
- **THEN** 系統應（SHALL）可紀錄所使用之模型／版本識別，供稽核與重現爭議案件之擷取結果

#### Scenario: 失敗與重試

- **WHEN** LLM 服務逾時或回傳錯誤
- **THEN** 系統應（SHALL）記錄失敗並支援依院方政策之重試或改為純人工輸入路徑，不應無提示地寫入空值為有效結論

---

### Requirement: 問卷系統整合（Survey System Integration）

系統應（SHALL）整合健檢問卷系統，取得肺癌風險評估問卷結果並與個案關聯。

#### Scenario: 問卷與個案鍵一致

- **WHEN** 問卷資料進入
- **THEN** 與病歷號或院方約定之唯一鍵正確關聯

---

### Requirement: 簡訊平台整合（SMS Platform Integration）

系統應（SHALL）整合遠傳公務機簡訊平台（`https://umc.fetnet.net`）作為唯一指定之院方簡訊出口（若院方另增備援通路，以 MODIFIED 補充）。

#### Scenario: 與 sms-fetnet 一致

- **WHEN** 發送追蹤簡訊
- **THEN** 經由該整合發送並記錄（見 `sms-fetnet`）

---

### Requirement: Power BI 報表與視覺化（Power BI Analytics）

系統應（SHALL）以提供**報表資料集**予 **Microsoft Power BI** 作為統計與圖表之主要實作方式（與 `reporting`）；視覺化、儀表板版面與發佈流程於 **Power BI 服務／Desktop** 完成。此整合與「自 EHR 取得 LDCT 報告」無涉；Power BI 於本專案之角色為**分析與報表消費端**，而非 LDCT 報告匯入來源。

#### Scenario: 資料集可供建模

- **WHEN** 資訊人員於 Power BI 建立與本平臺資料集之連線
- **THEN** 可依 `reporting` 所列維度建立量值與視覺物件

#### Scenario: 權限與閘道

- **WHEN** 連線需經組織網路或資料閘道
- **THEN** 應遵循院方 IT 對 Power BI 之身分、資料閘道與工作區權限設定（細節見 `design.md`）
