// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：setting.d.ts
// 创建时间：2026-06-27
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
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

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
  settingDescription?: string;

  /**
   * 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  settingGroup?: string;

  /**
   * 值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  valueType?: string;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 只读（字典 sys_yes_no_type；0=否 1=是）
   */
  isReadonly?: number;

  /**
   * 加密（字典 sys_yes_no_type；0=否 1=是）
   */
  isEncrypted?: number;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  settingStatus?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

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
  settingDescription?: string;

  /**
   * 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  settingGroup: string;

  /**
   * 值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  valueType: string;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 只读（字典 sys_yes_no_type；0=否 1=是）
   */
  isReadonly: number;

  /**
   * 加密（字典 sys_yes_no_type；0=否 1=是）
   */
  isEncrypted: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  settingStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

