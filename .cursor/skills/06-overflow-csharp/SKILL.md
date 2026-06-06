---
name: 06-overflow-csharp
description: >-
  审查或实现 Takt C# 溢出安全边界（算术 checked、分页 clamp、内存/导出上限）。
  用于 backend 列表/导出/批处理、CheckForOverflowUnderflow、pageSize、
  或用户提到 06-overflow-csharp 时。
---

# C# 溢出与安全边界

完整规范：`.cursor/rules/06-overflow-csharp.mdc`

铁律索引：`00-project.mdc` §五；联调见 `08-overflow-fullstack`。

## 检查清单

```
- [ ] pageSize = Min(maxPageSize, Max(1, pageSize))；Skip 用 checked
- [ ] 禁止无分页 ToList() 返前端；禁止 EF 全表加载再内存过滤
- [ ] 导出/报表：流式或分批；Excel 行数上限（TaktExcelHelper MaxImportRowsPerFile）
- [ ] DTO 主键/long 对前端用 string；禁止实体整表输出
- [ ] OOM/OverflowException 不吞；当 bug 修
- [ ] Directory.Build.props：CheckForOverflowUnderflow=true
- [ ] 缓冲区：Span + 边界检查；禁止用户长度 stackalloc
```

## 分页（超高频）

```csharp
page = Math.Max(1, page);
pageSize = Math.Min(maxPageSize, Math.Max(1, pageSize));
var skip = checked((page - 1) * pageSize);
```

## 三类溢出手段

| 类型 | 手段 |
|------|------|
| 算术 | checked / long·decimal |
| 内存 | 预防；不吞 OOM |
| 缓冲 | Span；offset+count 校验 |

## 交叉规则

- Helper 参数校验：`04-utils-csharp`
- 前后端联调：`08-overflow-fullstack`
- 前端 virtual/主键：`07-overflow-vue`
