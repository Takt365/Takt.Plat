// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material-document.d.ts
// 创建时间：2026-07-05
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
 * Takt物料凭证主表实体（公司级；行项目见 TaktMaterialDocumentItem）
 * 对应前端 TaktMaterialDocumentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialDocument
 * @description 对应后端 TaktMaterialDocumentDto
 */
export interface MaterialDocument extends CompanyDtoBase {
  /**
   * MaterialDocumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialDocumentId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
   */
  materialCode: string;

  /**
   * 物料凭证号（租户+公司+工厂内唯一）
   */
  materialDocumentCode: string;

  /**
   * 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  postedBy?: string;

  /**
   * 物料凭证状态（0=草稿，1=已过账，2=已作废）
   */
  materialDocumentStatus: number;

  /**
   * 物料凭证行项目列表（主子表关系） （子表：TaktMaterialDocumentItem）
   */
  items?: MaterialDocumentItem[];

}


/**
 * MaterialDocument 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialDocumentQuery
 * @description 对应后端 TaktMaterialDocumentQueryDto
 */
export interface MaterialDocumentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
   */
  materialCode?: string;

  /**
   * 物料凭证号（租户+公司+工厂内唯一）
   */
  materialDocumentCode?: string;

  /**
   * 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  postedBy?: string;

  /**
   * 物料凭证状态（0=草稿，1=已过账，2=已作废）
   */
  materialDocumentStatus?: number;

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
 * 创建MaterialDocument DTO
 * 对应前端 MaterialDocumentCreate
 * @description 对应后端 TaktMaterialDocumentCreateDto
 */
export interface MaterialDocumentCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
   */
  materialCode: string;

  /**
   * 物料凭证号（租户+公司+工厂内唯一）
   */
  materialDocumentCode: string;

  /**
   * 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  postedBy?: string;

  /**
   * 物料凭证状态（0=草稿，1=已过账，2=已作废）
   */
  materialDocumentStatus: number;

  /**
   * 物料凭证行项目列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialDocumentItemCreate[];

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
 * 更新MaterialDocument DTO
 * 继承 TaktMaterialDocumentCreateDto，添加 MaterialDocumentId 字段
 * 对应前端 MaterialDocumentUpdate
 * @description 对应后端 TaktMaterialDocumentUpdateDto
 */
export interface MaterialDocumentUpdate extends MaterialDocumentCreate {
  /**
   * MaterialDocumentID（标识要更新的实体）
   */
  materialDocumentId: string;

}


/**
 * MaterialDocument 状态更新 DTO
 * 对应前端 MaterialDocumentStatus
 * @description 对应后端 TaktMaterialDocumentStatusDto
 */
export interface MaterialDocumentStatus {
  /**
   * MaterialDocumentID
   */
  materialDocumentId: string;

  /**
   * 物料凭证状态（0=草稿，1=已过账，2=已作废）
   */
  materialDocumentStatus: number;

}


/**
 * MaterialDocument 导入模板行 DTO
 * 对应前端 MaterialDocumentTemplate
 * @description 对应后端 TaktMaterialDocumentTemplateDto
 */
export interface MaterialDocumentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
   */
  materialCode?: string;

  /**
   * 物料凭证号（租户+公司+工厂内唯一）
   */
  materialDocumentCode?: string;

  /**
   * 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  postedBy?: string;

  /**
   * 物料凭证状态（0=草稿，1=已过账，2=已作废）
   */
  materialDocumentStatus?: number;

  /**
   * 物料凭证行项目列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialDocumentItemCreate[];

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
 * MaterialDocument 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialDocumentImport
 * @description 对应后端 TaktMaterialDocumentImportDto
 */
export interface MaterialDocumentImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
   */
  materialCode?: string;

  /**
   * 物料凭证号（租户+公司+工厂内唯一）
   */
  materialDocumentCode?: string;

  /**
   * 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  postedBy?: string;

  /**
   * 物料凭证状态（0=草稿，1=已过账，2=已作废）
   */
  materialDocumentStatus?: number;

  /**
   * 物料凭证行项目列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialDocumentItemCreate[];

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
 * MaterialDocument 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialDocumentExport
 * @description 对应后端 TaktMaterialDocumentExportDto
 */
export interface MaterialDocumentExport {
  /**
   * MaterialDocumentID
   */
  materialDocumentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
   */
  materialCode: string;

  /**
   * 物料凭证号（租户+公司+工厂内唯一）
   */
  materialDocumentCode: string;

  /**
   * 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  postedBy?: string;

  /**
   * 物料凭证状态（0=草稿，1=已过账，2=已作废）
   */
  materialDocumentStatus: number;

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

