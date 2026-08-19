// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialZeroPriceService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 组件零价格清单服务接口（独立模块）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 组件零价格清单服务（FERT+未删除；X+F+移动平均价=0；建议代替末字母逆推）
/// </summary>
public interface ITaktBomMaterialZeroPriceService
{
    /// <summary>
    /// 查询栏工厂选项：当前公司 RelatedPlant ∩ 成本主表 PlantCode
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBomMaterialZeroPricePlantOptionsAsync();

    /// <summary>
    /// 查询栏机种选项：工厂 + 核算月 + MaterialType=FERT 下去重 ModelCode（空可选）
    /// </summary>
    /// <param name="queryDto">工厂 + FocusPeriod</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBomMaterialZeroPriceModelOptionsAsync(
        TaktBomMaterialZeroPriceModelOptionsQueryDto queryDto);

    /// <summary>
    /// 组件零价格合并清单（工厂+核算月；机种可选多选空=全部；仅主表 MaterialType=FERT 且未删除；X+F 且移动平均价=0；建议代替按末字母逆推）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页合并结果</returns>
    Task<TaktBomMaterialZeroPriceResultDto> GetBomMaterialZeroPriceListAsync(
        TaktBomMaterialZeroPriceQueryDto queryDto);

    /// <summary>
    /// 导出组件零价格合并清单
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialZeroPriceAsync(
        TaktBomMaterialZeroPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 按当前条件回填移动平均价（ComponentCode 空=批量；有值=操作列单条；明细与主表 ExtField 履历；重算产品/机种月成本）
    /// </summary>
    /// <param name="dto">工厂+核算月；组件可选；机种可选</param>
    /// <returns>回填统计</returns>
    Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> BackfillBomMaterialZeroPriceMovingAsync(
        TaktBomMaterialZeroPriceMovingBackfillDto dto);

    /// <summary>
    /// 手工替换更新零价组件移动平均价（不按机种过滤；工厂+核算月+组件全部明细；主表各机种产品/机种月成本+ExtField）
    /// </summary>
    /// <param name="dto">工厂+核算月+原组件+新组件+价/单位/币种</param>
    /// <returns>更新统计</returns>
    Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> ManualUpdateBomMaterialZeroPriceMovingAsync(
        TaktBomMaterialZeroPriceManualMovingDto dto);

    /// <summary>
    /// 将当前工厂+核算月（机种可选）范围内 BOM 展开树中 PCB SECT 整树明细写入 PcbSectIndicator=X
    /// </summary>
    /// <param name="dto">工厂+核算月；机种可选</param>
    /// <returns>打标统计</returns>
    Task<TaktBomMaterialZeroPricePcbSectMarkResultDto> MarkBomMaterialZeroPricePcbSectAsync(
        TaktBomMaterialZeroPricePcbSectMarkDto dto);
}
