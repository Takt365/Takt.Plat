// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：dict-data.d.ts
// 创建时间：2026-06-02
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktSelectOption, TenantDtoBase } from '@/types/common';

/**
 * 字典数据实体 字典类型的具体数据项，如：订单状态下的“待支付”、“已完成”等 租户级实体：字典数据在租户内共享，不需要公司隔离
 * 对应前端 TaktDictDataDto
 * 继承 TaktTenantDtoBase
 * 对应前端 DictData
 * @description 对应后端 TaktDictDataDto
 */
export interface DictData extends TenantDtoBase {
  /**
   * DictDataID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  dictDataId: string;

  /**
   * 字典类型ID（关联 TaktDictType.Id）
   */
  dictTypeId: string;

  /**
   * 字典类型名称（填充字段）
   */
  dictTypeName?: string;

  /**
   * 字典类型编码（关联 TaktDictType.DictTypeCode）
   */
  dictTypeCode: string;

  /**
   * 字典项标签（唯一索引：租户内 DictTypeId+DictLabel+DictValue+I18nKey 唯一，见 ix_dict_data_type_label_value_i18n_unique；如：待支付、已完成）
   */
  dictLabel: string;

  /**
   * 字典项值（实际存储值，如：0, 1, 2）
   */
  dictValue: string;

  /**
   * 国际化翻译键（用于多语言支持，如：dict.user_type.admin）
   */
  i18nKey: string;

  /**
   * 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
   */
  extLabel?: string;

  /**
   * 扩展值（用于存储额外的业务数据，如：编码、标识符等）
   */
  extValue?: string;

  /**
   * 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
   */
  listClass: number;

  /**
   * CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
   */
  cssClass: number;

  /**
   * 是否默认项（1=是，0=否）
   */
  isDefault: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 字典类型（多对一关联） （主表：TaktDictType）
   */
  dictType?: DictType;

}


/**
 * DictData 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DictDataQuery
 * @description 对应后端 TaktDictDataQueryDto
 */
export interface DictDataQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 字典类型ID（关联 TaktDictType.Id）
   */
  dictTypeId?: string;

  /**
   * 字典类型编码（关联 TaktDictType.DictTypeCode）
   */
  dictTypeCode?: string;

  /**
   * 字典项标签（唯一索引：租户内 DictTypeId+DictLabel+DictValue+I18nKey 唯一，见 ix_dict_data_type_label_value_i18n_unique；如：待支付、已完成）
   */
  dictLabel?: string;

  /**
   * 字典项值（实际存储值，如：0, 1, 2）
   */
  dictValue?: string;

  /**
   * 国际化翻译键（用于多语言支持，如：dict.user_type.admin）
   */
  i18nKey?: string;

  /**
   * 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
   */
  extLabel?: string;

  /**
   * 扩展值（用于存储额外的业务数据，如：编码、标识符等）
   */
  extValue?: string;

  /**
   * 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
   */
  listClass?: number;

  /**
   * CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
   */
  cssClass?: number;

  /**
   * 是否默认项（1=是，0=否）
   */
  isDefault?: number;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建DictData DTO
 * 对应前端 DictDataCreate
 * @description 对应后端 TaktDictDataCreateDto
 */
export interface DictDataCreate {
  /**
   * 字典类型ID（关联 TaktDictType.Id）
   */
  dictTypeId: string;

  /**
   * 字典类型编码（关联 TaktDictType.DictTypeCode）
   */
  dictTypeCode: string;

  /**
   * 字典项标签（唯一索引：租户内 DictTypeId+DictLabel+DictValue+I18nKey 唯一，见 ix_dict_data_type_label_value_i18n_unique；如：待支付、已完成）
   */
  dictLabel: string;

  /**
   * 字典项值（实际存储值，如：0, 1, 2）
   */
  dictValue: string;

  /**
   * 国际化翻译键（用于多语言支持，如：dict.user_type.admin）
   */
  i18nKey: string;

  /**
   * 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
   */
  extLabel?: string;

  /**
   * 扩展值（用于存储额外的业务数据，如：编码、标识符等）
   */
  extValue?: string;

  /**
   * 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
   */
  listClass: number;

  /**
   * CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
   */
  cssClass: number;

  /**
   * 是否默认项（1=是，0=否）
   */
  isDefault: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新DictData DTO
 * 继承 TaktDictDataCreateDto，添加 DictDataId 字段
 * 对应前端 DictDataUpdate
 * @description 对应后端 TaktDictDataUpdateDto
 */
export interface DictDataUpdate extends DictDataCreate {
  /**
   * DictDataID（标识要更新的实体）
   */
  dictDataId: string;

}


/**
 * DictData 排序更新 DTO
 * 对应前端 DictDataSort
 * @description 对应后端 TaktDictDataSortDto
 */
export interface DictDataSort {
  /**
   * DictDataID
   */
  dictDataId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * DictData 导入模板行 DTO
 * 对应前端 DictDataTemplate
 * @description 对应后端 TaktDictDataTemplateDto
 */
export interface DictDataTemplate {
  /**
   * 字典类型ID（关联 TaktDictType.Id）
   */
  dictTypeId?: string;

  /**
   * 字典类型编码（关联 TaktDictType.DictTypeCode）
   */
  dictTypeCode?: string;

  /**
   * 字典项标签（唯一索引：租户内 DictTypeId+DictLabel+DictValue+I18nKey 唯一，见 ix_dict_data_type_label_value_i18n_unique；如：待支付、已完成）
   */
  dictLabel?: string;

  /**
   * 字典项值（实际存储值，如：0, 1, 2）
   */
  dictValue?: string;

  /**
   * 国际化翻译键（用于多语言支持，如：dict.user_type.admin）
   */
  i18nKey?: string;

  /**
   * 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
   */
  extLabel?: string;

  /**
   * 扩展值（用于存储额外的业务数据，如：编码、标识符等）
   */
  extValue?: string;

  /**
   * 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
   */
  listClass?: number;

  /**
   * CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
   */
  cssClass?: number;

  /**
   * 是否默认项（1=是，0=否）
   */
  isDefault?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * DictData 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DictDataImport
 * @description 对应后端 TaktDictDataImportDto
 */
export interface DictDataImport {
  /**
   * 字典类型ID（关联 TaktDictType.Id）
   */
  dictTypeId?: string;

  /**
   * 字典类型编码（关联 TaktDictType.DictTypeCode）
   */
  dictTypeCode?: string;

  /**
   * 字典项标签（唯一索引：租户内 DictTypeId+DictLabel+DictValue+I18nKey 唯一，见 ix_dict_data_type_label_value_i18n_unique；如：待支付、已完成）
   */
  dictLabel?: string;

  /**
   * 字典项值（实际存储值，如：0, 1, 2）
   */
  dictValue?: string;

  /**
   * 国际化翻译键（用于多语言支持，如：dict.user_type.admin）
   */
  i18nKey?: string;

  /**
   * 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
   */
  extLabel?: string;

  /**
   * 扩展值（用于存储额外的业务数据，如：编码、标识符等）
   */
  extValue?: string;

  /**
   * 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
   */
  listClass?: number;

  /**
   * CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
   */
  cssClass?: number;

  /**
   * 是否默认项（1=是，0=否）
   */
  isDefault?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * DictData 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DictDataExport
 * @description 对应后端 TaktDictDataExportDto
 */
export interface DictDataExport {
  /**
   * DictDataID
   */
  dictDataId: string;

  /**
   * 字典类型ID（关联 TaktDictType.Id）
   */
  dictTypeId: string;

  /**
   * 字典类型编码（关联 TaktDictType.DictTypeCode）
   */
  dictTypeCode: string;

  /**
   * 字典项标签（唯一索引：租户内 DictTypeId+DictLabel+DictValue+I18nKey 唯一，见 ix_dict_data_type_label_value_i18n_unique；如：待支付、已完成）
   */
  dictLabel: string;

  /**
   * 字典项值（实际存储值，如：0, 1, 2）
   */
  dictValue: string;

  /**
   * 国际化翻译键（用于多语言支持，如：dict.user_type.admin）
   */
  i18nKey: string;

  /**
   * 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
   */
  extLabel?: string;

  /**
   * 扩展值（用于存储额外的业务数据，如：编码、标识符等）
   */
  extValue?: string;

  /**
   * 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
   */
  listClass: number;

  /**
   * CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
   */
  cssClass: number;

  /**
   * 是否默认项（1=是，0=否）
   */
  isDefault: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

/**
 * 租户下全部字典数据响应
 * @description 对应后端 TaktDataDictAllDto
 */
export interface DictDataAll {
  /**
   * 字典项列表（含 dictTypeCode 供前端分组）
   */
  items: TaktSelectOption[];
}

