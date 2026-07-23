            .ToList();
    }

    /// <summary>
    /// 获取 BOM 物料成本机种下拉选项（ModelCode 去重，可选按工厂过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项（DictValue=机种编码）</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostModelOptionsAsync(string? plantCode = null)
    {
        EnsureThreeLayerContext();
        var trimmedPlant = plantCode?.Trim();
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.BomMaterialCostStatus == 1
                && (string.IsNullOrWhiteSpace(trimmedPlant) || x.PlantCode == trimmedPlant),
            x => x.ModelCode ?? string.Empty,
            false);
        var modelNameLookup = await BuildModelNameLookupAsync();
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.ModelCode))
            .GroupBy(e => e.ModelCode!, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Take(MaxCascadeSelectOptions)
            .Select(g =>
            {
                var modelCode = g.Key;
                var label = modelNameLookup.TryGetValue(modelCode, out var modelName) && !string.IsNullOrWhiteSpace(modelName)
                    ? modelName
                    : modelCode;
                return new TaktSelectOption
                {
                    DictValue = modelCode,
                    DictLabel = label,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 按机种获取 BOM 物料成本产品下拉选项（ProductCode 去重）
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项（DictValue=产品编码）</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostProductOptionsByModelAsync(string modelCode, string? plantCode = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelCode);
        EnsureThreeLayerContext();
        var trimmedModelCode = modelCode.Trim();
        var trimmedPlant = plantCode?.Trim();
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.BomMaterialCostStatus == 1
                && x.ModelCode == trimmedModelCode
                && (string.IsNullOrWhiteSpace(trimmedPlant) || x.PlantCode == trimmedPlant),
            x => x.ProductCode ?? string.Empty,
            false);
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.ProductCode))
            .GroupBy(e => e.ProductCode!, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Take(MaxCascadeSelectOptions)
            .Select(g =>
            {
                var first = g.First();
                var description = first.ProductDescription?.Trim();
                var label = string.IsNullOrWhiteSpace(description) ? g.Key : $"{g.Key} - {description}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                    ExtValue = first.ModelCode,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 根据产品编码反查机种编码（优先 BOM 行 ModelCode，未命中时回退型号目的地）
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>机种编码；未匹配时返回 null</returns>
    public async Task<string?> GetBomMaterialCostModelCodeByProductAsync(string productCode, string? plantCode = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        EnsureThreeLayerContext();
        var trimmedProductCode = productCode.Trim();
        var trimmedPlant = plantCode?.Trim();
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.BomMaterialCostStatus == 1
                && x.ProductCode == trimmedProductCode
                && (string.IsNullOrWhiteSpace(trimmedPlant) || x.PlantCode == trimmedPlant),
            x => x.ProcessedDate,
            true);
        var modelCode = list
            .Select(x => x.ModelCode?.Trim())
            .FirstOrDefault(code => !string.IsNullOrWhiteSpace(code));
        if (!string.IsNullOrWhiteSpace(modelCode))
        {
            return modelCode;
        }
        var destination = await _modelDestinationRepository.FirstAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.MaterialCode == trimmedProductCode);
        return string.IsNullOrWhiteSpace(destination?.ModelCode) ? null : destination.ModelCode.Trim();
    }

    /// <summary>
    /// 创建BOM物料成本
    /// </summary>