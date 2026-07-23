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

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 字典数据实体
/// 字典类型的具体数据项，如：订单状态下的“待支付”、“已完成”等
/// 租户级实体：字典数据在租户内共享；CultureCode eo=全局通用（各语言均加载），非空为区域专用（如 zh-CN、ja-JP，与 TaktCulture.CultureCode / Accept-Language 一致）；前端字典缓存加载「eo + 当前登录 UI 语言」项
/// </summary>
[SugarTable("takt_foundation_dict_data", "字典数据表")]
[SugarIndex("ix_dict_data_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_dict_data_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_dict_data_type_label_i18n_unique", nameof(TenantCode), OrderByType.Asc, nameof(DictTypeId), OrderByType.Asc, nameof(CultureCode), OrderByType.Asc, nameof(DictLabel), OrderByType.Asc, nameof(I18nKey), OrderByType.Asc, true)]
[SugarIndex("ix_dict_data_type_value_culture", nameof(TenantCode), OrderByType.Asc, nameof(DictTypeCode), OrderByType.Asc, nameof(CultureCode), OrderByType.Asc, nameof(DictValue), OrderByType.Asc, false)]
public class TaktDictData : TaktTenantEntityBase
{
    /// <summary>
    /// 字典类型ID（关联 TaktDictType.Id；唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一）
    /// </summary>
    [SugarColumn(ColumnName = "dict_type_id", ColumnDescription = "字典类型ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典类型编码（关联 TaktDictType.DictTypeCode）
    /// </summary>
    [SugarColumn(ColumnName = "dict_type_code", ColumnDescription = "字典类型编码", ColumnDataType = "varchar", Length = 80, IsNullable = false, DefaultValue = "")]
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）
    /// </summary>
    [SugarColumn(ColumnName = "dict_label", ColumnDescription = "字典项标签", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典项值（实际存储值，如：0, 1, 2）
    /// </summary>
    [SugarColumn(ColumnName = "dict_value", ColumnDescription = "字典项值", ColumnDataType = "varchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 国际化翻译键（与 DictTypeCode 段对应，如 dict.sys.equipment.status.0、dict.logistics.supplier.category.1）
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
    /// 区域文化编码（eo=全局通用/世界语；如 zh-CN、ja-JP 与 TaktCulture.CultureCode 对齐，仅当前 Accept-Language 匹配时与全局项一并加载）
    /// </summary>
    [SugarColumn(ColumnName = "culture_code", ColumnDescription = "区域文化编码", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "eo")]
    public string CultureCode { get; set; } = "eo";

    /// <summary>
    /// 是否默认项（1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_default", ColumnDescription = "是否默认项", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDefault { get; set; } = 0;

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
