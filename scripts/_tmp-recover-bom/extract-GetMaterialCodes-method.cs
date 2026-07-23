        return await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
    }

    /// <summary>
    /// 按机种编码获取型号目的地物料编码集合（含 SAP 归一化形态，供 BOM 产品过滤）
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <returns>物料/产品编码集合</returns>
    private async Task<HashSet<string>> GetMaterialCodesByModelAsync(string modelCode)
    {
        var list = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.ModelCode != null
                && x.ModelCode == modelCode);
        var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in list)
        {
            if (string.IsNullOrWhiteSpace(item.MaterialCode))
            {
                continue;
            }
            var raw = item.MaterialCode.Trim();
            codes.Add(raw);
            var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(raw);
            if (!string.IsNullOrWhiteSpace(normalized))
            {
                codes.Add(normalized);
            }
        }
        return codes;
    }

    /// <summary>
    /// 解析月度涨跌分析涉及的产品编码列表
    /// </summary>
    /// <param name="rows">已加载成本行</param>
    /// <param name="productCode">单产品编码；为空表示机种下全部物料</param>
    /// <returns>产品编码列表</returns>
    private static List<string> ResolveProductCodesInScope(IReadOnlyList<TaktBomMaterialCost> rows, string? productCode)
    {
        if (!string.IsNullOrWhiteSpace(productCode))
        {
            return new List<string> { productCode.Trim() };
        }
        return rows
            .Where(TaktBomMaterialCostLineCostHelper.CountsTowardBomMaterialCost)
            .Select(r => r.ProductCode)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList()!;
    }

    /// <summary>
    /// 构建期间列顺序