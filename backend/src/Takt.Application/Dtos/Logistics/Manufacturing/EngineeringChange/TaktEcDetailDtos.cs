// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailDtos.cs
// 创建时间：2026-08-26
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
    /// BOM行号
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料编码
    /// </summary>
    public string? EcFinishedGoods { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料描述
    /// </summary>
    public string? EcFinishedGoodsDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = "Z0";

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? EcOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量
    /// </summary>
    public decimal? EcOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧位置
    /// </summary>
    public string? EcOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcOldPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EcOldRequiresInspection { get; set; } = 0;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新用量
    /// </summary>
    public decimal? EcNewUsageQuantity { get; set; }

    /// <summary>
    /// 新位置
    /// </summary>
    public string? EcNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EcNewRequiresInspection { get; set; } = 0;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_manufacturing_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
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
    /// BOM行号
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料编码
    /// </summary>
    public string? EcFinishedGoods { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料描述
    /// </summary>
    public string? EcFinishedGoodsDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? DiscontinuedStatus { get; set; }

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? EcOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量
    /// </summary>
    public decimal? EcOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧位置
    /// </summary>
    public string? EcOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcOldPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EcOldRequiresInspection { get; set; }

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新用量
    /// </summary>
    public decimal? EcNewUsageQuantity { get; set; }

    /// <summary>
    /// 新位置
    /// </summary>
    public string? EcNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EcNewRequiresInspection { get; set; }

    /// <summary>
    /// BOM生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EcBomDateStart { get; set; }

    /// <summary>
    /// BOM生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EcBomDateEnd { get; set; }

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_manufacturing_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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

    /// <summary>
    /// 制二课主表页签（1=采购 F 且仓库 C003 2=其它；仅 TaktEcSeizounikas/masters 使用）
    /// </summary>
    public int? PcbaTab { get; set; }
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
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
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
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// BOM行号
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    [Required(ErrorMessage = "机种不能为空")]
    public string EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料编码
    /// </summary>
    public string? EcFinishedGoods { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料描述
    /// </summary>
    public string? EcFinishedGoodsDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = "Z0";

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? EcOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量
    /// </summary>
    public decimal? EcOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧位置
    /// </summary>
    public string? EcOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcOldPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EcOldRequiresInspection { get; set; } = 0;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新用量
    /// </summary>
    public decimal? EcNewUsageQuantity { get; set; }

    /// <summary>
    /// 新位置
    /// </summary>
    public string? EcNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EcNewRequiresInspection { get; set; } = 0;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_manufacturing_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
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
    /// BOM行号
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料编码
    /// </summary>
    public string? EcFinishedGoods { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料描述
    /// </summary>
    public string? EcFinishedGoodsDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? DiscontinuedStatus { get; set; }

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? EcOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量
    /// </summary>
    public decimal? EcOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧位置
    /// </summary>
    public string? EcOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcOldPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EcOldRequiresInspection { get; set; }

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新用量
    /// </summary>
    public decimal? EcNewUsageQuantity { get; set; }

    /// <summary>
    /// 新位置
    /// </summary>
    public string? EcNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EcNewRequiresInspection { get; set; }

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_manufacturing_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
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
    /// BOM行号
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料编码
    /// </summary>
    public string? EcFinishedGoods { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料描述
    /// </summary>
    public string? EcFinishedGoodsDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? DiscontinuedStatus { get; set; }

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? EcOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量
    /// </summary>
    public decimal? EcOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧位置
    /// </summary>
    public string? EcOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcOldPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EcOldRequiresInspection { get; set; }

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新用量
    /// </summary>
    public decimal? EcNewUsageQuantity { get; set; }

    /// <summary>
    /// 新位置
    /// </summary>
    public string? EcNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EcNewRequiresInspection { get; set; }

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_manufacturing_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

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
    /// BOM行号
    /// </summary>
    public string? EcBomLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料编码
    /// </summary>
    public string? EcFinishedGoods { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料描述
    /// </summary>
    public string? EcFinishedGoodsDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    public string? EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
    /// </summary>
    public string? EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = "Z0";

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? EcOldMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧用量
    /// </summary>
    public decimal? EcOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧位置
    /// </summary>
    public string? EcOldItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 旧在库数量
    /// </summary>
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库
    /// </summary>
    public string? EcOldWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcOldPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 旧品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EcOldRequiresInspection { get; set; } = 0;

    /// <summary>
    /// 新物料编码
    /// </summary>
    public string? EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新用量
    /// </summary>
    public decimal? EcNewUsageQuantity { get; set; }

    /// <summary>
    /// 新位置
    /// </summary>
    public string? EcNewItemPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新在库数量
    /// </summary>
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库
    /// </summary>
    public string? EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
    /// </summary>
    public string? EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品是否需检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EcNewRequiresInspection { get; set; } = 0;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? EcIsCompatible { get; set; } = string.Empty;

    /// <summary>
    /// 二级区分（字典 logistics_manufacturing_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    public string? EcSecondDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
