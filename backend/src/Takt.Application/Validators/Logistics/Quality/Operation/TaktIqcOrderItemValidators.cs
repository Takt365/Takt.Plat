// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderItemValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：IqcOrderItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktIqcOrderItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建IqcOrderItem 验证器
// ========================================

/// <summary>
/// 创建IqcOrderItem DTO 验证器
/// </summary>
public class TaktIqcOrderItemCreateValidator : AbstractValidator<TaktIqcOrderItemCreateDto>
{
    /// <summary>
    /// 初始化 创建IqcOrderItem 校验规则
    /// </summary>
    public TaktIqcOrderItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.IqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("IQC检验单ID不能为负数");
        RuleFor(x => x.IqcOrderCode)
            .NotEmpty().WithMessage("IQC检验单编码不能为空")
            .MaximumLength(40).WithMessage("IQC检验单编码长度不能超过40个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(40).WithMessage("物料编码长度不能超过40个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.BatchNo)
            .MaximumLength(50).WithMessage("批次号长度不能超过50个字符");
        RuleFor(x => x.StandardCode)
            .NotEmpty().WithMessage("检验标准编码不能为空")
            .MaximumLength(40).WithMessage("检验标准编码长度不能超过40个字符");
        RuleFor(x => x.SamplingSchemeCode)
            .NotEmpty().WithMessage("抽样方案编码不能为空")
            .MaximumLength(40).WithMessage("抽样方案编码长度不能超过40个字符");
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
// 更新IqcOrderItem 验证器
// ========================================

/// <summary>
/// 更新IqcOrderItem DTO 验证器
/// </summary>
public class TaktIqcOrderItemUpdateValidator : AbstractValidator<TaktIqcOrderItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新IqcOrderItem 校验规则
    /// </summary>
    public TaktIqcOrderItemUpdateValidator()
    {
        RuleFor(x => x.IqcOrderItemId)
            .GreaterThan(0).WithMessage("IqcOrderItemID无效");
    }
}

// ========================================
// 导入IqcOrderItem 验证器
// ========================================

/// <summary>
/// 导入IqcOrderItem DTO 验证器
/// </summary>
public class TaktIqcOrderItemImportValidator : AbstractValidator<TaktIqcOrderItemImportDto>
{
    /// <summary>
    /// 初始化 导入IqcOrderItem 校验规则
    /// </summary>
    public TaktIqcOrderItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.IqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("IQC检验单ID不能为负数");
        RuleFor(x => x.IqcOrderCode)
            .NotEmpty().WithMessage("IQC检验单编码不能为空")
            .MaximumLength(40).WithMessage("IQC检验单编码长度不能超过40个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(40).WithMessage("物料编码长度不能超过40个字符");
        RuleFor(x => x.MaterialName)
            .NotEmpty().WithMessage("物料名称不能为空")
            .MaximumLength(40).WithMessage("物料名称长度不能超过40个字符");
        RuleFor(x => x.BatchNo)
            .MaximumLength(50).WithMessage("批次号长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.BatchNo));
        RuleFor(x => x.StandardCode)
            .NotEmpty().WithMessage("检验标准编码不能为空")
            .MaximumLength(40).WithMessage("检验标准编码长度不能超过40个字符");
        RuleFor(x => x.SamplingSchemeCode)
            .NotEmpty().WithMessage("抽样方案编码不能为空")
            .MaximumLength(40).WithMessage("抽样方案编码长度不能超过40个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
