// ========================================
// 项目名称：节节拍工厂·Takt Plat
// 命名空间：@/utils/takt-login-i18n
// 文件名称：takt-login-i18n.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：登录页 I18n 键常量（对齐后端种子 entity.* / common.*；无后端键的邮箱/手机标签见 login.page.field.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 用户名（entity.user.name） */
export const LOGIN_ENTITY_USER_NAME_KEY = 'entity.user.name';

/** 密码（entity.user.password） */
export const LOGIN_ENTITY_USER_PASSWORD_KEY = 'entity.user.password';

/** 租户编码（common.field.tenant.code） */
export const LOGIN_FIELD_TENANT_CODE_KEY = 'common.field.tenant.code';

/** 登录凭证（common.field.login.credentials） */
export const LOGIN_FIELD_CREDENTIALS_KEY = 'common.field.login.credentials';

/** 验证码（common.field.captcha.label） */
export const LOGIN_FIELD_CAPTCHA_KEY = 'common.field.captcha.label';

/** 用户名或邮箱（common.field.username.or.email） */
export const LOGIN_FIELD_USERNAME_OR_EMAIL_KEY = 'common.field.username.or.email';

/** 无权登录所选租户（common.permission.tenant.no.access） */
export const LOGIN_PERMISSION_TENANT_NO_ACCESS_KEY = 'common.permission.tenant.no.access';

/** 不支持在线重置密码（common.tip.reset.password.unavailable） */
export const LOGIN_TIP_RESET_PASSWORD_UNAVAILABLE_KEY = 'common.tip.reset.password.unavailable';

/** 密码重置邮件已发送（common.feedback.email.sent） */
export const LOGIN_FEEDBACK_EMAIL_SENT_KEY = 'common.feedback.email.sent';

/** 注册邮箱字段标签（后端无 entity.user.email，仅前端静态） */
export const LOGIN_PAGE_EMAIL_LABEL_KEY = 'login.page.field.email.label';

/** 注册手机号字段标签（后端无 entity.user.phone，仅前端静态） */
export const LOGIN_PAGE_PHONE_LABEL_KEY = 'login.page.field.phone.label';
