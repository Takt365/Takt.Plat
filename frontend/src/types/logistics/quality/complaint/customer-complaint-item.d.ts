// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customer-complaint-item.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 客诉明细实体
 * 对应前端 TaktCustomerComplaintItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerComplaintItem
 * @description 对应后端 TaktCustomerComplaintItemDto
 */
export interface CustomerComplaintItem extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  customerComplaintCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 不良项目类型（字典 logistics_quality_complaint_item_type）
   */
  itemType?: number;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
   */
  defectLevel?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善责任人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  improvementResponsible?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachmentPaths?: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * CustomerComplaintItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerComplaintItemExport
 * @description 对应后端 TaktCustomerComplaintItemExportDto
 */
export interface CustomerComplaintItemExport {
  /**
   * CustomerComplaintItemID
   */
  customerComplaintItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  customerComplaintCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 不良项目类型（字典 logistics_quality_complaint_item_type）
   */
  itemType: number;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
   */
  defectLevel: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善责任人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  improvementResponsible?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachmentPaths?: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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

