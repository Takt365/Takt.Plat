---
name: 10-master-detail
description: >-
  主子表 OneToMany 扩展（级联 Fill/Save、展开/抽屉子表）；须先满足 12-crud（主表+子表双实体 CRUD）。
  用于 master-detail、字典类型/数据、TaktXxxItem、sub 模板。
---

# 主子表（Master-Detail）

完整规范：`.cursor/rules/10-master-detail.mdc`

**前置（强制）**：[12-crud](../12-crud/SKILL.md) — 主表与子表实体各须完整 CRUD（参照 `TaktDictTypes` + `TaktDictDatas`）。

## 标准参照

| 层 | 文件 |
|----|------|
| 后端级联 | `TaktDictTypeService.cs` |
| 后端子表 CRUD | `TaktDictDataService.cs`、`TaktDictDatasController.cs` |
| 前端 | `views/foundation/dict/index.vue`、`takt-master-detail-table` |

## 交付顺序

```
1. 12-crud 基线（主表 + 子表双控制器/双 API）
2. 本章扩展（Fill/Save、展开懒加载、子表分页过滤）
```

## 主子表扩展清单

```
- [ ] 1. 主表 GetXxxListAsync 不加载全量子表（12-crud 列表规范）
- [ ] 2. FillXxxDetailsAsync + SaveXxxChildrenAsync（先删后插或文档化策略）
- [ ] 3. 子表 GetYyyListAsync：QueryDto 含主表 Id + 分页
- [ ] 4. 前端：展开/抽屉时 getYyyList({ xxxId, pageIndex, pageSize })
- [ ] 5. 表单：Create/Update DTO 可级联 items 数组
- [ ] 6. 外键 string；06/07/08 双端分页
```

## 禁止

- 跳过 12-crud 直接在主表 list 塞全量 items
- 子表无独立 list 且无 pageSize 上限

## 交叉规则

- CRUD 基线：`12-crud`（强制前置）
- 代码生成：`15-codegen`（后端+Vue 主子表）；复杂 UI 手工增强
- 视图/表单：`13-vue-view`、`14-vue-form`
- 树形父子：`11-tree-table`
