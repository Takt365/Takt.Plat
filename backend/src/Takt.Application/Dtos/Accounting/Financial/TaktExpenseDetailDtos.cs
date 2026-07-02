// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktExpenseDetailDtos.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Auto Generated)
// 功能描述：ExpenseDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktExpenseDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// ExpenseDetail 响应 DTO
// ========================================

/// <summary>
/// 费用单明细实体
/// 对应前端 TaktExpenseDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktExpenseDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ExpenseDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseDetailId { get; set; }

    /// <summary>
    /// 费用单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseId { get; set; }

    /// <summary>
    /// 费用单 名称（填充字段）
    /// </summary>
    public string? ExpenseName { get; set; }

    /// <summary>
    /// 费用单编号（冗余，便于查询）
    /// </summary>
    public string ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    public string AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 明细项名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 明细项说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal ItemQuantity { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal ItemAmount { get; set; }

    /// <summary>
    /// 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 发票号码
    /// </summary>
    public string? InvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime? ExpenseDetailDate { get; set; }

}

// ========================================
// ExpenseDetail 查询 DTO
// ========================================

/// <summary>
/// ExpenseDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktExpenseDetailQueryDto : TaktPagedQuery
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
    /// 费用单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExpenseId { get; set; }

    /// <summary>
    /// 费用单编号（冗余，便于查询）
    /// </summary>
    public string? ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    public string? AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 明细项名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 明细项说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? ItemQuantity { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? ItemAmount { get; set; }

    /// <summary>
    /// 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 发票号码
    /// </summary>
    public string? InvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 费用发生日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpenseDetailDateStart { get; set; }

    /// <summary>
    /// 费用发生日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpenseDetailDateEnd { get; set; }

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
// 创建ExpenseDetail DTO
// ========================================

/// <summary>
/// 创建ExpenseDetail DTO
/// </summary>
public class TaktExpenseDetailCreateDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 费用单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseId { get; set; }

    /// <summary>
    /// 费用单编号（冗余，便于查询）
    /// </summary>
    [Required(ErrorMessage = "费用单编号（冗余，便于查询）不能为空")]
    public string ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    [Required(ErrorMessage = "分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）不能为空")]
    public string AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 明细项名称
    /// </summary>
    [Required(ErrorMessage = "明细项名称不能为空")]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 明细项说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal ItemQuantity { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal ItemAmount { get; set; }

    /// <summary>
    /// 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 发票号码
    /// </summary>
    public string? InvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime? ExpenseDetailDate { get; set; }

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
// 更新ExpenseDetail DTO
// ========================================

/// <summary>
/// 更新ExpenseDetail DTO
/// 继承 TaktExpenseDetailCreateDto，添加 ExpenseDetailId 字段
/// </summary>
public class TaktExpenseDetailUpdateDto : TaktExpenseDetailCreateDto
{
    /// <summary>
    /// ExpenseDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseDetailId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ExpenseDetail 导入模板行 DTO
/// </summary>
public class TaktExpenseDetailTemplateDto
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
    /// 费用单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExpenseId { get; set; }

    /// <summary>
    /// 费用单编号（冗余，便于查询）
    /// </summary>
    public string? ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    public string? AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 明细项名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 明细项说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? ItemQuantity { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? ItemAmount { get; set; }

    /// <summary>
    /// 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 发票号码
    /// </summary>
    public string? InvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime? ExpenseDetailDate { get; set; }

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
/// ExpenseDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktExpenseDetailImportDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 费用单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExpenseId { get; set; }

    /// <summary>
    /// 费用单编号（冗余，便于查询）
    /// </summary>
    public string? ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    public string? AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 明细项名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 明细项说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? ItemQuantity { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? ItemAmount { get; set; }

    /// <summary>
    /// 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 发票号码
    /// </summary>
    public string? InvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime? ExpenseDetailDate { get; set; }

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
/// ExpenseDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktExpenseDetailExportDto
{
    /// <summary>
    /// ExpenseDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseDetailId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseId { get; set; }

    /// <summary>
    /// 费用单编号（冗余，便于查询）
    /// </summary>
    public string ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    public string AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 明细项名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 明细项说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal ItemQuantity { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal ItemAmount { get; set; }

    /// <summary>
    /// 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 发票号码
    /// </summary>
    public string? InvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime? ExpenseDetailDate { get; set; }

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
