        foreach (var (plantCode, modelCode, processedDate) in modelRefreshKeys)
        {
            await RefreshModelMonthlyAverageCostAsync(plantCode, modelCode, processedDate);
        }
        return (success, fail, errors);