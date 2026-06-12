# Takt.Plat

[English](README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [繁體中文（香港）](README.zh-HK.md)

Takt デジタルファクトリー · フロント／バックエンド分離型エンタープライズプラットフォーム（.NET 9 + Vue 3）。

> ⚠️ **重要な声明**：本プロジェクトは AI により生成されています（Cursor AI 等の AI 支援開発ツールを使用）。コードは AI により自動生成・最適化されています。
>
> 🚫 **Issue は受け付けません**：AI 生成プロジェクトのため、Issue・バグ報告・機能要望は一切受け付けません。必要に応じて Fork して自行改修してください。

---

## プロジェクト概要

| 指標 | 数量 | 説明 |
|------|------|------|
| ドメインエンティティ | 190 | `backend/src/Takt.Domain/Entities/` |
| API コントローラ | 190 | `backend/src/Takt.WebApi/Controllers/` |
| フロント CRUD ページ | 199 | `frontend/src/views/**/index.vue` |
| 対応 UI 言語 | 3 | `zh-CN` / `en-US` / `ja-JP` |

**認証・権限、基盤プラットフォーム、人事、物流・製造、会計、日常業務、ワークフロー、統計・ログ、コード生成** 等の業務ドメインをカバー。DDD レイヤリング + マルチテナント DB 分離 + 会社単位データ分離を採用。

---

## 技術スタック

### バックエンド

| カテゴリ | 選定 |
|----------|------|
| ランタイム | .NET 9 |
| ORM | SqlSugar 5.x（業務 DB）+ EF Core（OpenIddict 認証 DB） |
| データベース | SQL Server（`Database:DbType = 1`） |
| DI | Autofac（アプリケーションサービス／バリデータ自動スキャン登録） |
| 認証 | OpenIddict 7.x（OAuth 2.0 / OIDC） |
| バリデーション | FluentValidation |
| ログ | Serilog（コンソール + ローテーションファイル） |
| API ドキュメント | Scalar（開発環境 `/scalar`） |
| リアルタイム | SignalR（デュアル Hub） |

### フロントエンド

| カテゴリ | 選定 |
|----------|------|
| ビルド | Vite 8 |
| フレームワーク | Vue 3 Composition API + TypeScript |
| UI | Ant Design Vue 4.x |
| スタイル | Tailwind CSS 4.x |
| 状態 / ルータ / HTTP | Pinia、vue-router 5、Axios |
| 国際化 | vue-i18n（静的 `locales/**` + バックエンド動的シード） |
| ワークフロー設計 | LogicFlow + `@form-create/ant-design-vue` |
| チャート | ECharts 6 |
| リアルタイム | `@microsoft/signalr` |
| PWA | vite-plugin-pwa（本番デフォルト有効） |

---

## アーキテクチャレイヤ

```
Takt.WebApi          → コントローラ、Program.cs、OpenIddict、ミドルウェア
Takt.Application     → DTO、アプリケーションサービス、FluentValidation バリデータ
Takt.Infrastructure  → リポジトリ、シード、SignalR、キャッシュ、マルチ DB マッピング
Takt.Domain          → エンティティ、リポジトリインターフェース
Takt.Shared          → ページング、例外、Options、ヘルパー
```

依存方向：`WebApi → Application + Infrastructure + Shared`；`Application → Domain + Shared`；`Domain → Shared`。コントローラは `ITaktXxxService` のみ注入。SqlSugar への直接アクセスは禁止。

---

## プラットフォーム能力

### テナント → 会社 → 業務データ

```
テナント Tenant（テナント別 DB） → 会社 Company（会社コード） → 業務データ（部門/従業員/製造/販売等）
```

| レイヤ | バックエンド | フロントエンド |
|--------|--------------|----------------|
| **テナント** | 独立業務 DB `ConnectionStrings:Tenant_{code}`；`TaktTenant`；`TaktTenantEntityBase`（ユーザー/ロール/メニュー/辞書等） | `useTenantStore`、`takt-tenant-toggle`；ログイン前後でテナント選択可 |
| **会社** | `TaktCompany`；`TaktCompanyEntityBase`（従業員/部門/製造/財務等）；リポジトリ `Where(TenantCode, CompanyCode)` | `takt-company-toggle`；テナント連動でアクセス可能会社を切替 |
| **貫通** | `ITaktUserContext` + リクエストヘッダ `X-Tenant-Code` / `X-Company-Code` | `api/request.ts` がヘッダを自動付与 |

**起動 Init スイッチ**（`appsettings.json` → `Init` / `Database`）：

1. `InitDb` — `TenantCodes` 順に各テナント業務 DB のテーブルを作成
2. `SeedData` — `TenantCodes` 順にテナント DB を切替え全シード実行；会社マスタ/休日/組織人事等はシード内で `CompanyCodes` 順に書込；工場マスタは `PlantCodes` 順
3. `CompanyCodes` / `PlantCodes` の順序はシード定義と一致必須；`CompanyCodes` 先頭はデモアカウント主会社等のデフォルト（`GetSeedCompanyCode()`）

### グローバル SignalR

ログイン後 **デュアル Hub** を維持（`AddTaktSignalR`、JWT は `TaktSignalRTokenMiddleware` で認証）：

| Hub | パス | 役割 |
|-----|------|------|
| **TaktConnectHub** | `/hubs/TaktConnectHub` | 接続/切断、オンラインユーザー、オンライン統計、強制ログアウト `ForceLogout` |
| **TaktNotificationHub** | `/hubs/TaktNotificationHub` | ダイレクトメッセージ、会社内ブロードキャスト、メッセージ/オンライン統計プッシュ |

プッシュは **会社 + ユーザー** グループ（`TaktSignalRGroupNames`）で、テナント/会社分離と整合。ディスパッチ入口：`ITaktSignalRDispatchService`。

フロント：`utils/takt-signalr.ts`；`stores/foundation/signalr.ts`；レイアウトログイン後に自動接続。

### その他の能力

| 能力 | 説明 | 設定 / 入口 |
|------|------|-------------|
| **権限（RBAC）** | OpenIddict；メニュー権限コード `領域:ディレクトリ:…:エンティティ:操作`；`[TaktPermission]` | `OpenIddict`、`Takt.WebApi/Filters/` |
| **ログ** | Serilog ローテーション；`TaktLoggingMiddleware`；`TaktLoginLog` / `TaktOperLog` / `TaktDeltaLog` | `TaktLogging`、`Serilog` |
| **ローカライズ** | DB + I18n シード；フロント静的 `locales/**` + 動的 `mergeDynamicLocaleMessages` | `Localization`、`Infrastructure/Data/Seeds/I18nSeedData/` |
| **CAPTCHA** | `Slider` / `Behavior` | `Captcha`、`ITaktCaptchaService` |
| **キャッシュ** | `ITaktCacheService`（Memory / Redis） | `Cache:Provider` |
| **セキュリティ** | レート制限、CSRF、XSS、RSA パスワード転送 | `Security`、`PasswordPolicy:Transport` |
| **ワークフロー** | フロー定義/インスタンス/タスク/フォーム/変数/加签 | `Workflow/` エンティティ + LogicFlow デザイナ |
| **コード生成** | エンティティ → DTO/サービス/コントローラ/フロント/i18n 一括パイプライン | `scripts/generate-all.cjs` |

フロント：`v-permission`、CAPTCHA（`takt-captcha-slider` / `takt-captcha-behavior`）、標準 CRUD モーダル `takt-modal`。

---

## 業務モジュール

バックエンド `Controllers/` と `Domain/Entities/` は同一ドメイン構成；フロント `views/`、`api/`、`types/`、`locales/` と整合。

| ドメイン | サブモジュール | 主な能力 |
|----------|----------------|----------|
| **Identity** | ユーザー、ロール、メニュー、テナント、RBAC、認証 | ログイン認証、権限割当、マルチテナント/会社認可 |
| **Foundation** | 辞書、翻訳、設定、採番、メッセージ、オンライン、文化 | 基盤マスタ、站内メッセージ、動的 i18n |
| **HumanResource** | Organization（部門/役職）、Personnel（従業員）、Attendance（休日）、Talent | 組織構造、人事マスタ |
| **Logistics · Materials** | 資材、サプライヤ、メーカ、購買申請/発注/価格、工場 | 調達とマスタ |
| **Logistics · Sales** | 顧客、販売受注/価格 | 販売管理 |
| **Logistics · Quality** | Operation（IQC/IPQC/サンプリング）、Complaint、Cost | 入荷/工程検査、品質コスト |
| **Logistics · Manufacturing** | Bom、Scheduling（APS）、Output（PCBA/ASSY 出来高）、Defect（検査/修理/不良）、EngineeringChange（ECN） | 製造実行、出来高と不良 |
| **Logistics · Serial** | 製品シリアル入出庫 | シリアル追跡 |
| **Logistics · Maintenance** | 設備、保全 | 設備メンテナンス |
| **Accounting** | Financial（会社/科目/資産/会签）、Controlling（原価/利益センタ） | 財務マスタ |
| **Routine** | Announcement、NewsCenter、HelpDesk、DocumentCenter、ConferenceCenter、VisitorCenter | 公告、ニュース、チケット、文書、会議、来訪者 |
| **Workflow** | フロー定義、インスタンス、タスク、フォーム、変数、遷移、加签 | 承認エンジン |
| **Workflow · Engine** | `TaktFlowEngineController` + `ITaktFlowEngineService` | ランタイム：起票/待办/承認/加签（インスタンス CRUD と分離） |
| **Statistics** | Logging（ログイン/操作/変更ログ） | 監査と運用ログ |
| **Code** | Generator（コード生成テーブル設定） | オンラインコード生成メタデータ |

---

## プロジェクト構成

```
Takt.Plat/
├── backend/
│   ├── Takt.Plat.slnx
│   └── src/
│       ├── Takt.Shared/           # 共通型、Options、例外
│       ├── Takt.Domain/           # エンティティ、リポジトリ IF（190 エンティティ）
│       ├── Takt.Application/      # DTO、アプリケーションサービス、バリデータ
│       ├── Takt.Infrastructure/   # リポジトリ、シード、SignalR、ミドルウェア
│       └── Takt.WebApi/           # API 入口（Program.cs、Controllers）
├── frontend/
│   ├── src/
│   │   ├── api/                   # バックエンドモジュール別 REST クライアント
│   │   ├── types/                 # api と 1:1 の TS 型
│   │   ├── views/                 # ページ（標準 CRUD 構成）
│   │   ├── components/            # common/ + business/（takt-modal 等）
│   │   ├── stores/                # Pinia（foundation/signalr 含む）
│   │   ├── locales/               # 静的 i18n（export default { page: … }）
│   │   ├── router/                # 遅延読込ルート
│   │   └── styles/                # global.css、テーマ token
│   ├── vite.config.ts
│   └── package.json
├── scripts/                       # コード生成・エンティティ保守スクリプト
│   ├── generate-all.cjs           # 一括：DTO → サービス → コントローラ → フロント → i18n
│   ├── generate-from-backend.cjs  # フロント api/types 生成
│   ├── generate-vue-all-from-api.cjs  # Vue 3 テンプレート連結（CRUD / TREE / Master-Detail）
│   ├── generate-vue-crud-from-api.cjs
│   ├── generate-vue-tree-from-api.cjs
│   └── generate-vue-master-detail-from-api.cjs
├── .cursor/rules/                 # 開発規約（00-project / 01-backend / 02-frontend）
├── LICENSE
├── README.md                        # English（デフォルト）
├── README.zh-CN.md                  # 简体中文
├── README.ja-JP.md                  # 日本語
└── README.zh-HK.md                  # 繁體中文（香港）
```

---

## ビルドと実行

### 環境要件

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) LTS（フロント）
- SQL Server（接続文字列はローカル `appsettings*.json`；リポジトリは `appsettings.*.Example.json` テンプレのみ）

### バックエンド

```bash
dotnet restore backend/Takt.Plat.slnx
dotnet build backend/Takt.Plat.slnx -c Release
dotnet run --project backend/src/Takt.WebApi/Takt.WebApi.csproj
```

| 項目 | 値 |
|------|-----|
| HTTP | `http://localhost:60070` |
| HTTPS | `https://localhost:60071` |
| API ドキュメント | `https://localhost:60071/scalar` |

初回起動前に `backend/src/Takt.WebApi/appsettings.Example.json` 等を `appsettings.json` / `appsettings.Development.json` / `appsettings.Production.json` にコピー（Git コミット禁止）し、`ConnectionStrings`（OpenIddict + `Tenant_*`）を設定。`Init:InitDb` / `Init:SeedData` は `appsettings.json` 参照。

### フロントエンド

```bash
cd frontend
cp env.example .env
cp env.development.example .env.development
cp env.production.example .env.production
npm install
npm run dev      # 開発（デフォルト https://localhost:60081）
npm run build    # 本番ビルド（vue-tsc + vite build）
```

（Windows は `copy env.example .env` 等。`.env*` は `.gitignore` 対象；Example は `<...>` プレースホルダのみ。ローカル慣例：フロント `60081`、バック HTTPS `60071` — 各自の `.env*` に記載。）

| 項目 | 説明 |
|------|------|
| 開発 URL | `.env.development` の `VITE_APP_ORIGIN`（`env.development.example` 参照） |
| API プロキシ | `/api` → `VITE_API_PROXY_TARGET`（ローカルバック HTTPS ルート） |
| OAuth コールバック | `{VITE_APP_ORIGIN}/auth/callback`、バック `OpenIddict:SpaRedirectUris` と一致必須 |

バックエンド `Cors`、`OpenIddict:SpaRedirectUris` のフロント URL と一致させる（ローカル `appsettings.Development.json` 参照）。

### デフォルトアカウント（シード）

シードユーザー（テナント毎）：`admin`（スーパー管理者）、`guest`、`demo`。初期パスワード：`PasswordPolicy:DefaultPassword`（デフォルト `Takt@123456`）。本番環境では必ず変更。

---

## コード生成

エンティティ追加後、リポジトリルートで：

```bash
node scripts/generate-all.cjs              # 全量パイプライン
node scripts/generate-all.cjs --entity TaktXxx   # 単一エンティティ
cd frontend && npm run generate            # フロント api/types のみ
cd frontend && npm run generate:vue        # CRUD ページのみ
```

命名規約：`.cursor/rules/00-project.mdc`（コントローラ複数形、サービス単数形、前後端メソッド名整合等）。

---

## 開発規約

`.cursor/rules/` の要約：

- **バックエンド**：DDD レイヤ、コントローラ複数形 / サービス単数形、`GetXxxListAsync` 等の命名、i18n キー `entity.*` / `menu.*`
- **フロントエンド**：Ant Design Vue + Tailwind、静的 `t('路径.page.*')`、`v-permission` はメニューと一致
- **フォーマット**：「隔行空行」禁止、`03-format-blank-lines.mdc` 参照

---

## ライセンス

[MIT License](LICENSE) · Copyright (c) 2026 Takt Technologies Co., Ltd.

**メンテナ**：Takt.Plat（Cursor AI 等）
