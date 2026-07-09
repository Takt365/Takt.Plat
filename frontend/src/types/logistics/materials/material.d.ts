// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantDtoBase
} from '@/types/common';

/**
 * Takt全局物料实体（租户内共享主数据；工厂维度扩展见 TaktMaterialPlant）
 * 对应前端 TaktMaterialDto
 * 继承 TaktTenantDtoBase
 * 对应前端 Material
 * @description 对应后端 TaktMaterialDto
 */
export interface Material extends TenantDtoBase {
  /**
   * MaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialId: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector: string;

  /**
   * 物料层级
   */
  materialHierarchy?: string;

  /**
   * 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  isEndOfLife: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  materialStatus: number;

}


/**
 * Material 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialQuery
 * @description 对应后端 TaktMaterialQueryDto
 */
export interface MaterialQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector?: string;

  /**
   * 物料层级
   */
  materialHierarchy?: string;

  /**
   * 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  isEndOfLife?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  materialStatus?: number;

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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Material DTO
 * 对应前端 MaterialCreate
 * @description 对应后端 TaktMaterialCreateDto
 */
export interface MaterialCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector: string;

  /**
   * 物料层级
   */
  materialHierarchy?: string;

  /**
   * 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  isEndOfLife: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  materialStatus: number;

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
 * 更新Material DTO
 * 继承 TaktMaterialCreateDto，添加 MaterialId 字段
 * 对应前端 MaterialUpdate
 * @description 对应后端 TaktMaterialUpdateDto
 */
export interface MaterialUpdate extends MaterialCreate {
  /**
   * MaterialID（标识要更新的实体）
   */
  materialId: string;

}


/**
 * Material 状态更新 DTO
 * 对应前端 MaterialStatus
 * @description 对应后端 TaktMaterialStatusDto
 */
export interface MaterialStatus {
  /**
   * MaterialID
   */
  materialId: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  materialStatus: number;

}


/**
 * Material 导入模板行 DTO
 * 对应前端 MaterialTemplate
 * @description 对应后端 TaktMaterialTemplateDto
 */
export interface MaterialTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector?: string;

  /**
   * 物料层级
   */
  materialHierarchy?: string;

  /**
   * 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  isEndOfLife?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  materialStatus?: number;

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
 * Material 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialImport
 * @description 对应后端 TaktMaterialImportDto
 */
export interface MaterialImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector?: string;

  /**
   * 物料层级
   */
  materialHierarchy?: string;

  /**
   * 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  isEndOfLife?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  materialStatus?: number;

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
 * Material 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialExport
 * @description 对应后端 TaktMaterialExportDto
 */
export interface MaterialExport {
  /**
   * MaterialID
   */
  materialId: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector: string;

  /**
   * 物料层级
   */
  materialHierarchy?: string;

  /**
   * 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  isEndOfLife: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  materialStatus: number;

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

