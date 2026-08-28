// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktSetting.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：系统设置实体，存储系统配置参数
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 系统设置实体
/// 存储系统的各种配置参数，支持租户级配置隔离
/// </summary>
[SugarTable("takt_foundation_setting", "系统设置表")]
[SugarIndex("ix_setting_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_setting_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_setting_key_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SettingKey), OrderByType.Asc, true)]
public class TaktSetting : TaktCompanyEntityBase
{
    /// <summary>
    /// 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    [SugarColumn(ColumnName = "setting_key", ColumnDescription = "设置键", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    public string SettingKey { get; set; } = string.Empty;

    /// <summary>
    /// 设置值（字符串形式，复杂对象用JSON）
    /// </summary>
    [SugarColumn(ColumnName = "setting_value", ColumnDescription = "设置值", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? SettingValue { get; set; }

    /// <summary>
    /// 设置名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    [SugarColumn(ColumnName = "setting_name", ColumnDescription = "设置名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string SettingName { get; set; } = string.Empty;

    /// <summary>
    /// 设置描述
    /// </summary>
    [SugarColumn(ColumnName = "setting_description", ColumnDescription = "设置描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? SettingDescription { get; set; }

    /// <summary>
    /// 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    [SugarColumn(ColumnName = "setting_group", ColumnDescription = "设置类别", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "frontend")]
    public string SettingGroup { get; set; } = "frontend";

    /// <summary>
    /// 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    [SugarColumn(ColumnName = "value_type", ColumnDescription = "值类型", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "input")]
    public string ValueType { get; set; } = "input";

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 只读（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_readonly", ColumnDescription = "只读", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsReadonly { get; set; } = 0;

    /// <summary>
    /// 加密（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_encrypted", ColumnDescription = "加密", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsEncrypted { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "setting_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SettingStatus { get; set; } = 1;
}
