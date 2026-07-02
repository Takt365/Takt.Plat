// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/dashboard/data-board
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：dashboard/data-board 页面静态文案；引用键 dashboard.data-board.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    addmodule: "添加模块",
    removemodule: "移除模块",
    addsuccess: "已添加模块",
    removesuccess: "已移除模块",
    selectmoduletype: "选择统计模块类型",
    dragtoreorder: "拖动{title}排序",
    layoutlabel: "布局",
    layoutfullrow: "一行一列",
    layouthalfrow: "一行两列",
    overviewplaceholder: "统计概览，可在此接入指标卡片或汇总数据。",
    periodmonth: "统计周期：本月",
    customplaceholder: "自定义统计模块，可在后续扩展中配置。",
    modules: {
      overview: "统计概览",
      change: "变更统计",
      online: "在线统计",
      sales: "销售统计",
      production: "生产统计",
      custom: "自定义",
    },
    change: {
      summaryEcCount: "当月设变{ecCount}（{detailCount}）",
      total: "部门行总数",
      notimplemented: "未实施",
      implemented: "已实施",
      inprogressec: "实施中设变",
      notofficiallycompleted: "未正式完成",
    },
    online: {
      users: "在线用户",
      todayvisits: "今日访问",
      sessions: "当前会话",
    },
    sales: {
      orders: "发票数",
      amount: "销售额",
      yoy: "同比",
    },
    production: {
      output: "产量",
      yieldrate: "良率",
      wip: "在制品",
      monthstdcapacity: "月标准产能",
      monthprodactualqty: "月实际产量",
      monthachievementrate: "月达成率",
      monthdowntime: "停线时间",
      monthinputminutes: "投入工时",
      monthprodminutes: "生产工时",
      monthactualminutes: "实际工时",
      currentMonth: "本月",
      monthLabel: "{month}月",
      summaryProduction: "{month}生产：实绩{actual}（计划{plan}）/达成率{rate}%",
      summaryInput: "投入：{input}分钟（损失：{loss}分钟）",
    },
    overview: {
      todo: "待办",
      unread: "未读消息",
      online: "在线用户",
      monthorders: "本月订单",
      ectotal: "设变部门行",
      wip: "在制工单",
    },
  },
};
