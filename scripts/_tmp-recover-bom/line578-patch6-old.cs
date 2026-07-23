    /// <summary>
    /// 重算并写回机种月平均材料成本（同工厂+机种+处理月份全部 BOM 行）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="processedDate">处理日期</param>
    /// <returns>任务</returns>
    private async Task RefreshModelMonthlyAverageCostAsync(string plantCode, string modelCode, DateTime processedDate)
    {
        if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(modelCode))
        {
            return;
        }
        EnsureThreeLayerContext();
        var periodStart = new DateTime(processedDate.Year, processedDate.Month, 1);
        var periodEnd = periodStart.AddMonths(1).AddTicks(-1);
        var periodKey = TaktBomMaterialCostLineCostHelper.ToPeriodKey(processedDate);