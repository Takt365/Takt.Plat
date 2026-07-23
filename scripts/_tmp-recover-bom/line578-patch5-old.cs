        if (refreshGroups.Count > MaxRecalculateRefreshGroups)
        {
            throw new TaktBusinessException(
                $"重算维度组数为 {refreshGroups.Count}，超过单次上限 {MaxRecalculateRefreshGroups}，请缩小查询范围。");
        }
        foreach (var (plantCode, modelCode, year, month) in refreshGroups)
        {
            await RefreshModelMonthlyAverageCostAsync(plantCode, modelCode, new DateTime(year, month, 1));
        }
        return new TaktBomMaterialCostRecalculateModelAverageResultDto
        {
            ScannedRowCount = rows.Count,
            PatchedModelCodeRowCount = patchedModelCodeRowCount,
            RefreshedGroupCount = refreshGroups.Count,
        };
    }