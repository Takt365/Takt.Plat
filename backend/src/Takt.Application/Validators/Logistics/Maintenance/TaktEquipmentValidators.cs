// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Maintenance
// 文件名称：TaktEquipmentValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Equipment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEquipment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Maintenance;

namespace Takt.Application.Validators.Logistics.Maintenance;

// ========================================
// 创建Equipment 验证器
// ========================================

/// <summary>
/// 创建Equipment DTO 验证器
/// </summary>
public class TaktEquipmentCreateValidator : AbstractValidator<TaktEquipmentCreateDto>
{
    /// <summary>
    /// 初始化 创建Equipment 校验规则
    /// </summary>
    public TaktEquipmentCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(40).WithMessage("工厂代码长度不能超过40个字符");
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(40).WithMessage("设备编码长度不能超过40个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空")
            .MaximumLength(40).WithMessage("设备名称长度不能超过40个字符");
        RuleFor(x => x.EquipmentModel)
            .MaximumLength(100).WithMessage("设备型号长度不能超过100个字符");
        RuleFor(x => x.EquipmentSpecification)
            .MaximumLength(200).WithMessage("设备规格长度不能超过200个字符");
        RuleFor(x => x.EquipmentBrand)
            .MaximumLength(100).WithMessage("设备品牌长度不能超过100个字符");
        RuleFor(x => x.Manufacturer)
            .MaximumLength(200).WithMessage("制造商长度不能超过200个字符");
        RuleFor(x => x.DealerBy)
            .MaximumLength(200).WithMessage("经销商长度不能超过200个字符");
        RuleFor(x => x.SerialNumber)
            .MaximumLength(100).WithMessage("序列号/出厂编号长度不能超过100个字符");
        RuleFor(x => x.WorkshopBy)
            .MaximumLength(100).WithMessage("所属车间长度不能超过100个字符");
        RuleFor(x => x.ProductionLineBy)
            .MaximumLength(100).WithMessage("所属产线长度不能超过100个字符");
        RuleFor(x => x.WorkstationBy)
            .MaximumLength(100).WithMessage("所属工位长度不能超过100个字符");
        RuleFor(x => x.DeptBy)
            .MaximumLength(100).WithMessage("所属部门长度不能超过100个字符");
        RuleFor(x => x.EquipmentLocation)
            .MaximumLength(200).WithMessage("设备位置长度不能超过200个字符");
        RuleFor(x => x.ResponsibleUserBy)
            .MaximumLength(50).WithMessage("负责人长度不能超过50个字符");
        RuleFor(x => x.OperatorBy)
            .MaximumLength(50).WithMessage("操作人长度不能超过50个字符");
        RuleFor(x => x.TechnicalParameters)
            .MaximumLength(4000).WithMessage("设备技术参数长度不能超过4000个字符");
        RuleFor(x => x.EquipmentImages)
            .MaximumLength(2000).WithMessage("设备图片长度不能超过2000个字符");
        RuleFor(x => x.EquipmentDocuments)
            .MaximumLength(2000).WithMessage("设备文档长度不能超过2000个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Equipment 验证器
// ========================================

/// <summary>
/// 更新Equipment DTO 验证器
/// </summary>
public class TaktEquipmentUpdateValidator : AbstractValidator<TaktEquipmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新Equipment 校验规则
    /// </summary>
    public TaktEquipmentUpdateValidator()
    {
        RuleFor(x => x.EquipmentId)
            .GreaterThan(0).WithMessage("EquipmentID无效");
    }
}

// ========================================
// 导入Equipment 验证器
// ========================================

/// <summary>
/// 导入Equipment DTO 验证器
/// </summary>
public class TaktEquipmentImportValidator : AbstractValidator<TaktEquipmentImportDto>
{
    /// <summary>
    /// 初始化 导入Equipment 校验规则
    /// </summary>
    public TaktEquipmentImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(40).WithMessage("工厂代码长度不能超过40个字符");
        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(40).WithMessage("设备编码长度不能超过40个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空")
            .MaximumLength(40).WithMessage("设备名称长度不能超过40个字符");
        RuleFor(x => x.EquipmentModel)
            .MaximumLength(100).WithMessage("设备型号长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.EquipmentModel));
        RuleFor(x => x.EquipmentSpecification)
            .MaximumLength(200).WithMessage("设备规格长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.EquipmentSpecification));
        RuleFor(x => x.EquipmentBrand)
            .MaximumLength(100).WithMessage("设备品牌长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.EquipmentBrand));
        RuleFor(x => x.Manufacturer)
            .MaximumLength(200).WithMessage("制造商长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.Manufacturer));
        RuleFor(x => x.DealerBy)
            .MaximumLength(200).WithMessage("经销商长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.DealerBy));
        RuleFor(x => x.SerialNumber)
            .MaximumLength(100).WithMessage("序列号/出厂编号长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));
        RuleFor(x => x.WorkshopBy)
            .MaximumLength(100).WithMessage("所属车间长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.WorkshopBy));
        RuleFor(x => x.ProductionLineBy)
            .MaximumLength(100).WithMessage("所属产线长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.ProductionLineBy));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
