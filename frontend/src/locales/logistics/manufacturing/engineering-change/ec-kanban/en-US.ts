// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：en-US.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：EC kanban static copy; keys logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'EC Implementation Tracking',
    filter: {
      currentDept: 'Current blocking dept',
      implementationStatus: 'Implementation status',
      onlyNotOfficiallyCompleted: 'Not officially completed only',
    },
    column: {
      detailCount: 'Detail lines',
      currentDept: 'Blocking dept',
      pendingCount: 'Pending',
      implementationStatus: 'Status',
      path: 'Implementation path',
    },
    implementationStatus: {
      notStarted: 'Not started',
      inProgress: 'In progress',
      officiallyCompleted: 'Officially completed',
      fullyCompleted: 'Fully completed',
    },
    dept: {
      eng: 'Engineering',
      pmc: 'PMC',
      mp: 'Purchasing',
      iqc: 'IQC',
      mc: 'Material control',
      pcba: 'PCBA',
      assy: 'Assembly',
      qa: 'QA',
      te: 'Process tech',
    },
    hint: {
      officialCompletion: 'EC is officially complete when QA has implemented all detail lines',
    },
  },
};
