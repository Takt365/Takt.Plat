// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktInventoryImpairmentProvisionsController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：存货跌价准备控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 存货跌价准备控制器
/// 提供存货跌价准备的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "存货跌价准备")]
public class TaktInventoryImpairmentProvisionsController : TaktControllerBase
{
    private readonly ITaktInventoryImpairmentProvisionService _inventoryImpairmentProvisionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="inventoryImpairmentProvisionService">存货跌价准备服务</param>
    public TaktInventoryImpairmentProvisionsController(ITaktInventoryImpairmentProvisionService inventoryImpairmentProvisionService)
    {
        _inventoryImpairmentProvisionService = inventoryImpairmentProvisionService;
    }

    /// <summary>
    /// 获取存货跌价准备列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:list", "存货跌价准备列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetInventoryImpairmentProvisionListAsync([FromQuery] TaktInventoryImpairmentProvisionQueryDto queryDto)
    {
        try
        {
            var result = await _inventoryImpairmentProvisionService.GetInventoryImpairmentProvisionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:query", "存货跌价准备详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInventoryImpairmentProvisionByIdAsync(long id)
    {
        try
        {
            var result = await _inventoryImpairmentProvisionService.GetInventoryImpairmentProvisionByIdAsync(id);
            if (result == null)
            {
                return NotFound("存货跌价准备不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取存货跌价准备选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:query", "存货跌价准备选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetInventoryImpairmentProvisionOptionsAsync()
    {
        try
        {
            var result = await _inventoryImpairmentProvisionService.GetInventoryImpairmentProvisionOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建存货跌价准备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:create", "创建存货跌价准备")]
    [HttpPost]
    public async Task<IActionResult> CreateInventoryImpairmentProvisionAsync([FromBody] TaktInventoryImpairmentProvisionCreateDto dto)
    {
        try
        {
            var result = await _inventoryImpairmentProvisionService.CreateInventoryImpairmentProvisionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:update", "更新存货跌价准备")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventoryImpairmentProvisionAsync(long id, [FromBody] TaktInventoryImpairmentProvisionUpdateDto dto)
    {
        try
        {
            var result = await _inventoryImpairmentProvisionService.UpdateInventoryImpairmentProvisionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:delete", "删除存货跌价准备")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventoryImpairmentProvisionByIdAsync(long id)
    {
        try
        {
            await _inventoryImpairmentProvisionService.DeleteInventoryImpairmentProvisionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除存货跌价准备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:delete", "批量删除存货跌价准备")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteInventoryImpairmentProvisionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _inventoryImpairmentProvisionService.DeleteInventoryImpairmentProvisionBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新存货跌价准备状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:update", "更新存货跌价准备状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateInventoryImpairmentProvisionStatusAsync([FromBody] TaktInventoryImpairmentProvisionStatusDto dto)
    {
        try
        {
            var result = await _inventoryImpairmentProvisionService.UpdateInventoryImpairmentProvisionStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新存货跌价准备排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:update", "更新存货跌价准备排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateInventoryImpairmentProvisionSortAsync([FromBody] TaktInventoryImpairmentProvisionSortDto dto)
    {
        try
        {
            var result = await _inventoryImpairmentProvisionService.UpdateInventoryImpairmentProvisionSortAsync(dto);
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
    [TaktPermission("logistics:materials:inventory:impairment:provision:import", "获取存货跌价准备导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetInventoryImpairmentProvisionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _inventoryImpairmentProvisionService.GetInventoryImpairmentProvisionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入存货跌价准备
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:import", "导入存货跌价准备")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportInventoryImpairmentProvisionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _inventoryImpairmentProvisionService.ImportInventoryImpairmentProvisionAsync(stream, sheetName);
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
    /// 导出存货跌价准备
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:inventory:impairment:provision:export", "导出存货跌价准备")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportInventoryImpairmentProvisionAsync([FromQuery] TaktInventoryImpairmentProvisionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _inventoryImpairmentProvisionService.ExportInventoryImpairmentProvisionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
