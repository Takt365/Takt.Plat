---
name: 15-codegen
description: >-
  Takt 全栈代码生成（generate-all 流水线、单表/主子表/树表脚本识别、排除列表、生成后审阅）。
  用于 node scripts/generate-all、generate-vue-all-from-api、新建实体后生成代码，
  或用户提到 15-codegen、代码生成、generate-all 时。
---

# 全栈代码生成

完整规范：`.cursor/rules/15-codegen.mdc`

**入口**：`node scripts/generate-all.cjs --<Entity>` 或 `--all`（实体名不带 `Takt` 前缀）

## 新建实体工作流（推荐）

```
1. 写好 Takt.Domain/Entities/**/TaktXxx.cs
2. node scripts/generate-all.cjs --Xxx
3. dotnet build
4. 人工：QueryExpression、菜单+按钮 Permission、Mapster
5. 对照 12-crud + 10/11（形态）+ 13/14（Vue）+ 16-permission-i18n
```

## 流水线（8 步，不可跳序）

| 步 | 脚本 | 产出 |
|----|------|------|
| 0 | `generate-entity-rbac-navigations.cjs` | 实体 RBAC `[Navigate]` 区 |
| 1 | `generate-dtos-from-entity.cjs` | `TaktXxxDtos.cs` |
| 2 | `generate-validators-from-entity.cjs` | `TaktXxx*Validator.cs` |
| 3 | `generate-services-from-dtos.cjs` | `ITaktXxxService` / `TaktXxxService` |
| 4 | `generate-controllers-from-services.cjs` | `TaktXxxsController` |
| 5 | `generate-from-backend.cjs` | `types/` + `api/` |
| 6 | `generate-entity-i18n-seed.cjs` | `TaktXxxI18nSeedData.cs` |
| 7 | `generate-vue-all-from-api.cjs` | `index.vue` + `*-form.vue`（CRUD / TREE / Master-Detail） |

**写入策略**：不存在则创建，已存在则覆盖；`TaktAuth`/`TaktRbac`/`TaktFlowEngine` 等须 `--force` 才覆盖。

## 三种形态 × 生成范围

| 形态 | 实体特征 | 后端脚本 | Vue 脚本 |
|------|----------|----------|----------|
| 单表 `crud` | 无 OneToMany、无 ParentId | 标准 CRUD + Options | ✅ |
| 主子表 `sub` | `[Navigate] OneToMany` | Fill/Save 级联 | ✅ 展开行 + 表单 Tab |
| 树表 `tree` | 含 `ParentId` | tree / tree-options API | ❌ 手工 13/14 |

识别：`generate-services-from-dtos.cjs` → `identifyCrudType`。

## 单脚本命令（调试/局部生成）

```bash
node scripts/generate-dtos-from-entity.cjs --Holiday
node scripts/generate-services-from-dtos.cjs --Holiday
node scripts/generate-from-backend.cjs --Holiday
node scripts/generate-vue-crud-from-api.cjs --Holiday --view-path human-resource/attendance-leave/holiday
node scripts/generate-vue-all-from-api.cjs --CostCenter
node scripts/generate-all.cjs --Holiday --dry-run
```

## 排除（不生成或跳过）

| 类别 | 示例 |
|------|------|
| 手工 CRUD | User、Online、Message、DictType、DictData |
| RBAC 八表 | UserRole、RoleMenu、EmployeeDept… |
| 独立服务 | TaktAuth、TaktRbac、TaktFlowEngine |
| Vue 排除 | User、Menu、Dept、Dict*、Culture、Translation、workflow/** |

树表 **Dept**：后端可生成，Vue 须参照 `dept/index.vue` 手工。

## 生成后必检

```
- [ ] dotnet build
- [ ] QueryExpression / 业务过滤（服务私有方法）
- [ ] 权限四处一致（16-permission-i18n）
- [ ] entity.* i18n 种子与列标题 t('entity.*')
- [ ] 树表/排除实体：13-vue-view + 14-vue-form 手工页
- [ ] 12-crud 11 项能力 + 08 双端分页
- [ ] 03-format-blank-lines
```

## 命名（脚本强制）

| 层 | 规则 |
|----|------|
| 服务 | 单数 `TaktHolidayService` |
| 控制器 | 复数 `TaktHolidaysController` |
| API 路由 | `api/TaktHolidays`（复数） |
| 前端 api 文件 | kebab 单数 `holiday.ts` |
| 前端 types | 同模块 `{entity}.d.ts` |

复数特例：`generate-script-common.cjs` → `CONTROLLER_PLURAL_OVERRIDES`（User→Users 等）。

## 交叉规则

| 场景 | 规则 / Skill |
|------|----------------|
| CRUD 基线 | `12-crud` |
| 主子表 | `10-master-detail` |
| 树表 | `11-tree-table` |
| Vue 视图/表单 | `13-vue-view`、`14-vue-form` |
| 权限/翻译键 | `16-permission-i18n` |
| 后端/前端专域 | `01-backend`、`02-frontend` |
