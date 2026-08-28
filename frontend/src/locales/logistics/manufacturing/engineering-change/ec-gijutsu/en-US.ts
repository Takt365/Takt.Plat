// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-gijutsu
// 文件名称：en-US.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：EC page static copy; keys logistics.manufacturing.engineering.change.ec.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    sourceEcInput: {
      title: 'Import from Source EC',
      openButton: 'Source Import',
      searchPlaceholder: 'EC No. / Title / Model',
      detailCount: 'Detail Lines',
      selectRequired: 'Select one source EC',
      loadToForm: 'Load to Form',
      plantFromCompany: 'Company {company} → Plant {plant}',
      companyRequired: 'Select a company before importing source EC',
      formTitle: 'Import from Source EC (fill leader, category, upload attachments)',
      attachmentRequired: 'Add at least one attachment and upload the file',
      attachmentUploadRequired: 'Row {row}: attachment file not uploaded',
      importSelected: 'Import Selected ({count})',
      importSuccess: 'Successfully imported {count} EC record(s)',
      importPartial: 'Import finished: {success} succeeded, {fail} failed',
    },
    attachment: {
      docCode: {
        formatInvalid: 'Invalid document code format ({hint})',
        duplicate: 'Document code "{code}" already exists',
        hint: {
          empty: 'Enter document code',
          ec: 'Must match EC number',
          eppFpp: 'P-xxxx (P- + 4 digits, e.g. P-0001)',
          tl: 'DTS-xxxx (DTS- + 4 digits, e.g. DTS-0001)',
          quadDash: 'xxxx-xxxx (4 digits each, e.g. 1234-5678)',
        },
      },
      fileName: {
        duplicate: 'File name "{name}" already exists',
      },
    },
    tabs: {
      oldNewMaterial: 'Old / New Material',
    },
  },
};
