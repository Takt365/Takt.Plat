// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：pcba-output-detail.d.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * PCBA明细实体
 * 对应前端 TaktPcbaOutputDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaOutputDetail
 * @description 对应后端 TaktPcbaOutputDetailDto
 */
export interface PcbaOutputDetail extends CompanyDtoBase {

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaOutputId?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
   */
  timePeriod?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
   */
  teamCode?: string;

  /**
   * 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
   */
  prodEquipCode?: string;

  /**
   * 直接人员
   */
  directLabor?: number;

  /**
   * 间接人员
   */
  indirectLabor?: number;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
   */
  stdMinutes?: number;

  /**
   * 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
   */
  stdLaborCapacity?: number;

  /**
   * 标准点数（PCBA 专用，按工作中心回填）
   */
  stdShorts?: number;

  /**
   * 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
   */
  stdEquipmentCapacity?: number;

  /**
   * PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）
   */
  pcbBoardType?: string;

  /**
   * 面板别（字典 logistics_pcba_side_category；存 DictValue：b= B面 t= T面）
   */
  panelSide?: string;

  /**
   * 批次数量
   */
  batchQty?: number;

  /**
   * 当日完成数
   */
  dailyCompletedQty?: number;

  /**
   * 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
   */
  totalCompletedQty?: number;

  /**
   * 完成状态（计算结果：字典 logistics_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
   */
  completedStatus?: number;

  /**
   * 序列号（明细级）
   */
  serialCode?: string;

  /**
   * 不良台数
   */
  defectCount?: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes?: number;

  /**
   * 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 投入工数(分钟)（计算结果：明细 DirectLabor×60）
   */
  inputMinutes?: number;

  /**
   * 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
   */
  actualMinutes?: number;

  /**
   * 修工数(分钟)
   */
  repairMinutes?: number;

  /**
   * 切换次数
   */
  switchCount?: number;

  /**
   * 切换时间(分钟)
   */
  switchTime?: number;

  /**
   * 切停机时间(分钟)
   */
  stopTime?: number;

  /**
   * 总工数(分钟)
   */
  totalMinutes?: number;

  /**
   * 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 报工工时(分钟)
   */
  confirmMinutes?: number;

  /**
   * 混合生产（0=非混合；N=此生产时段内另有N笔报工）
   */
  mixedProd?: number;

  /**
   * 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
   */
  achievementRate?: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * PcbaOutputDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaOutputDetailExport
 * @description 对应后端 TaktPcbaOutputDetailExportDto
 */
export interface PcbaOutputDetailExport {
  /**
   * PcbaOutputDetailID
   */
  pcbaOutputDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaOutputId: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
   */
  timePeriod: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
   */
  teamCode: string;

  /**
   * 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
   */
  prodEquipCode: string;

  /**
   * 直接人员
   */
  directLabor: number;

  /**
   * 间接人员
   */
  indirectLabor: number;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
   */
  stdMinutes: number;

  /**
   * 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
   */
  stdLaborCapacity: number;

  /**
   * 标准点数（PCBA 专用，按工作中心回填）
   */
  stdShorts: number;

  /**
   * 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
   */
  stdEquipmentCapacity: number;

  /**
   * PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）
   */
  pcbBoardType: string;

  /**
   * 面板别（字典 logistics_pcba_side_category；存 DictValue：b= B面 t= T面）
   */
  panelSide: string;

  /**
   * 批次数量
   */
  batchQty: number;

  /**
   * 当日完成数
   */
  dailyCompletedQty: number;

  /**
   * 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
   */
  totalCompletedQty: number;

  /**
   * 完成状态（计算结果：字典 logistics_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
   */
  completedStatus: number;

  /**
   * 序列号（明细级）
   */
  serialCode: string;

  /**
   * 不良台数
   */
  defectCount: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes: number;

  /**
   * 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 投入工数(分钟)（计算结果：明细 DirectLabor×60）
   */
  inputMinutes: number;

  /**
   * 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
   */
  actualMinutes: number;

  /**
   * 修工数(分钟)
   */
  repairMinutes: number;

  /**
   * 切换次数
   */
  switchCount: number;

  /**
   * 切换时间(分钟)
   */
  switchTime: number;

  /**
   * 切停机时间(分钟)
   */
  stopTime: number;

  /**
   * 总工数(分钟)
   */
  totalMinutes: number;

  /**
   * 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 报工工时(分钟)
   */
  confirmMinutes: number;

  /**
   * 混合生产（0=非混合；N=此生产时段内另有N笔报工）
   */
  mixedProd: number;

  /**
   * 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
   */
  achievementRate: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

