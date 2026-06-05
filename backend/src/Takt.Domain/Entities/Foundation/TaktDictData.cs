// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktDictData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：字典数据实体，字典类型的具体数据项
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 字典数据实体
/// 字典类型的具体数据项，如：订单状态下的“待支付”、“已完成”等
/// 租户级实体：字典数据在租户内共享，不需要公司隔离
/// </summary>
[SugarTable("takt_foundation_dict_data", "字典数据表")]
[SugarIndex("ix_dict_data_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_dict_data_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_dict_data_type_label_i18n_unique", nameof(TenantCode), OrderByType.Asc, nameof(DictTypeId), OrderByType.Asc, nameof(DictLabel), OrderByType.Asc, nameof(I18nKey), OrderByType.Asc, true)]
public class TaktDictData : TaktTenantEntityBase
{
    /// <summary>
    /// 字典类型ID（关联 TaktDictType.Id；唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique）
    /// </summary>
    [SugarColumn(ColumnName = "dict_type_id", ColumnDescription = "字典类型ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long DictTypeId { get; set; } = 0;

    /// <summary>
    /// 字典类型编码（关联 TaktDictType.DictTypeCode）
    /// </summary>
    [SugarColumn(ColumnName = "dict_type_code", ColumnDescription = "字典类型编码", ColumnDataType = "varchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典项标签（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：待支付、已完成）
    /// </summary>
    [SugarColumn(ColumnName = "dict_label", ColumnDescription = "字典项标签", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典项值（实际存储值，如：0, 1, 2）
    /// </summary>
    [SugarColumn(ColumnName = "dict_value", ColumnDescription = "字典项值", ColumnDataType = "varchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 国际化翻译键（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：dict.user_type.admin）
    /// </summary>
    [SugarColumn(ColumnName = "i18n_key", ColumnDescription = "国际化翻译键", ColumnDataType = "varchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
    /// </summary>
    [SugarColumn(ColumnName = "ext_label", ColumnDescription = "扩展标签", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ExtLabel { get; set; }

    /// <summary>
    /// 扩展值（用于存储额外的业务数据，如：编码、标识符等）
    /// </summary>
    [SugarColumn(ColumnName = "ext_value", ColumnDescription = "扩展值", ColumnDataType = "varchar", Length = 200, IsNullable = true)]
    public string? ExtValue { get; set; }

    /// <summary>
    /// 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info）
    /// 用于下拉列表选项中显示的颜色标识
    /// </summary>
    [SugarColumn(ColumnName = "list_class", ColumnDescription = "列表样式类", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ListClass { get; set; } = 0;

    /// <summary>
    /// CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info）
    /// 用于数据表格中字典值显示的颜色标签
    /// </summary>
    [SugarColumn(ColumnName = "css_class", ColumnDescription = "CSS类名", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CssClass { get; set; } = 0;

    /// <summary>
    /// 是否默认项（1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_default", ColumnDescription = "是否默认项", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsDefault { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 字典类型（多对一关联）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(DictTypeCode))]
    public TaktDictType? DictType { get; set; }
}
