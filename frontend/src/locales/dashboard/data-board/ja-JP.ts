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
      total: "変更総数",
      inprogress: "進行中",
      completed: "完了",
    },
    online: {
      users: "オンラインユーザー",
      todayvisits: "今日のアクセス",
      sessions: "現在のセッション",
    },
    sales: {
      orders: "注文数",
      amount: "売上高",
      yoy: "前年比",
    },
    production: {
      output: "生産量",
      yieldrate: "良品率",
      wip: "仕掛品",
    },
  },
};
