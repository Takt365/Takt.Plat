---
name: 07-overflow-vue
description: >-
  审查或实现 Takt Vue 溢出安全边界（虚拟列表、分页、主键 string、递归深度）。
  用于 frontend 列表/表格/树/流程设计器、Number.MAX_SAFE_INTEGER、
  或用户提到 07-overflow-vue 时。
---

# Vue 溢出与安全边界

完整规范：`.cursor/rules/07-overflow-vue.mdc`

铁律索引：`00-project.mdc` §五；联调见 `08-overflow-fullstack`。

## 检查清单

```
- [ ] 列表必须分页 API；禁止 v-for 绑定未分页全量业务数组
- [ ] 大列表：virtual（takt-single-table / takt-tree-right-table 等）
- [ ] types/API：id: string；禁止 Number(雪花Id)
- [ ] 下拉/树 apiUrl：选项数上限；coerceSelectValue forceString
- [ ] 树/流程设计器：递归深度 ≤10；分支 ≤20
- [ ] 大数组日志：sampleForLog，禁止 JSON.stringify 全量
- [ ] 纯工具采样：05-utils-vue §十一
```

## 五类 Vue 溢出

| 类型 | 手段 |
|------|------|
| 渲染 | 虚拟列表 + 分页 |
| 数据量 | 分页 + 懒加载 |
| 递归 | 迭代；树深 ≤10 |
| 数字 | string / BigInt |
| JSON | 字段裁剪；拒绝巨型 payload |

## 交叉规则

- 后端分页/导出上限：`06-overflow-csharp`
- 联调事故链：`08-overflow-fullstack`
