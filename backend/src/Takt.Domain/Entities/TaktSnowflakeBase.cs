// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktSnowflakeBase.cs
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：雪花 bigint 主键 × 三种隔离；CodeFirst：Id=-2、RelatedPlant|PlantCode=-1（先于业务列默认 0，再先于 Scope≥100）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities;

/// <summary>
/// 租户级实体基类（雪花主键）
/// CodeFirst 列序：id → related_plant → 业务列 → tenant_code → culture_code …
/// </summary>
public abstract class TaktTenantEntityBase : TaktTenantEntityScopeBase
{
    /// <summary>
    /// 主键 ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "varchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string RelatedPlant { get; set; } = string.Empty;
}

/// <summary>
/// 公司级实体基类（雪花主键）
/// CodeFirst 列序：id → plant_code → 业务列 → tenant_code …
/// </summary>
public abstract class TaktCompanyEntityBase : TaktCompanyEntityScopeBase
{
    /// <summary>
    /// 主键 ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string PlantCode { get; set; } = string.Empty;
}

/// <summary>
/// 审批级实体基类（雪花主键）
/// CodeFirst 列序：id → plant_code → 业务列 → tenant_code …
/// </summary>
public abstract class TaktApprovalEntityBase : TaktApprovalEntityScopeBase
{
    /// <summary>
    /// 主键 ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string PlantCode { get; set; } = string.Empty;
}
