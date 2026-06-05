// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/identity
// 文件名称：login.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================


/**
 * 登录密码 RSA 公钥响应
 * 对应前端 LoginPublicKeyResponse
 * @description 对应后端 TaktLoginPublicKeyResponseDto
 */
export interface LoginPublicKeyResponse {
  /**
   * 算法标识（RSA-PKCS1）
   */
  algorithm: string;

  /**
   * RSA 公钥 PEM
   */
  publicKeyPem: string;

}


/**
 * 用户登录请求 DTO
 * 对应前端 LoginRequest
 * @description 对应后端 TaktLoginRequestDto
 */
export interface LoginRequest {
  /**
   * 用户名（8位）
   */
  username: string;

  /**
   * 密码（RSA PKCS#1 密文 Base64；signin 有 LoginTicket 时可省略）
   */
  password: string;

  /**
   * 租户编码（用于多租户登录）
   */
  tenantCode?: string;

  /**
   * 公司编码（可选；未传时后端按 takt_identity_user_company.is_default 解析）
   */
  companyCode?: string;

  /**
   * 区域文化编码（zh-CN / en-US / ja-JP；界面语言，登录公司解析不依赖此字段）
   */
  cultureCode?: string;

  /**
   * 验证码（如果启用）
   */
  captchaCode?: string;

  /**
   * 验证码ID（如果启用）
   */
  captchaId?: string;

  /**
   * 记住我（延长Token有效期）
   */
  rememberMe: boolean;

  /**
   * 登录票据（由 session/verify-password 签发；signin 凭此跳过重复验密）
   */
  loginTicket?: string;

}


/**
 * 登录预检请求 DTO（点登录：先租户用户权限，再密码；通过后决定是否弹验证码）
 * 对应前端 SessionVerifyPasswordRequest
 * @description 对应后端 TaktSessionVerifyPasswordRequestDto
 */
export interface SessionVerifyPasswordRequest {
  /**
   * 用户名
   */
  username: string;

  /**
   * 密码（RSA PKCS#1 密文 Base64）
   */
  password: string;

  /**
   * 租户编码
   */
  tenantCode: string;

}


/**
 * 登录预检响应 DTO（密码已通过；captchaRequired 为 true 时前端弹验证码后再 signin）
 * 对应前端 SessionVerifyPasswordResponse
 * @description 对应后端 TaktSessionVerifyPasswordResponseDto
 */
export interface SessionVerifyPasswordResponse {
  /**
   * 密码是否通过
   */
  passwordValid: boolean;

  /**
   * 是否需要弹出验证码
   */
  captchaRequired: boolean;

  /**
   * 登录票据（短时有效，供 session/signin 使用）
   */
  loginTicket: string;

}


/**
 * 用户登录响应 DTO
 * 对应前端 LoginResponse
 * @description 对应后端 TaktLoginResponseDto
 */
export interface LoginResponse {
  /**
   * 访问令牌（Access Token）
   */
  accessToken: string;

  /**
   * 刷新令牌（Refresh Token）
   */
  refreshToken: string;

  /**
   * 令牌类型（通常为 Bearer）
   */
  tokenType: string;

  /**
   * 过期时间（秒）
   */
  expiresIn: number;

  /**
   * 用户ID
   */
  userId: string;

  /**
   * 用户名
   */
  username: string;

  /**
   * 昵称
   */
  nickname?: string;

  /**
   * 租户编码
   */
  tenantCode: string;

  /**
   * 公司编码
   */
  companyCode: string;

  /**
   * 用户类型
   */
  userType: number;

  /**
   * 角色列表
   */
  roles: string[];

  /**
   * 权限列表
   */
  permissions: string[];

  /**
   * 是否需要修改密码（首次登录或密码过期）
   */
  mustChangePassword: boolean;

  /**
   * 最后登录时间
   */
  lastLoginAt?: string;

  /**
   * 登录时间
   */
  loginAt: string;

}


/**
 * 刷新令牌请求 DTO
 * 对应前端 RefreshTokenRequest
 * @description 对应后端 TaktRefreshTokenRequestDto
 */
export interface RefreshTokenRequest {
  /**
   * 刷新令牌
   */
  refreshToken: string;

}


/**
 * 用户信息响应 DTO
 * 用于获取当前登录用户详细信息
 * 对应前端 UserInfoResponse
 * @description 对应后端 TaktUserInfoResponseDto
 */
export interface UserInfoResponse {
  /**
   * 用户ID
   */
  userId: string;

  /**
   * 用户名
   */
  username: string;

  /**
   * 昵称
   */
  nickname?: string;

  /**
   * 用户类型
   */
  userType: number;

  /**
   * 用户类型名称
   */
  switch: any;

  /**
   * 员工姓名（从员工表 name 关联）
   */
  employeeName?: string;

  /**
   * 员工性别（0=未知，1=男，2=女）
   */
  gender: number;

  /**
   * 员工手机号码
   */
  mobile?: string;

  /**
   * 员工电子邮箱
   */
  email?: string;

  /**
   * 用户头像 URL（来自员工档案 photo_url）
   */
  avatar?: string;

  /**
   * 租户编码
   */
  tenantCode: string;

  /**
   * 公司编码
   */
  companyCode: string;

  /**
   * 用户默认区域文化 BCP47（takt_identity_user.default_culture）
   */
  defaultCulture: string;

  /**
   * 当前公司默认区域文化 BCP47（takt_company.default_culture）
   */
  companyDefaultCulture: string;

  /**
   * 公司名称（从公司表关联）
   */
  companyName?: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  userStatus: number;

  /**
   * 状态名称
   */
  roles: string[];

  /**
   * 权限列表
   */
  permissions: string[];

  /**
   * 可访问的菜单树（目录与菜单）
   */
  menus: MenuTree[];

  /**
   * 可访问的前端路由路径列表
   */
  routePaths: string[];

  /**
   * 可访问的公司列表
   */
  accessibleCompanies: string[];

  /**
   * 最后登录时间
   */
  lastLoginAt?: string;

  /**
   * 最后登录IP
   */
  lastLoginIp?: string;

  /**
   * 登录次数
   */
  loginCount: number;

  /**
   * 密码过期天数（0=永不过期）
   */
  passwordExpireDays: number;

  /**
   * 密码是否即将过期（7天内）
   */
  isPasswordExpiringSoon: boolean;

  /**
   * 创建时间
   */
  createdAt: string;

  /**
   * 更新时间
   */
  updatedAt: string;

}


/**
 * 登录前预览：用户默认公司、用户 DefaultCulture 与假日主题（与 TaktUser / TaktUserCompany 对齐）
 * 对应前端 LoginPreviewLocale
 * @description 对应后端 TaktLoginPreviewLocaleDto
 */
export interface LoginPreviewLocale {
  /**
   * 租户在 TaktTenant 中存在且启用
   */
  tenantFound: boolean;

  /**
   * 用户在 TaktUser 中存在且启用
   */
  userFound: boolean;

  /**
   * 已解析到 is_default=Yes 的 TaktUserCompany 且对应 TaktCompany 启用
   */
  defaultCompanyFound: boolean;

  /**
   * 用户默认登录公司代码（takt_identity_user_company.is_default=Yes）
   */
  companyCode: string;

  /**
   * 用户默认区域文化 BCP47（takt_identity_user.default_culture，用于界面语言）
   */
  defaultCulture: string;

  /**
   * 公司默认区域文化 BCP47（takt_company.default_culture，用于业务数据 CRUD 语言校验）
   */
  companyDefaultCulture: string;

}

