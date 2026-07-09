// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：fqc-order-item.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * FQC出货检验单明细实体
 * 对应前端 TaktFqcOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FqcOrderItem
 * @description 对应后端 TaktFqcOrderItemDto
 */
export interface FqcOrderItem extends CompanyDtoBase {
  /**
   * FqcOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  fqcOrderItemId: string;

  /**
   * FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
   */
  fqcOrderId: string;

  /**
   * FQC检验单 名称（填充字段）
   */
  fqcOrderName?: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 入库数量
   */
  warehouseQuantity: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（字典 logistics_quality_inspection_method）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * FQC检验单（主表） （主表：TaktFqcOrder）
   */
  order?: FqcOrder;

  /**
   * 不良处理记录列表（主子表关系） （子表：TaktFqcDefectHandling）
   */
  defectHandlings?: FqcDefectHandling[];

}


/**
 * FqcOrderItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FqcOrderItemQuery
 * @description 对应后端 TaktFqcOrderItemQueryDto
 */
export interface FqcOrderItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
   */
  fqcOrderId?: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 入库数量
   */
  warehouseQuantity?: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode?: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（字典 logistics_quality_inspection_method）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity?: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
   */
  inspectorBy?: string;

  /**
   * 检验日期（范围查询-开始）
   */
  inspectionDateStart?: string;

  /**
   * 检验日期（范围查询-结束）
   */
  inspectionDateEnd?: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建FqcOrderItem DTO
 * 对应前端 FqcOrderItemCreate
 * @description 对应后端 TaktFqcOrderItemCreateDto
 */
export interface FqcOrderItemCreate {
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
   * FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
   */
  fqcOrderId: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 入库数量
   */
  warehouseQuantity: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（字典 logistics_quality_inspection_method）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: FqcDefectHandlingCreate[];

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
 * 更新FqcOrderItem DTO
 * 继承 TaktFqcOrderItemCreateDto，添加 FqcOrderItemId 字段
 * 对应前端 FqcOrderItemUpdate
 * @description 对应后端 TaktFqcOrderItemUpdateDto
 */
export interface FqcOrderItemUpdate extends FqcOrderItemCreate {
  /**
   * FqcOrderItemID（标识要更新的实体）
   */
  fqcOrderItemId: string;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: any;

}


/**
 * FqcOrderItem 状态更新 DTO
 * 对应前端 FqcOrderItemStatus
 * @description 对应后端 TaktFqcOrderItemStatusDto
 */
export interface FqcOrderItemStatus {
  /**
   * FqcOrderItemID
   */
  fqcOrderItemId: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus: number;

}


/**
 * FqcOrderItem 作废/撤销作废 DTO
 * 对应前端 FqcOrderItemObsolete
 * @description 对应后端 TaktFqcOrderItemObsoleteDto
 */
export interface FqcOrderItemObsolete {
  /**
   * FqcOrderItemID
   */
  fqcOrderItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * FqcOrderItem 导入模板行 DTO
 * 对应前端 FqcOrderItemTemplate
 * @description 对应后端 TaktFqcOrderItemTemplateDto
 */
export interface FqcOrderItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
   */
  fqcOrderId?: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 入库数量
   */
  warehouseQuantity?: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode?: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（字典 logistics_quality_inspection_method）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity?: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
   */
  inspectorBy?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: FqcDefectHandlingCreate[];

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
 * FqcOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FqcOrderItemImport
 * @description 对应后端 TaktFqcOrderItemImportDto
 */
export interface FqcOrderItemImport {
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
   * FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
   */
  fqcOrderId?: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 入库数量
   */
  warehouseQuantity?: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode?: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（字典 logistics_quality_inspection_method）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity?: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
   */
  inspectorBy?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: FqcDefectHandlingCreate[];

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
 * FqcOrderItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FqcOrderItemExport
 * @description 对应后端 TaktFqcOrderItemExportDto
 */
export interface FqcOrderItemExport {
  /**
   * FqcOrderItemID
   */
  fqcOrderItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
   */
  fqcOrderId: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 入库数量
   */
  warehouseQuantity: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（字典 logistics_quality_inspection_method）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

