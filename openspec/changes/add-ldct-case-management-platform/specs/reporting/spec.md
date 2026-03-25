# reporting — 統計與報表

## ADDED Requirements

### Requirement: 報表資料集供應（Reporting Dataset Provisioning）

系統應（SHALL）提供可供 **Microsoft Power BI** 使用之**資料集**（結構化資料出口），使院方得以於 **Power BI** 中完成統計報表、圖表與儀表板之**視覺化與版面設計**；本平台以**資料集／連線能力**為主，**不強制**於本系統內建完整圖表報表畫面。資料出口型態可為：唯讀檢視、API、排程檔案匯出、或經資安核准之資料庫／閘道連線（細節見 `design.md`）。

#### Scenario: 管理者或授權人員可取得資料集

- **GIVEN** 使用者具存取報表資料之角色權限（見 `rbac`）
- **WHEN** 依院方設定連線或下載資料集
- **THEN** 可於 Power BI Desktop 或 Power BI 服務中建立語意模型並製作報表

#### Scenario: 資料集與業務定義一致

- **WHEN** 於 Power BI 中依資料集計算指標
- **THEN** 指標定義應與本 change 之個案、追蹤、結案狀態語意一致（欄位字典與計算邏輯見 `design.md` 或附錄）

---

### Requirement: 資料集涵蓋之分析維度（Dataset Coverage for Analytics）

資料集應（SHALL）包含或可追溯之資訊足以支援院方在 Power BI 中產出至少下列分析（得為明細表加關聯，由 DAX／Power Query 彙總）：

- **月報維度**：每月肺癌確診人數、良性腫瘤人數、追蹤完成率（定義應與院方一致，例如某期間內應追蹤個案中已完成追蹤或結案之比例）。
- **管理維度**：追蹤中個案數、結案個案數、依不同追蹤類型之統計。

#### Scenario: 月報指標可從資料集計算

- **WHEN** 資料集已依約定頻率更新
- **THEN** 授權使用者可於 Power BI 依月份篩選並計算上述月報相關指標

#### Scenario: 管理指標與清單邏輯一致

- **GIVEN** 本平台個案清單與追蹤清單之篩選條件已定義
- **WHEN** 於 Power BI 使用相同語意之欄位與篩選
- **THEN** 匯總數字應與本平台清單計數在相同定義下一致（允許合理之刷新延遲時須於 `design.md` 標示）

---

### Requirement: Power BI 連線與資料重新整理（Power BI Connectivity and Refresh）

系統應（SHALL）支援院方核准之 Power BI 連線與重新整理方式（例如：內部部署資料閘道、Import 模式之排程重新整理、或 DirectQuery／即時連線若院方基礎建設支援），並提供或文件化：**連線資訊、認證方式、建議重新整理頻率、以及資料延遲假設**。

#### Scenario: 重新整理失敗可追查

- **WHEN** 資料匯出或連線供應失敗（若由本平台產出檔案或 API）
- **THEN** 應有紀錄或監控供資訊人員追查（粒度依實作）

---

### Requirement: 本平台內建報表之定位（Optional In-App Reporting）

若本平台另提供簡易清單匯出或數字摘要，系統應（SHALL）確保其與**報表資料集**之欄位定義不互相矛盾；**正式統計與對外月報以 Power BI 產出物為院方約定之呈現管道時**，以院方文件為準。

#### Scenario: 不取代 Power BI 為唯一真相之設定

- **GIVEN** 院方已指定以 Power BI 為管理報表與月報之正式管道
- **WHEN** 使用者同時參考本平台畫面與 Power BI
- **THEN** 差異僅來自已宣告之刷新延遲或篩選條件不同，而非隱含邏輯衝突
