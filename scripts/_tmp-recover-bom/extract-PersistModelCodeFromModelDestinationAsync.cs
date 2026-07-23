    /// <summary>
    /// 按查询条件批量重算并回填机种月平均材料成本（维度：工厂+机种+处理月份；须指定处理月份范围，逐月计算）
    /// </summary>
    /// <param name="queryDto">与列表相同的筛选条件（须含 ProcessedDateStart/End；忽略分页）</param>
    /// <param name="forceRecalculate">为 true 时先清零再重算</param>
    /// <returns>重算统计</returns>
    public async Task<TaktBomMaterialCostRecalculateModelAverageResultDto> RecalculateBomMaterialCostModelMonthlyAverageAsync(
        TaktBomMaterialCostQueryDto queryDto,
        bool forceRecalculate = false)
    {
        queryDto ??= new TaktBomMaterialCostQueryDto();
        EnsureThreeLayerContext();
        if (!queryDto.BomMaterialCostStatus.HasValue)
        {
            queryDto.BomMaterialCostStatus = 1;
        }
        var normalizedQuery = NormalizeQueryToProcessedMonthBoundaries(queryDto);
        if (!normalizedQuery.ProcessedDateStart.HasValue || !normalizedQuery.ProcessedDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择处理月份范围后再重算");
        }
        var monthSequence = BuildRecalculateMonthSequenceFromQuery(normalizedQuery);
        if (monthSequence.Count == 0)
        {
            return new TaktBomMaterialCostRecalculateModelAverageResultDto();
        }
        var lookup = await BuildProductModelLookupAsync();
        var scannedRowCount = 0;
        var patchedModelCodeRowCount = 0;
        var refreshedGroupCount = 0;
        var skippedGroupCount = 0;
        var resetGroupCount = 0;
        foreach (var (year, month) in monthSequence)
        {
            var periodStart = new DateTime(year, month, 1);
            var periodEnd = ToProcessedMonthEnd(periodStart);
            var monthScopeQuery = BuildMonthScopeQueryForModelPatch(normalizedQuery, periodStart, periodEnd);
            var predicate = QueryExpression(monthScopeQuery);
            var monthRowCount = await _bomMaterialCostRepository.CountAsync(predicate);
            if (monthRowCount == 0)
            {
                continue;
            }
            if (monthRowCount > MaxRecalculateScanRowsPerMonth)
            {
                throw new TaktBusinessException(
                    $"{year}-{month:D2} 月 BOM 行数为 {monthRowCount}，超过单月重算上限 {MaxRecalculateScanRowsPerMonth}，请缩小筛选范围（如指定工厂/产品）。");
            }
            var monthRows = await _bomMaterialCostRepository.GetListForExportAsync(predicate, MaxRecalculateScanRowsPerMonth);
            scannedRowCount += monthRows.Count;
            patchedModelCodeRowCount += await PersistModelCodeFromModelDestinationAsync(monthRows, lookup);
            var singleMonthSequence = new List<(int Year, int Month)> { (year, month) };
            var refreshGroups = BuildRecalculateRefreshGroups(normalizedQuery, monthRows, singleMonthSequence);
            if (refreshGroups.Count > MaxRecalculateRefreshGroups)
            {
                throw new TaktBusinessException(
                    $"{year}-{month:D2} 月重算维度组数为 {refreshGroups.Count}，超过单月上限 {MaxRecalculateRefreshGroups}，请缩小筛选范围。");
            }
            foreach (var (plantCode, modelCode, groupYear, groupMonth) in refreshGroups
                         .OrderBy(g => g.PlantCode)
                         .ThenBy(g => g.ModelCode)
                         .ThenBy(g => g.Year)
                         .ThenBy(g => g.Month))
            {
                var processedDate = new DateTime(groupYear, groupMonth, 1);
                if (forceRecalculate)
                {
                    await ClearModelMonthlyAverageCostForGroupAsync(plantCode, modelCode, processedDate);
                    resetGroupCount += 1;
                    if (await RefreshModelMonthlyAverageCostAsync(plantCode, modelCode, processedDate, forceRecalculate: true))
                    {
                        refreshedGroupCount += 1;
                    }
                    continue;
                }
                if (await IsModelMonthlyAverageGroupCalculatedAsync(plantCode, modelCode, processedDate))
                {
                    skippedGroupCount += 1;
                    continue;
                }
                if (await RefreshModelMonthlyAverageCostAsync(plantCode, modelCode, processedDate))
                {
                    refreshedGroupCount += 1;
                }
            }
        }
        return new TaktBomMaterialCostRecalculateModelAverageResultDto
        {
            ScannedRowCount = scannedRowCount,
            PatchedModelCodeRowCount = patchedModelCodeRowCount,
            RefreshedGroupCount = refreshedGroupCount,
            SkippedGroupCount = skippedGroupCount,
            ResetGroupCount = resetGroupCount,
            ProcessedMonthCount = monthSequence.Count,
        };
    }