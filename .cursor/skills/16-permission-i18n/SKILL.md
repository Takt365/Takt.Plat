---
name: 16-permission-i18n
description: >-
  Takt 前后端权限验证与翻译键对齐（Permission 冒号、I18nKey 点号、四处一致、401/403 链路）。
  用于菜单种子、TaktPermission、v-permission、entity.* / menu.*、locales 静态键。
---

# 权限验证与翻译键

完整规范：`.cursor/rules/16-permission-i18n.mdc`

## 两套体系（禁止混用）

| | Permission | I18nKey |
|---|------------|---------|
| 分隔符 | `:` 冒号 | `.` 点号 |
| 用途 | API/按钮/路由鉴权 | `t('…')` 文案 |
| 示例 | `identity:user:create` | `entity.user.UserName` |

## 权限码四处一致

```
1. TaktMenu / 按钮菜单种子 Permission
2. 控制器 [TaktPermission("…", "…")]
3. v-permission / TaktToolsBar *-permission
4. （可选）路由 meta.permission
```

CRUD 末段：`list` | `query` | `create` | `update` | `delete` | `import` | `export` → 见 `12-crud` §三

## 后端验证

```
[Authorize] → TaktPermissionAuthorizationFilter → HasUserPermissionAsync
未登录 401 | 无权限 403 | 无 TaktPermission 仅验登录
```

## 前端验证

```
登录 → permissionStore（用户 permissions + 菜单树 permission）
v-permission → display:none | canActivateRoute → 路由守卫
```

**UI 隐藏不能替代后端鉴权。**

## 翻译键

| 类型 | 键 | 来源 |
|------|-----|------|
| 动态 menu | `menu.*` | 菜单种子 I18nKey |
| 动态 entity | `entity.{slug}.*` | TaktXxxI18nSeedData / generate-entity-i18n-seed |
| 动态 common | `common.page.*` | TaktCommonI18nSeedData |
| 静态页面 | `{路径}.page.*` | `locales/**/zh-CN.ts`，根键 `page` |

❌ locales 不写 `entity.*` / `menu.*`  
❌ Permission 不用点号；I18nKey 不用冒号

## 自检

```
- [ ] 四处 Permission 字符串完全一致（含 MenuOther 附属嵌套码）
- [ ] 菜单 I18nKey（menu.*）≠ Permission（identity:…:list）
- [ ] 列/表单用 entity.*；按钮文案 common.button.*
- [ ] 生成代码后核对 15-codegen §五 权限项
- [ ] 无 SoftDeleteObsolete / 双入口兼容旧菜单（00-project §1.9）
- [ ] 无独立页面的附属 CRUD 已写入 TaktMenuOtherSeedData
```

## 交叉规则

- 格式权威：`00-project` §1.6 / §1.7 / **§1.9 全新写法**
- CRUD 映射：`12-crud` §三
- 代码生成：`15-codegen`
- 前端 i18n 落地：`02-frontend` §6
- 附属权限种子：`TaktMenuOtherSeedData`（挂父级 L2，形如 `…:room:create`）
