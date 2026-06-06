---
name: 04-utils-csharp
description: >-
  新建或审查 Takt C# Helper/Utils（无状态、参数校验、XML 注释、泛型约束）。
  用于 backend Helpers、Takt.Shared/Helpers、Excel/字符串/正则工具类，
  或用户提到 04-utils-csharp、Helper 规范时。
---

# Takt C# 工具类（Helper）

完整规范：`.cursor/rules/04-utils-csharp.mdc`

## 五条硬约束（必检）

1. **禁止**静态工具存可变状态（网关类须在 remarks 说明）
2. **禁止**在 `Helpers/` 写扩展方法（扩展放 `Extensions/`）
3. 泛型 **必须** `where` 约束
4. 公开方法 **必须** 入口 `ThrowIfNull` / `ThrowIfNullOrWhiteSpace`
5. 公开方法 **完整 XML**（`<param>`、`<returns>`、`<exception>`）

## 新建 Helper 流程

```
- [ ] 1. 对照 TaktNamingHelper.cs 结构与注释密度
- [ ] 2. 确认职责域单一（不混 FTP+字符串+权限）
- [ ] 3. public static class TaktXxxHelper
- [ ] 4. 八段文件头（00-project §二）
- [ ] 5. 纯函数：副作用 I/O 须在方法名/XML 明示（参照 TaktFileHelper）
- [ ] 6. Shared 层禁止 TaktBusinessException（用 ArgumentException）
- [ ] 7. Try 语义：TryXxx + 明确返回值，禁止空 catch 吞异常
- [ ] 8. dotnet build 通过（含 CheckForOverflowUnderflow）
- [ ] 9. 空行：03-format-blank-lines
```

## 参照文件

| 类型 | 路径 |
|------|------|
| 纯工具 | `TaktNamingHelper.cs` |
| 字符串 | `TaktStringHelper.cs` |
| 泛型 I/O | `TaktExcelHelper.cs`（`where T : class`） |
| 网关 | `TaktLogger.cs`、`TaktLocationHelper.cs`（类 remarks 标明） |

## 与前端对齐

修改 naming/regex/mask 时同步检查 `05-utils-vue` 与 `frontend/src/utils/` 对应文件。
