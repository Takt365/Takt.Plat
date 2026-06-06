---
name: 01-backend
description: >-
  新建或审查 Takt 后端模块（DDD 分层、复数控制器、单数应用服务、CRUD、种子、权限）。
  用于 backend C# 实体/DTO/Service/Controller、SqlSugar、FluentValidation、
  或用户提到 01-backend、TaktXxxService 时。
---

# Takt 后端开发

完整规范：`.cursor/rules/01-backend.mdc`

## 标准参照（先读再写）

| 类型 | 文件 |
|------|------|
| 应用服务 CRUD | `TaktLoginLogService.cs` |
| 控制器 CRUD | `TaktLoginLogsController.cs`、`TaktUsersController.cs` |
| 控制器基类 | `TaktControllerBase.cs` |
| 实体 | `TaktUser.cs` |
| 用户种子 | `TaktUserSeedData.cs` |
| **全栈 CRUD 映射** | [12-crud](../12-crud/SKILL.md) |

## 新建 CRUD 模块（后端专责）

**前置（强制）**：先完成 [12-crud](../12-crud/SKILL.md) 基线清单（11 项能力、路由、权限）。

```
- [ ] 1. Domain：实体继承 Tenant/Company/Approval 基类；表名 takt_{模块}_{实体}
- [ ] 2. DTO：TaktXxxDto / QueryDto(继承 TaktPagedQuery) / Create / Update / Import / Export
- [ ] 3. 服务：ITaktXxxService + TaktXxxService（单数），继承 TaktServiceBase
- [ ] 4. 控制器：TaktXxxsController（复数），只注入 ITaktXxxService；❌ 控制器内 SqlSugar
- [ ] 5. Validator：FluentValidation，Application 层
- [ ] 6. 列表/详情/导入导出：实现要点见 01-backend §五；路由/权限见 12-crud §三
- [ ] 7. i18n 种子 + 菜单 Permission（00-project §1.6 / §1.7）
- [ ] 8. 分页：QueryExpression + GetPagedAsync；pageSize clamp（06-overflow-csharp）
- [ ] 9. XML 注释：类、公开方法、QueryExpression/FillXxxDto
- [ ] 10. 空行：03-format-blank-lines
```

## 分层铁律

```
WebApi → Application + Infrastructure + Shared
Application → Domain + Shared（❌ 不引用 Infrastructure）
Domain → Shared
```

❌ 控制器内 SqlSugar 查询  
❌ 控制器注入仓储  
✅ 异常 try/catch + HandleException；业务错误 ThrowBusinessExceptionLocalized

## 实体基类选型

| 基类 | 场景 |
|------|------|
| TaktTenantEntityBase | 租户级共享 |
| TaktCompanyEntityBase | 公司级业务 |
| TaktApprovalEntityBase | 带审批字段（简单态）；复杂流走 Workflow 引擎 |

## 代码生成（可选）

`scripts/generate-services-from-dtos.cjs`、`generate-controllers-from-services.cjs` — 控制器**复数**、服务**单数**。

## 代码生成

全栈流水线见 [15-codegen](../15-codegen/SKILL.md)。新建实体：`node scripts/generate-all.cjs --<Entity>` → build → 人工审阅 QueryExpression/权限。

## 交叉规则

- **CRUD 基线（强制）**：`12-crud`
- **代码生成**：`15-codegen`
- Helper：`04-utils-csharp`
- 溢出/分页/导出：`06-overflow-csharp`、`08-overflow-fullstack`
- 主子表：`10-master-detail`
- 树表：`11-tree-table`
- 工作流引擎：`09-workflow`（非普通 CRUD 控制器）
