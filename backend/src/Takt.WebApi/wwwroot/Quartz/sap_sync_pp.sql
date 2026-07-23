SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体一致（TaktPurchasePrice / Item / ScaleQuantity / ScaleValue）
-- Sap_Data.dbo.takt_logistics_materials_purchase_price*
--   → 当前租户库 dbo.takt_logistics_materials_purchase_price*
-- 列对照（仅业务列；基类 tenant/company/审计列由本脚本写入目标）：
--   主表 TaktPurchasePrice：plant_code, purchase_price_code, price_type, supplier_code, material_code,
--     purchase_group, tax_code, gr_based_invoice_inspection, pricing_date_control,
--     valid_from, valid_to, purchase_inquiry_id, purchase_inquiry_code, variable_key
--   明细 TaktPurchasePriceItem：无 plant_code；purchase_price_code, purchase_price_seq, price_type,
--     scale_*, calculation_type, price, untaxed_price, tax_included_price, condition_currency,
--     price_unit, unit_of_measure, min_order_quantity, rounding_value, planned_delivery_time_days, is_obsolete
--     （purchase_price_id 由主表 MERGE 后按 purchase_price_code 回填）
--   数量等级：purchase_price_code, purchase_price_seq, purchase_scale_seq, scale_quantity, price,
--     untaxed_price, tax_included_price, is_obsolete（purchase_price_item_id 按 code+seq 回填）
--   价值等级：同上，门槛列为 scale_value
-- 业务唯一键：
--   主表 plant_code + purchase_price_code
--   明细 purchase_price_code + purchase_price_seq
--   数量等级 code + seq + purchase_scale_seq + scale_quantity
--   价值等级 code + seq + purchase_scale_seq + scale_value

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#sq') IS NOT NULL DROP TABLE #sq;
IF OBJECT_ID('tempdb..#sv') IS NOT NULL DROP TABLE #sv;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#sq_delta') IS NOT NULL DROP TABLE #sq_delta;
IF OBJECT_ID('tempdb..#sv_delta') IS NOT NULL DROP TABLE #sv_delta;

CREATE TABLE #hdr (
  [rn] INT, [id] BIGINT,
  [plant_code] NVARCHAR(4), [purchase_price_code] NVARCHAR(20), [price_type] NVARCHAR(4),
  [supplier_code] NVARCHAR(40), [material_code] NVARCHAR(40),
  [purchase_group] NVARCHAR(3), [tax_code] NVARCHAR(4),
  [gr_based_invoice_inspection] INT, [pricing_date_control] INT,
  [valid_from] DATETIME, [valid_to] DATETIME,
  [purchase_inquiry_id] BIGINT, [purchase_inquiry_code] NVARCHAR(40), [variable_key] NVARCHAR(40)
);

CREATE TABLE #item (
  [rn] INT, [id] BIGINT, [purchase_price_id] BIGINT,
  [purchase_price_code] NVARCHAR(20), [purchase_price_seq] INT, [price_type] NVARCHAR(4),
  [scale_type] NVARCHAR(1), [scale_basis] NVARCHAR(1),
  [scale_quantity] DECIMAL(18,4), [scale_unit] NVARCHAR(5),
  [scale_value] DECIMAL(18,5), [scale_currency] NVARCHAR(3),
  [calculation_type] NVARCHAR(1), [price] DECIMAL(18,5),
  [untaxed_price] DECIMAL(18,5), [tax_included_price] DECIMAL(18,5),
  [condition_currency] NVARCHAR(3), [price_unit] INT, [unit_of_measure] NVARCHAR(5),
  [min_order_quantity] INT, [rounding_value] INT, [planned_delivery_time_days] INT, [is_obsolete] INT
);

CREATE TABLE #sq (
  [rn] INT, [id] BIGINT, [purchase_price_item_id] BIGINT,
  [purchase_price_code] NVARCHAR(20), [purchase_price_seq] INT, [purchase_scale_seq] INT,
  [scale_quantity] DECIMAL(18,4), [price] DECIMAL(18,5),
  [untaxed_price] DECIMAL(18,5), [tax_included_price] DECIMAL(18,5), [is_obsolete] INT
);

CREATE TABLE #sv (
  [rn] INT, [id] BIGINT, [purchase_price_item_id] BIGINT,
  [purchase_price_code] NVARCHAR(20), [purchase_price_seq] INT, [purchase_scale_seq] INT,
  [scale_value] DECIMAL(18,5), [price] DECIMAL(18,5),
  [untaxed_price] DECIMAL(18,5), [tax_included_price] DECIMAL(18,5), [is_obsolete] INT
);

CREATE TABLE #hdr_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, plant_code NVARCHAR(4), price_code NVARCHAR(20));
CREATE TABLE #item_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, price_code NVARCHAR(20), price_seq INT);
CREATE TABLE #sq_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, price_code NVARCHAR(20), price_seq INT, scale_seq INT);
CREATE TABLE #sv_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, price_code NVARCHAR(20), price_seq INT, scale_seq INT);

INSERT INTO #hdr
SELECT S.rn, @base_id + S.rn,
  S.[plant_code], S.[purchase_price_code], S.[price_type], S.[supplier_code], S.[material_code],
  S.[purchase_group], S.[tax_code],
  S.[gr_based_invoice_inspection], S.[pricing_date_control],
  S.[valid_from], S.[valid_to], S.[purchase_inquiry_id], S.[purchase_inquiry_code], S.[variable_key]
FROM (
  SELECT
    LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
    LTRIM(RTRIM(R.[purchase_price_code])) AS [purchase_price_code],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[price_type])), N''), N'PB00') AS [price_type],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[supplier_code])), N''), N'') AS [supplier_code],
    CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code],
    NULLIF(LTRIM(RTRIM(R.[purchase_group])), N'') AS [purchase_group],
    NULLIF(LTRIM(RTRIM(R.[tax_code])), N'') AS [tax_code],
    COALESCE(TRY_CAST(R.[gr_based_invoice_inspection] AS INT), 0) AS [gr_based_invoice_inspection],
    COALESCE(TRY_CAST(R.[pricing_date_control] AS INT), 1) AS [pricing_date_control],
    COALESCE(TRY_CAST(R.[valid_from] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS [valid_from],
    COALESCE(TRY_CAST(R.[valid_to] AS DATETIME), CAST('9999-12-31 23:59:59' AS DATETIME)) AS [valid_to],
    TRY_CAST(R.[purchase_inquiry_id] AS BIGINT) AS [purchase_inquiry_id],
    NULLIF(LTRIM(RTRIM(R.[purchase_inquiry_code])), N'') AS [purchase_inquiry_code],
    NULLIF(LTRIM(RTRIM(R.[variable_key])), N'') AS [variable_key],
    ROW_NUMBER() OVER (ORDER BY LTRIM(RTRIM(R.[plant_code])), LTRIM(RTRIM(R.[purchase_price_code]))) AS rn
  FROM [Sap_Data].[dbo].[takt_logistics_materials_purchase_price] R
  WHERE LTRIM(RTRIM(ISNULL(R.[purchase_price_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @hdr_source INT = (SELECT COUNT(*) FROM #hdr);
DECLARE @hdr_sap_raw INT = (
  SELECT COUNT(*) FROM [Sap_Data].[dbo].[takt_logistics_materials_purchase_price] R
  WHERE LTRIM(RTRIM(ISNULL(R.[purchase_price_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
);
IF @hdr_source <> @hdr_sap_raw
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(N'主表装入不一致: sap=', @hdr_sap_raw, N', loaded=', @hdr_source);
  THROW 50003, @hdr_src_msg, 1;
END;
IF EXISTS (SELECT 1 FROM #hdr GROUP BY [plant_code], [purchase_price_code] HAVING COUNT(*) > 1)
BEGIN
  DECLARE @hdr_dup NVARCHAR(400);
  SELECT TOP 1 @hdr_dup = CONCAT([plant_code], N' / ', [purchase_price_code], N' x', COUNT(*))
  FROM #hdr GROUP BY [plant_code], [purchase_price_code] HAVING COUNT(*) > 1;
  THROW 50001, @hdr_dup, 1;
END;

INSERT INTO #item
SELECT S.rn, @base_id + 1000000000 + S.rn, 0,
  S.[purchase_price_code], S.[purchase_price_seq], S.[price_type],
  S.[scale_type], S.[scale_basis], S.[scale_quantity], S.[scale_unit],
  S.[scale_value], S.[scale_currency], S.[calculation_type], S.[price],
  S.[untaxed_price], S.[tax_included_price], S.[condition_currency], S.[price_unit], S.[unit_of_measure],
  S.[min_order_quantity], S.[rounding_value], S.[planned_delivery_time_days], S.[is_obsolete]
FROM (
  SELECT
    LTRIM(RTRIM(R.[purchase_price_code])) AS [purchase_price_code],
    COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10) AS [purchase_price_seq],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[price_type])), N''), N'PB00') AS [price_type],
    NULLIF(LTRIM(RTRIM(R.[scale_type])), N'') AS [scale_type],
    NULLIF(LTRIM(RTRIM(R.[scale_basis])), N'') AS [scale_basis],
    ROUND(COALESCE(TRY_CAST(R.[scale_quantity] AS DECIMAL(18,8)), 0), 4) AS [scale_quantity],
    NULLIF(LTRIM(RTRIM(R.[scale_unit])), N'') AS [scale_unit],
    ROUND(COALESCE(TRY_CAST(R.[scale_value] AS DECIMAL(18,8)), 0), 5) AS [scale_value],
    NULLIF(LTRIM(RTRIM(R.[scale_currency])), N'') AS [scale_currency],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[calculation_type])), N''), N'A') AS [calculation_type],
    ROUND(COALESCE(TRY_CAST(R.[price] AS DECIMAL(18,8)), 0), 5) AS [price],
    ROUND(COALESCE(TRY_CAST(R.[untaxed_price] AS DECIMAL(18,8)), 0), 5) AS [untaxed_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_included_price] AS DECIMAL(18,8)), 0), 5) AS [tax_included_price],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[condition_currency])), N''), N'CNY') AS [condition_currency],
    COALESCE(TRY_CAST(R.[price_unit] AS INT), 1000) AS [price_unit],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[unit_of_measure])), N''), N'PC') AS [unit_of_measure],
    COALESCE(TRY_CAST(R.[min_order_quantity] AS INT), 0) AS [min_order_quantity],
    COALESCE(TRY_CAST(R.[rounding_value] AS INT), 0) AS [rounding_value],
    COALESCE(TRY_CAST(R.[planned_delivery_time_days] AS INT), 0) AS [planned_delivery_time_days],
    COALESCE(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
    ROW_NUMBER() OVER (ORDER BY LTRIM(RTRIM(R.[purchase_price_code])), COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10)) AS rn
  FROM [Sap_Data].[dbo].[takt_logistics_materials_purchase_price_item] R
  WHERE LTRIM(RTRIM(ISNULL(R.[purchase_price_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #hdr H
      WHERE H.[purchase_price_code] = LTRIM(RTRIM(R.[purchase_price_code]))
    )
) S;

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item);
DECLARE @item_sap_raw INT = (
  SELECT COUNT(*) FROM [Sap_Data].[dbo].[takt_logistics_materials_purchase_price_item] R
  WHERE LTRIM(RTRIM(ISNULL(R.[purchase_price_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #hdr H
      WHERE H.[purchase_price_code] = LTRIM(RTRIM(R.[purchase_price_code]))
    )
);
IF @item_source <> @item_sap_raw
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(N'明细装入不一致: sap=', @item_sap_raw, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;
IF EXISTS (SELECT 1 FROM #item GROUP BY [purchase_price_code], [purchase_price_seq] HAVING COUNT(*) > 1)
BEGIN
  DECLARE @item_dup NVARCHAR(400);
  SELECT TOP 1 @item_dup = CONCAT([purchase_price_code], N' / ', [purchase_price_seq], N' x', COUNT(*))
  FROM #item GROUP BY [purchase_price_code], [purchase_price_seq] HAVING COUNT(*) > 1;
  THROW 50001, @item_dup, 1;
END;

INSERT INTO #sq
SELECT S.rn, @base_id + 2000000000 + S.rn, 0,
  S.[purchase_price_code], S.[purchase_price_seq], S.[purchase_scale_seq],
  S.[scale_quantity], S.[price], S.[untaxed_price], S.[tax_included_price], S.[is_obsolete]
FROM (
  SELECT
    LTRIM(RTRIM(R.[purchase_price_code])) AS [purchase_price_code],
    COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10) AS [purchase_price_seq],
    COALESCE(TRY_CAST(R.[purchase_scale_seq] AS INT), 10) AS [purchase_scale_seq],
    ROUND(COALESCE(TRY_CAST(R.[scale_quantity] AS DECIMAL(18,8)), 0), 4) AS [scale_quantity],
    ROUND(COALESCE(TRY_CAST(R.[price] AS DECIMAL(18,8)), 0), 5) AS [price],
    ROUND(COALESCE(TRY_CAST(R.[untaxed_price] AS DECIMAL(18,8)), 0), 5) AS [untaxed_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_included_price] AS DECIMAL(18,8)), 0), 5) AS [tax_included_price],
    COALESCE(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
    ROW_NUMBER() OVER (
      ORDER BY LTRIM(RTRIM(R.[purchase_price_code])), COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10),
        COALESCE(TRY_CAST(R.[purchase_scale_seq] AS INT), 10), COALESCE(TRY_CAST(R.[scale_quantity] AS DECIMAL(18,8)), 0)
    ) AS rn
  FROM [Sap_Data].[dbo].[takt_logistics_materials_purchase_price_scale_quantity] R
  WHERE LTRIM(RTRIM(ISNULL(R.[purchase_price_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #item I
      WHERE I.[purchase_price_code] = LTRIM(RTRIM(R.[purchase_price_code]))
        AND I.[purchase_price_seq] = COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10)
    )
) S;

DECLARE @sq_source INT = (SELECT COUNT(*) FROM #sq);
DECLARE @sq_sap_raw INT = (
  SELECT COUNT(*) FROM [Sap_Data].[dbo].[takt_logistics_materials_purchase_price_scale_quantity] R
  WHERE LTRIM(RTRIM(ISNULL(R.[purchase_price_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #item I
      WHERE I.[purchase_price_code] = LTRIM(RTRIM(R.[purchase_price_code]))
        AND I.[purchase_price_seq] = COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10)
    )
);
IF @sq_source <> @sq_sap_raw
BEGIN
  DECLARE @sq_src_msg NVARCHAR(200) = CONCAT(N'数量等级装入不一致: sap=', @sq_sap_raw, N', loaded=', @sq_source);
  THROW 50003, @sq_src_msg, 1;
END;

INSERT INTO #sv
SELECT S.rn, @base_id + 3000000000 + S.rn, 0,
  S.[purchase_price_code], S.[purchase_price_seq], S.[purchase_scale_seq],
  S.[scale_value], S.[price], S.[untaxed_price], S.[tax_included_price], S.[is_obsolete]
FROM (
  SELECT
    LTRIM(RTRIM(R.[purchase_price_code])) AS [purchase_price_code],
    COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10) AS [purchase_price_seq],
    COALESCE(TRY_CAST(R.[purchase_scale_seq] AS INT), 10) AS [purchase_scale_seq],
    ROUND(COALESCE(TRY_CAST(R.[scale_value] AS DECIMAL(18,8)), 0), 5) AS [scale_value],
    ROUND(COALESCE(TRY_CAST(R.[price] AS DECIMAL(18,8)), 0), 5) AS [price],
    ROUND(COALESCE(TRY_CAST(R.[untaxed_price] AS DECIMAL(18,8)), 0), 5) AS [untaxed_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_included_price] AS DECIMAL(18,8)), 0), 5) AS [tax_included_price],
    COALESCE(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
    ROW_NUMBER() OVER (
      ORDER BY LTRIM(RTRIM(R.[purchase_price_code])), COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10),
        COALESCE(TRY_CAST(R.[purchase_scale_seq] AS INT), 10), COALESCE(TRY_CAST(R.[scale_value] AS DECIMAL(18,8)), 0)
    ) AS rn
  FROM [Sap_Data].[dbo].[takt_logistics_materials_purchase_price_scale_value] R
  WHERE LTRIM(RTRIM(ISNULL(R.[purchase_price_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #item I
      WHERE I.[purchase_price_code] = LTRIM(RTRIM(R.[purchase_price_code]))
        AND I.[purchase_price_seq] = COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10)
    )
) S;

DECLARE @sv_source INT = (SELECT COUNT(*) FROM #sv);
DECLARE @sv_sap_raw INT = (
  SELECT COUNT(*) FROM [Sap_Data].[dbo].[takt_logistics_materials_purchase_price_scale_value] R
  WHERE LTRIM(RTRIM(ISNULL(R.[purchase_price_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #item I
      WHERE I.[purchase_price_code] = LTRIM(RTRIM(R.[purchase_price_code]))
        AND I.[purchase_price_seq] = COALESCE(TRY_CAST(R.[purchase_price_seq] AS INT), 10)
    )
);
IF @sv_source <> @sv_sap_raw
BEGIN
  DECLARE @sv_src_msg NVARCHAR(200) = CONCAT(N'价值等级装入不一致: sap=', @sv_sap_raw, N', loaded=', @sv_source);
  THROW 50003, @sv_src_msg, 1;
END;

DECLARE @hdr_before INT = (SELECT COUNT(*) FROM [takt_logistics_materials_purchase_price] WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0);
DECLARE @item_before INT = (SELECT COUNT(*) FROM [takt_logistics_materials_purchase_price_item] WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0);
DECLARE @sq_before INT = (SELECT COUNT(*) FROM [takt_logistics_materials_purchase_price_scale_quantity] WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0);
DECLARE @sv_before INT = (SELECT COUNT(*) FROM [takt_logistics_materials_purchase_price_scale_value] WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0);

MERGE INTO [takt_logistics_materials_purchase_price] AS T
USING #hdr AS S
ON T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code]
 AND LTRIM(RTRIM(T.[purchase_price_code]))=S.[purchase_price_code]
WHEN MATCHED AND (
  T.[is_deleted]<>0
  OR LTRIM(RTRIM(ISNULL(T.[price_type],N'')))<>S.[price_type]
  OR LTRIM(RTRIM(ISNULL(T.[supplier_code],N'')))<>S.[supplier_code]
  OR LTRIM(RTRIM(ISNULL(T.[material_code],N'')))<>S.[material_code]
  OR LTRIM(RTRIM(ISNULL(T.[purchase_group],N'')))<>LTRIM(RTRIM(ISNULL(S.[purchase_group],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_code],N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_code],N'')))
  OR T.[gr_based_invoice_inspection]<>S.[gr_based_invoice_inspection]
  OR T.[pricing_date_control]<>S.[pricing_date_control]
  OR T.[valid_from]<>S.[valid_from] OR T.[valid_to]<>S.[valid_to]
  OR ISNULL(T.[purchase_inquiry_id],0)<>ISNULL(S.[purchase_inquiry_id],0)
  OR LTRIM(RTRIM(ISNULL(T.[purchase_inquiry_code],N'')))<>LTRIM(RTRIM(ISNULL(S.[purchase_inquiry_code],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[variable_key],N'')))<>LTRIM(RTRIM(ISNULL(S.[variable_key],N'')))
) THEN UPDATE SET
  T.[price_type]=S.[price_type], T.[supplier_code]=S.[supplier_code], T.[material_code]=S.[material_code],
  T.[purchase_group]=S.[purchase_group], T.[tax_code]=S.[tax_code],
  T.[gr_based_invoice_inspection]=S.[gr_based_invoice_inspection], T.[pricing_date_control]=S.[pricing_date_control],
  T.[valid_from]=S.[valid_from], T.[valid_to]=S.[valid_to],
  T.[purchase_inquiry_id]=S.[purchase_inquiry_id], T.[purchase_inquiry_code]=S.[purchase_inquiry_code],
  T.[variable_key]=S.[variable_key],
  T.[updated_by]=@sync_user_id, T.[updated_at]=@now, T.[is_deleted]=0, T.[deleted_by]=NULL, T.[deleted_at]=NULL
WHEN NOT MATCHED THEN INSERT (
  [id],[plant_code],[purchase_price_code],[price_type],[supplier_code],[material_code],
  [purchase_group],[tax_code],[gr_based_invoice_inspection],[pricing_date_control],
  [valid_from],[valid_to],[purchase_inquiry_id],[purchase_inquiry_code],[variable_key],
  [tenant_code],[company_code],[ext_field_json],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
) VALUES (
  S.[id],S.[plant_code],S.[purchase_price_code],S.[price_type],S.[supplier_code],S.[material_code],
  S.[purchase_group],S.[tax_code],S.[gr_based_invoice_inspection],S.[pricing_date_control],
  S.[valid_from],S.[valid_to],S.[purchase_inquiry_id],S.[purchase_inquiry_code],S.[variable_key],
  @tenant_code,@company_code,N'{}',N'',
  @sync_user_id,@now,@sync_user_id,@now,0
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[plant_code], INSERTED.[purchase_price_code]
INTO #hdr_delta(rn, oper_type, id, plant_code, price_code);

UPDATE S SET S.[id]=T.[id]
FROM #hdr S
INNER JOIN [takt_logistics_materials_purchase_price] T
  ON T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code] AND LTRIM(RTRIM(T.[purchase_price_code]))=S.[purchase_price_code];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_materials_purchase_price] T
WHERE T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
  AND NOT EXISTS (SELECT 1 FROM #hdr S WHERE S.[plant_code]=LTRIM(RTRIM(T.[plant_code])) AND S.[purchase_price_code]=LTRIM(RTRIM(T.[purchase_price_code])));
DECLARE @hdr_del INT = @@ROWCOUNT;

UPDATE I SET I.[purchase_price_id]=H.[id]
FROM #item I
INNER JOIN #hdr H ON H.[purchase_price_code]=I.[purchase_price_code];
DELETE FROM #item WHERE [purchase_price_id]=0 OR [purchase_price_id] IS NULL;

MERGE INTO [takt_logistics_materials_purchase_price_item] AS T
USING #item AS S
ON T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code
 AND T.[purchase_price_id]=S.[purchase_price_id] AND T.[purchase_price_seq]=S.[purchase_price_seq]
WHEN MATCHED AND (
  T.[is_deleted]<>0
  OR LTRIM(RTRIM(ISNULL(T.[purchase_price_code],N'')))<>S.[purchase_price_code]
  OR LTRIM(RTRIM(ISNULL(T.[price_type],N'')))<>S.[price_type]
  OR LTRIM(RTRIM(ISNULL(T.[scale_type],N'')))<>LTRIM(RTRIM(ISNULL(S.[scale_type],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[scale_basis],N'')))<>LTRIM(RTRIM(ISNULL(S.[scale_basis],N'')))
  OR ROUND(T.[scale_quantity],4)<>ROUND(S.[scale_quantity],4)
  OR LTRIM(RTRIM(ISNULL(T.[scale_unit],N'')))<>LTRIM(RTRIM(ISNULL(S.[scale_unit],N'')))
  OR ROUND(T.[scale_value],5)<>ROUND(S.[scale_value],5)
  OR LTRIM(RTRIM(ISNULL(T.[scale_currency],N'')))<>LTRIM(RTRIM(ISNULL(S.[scale_currency],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[calculation_type],N'')))<>S.[calculation_type]
  OR ROUND(T.[price],5)<>ROUND(S.[price],5)
  OR ROUND(T.[untaxed_price],5)<>ROUND(S.[untaxed_price],5)
  OR ROUND(T.[tax_included_price],5)<>ROUND(S.[tax_included_price],5)
  OR LTRIM(RTRIM(ISNULL(T.[condition_currency],N'')))<>S.[condition_currency]
  OR T.[price_unit]<>S.[price_unit]
  OR LTRIM(RTRIM(ISNULL(T.[unit_of_measure],N'')))<>S.[unit_of_measure]
  OR T.[min_order_quantity]<>S.[min_order_quantity]
  OR T.[rounding_value]<>S.[rounding_value]
  OR T.[planned_delivery_time_days]<>S.[planned_delivery_time_days]
  OR T.[is_obsolete]<>S.[is_obsolete]
) THEN UPDATE SET
  T.[purchase_price_code]=S.[purchase_price_code], T.[price_type]=S.[price_type],
  T.[scale_type]=S.[scale_type], T.[scale_basis]=S.[scale_basis],
  T.[scale_quantity]=S.[scale_quantity], T.[scale_unit]=S.[scale_unit],
  T.[scale_value]=S.[scale_value], T.[scale_currency]=S.[scale_currency],
  T.[calculation_type]=S.[calculation_type], T.[price]=S.[price],
  T.[untaxed_price]=S.[untaxed_price], T.[tax_included_price]=S.[tax_included_price],
  T.[condition_currency]=S.[condition_currency], T.[price_unit]=S.[price_unit],
  T.[unit_of_measure]=S.[unit_of_measure], T.[min_order_quantity]=S.[min_order_quantity],
  T.[rounding_value]=S.[rounding_value], T.[planned_delivery_time_days]=S.[planned_delivery_time_days],
  T.[is_obsolete]=S.[is_obsolete],
  T.[updated_by]=@sync_user_id, T.[updated_at]=@now, T.[is_deleted]=0, T.[deleted_by]=NULL, T.[deleted_at]=NULL
WHEN NOT MATCHED THEN INSERT (
  [id],[purchase_price_id],[purchase_price_code],[purchase_price_seq],[price_type],
  [scale_type],[scale_basis],[scale_quantity],[scale_unit],[scale_value],[scale_currency],
  [calculation_type],[price],[untaxed_price],[tax_included_price],
  [condition_currency],[price_unit],[unit_of_measure],
  [min_order_quantity],[rounding_value],[planned_delivery_time_days],[is_obsolete],
  [tenant_code],[company_code],[ext_field_json],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
) VALUES (
  S.[id],S.[purchase_price_id],S.[purchase_price_code],S.[purchase_price_seq],S.[price_type],
  S.[scale_type],S.[scale_basis],S.[scale_quantity],S.[scale_unit],S.[scale_value],S.[scale_currency],
  S.[calculation_type],S.[price],S.[untaxed_price],S.[tax_included_price],
  S.[condition_currency],S.[price_unit],S.[unit_of_measure],
  S.[min_order_quantity],S.[rounding_value],S.[planned_delivery_time_days],S.[is_obsolete],
  @tenant_code,@company_code,N'{}',N'',
  @sync_user_id,@now,@sync_user_id,@now,0
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[purchase_price_code], INSERTED.[purchase_price_seq]
INTO #item_delta(rn, oper_type, id, price_code, price_seq);

UPDATE S SET S.[id]=T.[id]
FROM #item S
INNER JOIN [takt_logistics_materials_purchase_price_item] T
  ON T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
 AND T.[purchase_price_id]=S.[purchase_price_id] AND T.[purchase_price_seq]=S.[purchase_price_seq];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_materials_purchase_price_item] T
WHERE T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
  AND NOT EXISTS (SELECT 1 FROM #item S WHERE S.[purchase_price_id]=T.[purchase_price_id] AND S.[purchase_price_seq]=T.[purchase_price_seq]);
DECLARE @item_del INT = @@ROWCOUNT;

UPDATE Q SET Q.[purchase_price_item_id]=I.[id]
FROM #sq Q INNER JOIN #item I ON I.[purchase_price_code]=Q.[purchase_price_code] AND I.[purchase_price_seq]=Q.[purchase_price_seq];
DELETE FROM #sq WHERE [purchase_price_item_id]=0 OR [purchase_price_item_id] IS NULL;

UPDATE V SET V.[purchase_price_item_id]=I.[id]
FROM #sv V INNER JOIN #item I ON I.[purchase_price_code]=V.[purchase_price_code] AND I.[purchase_price_seq]=V.[purchase_price_seq];
DELETE FROM #sv WHERE [purchase_price_item_id]=0 OR [purchase_price_item_id] IS NULL;

MERGE INTO [takt_logistics_materials_purchase_price_scale_quantity] AS T
USING #sq AS S
ON T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code
 AND T.[purchase_price_item_id]=S.[purchase_price_item_id] AND T.[purchase_price_seq]=S.[purchase_price_seq]
 AND T.[purchase_scale_seq]=S.[purchase_scale_seq] AND ROUND(T.[scale_quantity],4)=ROUND(S.[scale_quantity],4)
WHEN MATCHED AND (
  T.[is_deleted]<>0 OR LTRIM(RTRIM(ISNULL(T.[purchase_price_code],N'')))<>S.[purchase_price_code]
  OR ROUND(T.[price],5)<>ROUND(S.[price],5) OR ROUND(T.[untaxed_price],5)<>ROUND(S.[untaxed_price],5)
  OR ROUND(T.[tax_included_price],5)<>ROUND(S.[tax_included_price],5) OR T.[is_obsolete]<>S.[is_obsolete]
) THEN UPDATE SET
  T.[purchase_price_code]=S.[purchase_price_code], T.[price]=S.[price], T.[untaxed_price]=S.[untaxed_price],
  T.[tax_included_price]=S.[tax_included_price], T.[is_obsolete]=S.[is_obsolete],
  T.[updated_by]=@sync_user_id, T.[updated_at]=@now, T.[is_deleted]=0, T.[deleted_by]=NULL, T.[deleted_at]=NULL
WHEN NOT MATCHED THEN INSERT (
  [id],[purchase_price_item_id],[purchase_price_code],[purchase_price_seq],[purchase_scale_seq],
  [scale_quantity],[price],[untaxed_price],[tax_included_price],[is_obsolete],
  [tenant_code],[company_code],[ext_field_json],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
) VALUES (
  S.[id],S.[purchase_price_item_id],S.[purchase_price_code],S.[purchase_price_seq],S.[purchase_scale_seq],
  S.[scale_quantity],S.[price],S.[untaxed_price],S.[tax_included_price],S.[is_obsolete],
  @tenant_code,@company_code,N'{}',N'',
  @sync_user_id,@now,@sync_user_id,@now,0
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[purchase_price_code], INSERTED.[purchase_price_seq], INSERTED.[purchase_scale_seq]
INTO #sq_delta(rn, oper_type, id, price_code, price_seq, scale_seq);

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_materials_purchase_price_scale_quantity] T
WHERE T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
  AND NOT EXISTS (
    SELECT 1 FROM #sq S
    WHERE S.[purchase_price_item_id]=T.[purchase_price_item_id] AND S.[purchase_price_seq]=T.[purchase_price_seq]
      AND S.[purchase_scale_seq]=T.[purchase_scale_seq] AND ROUND(S.[scale_quantity],4)=ROUND(T.[scale_quantity],4)
  );
DECLARE @sq_del INT = @@ROWCOUNT;

MERGE INTO [takt_logistics_materials_purchase_price_scale_value] AS T
USING #sv AS S
ON T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code
 AND T.[purchase_price_item_id]=S.[purchase_price_item_id] AND T.[purchase_price_seq]=S.[purchase_price_seq]
 AND T.[purchase_scale_seq]=S.[purchase_scale_seq] AND ROUND(T.[scale_value],5)=ROUND(S.[scale_value],5)
WHEN MATCHED AND (
  T.[is_deleted]<>0 OR LTRIM(RTRIM(ISNULL(T.[purchase_price_code],N'')))<>S.[purchase_price_code]
  OR ROUND(T.[price],5)<>ROUND(S.[price],5) OR ROUND(T.[untaxed_price],5)<>ROUND(S.[untaxed_price],5)
  OR ROUND(T.[tax_included_price],5)<>ROUND(S.[tax_included_price],5) OR T.[is_obsolete]<>S.[is_obsolete]
) THEN UPDATE SET
  T.[purchase_price_code]=S.[purchase_price_code], T.[price]=S.[price], T.[untaxed_price]=S.[untaxed_price],
  T.[tax_included_price]=S.[tax_included_price], T.[is_obsolete]=S.[is_obsolete],
  T.[updated_by]=@sync_user_id, T.[updated_at]=@now, T.[is_deleted]=0, T.[deleted_by]=NULL, T.[deleted_at]=NULL
WHEN NOT MATCHED THEN INSERT (
  [id],[purchase_price_item_id],[purchase_price_code],[purchase_price_seq],[purchase_scale_seq],
  [scale_value],[price],[untaxed_price],[tax_included_price],[is_obsolete],
  [tenant_code],[company_code],[ext_field_json],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
) VALUES (
  S.[id],S.[purchase_price_item_id],S.[purchase_price_code],S.[purchase_price_seq],S.[purchase_scale_seq],
  S.[scale_value],S.[price],S.[untaxed_price],S.[tax_included_price],S.[is_obsolete],
  @tenant_code,@company_code,N'{}',N'',
  @sync_user_id,@now,@sync_user_id,@now,0
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[purchase_price_code], INSERTED.[purchase_price_seq], INSERTED.[purchase_scale_seq]
INTO #sv_delta(rn, oper_type, id, price_code, price_seq, scale_seq);

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_materials_purchase_price_scale_value] T
WHERE T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
  AND NOT EXISTS (
    SELECT 1 FROM #sv S
    WHERE S.[purchase_price_item_id]=T.[purchase_price_item_id] AND S.[purchase_price_seq]=T.[purchase_price_seq]
      AND S.[purchase_scale_seq]=T.[purchase_scale_seq] AND ROUND(S.[scale_value],5)=ROUND(T.[scale_value],5)
  );
DECLARE @sv_del INT = @@ROWCOUNT;

DECLARE @hdr_after INT = (SELECT COUNT(*) FROM [takt_logistics_materials_purchase_price] WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0);
DECLARE @item_after INT = (SELECT COUNT(*) FROM [takt_logistics_materials_purchase_price_item] WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0);
DECLARE @sq_after INT = (SELECT COUNT(*) FROM [takt_logistics_materials_purchase_price_scale_quantity] WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0);
DECLARE @sv_after INT = (SELECT COUNT(*) FROM [takt_logistics_materials_purchase_price_scale_value] WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0);

DECLARE @hdr_ins INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type='INSERT');
DECLARE @hdr_upd INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type='UPDATE');
DECLARE @item_ins INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type='INSERT');
DECLARE @item_upd INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type='UPDATE');
DECLARE @sq_ins INT = (SELECT COUNT(*) FROM #sq_delta WHERE oper_type='INSERT');
DECLARE @sq_upd INT = (SELECT COUNT(*) FROM #sq_delta WHERE oper_type='UPDATE');
DECLARE @sv_ins INT = (SELECT COUNT(*) FROM #sv_delta WHERE oper_type='INSERT');
DECLARE @sv_upd INT = (SELECT COUNT(*) FROM #sv_delta WHERE oper_type='UPDATE');

IF @hdr_after <> @hdr_source
BEGIN
  DECLARE @hdr_cnt_msg NVARCHAR(200) = CONCAT(N'主表有效行不一致: source=', @hdr_source, N', active=', @hdr_after);
  THROW 50002, @hdr_cnt_msg, 1;
END;

DECLARE @json_result NVARCHAR(MAX) =
  N'{"hdr_sap_raw":' + CAST(@hdr_sap_raw AS NVARCHAR)
  + N',"hdr_source":' + CAST(@hdr_source AS NVARCHAR)
  + N',"hdr_before":' + CAST(@hdr_before AS NVARCHAR)
  + N',"hdr_after":' + CAST(@hdr_after AS NVARCHAR)
  + N',"hdr_insert":' + CAST(@hdr_ins AS NVARCHAR)
  + N',"hdr_update":' + CAST(@hdr_upd AS NVARCHAR)
  + N',"hdr_soft_delete":' + CAST(@hdr_del AS NVARCHAR)
  + N',"item_sap_raw":' + CAST(@item_sap_raw AS NVARCHAR)
  + N',"item_source":' + CAST(@item_source AS NVARCHAR)
  + N',"item_before":' + CAST(@item_before AS NVARCHAR)
  + N',"item_after":' + CAST(@item_after AS NVARCHAR)
  + N',"item_insert":' + CAST(@item_ins AS NVARCHAR)
  + N',"item_update":' + CAST(@item_upd AS NVARCHAR)
  + N',"item_soft_delete":' + CAST(@item_del AS NVARCHAR)
  + N',"sq_sap_raw":' + CAST(@sq_sap_raw AS NVARCHAR)
  + N',"sq_source":' + CAST(@sq_source AS NVARCHAR)
  + N',"sq_before":' + CAST(@sq_before AS NVARCHAR)
  + N',"sq_after":' + CAST(@sq_after AS NVARCHAR)
  + N',"sq_insert":' + CAST(@sq_ins AS NVARCHAR)
  + N',"sq_update":' + CAST(@sq_upd AS NVARCHAR)
  + N',"sq_soft_delete":' + CAST(@sq_del AS NVARCHAR)
  + N',"sv_sap_raw":' + CAST(@sv_sap_raw AS NVARCHAR)
  + N',"sv_source":' + CAST(@sv_source AS NVARCHAR)
  + N',"sv_before":' + CAST(@sv_before AS NVARCHAR)
  + N',"sv_after":' + CAST(@sv_after AS NVARCHAR)
  + N',"sv_insert":' + CAST(@sv_ins AS NVARCHAR)
  + N',"sv_update":' + CAST(@sv_upd AS NVARCHAR)
  + N',"sv_soft_delete":' + CAST(@sv_del AS NVARCHAR) + N'}';

INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[created_by],[created_at]
) VALUES (
  @base_id + 1, N'SYSTEM_SAP_SYNC', N'SAP_SYNC', N'采购价格',
  N'exec_sql_merge', 'SQL', N'/sync/sap/purchase-price', CONCAT('batch_size=', @batch_size),
  @json_result, '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now, DATEDIFF(MILLISECOND,@now,GETDATE()), 1, '',
  @tenant_code, @company_code, @sync_user_id, @now
);

SELECT N'QUARTZ_SYNC_SUMMARY' AS [summary_tag], CAST(N'hdr' AS NVARCHAR(40)) AS [scope],
  @hdr_sap_raw AS [sap_raw_count], @hdr_source AS [source_count], @hdr_before AS [target_before],
  @hdr_after AS [target_after], @hdr_ins AS [insert_count], @hdr_upd AS [update_count], @hdr_del AS [delete_count]
UNION ALL SELECT N'QUARTZ_SYNC_SUMMARY', N'item', @item_sap_raw, @item_source, @item_before, @item_after, @item_ins, @item_upd, @item_del
UNION ALL SELECT N'QUARTZ_SYNC_SUMMARY', N'scale_quantity', @sq_sap_raw, @sq_source, @sq_before, @sq_after, @sq_ins, @sq_upd, @sq_del
UNION ALL SELECT N'QUARTZ_SYNC_SUMMARY', N'scale_value', @sv_sap_raw, @sv_source, @sv_before, @sv_after, @sv_ins, @sv_upd, @sv_del;
