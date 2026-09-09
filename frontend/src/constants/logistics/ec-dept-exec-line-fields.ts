// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/constants/logistics
// 文件名称：ec-dept-exec-line-fields.ts
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：各部门执行主表列顺序（对齐 Domain：行号→单号→停产→机种/完成品→业务上下文→实施→本课字段→完成品描述→部门）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 前列：行号、设变单号（对齐实体 LineNumber → EcCode）
 */
const EC_DEPT_EXEC_LINE_IDENTITY = ['lineNumber', 'ecCode'] as const

/**
 * 含机种/完成品课别的头段：停产状态 → 机种 → 完成品（描述在业务字段之后）
 */
const EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL = [
  'discontinuedStatus',
  'ecModelCode',
  'ecFinishedGoods',
] as const

/**
 * 实施区：是否实施、执行内容
 */
const EC_DEPT_EXEC_LINE_EXEC = ['isImplemented', 'execContent'] as const

/**
 * 完成品描述（实体中位于本课业务字段之后、部门之前）
 */
const EC_DEPT_EXEC_LINE_FINISHED_DESC = ['ecFinishedGoodsDescription'] as const

/**
 * 尾段：部门编码
 */
const EC_DEPT_EXEC_LINE_TAIL_DEPT = ['deptCode'] as const

/**
 * 各课实施/业务字段（实体 ExecContent 之后、完成品描述之前）
 */
const EC_DEPT_EXEC_LINE_IMPL = {
  /** TaktEcKoubai：采购订单发行日期、供应商、采购订单号码、旧品处理 */
  eckoubai: ['purchaseOrderIssueDate', 'supplier', 'purchaseOrderCode', 'ecOldPartDisposition'],
  /** TaktEcSeikan：预计生产日期、预定批次、Po残、结余、旧品处理 */
  ecseikan: [
    'scheduledProductionDate',
    'scheduledBatch',
    'poRemainder',
    'balance',
    'ecOldPartDisposition',
  ],
  /** TaktEcUkeken：受检单号、检验日期 */
  ecukeken: ['iqcOrderCode', 'inspectionDate'],
  /** TaktEcBukan：出库批次、出库日期 */
  ecbukan: ['outboundBatch', 'outboundDate'],
  /** TaktEcSmt：出库批次、出库日期 */
  ecsmt: ['outboundBatch', 'outboundDate'],
  /** TaktEcSeizounika：生产班组、生产日期、实施批次 */
  ecseizounika: ['productionTeam', 'productionDate', 'implementationBatch'],
  /** TaktEcSeizouikka：生产班组、生产日期、实施批次 */
  ecseizouikka: ['productionTeam', 'productionDate', 'implementationBatch'],
  /** TaktEcHinkan：生产班组、检验日期、检验批次、抽样号码 */
  echinkan: ['productionTeam', 'inspectionDate', 'inspectionBatch', 'samplingCode'],
  /** TaktEcSeizougijutsu：确认日期、是否更新SOP */
  ecseizougijutsu: ['confirmationDate', 'isSopUpdated'],
} as const

/**
 * 采购：新物料、新品仓库、新采购类型（无停产/机种/完成品）
 */
const EC_DEPT_EXEC_LINE_MID_KOUBAI = [
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewWarehouse',
  'ecNewPurchaseType',
] as const

/**
 * 受检：新物料、新品仓库、新品是否需检验
 */
const EC_DEPT_EXEC_LINE_MID_UKEKEN = [
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewWarehouse',
  'ecNewRequiresInspection',
] as const

/**
 * 部管：新物料 / 新采购类型 / 新品仓库（停产与机种/完成品已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_BUKAN = [
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewPurchaseType',
  'ecNewWarehouse',
] as const

/**
 * SMT：上阶 + 新物料（停产与机种/完成品已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_SMT = [
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewPurchaseType',
  'ecNewWarehouse',
] as const

/**
 * 制二：仅上阶（停产与机种/完成品已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_SEIZOUNIKA = [
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
] as const

/**
 * 生管/制一/品管/制技：头段之后无额外物料上下文
 */
const EC_DEPT_EXEC_LINE_MID_EMPTY = [] as const

const EC_DEPT_EXEC_LINE_MID_BY_SLUG: Record<string, readonly string[]> = {
  eckoubai: EC_DEPT_EXEC_LINE_MID_KOUBAI,
  ecukeken: EC_DEPT_EXEC_LINE_MID_UKEKEN,
  ecbukan: EC_DEPT_EXEC_LINE_MID_BUKAN,
  ecsmt: EC_DEPT_EXEC_LINE_MID_SMT,
  ecseizounika: EC_DEPT_EXEC_LINE_MID_SEIZOUNIKA,
  ecseizouikka: EC_DEPT_EXEC_LINE_MID_EMPTY,
  echinkan: EC_DEPT_EXEC_LINE_MID_EMPTY,
  ecseizougijutsu: EC_DEPT_EXEC_LINE_MID_EMPTY,
  ecseikan: EC_DEPT_EXEC_LINE_MID_EMPTY,
}

/** 含机种/完成品/停产头段的部门（采购/受检除外） */
const EC_DEPT_EXEC_HAS_DISC_MODEL = new Set([
  'ecbukan',
  'ecsmt',
  'ecseikan',
  'ecseizouikka',
  'echinkan',
  'ecseizougijutsu',
  'ecseizounika',
])

/** 执行部门实体 slug */
export type EcDeptExecSlug = keyof typeof EC_DEPT_EXEC_LINE_IMPL

/**
 * 取指定部门执行主表字段
 * 顺序：行号/单号 → [停产/机种/完成品] → 物料上下文 → 实施/执行内容 → 本课业务 → [完成品描述] → 部门
 * @param slug 实体 slug（eckoubai 对应 TaktEcKoubai）
 * @returns {readonly string[]} 主表列字段
 */
export function getEcDeptExecLineFields(slug: string): readonly string[] {
  const impl =
    slug in EC_DEPT_EXEC_LINE_IMPL
      ? EC_DEPT_EXEC_LINE_IMPL[slug as EcDeptExecSlug]
      : []
  const mid = EC_DEPT_EXEC_LINE_MID_BY_SLUG[slug] ?? EC_DEPT_EXEC_LINE_MID_EMPTY
  const hasDiscModel = EC_DEPT_EXEC_HAS_DISC_MODEL.has(slug)
  return [
    ...EC_DEPT_EXEC_LINE_IDENTITY,
    ...(hasDiscModel ? EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL : []),
    ...mid,
    ...EC_DEPT_EXEC_LINE_EXEC,
    ...impl,
    ...(hasDiscModel ? EC_DEPT_EXEC_LINE_FINISHED_DESC : []),
    ...EC_DEPT_EXEC_LINE_TAIL_DEPT,
  ]
}
