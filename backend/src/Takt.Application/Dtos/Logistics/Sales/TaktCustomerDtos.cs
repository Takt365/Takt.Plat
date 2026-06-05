// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktCustomerDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：Customer 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomer 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// Customer 响应 DTO
// ========================================

/// <summary>
/// Takt客户信息实体
/// 对应前端 TaktCustomerDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（唯一索引）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
    /// </summary>
    public int CustomerType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比，如：5.5表示5.5%折扣）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 销售员（人员代码）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户等级（0=普通，1=重要，2=VIP，3=战略）
    /// </summary>
    public int CustomerLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格客户（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 客户状态（1=启用，0=禁用）
    /// </summary>
    public int CustomerStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// Customer 查询 DTO
// ========================================

/// <summary>
/// Customer 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerQueryDto : TaktPagedQuery
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（唯一索引）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
    /// </summary>
    public int? CustomerType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    public int? PaymentTerms { get; set; }

    /// <summary>
    /// 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比，如：5.5表示5.5%折扣）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 销售员（人员代码）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户等级（0=普通，1=重要，2=VIP，3=战略）
    /// </summary>
    public int? CustomerLevel { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格客户（0=否，1=是）
    /// </summary>
    public int? IsQualified { get; set; }

    /// <summary>
    /// 客户状态（1=启用，0=禁用）
    /// </summary>
    public int? CustomerStatus { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建Customer DTO
// ========================================

/// <summary>
/// 创建Customer DTO
/// </summary>
public class TaktCustomerCreateDto
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
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "客户编码（唯一索引）不能为空")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
    /// </summary>
    public int CustomerType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    [Required(ErrorMessage = "结算币种代码不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比，如：5.5表示5.5%折扣）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 销售员（人员代码）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户等级（0=普通，1=重要，2=VIP，3=战略）
    /// </summary>
    public int CustomerLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格客户（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 客户状态（1=启用，0=禁用）
    /// </summary>
    public int CustomerStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
// 更新Customer DTO
// ========================================

/// <summary>
/// 更新Customer DTO
/// 继承 TaktCustomerCreateDto，添加 CustomerId 字段
/// </summary>
public class TaktCustomerUpdateDto : TaktCustomerCreateDto
{
    /// <summary>
    /// CustomerID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

}

// ========================================
// Customer 状态 DTO
// ========================================

/// <summary>
/// Customer 状态更新 DTO
/// </summary>
public class TaktCustomerStatusDto
{
    /// <summary>
    /// CustomerID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "客户状态（1=启用，0=禁用）不能为空")]
    public int CustomerStatus { get; set; } = 0;
}

// ========================================
// Customer 排序 DTO
// ========================================

/// <summary>
/// Customer 排序更新 DTO
/// </summary>
public class TaktCustomerSortDto
{
    /// <summary>
    /// CustomerID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Customer 导入模板行 DTO
/// </summary>
public class TaktCustomerTemplateDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（唯一索引）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
    /// </summary>
    public int? CustomerType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

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
/// Customer 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerImportDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（唯一索引）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
    /// </summary>
    public int? CustomerType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

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
/// Customer 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerExportDto
{
    /// <summary>
    /// CustomerID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（唯一索引）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
    /// </summary>
    public int CustomerType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比，如：5.5表示5.5%折扣）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 销售员（人员代码）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户等级（0=普通，1=重要，2=VIP，3=战略）
    /// </summary>
    public int CustomerLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格客户（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 客户状态（1=启用，0=禁用）
    /// </summary>
    public int CustomerStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
