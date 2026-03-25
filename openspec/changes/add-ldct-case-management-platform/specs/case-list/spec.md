# case-list — 個案清單管理

## ADDED Requirements

### Requirement: LDCT 個案總清單（Master Case List）

系統應（SHALL）提供 LDCT 個案總清單，並至少顯示下列欄位：病歷號、姓名、檢查日期、客戶名稱、追蹤類型、胸腔門診、3 months、6 months、12 months、LDCT 狀態、結案狀態。

#### Scenario: 清單載入

- **WHEN** 具權限使用者開啟個案總清單
- **THEN** 系統顯示符合權限範圍內之個案與上述欄位

---

### Requirement: 查詢與日期篩選（Search and Filter）

系統應（SHALL）支援依條件查詢個案，並支援依日期範圍篩選（例如檢查日期或追蹤日期，依院方定義之主要篩選欄位）。

#### Scenario: 篩選縮小結果

- **GIVEN** 清單中存在多筆個案
- **WHEN** 使用者設定日期範圍並套用篩選
- **THEN** 清單僅顯示該範圍內之個案

---

### Requirement: 檢視報告（View Report）

系統應（SHALL）允許使用者自個案清單檢視與該個案關聯之 LDCT 報告內容或連結（若完整影像或報告全文存於 EHR 或外部影像系統，至少提供摘要與外部連結或院方約定之開啟方式）。

#### Scenario: 開啟個案報告

- **WHEN** 使用者於清單對某筆個案選擇「查看報告」或同等操作
- **THEN** 系統顯示已匯入之報告欄位或導向約定之報告檢視方式

---

### Requirement: 加入追蹤（Add to Follow-up）

系統應（SHALL）支援將個案納入適當之追蹤流程或追蹤類型（與 `follow-up-workflow`、`follow-up-lists` 一致），並反映於清單之追蹤類型與相關欄位。

#### Scenario: 納入追蹤後清單更新

- **WHEN** 使用者對個案執行「加入追蹤」且儲存
- **THEN** 該個案出現於對應追蹤清單（未結案前提下），且總清單之追蹤相關欄位更新

---

### Requirement: 結案操作（Initiate Closure from List）

系統應（SHALL）允許自個案清單觸發結案流程（詳見 `case-closure`），並更新結案狀態顯示。

#### Scenario: 結案後自清單追蹤視角排除或標示

- **WHEN** 個案完成結案
- **THEN** 總清單顯示結案狀態；未結案追蹤清單之統計應排除已結案個案（與 `follow-up-lists` 一致）
