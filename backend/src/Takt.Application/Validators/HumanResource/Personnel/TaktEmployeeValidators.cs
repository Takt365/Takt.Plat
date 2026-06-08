// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.HumanResource.Personnel
// 文件名称：TaktEmployeeValidators.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：Employee 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployee 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.HumanResource.Personnel;

// ========================================
// 创建Employee 验证器
// ========================================

/// <summary>
/// 创建Employee DTO 验证器
/// </summary>
public class TaktEmployeeCreateValidator : AbstractValidator<TaktEmployeeCreateDto>
{
    /// <summary>
    /// 初始化 创建Employee 校验规则
    /// </summary>
    public TaktEmployeeCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeNo)
            .NotEmpty().WithMessage("员工编号不能为空")
            .MaximumLength(6).WithMessage("员工编号长度不能超过6个字符");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("姓名不能为空")
            .MaximumLength(50).WithMessage("姓名长度不能超过50个字符");
        RuleFor(x => x.IdCardNo)
            .MaximumLength(18).WithMessage("身份证号长度不能超过18个字符");
        RuleFor(x => x.Mobile)
            .MaximumLength(11).WithMessage("手机号码长度不能超过11个字符");
        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("电子邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("电子邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.NativePlace)
            .MaximumLength(100).WithMessage("籍贯长度不能超过100个字符");
        RuleFor(x => x.Ethnicity)
            .MaximumLength(20).WithMessage("民族长度不能超过20个字符");
        RuleFor(x => x.PoliticalStatus)
            .MaximumLength(20).WithMessage("政治面貌长度不能超过20个字符");
        RuleFor(x => x.GraduateSchool)
            .MaximumLength(100).WithMessage("毕业院校长度不能超过100个字符");
        RuleFor(x => x.Major)
            .MaximumLength(50).WithMessage("专业长度不能超过50个字符");
        RuleFor(x => x.ResignationReason)
            .MaximumLength(500).WithMessage("离职原因长度不能超过500个字符");
        RuleFor(x => x.PrimaryDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("当前主部门ID不能为负数");
        RuleFor(x => x.PrimaryPostId)
            .GreaterThanOrEqualTo(0).WithMessage("当前主岗位ID不能为负数");
        RuleFor(x => x.IsBuiltIn)
            .IsInEnum().WithMessage("是否内置无效");
        RuleFor(x => x.EmergencyContactName)
            .MaximumLength(50).WithMessage("紧急联系人姓名长度不能超过50个字符");
        RuleFor(x => x.EmergencyContactPhone)
            .MaximumLength(20).WithMessage("紧急联系人电话长度不能超过20个字符");
        RuleFor(x => x.HomeAddress)
            .MaximumLength(500).WithMessage("家庭住址长度不能超过500个字符");
        RuleFor(x => x.PhotoUrl)
            .MaximumLength(500).WithMessage("照片URL长度不能超过500个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Employee 验证器
// ========================================

/// <summary>
/// 更新Employee DTO 验证器
/// </summary>
public class TaktEmployeeUpdateValidator : AbstractValidator<TaktEmployeeUpdateDto>
{
    /// <summary>
    /// 初始化 更新Employee 校验规则
    /// </summary>
    public TaktEmployeeUpdateValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("EmployeeID无效");
    }
}

// ========================================
// 导入Employee 验证器
// ========================================

/// <summary>
/// 导入Employee DTO 验证器
/// </summary>
public class TaktEmployeeImportValidator : AbstractValidator<TaktEmployeeImportDto>
{
    /// <summary>
    /// 初始化 导入Employee 校验规则
    /// </summary>
    public TaktEmployeeImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.EmployeeNo)
            .NotEmpty().WithMessage("员工编号不能为空")
            .MaximumLength(6).WithMessage("员工编号长度不能超过6个字符");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("姓名不能为空")
            .MaximumLength(50).WithMessage("姓名长度不能超过50个字符");
        RuleFor(x => x.IdCardNo)
            .MaximumLength(18).WithMessage("身份证号长度不能超过18个字符").When(x => !string.IsNullOrWhiteSpace(x.IdCardNo));
        RuleFor(x => x.Mobile)
            .MaximumLength(11).WithMessage("手机号码长度不能超过11个字符").When(x => !string.IsNullOrWhiteSpace(x.Mobile));
        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("电子邮箱长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.Email))
            .EmailAddress().WithMessage("电子邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.NativePlace)
            .MaximumLength(100).WithMessage("籍贯长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.NativePlace));
        RuleFor(x => x.Ethnicity)
            .MaximumLength(20).WithMessage("民族长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.Ethnicity));
        RuleFor(x => x.PoliticalStatus)
            .MaximumLength(20).WithMessage("政治面貌长度不能超过20个字符").When(x => !string.IsNullOrWhiteSpace(x.PoliticalStatus));
        RuleFor(x => x.GraduateSchool)
            .MaximumLength(100).WithMessage("毕业院校长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.GraduateSchool));
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
