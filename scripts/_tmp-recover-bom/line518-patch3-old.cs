        if (!string.IsNullOrEmpty(queryDto?.ProductDescription))
        {
            exp = exp.And(x => x.ProductDescription != null && x.ProductDescription.Contains(queryDto.ProductDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.BomLevel))