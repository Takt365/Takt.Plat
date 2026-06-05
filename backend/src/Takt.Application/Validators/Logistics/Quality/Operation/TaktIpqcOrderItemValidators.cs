// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderItemValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：IpqcOrderItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktIpqcOrderItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建IpqcOrderItem 验证器
// ========================================

/// <summary>
/// 创建IpqcOrderItem DTO 验证器
/// </summary>
public class TaktIpqcOrderItemCreateValidator : AbstractValidator<TaktIpqcOrderItemCreateDto>
{
    /// <summary>
    /// 初始化 创建IpqcOrderItem 校验规则
    /// </summary>
    public TaktIpqcOrderItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.IpqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("IPQC检验单ID不能为负数");
        RuleFor(x => x.IpqcOrderCode)
            .NotEmpty().WithMessage("IPQC检验单编码不能为空")
            .MaximumLength(50).WithMessage("IPQC检验单编码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.BatchNo)
            .MaximumLength(50).WithMessage("批次号长度不能超过50个字符");
        RuleFor(x => x.StandardCode)
            .NotEmpty().WithMessage("检验标准编码不能为空")
            .MaximumLength(50).WithMessage("检验标准编码长度不能超过50个字符");
        RuleFor(x => x.SamplingSchemeCode)
            .NotEmpty().WithMessage("抽样方案编码不能为空")
            .MaximumLength(50).WithMessage("抽样方案编码长度不能超过50个字符");
        RuleFor(x => x.SampleSerialNo)
            .MaximumLength(100).WithMessage("抽检序列号长度不能超过100个字符");
        RuleFor(x => x.InspectionDescription)
            .MaximumLength(1000).WithMessage("检验说明长度不能超过1000个字符");
        RuleFor(x => x.InspectorBy)
            .NotEmpty().WithMessage("检验员不能为空")
            .MaximumLength(50).WithMessage("检验员长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新IpqcOrderItem 验证器
// ========================================

/// <summary>
/// 更新IpqcOrderItem DTO 验证器
/// </summary>
public class TaktIpqcOrderItemUpdateValidator : AbstractValidator<TaktIpqcOrderItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新IpqcOrderItem 校验规则
    /// </summary>
    public TaktIpqcOrderItemUpdateValidator()
    {
        RuleFor(x => x.IpqcOrderItemId)
            .GreaterThan(0).WithMessage("IpqcOrderItemID无效");
    }
}

// ========================================
// 导入IpqcOrderItem 验证器
// ========================================

/// <summary>
/// 导入IpqcOrderItem DTO 验证器
/// </summary>
public class TaktIpqcOrderItemImportValidator : AbstractValidator<TaktIpqcOrderItemImportDto>
{
    /// <summary>
    /// 初始化 导入IpqcOrderItem 校验规则
    /// </summary>
    public TaktIpqcOrderItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.IpqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("IPQC检验单ID不能为负数");
        RuleFor(x => x.IpqcOrderCode)
            .NotEmpty().WithMessage("IPQC检验单编码不能为空")
            .MaximumLength(50).WithMessage("IPQC检验单编码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.BatchNo)
            .MaximumLength(50).WithMessage("批次号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.BatchNo));
        RuleFor(x => x.StandardCode)
            .NotEmpty().WithMessage("检验标准编码不能为空")
            .MaximumLength(50).WithMessage("检验标准编码长度不能超过50个字符");
        RuleFor(x => x.SamplingSchemeCode)
            .NotEmpty().WithMessage("抽样方案编码不能为空")
            .MaximumLength(50).WithMessage("抽样方案编码长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
