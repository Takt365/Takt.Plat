// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptValidators.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：EcDept 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcDept 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcDept 验证器
// ========================================

/// <summary>
/// 创建EcDept DTO 验证器
/// </summary>
public class TaktEcDeptCreateValidator : AbstractValidator<TaktEcDeptCreateDto>
{
    /// <summary>
    /// 初始化 创建EcDept 校验规则
    /// </summary>
    public TaktEcDeptCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EcnDetailId)
            .GreaterThanOrEqualTo(0).WithMessage("设变明细ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检不能为空")
            .MaximumLength(20).WithMessage("部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检长度不能超过20个字符");
        RuleFor(x => x.Content)
            .MaximumLength(2000).WithMessage("内容长度不能超过2000个字符");
        RuleFor(x => x.ScheduledBatch)
            .MaximumLength(100).WithMessage("预定批次长度不能超过100个字符");
        RuleFor(x => x.PoRemainder)
            .MaximumLength(200).WithMessage("Po残长度不能超过200个字符");
        RuleFor(x => x.Balance)
            .MaximumLength(200).WithMessage("结余长度不能超过200个字符");
        RuleFor(x => x.OldProductHandling)
            .MaximumLength(500).WithMessage("旧品处理长度不能超过500个字符");
        RuleFor(x => x.Supplier)
            .MaximumLength(200).WithMessage("供应商长度不能超过200个字符");
        RuleFor(x => x.PurchaseOrderNo)
            .MaximumLength(100).WithMessage("采购订单号码长度不能超过100个字符");
        RuleFor(x => x.IqcOrderNo)
            .MaximumLength(50).WithMessage("受检单号长度不能超过50个字符");
        RuleFor(x => x.OutboundBatch)
            .MaximumLength(100).WithMessage("出库批次长度不能超过100个字符");
        RuleFor(x => x.ProductionBatch)
            .MaximumLength(100).WithMessage("生产批次长度不能超过100个字符");
        RuleFor(x => x.OutboundOrderNo)
            .MaximumLength(100).WithMessage("出库单号长度不能超过100个字符");
        RuleFor(x => x.ProductionTeam)
            .MaximumLength(100).WithMessage("生产班组长度不能超过100个字符");
        RuleFor(x => x.InspectionBatch)
            .MaximumLength(100).WithMessage("检验批次长度不能超过100个字符");
        RuleFor(x => x.SamplingNo)
            .MaximumLength(100).WithMessage("抽样号码长度不能超过100个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EcDept 验证器
// ========================================

/// <summary>
/// 更新EcDept DTO 验证器
/// </summary>
public class TaktEcDeptUpdateValidator : AbstractValidator<TaktEcDeptUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcDept 校验规则
    /// </summary>
    public TaktEcDeptUpdateValidator()
    {
        RuleFor(x => x.EcDeptId)
            .GreaterThan(0).WithMessage("EcDeptID无效");
    }
}

// ========================================
// 导入EcDept 验证器
// ========================================

/// <summary>
/// 导入EcDept DTO 验证器
/// </summary>
public class TaktEcDeptImportValidator : AbstractValidator<TaktEcDeptImportDto>
{
    /// <summary>
    /// 初始化 导入EcDept 校验规则
    /// </summary>
    public TaktEcDeptImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EcnDetailId)
            .GreaterThanOrEqualTo(0).WithMessage("设变明细ID不能为负数");
        RuleFor(x => x.EcNo)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检不能为空")
            .MaximumLength(20).WithMessage("部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检长度不能超过20个字符");
        RuleFor(x => x.Content)
            .MaximumLength(2000).WithMessage("内容长度不能超过2000个字符").When(x => !string.IsNullOrWhiteSpace(x.Content));
        RuleFor(x => x.ScheduledBatch)
            .MaximumLength(100).WithMessage("预定批次长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.ScheduledBatch));
        RuleFor(x => x.PoRemainder)
            .MaximumLength(200).WithMessage("Po残长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.PoRemainder));
        RuleFor(x => x.Balance)
            .MaximumLength(200).WithMessage("结余长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.Balance));
        RuleFor(x => x.OldProductHandling)
            .MaximumLength(500).WithMessage("旧品处理长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.OldProductHandling));
        RuleFor(x => x.Supplier)
            .MaximumLength(200).WithMessage("供应商长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.Supplier));
        RuleFor(x => x.PurchaseOrderNo)
            .MaximumLength(100).WithMessage("采购订单号码长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.PurchaseOrderNo));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
