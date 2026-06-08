---
name: 14-vue-form
description: >-
  新建或审查 Takt 表单组件 *-form.vue（单表/主子表/树表）。
  用于 views/**/components/*-form.vue、defineExpose、TaktTreeSelect 父级、
  或用户提到 14-vue-form、弹窗表单、级联 items 时。
---

# Vue 表单组件（*-form.vue）

完整规范：`.cursor/rules/14-vue-form.mdc`

**前置（强制）**：[12-crud](../12-crud/SKILL.md)

| 形态 | 参照 form.vue |
|------|---------------|
| 单表 | `views/identity/user/components/user-form.vue` |
| 主子表 | `views/foundation/dict/components/dict-type-form.vue` |
| 树表 | `views/human-resource/organization/dept/components/dept-form.vue` |

列表页集成 → [13-vue-view](../13-vue-view/SKILL.md)

## 通用清单（三种均须）

```
- [ ] 8 行 HTML 头；Props：formData?、loading?
- [ ] defineExpose：validate、getValues、resetFields（+ setServerValidationErrors 可选）
- [ ] a-form horizontal；标签 t('entity.*')；占位 common.page.form.placeholder.*
- [ ] TaktSelect / TaktTreeSelect；主键与 long 字段 string
- [ ] 03-format-blank-lines
```

## 单表增量

```
- [ ] formState + rules；watch formData 灌入/reset
- [ ] getValues → Create/Update DTO；编辑只读字段 disabled
- [ ] 复杂业务 a-tabs + locales {路径}.page.tabs.*
```

## 主子表增量

```
- [ ] Tab 主表：mainFormRef / mainFormState / mainFormRules
- [ ] Tab 子表：dictDataList/items 行内编辑或子组件
- [ ] 临时行 row-key client-${uuid}；持久化行 Id string
- [ ] validate：主表 + 子表规则；getValues 含 items 数组
- [ ] 与后端 CreateDto 级联字段名一致（10-master-detail）
```

## 树表增量

```
- [ ] parentId：TaktTreeSelect + /api/TaktXxxs/tree-options
- [ ] sortOrder：a-input-number min 0
- [ ] getValues parentId 为 string；根节点与后端约定（通常 "0"）
- [ ] index 新增时默认 parentId = 左侧选中树节点
```

## 父级提交模式

```typescript
await formRef.value?.validate()
const dto = formRef.value?.getValues()
// createXxx(dto) 或 updateXxx(id, dto)
```

## 代码生成

| 形态 | 脚本 |
|------|------|
| 单表 | `generate-vue-crud-from-api.cjs` |
| 主子表 | `generate-vue-master-detail-from-api.cjs` |
| 树表 | `generate-vue-tree-from-api.cjs` |

## 交叉规则

- 视图壳：`13-vue-view`
- 代码生成：`15-codegen`
- 主子表后端：`10-master-detail`
- 树表后端/API：`11-tree-table`
- UI/i18n：`02-frontend`
