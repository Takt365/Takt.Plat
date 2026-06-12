# Takt.Plat

[English](README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [繁體中文（香港）](README.zh-HK.md)

Takt Digital Factory · Full-stack enterprise platform (.NET 9 + Vue 3).

> ⚠️ **Important notice**: This project is AI-generated (using Cursor AI and other AI-assisted development tools). Code is automatically generated and optimized by AI.
>
> 🚫 **No issues accepted**: Because this is an AI-generated project, we do not accept issues, bug reports, or feature requests. Fork the repository and modify it yourself if needed.

---

## Overview

| Metric | Count | Notes |
|--------|-------|-------|
| Domain entities | 190 | `backend/src/Takt.Domain/Entities/` |
| API controllers | 190 | `backend/src/Takt.WebApi/Controllers/` |
| Frontend CRUD pages | 199 | `frontend/src/views/**/index.vue` |
| Supported UI locales | 3 | `zh-CN` / `en-US` / `ja-JP` |

Covers **identity & permissions, foundation platform, human resources, logistics & manufacturing, accounting, routine office, workflow, statistics & logging, and code generation**, using DDD layering, multi-tenant database sharding, and company-level data isolation.

---

## Tech stack

### Backend

| Category | Choice |
|----------|--------|
| Runtime | .NET 9 |
| ORM | SqlSugar 5.x (business DB) + EF Core (OpenIddict auth DB) |
| Database | SQL Server (`Database:DbType = 1`) |
| DI | Autofac (auto-scan application services / validators) |
| Auth | OpenIddict 7.x (OAuth 2.0 / OIDC) |
| Validation | FluentValidation |
| Logging | Serilog (console + rolling files) |
| API docs | Scalar (dev: `/scalar`) |
| Real-time | SignalR (dual Hub) |

### Frontend

| Category | Choice |
|----------|--------|
| Build | Vite 8 |
| Framework | Vue 3 Composition API + TypeScript |
| UI | Ant Design Vue 4.x |
| Styling | Tailwind CSS 4.x |
| State / router / HTTP | Pinia, vue-router 5, Axios |
| i18n | vue-i18n (static `locales/**` + dynamic backend seeds) |
| Workflow designer | LogicFlow + `@form-create/ant-design-vue` |
| Charts | ECharts 6 |
| Real-time | `@microsoft/signalr` |
| PWA | vite-plugin-pwa (enabled in production by default) |

---

## Architecture layers

```
Takt.WebApi          → Controllers, Program.cs, OpenIddict, middleware
Takt.Application     → DTOs, application services, FluentValidation validators
Takt.Infrastructure  → Repositories, seeds, SignalR, cache, multi-DB mapping
Takt.Domain          → Entities, repository interfaces
Takt.Shared          → Paging, exceptions, Options, helpers
```

Dependency flow: `WebApi → Application + Infrastructure + Shared`; `Application → Domain + Shared`; `Domain → Shared`. Controllers inject only `ITaktXxxService`; direct SqlSugar access in controllers is forbidden.

---

## Platform capabilities

### Tenant → Company → Business data

```
Tenant (per-tenant DB) → Company (company code) → Business data (dept/employee/manufacturing/sales, etc.)
```

| Layer | Backend | Frontend |
|-------|---------|----------|
| **Tenant** | Separate business DB `ConnectionStrings:Tenant_{code}`; `TaktTenant`; `TaktTenantEntityBase` (users/roles/menus/dicts, etc.) | `useTenantStore`, `takt-tenant-toggle`; tenant selectable before and after login |
| **Company** | `TaktCompany`; `TaktCompanyEntityBase` (employees/depts/manufacturing/finance, etc.); repository `Where(TenantCode, CompanyCode)` | `takt-company-toggle`; switches accessible companies with tenant |
| **End-to-end** | `ITaktUserContext` + headers `X-Tenant-Code` / `X-Company-Code` | `api/request.ts` attaches headers automatically |

**Startup Init flags** (`appsettings.json` → `Init` / `Database`):

1. `InitDb` — Create business DB tables for each tenant in `TenantCodes` order
2. `SeedData` — Run all seeds per tenant DB in `TenantCodes` order; company master/holidays/org-HR data written per `CompanyCodes` inside seeds; plant master per `PlantCodes`
3. `CompanyCodes` / `PlantCodes` order must match seed definitions; first `CompanyCodes` entry is still the demo account’s primary company (`GetSeedCompanyCode()`)

### Global SignalR

After login, **dual Hub** connections (`AddTaktSignalR`, JWT via `TaktSignalRTokenMiddleware`):

| Hub | Path | Role |
|-----|------|------|
| **TaktConnectHub** | `/hubs/TaktConnectHub` | Connect/disconnect, online users, online stats, `ForceLogout` |
| **TaktNotificationHub** | `/hubs/TaktNotificationHub` | Direct messages, company broadcast, message/online stats push |

Push targets **company + user** groups (`TaktSignalRGroupNames`), aligned with tenant/company isolation. Dispatch entry: `ITaktSignalRDispatchService`.

Frontend: `utils/takt-signalr.ts`; `stores/foundation/signalr.ts`; auto-connect after layout login.

### Other capabilities

| Capability | Description | Config / entry |
|------------|-------------|----------------|
| **RBAC** | OpenIddict; menu permission `domain:path:…:entity:action`; `[TaktPermission]` | `OpenIddict`, `Takt.WebApi/Filters/` |
| **Logging** | Serilog rolling files; `TaktLoggingMiddleware`; `TaktLoginLog` / `TaktOperLog` / `TaktDeltaLog` | `TaktLogging`, `Serilog` |
| **Localization** | DB + I18n seeds; frontend static `locales/**` + dynamic `mergeDynamicLocaleMessages` | `Localization`, `Infrastructure/Data/Seeds/I18nSeedData/` |
| **Captcha** | `Slider` / `Behavior` | `Captcha`, `ITaktCaptchaService` |
| **Cache** | `ITaktCacheService` (Memory / Redis) | `Cache:Provider` |
| **Security** | Rate limit, CSRF, XSS, RSA password transport | `Security`, `PasswordPolicy:Transport` |
| **Workflow** | Scheme/instance/task/form/variable/add-sign | `Workflow/` entities + LogicFlow designer |
| **Code generation** | Entity → DTO/service/controller/frontend/i18n pipeline | `scripts/generate-all.cjs` |

Frontend: `v-permission`, captcha (`takt-captcha-slider` / `takt-captcha-behavior`), standard CRUD modal `takt-modal`.

---

## Business modules

Backend `Controllers/` and `Domain/Entities/` share the same domain layout; frontend `views/`, `api/`, `types/`, `locales/` align accordingly.

| Domain | Submodules | Main capabilities |
|--------|------------|-------------------|
| **Identity** | Users, roles, menus, tenants, RBAC, auth | Login, permission assignment, multi-tenant/company authorization |
| **Foundation** | Dict, translation, settings, numbering, messages, online, culture | Platform master data, in-app messages, dynamic i18n |
| **HumanResource** | Organization (dept/post), Personnel (employee), Attendance (holidays), Talent | Org structure, HR master data |
| **Logistics · Materials** | Material, supplier, manufacturer, purchase requisition/order/price, plant | Procurement & master data |
| **Logistics · Sales** | Customer, sales order/price | Sales management |
| **Logistics · Quality** | Operation (IQC/IPQC/sampling), Complaint, Cost | Incoming/in-process inspection, quality cost |
| **Logistics · Manufacturing** | Bom, Scheduling (APS), Output (PCBA/ASSY), Defect (inspection/repair/defect), EngineeringChange (ECN) | MES, output & defect |
| **Logistics · Serial** | Product serial inbound/outbound | Serial traceability |
| **Logistics · Maintenance** | Equipment, maintenance | Equipment upkeep |
| **Accounting** | Financial (company/account/asset/countersign), Controlling (cost/profit center) | Finance master data |
| **Routine** | Announcement, NewsCenter, HelpDesk, DocumentCenter, ConferenceCenter, VisitorCenter | Announcements, news, tickets, documents, meetings, visitors |
| **Workflow** | Scheme, instance, task, form, variable, transition, add-sign | Approval engine |
| **Workflow · Engine** | `TaktFlowEngineController` + `ITaktFlowEngineService` | Runtime: start/todo/approve/add-sign (separate from instance CRUD) |
| **Statistics** | Logging (login/operation/change logs) | Audit & ops logs |
| **Code** | Generator (code-gen table config) | Online codegen metadata |

---

## Project structure

```
Takt.Plat/
├── backend/
│   ├── Takt.Plat.slnx
│   └── src/
│       ├── Takt.Shared/           # Shared types, Options, exceptions
│       ├── Takt.Domain/           # Entities, repo interfaces (190 entities)
│       ├── Takt.Application/      # DTOs, application services, validators
│       ├── Takt.Infrastructure/   # Repositories, seeds, SignalR, middleware
│       └── Takt.WebApi/           # API entry (Program.cs, Controllers)
├── frontend/
│   ├── src/
│   │   ├── api/                   # REST clients by backend module
│   │   ├── types/                 # TS types aligned with api/
│   │   ├── views/                 # Pages (standard CRUD shell)
│   │   ├── components/            # common/ + business/ (takt-modal, etc.)
│   │   ├── stores/                # Pinia (incl. foundation/signalr)
│   │   ├── locales/               # Static i18n (export default { page: … })
│   │   ├── router/                # Lazy-loaded routes
│   │   └── styles/                # global.css, theme tokens
│   ├── vite.config.ts
│   └── package.json
├── scripts/                       # Codegen & entity maintenance scripts
│   ├── generate-all.cjs           # One-shot: DTO → service → controller → frontend → i18n
│   ├── generate-from-backend.cjs  # Frontend api/types generation
│   ├── generate-vue-all-from-api.cjs  # Vue templates (CRUD / TREE / Master-Detail)
│   ├── generate-vue-crud-from-api.cjs
│   ├── generate-vue-tree-from-api.cjs
│   └── generate-vue-master-detail-from-api.cjs
├── .cursor/rules/                 # Dev conventions (00-project / 01-backend / 02-frontend)
├── LICENSE
├── README.md                        # English (default)
├── README.zh-CN.md                  # 简体中文
├── README.ja-JP.md                  # 日本語
└── README.zh-HK.md                  # 繁體中文（香港）
```

---

## Build & run

### Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) LTS (frontend)
- SQL Server (connection strings in local `appsettings*.json`; repo provides `appsettings.*.Example.json` templates only)

### Backend

```bash
dotnet restore backend/Takt.Plat.slnx
dotnet build backend/Takt.Plat.slnx -c Release
dotnet run --project backend/src/Takt.WebApi/Takt.WebApi.csproj
```

| Item | Value |
|------|-------|
| HTTP | `http://localhost:60070` |
| HTTPS | `https://localhost:60071` |
| API docs | `https://localhost:60071/scalar` |

Before first run, copy `backend/src/Takt.WebApi/appsettings.Example.json` (and related Example files) to `appsettings.json` / `appsettings.Development.json` / `appsettings.Production.json` (do not commit), and fill `ConnectionStrings` (OpenIddict + `Tenant_*`). See `Init:InitDb` / `Init:SeedData` in `appsettings.json`.

### Frontend

```bash
cd frontend
cp env.example .env
cp env.development.example .env.development
cp env.production.example .env.production
npm install
npm run dev      # Dev (default https://localhost:60081)
npm run build    # Production (vue-tsc + vite build)
```

(On Windows use `copy env.example .env`, etc. `.env*` is gitignored; Example files use `<...>` placeholders only. Typical local ports: frontend `60081`, backend HTTPS `60071` — set in your own `.env*`.)

| Item | Notes |
|------|-------|
| Dev origin | `VITE_APP_ORIGIN` in `.env.development` (see `env.development.example`) |
| API proxy | `/api` → `VITE_API_PROXY_TARGET` (local backend HTTPS root) |
| OAuth callback | `{VITE_APP_ORIGIN}/auth/callback`, must match backend `OpenIddict:SpaRedirectUris` |

Must align with backend `Cors` and `OpenIddict:SpaRedirectUris` (see local `appsettings.Development.json`).

### Default accounts (seed)

Seed users (per tenant): `admin` (super admin), `guest`, `demo`. Initial password: `PasswordPolicy:DefaultPassword` (default `Takt@123456`). Change in production.

---

## Code generation

After adding an entity, from repo root:

```bash
node scripts/generate-all.cjs              # Full pipeline
node scripts/generate-all.cjs --entity TaktXxx   # Single entity
cd frontend && npm run generate            # Frontend api/types only
cd frontend && npm run generate:vue        # CRUD pages only
```

Naming rules: `.cursor/rules/00-project.mdc` (plural controllers, singular services, aligned method names).

---

## Development conventions

Summarized from `.cursor/rules/`:

- **Backend**: DDD layers, plural controllers / singular services, `GetXxxListAsync` naming, i18n keys `entity.*` / `menu.*`
- **Frontend**: Ant Design Vue + Tailwind, static `t('path.page.*')`, `v-permission` aligned with menus
- **Formatting**: No “blank line every other line”; see `03-format-blank-lines.mdc`

---

## License

[MIT License](LICENSE) · Copyright (c) 2026 Takt Technologies Co., Ltd.

**Maintainer**: Takt.Plat (Cursor AI, etc.)
