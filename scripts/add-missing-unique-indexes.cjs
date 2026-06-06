/**
 * 为非日志实体补全缺失的唯一索引（名称以 _unique 结尾）
 */
const fs = require('fs');
const path = require('path');

const ENTITIES_ROOT = path.join(__dirname, '../backend/src/Takt.Domain/Entities');

/** @type {Record<string, string>} 相对路径 -> 待插入的唯一索引行 */
const UNIQUE_INDEX_ADDITIONS = {
  'Code/Generator/TaktGenTableColumn.cs':
    '[SugarIndex("ix_takt_code_generator_gen_table_column_column_unique", nameof(GenTableId), OrderByType.Asc, nameof(DatabaseColumnName), OrderByType.Asc, true)]',
  'HumanResource/Personnel/TaktEmployeeDelegation.cs':
    '[SugarIndex("ix_delegation_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OriginalEmployeeId), OrderByType.Asc, nameof(ProxyEmployeeId), OrderByType.Asc, nameof(DelegationType), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Bom/TaktModelDestination.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_model_destination_unique", nameof(PlantCode), OrderByType.Asc, nameof(MaterialName), OrderByType.Asc, nameof(ModelName), OrderByType.Asc, nameof(DestinationName), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Bom/TaktPackaging.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_packaging_unique", nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Bom/TaktStandardOperationTime.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_bom_standard_operation_time_unique", nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(WorkCenter), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Defect/TaktAssyDefectDetail.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_defect_assy_detail_line_unique", nameof(AssyDefectId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Defect/TaktPcbaInspectionDetail.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique", nameof(PcbaInspectionId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Defect/TaktPcbaRepairDetail.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique", nameof(PcbaRepairId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/EngineeringChange/TaktEcAttachment.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_line_unique", nameof(EcId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/EngineeringChange/TaktEcDept.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_ec_dept_unique", nameof(EcnDetailId), OrderByType.Asc, nameof(DeptCode), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/EngineeringChange/TaktEcDetail.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_ec_detail_line_unique", nameof(EcId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Output/TaktAssyOutputDetail.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_output_assy_detail_line_unique", nameof(AssyOutputId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Output/TaktPcbaOutputDetail.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_output_pcba_detail_line_unique", nameof(PcbaOutputId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Manufacturing/Scheduling/TaktApsScheduleItem.cs':
    '[SugarIndex("ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique", nameof(ApsScheduleId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Complaint/TaktCustomerComplaintHandling.cs':
    '[SugarIndex("ix_takt_logistics_quality_customer_complaint_handling_code_unique", nameof(ComplaintHandlingCode), OrderByType.Asc, true)]',
  'Logistics/Quality/Complaint/TaktCustomerComplaintItem.cs':
    '[SugarIndex("ix_takt_logistics_quality_customer_complaint_item_line_unique", nameof(ComplaintId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Complaint/TaktCustomerSatisfactionSurveyItem.cs':
    '[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique", nameof(SurveyId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Complaint/TaktSupplierEvaluationItem.cs':
    '[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_item_line_unique", nameof(EvaluationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityFailureAssyRework.cs':
    '[SugarIndex("ix_takt_logistics_quality_failure_assy_rework_line_unique", nameof(QualityFailureId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityFailureMeeting.cs':
    '[SugarIndex("ix_takt_logistics_quality_failure_meeting_line_unique", nameof(QualityFailureId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityFailurePcbaRework.cs':
    '[SugarIndex("ix_takt_logistics_quality_failure_pcba_rework_line_unique", nameof(QualityFailureId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityOperationCalibration.cs':
    '[SugarIndex("ix_takt_logistics_quality_operation_calibration_line_unique", nameof(QualityOperationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityOperationCustomerResponse.cs':
    '[SugarIndex("ix_takt_logistics_quality_operation_customer_response_line_unique", nameof(QualityOperationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityOperationFirstArticle.cs':
    '[SugarIndex("ix_takt_logistics_quality_operation_first_article_line_unique", nameof(QualityOperationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityOperationIncoming.cs':
    '[SugarIndex("ix_takt_logistics_quality_operation_incoming_line_unique", nameof(QualityOperationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityOperationOther.cs':
    '[SugarIndex("ix_takt_logistics_quality_operation_other_line_unique", nameof(QualityOperationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityOperationOutgoing.cs':
    '[SugarIndex("ix_takt_logistics_quality_operation_outgoing_line_unique", nameof(QualityOperationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Logistics/Quality/Cost/TaktQualityOperationReliability.cs':
    '[SugarIndex("ix_takt_logistics_quality_operation_reliability_line_unique", nameof(QualityOperationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]',
  'Routine/TaktNotice.cs':
    '[SugarIndex("ix_notice_title_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(Title), OrderByType.Asc, true)]',
};

function isLogEntity(filePath) {
  const normalized = filePath.replace(/\\/g, '/');
  return normalized.includes('/Statistics/Logging/') || /ChangeLog\.cs$/i.test(normalized);
}

/**
 * @param {string} content
 * @param {string} indexLine
 */
function insertUniqueIndex(content, indexLine) {
  const indexNameMatch = indexLine.match(/\[SugarIndex\("([^"]+)"/);
  if (!indexNameMatch) {
    return content;
  }
  if (content.includes(`"${indexNameMatch[1]}"`)) {
    return content;
  }
  const classMatch = content.match(/^public class/m);
  if (!classMatch || classMatch.index === undefined) {
    return content;
  }
  const beforeClass = content.slice(0, classMatch.index);
  const afterClass = content.slice(classMatch.index);
  const trimmedBefore = beforeClass.endsWith('\n') ? beforeClass : `${beforeClass}\n`;
  return `${trimmedBefore}${indexLine}\n${afterClass}`;
}

function main() {
  const dryRun = process.argv.includes('--dry-run');
  let count = 0;
  for (const [rel, indexLine] of Object.entries(UNIQUE_INDEX_ADDITIONS)) {
    const file = path.join(ENTITIES_ROOT, rel);
    if (!fs.existsSync(file)) {
      console.warn(`跳过（文件不存在）: ${rel}`);
      continue;
    }
    if (isLogEntity(file)) {
      continue;
    }
    const original = fs.readFileSync(file, 'utf8');
    const updated = insertUniqueIndex(original, indexLine);
    if (updated === original) {
      continue;
    }
    count += 1;
    console.log(`+ ${rel}`);
    if (!dryRun) {
      fs.writeFileSync(file, updated, 'utf8');
    }
  }
  console.log(`\n${dryRun ? '[dry-run] ' : ''}共 ${count} 个实体补全唯一索引`);
}

main();
