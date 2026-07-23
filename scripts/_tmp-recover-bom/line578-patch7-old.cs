        var catalog = await GetOrderedModelMaterialsAsync(modelCode);
        if (catalog.Count == 0)
        {
            return;
        }