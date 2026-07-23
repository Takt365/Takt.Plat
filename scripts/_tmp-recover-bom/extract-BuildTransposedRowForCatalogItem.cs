        return codes;
    }

    /// <summary>
    /// 按机种获取有序物料清单（型号目的地 MaterialCode 去重）
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <returns>物料编码与名称</returns>
    private async Task<List<(string ProductCode, string ProductDescription)>> GetOrderedModelMaterialsAsync(string modelCode)
    {
        var list = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.ModelCode != null
                && x.ModelCode == modelCode,
            x => x.SortOrder,
            false);
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.MaterialCode))
            .GroupBy(e => e.MaterialCode!, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Min(x => x.SortOrder))
            .ThenBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var first = g.First();
                var name = first.MaterialName?.Trim() ?? string.Empty;
                return (g.Key, name);
            })
            .ToList();
    }

    /// <summary>
    /// 按目录项构建转置行（匹配 BOM 产品行，无数据时仍输出空成本列）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="catalogProductCode">目录产品/物料编码</param>
    /// <param name="catalogDescription">目录描述</param>
    /// <param name="productGroups">BOM 产品分组</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>转置行</returns>
    private static TaktBomMaterialCostTransposedDto BuildTransposedRowForCatalogItem(
        string plantCode,
        string catalogProductCode,
        string catalogDescription,
        IReadOnlyDictionary<string, List<TaktBomMaterialCost>> productGroups,
        IReadOnlyList<string> periodOrder)
    {
        List<TaktBomMaterialCost>? matchedRows = null;
        string? matchedProductCode = null;
        foreach (var (productCode, rows) in productGroups)
        {
            if (!TaktBomMaterialCostLineCostHelper.ProductCodeMatches(productCode, catalogProductCode))
            {
                continue;
            }
            matchedRows = rows;
            matchedProductCode = productCode;
            break;
        }
        if (matchedRows == null || matchedProductCode == null)
        {
            return new TaktBomMaterialCostTransposedDto
            {
                PlantCode = plantCode,
                ProductCode = catalogProductCode,
                ProductDescription = catalogDescription,
                PeriodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal),
            };
        }
        var description = matchedRows.FirstOrDefault()?.ProductDescription?.Trim();
        if (string.IsNullOrWhiteSpace(description))
        {
            description = catalogDescription;
        }
        return BuildTransposedRow(plantCode, matchedProductCode, matchedRows, periodOrder) with
        {
            ProductCode = catalogProductCode,
            ProductDescription = description ?? catalogProductCode,
        };
    }

    /// <summary>
    /// 构建期间列顺序