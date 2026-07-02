// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：ja-JP.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設変看板静的文案；参照键 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '設変実施トラッキング',
    filter: {
      currentDept: '現在の停滞部門',
      implementationStatus: '実施状態',
      onlyNotOfficiallyCompleted: '正式未完了のみ',
    },
    column: {
      detailCount: '明細数',
      currentDept: '停滞部門',
      pendingCount: '未実施数',
      implementationStatus: '実施状態',
      path: '実施パス',
    },
    implementationStatus: {
      notStarted: '未着手',
      inProgress: '実施中',
      officiallyCompleted: '正式完了',
      fullyCompleted: '全完了',
    },
    dept: {
      eng: '技術',
      pmc: '生管',
      mp: '購買',
      iqc: '受入検査',
      mc: '部品管理',
      pcba: 'PCBA',
      assy: '組立',
      qa: '品質管理',
      te: '製技',
    },
    hint: {
      officialCompletion: '品質管理課が全明細を実施完了した時点で設変は正式完了',
    },
  },
};
