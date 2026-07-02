// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：zh-HK.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設變看板靜態文案；引用鍵 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '設變實施跟蹤',
    filter: {
      currentDept: '當前卡點部門',
      implementationStatus: '實施狀態',
      onlyNotOfficiallyCompleted: '僅未正式完成',
    },
    column: {
      detailCount: '明細數',
      currentDept: '當前卡點',
      pendingCount: '待實施數',
      implementationStatus: '實施狀態',
      path: '實施路徑',
    },
    implementationStatus: {
      notStarted: '未開始',
      inProgress: '實施中',
      officiallyCompleted: '正式完成',
      fullyCompleted: '全部完成',
    },
    dept: {
      eng: '技術',
      pmc: '生管',
      mp: '採購',
      iqc: '受檢',
      mc: '部管',
      pcba: '制二',
      assy: '制一',
      qa: '品管',
      te: '制技',
    },
    hint: {
      officialCompletion: '品管課全部明細已實施後，設變視為正式完成',
    },
  },
};
