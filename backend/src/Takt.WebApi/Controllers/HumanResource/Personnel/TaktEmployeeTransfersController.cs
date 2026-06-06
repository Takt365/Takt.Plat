// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeTransfersController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：员工调动控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Application.Services.HumanResource.Personnel;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Personnel;

/// <summary>
/// 员工调动控制器
/// 提供员工调动的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人事管理")]
[Route("api/[controller]", Name = "员工调动")]
public class TaktEmployeeTransfersController : TaktControllerBase
{
    private readonly ITaktEmployeeTransferService _employeeTransferService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeTransferService">员工调动服务</param>
    public TaktEmployeeTransfersController(ITaktEmployeeTransferService employeeTransferService)
    {
        _employeeTransferService = employeeTransferService;
    }

    /// <summary>
    /// 获取员工调动列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:list", "员工调动列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeTransferListAsync([FromQuery] TaktEmployeeTransferQueryDto queryDto)
    {
        try
        {
            var result = await _employeeTransferService.GetEmployeeTransferListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <returns>员工调动DTO</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:query", "员工调动详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeTransferByIdAsync(long id)
    {
        try
        {
            var result = await _employeeTransferService.GetEmployeeTransferByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工调动不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工调动选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:query", "员工调动选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeTransferOptionsAsync()
    {
        try
        {
            var result = await _employeeTransferService.GetEmployeeTransferOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工调动
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工调动DTO</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:create", "创建员工调动")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeTransferAsync([FromBody] TaktEmployeeTransferCreateDto dto)
    {
        try
        {
            var result = await _employeeTransferService.CreateEmployeeTransferAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工调动DTO</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:update", "更新员工调动")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeTransferAsync(long id, [FromBody] TaktEmployeeTransferUpdateDto dto)
    {
        try
        {
            var result = await _employeeTransferService.UpdateEmployeeTransferAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:delete", "删除员工调动")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeTransferByIdAsync(long id)
    {
        try
        {
            await _employeeTransferService.DeleteEmployeeTransferByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工调动
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:delete", "批量删除员工调动")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeTransferBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeTransferService.DeleteEmployeeTransferBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("humanresource:personnel:employeetransfer:import", "获取员工调动导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeTransferTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeTransferService.GetEmployeeTransferTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工调动
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:import", "导入员工调动")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeTransferAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeTransferService.ImportEmployeeTransferAsync(stream, sheetName);
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
    /// 导出员工调动
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:personnel:employeetransfer:export", "导出员工调动")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeTransferAsync([FromQuery] TaktEmployeeTransferQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeTransferService.ExportEmployeeTransferAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
