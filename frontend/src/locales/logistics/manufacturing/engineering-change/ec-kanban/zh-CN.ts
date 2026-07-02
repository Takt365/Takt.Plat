// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：zh-CN.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板静态文案；引用键 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '设变实施跟踪',
    filter: {
      currentDept: '当前卡点部门',
      implementationStatus: '实施状态',
      onlyNotOfficiallyCompleted: '仅未正式完成',
    },
    column: {
      detailCount: '明细数',
      currentDept: '当前卡点',
      pendingCount: '待实施数',
      implementationStatus: '实施状态',
      path: '实施路径',
    },
    implementationStatus: {
      notStarted: '未开始',
      inProgress: '实施中',
      officiallyCompleted: '正式完成',
      fullyCompleted: '全部完成',
    },
    dept: {
      eng: '技术',
      pmc: '生管',
      mp: '采购',
      iqc: '受检',
      mc: '部管',
      pcba: '制二',
      assy: '制一',
      qa: '品管',
      te: '制技',
    },
    hint: {
      officialCompletion: '品管课全部明细已实施后，设变视为正式完成',
    },
  },
};
