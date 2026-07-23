# Takt.Plat

[English](README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [繁體中文（香港）](README.zh-HK.md)

节拍数字工厂 · 前后端分离企业平台（.NET 9 + Vue 3）。

> ⚠️ **重要声明**：本项目为 AI 智能生成（使用 Cursor AI 等 AI 辅助开发工具），代码由 AI 自动生成并优化。
>
> 🚫 **不接受任何 Issue**：由于本项目是 AI 生成项目，我们不接受任何形式的 Issue、Bug 报告或功能请求。如有需要，请 Fork 后自行修改。

---

## 项目概览

| 指标 | 数量 | 说明 |
|------|------|------|
| 领域实体 | 190 | `backend/src/Takt.Domain/Entities/` |
| API 控制器 | 190 | `backend/src/Takt.WebApi/Controllers/` |
| 前端 CRUD 页面 | 199 | `frontend/src/views/**/index.vue` |
| 支持语言 | 3 | `zh-CN` / `en-US` / `ja-JP` |

覆盖 **身份与权限、基础平台、人力资源、物流制造、财务会计、日常办公、工作流、统计日志、代码生成** 等业务域，采用 DDD 分层 + 多租户分库 + 公司级数据隔离。

---

## 技术栈

### 后端

| 类别 | 选型 |
|------|------|
| 运行时 | .NET 9 |
| ORM | SqlSugar 5.x（业务库）+ EF Core（OpenIddict 认证库） |
| 数据库 | SQL Server（`Database:DbType = 1`） |
| DI | Autofac（应用服务 / 验证器自动扫描注册） |
| 认证 | OpenIddict 7.x（OAuth 2.0 / OIDC） |
| 验证 | FluentValidation |
| 日志 | Serilog（控制台 + 分级文件） |
| API 文档 | Scalar（开发环境 `/scalar`） |
| 实时通信 | SignalR（双 Hub） |

### 前端

| 类别 | 选型 |
|------|------|
| 构建 | Vite 8 |
| 框架 | Vue 3 Composition API + TypeScript |
| UI | Ant Design Vue 4.x |
| 样式 | Tailwind CSS 4.x |
| 状态 / 路由 / 请求 | Pinia、vue-router 5、Axios |
| 国际化 | vue-i18n（静态 `locales/**` + 后端动态种子） |
| 工作流设计 | LogicFlow + `@form-create/ant-design-vue` |
| 图表 | ECharts 6 |
| 实时通信 | `@microsoft/signalr` |
| PWA | vite-plugin-pwa（生产默认可用） |

---

## 架构分层

```
Takt.WebApi          → 控制器、Program.cs、OpenIddict、中间件
Takt.Application     → DTO、应用服务、FluentValidation 验证器
Takt.Infrastructure  → 仓储、种子、SignalR、缓存、多库映射
Takt.Domain          → 实体、仓储接口
Takt.Shared          → 分页、异常、Options、工具类
```

依赖方向：`WebApi → Application + Infrastructure + Shared`；`Application → Domain + Shared`；`Domain → Shared`。控制器仅注入 `ITaktXxxService`，禁止直接访问 SqlSugar。

---

## 平台能力

### 租户 → 公司 → 业务数据

```
租户 Tenant（按租户分库） → 公司 Company（公司代码） → 业务数据（部门/员工/制造/销售等）
```

| 层级 | 后端 | 前端 |
|------|------|------|
| **租户** | 独立业务库 `ConnectionStrings:Tenant_{code}`；`TaktTenant`；`TaktTenantEntityBase`（用户/角色/菜单/字典等） | `useTenantStore`、`takt-tenant-toggle`；登录前后均可选租户 |
| **公司** | `TaktCompany`；`TaktCompanyEntityBase`（员工/部门/制造/财务等）；仓储 `Where(TenantCode, CompanyCode)` | `takt-company-toggle`；与租户联动切换可访问公司 |
| **贯通** | `ITaktUserContext` + 请求头 `X-Tenant-Code` / `X-Company-Code` | `api/request.ts` 自动附加头 |

**启动 Init 开关**（`appsettings.json` → `Init` / `Database`）：

1. `InitDb` — 按 `TenantCodes` 顺序建各租户业务库表
2. `SeedData` — 按 `TenantCodes` 顺序切换租户库执行全部种子；公司主档/假日/组织人事等由种子内按 `CompanyCodes` 顺序写入；工厂主档按 `PlantCodes` 顺序
3. `CompanyCodes` / `PlantCodes` 列表顺序须与种子数据定义一致；`CompanyCodes` 首项仍作为演示账号主公司等默认归属（`GetSeedCompanyCode()`）

### 全局 SignalR

登录后维持 **双 Hub**（`AddTaktSignalR`，JWT 经 `TaktSignalRTokenMiddleware` 鉴权）：

| Hub | 路径 | 职责 |
|-----|------|------|
| **TaktConnectHub** | `/hubs/TaktConnectHub` | 连接/断开、在线用户、在线统计、强退 `ForceLogout` |
| **TaktNotificationHub** | `/hubs/TaktNotificationHub` | 私信、公司内广播、消息/在线统计推送 |

推送按 **公司 + 用户** 分组（`TaktSignalRGroupNames`），与租户/公司业务隔离一致。调度入口：`ITaktSignalRDispatchService`。

前端：`utils/takt-signalr.ts`；`stores/foundation/signalr.ts`；布局登录后自动连接。

### 其他能力

| 能力 | 说明 | 配置 / 入口 |
|------|------|-------------|
| **权限（RBAC）** | OpenIddict；菜单权限码 `领域:目录:…:实体:操作`；`[TaktPermission]` | `OpenIddict`、`Takt.WebApi/Filters/` |
| **日志** | Serilog 分级落盘；`TaktLoggingMiddleware`；`TaktLoginLog` / `TaktOperLog` / `TaktDeltaLog` | `TaktLogging`、`Serilog` |
| **本地化** | 库表 + I18n 种子；前端静态 `locales/**` + 动态 `mergeDynamicLocaleMessages` | `Localization`、`Infrastructure/Data/Seeds/I18nSeedData/` |
| **验证码** | `Slider` / `Behavior` | `Captcha`、`ITaktCaptchaService` |
| **缓存** | `ITaktCacheService`（Memory / Redis） | `Cache:Provider` |
| **安全** | 限流、CSRF、XSS、RSA 密码传输 | `Security`、`PasswordPolicy:Transport` |
| **工作流** | 流程方案/实例/任务/表单/变量/加签 | `Workflow/` 实体 + LogicFlow 设计器 |
| **代码生成** | 实体 → DTO/服务/控制器/前端/i18n 一键流水线 | `scripts/generate-all.cjs` |

前端：`v-permission`、验证码（`takt-captcha-slider` / `takt-captcha-behavior`）、`takt-modal` 标准 CRUD 弹窗。

---

## 业务模块

后端 `Controllers/` 与 `Domain/Entities/` 按相同领域划分；前端 `views/`、`api/`、`types/`、`locales/` 与之对齐。

| 领域 | 子模块 | 主要能力 |
|------|--------|----------|
| **Identity** | 用户、角色、菜单、租户、RBAC、认证 | 登录鉴权、权限分配、多租户/多公司授权 |
| **Foundation** | 字典、翻译、设置、编码、消息、在线、文化 | 平台基础数据、站内消息、动态 i18n |
| **HumanResource** | Organization（部门/岗位）、Personnel（员工）、Attendance（节假日）、Talent | 组织架构、人事主档 |
| **Logistics · Materials** | 物料、供应商、厂商、采购申请/订单/价格、工厂 | 采购与主数据 |
| **Logistics · Sales** | 客户、销售订单/价格 | 销售管理 |
| **Logistics · Quality** | Operation（IQC/IPQC/抽样）、Complaint、Cost | 来料/制程检验、质量成本 |
| **Logistics · Manufacturing** | Bom、Scheduling（APS）、Output（PCBA/ASSY 产出）、Defect（检验/维修/不良）、EngineeringChange（ECN） | 制造执行、产出与不良 |
| **Logistics · Serial** | 产品序列号入出库 | 序列号追溯 |
| **Logistics · Maintenance** | 设备、保养 | 设备维护 |
| **Accounting** | Financial（公司/科目/资产/会签）、Controlling（成本/利润中心） | 财务主档 |
| **Routine** | Announcement、NewsCenter、HelpDesk、DocumentCenter、ConferenceCenter、VisitorCenter | 公告、新闻、工单、文档、会议、访客 |
| **Workflow** | 流程方案、实例、任务、表单、变量、迁移、加签 | 审批流引擎 |
| **Workflow · Engine** | `TaktFlowEngineController` + `ITaktFlowEngineService` | 运行时：发起/待办/审批/加签（与实例 CRUD 分离） |
| **Statistics** | Logging（登录/操作/变更日志） | 审计与运维日志 |
| **Code** | Generator（代码生成表配置） | 在线代码生成元数据 |

---

## 项目结构

```
Takt.Plat/
├── backend/
│   ├── Takt.Plat.slnx
│   └── src/
│       ├── Takt.Shared/           # 公共类型、Options、异常
│       ├── Takt.Domain/           # 实体、仓储接口（190 实体）
│       ├── Takt.Application/      # DTO、应用服务、验证器
│       ├── Takt.Infrastructure/   # 仓储、种子、SignalR、中间件
│       └── Takt.WebApi/           # API 入口（Program.cs、Controllers）
├── frontend/
│   ├── src/
│   │   ├── api/                   # 按后端模块划分 REST 客户端
│   │   ├── types/                 # 与 api 一一对应的 TS 类型
│   │   ├── views/                 # 页面（标准 CRUD 结构）
│   │   ├── components/            # common/ + business/（takt-modal 等）
│   │   ├── stores/                # Pinia（含 foundation/signalr）
│   │   ├── locales/               # 静态 i18n（export default { page: … }）
│   │   ├── router/                # 懒加载路由
│   │   └── styles/                # global.css、主题 token
│   ├── vite.config.ts
│   └── package.json
├── scripts/                       # 代码生成与实体维护脚本
│   ├── generate-all.cjs           # 一键：DTO → 服务 → 控制器 → 前端 → i18n
│   ├── generate-from-backend.cjs  # 前端 api/types 生成
│   ├── generate-vue-all-from-api.cjs  # 串联 Vue 三模板（CRUD / TREE / Master-Detail）
│   ├── generate-vue-crud-from-api.cjs
│   ├── generate-vue-tree-from-api.cjs
│   └── generate-vue-master-detail-from-api.cjs
├── .cursor/rules/                 # 开发规范（00-project / 01-backend / 02-frontend）
├── LICENSE
├── README.md                        # English（默认）
├── README.zh-CN.md                  # 简体中文
├── README.ja-JP.md                  # 日本語
└── README.zh-HK.md                  # 繁體中文（香港）
```

---

## 编译与运行

### 环境要求

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) LTS（前端）
- SQL Server（连接字符串在本地 `appsettings*.json`，仓库仅提供 `appsettings.*.Example.json` 模板）

### 后端

```bash
dotnet restore backend/Takt.Plat.slnx
dotnet build backend/Takt.Plat.slnx -c Release
dotnet run --project backend/src/Takt.WebApi/Takt.WebApi.csproj
```

| 项 | 值 |
|----|-----|
| HTTP | `http://localhost:60070` |
| HTTPS | `https://localhost:60071` |
| API 文档 | `https://localhost:60071/scalar` |

首次启动前将 `backend/src/Takt.WebApi/appsettings.Example.json` 等 Example 文件复制为 `appsettings.json` / `appsettings.Development.json` / `appsettings.Production.json`（勿提交 Git），并填写 `ConnectionStrings`（OpenIddict + `Tenant_*`）。`Init:InitDb` / `Init:SeedData` 见 `appsettings.json`。

### 前端

```bash
cd frontend
cp env.example .env
cp env.development.example .env.development
cp env.production.example .env.production
npm install
npm run dev      # 开发（默认 https://localhost:60081）
npm run build    # 生产构建（vue-tsc + vite build）
```

（Windows 可用 `copy env.example .env` 等。`.env*` 已由 `.gitignore` 排除；Example 仅含 `<...>` 占位符，不含真实域名/端口。本地惯例可参考：前端 `60081`、后端 HTTPS `60071`，写入你自己的 `.env*` 即可。）

| 项 | 说明 |
|----|------|
| 开发地址 | `.env.development` 中 `VITE_APP_ORIGIN`（模板见 `env.development.example`，自行填端口） |
| API 代理 | `/api` → `VITE_API_PROXY_TARGET`（指向本机后端 HTTPS 根地址） |
| OAuth 回调 | `{VITE_APP_ORIGIN}/auth/callback`，须与后端 `OpenIddict:SpaRedirectUris` 一致 |

须与后端 `Cors`、`OpenIddict:SpaRedirectUris` 中的前端地址一致（见本地 `appsettings.Development.json`）。

### 默认账号（种子）

种子用户（各租户）：`admin`（超级管理员）、`guest`、`demo`。初始密码见 `PasswordPolicy:DefaultPassword`（默认 `Takt@123456`）。生产环境务必修改。

---

## 代码生成

新增实体后，可在仓库根目录执行：

```bash
node scripts/generate-all.cjs              # 全量流水线
node scripts/generate-all.cjs --entity TaktXxx   # 单实体
cd frontend && npm run generate            # 仅生成前端 api/types
cd frontend && npm run generate:vue        # 仅生成 CRUD 页面
```

生成规则与命名约定见 `.cursor/rules/00-project.mdc`（控制器复数、应用服务单数、前后端方法名对齐等）。

---

## 开发规范

仓库内 `.cursor/rules/` 定义完整约定，摘要：

- **后端**：DDD 分层、控制器复数 / 服务单数、`GetXxxListAsync` 等方法命名、i18n 种子键 `entity.*` / `menu.*`
- **前端**：Ant Design Vue + Tailwind、`t('路径.page.*')` 静态翻译、`v-permission` 权限码与菜单一致
- **格式**：禁止「隔行空行」，见 `03-format-blank-lines.mdc`

---

## 许可

[MIT License](LICENSE) · Copyright (c) 2026 Takt Technologies Co., Ltd.

**维护者**：Takt.Plat（Cursor AI 等）
