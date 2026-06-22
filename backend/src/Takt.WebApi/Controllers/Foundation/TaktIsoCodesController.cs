// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktIsoCodesController.cs
// 创建时间：2026-06-18
// 创建人：Takt365(Cursor AI)
// 功能描述：ISO编码控制器
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
/// ISO编码控制器
/// 提供ISO编码的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "ISO编码")]
public class TaktIsoCodesController : TaktControllerBase
{
    private readonly ITaktIsoCodeService _isoCodeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="isoCodeService">ISO编码服务</param>
    public TaktIsoCodesController(ITaktIsoCodeService isoCodeService)
    {
        _isoCodeService = isoCodeService;
    }

    /// <summary>
    /// 获取ISO编码列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:isocode:list", "ISO编码列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetIsoCodeListAsync([FromQuery] TaktIsoCodeQueryDto queryDto)
    {
        try
        {
            var result = await _isoCodeService.GetIsoCodeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <returns>ISO编码DTO</returns>
    [TaktPermission("foundation:isocode:query", "ISO编码详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIsoCodeByIdAsync(long id)
    {
        try
        {
            var result = await _isoCodeService.GetIsoCodeByIdAsync(id);
            if (result == null)
            {
                return NotFound("ISO编码不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取ISO编码选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:isocode:query", "ISO编码选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetIsoCodeOptionsAsync()
    {
        try
        {
            var result = await _isoCodeService.GetIsoCodeOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建ISO编码
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>ISO编码DTO</returns>
    [TaktPermission("foundation:isocode:create", "创建ISO编码")]
    [HttpPost]
    public async Task<IActionResult> CreateIsoCodeAsync([FromBody] TaktIsoCodeCreateDto dto)
    {
        try
        {
            var result = await _isoCodeService.CreateIsoCodeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>ISO编码DTO</returns>
    [TaktPermission("foundation:isocode:update", "更新ISO编码")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIsoCodeAsync(long id, [FromBody] TaktIsoCodeUpdateDto dto)
    {
        try
        {
            var result = await _isoCodeService.UpdateIsoCodeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:isocode:delete", "删除ISO编码")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIsoCodeByIdAsync(long id)
    {
        try
        {
            await _isoCodeService.DeleteIsoCodeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除ISO编码
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:isocode:delete", "批量删除ISO编码")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteIsoCodeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _isoCodeService.DeleteIsoCodeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新ISO编码状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>ISO编码DTO</returns>
    [TaktPermission("foundation:isocode:update", "更新ISO编码状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateIsoCodeStatusAsync([FromBody] TaktIsoCodeStatusDto dto)
    {
        try
        {
            var result = await _isoCodeService.UpdateIsoCodeStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新ISO编码排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>ISO编码DTO</returns>
    [TaktPermission("foundation:isocode:update", "更新ISO编码排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateIsoCodeSortAsync([FromBody] TaktIsoCodeSortDto dto)
    {
        try
        {
            var result = await _isoCodeService.UpdateIsoCodeSortAsync(dto);
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
    [TaktPermission("foundation:isocode:import", "获取ISO编码导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetIsoCodeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _isoCodeService.GetIsoCodeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入ISO编码
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:isocode:import", "导入ISO编码")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportIsoCodeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _isoCodeService.ImportIsoCodeAsync(stream, sheetName);
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
    /// 导出ISO编码
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:isocode:export", "导出ISO编码")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportIsoCodeAsync([FromQuery] TaktIsoCodeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _isoCodeService.ExportIsoCodeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
