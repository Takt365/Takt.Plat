// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/code/generator
// 文件名称：gen-table-column.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：code/generator 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantDtoBase
} from '@/types/common';

/**
 * Takt代码生成字段配置实体
 * 对应前端 TaktGenTableColumnDto
 * 继承 TaktTenantDtoBase
 * 对应前端 GenTableColumn
 * @description 对应后端 TaktGenTableColumnDto
 */
export interface GenTableColumn extends TenantDtoBase {
  /**
   * GenTableColumnID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  genTableColumnId: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId: string;

  /**
   * 生成表名称（填充字段）
   */
  genTableName?: string;

  /**
   * 行号（字段在表中的排列顺序，从1开始）
   */
  lineNumber: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据库数据类型（如：varchar、int、datetime、decimal等）
   */
  databaseDataType: string;

  /**
   * C#类型（对应C#数据类型，如：string、int、long、DateTime、decimal、bool、Guid等）
   */
  csharpDataType: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits: number;

  /**
   * 是否主键（1=是，0=否）
   */
  isPk: number;

  /**
   * 是否自增（1=是，0=否）
   */
  isIncrement: number;

  /**
   * 是否必填（1=是，0=否）
   */
  isRequired: number;

  /**
   * 是否为新增字段（1=是，0=否）
   */
  isCreate: number;

  /**
   * 是否更新字段（1=是，0=否）
   */
  isUpdate: number;

  /**
   * 是否查重字段（1=是，0=否）
   */
  isUnique: number;

  /**
   * 是否列表字段（1=是，0=否）
   */
  isList: number;

  /**
   * 是否导出字段（1=是，0=否）
   */
  isExport: number;

  /**
   * 是否可排序字段（1=是，0=否）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort: number;

  /**
   * 是否查询字段（1=是，0=否）
   */
  isQuery: number;

  /**
   * 查询方式（EQ=等于，NE=不等于，GT=大于，GTE=大于等于，LT=小于，LTE=小于等于，LIKE=模糊，BETWEEN=范围）
   */
  queryType: string;

  /**
   * 显示类型（input=输入框，textarea=文本域，select=下拉框，checkbox=复选框，radio=单选框，date=日期控件，time=时间控件，image=图片上传，file=文件上传，slider=滑块，switch=开关，editor=富文本编辑器）
   */
  htmlType: string;

  /**
   * 字典类型（关联数据字典）
   */
  dictType?: string;

  /**
   * 排序序号
   */
  sortOrder: number;

  /**
   * 所属表配置（主表，本表 GenTableId 关联 TaktGenTable.Id） （主表：TaktGenTable）
   */
  table?: GenTable;

}


/**
 * GenTableColumn 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 GenTableColumnQuery
 * @description 对应后端 TaktGenTableColumnQueryDto
 */
export interface GenTableColumnQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId?: string;

  /**
   * 行号（字段在表中的排列顺序，从1开始）
   */
  lineNumber?: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName?: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据库数据类型（如：varchar、int、datetime、decimal等）
   */
  databaseDataType?: string;

  /**
   * C#类型（对应C#数据类型，如：string、int、long、DateTime、decimal、bool、Guid等）
   */
  csharpDataType?: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName?: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length?: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits?: number;

  /**
   * 是否主键（1=是，0=否）
   */
  isPk?: number;

  /**
   * 是否自增（1=是，0=否）
   */
  isIncrement?: number;

  /**
   * 是否必填（1=是，0=否）
   */
  isRequired?: number;

  /**
   * 是否为新增字段（1=是，0=否）
   */
  isCreate?: number;

  /**
   * 是否更新字段（1=是，0=否）
   */
  isUpdate?: number;

  /**
   * 是否查重字段（1=是，0=否）
   */
  isUnique?: number;

  /**
   * 是否列表字段（1=是，0=否）
   */
  isList?: number;

  /**
   * 是否导出字段（1=是，0=否）
   */
  isExport?: number;

  /**
   * 是否可排序字段（1=是，0=否）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort?: number;

  /**
   * 是否查询字段（1=是，0=否）
   */
  isQuery?: number;

  /**
   * 查询方式（EQ=等于，NE=不等于，GT=大于，GTE=大于等于，LT=小于，LTE=小于等于，LIKE=模糊，BETWEEN=范围）
   */
  queryType?: string;

  /**
   * 显示类型（input=输入框，textarea=文本域，select=下拉框，checkbox=复选框，radio=单选框，date=日期控件，time=时间控件，image=图片上传，file=文件上传，slider=滑块，switch=开关，editor=富文本编辑器）
   */
  htmlType?: string;

  /**
   * 字典类型（关联数据字典）
   */
  dictType?: string;

  /**
   * 排序序号
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
 * 创建GenTableColumn DTO
 * 对应前端 GenTableColumnCreate
 * @description 对应后端 TaktGenTableColumnCreateDto
 */
export interface GenTableColumnCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId: string;

  /**
   * 行号（字段在表中的排列顺序，从1开始）
   */
  lineNumber: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据库数据类型（如：varchar、int、datetime、decimal等）
   */
  databaseDataType: string;

  /**
   * C#类型（对应C#数据类型，如：string、int、long、DateTime、decimal、bool、Guid等）
   */
  csharpDataType: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits: number;

  /**
   * 是否主键（1=是，0=否）
   */
  isPk: number;

  /**
   * 是否自增（1=是，0=否）
   */
  isIncrement: number;

  /**
   * 是否必填（1=是，0=否）
   */
  isRequired: number;

  /**
   * 是否为新增字段（1=是，0=否）
   */
  isCreate: number;

  /**
   * 是否更新字段（1=是，0=否）
   */
  isUpdate: number;

  /**
   * 是否查重字段（1=是，0=否）
   */
  isUnique: number;

  /**
   * 是否列表字段（1=是，0=否）
   */
  isList: number;

  /**
   * 是否导出字段（1=是，0=否）
   */
  isExport: number;

  /**
   * 是否可排序字段（1=是，0=否）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort: number;

  /**
   * 是否查询字段（1=是，0=否）
   */
  isQuery: number;

  /**
   * 查询方式（EQ=等于，NE=不等于，GT=大于，GTE=大于等于，LT=小于，LTE=小于等于，LIKE=模糊，BETWEEN=范围）
   */
  queryType: string;

  /**
   * 显示类型（input=输入框，textarea=文本域，select=下拉框，checkbox=复选框，radio=单选框，date=日期控件，time=时间控件，image=图片上传，file=文件上传，slider=滑块，switch=开关，editor=富文本编辑器）
   */
  htmlType: string;

  /**
   * 字典类型（关联数据字典）
   */
  dictType?: string;

  /**
   * 排序序号
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
 * 更新GenTableColumn DTO
 * 继承 TaktGenTableColumnCreateDto，添加 GenTableColumnId 字段
 * 对应前端 GenTableColumnUpdate
 * @description 对应后端 TaktGenTableColumnUpdateDto
 */
export interface GenTableColumnUpdate extends GenTableColumnCreate {
  /**
   * GenTableColumnID（标识要更新的实体）
   */
  genTableColumnId: string;

}


/**
 * GenTableColumn 排序更新 DTO
 * 对应前端 GenTableColumnSort
 * @description 对应后端 TaktGenTableColumnSortDto
 */
export interface GenTableColumnSort {
  /**
   * GenTableColumnID
   */
  genTableColumnId: string;

  /**
   * 排序序号
   */
  sortOrder: number;

}


/**
 * GenTableColumn 导入模板行 DTO
 * 对应前端 GenTableColumnTemplate
 * @description 对应后端 TaktGenTableColumnTemplateDto
 */
export interface GenTableColumnTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId?: string;

  /**
   * 行号（字段在表中的排列顺序，从1开始）
   */
  lineNumber?: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName?: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据库数据类型（如：varchar、int、datetime、decimal等）
   */
  databaseDataType?: string;

  /**
   * C#类型（对应C#数据类型，如：string、int、long、DateTime、decimal、bool、Guid等）
   */
  csharpDataType?: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName?: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length?: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits?: number;

  /**
   * 是否主键（1=是，0=否）
   */
  isPk?: number;

  /**
   * 是否自增（1=是，0=否）
   */
  isIncrement?: number;

  /**
   * 是否必填（1=是，0=否）
   */
  isRequired?: number;

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
 * GenTableColumn 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 GenTableColumnImport
 * @description 对应后端 TaktGenTableColumnImportDto
 */
export interface GenTableColumnImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId?: string;

  /**
   * 行号（字段在表中的排列顺序，从1开始）
   */
  lineNumber?: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName?: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据库数据类型（如：varchar、int、datetime、decimal等）
   */
  databaseDataType?: string;

  /**
   * C#类型（对应C#数据类型，如：string、int、long、DateTime、decimal、bool、Guid等）
   */
  csharpDataType?: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName?: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length?: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits?: number;

  /**
   * 是否主键（1=是，0=否）
   */
  isPk?: number;

  /**
   * 是否自增（1=是，0=否）
   */
  isIncrement?: number;

  /**
   * 是否必填（1=是，0=否）
   */
  isRequired?: number;

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
 * GenTableColumn 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 GenTableColumnExport
 * @description 对应后端 TaktGenTableColumnExportDto
 */
export interface GenTableColumnExport {
  /**
   * GenTableColumnID
   */
  genTableColumnId: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId: string;

  /**
   * 行号（字段在表中的排列顺序，从1开始）
   */
  lineNumber: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据库数据类型（如：varchar、int、datetime、decimal等）
   */
  databaseDataType: string;

  /**
   * C#类型（对应C#数据类型，如：string、int、long、DateTime、decimal、bool、Guid等）
   */
  csharpDataType: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits: number;

  /**
   * 是否主键（1=是，0=否）
   */
  isPk: number;

  /**
   * 是否自增（1=是，0=否）
   */
  isIncrement: number;

  /**
   * 是否必填（1=是，0=否）
   */
  isRequired: number;

  /**
   * 是否为新增字段（1=是，0=否）
   */
  isCreate: number;

  /**
   * 是否更新字段（1=是，0=否）
   */
  isUpdate: number;

  /**
   * 是否查重字段（1=是，0=否）
   */
  isUnique: number;

  /**
   * 是否列表字段（1=是，0=否）
   */
  isList: number;

  /**
   * 是否导出字段（1=是，0=否）
   */
  isExport: number;

  /**
   * 是否可排序字段（1=是，0=否）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort: number;

  /**
   * 是否查询字段（1=是，0=否）
   */
  isQuery: number;

  /**
   * 查询方式（EQ=等于，NE=不等于，GT=大于，GTE=大于等于，LT=小于，LTE=小于等于，LIKE=模糊，BETWEEN=范围）
   */
  queryType: string;

  /**
   * 显示类型（input=输入框，textarea=文本域，select=下拉框，checkbox=复选框，radio=单选框，date=日期控件，time=时间控件，image=图片上传，file=文件上传，slider=滑块，switch=开关，editor=富文本编辑器）
   */
  htmlType: string;

  /**
   * 字典类型（关联数据字典）
   */
  dictType?: string;

  /**
   * 排序序号
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

