// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDtos.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaOutput 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaOutput 生成，请按需审阅）
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
// PcbaOutput 响应 DTO
// ========================================

/// <summary>
/// PCBA日报实体 达成率(%) = 明细当日完成数量合计 ÷ 主表标准产能合计 × 100%。
/// 对应前端 TaktPcbaOutputDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaOutputDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaOutputID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_prod_category，存 DictValue：RD/EVT/DVT/EPP/PP/FPP/MP/RPR/RWR）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
    /// </summary>
    public string ProdTeam { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

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
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StdShorts { get; set; } = 0;

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// PCBA明细列表
    /// （子表：TaktPcbaOutputDetail）
    /// </summary>
    public List<TaktPcbaOutputDetailDto>? PcbaOutputDetails { get; set; }

}

// ========================================
// PcbaOutput 查询 DTO
// ========================================

/// <summary>
/// PcbaOutput 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaOutputQueryDto : TaktPagedQuery
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
    /// 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_prod_category，存 DictValue：RD/EVT/DVT/EPP/PP/FPP/MP/RPR/RWR）
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
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
    /// </summary>
    public string? ProdTeam { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
    /// 订单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StdShorts { get; set; }

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal? StdCapacity { get; set; }

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
// 创建PcbaOutput DTO
// ========================================

/// <summary>
/// 创建PcbaOutput DTO
/// </summary>
public class TaktPcbaOutputCreateDto
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
    /// 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_prod_category，存 DictValue：RD/EVT/DVT/EPP/PP/FPP/MP/RPR/RWR）
    /// </summary>
    [Required(ErrorMessage = "生产类别（字典 logistics_prod_category，存 DictValue：RD/EVT/DVT/EPP/PP/FPP/MP/RPR/RWR）不能为空")]
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
    /// </summary>
    [Required(ErrorMessage = "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）不能为空")]
    public string ProdTeam { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    [Required(ErrorMessage = "生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

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
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StdShorts { get; set; } = 0;

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// PCBA明细列表（子表，级联保存）
    /// </summary>
    public List<TaktPcbaOutputDetailCreateDto>? PcbaOutputDetails { get; set; }

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
// 更新PcbaOutput DTO
// ========================================

/// <summary>
/// 更新PcbaOutput DTO
/// 继承 TaktPcbaOutputCreateDto，添加 PcbaOutputId 字段
/// </summary>
public class TaktPcbaOutputUpdateDto : TaktPcbaOutputCreateDto
{
    /// <summary>
    /// PcbaOutputID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaOutput 导入模板行 DTO
/// </summary>
public class TaktPcbaOutputTemplateDto
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
    /// 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_prod_category，存 DictValue：RD/EVT/DVT/EPP/PP/FPP/MP/RPR/RWR）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
    /// </summary>
    public string? ProdTeam { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
    /// 订单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StdShorts { get; set; }

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// PCBA明细列表（子表，级联保存）
    /// </summary>
    public List<TaktPcbaOutputDetailCreateDto>? PcbaOutputDetails { get; set; }

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
/// PcbaOutput 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaOutputImportDto
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
    /// 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_prod_category，存 DictValue：RD/EVT/DVT/EPP/PP/FPP/MP/RPR/RWR）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
    /// </summary>
    public string? ProdTeam { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
    /// 订单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StdShorts { get; set; }

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// PCBA明细列表（子表，级联保存）
    /// </summary>
    public List<TaktPcbaOutputDetailCreateDto>? PcbaOutputDetails { get; set; }

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
/// PcbaOutput 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaOutputExportDto
{
    /// <summary>
    /// PcbaOutputID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_prod_category，存 DictValue：RD/EVT/DVT/EPP/PP/FPP/MP/RPR/RWR）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
    /// </summary>
    public string ProdTeam { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

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
    /// 订单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StdShorts { get; set; } = 0;

    /// <summary>
    /// 标准产能
    /// </summary>
    public decimal StdCapacity { get; set; }

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
