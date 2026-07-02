// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/dashboard/data-board
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：dashboard/data-board page static copy; keys dashboard.data-board.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    addmodule: "Add Module",
    removemodule: "Remove Module",
    addsuccess: "Module added",
    removesuccess: "Module removed",
    selectmoduletype: "Select statistics module type",
    dragtoreorder: "Drag {title} to reorder",
    layoutlabel: "Layout",
    layoutfullrow: "One row one column",
    layouthalfrow: "One row two columns",
    overviewplaceholder: "Statistics overview. Connect metric cards or summary data here.",
    periodmonth: "Period: this month",
    customplaceholder: "Custom statistics module. Configurable in future.",
    modules: {
      overview: "Overview",
      change: "Change Stats",
      online: "Online Stats",
      sales: "Sales Stats",
      production: "Production Stats",
      custom: "Custom",
    },
    change: {
      summaryEcCount: "This month EC {ecCount} ({detailCount} details)",
      total: "Dept Rows",
      notimplemented: "Not Implemented",
      implemented: "Implemented",
      inprogressec: "EC In Progress",
      notofficiallycompleted: "Not Officially Completed",
    },
    online: {
      users: "Online Users",
      todayvisits: "Today Visits",
      sessions: "Sessions",
    },
    sales: {
      orders: "Invoices",
      amount: "Sales Amount",
      yoy: "YoY",
    },
    production: {
      output: "Output",
      yieldrate: "Yield Rate",
      wip: "WIP",
      monthstdcapacity: "Monthly Std Capacity",
      monthprodactualqty: "Monthly Actual Qty",
      monthachievementrate: "Monthly Achievement",
      monthdowntime: "Downtime",
      monthinputminutes: "Input Minutes",
      monthprodminutes: "Production Minutes",
      monthactualminutes: "Actual Minutes",
      currentMonth: "This month",
      monthLabel: "Month {month}",
      summaryProduction: "{month}: actual {actual} (plan {plan}) / {rate}% achievement",
      summaryInput: "Input: {input} min (loss: {loss} min)",
    },
    overview: {
      todo: "Todo",
      unread: "Unread",
      online: "Online",
      monthorders: "Orders (month)",
      ectotal: "EC dept rows",
      wip: "WIP orders",
    },
  },
};
