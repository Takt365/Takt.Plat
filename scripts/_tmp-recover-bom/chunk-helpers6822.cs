            .ToList();
    }

    /// <summary>
    /// 构建产品→机种查找表（型号目的地 MaterialCode 去重，SortOrder 优先）
    /// </summary>
    /// <returns>产品编码到机种编码/名称</returns>
    private async Task<Dictionary<string, (string ModelCode, string ModelName)>> BuildProductModelLookupAsync()
    {
        var list = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder,
            false);
        var lookup = new Dictionary<string, (string ModelCode, string ModelName)>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in list)
        {
            if (string.IsNullOrWhiteSpace(item.MaterialCode) || string.IsNullOrWhiteSpace(item.ModelCode))
            {
                continue;
            }
            RegisterProductModelKey(lookup, item.MaterialCode.Trim(), item.ModelCode.Trim(), item.ModelName?.Trim() ?? string.Empty);
            var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(item.MaterialCode);
            if (!string.IsNullOrWhiteSpace(normalized))
            {
                RegisterProductModelKey(lookup, normalized, item.ModelCode.Trim(), item.ModelName?.Trim() ?? string.Empty);
            }
        }
        return lookup;
    }

    /// <summary>
    /// 注册产品机种映射（保留 SortOrder 先出现的项）
    /// </summary>
    /// <param name="lookup">查找表</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="modelName">机种名称</param>
    private static void RegisterProductModelKey(
        Dictionary<string, (string ModelCode, string ModelName)> lookup,
        string productCode,
        string modelCode,
        string modelName)
    {
        if (lookup.ContainsKey(productCode))
        {
            return;
        }
        lookup[productCode] = (modelCode, modelName);
    }

    /// <summary>
    /// 按产品编码回填机种编码/名称
    /// </summary>
    /// <param name="entity">BOM 行</param>
    /// <param name="lookup">产品→机种查找表</param>
    private static void ApplyModelMetadata(
        TaktBomMaterialCost entity,
        IReadOnlyDictionary<string, (string ModelCode, string ModelName)> lookup)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(lookup);
        var productCode = entity.ProductCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(productCode))
        {
            entity.ModelCode = string.Empty;
            entity.ModelName = string.Empty;
            return;
        }
        if (lookup.TryGetValue(productCode, out var model))
        {
            entity.ModelCode = model.ModelCode;
            entity.ModelName = model.ModelName;
            return;
        }
        foreach (var (key, value) in lookup)
        {
            if (!TaktBomMaterialCostLineCostHelper.ProductCodeMatches(key, productCode))
            {
                continue;
            }
            entity.ModelCode = value.ModelCode;
            entity.ModelName = value.ModelName;
            return;
        }
        entity.ModelCode = string.Empty;
        entity.ModelName = string.Empty;
    }

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
        var catalog = await GetOrderedModelMaterialsAsync(modelCode);
        if (catalog.Count == 0)
        {
            return;
        }
        var materialCodes = await GetMaterialCodesByModelAsync(modelCode);
        var monthRows = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.ProcessedDate >= periodStart
                && x.ProcessedDate <= periodEnd
                && x.BomMaterialCostStatus == 1
                && ((x.ModelCode != null && x.ModelCode == modelCode)
                    || (x.ProductCode != null && materialCodes.Contains(x.ProductCode))));
        var catalogCodes = catalog.Select(x => x.ProductCode).ToList();
        var average = TaktBomMaterialCostModelEnrichmentHelper.ComputeModelMonthlyAverageCost(
            catalogCodes,
            monthRows,
            plantCode,
            periodKey);
        await _bomMaterialCostRepository.UpdateAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.ProcessedDate >= periodStart
                && x.ProcessedDate <= periodEnd
                && x.ModelCode == modelCode,
            x => new TaktBomMaterialCost
            {
                ModelMonthlyAverageCost = average,
            });
    }

    /// <summary>
    /// 实体变更后刷新新旧机种月平均（工厂/机种/月份变化时）
    /// </summary>
    /// <param name="previousPlantCode">变更前工厂</param>
    /// <param name="previousModelCode">变更前机种</param>
    /// <param name="previousProcessedDate">变更前处理日期</param>
    /// <param name="currentPlantCode">当前工厂</param>
    /// <param name="currentModelCode">当前机种</param>
    /// <param name="currentProcessedDate">当前处理日期</param>
    /// <returns>任务</returns>
    private async Task RefreshModelMonthlyAverageCostForEntityChangeAsync(
        string previousPlantCode,
        string previousModelCode,
        DateTime previousProcessedDate,
        string currentPlantCode,
        string currentModelCode,
        DateTime currentProcessedDate)
    {
        if (!string.IsNullOrWhiteSpace(previousModelCode))
        {
            await RefreshModelMonthlyAverageCostAsync(previousPlantCode, previousModelCode, previousProcessedDate);
        }
        if (!string.IsNullOrWhiteSpace(currentModelCode)
            && (previousPlantCode != currentPlantCode
                || previousModelCode != currentModelCode
                || previousProcessedDate.Year != currentProcessedDate.Year
                || previousProcessedDate.Month != currentProcessedDate.Month))
        {
            await RefreshModelMonthlyAverageCostAsync(currentPlantCode, currentModelCode, currentProcessedDate);
        }
    }

    /// <summary>
    /// 获取机种名称（型号目的地首条匹配）