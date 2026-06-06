---
name: 08-overflow-fullstack
description: >-
  审查 Takt 前后端联动溢出（分页贯穿、主键 string、导出双端上限、事故链）。
  用于列表/导出联调、Code Review、全量拉数排查，
  或用户提到 08-overflow-fullstack 时。
---

# 前后端联动溢出陷阱

完整规范：`.cursor/rules/08-overflow-fullstack.mdc`

**真正的安全来自前后端共同设限，而不是某一端「扛住」。**

## 经典事故链（禁止）

```text
Vue 全量请求 → C# 全表 ToList → JSON 爆炸 → 浏览器卡死
```

## 联调检查清单

```
- [ ] 列表：前后端都分页（TaktPagedQuery；禁止 pageSize 9999）
- [ ] 主键：types ↔ DTO ↔ JSON 贯穿 string
- [ ] 导出：流式/分批 + Excel 行数上限双端一致
- [ ] 表格：大数据 virtual 或阈值自动开启
- [ ] 下拉/树：选项数上限 + coerceSelectValue
- [ ] 递归：树/设计器深度 ≤10
- [ ] 溢出/OOM 当 bug 修，禁止 catch 后静默成功
- [ ] 改列表/导出须同时检查 06-overflow-csharp 与 07-overflow-vue
```

## 正确架构

```text
Vue（分页 + virtual） → API（分页 + DTO 裁剪） → C#（IQueryable + Take） → DB
```

## 统一对照

| 场景 | Vue | C# |
|------|-----|-----|
| 数据太多 | 虚拟列表 | 分页 + Take |
| 数字太大 | string | DTO string 传 long |
| 内存暴涨 | 不拉全量 | 不 ToList 全表 |
| JSON 爆炸 | 不解析巨型 payload | DTO 投影 |
