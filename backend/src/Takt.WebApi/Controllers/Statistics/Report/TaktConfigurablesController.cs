// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Statistics.Report
// 文件名称：TaktConfigurablesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：自定义报表主控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Statistics.Report;
using Takt.Application.Services.Code.Database;
using Takt.Application.Services.Statistics.Report;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Statistics.Report;

/// <summary>
/// 自定义报表主控制器
/// 提供自定义报表主的 REST API
/// </summary>
[ApiModule(9, "统计看板")]
[Route("api/[controller]", Name = "自定义报表主")]
public class TaktConfigurablesController : TaktControllerBase
{
    private readonly ITaktConfigurableService _configurableService;
    private readonly ITaktDatabaseInfoService _databaseInfoService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableService">自定义报表主服务</param>
    /// <param name="databaseInfoService">数据库 introspect 服务</param>
    public TaktConfigurablesController(
        ITaktConfigurableService configurableService,
        ITaktDatabaseInfoService databaseInfoService)
    {
        _configurableService = configurableService;
        _databaseInfoService = databaseInfoService;
    }

    /// <summary>
    /// 获取自定义报表主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:report:configurable:list", "自定义报表主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConfigurableListAsync([FromQuery] TaktConfigurableQueryDto queryDto)
    {
        try
        {
            var result = await _configurableService.GetConfigurableListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <returns>自定义报表主DTO</returns>
    [TaktPermission("statistics:report:configurable:query", "自定义报表主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConfigurableByIdAsync(long id)
    {
        try
        {
            var result = await _configurableService.GetConfigurableByIdAsync(id);
            if (result == null)
            {
                return NotFound("自定义报表主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取报表下拉选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:report:configurable:query", "自定义报表主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConfigurableOptionsAsync()
    {
        try
        {
            var result = await _configurableService.GetConfigurableOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建自定义报表主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>自定义报表主DTO</returns>
    [TaktPermission("statistics:report:configurable:create", "创建自定义报表主")]
    [HttpPost]
    public async Task<IActionResult> CreateConfigurableAsync([FromBody] TaktConfigurableCreateDto dto)
    {
        try
        {
            var result = await _configurableService.CreateConfigurableAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>自定义报表主DTO</returns>
    [TaktPermission("statistics:report:configurable:update", "更新自定义报表主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConfigurableAsync(long id, [FromBody] TaktConfigurableUpdateDto dto)
    {
        try
        {
            var result = await _configurableService.UpdateConfigurableAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:report:configurable:delete", "删除自定义报表主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfigurableByIdAsync(long id)
    {
        try
        {
            await _configurableService.DeleteConfigurableByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除自定义报表主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:report:configurable:delete", "批量删除自定义报表主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConfigurableBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _configurableService.DeleteConfigurableBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自定义报表主状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktCommonStatus 枚举）</param>
    /// <returns>自定义报表主DTO</returns>
    [TaktPermission("statistics:report:configurable:update", "更新自定义报表主状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateConfigurableStatusAsync([FromBody] TaktConfigurableStatusDto dto)
    {
        try
        {
            var result = await _configurableService.UpdateConfigurableStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自定义报表主排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>自定义报表主DTO</returns>
    [TaktPermission("statistics:report:configurable:update", "更新自定义报表主排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateConfigurableSortAsync([FromBody] TaktConfigurableSortDto dto)
    {
        try
        {
            var result = await _configurableService.UpdateConfigurableSortAsync(dto);
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
    [TaktPermission("statistics:report:configurable:import", "获取自定义报表主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConfigurableTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _configurableService.GetConfigurableTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入自定义报表主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("statistics:report:configurable:import", "导入自定义报表主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConfigurableAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _configurableService.ImportConfigurableAsync(stream, sheetName);
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
    /// 导出自定义报表主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:report:configurable:export", "导出自定义报表主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConfigurableAsync([FromQuery] TaktConfigurableQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _configurableService.ExportConfigurableAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取可选租户业务库列表（报表设计选库）
    /// </summary>
    /// <returns>数据库摘要列表</returns>
    [TaktPermission("statistics:report:configurable:query", "自定义报表选库")]
    [HttpGet("schema/databases")]
    public async Task<IActionResult> GetConfigurableSchemaDatabasesAsync()
    {
        try
        {
            var result = await _databaseInfoService.GetDatabaseInfoListAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取指定租户库物理表列表（报表设计选表）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>表摘要列表</returns>
    [TaktPermission("statistics:report:configurable:query", "自定义报表选表")]
    [HttpGet("schema/tables")]
    public async Task<IActionResult> GetConfigurableSchemaTablesAsync([FromQuery] string tenantCode)
    {
        try
        {
            var result = await _databaseInfoService.GetDatabaseTableInfoListAsync(tenantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取指定物理表列列表（报表设计选列）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="tableName">表名</param>
    /// <returns>列摘要列表</returns>
    [TaktPermission("statistics:report:configurable:query", "自定义报表选列")]
    [HttpGet("schema/columns")]
    public async Task<IActionResult> GetConfigurableSchemaColumnsAsync(
        [FromQuery] string tenantCode,
        [FromQuery] string tableName)
    {
        try
        {
            var result = await _databaseInfoService.GetDatabaseTableColumnInfoListAsync(tenantCode, tableName);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 SQVI 运行时筛选条件
    /// </summary>
    /// <param name="id">报表主键</param>
    /// <returns>运行时屏幕定义</returns>
    [TaktPermission("statistics:report:configurable:run", "自定义报表运行时屏幕")]
    [HttpGet("{id}/runtime-screen")]
    public async Task<IActionResult> GetConfigurableRuntimeScreenAsync(long id)
    {
        try
        {
            var result = await _configurableService.GetConfigurableRuntimeScreenAsync(id);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 执行报表查询（分页）
    /// </summary>
    /// <param name="id">报表主键</param>
    /// <param name="dto">查询参数</param>
    /// <returns>查询结果</returns>
    [TaktPermission("statistics:report:configurable:run", "执行自定义报表查询")]
    [HttpPost("{id}/query")]
    public async Task<IActionResult> ExecuteConfigurableQueryAsync(long id, [FromBody] TaktConfigurableExecuteQueryDto dto)
    {
        try
        {
            var result = await _configurableService.ExecuteConfigurableQueryAsync(id, dto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 设计态预览查询（未保存定义）
    /// </summary>
    /// <param name="dto">报表定义与分页</param>
    /// <returns>查询结果</returns>
    [TaktPermission("statistics:report:configurable:query", "自定义报表设计预览")]
    [HttpPost("preview-query")]
    public async Task<IActionResult> PreviewConfigurableQueryAsync([FromBody] TaktConfigurablePreviewQueryDto dto)
    {
        try
        {
            var result = await _configurableService.PreviewConfigurableQueryAsync(dto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出报表数据（Excel）
    /// </summary>
    /// <param name="id">报表主键</param>
    /// <param name="dto">筛选值</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("statistics:report:configurable:run", "导出自定义报表数据")]
    [HttpPost("{id}/export-data")]
    public async Task<IActionResult> ExportConfigurableDataAsync(
        long id,
        [FromBody] TaktConfigurableExportDataDto dto,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _configurableService.ExportConfigurableDataAsync(
                id,
                dto,
                sheetName,
                exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
