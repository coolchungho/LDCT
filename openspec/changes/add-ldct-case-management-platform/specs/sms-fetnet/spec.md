# sms-fetnet — 簡訊通知（遠傳公務機）

## ADDED Requirements

### Requirement: 遠傳公務機簡訊平台整合（FET Enterprise SMS Integration）

系統應（SHALL）整合遠傳公務機簡訊平台（`https://umc.fetnet.net`）以發送簡訊；實際介面（API／閘道）依院方與電信商提供之方式實作。

#### Scenario: 發送成功

- **WHEN** 系統透過整合介面發送簡訊成功
- **THEN** 應寫入發送紀錄（見下則 Requirement）

#### Scenario: 發送失敗

- **WHEN** 發送失敗
- **THEN** 系統應（SHALL）記錄失敗原因與時間，供個管重試或改採其他聯繫方式

---

### Requirement: 批次發送簡訊（Batch SMS）

系統應（SHALL）支援自追蹤流程或清單中選取多筆個案，批次發送簡訊（受權限與範本規範約束）。

#### Scenario: 批次選取

- **GIVEN** 使用者於追蹤清單勾選多筆未結案個案
- **WHEN** 執行批次簡訊
- **THEN** 系統對每筆發送並逐筆記錄結果（成功／失敗）

---

### Requirement: 發送紀錄（SMS Audit Log）

系統應（SHALL）保存每次簡訊發送紀錄，至少包含：個案識別、發送時間、發送者或系統工作、簡訊內容或範本識別、結果狀態。

#### Scenario: 稽核與追蹤

- **WHEN** 管理者或個管師查詢發送紀錄
- **THEN** 可依個案或日期範圍檢索

---

### Requirement: 簡訊內容範例（SMS Content Template）

系統應（SHALL）支援使用與下列語意一致之提醒內容（可為可編輯範本，並帶入姓名／檢查／追蹤期間等變數）：

「親愛的貴賓您好：您於新光醫院接受低劑量肺部電腦斷層檢查，請依醫師建議 3–6 個月追蹤。」

#### Scenario: 套用範本

- **WHEN** 使用者選擇對應範本並發送
- **THEN** 發送內容與範本語意一致，且變數替換正確
