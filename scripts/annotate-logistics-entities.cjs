// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：annotate-logistics-entities.cjs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：同步 Logistics 实体 XML 注释（修 {    ///、从 i18n / 字典映射回填字段说明）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  entityClassToSlug,
  resolveEntityFieldI18nSegment,
} = require('./generate-script-common.cjs');

const ROOT = path.resolve(__dirname, '../backend/src');
const ENTITIES_DIRS = [
  path.join(ROOT, 'Takt.Domain/Entities/Logistics/Manufacturing'),
  path.join(ROOT, 'Takt.Domain/Entities/Logistics/CustomerService'),
  path.join(ROOT, 'Takt.Domain/Entities/Logistics/Maintenance'),
];
const I18N_ROOTS = [
  path.join(ROOT, 'Takt.Infrastructure/Data/Seeds/I18nSeedData/Logistics/Manufacturing'),
  path.join(ROOT, 'Takt.Infrastructure/Data/Seeds/I18nSeedData/Logistics/CustomerService'),
  path.join(ROOT, 'Takt.Infrastructure/Data/Seeds/I18nSeedData/Logistics/Maintenance'),
];

const ENTITY_BASE_PROPS = new Set([
  'Id', 'TenantCode', 'CompanyCode', 'ExtField', 'Remark',
  'CreatedBy', 'CreatedAt', 'UpdatedBy', 'UpdatedAt',
  'IsDeleted', 'DeletedBy', 'DeletedAt',
  'ApprovalStatus', 'InitiatorId', 'InitiatedAt', 'ApprovalOpinion', 'ApprovedBy', 'ApprovedAt',
  'FlowInstanceId',
]);

const FK_SUMMARY_OVERRIDES = {
  PlantCode: '工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）',
  ClientId: '客户端ID（关联 TaktClient.Id，选项 TaktClients/options）',
  ClientCode: '客户端编码（冗余，关联 TaktClient.ClientCode）',
  MaterialId: '物料ID（关联 TaktMaterialPlant.Id，选项 TaktMaterialPlants/options）',
  EquipmentId: '设备ID（关联 TaktEquipment.Id，选项 TaktEquipments/options）',
  CostCenterId: '成本中心ID（关联 TaktCostCenter.Id，选项 TaktCostCenters/options）',
  CostElementId: '成本要素ID（关联 TaktCostElement.Id，选项 TaktCostElements/options）',
  EmployeeId: '员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）',
  WorkstationId: '工位ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）',
  MaintenanceWorkOrderId: '维护工单ID（关联 TaktMaintenanceWorkOrder.Id，选项 TaktMaintenanceWorkOrders/options）',
  MaintenanceNotificationId: '来源维护通知单ID（关联 TaktMaintenanceNotification.Id，选项 TaktMaintenanceNotifications/options）',
  ServiceRequestId: '关联服务请求ID（关联 TaktServiceRequest.Id，选项 TaktServiceRequests/options）',
  ServiceOrderId: '关联服务订单ID（关联 TaktServiceOrder.Id，选项 TaktServiceOrders/options）',
  ServiceContractId: '关联服务合同ID（关联 TaktServiceContract.Id，选项 TaktServiceContracts/options）',
  AssignedEmployeeId: '指派员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）',
};

const CLASS_SUMMARY_APPEND = {
  TaktMaintenanceWorkOrder: '审批态见基类 ApprovalStatus，字典 sys_approval_status。',
  TaktMaintenanceNotification: '审批态见基类 ApprovalStatus，字典 sys_approval_status。',
};

const ENTITY_CLASS_FIELD_COMMENTS = {
  TaktServiceTicket: {
    TicketType: '工单类型（字典 logistics_service_ticket_type；0=维修，1=巡检，2=安装，3=升级，4=其他）',
    Priority: '优先级（字典 sys_priority_level_category；1=最高，2=高，3=普通，4=低）',
    AcceptanceResult: '验收结果（字典 logistics_acceptance_result；0=不合格，1=合格，2=部分合格）',
    TicketStatus: '工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）',
  },
  TaktServiceContract: {
    ContractType: '合同类型（字典 logistics_service_contract_type；0=维保，1=单次，2=框架，3=SLA，4=其他）',
    PaymentTerms: '付款条件（字典 logistics_service_payment_terms；0=预付，1=后付，2=月结30天，3=月结60天，4=其他）',
    ContractStatus: '合同状态（字典 logistics_service_contract_status；0=草稿，1=生效，2=暂停，3=到期，4=终止）',
  },
  TaktServiceOrder: {
    OrderType: '订单类型（字典 logistics_service_order_type；0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）',
    OrderStatus: '订单状态（字典 logistics_service_order_status；0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）',
  },
  TaktServiceRequest: {
    RequestType: '请求类型（字典 logistics_service_request_type；0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）',
    SourceChannel: '请求来源（字典 logistics_service_source_channel；0=电话，1=邮件，2=门户，3=现场，4=其他）',
    Priority: '优先级（字典 sys_priority_level_category；1=最高，2=高，3=普通，4=低）',
    RequestStatus: '请求状态（字典 logistics_service_request_status；0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）',
  },
  TaktEquipment: {
    EquipmentType: '登录设备（字典 logistics_equipment_type；0=生产设备，1=检测设备，2=包装设备，3=物流设备，4=辅助设备）',
    IsCritical: '是否关键设备（字典 sys_yes_no_type；0=否，1=是）',
    WarrantyStatus: '保修状态（字典 logistics_warranty_status；0=无保修，1=保修期内，2=保修期外，3=延保中）',
    EquipmentStatus: '设备状态（字典 sys_equipment_status；0=运行中，1=停机，2=维修中，3=故障，4=待报废，5=已报废）',
  },
  TaktMaintenanceWorkOrder: {
    MaintenanceCategory: '维护类别（字典 logistics_maintenance_category；1=预防性维护，2=纠正性维护，3=预测性维护，4=紧急维修，5=定期保养，6=大修，7=改造升级）',
    MaintenanceType: '维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）',
    Priority: '优先级（字典 sys_priority_level_category；1=最高，2=高，3=普通，4=低）',
    MaintenanceResult: '维护结果（字典 logistics_maintenance_result；0=正常，1=待观察，2=需再次维修，3=已报废）',
    IsHistoryArchived: '是否已归档至维护履历（字典 sys_yes_no_type；0=否，1=是）',
    WorkOrderStatus: '工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）',
    SettlementStatus: '结算状态（字典 logistics_settlement_status；0=未结算，1=部分结算，2=已结算）',
  },
  TaktMaintenanceNotification: {
    MaintenanceCategory: '维护类别（字典 logistics_maintenance_category；1=预防性维护，2=纠正性维护，3=预测性维护，4=紧急维修，5=定期保养，6=大修，7=改造升级）',
    Priority: '优先级（字典 sys_priority_level_category；1=最高，2=高，3=普通，4=低）',
    NotificationStatus: '通知单状态（字典 logistics_maintenance_notification_status；0=新建，1=已转工单，2=已关闭，3=已取消）',
  },
  TaktMaintenanceHistory: {
    MaintenanceType: '维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）',
    MaintenanceCategory: '维护类别（字典 logistics_maintenance_category；1=预防性维护，2=纠正性维护，3=预测性维护，4=紧急维修，5=定期保养，6=大修，7=改造升级）',
    MaintenanceResult: '维护结果（字典 logistics_maintenance_result；0=正常，1=待观察，2=需再次维修，3=已报废）',
    MaintenanceStatus: '履历状态（固定归档值 2=已完成，只读展示）',
  },
  TaktMaintenanceWorkOrderMaterial: {
    IssueStatus: '领料状态（字典 logistics_maintenance_issue_status；0=待领料，1=部分领料，2=已领料）',
  },
  TaktMaintenanceWorkOrderLabor: {
    ConfirmationStatus: '报工确认状态（字典 logistics_maintenance_confirmation_status；0=待确认，1=已确认）',
  },
};

/**
 * @param {string} dir
 * @returns {string[]}
 */
function listCsFiles(dir) {
  if (!fs.existsSync(dir)) {
    return [];
  }
  const out = [];
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      out.push(...listCsFiles(full));
    } else if (entry.name.endsWith('.cs')) {
      out.push(full);
    }
  }
  return out;
}

/**
 * @returns {Map<string, string>}
 */
function loadI18nZhContextNotes() {
  const map = new Map();
  for (const i18nRoot of I18N_ROOTS) {
    const files = listCsFiles(i18nRoot);
    const re = /new TranslationSeedItem\("([^"]+)",\s*"zh-CN",\s*"[^"]*",\s*"((?:[^"\\]|\\.)*)"\)/g;
    for (const file of files) {
      const content = fs.readFileSync(file, 'utf8');
      let m;
      while ((m = re.exec(content)) !== null) {
        const [, i18nKey, contextNote] = m;
        if (i18nKey.endsWith('._self')) {
          continue;
        }
        const note = contextNote.trim();
        if (note) {
          map.set(i18nKey, note);
        }
      }
    }
  }
  return map;
}

/**
 * @param {string} content
 * @returns {string|null}
 */
function parseClassName(content) {
  const m = content.match(/public class (Takt\w+)/);
  return m ? m[1] : null;
}

/**
 * 仅解析带 SugarColumn 的标量属性（排除导航属性）
 * @param {string} content
 * @returns {string[]}
 */
function parseScalarPropertyNames(content) {
  const names = [];
  const re = /\/\/\/ <summary>[\s\S]*?<\/summary>\s*(?:\[[^\]]+\]\s*)*\[SugarColumn[\s\S]*?\n\s*public\s+(?:[\w.?<>,\[\]\s]+)\s+(\w+)\s*\{/g;
  let m;
  while ((m = re.exec(content)) !== null) {
    names.push(m[1]);
  }
  return names;
}

/**
 * 修正类体起始 `{    ///` 格式
 * @param {string} content
 * @returns {string}
 */
function fixClassBodyBraceFormat(content) {
  return content.replace(/\{    \/\//g, '{\n    ///');
}

/**
 * 追加类级 summary（仅匹配 [SugarTable] 前、无缩进的类注释）
 * @param {string} content
 * @param {string} className
 * @returns {string}
 */
function appendClassSummary(content, className) {
  const append = CLASS_SUMMARY_APPEND[className];
  if (!append) {
    return content;
  }
  const re = /(\/\/\/ <summary>\s*\n\/\/\/ )([\s\S]*?)(\n\/\/\/ <\/summary>\s*\n\[SugarTable)/m;
  return content.replace(re, (full, p1, body, p3) => {
    const trimmed = body.trim();
    if (trimmed.includes(append)) {
      return full;
    }
    const sep = trimmed.endsWith('。') || trimmed.endsWith('）') ? '' : '。';
    return `${p1}${trimmed}${sep}${append}${p3}`;
  });
}

/**
 * 替换标量属性 summary（必须紧邻 [SugarColumn]）
 * @param {string} content
 * @param {string} propName
 * @param {string} newSummary
 * @returns {string}
 */
function replacePropertySummary(content, propName, newSummary) {
  const escaped = propName.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  const re = new RegExp(
    `(\\n    /// <summary>\\s*\\n    /// )([\\s\\S]*?)(\\n    /// </summary>\\s*\\n(?:    \\[[^\\]]+\\]\\s*\\n)*    \\[SugarColumn[\\s\\S]*?\\n    public[^\\n]*\\b${escaped}\\s*\\{)`,
    'm'
  );
  if (!re.test(content)) {
    return content;
  }
  return content.replace(re, `$1${newSummary}$3`);
}

/**
 * @param {string} content
 * @returns {string}
 */
function ensureNavigateRegion(content) {
  if (content.includes('导航属性区域')) {
    return content;
  }
  const idx = content.search(/\n    \[Navigate\(/);
  if (idx === -1) {
    return content;
  }
  const before = content.slice(0, idx);
  const after = content.slice(idx);
  return `${before}\n    // ========================================\n    // 导航属性区域\n    // ========================================${after}`;
}

/**
 * @param {string} filePath
 * @param {Map<string, string>} i18nMap
 */
function processEntityFile(filePath, i18nMap) {
  let content = fs.readFileSync(filePath, 'utf8');
  const original = content;

  content = fixClassBodyBraceFormat(content);
  content = content.replace(/\r\n/g, '\n');

  const className = parseClassName(content);
  if (!className) {
    return { filePath, changed: false };
  }

  content = appendClassSummary(content, className);

  let slug;
  try {
    slug = entityClassToSlug(className);
  } catch {
    return { filePath, changed: false };
  }

  const classFieldComments = ENTITY_CLASS_FIELD_COMMENTS[className] || {};
  const props = parseScalarPropertyNames(content);
  for (let i = props.length - 1; i >= 0; i -= 1) {
    const propName = props[i];
    if (ENTITY_BASE_PROPS.has(propName)) {
      continue;
    }
    let summary =
      classFieldComments[propName] ||
      FK_SUMMARY_OVERRIDES[propName];
    if (!summary) {
      const segment = resolveEntityFieldI18nSegment(propName, slug);
      const i18nKey = `entity.${slug}.${segment}`;
      summary = i18nMap.get(i18nKey);
    }
    if (summary) {
      content = replacePropertySummary(content, propName, summary);
    }
  }

  content = ensureNavigateRegion(content);

  if (content !== original) {
    fs.writeFileSync(filePath, content, 'utf8');
    return { filePath, changed: true };
  }
  return { filePath, changed: false };
}

function main() {
  const i18nMap = loadI18nZhContextNotes();
  let changedCount = 0;
  for (const dir of ENTITIES_DIRS) {
    for (const file of listCsFiles(dir)) {
      const result = processEntityFile(file, i18nMap);
      if (result.changed) {
        changedCount += 1;
        console.log(`updated: ${path.relative(ROOT, result.filePath)}`);
      }
    }
  }
  console.log(`done: ${changedCount} file(s) updated`);
}

main();
