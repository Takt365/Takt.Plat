---
name: 11-tree-table
description: >-
  树表 ParentId 扩展（GetXxxTreeAsync、左树右表、拍平分页）；须先满足 12-crud（节点完整 CRUD）。
  用于部门/菜单树、tree 模板、takt-tree-left-table。
---

# 树表（Tree Table）

完整规范：`.cursor/rules/11-tree-table.mdc`

**前置（强制）**：[12-crud](../12-crud/SKILL.md) — 节点实体须 `GetXxxListAsync` / Create / Update / Delete 等（参照 `TaktDeptService` + `TaktDeptsController`）。

## 标准参照

| 层 | 文件 |
|----|------|
| 后端 | `TaktDept.cs`、`TaktDeptService.cs`（list + tree 并存） |
| 控制器 | `TaktDeptsController.cs` |
| 前端 | `dept/index.vue`、`takt-tree-left-table`、`takt-tree-right-table` |

## 交付顺序

```
1. 12-crud 基线（节点 CRUD + 页面壳 + 权限）
2. 本章扩展（ParentId、GetXxxTreeAsync、左树右表、拍平分页）
```

## 树表扩展清单

```
- [ ] 1. 实体：ParentId、SortOrder、Level、Path（推荐）
- [ ] 2. GetXxxTreeAsync + BuildXxxTree；GetXxxTreeOptionsAsync
- [ ] 3. Create/Update 校验父节点、防环；Delete 有子节点策略
- [ ] 4. 控制器：GET tree、tree-options（叠加在 12-crud 路由之上）
- [ ] 5. 前端：getXxxTree；拍平 → 客户端分页 + virtual
- [ ] 6. 拖拽改 parentId/sortOrder → updateXxx（12-crud）
- [ ] 7. 树深 ≤10；07-overflow-vue
```

## 禁止

- 只有 tree 接口、缺少节点 Create/Update/Delete（违反 12-crud）
- 万级节点不拍平、不分页

## 交叉规则

- CRUD 基线：`12-crud`（强制前置）
- 代码生成：`15-codegen`（ParentId → tree API；Vue 须手工 13/14）
- 视图/表单：`13-vue-view`、`14-vue-form`
- 外键明细：`10-master-detail`
