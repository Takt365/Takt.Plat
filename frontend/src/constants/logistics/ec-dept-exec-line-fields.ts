// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/constants/logistics
// 文件名称：ec-dept-exec-line-fields.ts
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：各部门执行子表列：实施相关在前，机种/完成品等上下文在后
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 统一前列：设变单号、行号、实施、执行内容
 */
const EC_DEPT_EXEC_LINE_LEAD = [
  'ecCode',
  'lineNumber',
  'isImplemented',
  'execContent',
] as const

/**
 * 各课实施字段（实体 ExecContent 之后、IsObsolete 之前的本课列）
 */
const EC_DEPT_EXEC_LINE_IMPL = {
  /** TaktEcKoubai：采购订单发行日期、供应商、采购订单号码 */
  eckoubai: ['purchaseOrderIssueDate', 'supplier', 'purchaseOrderCode'],
  /** TaktEcSeikan：预计生产日期、预定批次、Po残、结余、旧品处理 */
  ecseikan: [
    'scheduledProductionDate',
    'scheduledBatch',
    'poRemainder',
    'balance',
    'oldProductHandling',
  ],
  /** TaktEcUkeken：受检单号、检验日期 */
  ecukeken: ['iqcOrderCode', 'inspectionDate'],
  /** TaktEcBukan：出库批次、出库日期 */
  ecbukan: ['outboundBatch', 'outboundDate'],
  /** TaktEcSeizounika：生产日期、生产批次、生产班组、出库单号 */
  ecseizounika: ['productionDate', 'productionBatch', 'productionTeam', 'outboundOrderCode'],
  /** TaktEcSeizouikka：生产班组、生产日期、实施批次 */
  ecseizouikka: ['productionTeam', 'productionDate', 'implementationBatch'],
  /** TaktEcHinkan：生产班组、检验日期、检验批次、抽样号码 */
  echinkan: ['productionTeam', 'inspectionDate', 'inspectionBatch', 'samplingCode'],
  /** TaktEcSeizougijutsu：确认日期、是否更新SOP */
  ecseizougijutsu: ['confirmationDate', 'isSopUpdated'],
} as const

/**
 * 统一后列：机种、完成品、上阶物料、完成品物料状态、部门编码
 */
const EC_DEPT_EXEC_LINE_CONTEXT = [
  'ecModelCode',
  'ecFinishedGoods',
  'ecFinishedGoodsDescription',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'discontinuedStatus',
  'deptCode',
] as const

/** 执行部门实体 slug */
export type EcDeptExecSlug = keyof typeof EC_DEPT_EXEC_LINE_IMPL

/**
 * 取指定部门执行子表字段
 * 顺序：设变单号、行号、实施、执行内容、本课实施字段、机种/完成品等上下文
 * @param slug 实体 slug（eckoubai 对应 TaktEcKoubai）
 * @returns {readonly string[]} 子表列字段
 */
export function getEcDeptExecLineFields(slug: string): readonly string[] {
  const impl =
    slug in EC_DEPT_EXEC_LINE_IMPL
      ? EC_DEPT_EXEC_LINE_IMPL[slug as EcDeptExecSlug]
      : []
  return [...EC_DEPT_EXEC_LINE_LEAD, ...impl, ...EC_DEPT_EXEC_LINE_CONTEXT]
}
