        if (refreshGroups.Count > MaxRecalculateRefreshGroups)
        {
            throw new TaktBusinessException(
                $"重算维度组数为 {refreshGroups.Count}，超过单次上限 {MaxRecalculateRefreshGroups}，请缩小查询范围。");
        }
        var refreshedGroupCount = 0;
        var skippedGroupCount = 0;
        var resetGroupCount = 0;
        foreach (var (plantCode, modelCode, year, month) in refreshGroups)
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
        };
    }