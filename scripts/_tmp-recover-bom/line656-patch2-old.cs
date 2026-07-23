    /// <summary>
    /// 按查询条件批量重算并回填机种月平均材料成本（维度：工厂+机种+处理月份；逐月计算）
    /// </summary>
    /// <param name="queryDto">与列表相同的筛选条件（忽略分页）</param>
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
        var predicate = QueryExpression(normalizedQuery);
        var totalMatched = await _bomMaterialCostRepository.CountAsync(predicate);
        if (totalMatched == 0)
        {
            return new TaktBomMaterialCostRecalculateModelAverageResultDto();
        }
        if (totalMatched > MaxRecalculateScanRows)
        {
            throw new TaktBusinessException(
                $"符合条件的记录数为 {totalMatched}，超过单次重算上限 {MaxRecalculateScanRows}，请缩小查询范围。");
        }
        var rows = await _bomMaterialCostRepository.GetListForExportAsync(predicate, MaxRecalculateScanRows);
        var monthSequence = BuildRecalculateMonthSequence(normalizedQuery, rows);
        var patchedModelCodeRowCount = await PatchModelCodeFromModelDestinationForMonthsAsync(normalizedQuery, monthSequence);
        rows = await _bomMaterialCostRepository.GetListForExportAsync(predicate, MaxRecalculateScanRows);
        var refreshGroups = BuildRecalculateRefreshGroups(normalizedQuery, rows, monthSequence);
        if (refreshGroups.Count > MaxRecalculateRefreshGroups)
        {
            throw new TaktBusinessException(
                $"重算维度组数为 {refreshGroups.Count}，超过单次上限 {MaxRecalculateRefreshGroups}，请缩小查询范围。");
        }
        var refreshedGroupCount = 0;
        var skippedGroupCount = 0;
        var resetGroupCount = 0;
        foreach (var (plantCode, modelCode, year, month) in refreshGroups.OrderBy(g => g.PlantCode).ThenBy(g => g.ModelCode).ThenBy(g => g.Year).ThenBy(g => g.Month))
        {
            var processedDate = new DateTime(year, month, 1);
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
        return new TaktBomMaterialCostRecalculateModelAverageResultDto
        {
            ScannedRowCount = rows.Count,
            PatchedModelCodeRowCount = patchedModelCodeRowCount,
            RefreshedGroupCount = refreshedGroupCount,
            SkippedGroupCount = skippedGroupCount,
            ResetGroupCount = resetGroupCount,
            ProcessedMonthCount = monthSequence.Count,
        };
    }