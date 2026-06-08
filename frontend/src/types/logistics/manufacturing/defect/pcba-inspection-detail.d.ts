// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：pcba-inspection-detail.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/defect 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * PCBA检查明细实体
 * 对应前端 TaktPcbaInspectionDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaInspectionDetail
 * @description 对应后端 TaktPcbaInspectionDetailDto
 */
export interface PcbaInspectionDetail extends CompanyDtoBase {
  /**
   * PcbaInspectionDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  pcbaInspectionDetailId: string;

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId: string;

  /**
   * PCBA检查日报名称（填充字段）
   */
  pcbaInspectionName?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * PCBA板别
   */
  pcbaBoardType?: string;

  /**
   * 目视线别
   */
  visualInspectionLine?: string;

  /**
   * AOI线别
   */
  aoiLine?: string;

  /**
   * B面实装日期
   */
  bSideAssemblyDate?: string;

  /**
   * T面实装日期
   */
  tSideAssemblyDate?: string;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 检查员
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty: number;

  /**
   * 检查数量
   */
  inspectionQty: number;

  /**
   * 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
   */
  inspectionStatus: number;

  /**
   * 生产线
   */
  prodLine?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours: number;

  /**
   * AOI工数
   */
  aoiWorkHours: number;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

  /**
   * PCBA检查日报（主表） （主表：TaktPcbaInspection）
   */
  pcbaInspection?: PcbaInspection;

}


/**
 * PcbaInspectionDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PcbaInspectionDetailQuery
 * @description 对应后端 TaktPcbaInspectionDetailQueryDto
 */
export interface PcbaInspectionDetailQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * PCBA板别
   */
  pcbaBoardType?: string;

  /**
   * 目视线别
   */
  visualInspectionLine?: string;

  /**
   * AOI线别
   */
  aoiLine?: string;

  /**
   * B面实装日期（范围查询-开始）
   */
  bSideAssemblyDateStart?: string;

  /**
   * B面实装日期（范围查询-结束）
   */
  bSideAssemblyDateEnd?: string;

  /**
   * T面实装日期（范围查询-开始）
   */
  tSideAssemblyDateStart?: string;

  /**
   * T面实装日期（范围查询-结束）
   */
  tSideAssemblyDateEnd?: string;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 检查员
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty?: number;

  /**
   * 检查数量
   */
  inspectionQty?: number;

  /**
   * 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
   */
  inspectionStatus?: number;

  /**
   * 生产线
   */
  prodLine?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours?: number;

  /**
   * AOI工数
   */
  aoiWorkHours?: number;

  /**
   * 不良数量
   */
  defectQty?: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

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
 * 创建PcbaInspectionDetail DTO
 * 对应前端 PcbaInspectionDetailCreate
 * @description 对应后端 TaktPcbaInspectionDetailCreateDto
 */
export interface PcbaInspectionDetailCreate {
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
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * PCBA板别
   */
  pcbaBoardType?: string;

  /**
   * 目视线别
   */
  visualInspectionLine?: string;

  /**
   * AOI线别
   */
  aoiLine?: string;

  /**
   * B面实装日期
   */
  bSideAssemblyDate?: string;

  /**
   * T面实装日期
   */
  tSideAssemblyDate?: string;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 检查员
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty: number;

  /**
   * 检查数量
   */
  inspectionQty: number;

  /**
   * 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
   */
  inspectionStatus: number;

  /**
   * 生产线
   */
  prodLine?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours: number;

  /**
   * AOI工数
   */
  aoiWorkHours: number;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

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
 * 更新PcbaInspectionDetail DTO
 * 继承 TaktPcbaInspectionDetailCreateDto，添加 PcbaInspectionDetailId 字段
 * 对应前端 PcbaInspectionDetailUpdate
 * @description 对应后端 TaktPcbaInspectionDetailUpdateDto
 */
export interface PcbaInspectionDetailUpdate extends PcbaInspectionDetailCreate {
  /**
   * PcbaInspectionDetailID（标识要更新的实体）
   */
  pcbaInspectionDetailId: string;

}


/**
 * PcbaInspectionDetail 状态更新 DTO
 * 对应前端 PcbaInspectionDetailStatus
 * @description 对应后端 TaktPcbaInspectionDetailStatusDto
 */
export interface PcbaInspectionDetailStatus {
  /**
   * PcbaInspectionDetailID
   */
  pcbaInspectionDetailId: string;

  /**
   * 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
   */
  inspectionStatus: number;

}


/**
 * PcbaInspectionDetail 导入模板行 DTO
 * 对应前端 PcbaInspectionDetailTemplate
 * @description 对应后端 TaktPcbaInspectionDetailTemplateDto
 */
export interface PcbaInspectionDetailTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * PCBA板别
   */
  pcbaBoardType?: string;

  /**
   * 目视线别
   */
  visualInspectionLine?: string;

  /**
   * AOI线别
   */
  aoiLine?: string;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 检查员
   */
  inspectorName?: string;

  /**
   * 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
   */
  inspectionStatus?: number;

  /**
   * 生产线
   */
  prodLine?: string;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

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
 * PcbaInspectionDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PcbaInspectionDetailImport
 * @description 对应后端 TaktPcbaInspectionDetailImportDto
 */
export interface PcbaInspectionDetailImport {
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
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * PCBA板别
   */
  pcbaBoardType?: string;

  /**
   * 目视线别
   */
  visualInspectionLine?: string;

  /**
   * AOI线别
   */
  aoiLine?: string;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 检查员
   */
  inspectorName?: string;

  /**
   * 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
   */
  inspectionStatus?: number;

  /**
   * 生产线
   */
  prodLine?: string;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

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
 * PcbaInspectionDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaInspectionDetailExport
 * @description 对应后端 TaktPcbaInspectionDetailExportDto
 */
export interface PcbaInspectionDetailExport {
  /**
   * PcbaInspectionDetailID
   */
  pcbaInspectionDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * PCBA板别
   */
  pcbaBoardType?: string;

  /**
   * 目视线别
   */
  visualInspectionLine?: string;

  /**
   * AOI线别
   */
  aoiLine?: string;

  /**
   * B面实装日期
   */
  bSideAssemblyDate?: string;

  /**
   * T面实装日期
   */
  tSideAssemblyDate?: string;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 检查员
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty: number;

  /**
   * 检查数量
   */
  inspectionQty: number;

  /**
   * 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
   */
  inspectionStatus: number;

  /**
   * 生产线
   */
  prodLine?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours: number;

  /**
   * AOI工数
   */
  aoiWorkHours: number;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

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

