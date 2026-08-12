SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体 TaktSalesForecast / TaktSalesForecastItem 一致
-- {{SourceDatabase}}.dbo.takt_logistics_manufacturing_mds_sales_forecast[_item]
--   → 当前租户库同名表
-- 业务唯一键：
--   主表 PlantCode + SalesForecastCode(sales_plan_code) + ReceiveVersionNo
--   明细 SalesForecastId + FiscalYear + PlanMonth
--     （Sap 侧用 Plant + sales_plan_code + receive_version_no + fiscal_year + plan_month 关联）

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;

CREATE TABLE #hdr (
  [rn] INT, [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [sales_plan_code] NVARCHAR(20),
  [plan_date] DATETIME,
  [sales_product] NVARCHAR(7),
  [product_category_code] NVARCHAR(4),
  [profit_center_code] NVARCHAR(4),
  [model_code] NVARCHAR(40),
  [material_code] NVARCHAR(20),
  [material_description] NVARCHAR(40),
  [customer_code] NVARCHAR(10),
  [customer_name1] NVARCHAR(140),
  [planner_id] BIGINT,
  [plan_by] NVARCHAR(50),
  [total_quantity] DECIMAL(18,4),
  [total_amount] DECIMAL(18,2),
  [converted_quantity] DECIMAL(18,4),
  [converted_amount] DECIMAL(18,2),
  [plan_status] INT,
  [converted_status] INT,
  [plan_description] NVARCHAR(1000),
  [is_deleted] INT
);

CREATE TABLE #item (
  [rn] INT, [id] BIGINT, [sales_plan_id] BIGINT,
  [plant_code] NVARCHAR(4),
  [sales_plan_code] NVARCHAR(20),
  [plan_date] DATETIME,
  [line_number] INT,
  [fiscal_year] NVARCHAR(6),
  [plan_month] INT,
  [plan_quantity_001] DECIMAL(18,4),
  [plan_quantity_002] DECIMAL(18,4),
  [plan_quantity_delta] DECIMAL(18,4),
  [converted_quantity] DECIMAL(18,4),
  [estimated_unit_price] DECIMAL(18,2),
  [estimated_amount] DECIMAL(18,2),
  [is_obsolete] INT,
  [is_deleted] INT
);

CREATE TABLE #hdr_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  plant_code NVARCHAR(4), sales_plan_code NVARCHAR(20), plan_date DATETIME
);
CREATE TABLE #item_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  sales_plan_code NVARCHAR(20), fiscal_year NVARCHAR(6), plan_month INT
);

INSERT INTO #hdr
SELECT
  S.rn, @base_id + S.rn,
  S.[plant_code], S.[sales_plan_code], S.[plan_date],
  S.[sales_product], S.[product_category_code], S.[profit_center_code], S.[model_code],
  S.[material_code], S.[material_description],
  S.[customer_code], S.[customer_name1], S.[planner_id], S.[plan_by],
  S.[total_quantity], S.[total_amount], S.[converted_quantity], S.[converted_amount],
  S.[plan_status], S.[converted_status], S.[plan_description], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY N.[plant_code], N.[sales_plan_code], N.[receive_version_no]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LTRIM(RTRIM(R.[sales_plan_code])) AS [sales_plan_code],
      CAST(COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATETIME) AS [plan_date],
      N'Product' AS [sales_product],
      LEFT(ISNULL(NULLIF(LTRIM(RTRIM(R.[product_category_code])), N''), N''), 4) AS [product_category_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[profit_center_code], N''))), 4), N'') AS [profit_center_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[model_code], N''))), 40), N'') AS [model_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LEFT(LTRIM(RTRIM(ISNULL(R.[material_code], N''))), 20)
      END AS [material_code],
      LEFT(ISNULL(NULLIF(LTRIM(RTRIM(R.[material_description])), N''), N''), 40) AS [material_description],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_code], N''))), 10), N'') AS [customer_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_name1], N''))), 140), N'') AS [customer_name1],
      TRY_CAST(R.[planner_id] AS BIGINT) AS [planner_id],
      LEFT(ISNULL(NULLIF(LTRIM(RTRIM(R.[plan_by])), N''), N'SYNC'), 50) AS [plan_by],
      ROUND(COALESCE(TRY_CAST(R.[total_quantity] AS DECIMAL(18,8)), 0), 4) AS [total_quantity],
      ROUND(COALESCE(TRY_CAST(R.[total_amount] AS DECIMAL(18,8)), 0), 2) AS [total_amount],
      ROUND(COALESCE(TRY_CAST(R.[converted_quantity] AS DECIMAL(18,8)), 0), 4) AS [converted_quantity],
      ROUND(COALESCE(TRY_CAST(R.[converted_amount] AS DECIMAL(18,8)), 0), 2) AS [converted_amount],
      COALESCE(TRY_CAST(R.[plan_status] AS INT), 1) AS [plan_status],
      COALESCE(TRY_CAST(R.[converted_status] AS INT), 0) AS [converted_status],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[plan_description], N''))), 1000), N'') AS [plan_description],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[plant_code])),
          LTRIM(RTRIM(R.[sales_plan_code])),
          CAST(COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE)
        ORDER BY LEN(ISNULL(LTRIM(RTRIM(R.[material_code])), N'')) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_mds_sales_forecast] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[sales_plan_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[product_category_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

UPDATE H
SET H.[material_description] = LEFT(ISNULL(NULLIF(H.[material_description], N''), ISNULL(MP.[material_description], N'')), 40)
FROM #hdr H
OUTER APPLY (
  SELECT TOP (1) LTRIM(RTRIM(P.[material_description])) AS [material_description]
  FROM [takt_logistics_materials_material_plant] P
  WHERE P.[tenant_code] = @tenant_code
    AND P.[company_code] = @company_code
    AND P.[is_deleted] = 0
    AND LTRIM(RTRIM(P.[plant_code])) = H.[plant_code]
    AND LTRIM(RTRIM(P.[material_code])) = H.[material_code]
    AND LTRIM(RTRIM(ISNULL(P.[material_description], N''))) <> N''
  ORDER BY P.[id]
) MP
WHERE H.[material_description] = N'';

UPDATE H
SET H.[customer_name1] = LEFT(ISNULL(NULLIF(H.[customer_name1], N''), ISNULL(C.[customer_name1], N'')), 140)
FROM #hdr H
OUTER APPLY (
  SELECT TOP (1) LTRIM(RTRIM(P.[customer_name1])) AS [customer_name1]
  FROM [takt_logistics_sales_customer] P
  WHERE P.[tenant_code] = @tenant_code
    AND P.[company_code] = @company_code
    AND P.[is_deleted] = 0
    AND LTRIM(RTRIM(P.[plant_code])) = H.[plant_code]
    AND LTRIM(RTRIM(P.[customer_code])) = H.[customer_code]
) C
WHERE H.[customer_code] IS NOT NULL
  AND H.[customer_code] <> N''
  AND ISNULL(H.[customer_name1], N'') = N'';

DECLARE @hdr_source INT = (SELECT COUNT(*) FROM #hdr);
DECLARE @hdr_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_mds_sales_forecast] R
  WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[sales_plan_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[product_category_code], N''))) <> N''
);
DECLARE @hdr_sap_keys INT = (
  SELECT COUNT(*) FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])),
      LTRIM(RTRIM(R.[sales_plan_code])),
      CAST(COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE)
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_mds_sales_forecast] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[sales_plan_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[product_category_code], N''))) <> N''
    GROUP BY
      LTRIM(RTRIM(R.[plant_code])),
      LTRIM(RTRIM(R.[sales_plan_code])),
      CAST(COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE)
  ) K
);
IF @hdr_source <> @hdr_sap_keys
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(
    N'主表业务键装入不一致: keys=', @hdr_sap_keys, N', loaded=', @hdr_source, N', sap_raw=', @hdr_sap_raw);
  THROW 50003, @hdr_src_msg, 1;
END;

INSERT INTO #item
SELECT
  S.rn, @base_id + 1000000000 + S.rn, 0,
  S.[plant_code], S.[sales_plan_code], S.[plan_date],
  S.[line_number], S.[fiscal_year], S.[plan_month],
  S.[plan_quantity_001], S.[plan_quantity_002], S.[plan_quantity_delta],
  S.[converted_quantity], S.[estimated_unit_price], S.[estimated_amount], S.[is_obsolete], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY N.[plant_code], N.[sales_plan_code], N.[plan_date], N.[fiscal_year], N.[plan_month]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LTRIM(RTRIM(R.[sales_plan_code])) AS [sales_plan_code],
      CAST(COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATETIME) AS [plan_date],
      COALESCE(
        TRY_CAST(R.[line_number] AS INT),
        COALESCE(TRY_CAST(R.[plan_month] AS INT), 1) * 10
      ) AS [line_number],
      LEFT(LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))), 6) AS [fiscal_year],
      CASE
        WHEN COALESCE(TRY_CAST(R.[plan_month] AS INT), 0) BETWEEN 1 AND 12
          THEN TRY_CAST(R.[plan_month] AS INT)
        ELSE 1
      END AS [plan_month],
      ROUND(COALESCE(TRY_CAST(R.[plan_quantity_001] AS DECIMAL(18,8)), 0), 4) AS [plan_quantity_001],
      ROUND(COALESCE(TRY_CAST(R.[plan_quantity_002] AS DECIMAL(18,8)), 0), 4) AS [plan_quantity_002],
      ROUND(
        COALESCE(TRY_CAST(R.[plan_quantity_002] AS DECIMAL(18,8)), 0)
        - COALESCE(TRY_CAST(R.[plan_quantity_001] AS DECIMAL(18,8)), 0)
      , 4) AS [plan_quantity_delta],
      ROUND(COALESCE(TRY_CAST(R.[converted_quantity] AS DECIMAL(18,8)), 0), 4) AS [converted_quantity],
      ROUND(COALESCE(TRY_CAST(R.[estimated_unit_price] AS DECIMAL(18,8)), 0), 2) AS [estimated_unit_price],
      ROUND(COALESCE(TRY_CAST(R.[estimated_amount] AS DECIMAL(18,8)), 0), 2) AS [estimated_amount],
      COALESCE(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[plant_code])),
          LTRIM(RTRIM(R.[sales_plan_code])),
          CAST(COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE),
          LEFT(LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))), 6),
          CASE
            WHEN COALESCE(TRY_CAST(R.[plan_month] AS INT), 0) BETWEEN 1 AND 12
              THEN TRY_CAST(R.[plan_month] AS INT)
            ELSE 1
          END
        ORDER BY ABS(
          COALESCE(TRY_CAST(R.[plan_quantity_002] AS DECIMAL(18,8)), 0)
          - COALESCE(TRY_CAST(R.[plan_quantity_001] AS DECIMAL(18,8)), 0)
        ) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_mds_sales_forecast_item] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[sales_plan_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))) <> N''
      AND EXISTS (
        SELECT 1 FROM #hdr H
        WHERE H.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
          AND H.[sales_plan_code] = LTRIM(RTRIM(R.[sales_plan_code]))
          AND CAST(H.[plan_date] AS DATE) = CAST(
            COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE)
      )
  ) N
  WHERE N.dup_rn = 1
) S;

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item);
DECLARE @item_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_mds_sales_forecast_item] R
  WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[sales_plan_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #hdr H
      WHERE H.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
        AND H.[sales_plan_code] = LTRIM(RTRIM(R.[sales_plan_code]))
        AND CAST(H.[plan_date] AS DATE) = CAST(
          COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE)
    )
);
DECLARE @item_sap_keys INT = (
  SELECT COUNT(*) FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])),
      LTRIM(RTRIM(R.[sales_plan_code])),
      CAST(COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE),
      LEFT(LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))), 6),
      CASE
        WHEN COALESCE(TRY_CAST(R.[plan_month] AS INT), 0) BETWEEN 1 AND 12
          THEN TRY_CAST(R.[plan_month] AS INT)
        ELSE 1
      END
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_mds_sales_forecast_item] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[sales_plan_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))) <> N''
      AND EXISTS (
        SELECT 1 FROM #hdr H
        WHERE H.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
          AND H.[sales_plan_code] = LTRIM(RTRIM(R.[sales_plan_code]))
          AND CAST(H.[plan_date] AS DATE) = CAST(
            COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE)
      )
    GROUP BY
      LTRIM(RTRIM(R.[plant_code])),
      LTRIM(RTRIM(R.[sales_plan_code])),
      CAST(COALESCE(TRY_CAST(R.[plan_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE),
      LEFT(LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))), 6),
      CASE
        WHEN COALESCE(TRY_CAST(R.[plan_month] AS INT), 0) BETWEEN 1 AND 12
          THEN TRY_CAST(R.[plan_month] AS INT)
        ELSE 1
      END
  ) K
);
IF @item_source <> @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(
    N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source, N', sap_raw=', @item_sap_raw);
  THROW 50003, @item_src_msg, 1;
END;

UPDATE S SET S.[id] = COALESCE(T.[id], S.[id])
FROM #hdr S
LEFT JOIN [takt_logistics_manufacturing_mds_sales_forecast] T
  ON T.[tenant_code] = @tenant_code AND T.[company_code] = @company_code
 AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
 AND LTRIM(RTRIM(T.[sales_plan_code])) = S.[sales_plan_code]
 AND CAST(T.[plan_date] AS DATE) = CAST(S.[plan_date] AS DATE);

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_mds_sales_forecast]
  WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0
);
DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_mds_sales_forecast_item]
  WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0
);

MERGE INTO [takt_logistics_manufacturing_mds_sales_forecast] AS T
USING #hdr AS S
ON T.[tenant_code] = @tenant_code AND T.[company_code]=@company_code
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code]
 AND LTRIM(RTRIM(T.[sales_plan_code]))=S.[sales_plan_code]
 AND CAST(T.[plan_date] AS DATE)=CAST(S.[plan_date] AS DATE)
WHEN MATCHED AND (
  ISNULL(T.[is_deleted],0)<>S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[sales_product],N'')))<>S.[sales_product]
  OR LTRIM(RTRIM(ISNULL(T.[product_category_code],N'')))<>S.[product_category_code]
  OR LTRIM(RTRIM(ISNULL(T.[profit_center_code],N'')))<>LTRIM(RTRIM(ISNULL(S.[profit_center_code],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[model_code],N'')))<>LTRIM(RTRIM(ISNULL(S.[model_code],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_code],N'')))<>S.[material_code]
  OR LTRIM(RTRIM(ISNULL(T.[material_description],N'')))<>S.[material_description]
  OR LTRIM(RTRIM(ISNULL(T.[customer_code],N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_code],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_name1],N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_name1],N'')))
  OR ISNULL(T.[planner_id], -1)<>ISNULL(S.[planner_id], -1)
  OR LTRIM(RTRIM(ISNULL(T.[plan_by],N'')))<>S.[plan_by]
  OR ROUND(T.[total_quantity],4)<>ROUND(S.[total_quantity],4)
  OR ROUND(T.[total_amount],2)<>ROUND(S.[total_amount],2)
  OR ROUND(T.[converted_quantity],4)<>ROUND(S.[converted_quantity],4)
  OR ROUND(T.[converted_amount],2)<>ROUND(S.[converted_amount],2)
  OR T.[plan_status]<>S.[plan_status]
  OR T.[converted_status]<>S.[converted_status]
  OR LTRIM(RTRIM(ISNULL(T.[plan_description],N'')))<>LTRIM(RTRIM(ISNULL(S.[plan_description],N'')))
) THEN UPDATE SET
  T.[sales_product]=S.[sales_product],
  T.[product_category_code]=S.[product_category_code],
  T.[profit_center_code]=S.[profit_center_code],
  T.[model_code]=S.[model_code],
  T.[material_code]=S.[material_code],
  T.[material_description]=S.[material_description],
  T.[customer_code]=S.[customer_code],
  T.[customer_name1]=S.[customer_name1],
  T.[planner_id]=S.[planner_id],
  T.[plan_by]=S.[plan_by],
  T.[total_quantity]=S.[total_quantity],
  T.[total_amount]=S.[total_amount],
  T.[converted_quantity]=S.[converted_quantity],
  T.[converted_amount]=S.[converted_amount],
  T.[plan_status]=S.[plan_status],
  T.[converted_status]=S.[converted_status],
  T.[plan_description]=S.[plan_description],
    T.[updated_by] =@sync_user_id, T.[updated_at]=@now,
  T.[is_deleted]=S.[is_deleted], T.[deleted_by]=CASE WHEN S.[is_deleted]=1 THEN @sync_user_id ELSE NULL END, T.[deleted_at]=CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN INSERT (
  [id],[plant_code],[sales_plan_code],[plan_date],
  [sales_product],[product_category_code],[profit_center_code],[model_code],
  [material_code],[material_description],[customer_code],[customer_name1],
  [planner_id],[plan_by],[total_quantity],[total_amount],
  [converted_quantity],[converted_amount],[plan_status],[converted_status],[plan_description],[tenant_code],[company_code],[ext_field],[remark],
  [approval_status],[approved_by],[approved_at],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
) VALUES (
  S.[id],S.[plant_code],S.[sales_plan_code],S.[plan_date],
  S.[sales_product],S.[product_category_code],S.[profit_center_code],S.[model_code],
  S.[material_code],S.[material_description],S.[customer_code],S.[customer_name1],
  S.[planner_id],S.[plan_by],S.[total_quantity],S.[total_amount],
  S.[converted_quantity],S.[converted_amount],S.[plan_status],S.[converted_status],S.[plan_description],@tenant_code,@company_code,N'{}',N'',
  2,@sync_user_id,@now,
  @sync_user_id,@now,@sync_user_id,@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN @sync_user_id ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[plant_code], INSERTED.[sales_plan_code], INSERTED.[plan_date]
INTO #hdr_delta(rn, oper_type, id, plant_code, sales_plan_code, plan_date);

UPDATE S SET S.[id]=T.[id]
FROM #hdr S
INNER JOIN [takt_logistics_manufacturing_mds_sales_forecast] T
  ON T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code]
 AND LTRIM(RTRIM(T.[sales_plan_code]))=S.[sales_plan_code]
 AND CAST(T.[plan_date] AS DATE)=CAST(S.[plan_date] AS DATE);

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now,
  T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_manufacturing_mds_sales_forecast] T
WHERE T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
  AND NOT EXISTS (
    SELECT 1 FROM #hdr S
    WHERE S.[plant_code]=LTRIM(RTRIM(T.[plant_code]))
      AND S.[sales_plan_code]=LTRIM(RTRIM(T.[sales_plan_code]))
      AND CAST(S.[plan_date] AS DATE)=CAST(T.[plan_date] AS DATE)
  );
DECLARE @hdr_del INT = @@ROWCOUNT;

UPDATE I SET I.[sales_plan_id]=H.[id]
FROM #item I
INNER JOIN #hdr H
  ON H.[plant_code]=I.[plant_code]
 AND H.[sales_plan_code]=I.[sales_plan_code]
 AND CAST(H.[plan_date] AS DATE)=CAST(I.[plan_date] AS DATE);
DELETE FROM #item WHERE [sales_plan_id]=0 OR [sales_plan_id] IS NULL;

UPDATE S SET S.[id]=COALESCE(T.[id], S.[id])
FROM #item S
LEFT JOIN [takt_logistics_manufacturing_mds_sales_forecast_item] T
  ON T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code
 AND T.[sales_plan_id]=S.[sales_plan_id]
 AND LTRIM(RTRIM(T.[fiscal_year]))=S.[fiscal_year]
 AND T.[plan_month]=S.[plan_month];

MERGE INTO [takt_logistics_manufacturing_mds_sales_forecast_item] AS T
USING #item AS S
ON T.[tenant_code] = @tenant_code AND T.[company_code]=@company_code
 AND T.[sales_plan_id]=S.[sales_plan_id]
 AND LTRIM(RTRIM(T.[fiscal_year]))=S.[fiscal_year]
 AND T.[plan_month]=S.[plan_month]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted],0)<>S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[sales_plan_code],N'')))<>S.[sales_plan_code]
  OR T.[line_number]<>S.[line_number]
  OR ROUND(T.[plan_quantity_001],4)<>ROUND(S.[plan_quantity_001],4)
  OR ROUND(T.[plan_quantity_002],4)<>ROUND(S.[plan_quantity_002],4)
  OR ROUND(T.[plan_quantity_delta],4)<>ROUND(S.[plan_quantity_delta],4)
  OR ROUND(T.[converted_quantity],4)<>ROUND(S.[converted_quantity],4)
  OR ROUND(T.[estimated_unit_price],2)<>ROUND(S.[estimated_unit_price],2)
  OR ROUND(T.[estimated_amount],2)<>ROUND(S.[estimated_amount],2)
  OR T.[is_obsolete]<>S.[is_obsolete]
) THEN UPDATE SET
  T.[sales_plan_code]=S.[sales_plan_code],
  T.[line_number]=S.[line_number],
  T.[plan_quantity_001]=S.[plan_quantity_001],
  T.[plan_quantity_002]=S.[plan_quantity_002],
  T.[plan_quantity_delta]=S.[plan_quantity_delta],
  T.[converted_quantity]=S.[converted_quantity],
  T.[estimated_unit_price]=S.[estimated_unit_price],
  T.[estimated_amount]=S.[estimated_amount],
  T.[is_obsolete]=S.[is_obsolete],
    T.[updated_by] =@sync_user_id, T.[updated_at]=@now,
  T.[is_deleted]=S.[is_deleted], T.[deleted_by]=CASE WHEN S.[is_deleted]=1 THEN @sync_user_id ELSE NULL END, T.[deleted_at]=CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN INSERT (
  [id],[sales_plan_id],[sales_plan_code],[line_number],
  [fiscal_year],[plan_month],
  [plan_quantity_001],[plan_quantity_002],[plan_quantity_delta],
  [converted_quantity],[estimated_unit_price],[estimated_amount],[is_obsolete],[tenant_code],[company_code],[ext_field],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
) VALUES (
  S.[id],S.[sales_plan_id],S.[sales_plan_code],S.[line_number],
  S.[fiscal_year],S.[plan_month],
  S.[plan_quantity_001],S.[plan_quantity_002],S.[plan_quantity_delta],
  S.[converted_quantity],S.[estimated_unit_price],S.[estimated_amount],S.[is_obsolete],@tenant_code,@company_code,N'{}',N'',
  @sync_user_id,@now,@sync_user_id,@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN @sync_user_id ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[sales_plan_code], INSERTED.[fiscal_year], INSERTED.[plan_month]
INTO #item_delta(rn, oper_type, id, sales_plan_code, fiscal_year, plan_month);

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now,
  T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_manufacturing_mds_sales_forecast_item] T
WHERE T.[tenant_code]=@tenant_code AND T.[company_code]=@company_code AND T.[is_deleted]=0
  AND NOT EXISTS (
    SELECT 1 FROM #item S
    WHERE S.[sales_plan_id]=T.[sales_plan_id]
      AND S.[fiscal_year]=LTRIM(RTRIM(T.[fiscal_year]))
      AND S.[plan_month]=T.[plan_month]
  );
DECLARE @item_del INT = @@ROWCOUNT;

DECLARE @hdr_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_mds_sales_forecast]
  WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0
);
DECLARE @item_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_mds_sales_forecast_item]
  WHERE [tenant_code]=@tenant_code AND [company_code]=@company_code AND [is_deleted]=0
);
DECLARE @hdr_ins INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type='INSERT');
DECLARE @hdr_upd INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type='UPDATE');
DECLARE @item_ins INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type='INSERT');
DECLARE @item_upd INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type='UPDATE');

IF @hdr_after <> @hdr_source
BEGIN
  DECLARE @hdr_cnt_msg NVARCHAR(200) = CONCAT(N'主表有效行不一致: source=', @hdr_source, N', active=', @hdr_after);
  THROW 50002, @hdr_cnt_msg, 1;
END;
IF @item_after <> @item_source
BEGIN
  DECLARE @item_cnt_msg NVARCHAR(200) = CONCAT(N'明细有效行不一致: source=', @item_source, N', active=', @item_after);
  THROW 50002, @item_cnt_msg, 1;
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
  + N',"item_soft_delete":' + CAST(@item_del AS NVARCHAR) + N'}';



INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[created_by],[created_at]
) VALUES (
  @base_id + 1, N'SYSTEM_SYNC', N'SYNC', N'销售预测',
  N'exec_sql_merge', 'SQL', N'/sync/sales-forecast', CONCAT('batch_size=', @batch_size),
  @json_result, '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now, DATEDIFF(MILLISECOND,@now,GETDATE()), 1, '',
  @tenant_code, @company_code, @sync_user_id, @now
);

SELECT N'QUARTZ_SYNC_SUMMARY' AS [summary_tag], CAST(N'hdr' AS NVARCHAR(40)) AS [scope],
  @hdr_sap_raw AS [source_raw_count], @hdr_source AS [source_count], @hdr_before AS [target_before],
  @hdr_after AS [target_after], @hdr_ins AS [insert_count], @hdr_upd AS [update_count], @hdr_del AS [delete_count]
UNION ALL SELECT N'QUARTZ_SYNC_SUMMARY', N'item',
  @item_sap_raw, @item_source, @item_before, @item_after, @item_ins, @item_upd, @item_del;
