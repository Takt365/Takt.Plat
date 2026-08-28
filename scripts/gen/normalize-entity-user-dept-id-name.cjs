'use strict';

/**
 * 全量统一实体用户/部门/员工选项字段为 XxxId + XxxName 模式。
 * 参照 TaktExpense / TaktMessage / TaktCustomerComplaint。
 *
 * 用法: node scripts/gen/normalize-entity-user-dept-id-name.cjs
 */

const fs = require('fs');
const path = require('path');

const entitiesRoot = path.join(__dirname, '../../backend/src/Takt.Domain/Entities');

/** @param {string} rel */
function entityPath(rel) {
  return path.join(entitiesRoot, rel.replace(/\//g, path.sep));
}

/**
 * @param {string} filePath
 * @param {Array<[RegExp|string, string]>} replacements
 */
function applyReplacements(filePath, replacements) {
  if (!fs.existsSync(filePath)) {
    console.warn('SKIP (missing):', filePath);
    return 0;
  }
  let content = fs.readFileSync(filePath, 'utf8');
  let count = 0;
  for (const [from, to] of replacements) {
    const next = typeof from === 'string' ? content.split(from).join(to) : content.replace(from, to);
    if (next !== content) {
      count += 1;
      content = next;
    }
  }
  if (count > 0) {
    fs.writeFileSync(filePath, content);
    console.log('UPDATED:', path.relative(entitiesRoot, filePath), `(${count} blocks)`);
  }
  return count;
}

const EMPLOYEE_NAME_BLOCK = (idProp, idCol, nameProp, nameCol, label, nullable = true) => {
  const nt = nullable ? '?' : '';
  const nl = nullable ? 'true' : 'false';
  return `    /// <summary>
    /// ${label}名称（冗余：按 ${idProp} 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "${nameCol}", ColumnDescription = "${label}名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ${nameProp} { get; set; }`;
};

const USER_NAME_BLOCK = (idProp, nameProp, nameCol, label) => `    /// <summary>
    /// ${label}（冗余：按 ${idProp} 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "${nameCol}", ColumnDescription = "${label}", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? ${nameProp} { get; set; }`;

const DEPT_NAME_BLOCK = (idProp, nameProp, nameCol, label) => `    /// <summary>
    /// ${label}（冗余：按 ${idProp} 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "${nameCol}", ColumnDescription = "${label}", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ${nameProp} { get; set; }`;

const DEFECT_HANDLING_TRIPLE = `    /// <summary>
    /// 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_dept_id", ColumnDescription = "责任部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleDeptId { get; set; }
${DEPT_NAME_BLOCK('ResponsibleDeptId', 'ResponsibleDeptName', 'responsible_dept_name', '责任部门名称')}
    /// <summary>
    /// 责任人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_person_id", ColumnDescription = "责任人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsiblePersonId { get; set; }
${EMPLOYEE_NAME_BLOCK('ResponsiblePersonId', 'responsible_person_id', 'ResponsiblePersonName', 'responsible_person_name', '责任人')}
    /// <summary>
    /// 处理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "handler_id", ColumnDescription = "处理人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HandlerId { get; set; }
${EMPLOYEE_NAME_BLOCK('HandlerId', 'handler_id', 'HandlerName', 'handler_name', '处理人')}`;

const DEFECT_HANDLING_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ 责任部门[\s\S]*?\r?\n    public string\? ResponsibleDept \{ get; set; \}\r?\n    \/\/\/ <summary>[\s\S]*?\r?\n    public string\? HandlerBy \{ get; set; \}/;

const SALES_BY_NEW = `    /// <summary>
    /// 销售员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sales_employee_id", ColumnDescription = "销售员ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesEmployeeId { get; set; }
${EMPLOYEE_NAME_BLOCK('SalesEmployeeId', 'sales_employee_id', 'SalesEmployeeName', 'sales_employee_name', '销售员')}`;

const SALES_BY_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ 销售员[\s\S]*?\r?\n    public string\? SalesBy \{ get; set; \}/;

const POSTED_BY_NEW = `    /// <summary>
    /// 过账人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "posted_by_employee_id", ColumnDescription = "过账人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostedByEmployeeId { get; set; }
${EMPLOYEE_NAME_BLOCK('PostedByEmployeeId', 'posted_by_employee_id', 'PostedByEmployeeName', 'posted_by_employee_name', '过账人')}`;

const POSTED_BY_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ (?:用户名|已创建的)[\s\S]*?DictValue=EmployeeCode[\s\S]*?\r?\n    public string\? PostedBy \{ get; set; \}/;

const INSPECTOR_NEW = `    /// <summary>
    /// 检验员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inspector_id", ColumnDescription = "检验员ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InspectorId { get; set; }
${EMPLOYEE_NAME_BLOCK('InspectorId', 'inspector_id', 'InspectorName', 'inspector_name', '检验员')}`;

const INSPECTOR_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ 检验员[\s\S]*?\r?\n    public string\? InspectorBy \{ get; set; \}/;

const INSPECTOR_NAME_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ 检验员[\s\S]*?\r?\n    public string\? InspectorName \{ get; set; \}/;

const JUDGE_NEW = `    /// <summary>
    /// 判定人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "judge_by_employee_id", ColumnDescription = "判定人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? JudgeByEmployeeId { get; set; }
${EMPLOYEE_NAME_BLOCK('JudgeByEmployeeId', 'judge_by_employee_id', 'JudgeByEmployeeName', 'judge_by_employee_name', '判定人')}`;

const JUDGE_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ 判定人[\s\S]*?\r?\n    public string\? JudgeBy \{ get; set; \}/;

const PLANNER_NAME_BLOCK = `    /// <summary>
    /// 计划人名称（冗余：按 PlannerId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "planner_name", ColumnDescription = "计划人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? PlannerName { get; set; }`;

const PLAN_BY_OLD = /\r?\n    \/\/\/ <summary>\r?\n    \/\/\/ 计划人[\s\S]*?\r?\n    public string PlanBy \{ get; set; \} = string\.Empty;\r?\n/;

const EC_DEPT_NEW = `    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }
${DEPT_NAME_BLOCK('DeptId', 'DeptName', 'dept_name', '部门名称')}`;

const EC_DEPT_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ 部门编码[\s\S]*?\r?\n    public string DeptCode \{ get; set; \} = string\.Empty;/;

const MANAGER_USER_NEW = `    /// <summary>
    /// 仓库负责人（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "manager_user_id", ColumnDescription = "仓库负责人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerUserId { get; set; }
${USER_NAME_BLOCK('ManagerUserId', 'ManagerUserName', 'manager_user_name', '仓库负责人姓名')}`;

const MANAGER_USER_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ 仓库负责人用户编码[\s\S]*?\r?\n    public string\? ManagerUserCode \{ get; set; \}/;

const TEAM_LEADER_NEW = `    /// <summary>
    /// 班组长（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "team_leader_employee_id", ColumnDescription = "班组长ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TeamLeaderEmployeeId { get; set; }
    /// <summary>
    /// 班组长名称（冗余：按 TeamLeaderEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "team_leader_name", ColumnDescription = "班组长名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? TeamLeaderName { get; set; }`;

const TEAM_LEADER_OLD = /    \/\/\/ <summary>\r?\n    \/\/\/ 班组长姓名[\s\S]*?\r?\n    public string\? TeamLeaderName \{ get; set; \}/;

/** Category B: additive Name fields */
const categoryB = [
  ['HumanResource/Attendance/TaktShiftSchedule.cs', [
    ['public long? DeptId { get; set; }', `public long? DeptId { get; set; }\n${DEPT_NAME_BLOCK('DeptId', 'DeptName', 'dept_name', '部门名称')}`],
    ['public long? EmployeeId { get; set; }', `public long? EmployeeId { get; set; }\n${EMPLOYEE_NAME_BLOCK('EmployeeId', 'employee_id', 'EmployeeName', 'employee_name', '员工')}`],
    ['部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）', '部门（选项 TaktDepts/tree-options；DictValue=Id；ScheduleType=0 时必填）'],
    ['员工（选项 TaktEmployees/options；ScheduleType=1 时必填，DictValue=Id）', '员工（选项 TaktEmployees/options；DictValue=Id；ScheduleType=1 时必填）'],
  ]],
  ['HumanResource/Performance/TaktPerfAssessment.cs', [
    [/public long\?? ReviewerId \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('ReviewerId', 'reviewer_id', 'ReviewerName', 'reviewer_name', '评审人')}`],
  ]],
  ['HumanResource/Performance/TaktPerfAnalysis.cs', [
    [/public long\?? MentorId \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('MentorId', 'mentor_id', 'MentorName', 'mentor_name', '指导老师')}`],
  ]],
  ['HumanResource/Talent/TaktTalentStaffingRequirement.cs', [
    [/public long\?? DeptId \{ get; set; \}/, (m) => `${m}\n${DEPT_NAME_BLOCK('DeptId', 'DeptName', 'dept_name', '申请部门名称')}`],
    [/public long\?? ReplaceEmployeeId \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('ReplaceEmployeeId', 'replace_employee_id', 'ReplaceEmployeeName', 'replace_employee_name', '替补员工')}`],
    ['申请部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）', '申请部门（选项 TaktDepts/tree-options；DictValue=Id）'],
  ]],
  ['HumanResource/Talent/TaktTalentOffer.cs', [
    [/public long\?? EmployeeId \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('EmployeeId', 'employee_id', 'EmployeeName', 'employee_name', '员工')}`],
    ['所属部门（选项 TaktDepts/tree-options,DictValue=Id）', '所属部门（选项 TaktDepts/tree-options；DictValue=Id）'],
  ]],
  ['Logistics/Manufacturing/Sop/TaktSopExec.cs', [
    [/public long EmployeeId \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('EmployeeId', 'employee_id', 'EmployeeName', 'employee_name', '员工', false)}`],
  ]],
  ['Logistics/Manufacturing/Sop/TaktSopEsdCheck.cs', [
    [/public long EmployeeId \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('EmployeeId', 'employee_id', 'EmployeeName', 'employee_name', '员工', false)}`],
  ]],
  ['Logistics/Manufacturing/Sop/TaktSopExecStep.cs', [
    [/public long\?? ConfirmedBy \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('ConfirmedBy', 'confirmed_by', 'ConfirmedByName', 'confirmed_by_name', '确认人')}`],
  ]],
  ['Logistics/Manufacturing/Sop/TaktSopCall.cs', [
    [/public long CallerId \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('CallerId', 'caller_id', 'CallerName', 'caller_name', '呼叫人', false)}`],
    [/public long\?? RespondedBy \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('RespondedBy', 'responded_by', 'RespondedByName', 'responded_by_name', '响应人')}`],
  ]],
];

/** Read TaktSopAck for AcknowledgedBy */
function patchSopAck() {
  const p = entityPath('Logistics/Manufacturing/Sop/TaktSopAck.cs');
  if (!fs.existsSync(p)) return;
  applyReplacements(p, [
    [/public long\?? AcknowledgedBy \{ get; set; \}/, (m) => `${m}\n${EMPLOYEE_NAME_BLOCK('AcknowledgedBy', 'acknowledged_by', 'AcknowledgedByName', 'acknowledged_by_name', '确认人')}`],
  ]);
}

function patchPurchaseRequest() {
  const p = entityPath('Logistics/Procurement/TaktPurchaseRequest.cs');
  applyReplacements(p, [
    [/\[SugarIndex\("ix_takt_logistics_procurement_purchase_request_request_by"[\s\S]*?\)\]\r?\n/, ''],
    ['nameof(RequestBy)', 'nameof(RequestEmployeeId)'],
    ['nameof(RequestId)', 'nameof(RequestEmployeeId)'],
    ['ix_takt_logistics_procurement_purchase_request_request_id', 'ix_takt_logistics_procurement_purchase_request_request_employee_id'],
    [`    /// <summary>
    /// 申请人员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "request_id", ColumnDescription = "申请人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestId { get; set; }
    /// <summary>
    /// 申请人（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "request_by", ColumnDescription = "申请人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string RequestBy { get; set; } = string.Empty;`, `    /// <summary>
    /// 申请人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "request_employee_id", ColumnDescription = "申请人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestEmployeeId { get; set; }
${EMPLOYEE_NAME_BLOCK('RequestEmployeeId', 'request_employee_id', 'RequestEmployeeName', 'request_employee_name', '申请人')}`],
  ]);
}

function patchPurchaseInquiry() {
  const p = entityPath('Logistics/Procurement/TaktPurchaseInquiry.cs');
  applyReplacements(p, [
    [`    /// <summary>
    /// 询价人员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_id", ColumnDescription = "询价人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryId { get; set; }
    /// <summary>
    /// 询价人（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_by", ColumnDescription = "询价人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string InquiryBy { get; set; } = string.Empty;`, `    /// <summary>
    /// 询价人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_employee_id", ColumnDescription = "询价人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryEmployeeId { get; set; }
${EMPLOYEE_NAME_BLOCK('InquiryEmployeeId', 'inquiry_employee_id', 'InquiryEmployeeName', 'inquiry_employee_name', '询价人')}`],
  ]);
}

function patchComplaintHandling() {
  const p = entityPath('Logistics/Quality/Complaint/TaktCustomerComplaintHandling.cs');
  applyReplacements(p, [[DEFECT_HANDLING_OLD, DEFECT_HANDLING_TRIPLE]]);
}

function patchSupplierEvaluation() {
  const p = entityPath('Logistics/Quality/Complaint/TaktSupplierEvaluation.cs');
  applyReplacements(p, [
    [`    /// <summary>
    /// 评价人（选项 TaktEmployees/options；DictValue=EmployeeCode）
    /// </summary>
    [SugarColumn(ColumnName = "evaluator_by", ColumnDescription = "评价人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? EvaluatorBy { get; set; }
    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_dept", ColumnDescription = "评价部门", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? EvaluationDept { get; set; }`, `    /// <summary>
    /// 评价人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "evaluator_by_employee_id", ColumnDescription = "评价人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluatorByEmployeeId { get; set; }
${EMPLOYEE_NAME_BLOCK('EvaluatorByEmployeeId', 'evaluator_by_employee_id', 'EvaluatorByEmployeeName', 'evaluator_by_employee_name', '评价人')}
    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_dept_id", ColumnDescription = "评价部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluationDeptId { get; set; }
${DEPT_NAME_BLOCK('EvaluationDeptId', 'EvaluationDeptName', 'evaluation_dept_name', '评价部门名称')}`],
  ]);
}

function patchComplaintItem() {
  const p = entityPath('Logistics/Quality/Complaint/TaktCustomerComplaintItem.cs');
  applyReplacements(p, [
    [/    \/\/\/ <summary>\r?\n    \/\/\/ 改善责任人[\s\S]*?\r?\n    public string\? ImprovementResponsible \{ get; set; \}/, `    /// <summary>
    /// 改善责任人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "improvement_responsible_id", ColumnDescription = "改善责任人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ImprovementResponsibleId { get; set; }
${EMPLOYEE_NAME_BLOCK('ImprovementResponsibleId', 'improvement_responsible_id', 'ImprovementResponsibleName', 'improvement_responsible_name', '改善责任人')}`],
  ]);
}

function patchSatisfactionSurvey() {
  const p = entityPath('Logistics/Quality/Complaint/TaktCustomerSatisfactionSurvey.cs');
  applyReplacements(p, [
    [/    \/\/\/ <summary>\r?\n    \/\/\/ 调查人[\s\S]*?\r?\n    public string\? SurveyorBy \{ get; set; \}/, `    /// <summary>
    /// 调查人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "surveyor_by_employee_id", ColumnDescription = "调查人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SurveyorByEmployeeId { get; set; }
${EMPLOYEE_NAME_BLOCK('SurveyorByEmployeeId', 'surveyor_by_employee_id', 'SurveyorByEmployeeName', 'surveyor_by_employee_name', '调查人')}`],
  ]);
}

function patchMpsPersonnelFields(file, fields) {
  const p = entityPath(file);
  let content = fs.readFileSync(p, 'utf8');
  for (const [oldName, idProp, nameProp, idCol, label] of fields) {
    const oldRe = new RegExp(`    /// <summary>\\r?\\n    /// ${oldName}[\\s\\S]*?\\r?\\n    public string\\? ${oldName} \\{ get; set; \\}`);
    const neu = `    /// <summary>
    /// ${label}（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "${idCol}", ColumnDescription = "${label}ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ${idProp} { get; set; }
${EMPLOYEE_NAME_BLOCK(idProp, idCol, nameProp, idCol.replace(/_id$/, '_name'), label)}`;
    content = content.replace(oldRe, neu);
  }
  fs.writeFileSync(p, content);
  console.log('UPDATED:', file);
}

function patchDefectDetailRepair(file, propOld, idProp, nameProp, idCol, label) {
  const p = entityPath(file);
  applyReplacements(p, [
    [new RegExp(`    /// <summary>\\r?\\n    /// ${label}[\\s\\S]*?\\r?\\n    public string\\? ${propOld} \\{ get; set; \\}`), `    /// <summary>
    /// ${label}（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "${idCol}", ColumnDescription = "${label}ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ${idProp} { get; set; }
${EMPLOYEE_NAME_BLOCK(idProp, idCol, nameProp, idCol.replace(/_id$/, '_name'), label)}`],
  ]);
}

function patchCustomerComplaintDeptComment() {
  applyReplacements(entityPath('Logistics/Quality/Complaint/TaktCustomerComplaint.cs'), [
    ['责任部门 ID（选项 TaktDepts/options；DictValue=Id）', '责任部门（选项 TaktDepts/tree-options；DictValue=Id）'],
    ['责任部门名称', '责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）'],
    ['责任人姓名', '责任人名称（冗余：按 ResponsiblePersonId 取 TaktEmployee.EmployeeName 联动）'],
  ]);
}

function normalizeRedundantComments(content) {
  return content
    .replace(/（冗余字段，便于查询）/g, '（冗余：按对应 Id 取主数据名称联动）')
    .replace(/TaktDepts\/options/g, 'TaktDepts/tree-options')
    .replace(/DictValue=EmployeeCode/g, 'DictValue=Id')
    .replace(/DictValue=DeptCode/g, 'DictValue=Id')
    .replace(/DictValue=UserName/g, 'DictValue=Id')
    .replace(/，存员工姓名或工号/g, '；DictValue=Id')
    .replace(/（人员代码）/g, '（选项 TaktEmployees/options；DictValue=Id）');
}

function walkEntities(dir, fn) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) walkEntities(full, fn);
    else if (entry.name.endsWith('.cs')) fn(full);
  }
}

let total = 0;

// Category B
for (const [rel, reps] of categoryB) {
  const filePath = entityPath(rel);
  if (!fs.existsSync(filePath)) continue;
  let content = fs.readFileSync(filePath, 'utf8');
  let changed = 0;
  for (const [from, to] of reps) {
    const replacement = typeof to === 'function' ? (m) => to(m) : to;
    const next = typeof from === 'string'
      ? content.split(from).join(replacement)
      : content.replace(from, replacement);
    if (next !== content) { changed++; content = next; }
  }
  if (changed) {
    fs.writeFileSync(filePath, content);
    console.log('UPDATED:', rel, `(${changed})`);
    total += changed;
  }
}

patchSopAck();
patchPurchaseRequest();
patchPurchaseInquiry();
patchComplaintHandling();
patchSupplierEvaluation();
patchComplaintItem();
patchSatisfactionSurvey();
patchCustomerComplaintDeptComment();

// Defect handling x4
for (const f of [
  'Logistics/Quality/Operation/TaktFqcDefectHandling.cs',
  'Logistics/Quality/Operation/TaktIqcDefectHandling.cs',
  'Logistics/Quality/Operation/TaktIpqcDefectHandling.cs',
]) {
  applyReplacements(entityPath(f), [[DEFECT_HANDLING_OLD, DEFECT_HANDLING_TRIPLE]]);
}

// Sales
for (const f of [
  'Logistics/Sales/TaktSalesOrder.cs',
  'Logistics/Sales/TaktSalesQuotation.cs',
  'Logistics/Sales/TaktCustomer.cs',
]) {
  applyReplacements(entityPath(f), [[SALES_BY_OLD, SALES_BY_NEW]]);
}

// PostedBy
for (const f of [
  'Logistics/Sales/TaktSalesInvoice.cs',
  'Logistics/Sales/TaktSalesInvoiceItem.cs',
  'Logistics/Procurement/TaktPurchaseInvoice.cs',
  'Logistics/Materials/TaktMaterialDocument.cs',
  'Logistics/Materials/TaktMaterialDocumentItem.cs',
]) {
  applyReplacements(entityPath(f), [[POSTED_BY_OLD, POSTED_BY_NEW]]);
}

// Inspector / Judge
for (const f of [
  'Logistics/Quality/Operation/TaktFqcOrderItem.cs',
  'Logistics/Quality/Operation/TaktIqcOrderItem.cs',
  'Logistics/Quality/Operation/TaktIpqcOrderItem.cs',
]) {
  applyReplacements(entityPath(f), [[INSPECTOR_OLD, INSPECTOR_NEW]]);
}
for (const f of [
  'Logistics/Quality/Operation/TaktFqcOrder.cs',
  'Logistics/Quality/Operation/TaktIqcOrder.cs',
  'Logistics/Quality/Operation/TaktIpqcOrder.cs',
]) {
  applyReplacements(entityPath(f), [[JUDGE_OLD, JUDGE_NEW]]);
}

// Planner: remove PlanBy, ensure PlannerName
for (const f of [
  'Logistics/Manufacturing/Mds/TaktSalesForecast.cs',
  'Logistics/Manufacturing/Mrp/TaktMaterialRequirementsPlanning.cs',
  'Logistics/Manufacturing/Mrp/TaktProductionPlan.cs',
  'Logistics/Manufacturing/Mrp/TaktPurchasePlan.cs',
  'Logistics/Procurement/TaktPurchaseForecast.cs',
]) {
  const p = entityPath(f);
  if (!fs.existsSync(p)) continue;
  let c = fs.readFileSync(p, 'utf8');
  c = c.replace(PLAN_BY_OLD, `\n${PLANNER_NAME_BLOCK}\n`);
  if (!c.includes('PlannerName')) {
    c = c.replace(/public long\? PlannerId \{ get; set; \}/, `public long? PlannerId { get; set; }\n${PLANNER_NAME_BLOCK}`);
  }
  fs.writeFileSync(p, c);
  console.log('UPDATED:', f, '(planner)');
}

// EC dept
for (const f of [
  'Logistics/Manufacturing/EngineeringChange/TaktEcBukan.cs',
  'Logistics/Manufacturing/EngineeringChange/TaktEcExecutionTask.cs',
  'Logistics/Manufacturing/EngineeringChange/TaktEcHinkan.cs',
  'Logistics/Manufacturing/EngineeringChange/TaktEcKoubai.cs',
  'Logistics/Manufacturing/EngineeringChange/TaktEcSeikan.cs',
  'Logistics/Manufacturing/EngineeringChange/TaktEcSeizougijutsu.cs',
  'Logistics/Manufacturing/EngineeringChange/TaktEcSeizouikka.cs',
  'Logistics/Manufacturing/EngineeringChange/TaktEcSeizounika.cs',
  'Logistics/Manufacturing/EngineeringChange/TaktEcUkeken.cs',
]) {
  applyReplacements(entityPath(f), [[EC_DEPT_OLD, EC_DEPT_NEW]]);
}

applyReplacements(entityPath('Logistics/Materials/TaktWarehouse.cs'), [[MANAGER_USER_OLD, MANAGER_USER_NEW]]);
applyReplacements(entityPath('Logistics/Manufacturing/Mps/TaktProductionTeam.cs'), [[TEAM_LEADER_OLD, TEAM_LEADER_NEW]]);

patchMpsPersonnelFields('Logistics/Manufacturing/Mps/TaktEquipmentOperationRate.cs', [
  ['EquipmentOperator', 'EquipmentOperatorId', 'EquipmentOperatorName', 'equipment_operator_id', '设备操作员'],
  ['EquipmentMaintainer', 'EquipmentMaintainerId', 'EquipmentMaintainerName', 'equipment_maintainer_id', '设备维护员'],
  ['TeamLeader', 'TeamLeaderId', 'TeamLeaderName', 'team_leader_id', '班组长'],
]);
patchMpsPersonnelFields('Logistics/Manufacturing/Mps/TaktPersonnelOperationRate.cs', [
  ['TeamLeader', 'TeamLeaderId', 'TeamLeaderName', 'team_leader_id', '班组长'],
  ['Supervisor', 'SupervisorId', 'SupervisorName', 'supervisor_id', '主管'],
]);
applyReplacements(entityPath('Logistics/Manufacturing/Mps/TaktProductionEquipment.cs'), [
  [/    \/\/\/ <summary>\r?\n    \/\/\/ 设备管理员[\s\S]*?\r?\n    public string\? EquipAdministrator \{ get; set; \}/, `    /// <summary>
    /// 设备管理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "equip_administrator_id", ColumnDescription = "设备管理员ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipAdministratorId { get; set; }
${EMPLOYEE_NAME_BLOCK('EquipAdministratorId', 'equip_administrator_id', 'EquipAdministratorName', 'equip_administrator_name', '设备管理员')}`],
]);

patchDefectDetailRepair('Logistics/Manufacturing/Defect/TaktAssyDefectDetail.cs', 'RepairOperator', 'RepairOperatorId', 'RepairOperatorName', 'repair_operator_id', '修理员');
patchDefectDetailRepair('Logistics/Manufacturing/Defect/TaktPcbaRepairDetail.cs', 'RepairOperator', 'RepairOperatorId', 'RepairOperatorName', 'repair_operator_id', '修理员');
applyReplacements(entityPath('Logistics/Manufacturing/Defect/TaktPcbaInspectionDetail.cs'), [[INSPECTOR_NAME_OLD, INSPECTOR_NEW.replace('inspector_id', 'inspector_id').replace('InspectorId', 'InspectorId')]]);

// Global comment pass
walkEntities(entitiesRoot, (filePath) => {
  const raw = fs.readFileSync(filePath, 'utf8');
  const next = normalizeRedundantComments(raw);
  if (next !== raw) {
    fs.writeFileSync(filePath, next);
    console.log('COMMENTS:', path.relative(entitiesRoot, filePath));
  }
});

console.log('\n✨ normalize-entity-user-dept-id-name 完成');
