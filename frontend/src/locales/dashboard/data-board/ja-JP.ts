// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/dashboard/data-board
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：dashboard/data-board 页面静态文案；引用键 dashboard.data-board.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    addmodule: "モジュールを追加",
    removemodule: "モジュールを削除",
    addsuccess: "モジュールを追加しました",
    removesuccess: "モジュールを削除しました",
    selectmoduletype: "統計モジュールの種類を選択",
    dragtoreorder: "ドラッグで{title}を並べ替え",
    layoutlabel: "レイアウト",
    layoutfullrow: "1行1列",
    layouthalfrow: "1行2列",
    overviewplaceholder: "統計概要。指標カードや集計データをここに接続できます。",
    periodmonth: "集計期間：今月",
    customplaceholder: "カスタム統計モジュール。今後の拡張で設定可能です。",
    modules: {
      overview: "統計概要",
      change: "変更統計",
      online: "オンライン統計",
      sales: "販売統計",
      production: "生産統計",
      custom: "カスタム",
    },
    change: {
      summaryEcCount: "当月設変{ecCount}（{detailCount}）",
      total: "部門行合計",
      notimplemented: "未実施",
      implemented: "実施済",
      inprogressec: "実施中設変",
      notofficiallycompleted: "正式未完了",
    },
    online: {
      users: "オンラインユーザー",
      todayvisits: "今日のアクセス",
      sessions: "現在のセッション",
    },
    sales: {
      orders: "請求書数",
      amount: "売上高",
      yoy: "前年比",
    },
    production: {
      output: "生産量",
      yieldrate: "良品率",
      wip: "仕掛品",
      monthstdcapacity: "月標準生産能力",
      monthprodactualqty: "月実績数量",
      monthachievementrate: "月達成率",
      monthdowntime: "停止時間",
      monthinputminutes: "投入工数",
      monthprodminutes: "生産工数",
      monthactualminutes: "実績工数",
      currentMonth: "今月",
      monthLabel: "{month}月",
      summaryProduction: "{month}生産：実績{actual}（計画{plan}）/達成率{rate}%",
      summaryInput: "投入：{input}分（損失：{loss}分）",
    },
    overview: {
      todo: "未処理",
      unread: "未読",
      online: "オンライン",
      monthorders: "今月受注",
      ectotal: "設変部門行",
      wip: "仕掛指図",
    },
  },
};
