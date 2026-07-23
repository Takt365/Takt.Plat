    public async Task<TaktBomMaterialCostDto> CreateBomMaterialCostAsync(TaktBomMaterialCostCreateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = dto.Adapt<TaktBomMaterialCost>();
        entity.ProductCode = TaktStringHelper.NormalizeSapNumericMaterialCode(entity.ProductCode);
        entity.ComponentCode = TaktStringHelper.NormalizeSapNumericMaterialCode(entity.ComponentCode);
        var isUnique_ix_takt_logistics_manufacturing_bom_material_cost_line_unique = await _uniqueValidator.IsUniqueAsync(
            _bomMaterialCostRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProductCode == entity.ProductCode
                && x.SequenceNo == entity.SequenceNo
                && x.BomItemNo == entity.BomItemNo
                && x.ComponentCode == entity.ComponentCode
                && x.ProcessedDate == entity.ProcessedDate);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_material_cost_line_unique)
        {
            throw new TaktBusinessException("BOM物料成本的PlantCode、ProductCode、SequenceNo、BomItemNo、ComponentCode、ProcessedDate已存在");
        }
        var lookup = await BuildProductModelLookupAsync();
        ApplyModelMetadata(entity, lookup);
        entity.ModelMonthlyAverageCost = 0;
        entity = await _bomMaterialCostRepository.CreateAsync(entity);
        if (!string.IsNullOrWhiteSpace(entity.ModelCode))
        {
            await RefreshModelMonthlyAverageCostAsync(entity.PlantCode, entity.ModelCode, entity.ProcessedDate);
        }
        return await GetBomMaterialCostByIdAsync(entity.Id) ?? entity.Adapt<TaktBomMaterialCostDto>();
    }