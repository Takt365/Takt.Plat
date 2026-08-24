// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Ecn
// 文件名称：TaktEcnDetail.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：设变明细实体（技术阶段一 ③）；BOM/料号变更行，与主表/附件保存后自动生成通知并初始化各部门执行行
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变明细实体（技术阶段一 ③，隶属 TaktEcGijutsu）。技术维护 BOM/料号变更行；存在明细时保存主表后系统自动生成 TaktEcNotification，
/// 阶段二各部门在 TaktEcSeikan/Mp 等表按明细行（EcnDetailId）填报执行，本实体通过 OneToOne 导航直接关联各课部门执行表。
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_detail", "设变明细表")]
[SugarIndex("ix_ec_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_detail_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_detail_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktEcDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "ec_id", ColumnDescription = "设变ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }


    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// BOM行号（Ec_bom_line_no）
    /// </summary>
    [SugarColumn(ColumnName = "ec_bom_line_code", ColumnDescription = "BOM行号", Length = 8, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcBomLineCode { get; set; }

    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    [SugarColumn(ColumnName = "ec_model", ColumnDescription = "机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcModel { get; set; } = string.Empty;

    /// <summary>
    /// 完成品（Ec_bomitem）
    /// </summary>
    [SugarColumn(ColumnName = "ec_bomitem", ColumnDescription = "完成品", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcBomItem { get; set; }

    /// <summary>
    /// 完成品描述（Ec_bomitemtext）
    /// </summary>
    [SugarColumn(ColumnName = "ec_bomitemtext", ColumnDescription = "完成品描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcBomItemText { get; set; }

    /// <summary>
    /// 上阶物料（Ec_bomsubitem）
    /// </summary>
    [SugarColumn(ColumnName = "ec_bomsubitem", ColumnDescription = "上阶物料", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcBomSubItem { get; set; }

    /// <summary>
    /// 上阶物料描述（冗余：按 EcBomSubItem 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "ec_bomsubitemtext", ColumnDescription = "上阶物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcBomSubItemText { get; set; }

    /// <summary>
    /// 完成品EOL（End of Line，0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "ec_is_end_of_line", ColumnDescription = "完成品EOL", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsEndOfLine { get; set; } = 0;

    /// <summary>
    /// 旧料号（Ec_olditem）
    /// </summary>
    [SugarColumn(ColumnName = "ec_olditem", ColumnDescription = "旧料号", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldItem { get; set; }

    /// <summary>
    /// 旧料号描述（Ec_oldtext）
    /// </summary>
    [SugarColumn(ColumnName = "ec_oldtext", ColumnDescription = "旧料号描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldText { get; set; }

    /// <summary>
    /// 旧用量（Ec_oldusage）
    /// </summary>
    [SugarColumn(ColumnName = "ec_oldusage", ColumnDescription = "旧用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcOldUsage { get; set; }

    /// <summary>
    /// 旧位置（Ec_oldposition）
    /// </summary>
    [SugarColumn(ColumnName = "ec_oldposition", ColumnDescription = "旧位置", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldPosition { get; set; }

    /// <summary>
    /// 旧在库数量（Ec_oldstock）
    /// </summary>
    [SugarColumn(ColumnName = "ec_oldstock", ColumnDescription = "旧在库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（Ec_oldwarehouse）
    /// </summary>
    [SugarColumn(ColumnName = "ec_oldwarehouse", ColumnDescription = "旧品仓库", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldWarehouse { get; set; }

    /// <summary>
    /// 旧品是否采购（0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "ec_is_old_procurement", ColumnDescription = "旧品是否采购", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsOldProcurement { get; set; } = 0;

    /// <summary>
    /// 旧品是否检查（0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "ec_is_old_check", ColumnDescription = "旧品是否检查", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsOldCheck { get; set; } = 0;

    /// <summary>
    /// 新料号（Ec_newitem）
    /// </summary>
    [SugarColumn(ColumnName = "ec_newitem", ColumnDescription = "新料号", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewItem { get; set; }

    /// <summary>
    /// 新料号描述（Ec_newtext）
    /// </summary>
    [SugarColumn(ColumnName = "ec_newtext", ColumnDescription = "新料号描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewText { get; set; }

    /// <summary>
    /// 新用量（Ec_newusage）
    /// </summary>
    [SugarColumn(ColumnName = "ec_newusage", ColumnDescription = "新用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcNewUsage { get; set; }

    /// <summary>
    /// 新位置（Ec_newposition）
    /// </summary>
    [SugarColumn(ColumnName = "ec_newposition", ColumnDescription = "新位置", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewPosition { get; set; }

    /// <summary>
    /// 新在库数量（Ec_newstock）
    /// </summary>
    [SugarColumn(ColumnName = "ec_newstock", ColumnDescription = "新在库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（Ec_newwarehouse）
    /// </summary>
    [SugarColumn(ColumnName = "ec_newwarehouse", ColumnDescription = "新品仓库", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewWarehouse { get; set; }

    /// <summary>
    /// 新品是否采购（0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "ec_is_new_procurement", ColumnDescription = "新品是否采购", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsNewProcurement { get; set; } = 0;

    /// <summary>
    /// 新品是否检查（0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "ec_is_new_check", ColumnDescription = "新品是否检查", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsNewCheck { get; set; } = 0;

    /// <summary>
    /// BOM生效日期（Ec_bomdate）
    /// </summary>
    [SugarColumn(ColumnName = "ec_bomdate", ColumnDescription = "BOM生效日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
    /// </summary>
    [SugarColumn(ColumnName = "ec_is_compatible", ColumnDescription = "兼容性", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcIsCompatible { get; set; }

    /// <summary>
    /// 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    [SugarColumn(ColumnName = "ec_second_distinction", ColumnDescription = "二级区分", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcSecondDistinction { get; set; }

    /// <summary>
    /// 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "ec_instruction", ColumnDescription = "生产指令", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcInstruction { get; set; }

    /// <summary>
    /// 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "ec_legacy_part_disposition", ColumnDescription = "旧品处理", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcLegacyPartDisposition { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变技术课主表（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcId))]
    public TaktEcGijutsu? EcGijutsu { get; set; }

    /// <summary>
    /// 生管课执行行（TaktEcSeikan，每明细一行）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktEcSeikan.EcnDetailId))]
    public TaktEcSeikan? EcSeikan { get; set; }

    /// <summary>
    /// 采购课执行行（TaktEcKoubai，每明细一行）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktEcKoubai.EcnDetailId))]
    public TaktEcKoubai? EcKoubai { get; set; }

    /// <summary>
    /// 受检课执行行（TaktEcUkeken，每明细一行）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktEcUkeken.EcnDetailId))]
    public TaktEcUkeken? EcUkeken { get; set; }

    /// <summary>
    /// 部管课执行行（TaktEcBukan，每明细一行）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktEcBukan.EcnDetailId))]
    public TaktEcBukan? EcBukan { get; set; }

    /// <summary>
    /// 制二课执行行（TaktEcSeizounika，每明细一行）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktEcSeizounika.EcnDetailId))]
    public TaktEcSeizounika? EcSeizounika { get; set; }

    /// <summary>
    /// 制一课执行行（TaktEcSeizouikka，每明细一行）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktEcSeizouikka.EcnDetailId))]
    public TaktEcSeizouikka? EcSeizouikka { get; set; }

    /// <summary>
    /// 品管课执行行（TaktEcHinkan，每明细一行）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktEcHinkan.EcnDetailId))]
    public TaktEcHinkan? EcHinkan { get; set; }

    /// <summary>
    /// 制技课执行行（TaktEcSeizougijutsu，每明细一行）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktEcSeizougijutsu.EcnDetailId))]
    public TaktEcSeizougijutsu? EcSeizougijutsu { get; set; }
}
