// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/dashboard/data-board
// 文件名称：zh-HK.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：dashboard/data-board 页面静态文案；引用键 dashboard.data-board.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    addmodule: "添加模塊",
    removemodule: "移除模塊",
    addsuccess: "已添加模塊",
    removesuccess: "已移除模塊",
    selectmoduletype: "選擇統計模塊類型",
    dragtoreorder: "拖動{title}排序",
    layoutlabel: "佈局",
    layoutfullrow: "一行一列",
    layouthalfrow: "一行兩列",
    overviewplaceholder: "統計概覽，可在此接入指標卡片或彙總數據。",
    periodmonth: "統計週期：本月",
    customplaceholder: "自定義統計模塊，可在後續擴展中配置。",
    modules: {
      overview: "統計概覽",
      change: "變更統計",
      online: "在線統計",
      sales: "銷售統計",
      production: "生產統計",
      custom: "自定義",
    },
    change: {
      summaryEcCount: "當月設變{ecCount}（{detailCount}）",
      total: "部門行總數",
      notimplemented: "未實施",
      implemented: "已實施",
      inprogressec: "實施中設變",
      notofficiallycompleted: "未正式完成",
    },
    online: {
      users: "在線用戶",
      todayvisits: "今日訪問",
      sessions: "當前會話",
    },
    sales: {
      orders: "發票數",
      amount: "銷售額",
      yoy: "同比",
    },
    production: {
      output: "產量",
      yieldrate: "良率",
      wip: "在製品",
      monthstdcapacity: "月標準產能",
      monthprodactualqty: "月實際產量",
      monthachievementrate: "月達成率",
      monthdowntime: "停線時間",
      monthinputminutes: "投入工時",
      monthprodminutes: "生產工時",
      monthactualminutes: "實際工時",
      currentMonth: "本月",
      monthLabel: "{month}月",
      summaryProduction: "{month}生產：實績{actual}（計劃{plan}）/達成率{rate}%",
      summaryInput: "投入：{input}分鐘（損失：{loss}分鐘）",
    },
    overview: {
      todo: "待辦",
      unread: "未讀消息",
      online: "在線用戶",
      monthorders: "本月訂單",
      ectotal: "設變部門行",
      wip: "在制工單",
    },
  },
};
