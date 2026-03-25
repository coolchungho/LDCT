# reporting — 統計與報表

## ADDED Requirements

### Requirement: 月報表（Monthly Report）

系統應（SHALL）產生月報表，至少包含：每月肺癌確診人數、良性腫瘤人數、追蹤完成率（定義應與院方一致，例如：某期間內應追蹤個案中已完成追蹤或結案之比例）。

#### Scenario: 選擇月份匯出

- **WHEN** 具權限使用者選擇年月並產生月報
- **THEN** 顯示或匯出上述指標

---

### Requirement: 管理報表（Management Dashboard Report）

系統應（SHALL）產生管理報表，至少包含：追蹤中個案數、結案個案數、依不同追蹤類型之統計。

#### Scenario: 即時或準即時統計

- **WHEN** 管理者開啟管理報表
- **THEN** 數字與個案清單篩選結果在相同條件下一致（允許快取延遲時須標示，實作明確化）

---

### Requirement: 報表匯出（Export）

系統應（SHALL）支援將月報與管理報表匯出為院方常用格式（例如 PDF／Excel，**TBD** 於實作選定）。

#### Scenario: 匯出檔下載

- **WHEN** 使用者選擇匯出
- **THEN** 產生可下載之檔案且內容與畫面統計一致
