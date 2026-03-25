# rbac — 系統角色與權限

## ADDED Requirements

### Requirement: 個案管理師角色（Case Manager Role）

系統應（SHALL）提供「個案管理師」角色，得執行：追蹤作業、電訪與聯繫紀錄、結案、確診資料登錄、簡訊觸發（依院方授權）、**自本系統連結 HIS 以確認相關就醫資訊**（與 `system-integrations`）、檢視報表（若院方限制僅管理者可看報表，則個管師不含匯出，以實作與 MODIFIED 補充）。

#### Scenario: 個管師可操作追蹤與結案

- **GIVEN** 使用者具個案管理師身分
- **WHEN** 登入系統
- **THEN** 可使用追蹤清單、追蹤紀錄、結案功能，且無法執行超出角色之管理功能（如刪除全庫資料，若定義）

#### Scenario: 個管師可連結 HIS

- **GIVEN** 使用者具個案管理師身分且院方已啟用 HIS 連結
- **WHEN** 於個案相關畫面操作 HIS 連結
- **THEN** 得依 `system-integrations` 開啟或導向 HIS 以確認資訊

---

### Requirement: 護理師角色（Nurse Role）

系統應（SHALL）提供「護理師」角色，得執行肺癌風險評估問卷之相關填寫或與問卷系統同步後之檢視（依實際：平板表單在本系統或僅同步結果）。

#### Scenario: 護理師填寫問卷

- **WHEN** 護理師於授權範圍內操作問卷功能
- **THEN** 可送出並與個案關聯（與 `data-sources` 之問卷整合一致）

---

### Requirement: 醫師角色（Physician Role）

系統應（SHALL）提供「醫師」角色，得執行報告判讀相關功能（例如檢視影像報告、補充判讀說明，依院方流程）。

#### Scenario: 醫師檢視與補充

- **WHEN** 醫師開啟個案報告與判讀欄位
- **THEN** 可檢視並於權限內編輯判讀補充（若僅個管可編，則醫師唯讀，實作明確化）

---

### Requirement: 管理者角色（Administrator Role）

系統應（SHALL）提供「管理者」角色，得執行統計分析、報表產出與匯出，以及使用者／角色管理（若本系統含後台）。

#### Scenario: 管理者存取報表

- **WHEN** 管理者開啟統計與報表模組
- **THEN** 可取得 `reporting` 所定義之月報與管理報表

---

### Requirement: 身分驗證與授權（Authentication and Authorization）

系統必須（MUST）在存取個資與醫療資料前驗證使用者身分，並依角色授權；未授權之操作應拒絕並記錄（稽核粒度依院方政策）。

#### Scenario: 未授權存取

- **WHEN** 無角色或越權使用者嘗試開啟個案資料
- **THEN** 系統拒絕並不洩漏資料內容
