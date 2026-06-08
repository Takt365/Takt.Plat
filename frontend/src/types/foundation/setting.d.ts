// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：setting.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 系统设置实体 存储系统的各种配置参数，支持租户级配置隔离
 * 对应前端 TaktSettingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Setting
 * @description 对应后端 TaktSettingDto
 */
export interface Setting extends CompanyDtoBase {
  /**
   * SettingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  settingId: string;

  /**
   * 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
   */
  settingKey: string;

  /**
   * 设置值（字符串形式，复杂对象用JSON）
   */
  settingValue?: string;

  /**
   * 设置名称（显示名称，如：站点名称、最大上传大小）
   */
  settingName: string;

  /**
   * 设置描述
   */
  description?: string;

  /**
   * 设置类别（0=前端，1=后端）
   */
  settingGroup: number;

  /**
   * 值类型（用于前端渲染不同的输入控件）
   */
  valueType: number;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn: number;

  /**
   * 是否只读（0=否，1=是，只读设置不可修改）
   */
  isReadonly: number;

  /**
   * 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
   */
  isEncrypted: number;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * Setting 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SettingQuery
 * @description 对应后端 TaktSettingQueryDto
 */
export interface SettingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
   */
  settingKey?: string;

  /**
   * 设置值（字符串形式，复杂对象用JSON）
   */
  settingValue?: string;

  /**
   * 设置名称（显示名称，如：站点名称、最大上传大小）
   */
  settingName?: string;

  /**
   * 设置描述
   */
  description?: string;

  /**
   * 设置类别（0=前端，1=后端）
   */
  settingGroup?: number;

  /**
   * 值类型（用于前端渲染不同的输入控件）
   */
  valueType?: number;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn?: number;

  /**
   * 是否只读（0=否，1=是，只读设置不可修改）
   */
  isReadonly?: number;

  /**
   * 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
   */
  isEncrypted?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Setting DTO
 * 对应前端 SettingCreate
 * @description 对应后端 TaktSettingCreateDto
 */
export interface SettingCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
   */
  settingKey: string;

  /**
   * 设置值（字符串形式，复杂对象用JSON）
   */
  settingValue?: string;

  /**
   * 设置名称（显示名称，如：站点名称、最大上传大小）
   */
  settingName: string;

  /**
   * 设置描述
   */
  description?: string;

  /**
   * 设置类别（0=前端，1=后端）
   */
  settingGroup: number;

  /**
   * 值类型（用于前端渲染不同的输入控件）
   */
  valueType: number;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn: number;

  /**
   * 是否只读（0=否，1=是，只读设置不可修改）
   */
  isReadonly: number;

  /**
   * 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
   */
  isEncrypted: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Setting DTO
 * 继承 TaktSettingCreateDto，添加 SettingId 字段
 * 对应前端 SettingUpdate
 * @description 对应后端 TaktSettingUpdateDto
 */
export interface SettingUpdate extends SettingCreate {
  /**
   * SettingID（标识要更新的实体）
   */
  settingId: string;

}


/**
 * Setting 排序更新 DTO
 * 对应前端 SettingSort
 * @description 对应后端 TaktSettingSortDto
 */
export interface SettingSort {
  /**
   * SettingID
   */
  settingId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * Setting 导入模板行 DTO
 * 对应前端 SettingTemplate
 * @description 对应后端 TaktSettingTemplateDto
 */
export interface SettingTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
   */
  settingKey?: string;

  /**
   * 设置值（字符串形式，复杂对象用JSON）
   */
  settingValue?: string;

  /**
   * 设置名称（显示名称，如：站点名称、最大上传大小）
   */
  settingName?: string;

  /**
   * 设置描述
   */
  description?: string;

  /**
   * 设置类别（0=前端，1=后端）
   */
  settingGroup?: number;

  /**
   * 值类型（用于前端渲染不同的输入控件）
   */
  valueType?: number;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn?: number;

  /**
   * 是否只读（0=否，1=是，只读设置不可修改）
   */
  isReadonly?: number;

  /**
   * 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
   */
  isEncrypted?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Setting 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SettingImport
 * @description 对应后端 TaktSettingImportDto
 */
export interface SettingImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
   */
  settingKey?: string;

  /**
   * 设置值（字符串形式，复杂对象用JSON）
   */
  settingValue?: string;

  /**
   * 设置名称（显示名称，如：站点名称、最大上传大小）
   */
  settingName?: string;

  /**
   * 设置描述
   */
  description?: string;

  /**
   * 设置类别（0=前端，1=后端）
   */
  settingGroup?: number;

  /**
   * 值类型（用于前端渲染不同的输入控件）
   */
  valueType?: number;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn?: number;

  /**
   * 是否只读（0=否，1=是，只读设置不可修改）
   */
  isReadonly?: number;

  /**
   * 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
   */
  isEncrypted?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Setting 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SettingExport
 * @description 对应后端 TaktSettingExportDto
 */
export interface SettingExport {
  /**
   * SettingID
   */
  settingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
   */
  settingKey: string;

  /**
   * 设置值（字符串形式，复杂对象用JSON）
   */
  settingValue?: string;

  /**
   * 设置名称（显示名称，如：站点名称、最大上传大小）
   */
  settingName: string;

  /**
   * 设置描述
   */
  description?: string;

  /**
   * 设置类别（0=前端，1=后端）
   */
  settingGroup: number;

  /**
   * 值类型（用于前端渲染不同的输入控件）
   */
  valueType: number;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn: number;

  /**
   * 是否只读（0=否，1=是，只读设置不可修改）
   */
  isReadonly: number;

  /**
   * 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
   */
  isEncrypted: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

