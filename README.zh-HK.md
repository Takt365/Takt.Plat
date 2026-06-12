# Takt.Plat

[English](README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [繁體中文（香港）](README.zh-HK.md)

節拍數字工廠 · 前後端分離企業平台（.NET 9 + Vue 3）。

> ⚠️ **重要聲明**：本項目為 AI 智能生成（使用 Cursor AI 等 AI 輔助開發工具），代碼由 AI 自動生成並優化。
>
> 🚫 **不接受任何 Issue**：由於本項目是 AI 生成項目，我們不接受任何形式的 Issue、Bug 報告或功能請求。如有需要，請 Fork 後自行修改。

---

## 項目概覽

| 指標 | 數量 | 說明 |
|------|------|------|
| 領域實體 | 190 | `backend/src/Takt.Domain/Entities/` |
| API 控制器 | 190 | `backend/src/Takt.WebApi/Controllers/` |
| 前端 CRUD 頁面 | 199 | `frontend/src/views/**/index.vue` |
| 支援語言 | 3 | `zh-CN` / `en-US` / `ja-JP` |

覆蓋 **身份與權限、基礎平台、人力資源、物流製造、財務會計、日常辦公、工作流、統計日誌、代碼生成** 等業務域，採用 DDD 分層 + 多租戶分庫 + 公司級數據隔離。

---

## 技術棧

### 後端

| 類別 | 選型 |
|------|------|
| 運行時 | .NET 9 |
| ORM | SqlSugar 5.x（業務庫）+ EF Core（OpenIddict 認證庫） |
| 資料庫 | SQL Server（`Database:DbType = 1`） |
| DI | Autofac（應用服務 / 驗證器自動掃描註冊） |
| 認證 | OpenIddict 7.x（OAuth 2.0 / OIDC） |
| 驗證 | FluentValidation |
| 日誌 | Serilog（控制台 + 分級檔案） |
| API 文件 | Scalar（開發環境 `/scalar`） |
| 實時通訊 | SignalR（雙 Hub） |

### 前端

| 類別 | 選型 |
|------|------|
| 構建 | Vite 8 |
| 框架 | Vue 3 Composition API + TypeScript |
| UI | Ant Design Vue 4.x |
| 樣式 | Tailwind CSS 4.x |
| 狀態 / 路由 / 請求 | Pinia、vue-router 5、Axios |
| 國際化 | vue-i18n（靜態 `locales/**` + 後端動態種子） |
| 工作流設計 | LogicFlow + `@form-create/ant-design-vue` |
| 圖表 | ECharts 6 |
| 實時通訊 | `@microsoft/signalr` |
| PWA | vite-plugin-pwa（生產默認可用） |

---

## 架構分層

```
Takt.WebApi          → 控制器、Program.cs、OpenIddict、中介軟件
Takt.Application     → DTO、應用服務、FluentValidation 驗證器
Takt.Infrastructure  → 倉儲、種子、SignalR、緩存、多庫映射
Takt.Domain          → 實體、倉儲介面
Takt.Shared          → 分頁、異常、Options、工具類
```

依賴方向：`WebApi → Application + Infrastructure + Shared`；`Application → Domain + Shared`；`Domain → Shared`。控制器僅注入 `ITaktXxxService`，禁止直接存取 SqlSugar。

---

## 平台能力

### 租戶 → 公司 → 業務數據

```
租戶 Tenant（按租戶分庫） → 公司 Company（公司代碼） → 業務數據（部門/員工/製造/銷售等）
```

| 層級 | 後端 | 前端 |
|------|------|------|
| **租戶** | 獨立業務庫 `ConnectionStrings:Tenant_{code}`；`TaktTenant`；`TaktTenantEntityBase`（用戶/角色/選單/字典等） | `useTenantStore`、`takt-tenant-toggle`；登入前後均可選租戶 |
| **公司** | `TaktCompany`；`TaktCompanyEntityBase`（員工/部門/製造/財務等）；倉儲 `Where(TenantCode, CompanyCode)` | `takt-company-toggle`；與租戶聯動切換可存取公司 |
| **貫通** | `ITaktUserContext` + 請求頭 `X-Tenant-Code` / `X-Company-Code` | `api/request.ts` 自動附加頭 |

**啟動 Init 開關**（`appsettings.json` → `Init` / `Database`）：

1. `InitDb` — 按 `TenantCodes` 順序建各租戶業務庫表
2. `SeedData` — 按 `TenantCodes` 順序切換租戶庫執行全部種子；公司主檔/假日/組織人事等由種子內按 `CompanyCodes` 順序寫入；工廠主檔按 `PlantCodes` 順序
3. `CompanyCodes` / `PlantCodes` 列表順序須與種子數據定義一致；`CompanyCodes` 首項仍作為演示帳號主公司等默認歸屬（`GetSeedCompanyCode()`）

### 全局 SignalR

登入後維持 **雙 Hub**（`AddTaktSignalR`，JWT 經 `TaktSignalRTokenMiddleware` 鑑權）：

| Hub | 路徑 | 職責 |
|-----|------|------|
| **TaktConnectHub** | `/hubs/TaktConnectHub` | 連接/斷開、在線用戶、在線統計、強退 `ForceLogout` |
| **TaktNotificationHub** | `/hubs/TaktNotificationHub` | 私信、公司內廣播、訊息/在線統計推送 |

推送按 **公司 + 用戶** 分組（`TaktSignalRGroupNames`），與租戶/公司業務隔離一致。調度入口：`ITaktSignalRDispatchService`。

前端：`utils/takt-signalr.ts`；`stores/foundation/signalr.ts`；佈局登入後自動連接。

### 其他能力

| 能力 | 說明 | 配置 / 入口 |
|------|------|-------------|
| **權限（RBAC）** | OpenIddict；選單權限碼 `領域:目錄:…:實體:操作`；`[TaktPermission]` | `OpenIddict`、`Takt.WebApi/Filters/` |
| **日誌** | Serilog 分級落盤；`TaktLoggingMiddleware`；`TaktLoginLog` / `TaktOperLog` / `TaktDeltaLog` | `TaktLogging`、`Serilog` |
| **本地化** | 庫表 + I18n 種子；前端靜態 `locales/**` + 動態 `mergeDynamicLocaleMessages` | `Localization`、`Infrastructure/Data/Seeds/I18nSeedData/` |
| **驗證碼** | `Slider` / `Behavior` | `Captcha`、`ITaktCaptchaService` |
| **緩存** | `ITaktCacheService`（Memory / Redis） | `Cache:Provider` |
| **安全** | 限流、CSRF、XSS、RSA 密碼傳輸 | `Security`、`PasswordPolicy:Transport` |
| **工作流** | 流程方案/實例/任務/表單/變量/加簽 | `Workflow/` 實體 + LogicFlow 設計器 |
| **代碼生成** | 實體 → DTO/服務/控制器/前端/i18n 一鍵流水線 | `scripts/generate-all.cjs` |

前端：`v-permission`、驗證碼（`takt-captcha-slider` / `takt-captcha-behavior`）、`takt-modal` 標準 CRUD 彈窗。

---

## 業務模組

後端 `Controllers/` 與 `Domain/Entities/` 按相同領域劃分；前端 `views/`、`api/`、`types/`、`locales/` 與之對齊。

| 領域 | 子模組 | 主要能力 |
|------|--------|----------|
| **Identity** | 用戶、角色、選單、租戶、RBAC、認證 | 登入鑑權、權限分配、多租戶/多公司授權 |
| **Foundation** | 字典、翻譯、設定、編號、訊息、在線、文化 | 平台基礎數據、站內訊息、動態 i18n |
| **HumanResource** | Organization（部門/崗位）、Personnel（員工）、Attendance（節假日）、Talent | 組織架構、人事主檔 |
| **Logistics · Materials** | 物料、供應商、廠商、採購申請/訂單/價格、工廠 | 採購與主數據 |
| **Logistics · Sales** | 客戶、銷售訂單/價格 | 銷售管理 |
| **Logistics · Quality** | Operation（IQC/IPQC/抽樣）、Complaint、Cost | 來料/製程檢驗、質量成本 |
| **Logistics · Manufacturing** | Bom、Scheduling（APS）、Output（PCBA/ASSY 產出）、Defect（檢驗/維修/不良）、EngineeringChange（ECN） | 製造執行、產出與不良 |
| **Logistics · Serial** | 產品序列號入出庫 | 序列號追溯 |
| **Logistics · Maintenance** | 設備、保養 | 設備維護 |
| **Accounting** | Financial（公司/科目/資產/會簽）、Controlling（成本/利潤中心） | 財務主檔 |
| **Routine** | Announcement、NewsCenter、HelpDesk、DocumentCenter、ConferenceCenter、VisitorCenter | 公告、新聞、工單、文件、會議、訪客 |
| **Workflow** | 流程方案、實例、任務、表單、變量、遷移、加簽 | 審批流引擎 |
| **Workflow · Engine** | `TaktFlowEngineController` + `ITaktFlowEngineService` | 運行時：發起/待辦/審批/加簽（與實例 CRUD 分離） |
| **Statistics** | Logging（登入/操作/變更日誌） | 審計與運維日誌 |
| **Code** | Generator（代碼生成表配置） | 在線代碼生成元數據 |

---

## 項目結構

```
Takt.Plat/
├── backend/
│   ├── Takt.Plat.slnx
│   └── src/
│       ├── Takt.Shared/           # 公共類型、Options、異常
│       ├── Takt.Domain/           # 實體、倉儲介面（190 實體）
│       ├── Takt.Application/      # DTO、應用服務、驗證器
│       ├── Takt.Infrastructure/   # 倉儲、種子、SignalR、中介軟件
│       └── Takt.WebApi/           # API 入口（Program.cs、Controllers）
├── frontend/
│   ├── src/
│   │   ├── api/                   # 按後端模組劃分 REST 客戶端
│   │   ├── types/                 # 與 api 一一對應的 TS 類型
│   │   ├── views/                 # 頁面（標準 CRUD 結構）
│   │   ├── components/            # common/ + business/（takt-modal 等）
│   │   ├── stores/                # Pinia（含 foundation/signalr）
│   │   ├── locales/               # 靜態 i18n（export default { page: … }）
│   │   ├── router/                # 懶加載路由
│   │   └── styles/                # global.css、主題 token
│   ├── vite.config.ts
│   └── package.json
├── scripts/                       # 代碼生成與實體維護腳本
│   ├── generate-all.cjs           # 一鍵：DTO → 服務 → 控制器 → 前端 → i18n
│   ├── generate-from-backend.cjs  # 前端 api/types 生成
│   ├── generate-vue-all-from-api.cjs  # 串聯 Vue 三模板（CRUD / TREE / Master-Detail）
│   ├── generate-vue-crud-from-api.cjs
│   ├── generate-vue-tree-from-api.cjs
│   └── generate-vue-master-detail-from-api.cjs
├── .cursor/rules/                 # 開發規範（00-project / 01-backend / 02-frontend）
├── LICENSE
├── README.md                        # English（默認）
├── README.zh-CN.md                  # 简体中文
├── README.ja-JP.md                  # 日本語
└── README.zh-HK.md                  # 繁體中文（香港）
```

---

## 編譯與運行

### 環境要求

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) LTS（前端）
- SQL Server（連接字串在本地 `appsettings*.json`，倉庫僅提供 `appsettings.*.Example.json` 模板）

### 後端

```bash
dotnet restore backend/Takt.Plat.slnx
dotnet build backend/Takt.Plat.slnx -c Release
dotnet run --project backend/src/Takt.WebApi/Takt.WebApi.csproj
```

| 項 | 值 |
|----|-----|
| HTTP | `http://localhost:60070` |
| HTTPS | `https://localhost:60071` |
| API 文件 | `https://localhost:60071/scalar` |

首次啟動前將 `backend/src/Takt.WebApi/appsettings.Example.json` 等 Example 檔案複製為 `appsettings.json` / `appsettings.Development.json` / `appsettings.Production.json`（勿提交 Git），並填寫 `ConnectionStrings`（OpenIddict + `Tenant_*`）。`Init:InitDb` / `Init:SeedData` 見 `appsettings.json`。

### 前端

```bash
cd frontend
cp env.example .env
cp env.development.example .env.development
cp env.production.example .env.production
npm install
npm run dev      # 開發（默認 https://localhost:60081）
npm run build    # 生產構建（vue-tsc + vite build）
```

（Windows 可用 `copy env.example .env` 等。`.env*` 已由 `.gitignore` 排除；Example 僅含 `<...>` 佔位符，不含真實域名/端口。本地慣例可參考：前端 `60081`、後端 HTTPS `60071`，寫入你自己的 `.env*` 即可。）

| 項 | 說明 |
|----|------|
| 開發地址 | `.env.development` 中 `VITE_APP_ORIGIN`（模板見 `env.development.example`，自行填端口） |
| API 代理 | `/api` → `VITE_API_PROXY_TARGET`（指向本機後端 HTTPS 根地址） |
| OAuth 回調 | `{VITE_APP_ORIGIN}/auth/callback`，須與後端 `OpenIddict:SpaRedirectUris` 一致 |

須與後端 `Cors`、`OpenIddict:SpaRedirectUris` 中的前端地址一致（見本地 `appsettings.Development.json`）。

### 默認帳號（種子）

種子用戶（各租戶）：`admin`（超級管理員）、`guest`、`demo`。初始密碼見 `PasswordPolicy:DefaultPassword`（默認 `Takt@123456`）。生產環境務必修改。

---

## 代碼生成

新增實體後，可在倉庫根目錄執行：

```bash
node scripts/generate-all.cjs              # 全量流水線
node scripts/generate-all.cjs --entity TaktXxx   # 單實體
cd frontend && npm run generate            # 僅生成前端 api/types
cd frontend && npm run generate:vue        # 僅生成 CRUD 頁面
```

生成規則與命名約定見 `.cursor/rules/00-project.mdc`（控制器複數、應用服務單數、前後端方法名對齊等）。

---

## 開發規範

倉庫內 `.cursor/rules/` 定義完整約定，摘要：

- **後端**：DDD 分層、控制器複數 / 服務單數、`GetXxxListAsync` 等方法命名、i18n 種子鍵 `entity.*` / `menu.*`
- **前端**：Ant Design Vue + Tailwind、`t('路徑.page.*')` 靜態翻譯、`v-permission` 權限碼與選單一致
- **格式**：禁止「隔行空行」，見 `03-format-blank-lines.mdc`

---

## 許可

[MIT License](LICENSE) · Copyright (c) 2026 Takt Technologies Co., Ltd.

**維護者**：Takt.Plat（Cursor AI 等）
