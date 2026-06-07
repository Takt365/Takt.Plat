// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktPackagingsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：物料包装信息控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// 物料包装信息控制器
/// 提供物料包装信息的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "物料包装信息")]
public class TaktPackagingsController : TaktControllerBase
{
    private readonly ITaktPackagingService _packagingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="packagingService">物料包装信息服务</param>
    public TaktPackagingsController(ITaktPackagingService packagingService)
    {
        _packagingService = packagingService;
    }

    /// <summary>
    /// 获取物料包装信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:list", "物料包装信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPackagingListAsync([FromQuery] TaktPackagingQueryDto queryDto)
    {
        try
        {
            var result = await _packagingService.GetPackagingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <returns>物料包装信息DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:query", "物料包装信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPackagingByIdAsync(long id)
    {
        try
        {
            var result = await _packagingService.GetPackagingByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料包装信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料包装信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:query", "物料包装信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPackagingOptionsAsync()
    {
        try
        {
            var result = await _packagingService.GetPackagingOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料包装信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料包装信息DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:create", "创建物料包装信息")]
    [HttpPost]
    public async Task<IActionResult> CreatePackagingAsync([FromBody] TaktPackagingCreateDto dto)
    {
        try
        {
            var result = await _packagingService.CreatePackagingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料包装信息DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:update", "更新物料包装信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackagingAsync(long id, [FromBody] TaktPackagingUpdateDto dto)
    {
        try
        {
            var result = await _packagingService.UpdatePackagingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:delete", "删除物料包装信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackagingByIdAsync(long id)
    {
        try
        {
            await _packagingService.DeletePackagingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料包装信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:delete", "批量删除物料包装信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePackagingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _packagingService.DeletePackagingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料包装信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>物料包装信息DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:update", "更新物料包装信息排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePackagingSortAsync([FromBody] TaktPackagingSortDto dto)
    {
        try
        {
            var result = await _packagingService.UpdatePackagingSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:bom:packaging:import", "获取物料包装信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPackagingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _packagingService.GetPackagingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料包装信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:import", "导入物料包装信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPackagingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _packagingService.ImportPackagingAsync(stream, sheetName);
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
    /// 导出物料包装信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:packaging:export", "导出物料包装信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPackagingAsync([FromQuery] TaktPackagingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _packagingService.ExportPackagingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
