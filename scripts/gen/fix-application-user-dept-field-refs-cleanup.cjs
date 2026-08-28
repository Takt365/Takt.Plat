'use strict';

/**
 * 修复 Application 层人员/部门字段重复后缀（如 AcceptedByEmployeeNameEmployeeName）。
 * 用法: node scripts/gen/fix-application-user-dept-field-refs-cleanup.cjs
 */

const fs = require('fs');
const path = require('path');

const appRoot = path.join(__dirname, '../../backend/src/Takt.Application');

/** @type {Array<[string, string]>} */
const CLEANUPS = [
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
  ['EvaluationDeptNameName', 'EvaluationDeptName'],
  ['ResponsibleDeptNameId', 'ResponsibleDeptId'],
  ['ResponsiblePersonNameId', 'ResponsiblePersonId'],
  ['EquipAdministratorNameName', 'EquipAdministratorName'],
  ['ImprovementResponsible', 'ImprovementResponsibleName'],
];

/** @param {string} dir */
function walkCsFiles(dir) {
  /** @type {string[]} */
  const out = [];
  for (const name of fs.readdirSync(dir)) {
    const full = path.join(dir, name);
    if (fs.statSync(full).isDirectory()) {
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
  for (const [from, to] of CLEANUPS) {
    content = content.split(from).join(to);
  }
  if (content !== original) {
    fs.writeFileSync(filePath, content);
    filesChanged += 1;
    console.log('FIXED:', path.relative(appRoot, filePath));
  }
}

console.log(`\nCleanup done. ${filesChanged} file(s) updated.`);
