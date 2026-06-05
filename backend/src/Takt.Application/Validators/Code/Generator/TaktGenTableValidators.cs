// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Code.Generator
// 文件名称：TaktGenTableValidators.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：GenTable 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktGenTable 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Generator;

namespace Takt.Application.Validators.Code.Generator;

// ========================================
// 创建GenTable 验证器
// ========================================

/// <summary>
/// 创建GenTable DTO 验证器
/// </summary>
public class TaktGenTableCreateValidator : AbstractValidator<TaktGenTableCreateDto>
{
    /// <summary>
    /// 初始化 创建GenTable 校验规则
    /// </summary>
    public TaktGenTableCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.DataSource)
            .NotEmpty().WithMessage("数据源不能为空")
            .MaximumLength(200).WithMessage("数据源长度不能超过200个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("数据表名称不能为空")
            .MaximumLength(200).WithMessage("数据表名称长度不能超过200个字符");
        RuleFor(x => x.TableComment)
            .MaximumLength(500).WithMessage("表描述长度不能超过500个字符");
        RuleFor(x => x.SubTableName)
            .MaximumLength(100).WithMessage("关联父表名长度不能超过100个字符");
        RuleFor(x => x.SubTableFkName)
            .MaximumLength(100).WithMessage("本表关联父表的外键名长度不能超过100个字符");
        RuleFor(x => x.TreeCode)
            .MaximumLength(50).WithMessage("树编码字段长度不能超过50个字符");
        RuleFor(x => x.TreeParentCode)
            .MaximumLength(50).WithMessage("树父编码字段长度不能超过50个字符");
        RuleFor(x => x.TreeName)
            .MaximumLength(50).WithMessage("树名称字段长度不能超过50个字符");
        RuleFor(x => x.GenTemplateCategory)
            .NotEmpty().WithMessage("生成模板类型不能为空")
            .MaximumLength(50).WithMessage("生成模板类型长度不能超过50个字符");
        RuleFor(x => x.GenModuleName)
            .MaximumLength(50).WithMessage("模块名长度不能超过50个字符");
        RuleFor(x => x.GenBusinessName)
            .NotEmpty().WithMessage("业务名不能为空")
            .MaximumLength(50).WithMessage("业务名长度不能超过50个字符");
        RuleFor(x => x.GenFunctionName)
            .MaximumLength(50).WithMessage("功能名长度不能超过50个字符");
        RuleFor(x => x.PermsPrefix)
            .NotEmpty().WithMessage("权限前缀不能为空")
            .MaximumLength(100).WithMessage("权限前缀长度不能超过100个字符");
        RuleFor(x => x.MenuButtonGroup)
            .MaximumLength(500).WithMessage("菜单权限组长度不能超过500个字符");
        RuleFor(x => x.NamePrefix)
            .MaximumLength(50).WithMessage("命名空间前缀长度不能超过50个字符");
        RuleFor(x => x.EntityNamespace)
            .MaximumLength(200).WithMessage("实体命名空间长度不能超过200个字符");
        RuleFor(x => x.EntityClassName)
            .NotEmpty().WithMessage("实体类名称不能为空")
            .MaximumLength(100).WithMessage("实体类名称长度不能超过100个字符");
        RuleFor(x => x.DtoNamespace)
            .MaximumLength(200).WithMessage("传输对象Dto命名空间长度不能超过200个字符");
        RuleFor(x => x.DtoClassName)
            .MaximumLength(100).WithMessage("传输对象 Dto 类名长度不能超过100个字符");
        RuleFor(x => x.ServiceNamespace)
            .MaximumLength(200).WithMessage("服务命名空间长度不能超过200个字符");
        RuleFor(x => x.IServiceClassName)
            .MaximumLength(100).WithMessage("服务接口类名称长度不能超过100个字符");
        RuleFor(x => x.ServiceClassName)
            .MaximumLength(100).WithMessage("服务类名称长度不能超过100个字符");
        RuleFor(x => x.ControllerNamespace)
            .MaximumLength(200).WithMessage("控制器命名空间长度不能超过200个字符");
        RuleFor(x => x.ControllerClassName)
            .MaximumLength(100).WithMessage("控制器类名称长度不能超过100个字符");
        RuleFor(x => x.RepositoryInterfaceNamespace)
            .MaximumLength(200).WithMessage("仓储接口命名空间长度不能超过200个字符");
        RuleFor(x => x.IRepositoryClassName)
            .MaximumLength(100).WithMessage("仓储接口类名称长度不能超过100个字符");
        RuleFor(x => x.RepositoryNamespace)
            .MaximumLength(200).WithMessage("仓储命名空间长度不能超过200个字符");
        RuleFor(x => x.RepositoryClassName)
            .MaximumLength(100).WithMessage("仓储类名称长度不能超过100个字符");
        RuleFor(x => x.GenFunction)
            .MaximumLength(500).WithMessage("生成功能，JSON 格式。对象形式长度不能超过500个字符");
        RuleFor(x => x.GenPath)
            .NotEmpty().WithMessage("生成路径不能为空")
            .MaximumLength(500).WithMessage("生成路径长度不能超过500个字符");
        RuleFor(x => x.ParentMenuId)
            .GreaterThanOrEqualTo(0).WithMessage("上级菜单ID不能为负数");
        RuleFor(x => x.SortField)
            .NotEmpty().WithMessage("排序字段不能为空")
            .MaximumLength(100).WithMessage("排序字段长度不能超过100个字符");
        RuleFor(x => x.SortType)
            .NotEmpty().WithMessage("排序类型不能为空")
            .MaximumLength(10).WithMessage("排序类型长度不能超过10个字符");
        RuleFor(x => x.GenAuthor)
            .NotEmpty().WithMessage("作者不能为空")
            .MaximumLength(50).WithMessage("作者长度不能超过50个字符");
        RuleFor(x => x.OtherGenOptions)
            .MaximumLength(2000).WithMessage("其他生成选项长度不能超过2000个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新GenTable 验证器
// ========================================

/// <summary>
/// 更新GenTable DTO 验证器
/// </summary>
public class TaktGenTableUpdateValidator : AbstractValidator<TaktGenTableUpdateDto>
{
    /// <summary>
    /// 初始化 更新GenTable 校验规则
    /// </summary>
    public TaktGenTableUpdateValidator()
    {
        RuleFor(x => x.GenTableId)
            .GreaterThan(0).WithMessage("GenTableID无效");
    }
}

// ========================================
// 导入GenTable 验证器
// ========================================

/// <summary>
/// 导入GenTable DTO 验证器
/// </summary>
public class TaktGenTableImportValidator : AbstractValidator<TaktGenTableImportDto>
{
    /// <summary>
    /// 初始化 导入GenTable 校验规则
    /// </summary>
    public TaktGenTableImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.DataSource)
            .NotEmpty().WithMessage("数据源不能为空")
            .MaximumLength(200).WithMessage("数据源长度不能超过200个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("数据表名称不能为空")
            .MaximumLength(200).WithMessage("数据表名称长度不能超过200个字符");
        RuleFor(x => x.TableComment)
            .MaximumLength(500).WithMessage("表描述长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.TableComment));
        RuleFor(x => x.SubTableName)
            .MaximumLength(100).WithMessage("关联父表名长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.SubTableName));
        RuleFor(x => x.SubTableFkName)
            .MaximumLength(100).WithMessage("本表关联父表的外键名长度不能超过100个字符").When(x => !string.IsNullOrWhiteSpace(x.SubTableFkName));
        RuleFor(x => x.TreeCode)
            .MaximumLength(50).WithMessage("树编码字段长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TreeCode));
        RuleFor(x => x.TreeParentCode)
            .MaximumLength(50).WithMessage("树父编码字段长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TreeParentCode));
        RuleFor(x => x.TreeName)
            .MaximumLength(50).WithMessage("树名称字段长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.TreeName));
        RuleFor(x => x.GenTemplateCategory)
            .NotEmpty().WithMessage("生成模板类型不能为空")
            .MaximumLength(50).WithMessage("生成模板类型长度不能超过50个字符");
        RuleFor(x => x.GenModuleName)
            .MaximumLength(50).WithMessage("模块名长度不能超过50个字符").When(x => !string.IsNullOrWhiteSpace(x.GenModuleName));
        RuleFor(x => x.GenBusinessName)
            .NotEmpty().WithMessage("业务名不能为空")
            .MaximumLength(50).WithMessage("业务名长度不能超过50个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtFieldJson));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
