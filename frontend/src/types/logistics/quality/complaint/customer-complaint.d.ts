// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customer-complaint.d.ts
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
 * 客诉主表实体
 * 对应前端 TaktCustomerComplaintDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerComplaint
 * @description 对应后端 TaktCustomerComplaintDto
 */
export interface CustomerComplaint extends CompanyDtoBase {

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus?: number;

  /**
   * 客诉明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerComplaintItemCreate[];

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
 * CustomerComplaint 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerComplaintExport
 * @description 对应后端 TaktCustomerComplaintExportDto
 */
export interface CustomerComplaintExport {
  /**
   * CustomerComplaintID
   */
  customerComplaintId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate: string;

  /**
   * 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod: number;

  /**
   * 投诉类型（字典 logistics_quality_complaint_type）
   */
  complaintType: number;

  /**
   * 投诉等级（字典 logistics_quality_complaint_level）
   */
  complaintLevel: number;

  /**
   * 责任部门 ID（选项 TaktDepts/options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称
   */
  responsibleDeptName?: string;

  /**
   * 责任人 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  responsiblePersonId?: string;

  /**
   * 责任人姓名
   */
  responsiblePersonName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉描述
   */
  complaintDescription: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus: number;

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

