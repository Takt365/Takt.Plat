// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyOutput 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssyOutput 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

// ========================================
// AssyOutput 响应 DTO
// ========================================

/// <summary>
/// 组立日报（产出）主表实体
/// 对应前端 TaktAssyOutputDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssyOutputDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssyOutputID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别 RD: 研发  EVT: 工程验证测试  DVT: 设计验证测试  EPP: 工程试产  PP: 试产  FPP: 正式生产  MP: 大规模生产  RPR: 维修生产  RWR: 返工生产
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 生产订单类型
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// 组立日报明细列表
    /// （子表：TaktAssyOutputDetail）
    /// </summary>
    public List<TaktAssyOutputDetailDto>? AssyOutputDetails { get; set; }

}

// ========================================
// AssyOutput 查询 DTO
// ========================================

/// <summary>
/// AssyOutput 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssyOutputQueryDto : TaktPagedQuery
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
    /// 生产类别 RD: 研发  EVT: 工程验证测试  DVT: 设计验证测试  EPP: 工程试产  PP: 试产  FPP: 正式生产  MP: 大规模生产  RPR: 维修生产  RWR: 返工生产
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期（范围查询-开始）
    /// </summary>
    public DateTime? ProdDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围查询-结束）
    /// </summary>
    public DateTime? ProdDateEnd { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 生产订单类型
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int? Status { get; set; }

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
// 创建AssyOutput DTO
// ========================================

/// <summary>
/// 创建AssyOutput DTO
/// </summary>
public class TaktAssyOutputCreateDto
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
    /// 生产类别 RD: 研发  EVT: 工程验证测试  DVT: 设计验证测试  EPP: 工程试产  PP: 试产  FPP: 正式生产  MP: 大规模生产  RPR: 维修生产  RWR: 返工生产
    /// </summary>
    [Required(ErrorMessage = "生产类别 RD: 研发  EVT: 工程验证测试  DVT: 设计验证测试  EPP: 工程试产  PP: 试产  FPP: 正式生产  MP: 大规模生产  RPR: 维修生产  RWR: 返工生产不能为空")]
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    [Required(ErrorMessage = "生产线不能为空")]
    public string ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 生产订单类型
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    [Required(ErrorMessage = "生产工单号不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    [Required(ErrorMessage = "机种不能为空")]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// 组立日报明细列表（子表，级联保存）
    /// </summary>
    public List<TaktAssyOutputDetailCreateDto>? AssyOutputDetails { get; set; }

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
// 更新AssyOutput DTO
// ========================================

/// <summary>
/// 更新AssyOutput DTO
/// 继承 TaktAssyOutputCreateDto，添加 AssyOutputId 字段
/// </summary>
public class TaktAssyOutputUpdateDto : TaktAssyOutputCreateDto
{
    /// <summary>
    /// AssyOutputID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

}

// ========================================
// AssyOutput 状态 DTO
// ========================================

/// <summary>
/// AssyOutput 状态更新 DTO
/// </summary>
public class TaktAssyOutputStatusDto
{
    /// <summary>
    /// AssyOutputID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public int Status { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AssyOutput 导入模板行 DTO
/// </summary>
public class TaktAssyOutputTemplateDto
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
    /// 生产类别 RD: 研发  EVT: 工程验证测试  DVT: 设计验证测试  EPP: 工程试产  PP: 试产  FPP: 正式生产  MP: 大规模生产  RPR: 维修生产  RWR: 返工生产
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 生产订单类型
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    public int? Status { get; set; }

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
/// AssyOutput 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssyOutputImportDto
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
    /// 生产类别 RD: 研发  EVT: 工程验证测试  DVT: 设计验证测试  EPP: 工程试产  PP: 试产  FPP: 正式生产  MP: 大规模生产  RPR: 维修生产  RWR: 返工生产
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 生产订单类型
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    public int? Status { get; set; }

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
/// AssyOutput 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssyOutputExportDto
{
    /// <summary>
    /// AssyOutputID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别 RD: 研发  EVT: 工程验证测试  DVT: 设计验证测试  EPP: 工程试产  PP: 试产  FPP: 正式生产  MP: 大规模生产  RPR: 维修生产  RWR: 返工生产
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 生产订单类型
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; } = 0;

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
