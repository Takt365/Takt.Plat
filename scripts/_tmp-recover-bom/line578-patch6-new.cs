    /// <summary>
    /// 重算并写回机种月平均材料成本（同工厂+机种+处理月份全部 BOM 行；已计算机种月平均时默认跳过）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="processedDate">处理日期</param>
    /// <param name="forceRecalculate">为 true 时忽略已计算状态并强制重算</param>
    /// <returns>是否执行了重算</returns>
    private async Task<bool> RefreshModelMonthlyAverageCostAsync(
        string plantCode,
        string modelCode,
        DateTime processedDate,
        bool forceRecalculate = false)
    {
        if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(modelCode))
        {
            return false;
        }
        EnsureThreeLayerContext();
        var periodStart = new DateTime(processedDate.Year, processedDate.Month, 1);
        var periodEnd = periodStart.AddMonths(1).AddTicks(-1);
        if (!forceRecalculate && await IsModelMonthlyAverageGroupCalculatedAsync(plantCode, modelCode, processedDate))
        {
            return false;
        }
        var periodKey = TaktBomMaterialCostLineCostHelper.ToPeriodKey(processedDate);