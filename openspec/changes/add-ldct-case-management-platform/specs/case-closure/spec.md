# case-closure — 結案管理

## ADDED Requirements

### Requirement: 結案分類代碼（Closure Type Codes）

系統應（SHALL）支援下列結案分類，並以代碼儲存與顯示：

| 代碼 | 類型 |
|------|------|
| 0 | 失聯 |
| 1 | 外院追蹤 |
| 2 | 本院追蹤 |
| 3 | 複檢 LDCT |
| 4 | 確診程序 |
| 5 | 提醒追蹤／衛教 |
| 6 | 肺轉移或其他癌症 |

#### Scenario: 結案時必選分類

- **WHEN** 使用者執行結案操作
- **THEN** 必須（MUST）選擇上述代碼之一（或院方允許之「暫存」流程若另有規定，則以後續 MODIFIED 補充）

---

### Requirement: 結案狀態與清單一致性（Closure State Consistency）

系統應（SHALL）於個案結案後，更新「結案狀態」與結案代碼，並使該個案自「未結案追蹤清單」統計中排除（與 `follow-up-lists`、`case-list` 一致）。

#### Scenario: 結案後統計更新

- **GIVEN** 某追蹤清單含該未結案個案
- **WHEN** 該個案完成結案
- **THEN** 未結案筆數減一，且總清單顯示已結案與代碼對應之類型
