// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-invoice-item.d.ts
// 创建时间：2026-08-10
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt采购发票明细实体（公司级；主子表关系见 PurchaseInvoiceId）
 * 对应前端 TaktPurchaseInvoiceItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInvoiceItem
 * @description 对应后端 TaktPurchaseInvoiceItemDto
 */
export interface PurchaseInvoiceItem extends CompanyDtoBase {

  /**
   * 凭证编号（冗余；会计年度见主表 FiscalYear）
   */
  purchaseInvoiceCode?: string;

  /**
   * 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 项目（采购凭证项目）
   */
  purchaseOrderItem?: number;

  /**
   * 科目分配序号
   */
  accountAssignmentSeq?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估范围
   */
  valuationArea?: string;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 税码
   */
  taxCode?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 订单单位
   */
  orderUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 总库存
   */
  valuatedStockQuantity?: number;

  /**
   * 上一过账期间库存
   */
  previousPeriodStock?: number;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 评估类
   */
  valuationClass?: string;

  /**
   * 标识: 更新采购订单历史
   */
  updatePoHistoryFlag?: string;

  /**
   * 后续借/贷
   */
  subsequentDebitCredit?: string;

  /**
   * 价格冻结原因
   */
  blockReasonPrice?: string;

  /**
   * 数量冻结原因
   */
  blockReasonQuantity?: string;

  /**
   * 质量冻结原因
   */
  blockReasonQuality?: string;

  /**
   * 增强冻结原因
   */
  blockReasonEnhanced?: string;

  /**
   * 价值串
   */
  valueString?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 条件类型
   */
  conditionType?: string;

  /**
   * 总价值
   */
  totalValuatedStockValue?: number;

  /**
   * 前期总值
   */
  previousPeriodValue?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 当前期间年
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 库存物料
   */
  stockManagedMaterialCode?: string;

  /**
   * 文本
   */
  itemText?: string;

  /**
   * 来自到达的发票的存货过帐行
   */
  materialDocumentItem?: number;

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
 * PurchaseInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInvoiceItemExport
 * @description 对应后端 TaktPurchaseInvoiceItemExportDto
 */
export interface PurchaseInvoiceItemExport {
  /**
   * PurchaseInvoiceItemID
   */
  purchaseInvoiceItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId: string;

  /**
   * 工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 凭证编号（冗余；会计年度见主表 FiscalYear）
   */
  purchaseInvoiceCode: string;

  /**
   * 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber: number;

  /**
   * 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 项目（采购凭证项目）
   */
  purchaseOrderItem?: number;

  /**
   * 科目分配序号
   */
  accountAssignmentSeq?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估范围
   */
  valuationArea?: string;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 税码
   */
  taxCode?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 订单单位
   */
  orderUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 总库存
   */
  valuatedStockQuantity?: number;

  /**
   * 上一过账期间库存
   */
  previousPeriodStock?: number;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 评估类
   */
  valuationClass?: string;

  /**
   * 标识: 更新采购订单历史
   */
  updatePoHistoryFlag?: string;

  /**
   * 后续借/贷
   */
  subsequentDebitCredit?: string;

  /**
   * 价格冻结原因
   */
  blockReasonPrice?: string;

  /**
   * 数量冻结原因
   */
  blockReasonQuantity?: string;

  /**
   * 质量冻结原因
   */
  blockReasonQuality?: string;

  /**
   * 增强冻结原因
   */
  blockReasonEnhanced?: string;

  /**
   * 价值串
   */
  valueString?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 条件类型
   */
  conditionType?: string;

  /**
   * 总价值
   */
  totalValuatedStockValue?: number;

  /**
   * 前期总值
   */
  previousPeriodValue?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 当前期间年
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 库存物料
   */
  stockManagedMaterialCode?: string;

  /**
   * 文本
   */
  itemText?: string;

  /**
   * 来自到达的发票的存货过帐行
   */
  materialDocumentItem?: number;

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

