// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaInspection 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaInspection 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Defect;

// ========================================
// PcbaInspection 响应 DTO
// ========================================

/// <summary>
/// PCBA检查日报实体
/// 对应前端 TaktPcbaInspectionDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaInspectionDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaInspectionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

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
    /// 生产工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 机种
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态(0=正常 1=停用)
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// PCBA检查明细列表
    /// （子表：TaktPcbaInspectionDetail）
    /// </summary>
    public List<TaktPcbaInspectionDetailDto>? PcbaInspectionDetails { get; set; }

}

// ========================================
// PcbaInspection 查询 DTO
// ========================================

/// <summary>
/// PcbaInspection 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaInspectionQueryDto : TaktPagedQuery
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
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 机种
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态(0=正常 1=停用)
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
// 创建PcbaInspection DTO
// ========================================

/// <summary>
/// 创建PcbaInspection DTO
/// </summary>
public class TaktPcbaInspectionCreateDto
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
    /// 生产工单号
    /// </summary>
    [Required(ErrorMessage = "生产工单号不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 机种
    /// </summary>
    [Required(ErrorMessage = "机种不能为空")]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态(0=正常 1=停用)
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// PCBA检查明细列表（子表，级联保存）
    /// </summary>
    public List<TaktPcbaInspectionDetailCreateDto>? PcbaInspectionDetails { get; set; }

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
// 更新PcbaInspection DTO
// ========================================

/// <summary>
/// 更新PcbaInspection DTO
/// 继承 TaktPcbaInspectionCreateDto，添加 PcbaInspectionId 字段
/// </summary>
public class TaktPcbaInspectionUpdateDto : TaktPcbaInspectionCreateDto
{
    /// <summary>
    /// PcbaInspectionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

}

// ========================================
// PcbaInspection 状态 DTO
// ========================================

/// <summary>
/// PcbaInspection 状态更新 DTO
/// </summary>
public class TaktPcbaInspectionStatusDto
{
    /// <summary>
    /// PcbaInspectionID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

    /// <summary>
    /// 状态(0=正常 1=停用)
    /// </summary>
    [Required(ErrorMessage = "状态(0=正常 1=停用)不能为空")]
    public int Status { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaInspection 导入模板行 DTO
/// </summary>
public class TaktPcbaInspectionTemplateDto
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
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态(0=正常 1=停用)
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
/// PcbaInspection 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaInspectionImportDto
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
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态(0=正常 1=停用)
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
/// PcbaInspection 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaInspectionExportDto
{
    /// <summary>
    /// PcbaInspectionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

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
    /// 生产工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 机种
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态(0=正常 1=停用)
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
