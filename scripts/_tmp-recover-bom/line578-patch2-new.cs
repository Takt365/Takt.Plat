        entity = await _bomMaterialCostRepository.CreateAsync(entity);
        if (!string.IsNullOrWhiteSpace(entity.ModelCode))
        {
            await ApplyOrRefreshModelMonthlyAverageAfterSaveAsync(entity);
        }
        return await GetBomMaterialCostByIdAsync(entity.Id) ?? entity.Adapt<TaktBomMaterialCostDto>();