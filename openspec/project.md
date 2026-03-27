# LDCT 肺癌篩檢個案管理 — 專案脈絡

## 專案簡述

本專案為 **LDCT（低劑量肺部電腦斷層）篩檢個案追蹤管理平台**，協助個案管理師整合健檢名單、影像報告、問卷與追蹤紀錄，並支援簡訊提醒、電訪紀錄、結案分類與統計報表。

## 利害關係人

- 個案管理師：追蹤、電訪、結案、確診資料登錄、**自本系統連結 HIS 確認相關就醫資訊**
- 護理師：肺癌風險評估問卷（平板）
- 醫師：報告判讀（與外部報告流程銜接）
- 管理者：統計與報表

## 技術架構

| 層級 | 決策 |
|------|------|
| **專案／雛型** | **單一解決方案或儲存庫**內 **前後端分離**：**ASP.NET Core Web API**（後端）＋ **Vue 3** 前端（**TypeScript**）各自獨立子專／建置產物，以 HTTP API 整合（CORS、驗證、Cookie／JWT 等於實作與院方資安定案）。 |
| **Web 前端** | **Vue 3**、**TypeScript**（建置管線例如 **Vite**，於實作定案）。 |
| **應用後端** | **ASP.NET Core**（**Web API**）；業務邏輯、排程匯入、**SQL Server** 存取及外部整合（EHR、簡訊等）由此層處理。 |
| **資料庫** | **Microsoft SQL Server**（營運資料、稽核、報表資料集來源等）。 |
| **週邊整合** | **LLM**（LDCT 報告欄位擷取）、**EHR**、**HIS** 連結、**Power BI**、簡訊 API 等，介接型態見變更內 `design.md`；LLM 服務型態須經院方資安核定。 |
| **部署目標** | **Kubernetes（K8s）**（建議後端、前端各一 **Deployment**／**Service**，**Ingress**、TLS、**Secret**／ConfigMap 與 SQL 連線等與院方 IT 定案）。 |
| **備援** | 叢集拓撲、備援與 **RPO／RTO** — **TBD**（實作前與院方 IT 定案）。 |

## 儲存庫實作目錄（雛型）

| 路徑 | 說明 |
|------|------|
| `src/Api` | ASP.NET Core 8 Web API、EF Core、`/api/v1/*`、`/health` |
| `src/Web` | Vue 3 + TypeScript（Vite）；開發時 proxy 至本機 API |
| `LDCT.sln` | 方案檔（現含後端專案；前端以 `npm` 建置） |
| `docker-compose.yml` | 雛型：**SQL Server 2022** + **API** 映像 |
| `k8s/*.yaml` | 範例：**Deployment**、**Service**、**Ingress**、**PDB**、**HPA**、**Secret** 示意 |
| `.github/workflows/ci.yml` | CI：`dotnet build` + `npm ci` / `npm run build` |

**測試主機（雛型）**：`10.41.100.1` — 前端 `http://10.41.100.1:5173`、API `http://10.41.100.1:5003`（後端請用 `http-lan` 設定檔或 docker-compose 對外埠）；CORS 與 Vite 設定見 [計畫書.md](../計畫書.md) 第五節。

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
