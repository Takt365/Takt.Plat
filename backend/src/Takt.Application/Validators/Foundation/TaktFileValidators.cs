// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktFileValidators.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：File 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFile 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;
using Takt.Shared.Enums;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建File 验证器
// ========================================

/// <summary>
/// 创建File DTO 验证器
/// </summary>
public class TaktFileCreateValidator : AbstractValidator<TaktFileCreateDto>
{
    /// <summary>
    /// 初始化 创建File 校验规则
    /// </summary>
    public TaktFileCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(40).WithMessage("租户编码长度不能超过40个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(40).WithMessage("公司代码长度不能超过40个字符");
        RuleFor(x => x.FileCode)
            .NotEmpty().WithMessage("文件编码不能为空")
            .MaximumLength(40).WithMessage("文件编码长度不能超过40个字符");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(40).WithMessage("文件名称长度不能超过40个字符");
        RuleFor(x => x.FileOriginalName)
            .NotEmpty().WithMessage("文件原始名称不能为空")
            .MaximumLength(40).WithMessage("文件原始名称长度不能超过40个字符");
        RuleFor(x => x.FilePath)
            .NotEmpty().WithMessage("文件路径不能为空")
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符");
        RuleFor(x => x.FileType)
            .NotEmpty().WithMessage("文件 MIME 类型不能为空")
            .MaximumLength(100).WithMessage("文件 MIME 类型长度不能超过100个字符");
        RuleFor(x => x.FileExtension)
            .NotEmpty().WithMessage("文件扩展名不能为空")
            .MaximumLength(20).WithMessage("文件扩展名长度不能超过20个字符");
        RuleFor(x => x.FileHash)
            .MaximumLength(64).WithMessage("文件哈希值长度不能超过64个字符");
        RuleFor(x => x.FileCategory)
            .IsInEnum().WithMessage("文件分类无效");
        RuleFor(x => x.StorageType)
            .IsInEnum().WithMessage("存储方式无效");
        RuleFor(x => x.StorageConfig)
            .MaximumLength(1000).WithMessage("存储配置长度不能超过1000个字符");
        RuleFor(x => x.AccessUrl)
            .NotEmpty().WithMessage("访问地址不能为空")
            .MaximumLength(1000).WithMessage("访问地址长度不能超过1000个字符");
        RuleFor(x => x.FileStatus)
            .IsInEnum().WithMessage("状态无效");
        RuleFor(x => x.IsPublic)
            .IsInEnum().WithMessage("是否公开无效");
        RuleFor(x => x.FileDescription)
            .NotEmpty().WithMessage("文件描述不能为空")
            .MaximumLength(500).WithMessage("文件描述长度不能超过500个字符");
        RuleFor(x => x.FileTags)
            .NotEmpty().WithMessage("文件标签不能为空")
            .MaximumLength(200).WithMessage("文件标签长度不能超过200个字符");
        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("IP 地址不能为空")
            .MaximumLength(50).WithMessage("IP 地址长度不能超过50个字符");
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("位置不能为空")
            .MaximumLength(200).WithMessage("位置长度不能超过200个字符");
        RuleFor(x => x.ExtFieldJson)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新File 验证器
// ========================================

/// <summary>
/// 更新File DTO 验证器
/// </summary>
public class TaktFileUpdateValidator : AbstractValidator<TaktFileUpdateDto>
{
    /// <summary>
    /// 初始化 更新File 校验规则
    /// </summary>
    public TaktFileUpdateValidator()
    {
        RuleFor(x => x.FileId)
            .GreaterThan(0).WithMessage("FileID无效");
    }
}

// ========================================
// 分片上传验证器
// ========================================

/// <summary>
/// 分片存在性检查 DTO 验证器
/// </summary>
public class TaktFileChunkCheckValidator : AbstractValidator<TaktFileChunkCheckDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktFileChunkCheckValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("文件标识不能为空")
            .MaximumLength(128).WithMessage("文件标识长度不能超过128个字符");
        RuleFor(x => x.ChunkNumber)
            .GreaterThan(0).WithMessage("分片序号无效");
        RuleFor(x => x.TotalSize)
            .GreaterThanOrEqualTo(0).WithMessage("文件总大小无效");
    }
}

/// <summary>
/// 分片上传 DTO 验证器
/// </summary>
public class TaktFileChunkUploadValidator : AbstractValidator<TaktFileChunkUploadDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktFileChunkUploadValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("文件标识不能为空")
            .MaximumLength(128).WithMessage("文件标识长度不能超过128个字符");
        RuleFor(x => x.ChunkNumber)
            .GreaterThan(0).WithMessage("分片序号无效");
        RuleFor(x => x.TotalChunks)
            .GreaterThan(0).WithMessage("总分片数无效");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名不能为空")
            .MaximumLength(40).WithMessage("文件名长度不能超过40个字符");
        RuleFor(x => x.TotalSize)
            .GreaterThan(0).WithMessage("文件总大小无效");
    }
}

/// <summary>
/// 分片合并 DTO 验证器
/// </summary>
public class TaktFileChunkMergeValidator : AbstractValidator<TaktFileChunkMergeDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktFileChunkMergeValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("文件标识不能为空")
            .MaximumLength(128).WithMessage("文件标识长度不能超过128个字符");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名不能为空")
            .MaximumLength(40).WithMessage("文件名长度不能超过40个字符");
        RuleFor(x => x.TotalChunks)
            .GreaterThan(0).WithMessage("总分片数无效");
        RuleFor(x => x.TotalSize)
            .GreaterThan(0).WithMessage("文件总大小无效");
    }
}

/// <summary>
/// 已上传分片列表 DTO 验证器
/// </summary>
public class TaktFileChunkListValidator : AbstractValidator<TaktFileChunkListDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktFileChunkListValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("文件标识不能为空")
            .MaximumLength(128).WithMessage("文件标识长度不能超过128个字符");
        RuleFor(x => x.TotalChunks)
            .GreaterThanOrEqualTo(0).WithMessage("总分片数无效");
    }
}

/// <summary>
/// 取消分片上传 DTO 验证器
/// </summary>
public class TaktFileChunkCancelValidator : AbstractValidator<TaktFileChunkCancelDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktFileChunkCancelValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("文件标识不能为空")
            .MaximumLength(128).WithMessage("文件标识长度不能超过128个字符");
    }
}
