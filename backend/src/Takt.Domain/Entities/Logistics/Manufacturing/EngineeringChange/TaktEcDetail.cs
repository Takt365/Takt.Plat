// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetail.cs
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
    /// BOM行号
    /// </summary>
    [SugarColumn(ColumnName = "ec_bom_line_code", ColumnDescription = "BOM行号", Length = 8, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcBomLineCode { get; set; }

    /// <summary>
    /// 机种编码
    /// </summary>
    [SugarColumn(ColumnName = "ec_model_code", ColumnDescription = "机种编码", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 完成品
    /// </summary>
    [SugarColumn(ColumnName = "ec_finished_goods", ColumnDescription = "完成品", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcFinishedGoods { get; set; }

    /// <summary>
    /// 完成品描述
    /// </summary>
    [SugarColumn(ColumnName = "ec_finished_goods_description", ColumnDescription = "完成品描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcFinishedGoodsDescription { get; set; }

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    [SugarColumn(ColumnName = "ec_parent_material_code", ColumnDescription = "上阶物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcParentMaterialCode { get; set; }

    /// <summary>
    /// 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
    /// </summary>
    [SugarColumn(ColumnName = "ec_parent_material_description", ColumnDescription = "上阶物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcParentMaterialDescription { get; set; }

    /// <summary>
    /// 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    [SugarColumn(ColumnName = "discontinued_status", ColumnDescription = "完成品物料状态", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "Z0")]
    public string DiscontinuedStatus { get; set; } = "Z0";

    /// <summary>
    /// 旧物料编码
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_material_code", ColumnDescription = "旧物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldMaterialCode { get; set; }

    /// <summary>
    /// 旧物料描述
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_material_description", ColumnDescription = "旧物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldMaterialDescription { get; set; }

    /// <summary>
    /// 旧用量
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_usage_quantity", ColumnDescription = "旧用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧位置
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_item_position", ColumnDescription = "旧位置", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldItemPosition { get; set; }

    /// <summary>
    /// 旧在库数量
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_stock", ColumnDescription = "旧在库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_warehouse", ColumnDescription = "旧品仓库", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldWarehouse { get; set; }

    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_purchase_type", ColumnDescription = "旧采购类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EcOldPurchaseType { get; set; }

    /// <summary>
    /// 旧品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_requires_inspection", ColumnDescription = "旧品是否需检验", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EcOldRequiresInspection { get; set; } = 0;

    /// <summary>
    /// 新物料编码
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_material_code", ColumnDescription = "新物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewMaterialCode { get; set; }

    /// <summary>
    /// 新物料描述
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_material_description", ColumnDescription = "新物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewMaterialDescription { get; set; }

    /// <summary>
    /// 新用量
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_usage_quantity", ColumnDescription = "新用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcNewUsageQuantity { get; set; }

    /// <summary>
    /// 新位置
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_item_position", ColumnDescription = "新位置", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewItemPosition { get; set; }

    /// <summary>
    /// 新在库数量
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_stock", ColumnDescription = "新在库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_warehouse", ColumnDescription = "新品仓库", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewWarehouse { get; set; }

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_purchase_type", ColumnDescription = "新采购类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EcNewPurchaseType { get; set; }

    /// <summary>
    /// 新品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_requires_inspection", ColumnDescription = "新品是否需检验", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EcNewRequiresInspection { get; set; } = 0;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    [SugarColumn(ColumnName = "ec_bomdate", ColumnDescription = "BOM生效日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    [SugarColumn(ColumnName = "ec_is_compatible", ColumnDescription = "兼容性", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcIsCompatible { get; set; }

    /// <summary>
    /// 二级区分（字典 logistics_manufacturing_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    [SugarColumn(ColumnName = "ec_second_distinction", ColumnDescription = "二级区分", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcSecondDistinction { get; set; }

    /// <summary>
    /// 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "ec_instruction", ColumnDescription = "生产指令", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcInstruction { get; set; }

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_part_disposition", ColumnDescription = "旧品处理", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldPartDisposition { get; set; }

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
