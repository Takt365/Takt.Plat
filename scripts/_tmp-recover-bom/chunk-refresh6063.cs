            x => new TaktBomMaterialCost
            {
                ModelMonthlyAverageCost = average,
            });
        return true;
    }

    /// <summary>
    /// 保存/导入后：维度已计算机种月平均则复制到当前行，否则首次重算
    /// </summary>
    /// <param name="entity">已持久化的 BOM 行</param>
    /// <returns>任务</returns>
    private async Task ApplyOrRefreshModelMonthlyAverageAfterSaveAsync(TaktBomMaterialCost entity)
    {
        if (string.IsNullOrWhiteSpace(entity.PlantCode) || string.IsNullOrWhiteSpace(entity.ModelCode))
        {
            return;
        }
        var existing = await GetCalculatedModelMonthlyAverageAsync(entity.PlantCode, entity.ModelCode, entity.ProcessedDate);
        if (existing.HasValue)
        {
            if (entity.ModelMonthlyAverageCost != existing.Value)
            {
                entity.ModelMonthlyAverageCost = existing.Value;
                await _bomMaterialCostRepository.UpdateAsync(entity);
            }
            return;
        }
        await RefreshModelMonthlyAverageCostAsync(entity.PlantCode, entity.ModelCode, entity.ProcessedDate);
    }

    /// <summary>
    /// 导入批次结束后：维度已计算则补写未填行，否则首次重算
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="processedDate">处理日期</param>
    /// <returns>任务</returns>
    private async Task ApplyOrRefreshModelMonthlyAverageForGroupAsync(
        string plantCode,
        string modelCode,
        DateTime processedDate)
    {
        var existing = await GetCalculatedModelMonthlyAverageAsync(plantCode, modelCode, processedDate);
        if (existing.HasValue)
        {
            await CopyModelMonthlyAverageToUncalculatedRowsAsync(plantCode, modelCode, processedDate, existing.Value);
            return;
        }
        await RefreshModelMonthlyAverageCostAsync(plantCode, modelCode, processedDate);
    }

    /// <summary>
    /// 判断同工厂+机种+处理月份是否已计算机种月平均（任一行 ModelMonthlyAverageCost &gt; 0）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="processedDate">处理日期</param>
    /// <returns>是否已计算</returns>
    private async Task<bool> IsModelMonthlyAverageGroupCalculatedAsync(
        string plantCode,
        string modelCode,
        DateTime processedDate)
    {
        var average = await GetCalculatedModelMonthlyAverageAsync(plantCode, modelCode, processedDate);
        return average.HasValue;
    }

    /// <summary>
    /// 读取维度组内已写入的机种月平均材料成本
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="processedDate">处理日期</param>
    /// <returns>已计算的平均值；未计算时为 null</returns>
    private async Task<decimal?> GetCalculatedModelMonthlyAverageAsync(
        string plantCode,
        string modelCode,
        DateTime processedDate)
    {
        if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(modelCode))
        {
            return null;
        }
        EnsureThreeLayerContext();
        var periodStart = new DateTime(processedDate.Year, processedDate.Month, 1);
        var periodEnd = periodStart.AddMonths(1).AddTicks(-1);
        var row = await _bomMaterialCostRepository.FirstAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.ModelCode == modelCode
                && x.ProcessedDate >= periodStart
                && x.ProcessedDate <= periodEnd
                && x.BomMaterialCostStatus == 1
                && x.ModelMonthlyAverageCost > 0);
        return row == null ? null : row.ModelMonthlyAverageCost;
    }

    /// <summary>
    /// 将已计算机种月平均复制到同维度内尚未填写的 BOM 行
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="processedDate">处理日期</param>
    /// <param name="average">机种月平均材料成本</param>
    /// <returns>任务</returns>
    private async Task CopyModelMonthlyAverageToUncalculatedRowsAsync(
        string plantCode,
        string modelCode,
        DateTime processedDate,
        decimal average)
    {
        EnsureThreeLayerContext();
        var periodStart = new DateTime(processedDate.Year, processedDate.Month, 1);
        var periodEnd = periodStart.AddMonths(1).AddTicks(-1);
        await _bomMaterialCostRepository.UpdateAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.ModelCode == modelCode
                && x.ProcessedDate >= periodStart
                && x.ProcessedDate <= periodEnd
                && x.BomMaterialCostStatus == 1
                && x.ModelMonthlyAverageCost <= 0,
            x => new TaktBomMaterialCost
            {
                ModelMonthlyAverageCost = average,
            });
    }

    /// <summary>
    /// 清零同工厂+机种+处理月份下全部 BOM 行的机种月平均（强制重算前）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="processedDate">处理日期</param>
    /// <returns>任务</returns>
    private async Task ClearModelMonthlyAverageCostForGroupAsync(
        string plantCode,
        string modelCode,
        DateTime processedDate)
    {
        EnsureThreeLayerContext();
        var periodStart = new DateTime(processedDate.Year, processedDate.Month, 1);
        var periodEnd = periodStart.AddMonths(1).AddTicks(-1);
        await _bomMaterialCostRepository.UpdateAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.ModelCode == modelCode
                && x.ProcessedDate >= periodStart
                && x.ProcessedDate <= periodEnd
                && x.BomMaterialCostStatus == 1,
            x => new TaktBomMaterialCost
            {
                ModelMonthlyAverageCost = 0,
            });
    }