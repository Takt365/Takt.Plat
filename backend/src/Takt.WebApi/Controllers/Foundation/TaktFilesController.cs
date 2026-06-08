// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktFilesController.cs
// 创建时间：2026-06-08
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

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 文件控制器
/// 提供文件的 REST API
/// </summary>
[ApiModule(TaktModule.Foundation, "基础设置")]
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
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:file:import", "获取文件导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFileTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _fileService.GetFileTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入文件
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:file:import", "导入文件")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFileAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _fileService.ImportFileAsync(stream, sheetName);
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
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
}
