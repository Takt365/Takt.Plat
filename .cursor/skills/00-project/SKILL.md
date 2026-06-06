---
name: 00-project
description: >-
  Takt.Plat 项目通用约定与规则 Skill 索引（命名、文件头、i18n 键、权限码、Git 提交）。
  用于新建文件、不确定命名/翻译键/权限、选型 .cursor/rules 对应 skill，
  或用户提到 00-project、Takt 命名规范时。
---

# Takt 项目通用约定

完整规范：`.cursor/rules/00-project.mdc`

## 命名速查

| 范围 | 规则 |
|------|------|
| C# 类/文件 | `Takt` + PascalCase；控制器**复数**、服务**单数** |
| 前端文件 | kebab-case（`user.ts`、`user-form.vue`） |
| 后端方法 | `GetXxxListAsync`（禁止 `GetListAsync`） |
| 前端 API | `getXxxList`（禁止 `getList`） |
| 主键（前端 types） | `id: string` |

## 规则分层（避免重复阅读）

| 层次 | 权威源 |
|------|--------|
| 命名/权限/i18n | `00-project` §1 |
| CRUD 全栈基线 | `12-crud` |
| 后端/前端专域 | `01-backend` / `02-frontend` |
| 表形态扩展 | `10-master-detail` / `11-tree-table`（**叠加** 12-crud） |
| Vue 视图/表单 | `13-vue-view` / `14-vue-form` |
| 代码生成 | `15-codegen` |
| 权限/翻译键 | `16-permission-i18n` |
| 溢出 | `06` / `07` / `08` |
| 工具类 | `04` / `05` |
| 空行 | `03-format-blank-lines` |

§1.4/§1.5 与 `12-crud` 能力表**同源**：命名以本章为准；CRUD 联调与页面壳以 `12-crud` 为准。

## i18n 键（动态，后端种子）

- 分隔符：小写 + **点号** `.`
- 结构：`用途.目录[.子目录…].末段`
- 菜单：`menu.{领域}.{目录…}.{项}`
- 实体字段：`entity.{实体}.{字段}`
- 部门：`org.dept.{编码小写}`
- ❌ 段内下划线拼假一段（`quote_a`）

## 静态 locales（前端）

- 路径决定前缀：`locales/identity/user/zh-CN.ts` → `identity.user.page.*`
- 文件内：`export default { page: { … } }`
- 引用：`t('identity.user.page.title')`（**必须有 `.page`**）

## 权限码

- 结构：`领域:目录[:子目录…]:实体:操作`（**冒号**分隔）
- 末段必须是操作：`list`、`create`、`update`…
- 菜单种子、`[Authorize]`、`v-permission` 三者一致

## 文件头（强制）

| 类型 | 要求 |
|------|------|
| C# | 八段 `// =====` 块 + XML 注释 |
| TS（非 locales） | 同 C# 块注释 |
| Vue | 顶部 **8 行 HTML 注释**（无创建时间/创建人） |

## Git 提交

`<type>(<scope>): <subject>` — 如 `feat(identity): 用户列表分页查询`

## 写入前必检

```
- [ ] 空行：遵守 03-format-blank-lines（无隔行空行）
- [ ] 命名：C# 有 Takt 前缀；方法带实体前缀
- [ ] i18n/权限：键格式符合上表
- [ ] 溢出/工具/工作流：见下方 Skill 索引选型
```

## 规则与 Skill 索引（00~16）

规则源：`.cursor/rules/`。实现任务优先打开对应 **skill**（检查清单），细则以 **mdc** 为准。

| 编号 | 规则 | Skill | 何时用 |
|------|------|-------|--------|
| 00 | `00-project.mdc` | 本文件 | 命名、i18n、权限、文件头、Git |
| 01 | `01-backend.mdc` | [01-backend](../01-backend/SKILL.md) | 后端分层、DDD、种子 |
| 02 | `02-frontend.mdc` | [02-frontend](../02-frontend/SKILL.md) | 前端技术栈、UI 组件 |
| 03 | `03-format-blank-lines.mdc` | [03-format-blank-lines](../03-format-blank-lines/SKILL.md) | 空行格式、写入后自检 |
| 04 | `04-utils-csharp.mdc` | [04-utils-csharp](../04-utils-csharp/SKILL.md) | `Helpers/`、`Utils/` |
| 05 | `05-utils-vue.mdc` | [05-utils-vue](../05-utils-vue/SKILL.md) | `frontend/src/utils/` |
| 06 | `06-overflow-csharp.mdc` | [06-overflow-csharp](../06-overflow-csharp/SKILL.md) | 后端分页/导出/算术 |
| 07 | `07-overflow-vue.mdc` | [07-overflow-vue](../07-overflow-vue/SKILL.md) | virtual/主键 string |
| 08 | `08-overflow-fullstack.mdc` | [08-overflow-fullstack](../08-overflow-fullstack/SKILL.md) | 前后端联调 |
| 09 | `09-workflow.mdc` | [09-workflow](../09-workflow/SKILL.md) | 审批工作流 |
| 10 | `10-master-detail.mdc` | [10-master-detail](../10-master-detail/SKILL.md) | 主子表 OneToMany |
| 11 | `11-tree-table.mdc` | [11-tree-table](../11-tree-table/SKILL.md) | 树表 ParentId |
| 12 | `12-crud.mdc` | [12-crud](../12-crud/SKILL.md) | **CRUD 全栈基线**（单表/主子表/树表均须） |
| 13 | `13-vue-view.mdc` | [13-vue-view](../13-vue-view/SKILL.md) | **index.vue** 三种视图壳 |
| 14 | `14-vue-form.mdc` | [14-vue-form](../14-vue-form/SKILL.md) | ***-form.vue** 三种表单 |
| 15 | `15-codegen.mdc` | [15-codegen](../15-codegen/SKILL.md) | **generate-all** 全栈生成 |
| 16 | `16-permission-i18n.mdc` | [16-permission-i18n](../16-permission-i18n/SKILL.md) | **Permission + I18nKey** 对齐 |

## 快速选型

| 任务 | Skill |
|------|-------|
| **任何列表模块（先过 CRUD 基线）** | **`12-crud`** |
| 单表 | `12-crud` → `15-codegen` → `13-vue-view` + `14-vue-form` |
| 主子表 | `12-crud` → `15-codegen` → `10-master-detail` → `13-vue-view` + `14-vue-form` |
| 树表 | `12-crud` → `15-codegen`（后端）→ `11-tree-table` → `13-vue-view` + `14-vue-form`（Vue 手工） |
| 新建实体全栈 | **`15-codegen`** + `12-crud` |
| 新建后端 | `01-backend` + `12-crud` |
| 新建前端页 | `02-frontend` + `12-crud` + `13-vue-view` + `14-vue-form` |
| 文件空行/行数翻倍 | `03-format-blank-lines` |
| C# Helper | `04-utils-csharp` |
| 前端 utils | `05-utils-vue` |
| 列表/导出/大数组 | `06-overflow-csharp` + `07-overflow-vue` + `08-overflow-fullstack` |
| 审批/流程 | `09-workflow` |
| 权限/翻译键对齐 | **`16-permission-i18n`** |

## 推荐组合

- **全栈 CRUD 基线**：`12-crud` + `15-codegen` + `13-vue-view` + `14-vue-form` + `08-overflow-fullstack`
- **主子表**：`12-crud` → `10-master-detail` → `13-vue-view` / `14-vue-form`
- **树表**：`12-crud` → `11-tree-table` → `13-vue-view` / `14-vue-form`
- **任意写文件**：末尾跑 `03-format-blank-lines`
