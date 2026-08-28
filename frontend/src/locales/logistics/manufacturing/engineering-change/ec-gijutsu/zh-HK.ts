// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-gijutsu
// 文件名称：zh-HK.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設變主頁面靜態文案；引用鍵 logistics.manufacturing.engineering.change.ec.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    sourceEcInput: {
      title: '來源設變錄入',
      openButton: '來源錄入',
      searchPlaceholder: '設變號碼 / 標題 / 機種',
      detailCount: '明細行數',
      selectRequired: '請選擇一條來源設變',
      loadToForm: '載入至表單',
      plantFromCompany: '公司 {company} → 工廠 {plant}',
      companyRequired: '請先選擇公司後再錄入來源設變',
      formTitle: '來源設變導入（請填寫負責人、管理區分並上傳附件）',
      attachmentRequired: '請至少添加一條附件並上傳文件',
      attachmentUploadRequired: '第 {row} 行附件尚未上傳文件',
      importSelected: '導入選中（{count}）',
      importSuccess: '已成功導入 {count} 條設變',
      importPartial: '導入完成：成功 {success} 條，失敗 {fail} 條',
    },
    attachment: {
      docCode: {
        formatInvalid: '文件編碼格式不正確（{hint}）',
        duplicate: '文件編碼「{code}」已存在，不可重複',
        hint: {
          empty: '請輸入文件編碼',
          ec: '與設變單號一致',
          eppFpp: 'P-xxxx（P- + 4 位數字，如 P-0001）',
          tl: 'DTS-xxxx（DTS- + 4 位數字，如 DTS-0001）',
          quadDash: 'xxxx-xxxx（各 4 位數字，如 1234-5678）',
        },
      },
      fileName: {
        duplicate: '文件名稱「{name}」已存在，不可重複',
      },
    },
    tabs: {
      oldNewMaterial: '舊新物料',
    },
  },
};
