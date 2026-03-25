# LDCT 肺癌篩檢個案管理 — 專案脈絡

## 專案簡述

本專案為 **LDCT（低劑量肺部電腦斷層）篩檢個案追蹤管理平台**，協助個案管理師整合健檢名單、影像報告、問卷與追蹤紀錄，並支援簡訊提醒、電訪紀錄、結案分類與統計報表。

## 利害關係人

- 個案管理師：追蹤、電訪、結案、確診資料登錄、**自本系統連結 HIS 確認相關就醫資訊**
- 護理師：肺癌風險評估問卷（平板）
- 醫師：報告判讀（與外部報告流程銜接）
- 管理者：統計與報表

## 技術架構

**TBD**（實作階段定案）：前後端應用、資料庫、部署拓撲與周邊服務介接（含 **LLM** 用於 LDCT 報告欄位擷取，服務型態須經院方資安核定，見變更內 `design.md`）。

## 法遵與資安（待院方資安／法遵補齊）

- 病歷號、姓名、聯絡方式等個資之存取控管、加密傳輸、稽核日誌保留期與範圍，應依院方政策與個資法落實；細節於實作前與資安確認後補入 `design.md` 或獨立資安附錄。
- 若 **LDCT 報告文字／影像描述** 傳送至 **LLM** 服務，須另行評估：是否允許上雲、資料留存、禁止用於模型訓練、以及去識別化或最小化傳輸等要求。

## 外部系統（摘要）

| 系統 | 用途 |
|------|------|
| HIS | 個管師由本平台**連結開啟**後查詢門診／手術紀錄等（權威資料在 HIS；本平台保存摘要／註記） |
| EHR | LDCT 報告之主取得來源（影像／報告全文可能由 RIS／PACS 彙整至 EHR） |
| LLM | 自報告敘述／文件擷取結構化欄位（與 EHR 銜接，見變更 spec） |
| Power BI | 統計報表與儀表板（連線本平台**報表資料集**；非 LDCT 報告匯入來源） |
| 健檢／健管名單 | 匯出檔匯入 |
| 健檢問卷系統 | 肺癌風險評估問卷 |
| 遠傳公務機簡訊平台（`https://umc.fetnet.net`） | 批次簡訊 |

## OpenSpec 目錄

- `openspec/specs/`：已上線能力之現況真理（變更 archive 後合併）
- `openspec/changes/<name>/`：進行中變更（proposal、tasks、specs Delta）

## 相關文件

- 計畫書（與本專案同步）：[計畫書.md](../計畫書.md)
- 系統分析：[docs/SA.md](../docs/SA.md)
- 系統設計：[docs/SD.md](../docs/SD.md)
- 變更提案：[changes/add-ldct-case-management-platform/proposal.md](changes/add-ldct-case-management-platform/proposal.md)
- 本機驗證（若已安裝 OpenSpec CLI）：`openspec validate add-ldct-case-management-platform --strict`
- 部署後合併：`openspec archive add-ldct-case-management-platform --yes`
