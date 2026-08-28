'use strict';

/**
 * 同步 Application 层对已重命名实体人员/部门字段的引用（DTO / Service / Validator）。
 * 用法: node scripts/gen/fix-application-user-dept-field-refs.cjs
 */

const fs = require('fs');
const path = require('path');

const appRoot = path.join(__dirname, '../../backend/src/Takt.Application');

/** @type {Array<[string, string]>} */
const REPLACEMENTS = [
  ['AcceptedByEmployeeNameEmployeeName', 'AcceptedByEmployeeName'],
  ['AccountManagerEmployeeNameEmployeeName', 'AccountManagerEmployeeName'],
  ['ReportedByEmployeeNameEmployeeName', 'ReportedByEmployeeName'],
  ['EnteredByEmployeeNameEmployeeName', 'EnteredByEmployeeName'],
  ['ServiceEmployeeNameEmployeeName', 'ServiceEmployeeName'],
  ['CompanyManagerUserNameUserName', 'CompanyManagerUserName'],
  ['PlantManagerUserNameUserName', 'PlantManagerUserName'],
  ['ResponsiblePersonNameName', 'ResponsiblePersonName'],
  ['OperatorEmployeeNameEmployeeName', 'OperatorEmployeeName'],
  ['SalesEmployeeNameEmployeeName', 'SalesEmployeeName'],
  ['InquiryEmployeeNameEmployeeName', 'InquiryEmployeeName'],
  ['RequestEmployeeNameEmployeeName', 'RequestEmployeeName'],
  ['JudgeByEmployeeNameEmployeeName', 'JudgeByEmployeeName'],
  ['PostedByEmployeeNameEmployeeName', 'PostedByEmployeeName'],
  ['EvaluatorByEmployeeNameEmployeeName', 'EvaluatorByEmployeeName'],
  ['ResponsibleDeptNameName', 'ResponsibleDeptName'],
  ['SurveyorByEmployeeNameEmployeeName', 'SurveyorByEmployeeName'],
  ['CompanyManagerUserName', 'CompanyManagerUserName'],
  ['PlantManagerUserName', 'PlantManagerUserName'],
  ['AccountManagerEmployeeName', 'AccountManagerEmployeeName'],
  ['ServiceEmployeeName', 'ServiceEmployeeName'],
  ['AcceptedByEmployeeName', 'AcceptedByEmployeeName'],
  ['ReportedByEmployeeName', 'ReportedByEmployeeName'],
  ['EnteredByEmployeeName', 'EnteredByEmployeeName'],
  ['CompanyManager', 'CompanyManagerUserName'],
  ['PlantManager', 'PlantManagerUserName'],
  ['AccountManager', 'AccountManagerEmployeeName'],
  ['ServiceBy', 'ServiceEmployeeName'],
  ['AcceptedBy', 'AcceptedByEmployeeName'],
  ['ReportedBy', 'ReportedByEmployeeName'],
  ['EnteredBy', 'EnteredByEmployeeName'],
  ['ManagerUserCode', 'ManagerUserName'],
  ['SalesBy', 'SalesEmployeeName'],
  ['InquiryBy', 'InquiryEmployeeName'],
  ['RequestBy', 'RequestEmployeeName'],
  ['PlanBy', 'PlannerName'],
  ['PlannerId', 'PlannerEmployeeId'],
  ['EvaluatorBy', 'EvaluatorByEmployeeName'],
  ['EvaluationDept', 'EvaluationDeptName'],
  ['InspectorBy', 'InspectorName'],
  ['HandlerBy', 'HandlerName'],
  ['ResponsibleUserBy', 'ResponsibleUserName'],
  ['OperatorBy', 'OperatorEmployeeName'],
  ['DeptBy', 'DeptName'],
  ['JudgeBy', 'JudgeByEmployeeName'],
  ['ResponsibleDept', 'ResponsibleDeptName'],
  ['ResponsibleBy', 'ResponsiblePersonName'],
  ['RepairOperator', 'RepairOperatorName'],
  ['SurveyorBy', 'SurveyorByEmployeeName'],
  ['EquipAdministrator', 'EquipAdministratorName'],
];

/** @type {Array<[RegExp, string]>} */
const REGEX_REPLACEMENTS = [
  [/\bInquiryId\b/g, 'InquiryEmployeeId'],
  [/\bRequestId\b/g, 'RequestEmployeeId'],
  [/\bPostedBy\b(?!Employee)/g, 'PostedByEmployeeName'],
];

/** @param {string} dir */
function walkCsFiles(dir) {
  /** @type {string[]} */
  const out = [];
  for (const name of fs.readdirSync(dir)) {
    const full = path.join(dir, name);
    const st = fs.statSync(full);
    if (st.isDirectory()) {
      out.push(...walkCsFiles(full));
    } else if (name.endsWith('.cs')) {
      out.push(full);
    }
  }
  return out;
}

let filesChanged = 0;
for (const filePath of walkCsFiles(appRoot)) {
  let content = fs.readFileSync(filePath, 'utf8');
  const original = content;
  for (const [from, to] of REPLACEMENTS) {
    if (from === to) {
      continue;
    }
    content = content.split(from).join(to);
  }
  for (const [re, to] of REGEX_REPLACEMENTS) {
    content = content.replace(re, to);
  }
  if (content !== original) {
    fs.writeFileSync(filePath, content);
    filesChanged += 1;
    console.log('UPDATED:', path.relative(appRoot, filePath));
  }
}

console.log(`\nDone. ${filesChanged} file(s) updated.`);
