# follow-up-workflow — 追蹤流程管理

## ADDED Requirements

### Requirement: 胸腔門診與 3 個月追蹤流程（Chest OP and 3-Month Track）

針對歸類為「胸腔門診追蹤」或「3 個月追蹤」之個案，系統應（SHALL）支援下列追蹤流程階段：第一次簡訊提醒、第二次電訪、第三次電訪；並依狀況允許結案（結案代碼見 `case-closure`）。

#### Scenario: 第一次階段為簡訊

- **GIVEN** 個案適用胸腔門診或 3 個月追蹤流程
- **WHEN** 進入第一次追蹤
- **THEN** 系統應（SHALL）以「簡訊」為對應之第一次動作類型，並可記錄完成（與 `follow-up-logging`、`sms-fetnet` 搭配）

#### Scenario: 第二、三次為電訪

- **WHEN** 進入第二、三次追蹤
- **THEN** 系統應（SHALL）以「電訪」為對應動作類型，並可記錄完成（與 `follow-up-logging` 搭配）

#### Scenario: 依狀況結案

- **WHEN** 個管師判定可結案
- **THEN** 系統應（SHALL）允許執行結案並停止該類型之未結案追蹤要求（與 `case-closure` 一致）

---

### Requirement: 6 個月追蹤流程（6-Month Track）

針對「6 個月追蹤」個案，系統應（SHALL）支援流程：第一次簡訊提醒、第二次電訪。

#### Scenario: 簡訊後電訪

- **GIVEN** 個案適用 6 個月追蹤流程
- **WHEN** 依序完成第一次與第二次
- **THEN** 第一次類型為簡訊、第二次類型為電訪，並可記錄於追蹤紀錄

---

### Requirement: 1 年追蹤與結案（12-Month Track）

針對「12 個月追蹤」個案，系統應（SHALL）支援：若無異常，個管師可直接結案（或依院方定義之「無需進一步追蹤」條件）。

#### Scenario: 無異常直接結案

- **GIVEN** 個案之 1 年追蹤經評估無異常
- **WHEN** 個管師執行結案
- **THEN** 系統應（SHALL）完成結案並更新結案狀態，且不強制要求額外簡訊／電訪次數（若院方另有強制步驟，以 MODIFIED 變更補充）

---

### Requirement: 流程階段可視化（Workflow Progress）

系統應（SHALL）對於適用上述流程之個案，顯示目前進度（例如第幾次、下一步建議動作），便於個管師操作。

#### Scenario: 檢視個案進度

- **WHEN** 使用者開啟個案之追蹤流程畫面
- **THEN** 系統顯示已完成與待辦之階段（簡訊／電訪）
