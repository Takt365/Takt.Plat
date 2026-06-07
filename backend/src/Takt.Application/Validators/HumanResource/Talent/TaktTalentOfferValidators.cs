// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Talent
// 文件名称：TaktTalentOfferValidators.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentOffer 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTalentOffer 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Talent;

namespace Takt.Application.Validators.HumanResource.Talent;

// ========================================
// 创建TalentOffer 验证器
// ========================================

/// <summary>
/// 创建TalentOffer DTO 验证器
/// </summary>
public class TaktTalentOfferCreateValidator : AbstractValidator<TaktTalentOfferCreateDto>
{
    /// <summary>
    /// 初始化 创建TalentOffer 校验规则
    /// </summary>
    public TaktTalentOfferCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.InterviewId)
            .GreaterThanOrEqualTo(0).WithMessage("面试安排ID不能为负数");
        RuleFor(x => x.OfferNo)
            .NotEmpty().WithMessage("录用编号不能为空")
            .MaximumLength(20).WithMessage("录用编号长度不能超过20个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("关联员工ID不能为负数");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("拟录用部门ID不能为负数");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("拟录用部门名称不能为空")
            .MaximumLength(100).WithMessage("拟录用部门名称长度不能超过100个字符");
        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(0).WithMessage("拟录用岗位ID不能为负数");
        RuleFor(x => x.PostName)
            .MaximumLength(100).WithMessage("拟录用岗位名称长度不能超过100个字符");
        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("录用说明长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TalentOffer 验证器
// ========================================

/// <summary>
/// 更新TalentOffer DTO 验证器
/// </summary>
public class TaktTalentOfferUpdateValidator : AbstractValidator<TaktTalentOfferUpdateDto>
{
    /// <summary>
    /// 初始化 更新TalentOffer 校验规则
    /// </summary>
    public TaktTalentOfferUpdateValidator()
    {
        RuleFor(x => x.TalentOfferId)
            .GreaterThan(0).WithMessage("TalentOfferID无效");
    }
}

// ========================================
// 导入TalentOffer 验证器
// ========================================

/// <summary>
/// 导入TalentOffer DTO 验证器
/// </summary>
public class TaktTalentOfferImportValidator : AbstractValidator<TaktTalentOfferImportDto>
{
    /// <summary>
    /// 初始化 导入TalentOffer 校验规则
    /// </summary>
    public TaktTalentOfferImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.InterviewId)
            .GreaterThanOrEqualTo(0).WithMessage("面试安排ID不能为负数");
        RuleFor(x => x.OfferNo)
            .NotEmpty().WithMessage("录用编号不能为空")
            .MaximumLength(20).WithMessage("录用编号长度不能超过20个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("关联员工ID不能为负数");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("拟录用部门ID不能为负数");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("拟录用部门名称不能为空")
            .MaximumLength(100).WithMessage("拟录用部门名称长度不能超过100个字符");
        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(0).WithMessage("拟录用岗位ID不能为负数");
        RuleFor(x => x.PostName)
            .MaximumLength(100).WithMessage("拟录用岗位名称长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.PostName));
        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("录用说明长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Reason));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
