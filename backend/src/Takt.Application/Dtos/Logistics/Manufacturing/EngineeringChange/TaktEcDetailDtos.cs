// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailDtos.cs
// 创建时间：2026-08-11
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
/// 设变明细实体（技术阶段一 ③，隶属 TaktEcGijutsu）。技术维护 BOM/料号变更行；存在明细时保存主表后系统自动生成 TaktEcNotification， 阶段二各部门在 TaktEcSeikan/Mp 等表按明细行（EcnDetailId）填报执行，本实体通过 OneToOne 导航直接关联各课部门执行表。
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
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// BOM行号（Ec_bom_line_no）
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string EcModel { get; set; } = string.Empty;

    /// <summary>
    /// 完成品（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// 完成品描述（Ec_bomitemtext）
    /// </summary>
    public string? EcBomItemText { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（Ec_bomsubitemtext）
    /// </summary>
    public string? EcBomSubItemText { get; set; } = string.Empty;

    /// <summary>
    /// 完成品EOL（End of Line，0=否 1=是）
    /// </summary>
    public int IsEndOfLine { get; set; } = 0;

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量（Ec_oldusage）
    /// </summary>
    public decimal? EcOldUsage { get; set; }

    /// <summary>
    /// 旧位置（Ec_oldposition）
    /// </summary>
    public string? EcOldPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量（Ec_oldstock）
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（Ec_oldwarehouse）
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否采购（0=否 1=是）
    /// </summary>
    public int IsOldProcurement { get; set; } = 0;

    /// <summary>
    /// 旧品是否检查（0=否 1=是）
    /// </summary>
    public int IsOldCheck { get; set; } = 0;

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新用量（Ec_newusage）
    /// </summary>
    public decimal? EcNewUsage { get; set; }

    /// <summary>
    /// 新位置（Ec_newposition）
    /// </summary>
    public string? EcNewPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量（Ec_newstock）
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（Ec_newwarehouse）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否采购（0=否 1=是）
    /// </summary>
    public int IsNewProcurement { get; set; } = 0;

    /// <summary>
    /// 新品是否检查（0=否 1=是）
    /// </summary>
    public int IsNewCheck { get; set; } = 0;

    /// <summary>
    /// BOM生效日期（Ec_bomdate）
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变技术课主表（多对一）
    /// （主表：TaktEcGijutsu）
    /// </summary>
    public TaktEcGijutsuDto? EcGijutsu { get; set; }

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
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// BOM行号（Ec_bom_line_no）
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string? EcModel { get; set; } = string.Empty;

    /// <summary>
    /// 完成品（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// 完成品描述（Ec_bomitemtext）
    /// </summary>
    public string? EcBomItemText { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（Ec_bomsubitemtext）
    /// </summary>
    public string? EcBomSubItemText { get; set; } = string.Empty;

    /// <summary>
    /// 完成品EOL（End of Line，0=否 1=是）
    /// </summary>
    public int? IsEndOfLine { get; set; }

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量（Ec_oldusage）
    /// </summary>
    public decimal? EcOldUsage { get; set; }

    /// <summary>
    /// 旧位置（Ec_oldposition）
    /// </summary>
    public string? EcOldPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量（Ec_oldstock）
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（Ec_oldwarehouse）
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否采购（0=否 1=是）
    /// </summary>
    public int? IsOldProcurement { get; set; }

    /// <summary>
    /// 旧品是否检查（0=否 1=是）
    /// </summary>
    public int? IsOldCheck { get; set; }

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新用量（Ec_newusage）
    /// </summary>
    public decimal? EcNewUsage { get; set; }

    /// <summary>
    /// 新位置（Ec_newposition）
    /// </summary>
    public string? EcNewPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量（Ec_newstock）
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（Ec_newwarehouse）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否采购（0=否 1=是）
    /// </summary>
    public int? IsNewProcurement { get; set; }

    /// <summary>
    /// 新品是否检查（0=否 1=是）
    /// </summary>
    public int? IsNewCheck { get; set; }

    /// <summary>
    /// BOM生效日期（Ec_bomdate）（范围查询-开始）
    /// </summary>
    public DateTime? EcBomDateStart { get; set; }

    /// <summary>
    /// BOM生效日期（Ec_bomdate）（范围查询-结束）
    /// </summary>
    public DateTime? EcBomDateEnd { get; set; }

    /// <summary>
    /// 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
    public string? ExtField { get; set; }

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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余字段,便于查询）不能为空")]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// BOM行号（Ec_bom_line_no）
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    [Required(ErrorMessage = "机种（Ec_model）不能为空")]
    public string EcModel { get; set; } = string.Empty;

    /// <summary>
    /// 完成品（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// 完成品描述（Ec_bomitemtext）
    /// </summary>
    public string? EcBomItemText { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（Ec_bomsubitemtext）
    /// </summary>
    public string? EcBomSubItemText { get; set; } = string.Empty;

    /// <summary>
    /// 完成品EOL（End of Line，0=否 1=是）
    /// </summary>
    public int IsEndOfLine { get; set; } = 0;

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量（Ec_oldusage）
    /// </summary>
    public decimal? EcOldUsage { get; set; }

    /// <summary>
    /// 旧位置（Ec_oldposition）
    /// </summary>
    public string? EcOldPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量（Ec_oldstock）
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（Ec_oldwarehouse）
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否采购（0=否 1=是）
    /// </summary>
    public int IsOldProcurement { get; set; } = 0;

    /// <summary>
    /// 旧品是否检查（0=否 1=是）
    /// </summary>
    public int IsOldCheck { get; set; } = 0;

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新用量（Ec_newusage）
    /// </summary>
    public decimal? EcNewUsage { get; set; }

    /// <summary>
    /// 新位置（Ec_newposition）
    /// </summary>
    public string? EcNewPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量（Ec_newstock）
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（Ec_newwarehouse）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否采购（0=否 1=是）
    /// </summary>
    public int IsNewProcurement { get; set; } = 0;

    /// <summary>
    /// 新品是否检查（0=否 1=是）
    /// </summary>
    public int IsNewCheck { get; set; } = 0;

    /// <summary>
    /// BOM生效日期（Ec_bomdate）
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

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
// EcDetail 作废 DTO
// ========================================

/// <summary>
/// EcDetail 作废/撤销作废 DTO
/// </summary>
public class TaktEcDetailObsoleteDto
{
    /// <summary>
    /// EcDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// BOM行号（Ec_bom_line_no）
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string? EcModel { get; set; } = string.Empty;

    /// <summary>
    /// 完成品（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// 完成品描述（Ec_bomitemtext）
    /// </summary>
    public string? EcBomItemText { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（Ec_bomsubitemtext）
    /// </summary>
    public string? EcBomSubItemText { get; set; } = string.Empty;

    /// <summary>
    /// 完成品EOL（End of Line，0=否 1=是）
    /// </summary>
    public int? IsEndOfLine { get; set; }

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量（Ec_oldusage）
    /// </summary>
    public decimal? EcOldUsage { get; set; }

    /// <summary>
    /// 旧位置（Ec_oldposition）
    /// </summary>
    public string? EcOldPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量（Ec_oldstock）
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（Ec_oldwarehouse）
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否采购（0=否 1=是）
    /// </summary>
    public int? IsOldProcurement { get; set; }

    /// <summary>
    /// 旧品是否检查（0=否 1=是）
    /// </summary>
    public int? IsOldCheck { get; set; }

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新用量（Ec_newusage）
    /// </summary>
    public decimal? EcNewUsage { get; set; }

    /// <summary>
    /// 新位置（Ec_newposition）
    /// </summary>
    public string? EcNewPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量（Ec_newstock）
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（Ec_newwarehouse）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否采购（0=否 1=是）
    /// </summary>
    public int? IsNewProcurement { get; set; }

    /// <summary>
    /// 新品是否检查（0=否 1=是）
    /// </summary>
    public int? IsNewCheck { get; set; }

    /// <summary>
    /// BOM生效日期（Ec_bomdate）
    /// </summary>
    public DateTime? EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// BOM行号（Ec_bom_line_no）
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string? EcModel { get; set; } = string.Empty;

    /// <summary>
    /// 完成品（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// 完成品描述（Ec_bomitemtext）
    /// </summary>
    public string? EcBomItemText { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（Ec_bomsubitemtext）
    /// </summary>
    public string? EcBomSubItemText { get; set; } = string.Empty;

    /// <summary>
    /// 完成品EOL（End of Line，0=否 1=是）
    /// </summary>
    public int? IsEndOfLine { get; set; }

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量（Ec_oldusage）
    /// </summary>
    public decimal? EcOldUsage { get; set; }

    /// <summary>
    /// 旧位置（Ec_oldposition）
    /// </summary>
    public string? EcOldPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量（Ec_oldstock）
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（Ec_oldwarehouse）
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否采购（0=否 1=是）
    /// </summary>
    public int? IsOldProcurement { get; set; }

    /// <summary>
    /// 旧品是否检查（0=否 1=是）
    /// </summary>
    public int? IsOldCheck { get; set; }

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新用量（Ec_newusage）
    /// </summary>
    public decimal? EcNewUsage { get; set; }

    /// <summary>
    /// 新位置（Ec_newposition）
    /// </summary>
    public string? EcNewPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量（Ec_newstock）
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（Ec_newwarehouse）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否采购（0=否 1=是）
    /// </summary>
    public int? IsNewProcurement { get; set; }

    /// <summary>
    /// 新品是否检查（0=否 1=是）
    /// </summary>
    public int? IsNewCheck { get; set; }

    /// <summary>
    /// BOM生效日期（Ec_bomdate）
    /// </summary>
    public DateTime? EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

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
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// BOM行号（Ec_bom_line_no）
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string EcModel { get; set; } = string.Empty;

    /// <summary>
    /// 完成品（Ec_bomitem）
    /// </summary>
    public string? EcBomItem { get; set; } = string.Empty;

    /// <summary>
    /// 完成品描述（Ec_bomitemtext）
    /// </summary>
    public string? EcBomItemText { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料（Ec_bomsubitem）
    /// </summary>
    public string? EcBomSubItem { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（Ec_bomsubitemtext）
    /// </summary>
    public string? EcBomSubItemText { get; set; } = string.Empty;

    /// <summary>
    /// 完成品EOL（End of Line，0=否 1=是）
    /// </summary>
    public int IsEndOfLine { get; set; } = 0;

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    public string? EcOldItem { get; set; } = string.Empty;

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    public string? EcOldText { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量（Ec_oldusage）
    /// </summary>
    public decimal? EcOldUsage { get; set; }

    /// <summary>
    /// 旧位置（Ec_oldposition）
    /// </summary>
    public string? EcOldPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量（Ec_oldstock）
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（Ec_oldwarehouse）
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否采购（0=否 1=是）
    /// </summary>
    public int IsOldProcurement { get; set; } = 0;

    /// <summary>
    /// 旧品是否检查（0=否 1=是）
    /// </summary>
    public int IsOldCheck { get; set; } = 0;

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    public string? EcNewItem { get; set; } = string.Empty;

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    public string? EcNewText { get; set; } = string.Empty;

    /// <summary>
    /// 新用量（Ec_newusage）
    /// </summary>
    public decimal? EcNewUsage { get; set; }

    /// <summary>
    /// 新位置（Ec_newposition）
    /// </summary>
    public string? EcNewPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量（Ec_newstock）
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（Ec_newwarehouse）
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否采购（0=否 1=是）
    /// </summary>
    public int IsNewProcurement { get; set; } = 0;

    /// <summary>
    /// 新品是否检查（0=否 1=是）
    /// </summary>
    public int IsNewCheck { get; set; } = 0;

    /// <summary>
    /// BOM生效日期（Ec_bomdate）
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
