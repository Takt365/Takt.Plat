// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialDocumentDtos.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialDocument 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialDocument 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// MaterialDocument 响应 DTO
// ========================================

/// <summary>
/// Takt物料凭证主表实体（公司级；行项目见 TaktMaterialDocumentItem）
/// 对应前端 TaktMaterialDocumentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialDocumentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialDocumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证号（租户+公司+工厂内唯一）
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int MaterialDocumentStatus { get; set; } = 0;

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）
    /// （子表：TaktMaterialDocumentItem）
    /// </summary>
    public List<TaktMaterialDocumentItemDto>? Items { get; set; }

}

// ========================================
// MaterialDocument 查询 DTO
// ========================================

/// <summary>
/// MaterialDocument 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialDocumentQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证号（租户+公司+工厂内唯一）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int? MaterialDocumentStatus { get; set; }

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
// 创建MaterialDocument DTO
// ========================================

/// <summary>
/// 创建MaterialDocument DTO
/// </summary>
public class TaktMaterialDocumentCreateDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
    /// </summary>
    [Required(ErrorMessage = "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证号（租户+公司+工厂内唯一）
    /// </summary>
    [Required(ErrorMessage = "物料凭证号（租户+公司+工厂内唯一）不能为空")]
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int MaterialDocumentStatus { get; set; } = 0;

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDocumentItemCreateDto>? Items { get; set; }

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
// 更新MaterialDocument DTO
// ========================================

/// <summary>
/// 更新MaterialDocument DTO
/// 继承 TaktMaterialDocumentCreateDto，添加 MaterialDocumentId 字段
/// </summary>
public class TaktMaterialDocumentUpdateDto : TaktMaterialDocumentCreateDto
{
    /// <summary>
    /// MaterialDocumentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktMaterialDocumentItemUpdateDto>? Items { get; set; }

}

// ========================================
// MaterialDocument 状态 DTO
// ========================================

/// <summary>
/// MaterialDocument 状态更新 DTO
/// </summary>
public class TaktMaterialDocumentStatusDto
{
    /// <summary>
    /// MaterialDocumentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    [Required(ErrorMessage = "物料凭证状态（0=草稿，1=已过账，2=已作废）不能为空")]
    public int MaterialDocumentStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialDocument 导入模板行 DTO
/// </summary>
public class TaktMaterialDocumentTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证号（租户+公司+工厂内唯一）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int? MaterialDocumentStatus { get; set; }

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDocumentItemCreateDto>? Items { get; set; }

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
/// MaterialDocument 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialDocumentImportDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证号（租户+公司+工厂内唯一）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int? MaterialDocumentStatus { get; set; }

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDocumentItemCreateDto>? Items { get; set; }

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
/// MaterialDocument 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialDocumentExportDto
{
    /// <summary>
    /// MaterialDocumentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options；从物料主数据跳转按此字段查凭证列表）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证号（租户+公司+工厂内唯一）
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int MaterialDocumentStatus { get; set; } = 0;

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
