// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：fqc-defect-handling.d.ts
// 创建时间：2026-07-23
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
 * FQC出货检验不良处理记录实体
 * 对应前端 TaktFqcDefectHandlingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FqcDefectHandling
 * @description 对应后端 TaktFqcDefectHandlingDto
 */
export interface FqcDefectHandling extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * FQC不良处理编码
   */
  fqcDefectHandlingCode?: string;

  /**
   * FQC检验单明细 ID（选项 TaktFqcOrderItems/options；DictValue=Id）
   */
  fqcOrderItemId?: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良类型（字典 logistics_quality_defect_type）
   */
  defectType?: number;

  /**
   * 不良现象编码
   */
  defectCode?: string;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 处理方式（字典 logistics_quality_defect_handling_method）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
   */
  responsibleDept?: string;

  /**
   * 责任人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  responsibleBy?: string;

  /**
   * 处理人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  handlerBy?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 处理状态（字典 logistics_quality_defect_handling_status）
   */
  handlingStatus?: number;

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
 * FqcDefectHandling 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FqcDefectHandlingExport
 * @description 对应后端 TaktFqcDefectHandlingExportDto
 */
export interface FqcDefectHandlingExport {
  /**
   * FqcDefectHandlingID
   */
  fqcDefectHandlingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * FQC不良处理编码
   */
  fqcDefectHandlingCode: string;

  /**
   * FQC检验单明细 ID（选项 TaktFqcOrderItems/options；DictValue=Id）
   */
  fqcOrderItemId: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良类型（字典 logistics_quality_defect_type）
   */
  defectType: number;

  /**
   * 不良现象编码
   */
  defectCode: string;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 处理方式（字典 logistics_quality_defect_handling_method）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
   */
  responsibleDept?: string;

  /**
   * 责任人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  responsibleBy?: string;

  /**
   * 处理人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  handlerBy?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 处理状态（字典 logistics_quality_defect_handling_status）
   */
  handlingStatus: number;

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

