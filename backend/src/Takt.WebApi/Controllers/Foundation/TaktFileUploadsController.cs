// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktFileUploadsController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：文件上传运行时控制器（整文件/分片/下载；与 TaktFiles CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;
using Takt.Shared.Enums;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 文件上传运行时控制器（对应 <see cref="ITaktFileUploadService"/>）
/// </summary>
[ApiModule(TaktModule.Foundation, "文件上传")]
[Route("api/[controller]", Name = "文件上传")]
public class TaktFileUploadsController : TaktControllerBase
{
    private readonly ITaktFileUploadService _fileUploadService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fileUploadService">文件上传应用服务</param>
    public TaktFileUploadsController(ITaktFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    /// <summary>
    /// 整文件上传
    /// </summary>
    /// <param name="file">文件</param>
    /// <param name="fileDescription">描述</param>
    /// <param name="fileTags">标签</param>
    /// <param name="isPublic">是否公开</param>
    /// <returns>文件 DTO</returns>
    [TaktPermission("foundation:file:create", "上传文件")]
    [HttpPost]
    [RequestSizeLimit(524_288_000)]
    public async Task<IActionResult> UploadFileAsync(
        IFormFile file,
        [FromForm] string? fileDescription = null,
        [FromForm] string? fileTags = null,
        [FromForm] TaktFilePublicAccess? isPublic = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要上传的文件");
            }

            await using var stream = file.OpenReadStream();
            var meta = new TaktFileUploadMetaDto
            {
                FileDescription = fileDescription,
                FileTags = fileTags,
                IsPublic = isPublic,
            };
            var result = await _fileUploadService.UploadFileAsync(stream, file.FileName, file.ContentType, meta);
            return Success(result, "上传成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 检查分片是否已上传
    /// </summary>
    /// <param name="dto">检查参数</param>
    /// <returns>是否存在</returns>
    [TaktPermission("foundation:file:create", "检查上传分片")]
    [HttpPost("check")]
    public async Task<IActionResult> CheckFileChunkAsync([FromBody] TaktFileChunkCheckDto dto)
    {
        try
        {
            var result = await _fileUploadService.CheckFileChunkAsync(dto);
            return Success(result, "检查成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 上传分片
    /// </summary>
    /// <param name="file">分片数据</param>
    /// <param name="dto">分片元数据</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:file:create", "上传文件分片")]
    [HttpPost("chunk")]
    [RequestSizeLimit(524_288_000)]
    public async Task<IActionResult> UploadFileChunkAsync(IFormFile file, [FromForm] TaktFileChunkUploadDto dto)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("分片数据为空");
            }

            await using var stream = file.OpenReadStream();
            await _fileUploadService.UploadFileChunkAsync(stream, dto);
            return Success("分片上传成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 合并分片
    /// </summary>
    /// <param name="dto">合并参数</param>
    /// <returns>文件 DTO</returns>
    [TaktPermission("foundation:file:create", "合并上传分片")]
    [HttpPost("merge")]
    public async Task<IActionResult> MergeFileChunksAsync([FromBody] TaktFileChunkMergeDto dto)
    {
        try
        {
            var result = await _fileUploadService.MergeFileChunksAsync(dto);
            return Success(result, "合并成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="id">文件 ID</param>
    /// <returns>文件流</returns>
    [TaktPermission("foundation:file:query", "下载文件")]
    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadFileAsync(long id)
    {
        try
        {
            var result = await _fileUploadService.DownloadFileAsync(id);
            return File(result.Stream, result.ContentType, result.FileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文件公开范围
    /// </summary>
    /// <param name="id">文件 ID</param>
    /// <param name="dto">公开范围</param>
    /// <returns>文件 DTO</returns>
    [TaktPermission("foundation:file:update", "更新文件公开范围")]
    [HttpPut("{id}/is-public")]
    public async Task<IActionResult> ChangeFilePublicAccessAsync(long id, [FromBody] TaktFilePublicAccessDto dto)
    {
        try
        {
            var result = await _fileUploadService.ChangeFilePublicAccessAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
