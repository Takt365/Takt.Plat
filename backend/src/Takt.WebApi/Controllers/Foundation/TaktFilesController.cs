// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktFilesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：文件控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 文件控制器
/// 提供文件 CRUD、上传/分片/下载及导出 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "文件")]
public class TaktFilesController : TaktControllerBase
{
    private readonly ITaktFileService _fileService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fileService">文件服务</param>
    public TaktFilesController(ITaktFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// 获取文件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:file:list", "文件列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFileListAsync([FromQuery] TaktFileQueryDto queryDto)
    {
        try
        {
            var result = await _fileService.GetFileListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:query", "文件详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileByIdAsync(long id)
    {
        try
        {
            var result = await _fileService.GetFileByIdAsync(id);
            if (result == null)
            {
                return NotFound("文件不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取文件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:file:query", "文件选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFileOptionsAsync()
    {
        try
        {
            var result = await _fileService.GetFileOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:create", "创建文件")]
    [HttpPost]
    public async Task<IActionResult> CreateFileAsync([FromBody] TaktFileCreateDto dto)
    {
        try
        {
            var result = await _fileService.CreateFileAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:update", "更新文件")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFileAsync(long id, [FromBody] TaktFileUpdateDto dto)
    {
        try
        {
            var result = await _fileService.UpdateFileAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:file:delete", "删除文件")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFileByIdAsync(long id)
    {
        try
        {
            await _fileService.DeleteFileByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除文件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:file:delete", "批量删除文件")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFileBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _fileService.DeleteFileBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文件状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktCommonStatus 枚举）</param>
    /// <returns>文件DTO</returns>
    [TaktPermission("foundation:file:update", "更新文件状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFileStatusAsync([FromBody] TaktFileStatusDto dto)
    {
        try
        {
            var result = await _fileService.UpdateFileStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出文件
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:file:export", "导出文件")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFileAsync([FromQuery] TaktFileQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _fileService.ExportFileAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 整文件上传
    /// </summary>
    /// <param name="file">文件</param>
    /// <param name="fileDescription">描述</param>
    /// <param name="fileTags">标签</param>
    /// <param name="isPublic">是否公开</param>
    /// <returns>文件 DTO</returns>
    [TaktPermission("foundation:file:upload", "上传文件")]
    [HttpPost("upload")]
    [RequestSizeLimit(524_288_000)]
    public async Task<IActionResult> UploadFileAsync(
        IFormFile file,
        [FromForm] string? fileDescription = null,
        [FromForm] string? fileTags = null,
        [FromForm] int? isPublic = null,
        [FromForm] TaktFileUploadType fileUploadType = TaktFileUploadType.Normal,
        [FromForm] string? targetFileName = null,
        [FromForm] string? categoryPath = null)
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
                FileUploadType = fileUploadType,
                TargetFileName = targetFileName,
                CategoryPath = categoryPath,
            };
            var result = await _fileService.UploadFileAsync(stream, file.FileName, file.ContentType, meta);
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
    [TaktPermission("foundation:file:check", "检查上传分片")]
    [HttpPost("check")]
    public async Task<IActionResult> CheckFileChunkAsync([FromBody] TaktFileChunkCheckDto dto)
    {
        try
        {
            var result = await _fileService.CheckFileChunkAsync(dto);
            return Success(result, "检查成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 列出已上传分片（断点续传）
    /// </summary>
    /// <param name="dto">查询参数</param>
    /// <returns>已上传分片序号</returns>
    [TaktPermission("foundation:file:check", "查询已上传分片")]
    [HttpPost("chunk-list")]
    public async Task<IActionResult> ListFileChunksAsync([FromBody] TaktFileChunkListDto dto)
    {
        try
        {
            var result = await _fileService.ListFileChunksAsync(dto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 取消分片上传
    /// </summary>
    /// <param name="dto">取消参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:file:chunk", "取消分片上传")]
    [HttpDelete("chunk")]
    public async Task<IActionResult> CancelFileChunksAsync([FromBody] TaktFileChunkCancelDto dto)
    {
        try
        {
            await _fileService.CancelFileChunksAsync(dto);
            return Success("已取消分片上传");
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
    [TaktPermission("foundation:file:chunk", "上传文件分片")]
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
            await _fileService.UploadFileChunkAsync(stream, dto);
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
    [TaktPermission("foundation:file:merge", "合并上传分片")]
    [HttpPost("merge")]
    public async Task<IActionResult> MergeFileChunksAsync([FromBody] TaktFileChunkMergeDto dto)
    {
        try
        {
            var result = await _fileService.MergeFileChunksAsync(dto);
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
    [TaktPermission("foundation:file:download", "下载文件")]
    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadFileAsync(long id)
    {
        try
        {
            var result = await _fileService.DownloadFileAsync(id);
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
            var result = await _fileService.ChangeFilePublicAccessAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
