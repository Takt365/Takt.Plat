// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktProductionTeamDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionTeam 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionTeam 生成，请按需审阅）
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
// ProductionTeam 响应 DTO
// ========================================

/// <summary>
/// 生产班组实体（生产线班组主数据）
/// 对应前端 TaktProductionTeamDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionTeamDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionTeamID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_team_category）
    /// </summary>
    public string TeamCategory { get; set; } = "A";

    /// <summary>
    /// 班组长
    /// </summary>
    [AdaptMember("TeamLeaderName")]
    public string? TeamLeader { get; set; }

    /// <summary>
    /// 班次（字典 logistics_shift_category）
    /// </summary>
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 启用状态（1=启用，0=禁用）
    /// </summary>
    [AdaptMember("Status")]
    public int ProductionTeamStatus { get; set; } = 0;

}

// ========================================
// ProductionTeam 查询 DTO
// ========================================

/// <summary>
/// ProductionTeam 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionTeamQueryDto : TaktPagedQuery
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
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string? TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_team_category）
    /// </summary>
    public string? TeamCategory { get; set; }

    /// <summary>
    /// 班组长
    /// </summary>
    [AdaptMember("TeamLeaderName")]
    public string? TeamLeader { get; set; }

    /// <summary>
    /// 班次（字典 logistics_shift_category）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 启用状态（1=启用，0=禁用）
    /// </summary>
    [AdaptMember("Status")]
    public int? ProductionTeamStatus { get; set; }

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
// 创建ProductionTeam DTO
// ========================================

/// <summary>
/// 创建ProductionTeam DTO
/// </summary>
public class TaktProductionTeamCreateDto
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
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    [Required(ErrorMessage = "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）不能为空")]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    [Required(ErrorMessage = "班组名称（显示名称，如：SMT一班、手插二班等）不能为空")]
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_team_category）
    /// </summary>
    [Required(ErrorMessage = "班组分类不能为空")]
    public string TeamCategory { get; set; } = "A";

    /// <summary>
    /// 班组长
    /// </summary>
    [AdaptMember("TeamLeaderName")]
    public string? TeamLeader { get; set; }

    /// <summary>
    /// 班次（字典 logistics_shift_category）
    /// </summary>
    [Required(ErrorMessage = "班次不能为空")]
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 启用状态（1=启用，0=禁用）
    /// </summary>
    [AdaptMember("Status")]
    public int ProductionTeamStatus { get; set; } = 0;

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
// 更新ProductionTeam DTO
// ========================================

/// <summary>
/// 更新ProductionTeam DTO
/// 继承 TaktProductionTeamCreateDto，添加 ProductionTeamId 字段
/// </summary>
public class TaktProductionTeamUpdateDto : TaktProductionTeamCreateDto
{
    /// <summary>
    /// ProductionTeamID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamId { get; set; }

}

// ========================================
// ProductionTeam 状态 DTO
// ========================================

/// <summary>
/// ProductionTeam 状态更新 DTO
/// </summary>
public class TaktProductionTeamStatusDto
{
    /// <summary>
    /// ProductionTeamID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamId { get; set; }

    /// <summary>
    /// 启用状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "启用状态（1=启用，0=禁用）不能为空")]
    public int ProductionTeamStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionTeam 导入模板行 DTO
/// </summary>
public class TaktProductionTeamTemplateDto
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
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string? TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_team_category）
    /// </summary>
    public string? TeamCategory { get; set; }

    /// <summary>
    /// 班组长
    /// </summary>
    [AdaptMember("TeamLeaderName")]
    public string? TeamLeader { get; set; }

    /// <summary>
    /// 班次（字典 logistics_shift_category）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 启用状态（1=启用，0=禁用）
    /// </summary>
    [AdaptMember("Status")]
    public int? ProductionTeamStatus { get; set; }

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
/// ProductionTeam 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionTeamImportDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string? TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_team_category）
    /// </summary>
    public string? TeamCategory { get; set; }

    /// <summary>
    /// 班组长
    /// </summary>
    [AdaptMember("TeamLeaderName")]
    public string? TeamLeader { get; set; }

    /// <summary>
    /// 班次（字典 logistics_shift_category）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 启用状态（1=启用，0=禁用）
    /// </summary>
    [AdaptMember("Status")]
    public int? ProductionTeamStatus { get; set; }

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
/// ProductionTeam 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionTeamExportDto
{
    /// <summary>
    /// ProductionTeamID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_team_category）
    /// </summary>
    public string TeamCategory { get; set; } = "A";

    /// <summary>
    /// 班组长
    /// </summary>
    [AdaptMember("TeamLeaderName")]
    public string? TeamLeader { get; set; }

    /// <summary>
    /// 班次（字典 logistics_shift_category）
    /// </summary>
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 启用状态（1=启用，0=禁用）
    /// </summary>
    [AdaptMember("Status")]
    public int ProductionTeamStatus { get; set; } = 0;

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
