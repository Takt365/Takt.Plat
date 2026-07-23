        entity = await _bomMaterialCostRepository.CreateAsync(entity);
        if (!string.IsNullOrWhiteSpace(entity.ModelCode))
        {
            await RefreshModelMonthlyAverageCostAsync(entity.PlantCode, entity.ModelCode, entity.ProcessedDate);
        }
        return await GetBomMaterialCostByIdAsync(entity.Id) ?? entity.Adapt<TaktBomMaterialCostDto>();