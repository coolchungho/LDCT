# clinical-alerts — 臨床提醒

## ADDED Requirements

### Requirement: 病理報告關鍵字提醒（Pathology Keyword Alert）

系統應（SHALL）於病理報告或相關文字欄位出現「惡性腫瘤」等院方定義之關鍵字時，對該個案產生或凸顯臨床提醒，供個管師優先處理。

#### Scenario: 關鍵字觸發

- **GIVEN** 個案之病理報告欄位已輸入含「惡性腫瘤」之文字
- **WHEN** 資料儲存
- **THEN** 系統顯示提醒狀態或進入提醒清單（實作可為即時旗標或排程掃描，須於實作明確化）

---

### Requirement: 肺部切片檢體結果提醒（Biopsy Result Alert）

系統應（SHALL）對肺部切片檢體結果之登錄或匯入提供提醒機制（例如：結果到齊、需後續處置），細節規則依院方與 `data-sources` 欄位定義。

#### Scenario: 有待追蹤之切片結果

- **WHEN** 切片相關欄位顯示需追蹤或異常狀態
- **THEN** 個案應出現於提醒區域或具可篩選之提醒標記

---

### Requirement: 尚未追蹤個案提醒（Overdue Follow-up Alert）

系統應（SHALL）依追蹤日期與流程進度，對逾期未依節點完成追蹤之個案提供提醒（例如：已逾追蹤日仍未完成第一次簡訊或電訪）。

#### Scenario: 逾期列示

- **GIVEN** 個案已逾約定之追蹤日期且未完成應有之追蹤步驟
- **WHEN** 使用者檢視提醒或儀表
- **THEN** 該個案可被篩選或列於「尚未追蹤／逾期」清單
