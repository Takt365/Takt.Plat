// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcViewDtos.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板/投入批次/物料确认/部门视图/旧品管制 专用 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 部门视图（技术/采购/生管等）共用 DTO
// ========================================

/// <summary>
/// 设变部门视图行（明细 + 部门记录合并展示）
/// </summary>
public class TaktEcDeptViewDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcDeptViewID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDeptViewId { get; set; }

    /// <summary>部门记录 ID；未创建时为空</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }
    /// <summary>设变明细 ID</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>设变主表 ID</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }
    /// <summary>设变单号</summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>明细行号</summary>
    public int LineNumber { get; set; }
    /// <summary>机种（Ec_model）</summary>
    public string EcModel { get; set; } = string.Empty;
    /// <summary>旧料号</summary>
    public string? EcOldItem { get; set; }
    /// <summary>新料号</summary>
    public string? EcNewItem { get; set; }
    /// <summary>旧料号描述</summary>
    public string? EcOldText { get; set; }
    /// <summary>新料号描述</summary>
    public string? EcNewText { get; set; }
    /// <summary>部门编码</summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>是否实施（0=否 1=是）</summary>
    public int IsImplemented { get; set; }
    /// <summary>内容</summary>
    public string? Content { get; set; }
    /// <summary>录入日期</summary>
    public DateTime? EntryDate { get; set; }
    /// <summary>担当（EcLeader）</summary>
    public string? EcLeader { get; set; }
    /// <summary>预计生产日期</summary>
    public DateTime? ScheduledProductionDate { get; set; }
    /// <summary>预定批次</summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>Po残</summary>
    public string? PoRemainder { get; set; }
    /// <summary>结余</summary>
    public string? Balance { get; set; }
    /// <summary>旧品处理</summary>
    public string? OldProductHandling { get; set; }
    /// <summary>采购订单发行日期</summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }
    /// <summary>供应商</summary>
    public string? Supplier { get; set; }
    /// <summary>采购订单号码</summary>
    public string? PurchaseOrderNo { get; set; }
    /// <summary>受检单号</summary>
    public string? IqcOrderNo { get; set; }
    /// <summary>检验日期</summary>
    public DateTime? InspectionDate { get; set; }
    /// <summary>出库批次</summary>
    public string? OutboundBatch { get; set; }
    /// <summary>出库日期</summary>
    public DateTime? OutboundDate { get; set; }
    /// <summary>生产日期</summary>
    public DateTime? ProductionDate { get; set; }
    /// <summary>生产批次</summary>
    public string? ProductionBatch { get; set; }
    /// <summary>出库单号</summary>
    public string? OutboundOrderNo { get; set; }
    /// <summary>生产班组</summary>
    public string? ProductionTeam { get; set; }
    /// <summary>实施日期</summary>
    public DateTime? ImplementationDate { get; set; }
    /// <summary>实施批次</summary>
    public string? ImplementationBatch { get; set; }
    /// <summary>检验批次</summary>
    public string? InspectionBatch { get; set; }
    /// <summary>抽样号码</summary>
    public string? SamplingNo { get; set; }
    /// <summary>确认日期</summary>
    public DateTime? ConfirmationDate { get; set; }
    /// <summary>是否更新 SOP（0=否 1=是）</summary>
    public int IsSopUpdated { get; set; }


    /// <summary>
    /// PurchaseOrderCode
    /// </summary>
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// IqcOrderCode
    /// </summary>
    public string? IqcOrderCode { get; set; }

    /// <summary>
    /// OutboundOrderCode
    /// </summary>
    public string? OutboundOrderCode { get; set; }

    /// <summary>
    /// SamplingCode
    /// </summary>
    public string? SamplingCode { get; set; }
}

/// <summary>
/// 设变部门视图查询 DTO
/// </summary>
public class TaktEcDeptViewQueryDto : TaktPagedQuery
{
    /// <summary>设变单号</summary>
    public string? EcNo { get; set; }
    /// <summary>机种（Ec_model）</summary>
    public string? EcModel { get; set; }
    /// <summary>是否实施</summary>
    public int? IsImplemented { get; set; }
    /// <summary>旧料号</summary>
    public string? EcOldItem { get; set; }
    /// <summary>新料号</summary>
    public string? EcNewItem { get; set; }

    /// <summary>
    /// EcCode
    /// </summary>
    public string? EcCode { get; set; }
}

/// <summary>
/// 设变部门视图更新 DTO
/// </summary>
public class TaktEcDeptViewUpdateDto
{
    /// <summary>设变明细 ID</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>是否实施</summary>
    public int IsImplemented { get; set; }
    /// <summary>内容</summary>
    public string? Content { get; set; }
    /// <summary>录入日期</summary>
    public DateTime? EntryDate { get; set; }
    /// <summary>担当（EcLeader）</summary>
    public string? EcLeader { get; set; }
    /// <summary>预计生产日期</summary>
    public DateTime? ScheduledProductionDate { get; set; }
    /// <summary>预定批次</summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>Po残</summary>
    public string? PoRemainder { get; set; }
    /// <summary>结余</summary>
    public string? Balance { get; set; }
    /// <summary>旧品处理</summary>
    public string? OldProductHandling { get; set; }
    /// <summary>采购订单发行日期</summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }
    /// <summary>供应商</summary>
    public string? Supplier { get; set; }
    /// <summary>采购订单号码</summary>
    public string? PurchaseOrderNo { get; set; }
    /// <summary>受检单号</summary>
    public string? IqcOrderNo { get; set; }
    /// <summary>检验日期</summary>
    public DateTime? InspectionDate { get; set; }
    /// <summary>出库批次</summary>
    public string? OutboundBatch { get; set; }
    /// <summary>出库日期</summary>
    public DateTime? OutboundDate { get; set; }
    /// <summary>生产日期</summary>
    public DateTime? ProductionDate { get; set; }
    /// <summary>生产批次</summary>
    public string? ProductionBatch { get; set; }
    /// <summary>出库单号</summary>
    public string? OutboundOrderNo { get; set; }
    /// <summary>生产班组</summary>
    public string? ProductionTeam { get; set; }
    /// <summary>实施日期</summary>
    public DateTime? ImplementationDate { get; set; }
    /// <summary>实施批次</summary>
    public string? ImplementationBatch { get; set; }
    /// <summary>检验批次</summary>
    public string? InspectionBatch { get; set; }
    /// <summary>抽样号码</summary>
    public string? SamplingNo { get; set; }
    /// <summary>确认日期</summary>
    public DateTime? ConfirmationDate { get; set; }
    /// <summary>是否更新 SOP</summary>
    public int IsSopUpdated { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }


    /// <summary>
    /// PurchaseOrderCode
    /// </summary>
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// IqcOrderCode
    /// </summary>
    public string? IqcOrderCode { get; set; }

    /// <summary>
    /// OutboundOrderCode
    /// </summary>
    public string? OutboundOrderCode { get; set; }

    /// <summary>
    /// SamplingCode
    /// </summary>
    public string? SamplingCode { get; set; }
}

/// <summary>
/// 设变部门视图导入模板 DTO
/// </summary>
public class TaktEcDeptViewTemplateDto
{
    /// <summary>设变明细 ID（与 EcNo+LineNumber 二选一）</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }
    /// <summary>设变单号</summary>
    public string? EcNo { get; set; }
    /// <summary>设变明细行号</summary>
    public int? LineNumber { get; set; }
    /// <summary>是否实施</summary>
    public int? IsImplemented { get; set; }
    /// <summary>内容</summary>
    public string? Content { get; set; }
    /// <summary>录入日期</summary>
    public DateTime? EntryDate { get; set; }
    /// <summary>担当（EcLeader）</summary>
    public string? EcLeader { get; set; }
    /// <summary>预计生产日期</summary>
    public DateTime? ScheduledProductionDate { get; set; }
    /// <summary>预定批次</summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>Po残</summary>
    public string? PoRemainder { get; set; }
    /// <summary>结余</summary>
    public string? Balance { get; set; }
    /// <summary>旧品处理</summary>
    public string? OldProductHandling { get; set; }
    /// <summary>采购订单发行日期</summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }
    /// <summary>供应商</summary>
    public string? Supplier { get; set; }
    /// <summary>采购订单号码</summary>
    public string? PurchaseOrderNo { get; set; }
    /// <summary>受检单号</summary>
    public string? IqcOrderNo { get; set; }
    /// <summary>检验日期</summary>
    public DateTime? InspectionDate { get; set; }
    /// <summary>出库批次</summary>
    public string? OutboundBatch { get; set; }
    /// <summary>出库日期</summary>
    public DateTime? OutboundDate { get; set; }
    /// <summary>生产日期</summary>
    public DateTime? ProductionDate { get; set; }
    /// <summary>生产批次</summary>
    public string? ProductionBatch { get; set; }
    /// <summary>出库单号</summary>
    public string? OutboundOrderNo { get; set; }
    /// <summary>生产班组</summary>
    public string? ProductionTeam { get; set; }
    /// <summary>实施日期</summary>
    public DateTime? ImplementationDate { get; set; }
    /// <summary>实施批次</summary>
    public string? ImplementationBatch { get; set; }
    /// <summary>检验批次</summary>
    public string? InspectionBatch { get; set; }
    /// <summary>抽样号码</summary>
    public string? SamplingNo { get; set; }
    /// <summary>确认日期</summary>
    public DateTime? ConfirmationDate { get; set; }
    /// <summary>是否更新 SOP</summary>
    public int? IsSopUpdated { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 设变部门视图导入 DTO
/// </summary>
public class TaktEcDeptViewImportDto : TaktEcDeptViewTemplateDto
{



    /// <summary>
    /// EcCode
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// PurchaseOrderCode
    /// </summary>
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// IqcOrderCode
    /// </summary>
    public string? IqcOrderCode { get; set; }

    /// <summary>
    /// OutboundOrderCode
    /// </summary>
    public string? OutboundOrderCode { get; set; }

    /// <summary>
    /// SamplingCode
    /// </summary>
    public string? SamplingCode { get; set; }
}

// ========================================
// 设变看板
// ========================================

/// <summary>
/// 设变看板行（按设变单汇总各部门实施进度）
/// </summary>
public class TaktEcKanbanDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcKanbanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcKanbanId { get; set; }

    /// <summary>设变主表 ID</summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }
    /// <summary>设变单号</summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>设变标题</summary>
    public string EcTitle { get; set; } = string.Empty;
    /// <summary>变更状态</summary>
    public int ChangeStatus { get; set; }
    /// <summary>设变状态</summary>
    public int EcStatus { get; set; }
    /// <summary>负责人</summary>
    public string EcLeader { get; set; } = string.Empty;
    /// <summary>明细行数</summary>
    public int DetailCount { get; set; }
    /// <summary>各部门实施汇总</summary>
    public List<TaktEcKanbanDeptStageDto> DeptStages { get; set; } = [];
    /// <summary>当前待实施部门编码（路径上首个未全部完成的部门；字典 logistics_ec_dept_code）</summary>
    public string? CurrentDeptCode { get; set; }
    /// <summary>当前部门待实施明细数</summary>
    public int PendingAtCurrentDeptCount { get; set; }
    /// <summary>实施路径状态（0 未开始 1 实施中 2 正式完成 3 全部完成）</summary>
    public int ImplementationStatus { get; set; }
    /// <summary>品管课是否已全部实施（正式完成）</summary>
    public int IsOfficiallyCompleted { get; set; }
}

/// <summary>
/// 看板部门阶段
/// </summary>
public class TaktEcKanbanDeptStageDto
{
    /// <summary>部门编码</summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>已实施明细数</summary>
    public int ImplementedCount { get; set; }
    /// <summary>明细总数</summary>
    public int TotalCount { get; set; }
}

/// <summary>
/// 设变看板查询 DTO
/// </summary>
public class TaktEcKanbanQueryDto : TaktPagedQuery
{
    /// <summary>设变单号</summary>
    public string? EcNo { get; set; }
    /// <summary>变更状态</summary>
    public int? ChangeStatus { get; set; }
    /// <summary>设变状态</summary>
    public int? EcStatus { get; set; }
    /// <summary>当前待实施部门编码</summary>
    public string? CurrentDeptCode { get; set; }
    /// <summary>实施路径状态（0 未开始 1 实施中 2 正式完成 3 全部完成）</summary>
    public int? ImplementationStatus { get; set; }
    /// <summary>仅未正式完成（品管课未全部实施）</summary>
    public int? OnlyNotOfficiallyCompleted { get; set; }

    /// <summary>
    /// EcCode
    /// </summary>
    public string? EcCode { get; set; }

    /// <summary>
    /// CultureCode
    /// </summary>
    public string? CultureCode { get; set; }
}

// ========================================
// 投入批次
// ========================================

/// <summary>
/// 投入批次视图行
/// </summary>
public class TaktEcBatchDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcBatchID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcBatchId { get; set; }

    /// <summary>设变明细 ID</summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>设变单号</summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>行号</summary>
    public int LineNumber { get; set; }
    /// <summary>机种（Ec_model）</summary>
    public string EcModel { get; set; } = string.Empty;
    /// <summary>新料号</summary>
    public string? EcNewItem { get; set; }
    /// <summary>预定批次（生管）</summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>生产批次（制二）</summary>
    public string? ProductionBatch { get; set; }
    /// <summary>预计生产日期</summary>
    public DateTime? ScheduledProductionDate { get; set; }
    /// <summary>生产日期</summary>
    public DateTime? ProductionDate { get; set; }
}

/// <summary>
/// 投入批次查询 DTO
/// </summary>
public class TaktEcBatchQueryDto : TaktPagedQuery
{
    /// <summary>设变单号</summary>
    public string? EcNo { get; set; }
    /// <summary>批次号（预定/生产批次模糊）</summary>
    public string? BatchNo { get; set; }
    /// <summary>机种（Ec_model）</summary>
    public string? EcModel { get; set; }

    /// <summary>
    /// 批次号（预定/生产批次模糊）
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

/// <summary>
/// 投入批次更新 DTO
/// </summary>
public class TaktEcBatchUpdateDto
{
    /// <summary>设变明细 ID</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>预定批次</summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>生产批次</summary>
    public string? ProductionBatch { get; set; }
    /// <summary>预计生产日期</summary>
    public DateTime? ScheduledProductionDate { get; set; }
    /// <summary>生产日期</summary>
    public DateTime? ProductionDate { get; set; }
}

// ========================================
// 物料确认
// ========================================

/// <summary>
/// 物料确认视图行
/// </summary>
public class TaktEcKakuninDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcKakuninID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcKakuninId { get; set; }

    /// <summary>设变明细 ID</summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>设变单号</summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>行号</summary>
    public int LineNumber { get; set; }
    /// <summary>机种（Ec_model）</summary>
    public string EcModel { get; set; } = string.Empty;
    /// <summary>旧料号</summary>
    public string? EcOldItem { get; set; }
    /// <summary>新料号</summary>
    public string? EcNewItem { get; set; }
    /// <summary>旧品是否采购（0=否 1=是）</summary>
    public int IsOldProcurement { get; set; }
    /// <summary>旧品是否检查（0=否 1=是）</summary>
    public int IsOldCheck { get; set; }
    /// <summary>新品是否采购（0=否 1=是）</summary>
    public int IsNewProcurement { get; set; }
    /// <summary>新品是否检查（0=否 1=是）</summary>
    public int IsNewCheck { get; set; }
}

/// <summary>
/// 物料确认查询 DTO
/// </summary>
public class TaktEcKakuninQueryDto : TaktPagedQuery
{
    /// <summary>设变单号</summary>
    public string? EcNo { get; set; }
    /// <summary>旧品是否检查</summary>
    public int? IsOldCheck { get; set; }
    /// <summary>新品是否检查</summary>
    public int? IsNewCheck { get; set; }
    /// <summary>机种（Ec_model）</summary>
    public string? EcModel { get; set; }
    /// <summary>新料号</summary>
    public string? EcNewItem { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

/// <summary>
/// 物料确认更新 DTO
/// </summary>
public class TaktEcKakuninUpdateDto
{
    /// <summary>设变明细 ID</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>旧品是否采购（0=否 1=是）</summary>
    public int IsOldProcurement { get; set; }
    /// <summary>旧品是否检查（0=否 1=是）</summary>
    public int IsOldCheck { get; set; }
    /// <summary>新品是否采购（0=否 1=是）</summary>
    public int IsNewProcurement { get; set; }
    /// <summary>新品是否检查（0=否 1=是）</summary>
    public int IsNewCheck { get; set; }
}

// ========================================
// 旧品管制
// ========================================

/// <summary>
/// 旧品管制视图行
/// </summary>
public class TaktEcLegacyProductDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcLegacyProductID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcLegacyProductId { get; set; }

    /// <summary>设变明细 ID</summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>设变单号</summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>行号</summary>
    public int LineNumber { get; set; }
    /// <summary>机种（Ec_model）</summary>
    public string EcModel { get; set; } = string.Empty;
    /// <summary>旧料号</summary>
    public string? EcOldItem { get; set; }
    /// <summary>旧料号描述</summary>
    public string? EcOldText { get; set; }
    /// <summary>旧用量</summary>
    public decimal? EcOldUsage { get; set; }
    /// <summary>新料号</summary>
    public string? EcNewItem { get; set; }
    /// <summary>旧品处理（生管部门字段）</summary>
    public string? OldProductHandling { get; set; }
    /// <summary>停产状态（EOL，0=否 1=是，字典 logistics_material_eol_status）</summary>
    public int IsEndOfLine { get; set; }
}

/// <summary>
/// 旧品管制查询 DTO
/// </summary>
public class TaktEcLegacyProductQueryDto : TaktPagedQuery
{
    /// <summary>设变单号</summary>
    public string? EcNo { get; set; }
    /// <summary>旧料号</summary>
    public string? EcOldItem { get; set; }
    /// <summary>机种（Ec_model）</summary>
    public string? EcModel { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

/// <summary>
/// 旧品管制更新 DTO
/// </summary>
public class TaktEcLegacyProductUpdateDto
{
    /// <summary>设变明细 ID</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>旧品处理</summary>
    public string? OldProductHandling { get; set; }
    /// <summary>停产状态（EOL，0=否 1=是，字典 logistics_material_eol_status）</summary>
    public int IsEndOfLine { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
}
