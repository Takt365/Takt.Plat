// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：manufacturer-material.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt制造商物料明细实体
 * 对应前端 TaktManufacturerMaterialDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ManufacturerMaterial
 * @description 对应后端 TaktManufacturerMaterialDto
 */
export interface ManufacturerMaterial extends CompanyDtoBase {
  /**
   * ManufacturerMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  manufacturerMaterialId: string;

  /**
   * 制造商ID（关联TaktManufacturer主表）
   */
  manufacturerId: string;

  /**
   * 制造商名称（填充字段）
   */
  manufacturerName?: string;

  /**
   * 制造商编码（冗余字段，便于查询）
   */
  manufacturerCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType: number;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode: string;

  /**
   * 制造商物料名称（制造商内部的物料名称）
   */
  manufacturerMaterialName: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

  /**
   * 物料编码（对应的内部物料编码）
   */
  materialCode: string;

}


/**
 * ManufacturerMaterial 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ManufacturerMaterialQuery
 * @description 对应后端 TaktManufacturerMaterialQueryDto
 */
export interface ManufacturerMaterialQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 制造商ID（关联TaktManufacturer主表）
   */
  manufacturerId?: string;

  /**
   * 制造商编码（冗余字段，便于查询）
   */
  manufacturerCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType?: number;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 制造商物料名称（制造商内部的物料名称）
   */
  manufacturerMaterialName?: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

  /**
   * 物料编码（对应的内部物料编码）
   */
  materialCode?: string;

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
 * 创建ManufacturerMaterial DTO
 * 对应前端 ManufacturerMaterialCreate
 * @description 对应后端 TaktManufacturerMaterialCreateDto
 */
export interface ManufacturerMaterialCreate {
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
   * 制造商ID（关联TaktManufacturer主表）
   */
  manufacturerId: string;

  /**
   * 制造商编码（冗余字段，便于查询）
   */
  manufacturerCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType: number;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode: string;

  /**
   * 制造商物料名称（制造商内部的物料名称）
   */
  manufacturerMaterialName: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

  /**
   * 物料编码（对应的内部物料编码）
   */
  materialCode: string;

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
 * 更新ManufacturerMaterial DTO
 * 继承 TaktManufacturerMaterialCreateDto，添加 ManufacturerMaterialId 字段
 * 对应前端 ManufacturerMaterialUpdate
 * @description 对应后端 TaktManufacturerMaterialUpdateDto
 */
export interface ManufacturerMaterialUpdate extends ManufacturerMaterialCreate {
  /**
   * ManufacturerMaterialID（标识要更新的实体）
   */
  manufacturerMaterialId: string;

}


/**
 * ManufacturerMaterial 导入模板行 DTO
 * 对应前端 ManufacturerMaterialTemplate
 * @description 对应后端 TaktManufacturerMaterialTemplateDto
 */
export interface ManufacturerMaterialTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 制造商ID（关联TaktManufacturer主表）
   */
  manufacturerId?: string;

  /**
   * 制造商编码（冗余字段，便于查询）
   */
  manufacturerCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType?: number;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 制造商物料名称（制造商内部的物料名称）
   */
  manufacturerMaterialName?: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

  /**
   * 物料编码（对应的内部物料编码）
   */
  materialCode?: string;

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
 * ManufacturerMaterial 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ManufacturerMaterialImport
 * @description 对应后端 TaktManufacturerMaterialImportDto
 */
export interface ManufacturerMaterialImport {
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
   * 制造商ID（关联TaktManufacturer主表）
   */
  manufacturerId?: string;

  /**
   * 制造商编码（冗余字段，便于查询）
   */
  manufacturerCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType?: number;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode?: string;

  /**
   * 制造商物料名称（制造商内部的物料名称）
   */
  manufacturerMaterialName?: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

  /**
   * 物料编码（对应的内部物料编码）
   */
  materialCode?: string;

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
 * ManufacturerMaterial 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ManufacturerMaterialExport
 * @description 对应后端 TaktManufacturerMaterialExportDto
 */
export interface ManufacturerMaterialExport {
  /**
   * ManufacturerMaterialID
   */
  manufacturerMaterialId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 制造商ID（关联TaktManufacturer主表）
   */
  manufacturerId: string;

  /**
   * 制造商编码（冗余字段，便于查询）
   */
  manufacturerCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType: number;

  /**
   * 制造商物料编码（制造商内部的物料编号）
   */
  manufacturerMaterialCode: string;

  /**
   * 制造商物料名称（制造商内部的物料名称）
   */
  manufacturerMaterialName: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

  /**
   * 物料编码（对应的内部物料编码）
   */
  materialCode: string;

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

