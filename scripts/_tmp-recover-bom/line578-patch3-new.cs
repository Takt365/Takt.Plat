        foreach (var (plantCode, modelCode, processedDate) in modelRefreshKeys)
        {
            await ApplyOrRefreshModelMonthlyAverageForGroupAsync(plantCode, modelCode, processedDate);
        }
        return (success, fail, errors);