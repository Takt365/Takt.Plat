// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：EcDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// EcDetail 响应 DTO
// ========================================

/// <summary>
/// 设变（ECN）子表实体
/// 对应前端 TaktEcDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变主表名称（填充字段）
    /// </summary>
    public string? EcName { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 型号（Ec_model）
    /// </summary>
    public string EcModel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 主项料号（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 子项料号（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 编号（Ec_bomno）
    /// </summary>
    public string? EcBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 变更内容（Ec_change）
    /// </summary>
    public string? EcChange { get; set; } = string.Empty;

    /// <summary>
    /// 本地/现场（Ec_local）
    /// </summary>
    public string? EcLocal { get; set; } = string.Empty;

    /// <summary>
    /// 备注（Ec_note）
    /// </summary>
    public string? EcNote { get; set; } = string.Empty;

    /// <summary>
    /// 工序（Ec_process）
    /// </summary>
    public string? EcProcess { get; set; } = string.Empty;

    /// <summary>
    /// BOM 日期（Ec_bomdate）
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 录入日期（Ec_entrydate）
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧数量（Ec_oldqty）
    /// </summary>
    public decimal? EcOldQty { get; set; }

    /// <summary>
    /// 旧单位/设置（Ec_oldset）
    /// </summary>
    public string? EcOldSet { get; set; } = string.Empty;

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新数量（Ec_newqty）
    /// </summary>
    public decimal? EcNewQty { get; set; }

    /// <summary>
    /// 新单位/设置（Ec_newset）
    /// </summary>
    public string? EcNewSet { get; set; } = string.Empty;

    /// <summary>
    /// 是否采购（0=否 1=是）
    /// </summary>
    public int IsProcurement { get; set; } = 0;

    /// <summary>
    /// 是否检查（0=否 1=是）
    /// </summary>
    public int IsCheck { get; set; } = 0;

    /// <summary>
    /// 仓库（Ec_warehouse）
    /// </summary>
    public string? EcWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// EOL（End of Line，0=否 1=是）
    /// </summary>
    public int IsEndOfLine { get; set; } = 0;

    /// <summary>
    /// 设变主表
    /// （主表：TaktEc）
    /// </summary>
    public TaktEcDto? Ec { get; set; }

    /// <summary>
    /// 设变明细-部门记录列表（按 DeptCode 区分部门：Assy/It/Cus/Fins/Gas/Iqc/Mc/Mp/Pcba/Pmc/Qa/Te/Eng）
    /// （子表：TaktEcDept）
    /// </summary>
    public List<TaktEcDeptDto>? DeptRecords { get; set; }

}

// ========================================
// EcDetail 查询 DTO
// ========================================

/// <summary>
/// EcDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcDetailQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 型号（Ec_model）
    /// </summary>
    public string? EcModel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 主项料号（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 子项料号（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 编号（Ec_bomno）
    /// </summary>
    public string? EcBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 变更内容（Ec_change）
    /// </summary>
    public string? EcChange { get; set; } = string.Empty;

    /// <summary>
    /// 本地/现场（Ec_local）
    /// </summary>
    public string? EcLocal { get; set; } = string.Empty;

    /// <summary>
    /// 备注（Ec_note）
    /// </summary>
    public string? EcNote { get; set; } = string.Empty;

    /// <summary>
    /// 工序（Ec_process）
    /// </summary>
    public string? EcProcess { get; set; } = string.Empty;

    /// <summary>
    /// BOM 日期（Ec_bomdate）（范围查询-开始）
    /// </summary>
    public DateTime? EcBomDateStart { get; set; }

    /// <summary>
    /// BOM 日期（Ec_bomdate）（范围查询-结束）
    /// </summary>
    public DateTime? EcBomDateEnd { get; set; }

    /// <summary>
    /// 录入日期（Ec_entrydate）（范围查询-开始）
    /// </summary>
    public DateTime? EcEntryDateStart { get; set; }

    /// <summary>
    /// 录入日期（Ec_entrydate）（范围查询-结束）
    /// </summary>
    public DateTime? EcEntryDateEnd { get; set; }

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧数量（Ec_oldqty）
    /// </summary>
    public decimal? EcOldQty { get; set; }

    /// <summary>
    /// 旧单位/设置（Ec_oldset）
    /// </summary>
    public string? EcOldSet { get; set; } = string.Empty;

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新数量（Ec_newqty）
    /// </summary>
    public decimal? EcNewQty { get; set; }

    /// <summary>
    /// 新单位/设置（Ec_newset）
    /// </summary>
    public string? EcNewSet { get; set; } = string.Empty;

    /// <summary>
    /// 是否采购（0=否 1=是）
    /// </summary>
    public int? IsProcurement { get; set; }

    /// <summary>
    /// 是否检查（0=否 1=是）
    /// </summary>
    public int? IsCheck { get; set; }

    /// <summary>
    /// 仓库（Ec_warehouse）
    /// </summary>
    public string? EcWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// EOL（End of Line，0=否 1=是）
    /// </summary>
    public int? IsEndOfLine { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建EcDetail DTO
// ========================================

/// <summary>
/// 创建EcDetail DTO
/// </summary>
public class TaktEcDetailCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余字段,便于查询）不能为空")]
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 型号（Ec_model）
    /// </summary>
    [Required(ErrorMessage = "型号（Ec_model）不能为空")]
    public string EcModel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 主项料号（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 子项料号（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 编号（Ec_bomno）
    /// </summary>
    public string? EcBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 变更内容（Ec_change）
    /// </summary>
    public string? EcChange { get; set; } = string.Empty;

    /// <summary>
    /// 本地/现场（Ec_local）
    /// </summary>
    public string? EcLocal { get; set; } = string.Empty;

    /// <summary>
    /// 备注（Ec_note）
    /// </summary>
    public string? EcNote { get; set; } = string.Empty;

    /// <summary>
    /// 工序（Ec_process）
    /// </summary>
    public string? EcProcess { get; set; } = string.Empty;

    /// <summary>
    /// BOM 日期（Ec_bomdate）
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 录入日期（Ec_entrydate）
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧数量（Ec_oldqty）
    /// </summary>
    public decimal? EcOldQty { get; set; }

    /// <summary>
    /// 旧单位/设置（Ec_oldset）
    /// </summary>
    public string? EcOldSet { get; set; } = string.Empty;

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新数量（Ec_newqty）
    /// </summary>
    public decimal? EcNewQty { get; set; }

    /// <summary>
    /// 新单位/设置（Ec_newset）
    /// </summary>
    public string? EcNewSet { get; set; } = string.Empty;

    /// <summary>
    /// 是否采购（0=否 1=是）
    /// </summary>
    public int IsProcurement { get; set; } = 0;

    /// <summary>
    /// 是否检查（0=否 1=是）
    /// </summary>
    public int IsCheck { get; set; } = 0;

    /// <summary>
    /// 仓库（Ec_warehouse）
    /// </summary>
    public string? EcWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// EOL（End of Line，0=否 1=是）
    /// </summary>
    public int IsEndOfLine { get; set; } = 0;

    /// <summary>
    /// 设变明细-部门记录列表（按 DeptCode 区分部门：Assy/It/Cus/Fins/Gas/Iqc/Mc/Mp/Pcba/Pmc/Qa/Te/Eng）（子表，级联保存）
    /// </summary>
    public List<TaktEcDeptCreateDto>? DeptRecords { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新EcDetail DTO
// ========================================

/// <summary>
/// 更新EcDetail DTO
/// 继承 TaktEcDetailCreateDto，添加 EcDetailId 字段
/// </summary>
public class TaktEcDetailUpdateDto : TaktEcDetailCreateDto
{
    /// <summary>
    /// EcDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcDetail 导入模板行 DTO
/// </summary>
public class TaktEcDetailTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 型号（Ec_model）
    /// </summary>
    public string? EcModel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 主项料号（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 子项料号（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 编号（Ec_bomno）
    /// </summary>
    public string? EcBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 变更内容（Ec_change）
    /// </summary>
    public string? EcChange { get; set; } = string.Empty;

    /// <summary>
    /// 本地/现场（Ec_local）
    /// </summary>
    public string? EcLocal { get; set; } = string.Empty;

    /// <summary>
    /// 备注（Ec_note）
    /// </summary>
    public string? EcNote { get; set; } = string.Empty;

    /// <summary>
    /// 工序（Ec_process）
    /// </summary>
    public string? EcProcess { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// EcDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcDetailImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 型号（Ec_model）
    /// </summary>
    public string? EcModel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 主项料号（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 子项料号（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 编号（Ec_bomno）
    /// </summary>
    public string? EcBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 变更内容（Ec_change）
    /// </summary>
    public string? EcChange { get; set; } = string.Empty;

    /// <summary>
    /// 本地/现场（Ec_local）
    /// </summary>
    public string? EcLocal { get; set; } = string.Empty;

    /// <summary>
    /// 备注（Ec_note）
    /// </summary>
    public string? EcNote { get; set; } = string.Empty;

    /// <summary>
    /// 工序（Ec_process）
    /// </summary>
    public string? EcProcess { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// EcDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcDetailExportDto
{
    /// <summary>
    /// EcDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 型号（Ec_model）
    /// </summary>
    public string EcModel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 主项料号（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 子项料号（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// BOM 编号（Ec_bomno）
    /// </summary>
    public string? EcBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 变更内容（Ec_change）
    /// </summary>
    public string? EcChange { get; set; } = string.Empty;

    /// <summary>
    /// 本地/现场（Ec_local）
    /// </summary>
    public string? EcLocal { get; set; } = string.Empty;

    /// <summary>
    /// 备注（Ec_note）
    /// </summary>
    public string? EcNote { get; set; } = string.Empty;

    /// <summary>
    /// 工序（Ec_process）
    /// </summary>
    public string? EcProcess { get; set; } = string.Empty;

    /// <summary>
    /// BOM 日期（Ec_bomdate）
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 录入日期（Ec_entrydate）
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧数量（Ec_oldqty）
    /// </summary>
    public decimal? EcOldQty { get; set; }

    /// <summary>
    /// 旧单位/设置（Ec_oldset）
    /// </summary>
    public string? EcOldSet { get; set; } = string.Empty;

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新数量（Ec_newqty）
    /// </summary>
    public decimal? EcNewQty { get; set; }

    /// <summary>
    /// 新单位/设置（Ec_newset）
    /// </summary>
    public string? EcNewSet { get; set; } = string.Empty;

    /// <summary>
    /// 是否采购（0=否 1=是）
    /// </summary>
    public int IsProcurement { get; set; } = 0;

    /// <summary>
    /// 是否检查（0=否 1=是）
    /// </summary>
    public int IsCheck { get; set; } = 0;

    /// <summary>
    /// 仓库（Ec_warehouse）
    /// </summary>
    public string? EcWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// EOL（End of Line，0=否 1=是）
    /// </summary>
    public int IsEndOfLine { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
