SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @plant_code NVARCHAR(4) = N'{{PlantCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- =============================================================================
-- 跨库同结构同步（服务端直读，无客户端中转）
-- {{SourceDatabase}}.dbo.takt_logistics_manufacturing_bom_material_cost_item
--   → #st_source（源侧规范化一次）→ MERGE → dbo 同名表
-- 性能要点：
--   1) 目标键列禁止 LTRIM/RTRIM/ISNULL/ROUND（必须走唯一索引 line_unique）
--   2) 可空键列 NULL→''、数量统一 5 位，保证与 #st_source 裸等值可命中
--   3) 不写海量 delta_log（仅 oper_log 摘要）
-- 业务唯一键：Plant+BomLevel+BomItemCode+Product+LineNumber+Component
--   +ComponentQuantity+BatchIndicator+ProductionRelated+PurchaseType+SpecialProcurementType+CostingDate
-- =============================================================================

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT NOT NULL,
  [id] BIGINT NOT NULL,
  [plant_code] NVARCHAR(4) NOT NULL,
  [bom_level] NVARCHAR(20) NOT NULL,
  [bom_item_code] NVARCHAR(4) NOT NULL,
  [product_code] NVARCHAR(20) NOT NULL,
  [line_number] INT NOT NULL,
  [product_description] NVARCHAR(40) NOT NULL,
  [component_code] NVARCHAR(20) NOT NULL,
  [component_description] NVARCHAR(40) NOT NULL,
  [component_quantity] DECIMAL(18,5) NOT NULL,
  [batch_indicator] NVARCHAR(1) NOT NULL,
  [production_related] NVARCHAR(1) NOT NULL,
  [purchase_type] NVARCHAR(1) NOT NULL,
  [special_procurement_type] NVARCHAR(50) NOT NULL,
  [profit_center_code] NVARCHAR(4) NOT NULL,
  [moving_average_price] DECIMAL(18,5) NOT NULL,
  [moving_price_unit] INT NOT NULL,
  [moving_price_currency_code] NVARCHAR(3) NOT NULL,
  [purchase_organization] NVARCHAR(4) NOT NULL,
  [purchase_group] NVARCHAR(3) NOT NULL,
  [supplier_code] NVARCHAR(10) NOT NULL,
  [net_purchase_price] DECIMAL(18,5) NOT NULL,
  [purchase_price_unit] INT NOT NULL,
  [purchase_currency_code] NVARCHAR(3) NOT NULL,
  [costing_date] DATETIME NOT NULL,
  [tenant_code] NVARCHAR(3) NOT NULL,
  [company_code] NVARCHAR(4) NOT NULL,
  [culture_code] NVARCHAR(5) NOT NULL,
  [ext_field] NVARCHAR(MAX) NOT NULL,
  [remark] NVARCHAR(MAX) NOT NULL,
  [is_deleted] INT NOT NULL,
  [created_at] DATETIME NULL,
  [updated_by] BIGINT NOT NULL
);

-- ② 源库 → #st_source（规范化 + 业务键去重；只做一次）
INSERT INTO #st_source (
  [rn],[id],[plant_code],[bom_level],[bom_item_code],[product_code],[line_number],[product_description],
  [component_code],[component_description],[component_quantity],
  [batch_indicator],[production_related],[purchase_type],[special_procurement_type],
  [profit_center_code],[moving_average_price],[moving_price_unit],[moving_price_currency_code],
  [purchase_organization],[purchase_group],[supplier_code],[net_purchase_price],
  [purchase_price_unit],[purchase_currency_code],[costing_date],
  [tenant_code],[company_code],[culture_code],[ext_field],[remark],[is_deleted],[created_at],[updated_by]
)
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[bom_level],
  S.[bom_item_code],
  S.[product_code],
  S.[line_number],
  S.[product_description],
  S.[component_code],
  S.[component_description],
  S.[component_quantity],
  S.[batch_indicator],
  S.[production_related],
  S.[purchase_type],
  S.[special_procurement_type],
  S.[profit_center_code],
  S.[moving_average_price],
  S.[moving_price_unit],
  S.[moving_price_currency_code],
  S.[purchase_organization],
  S.[purchase_group],
  S.[supplier_code],
  S.[net_purchase_price],
  S.[purchase_price_unit],
  S.[purchase_currency_code],
  S.[costing_date],
  S.[tenant_code],
  S.[company_code],
  S.[culture_code],
  N'{}',
  N'',
  S.[is_deleted],
  S.[created_at],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY
        N.[plant_code], N.[bom_level], N.[bom_item_code], N.[product_code], N.[line_number],
        N.[component_code], N.[component_quantity],
        N.[batch_indicator], N.[production_related], N.[purchase_type],
        N.[special_procurement_type], N.[costing_date]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[bom_level])), N''), N'') AS [bom_level],
      LTRIM(RTRIM(R.[bom_item_code])) AS [bom_item_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
          AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
        ELSE LTRIM(RTRIM(R.[product_code]))
      END AS [product_code],
      R.[line_number] AS [line_number],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[product_description])), N''), N'') AS [product_description],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[component_code]))) = 18
          AND LTRIM(RTRIM(R.[component_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[component_code])), 10)
        ELSE LTRIM(RTRIM(R.[component_code]))
      END AS [component_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[component_description])), N''), N'') AS [component_description],
      ROUND(COALESCE(TRY_CAST(R.[component_quantity] AS DECIMAL(18,8)), 0), 5) AS [component_quantity],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[batch_indicator])), N''), N'') AS [batch_indicator],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[production_related])), N''), N'') AS [production_related],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_type])), N''), N'F') AS [purchase_type],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[special_procurement_type])), N''), N'') AS [special_procurement_type],
      ISNULL(NULLIF(LTRIM(RTRIM(LEFT(R.[profit_center_code], 4))), N''), N'') AS [profit_center_code],
      ROUND(COALESCE(TRY_CAST(R.[moving_average_price] AS DECIMAL(18,8)), 0), 5) AS [moving_average_price],
      COALESCE(TRY_CAST(R.[moving_price_unit] AS INT), 1) AS [moving_price_unit],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[moving_price_currency_code])), N''), N'') AS [moving_price_currency_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_organization])), N''), N'') AS [purchase_organization],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_group])), N''), N'') AS [purchase_group],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[supplier_code])), N''), N'') AS [supplier_code],
      ROUND(COALESCE(TRY_CAST(R.[net_purchase_price] AS DECIMAL(18,8)), 0), 5) AS [net_purchase_price],
      COALESCE(TRY_CAST(R.[purchase_price_unit] AS INT), 1) AS [purchase_price_unit],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_currency_code])), N''), N'') AS [purchase_currency_code],
      R.[costing_date] AS [costing_date],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      R.[created_at] AS [created_at],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[plant_code])),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[bom_level])), N''), N''),
          LTRIM(RTRIM(R.[bom_item_code])),
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
              AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
            ELSE LTRIM(RTRIM(R.[product_code]))
          END,
          R.[line_number],
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[component_code]))) = 18
              AND LTRIM(RTRIM(R.[component_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[component_code])), 10)
            ELSE LTRIM(RTRIM(R.[component_code]))
          END,
          ROUND(COALESCE(TRY_CAST(R.[component_quantity] AS DECIMAL(18,8)), 0), 5),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[batch_indicator])), N''), N''),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[production_related])), N''), N''),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_type])), N''), N'F'),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[special_procurement_type])), N''), N''),
          R.[costing_date]
        ORDER BY
          ROUND(COALESCE(TRY_CAST(R.[moving_average_price] AS DECIMAL(18,8)), 0), 5) DESC,
          ROUND(COALESCE(TRY_CAST(R.[net_purchase_price] AS DECIMAL(18,8)), 0), 5) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost_item] R
    WHERE R.[costing_date] IS NOT NULL
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[product_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[component_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = @@ROWCOUNT;

CREATE UNIQUE CLUSTERED INDEX [ix_st_source_bc_uk] ON #st_source (
  [tenant_code], [company_code], [plant_code], [bom_level], [bom_item_code], [product_code], [line_number],
  [component_code], [component_quantity], [batch_indicator],
  [production_related], [purchase_type], [special_procurement_type], [costing_date]
);

-- ① 目标键列规范化（装入后按源隔离范围；否则 NULL/精度与源空串无法裸等值匹配）
UPDATE T
SET
  T.[bom_level] = ISNULL(T.[bom_level], N''),
  T.[batch_indicator] = ISNULL(T.[batch_indicator], N''),
  T.[production_related] = ISNULL(T.[production_related], N''),
  T.[special_procurement_type] = ISNULL(T.[special_procurement_type], N''),
  T.[component_quantity] = ROUND(T.[component_quantity], 5)
FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] T
WHERE EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
  AND (
    T.[bom_level] IS NULL
    OR T.[batch_indicator] IS NULL
    OR T.[production_related] IS NULL
    OR T.[special_procurement_type] IS NULL
    OR T.[component_quantity] <> ROUND(T.[component_quantity], 5)
  );

-- 源原始行数：同过滤条件简单 COUNT（无 GROUP BY / 去重变换）
DECLARE @sap_raw_count INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost_item] R
  WHERE R.[costing_date] IS NOT NULL
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[product_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[component_code], N''))) <> N''
);
DECLARE @sap_key_count INT = @source_count;
DECLARE @dedupe_dropped INT = @sap_raw_count - @sap_key_count;

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

IF OBJECT_ID('tempdb..#merge_action') IS NOT NULL DROP TABLE #merge_action;
CREATE TABLE #merge_action (
  [oper_type] NVARCHAR(10) NOT NULL
);

-- ③ MERGE：ON 全部裸等值（对齐唯一索引列序语义；禁止对 T 套函数）
MERGE INTO [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS T
USING #st_source AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND T.[plant_code] = S.[plant_code]
AND T.[bom_level] = S.[bom_level]
AND T.[bom_item_code] = S.[bom_item_code]
AND T.[product_code] = S.[product_code]
AND T.[line_number] = S.[line_number]
AND T.[component_code] = S.[component_code]
AND T.[component_quantity] = S.[component_quantity]
AND T.[batch_indicator] = S.[batch_indicator]
AND T.[production_related] = S.[production_related]
AND T.[purchase_type] = S.[purchase_type]
AND T.[special_procurement_type] = S.[special_procurement_type]
AND T.[costing_date] = S.[costing_date]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR ISNULL(T.[culture_code], N'') <> S.[culture_code]
  OR ISNULL(T.[product_description], N'') <> S.[product_description]
  OR ISNULL(T.[component_description], N'') <> S.[component_description]
  OR ISNULL(T.[profit_center_code], N'') <> S.[profit_center_code]
  OR T.[moving_average_price] <> S.[moving_average_price]
  OR T.[moving_price_unit] <> S.[moving_price_unit]
  OR ISNULL(T.[moving_price_currency_code], N'') <> S.[moving_price_currency_code]
  OR ISNULL(T.[purchase_organization], N'') <> S.[purchase_organization]
  OR ISNULL(T.[purchase_group], N'') <> S.[purchase_group]
  OR ISNULL(T.[supplier_code], N'') <> S.[supplier_code]
  OR T.[net_purchase_price] <> S.[net_purchase_price]
  OR T.[purchase_price_unit] <> S.[purchase_price_unit]
  OR ISNULL(T.[purchase_currency_code], N'') <> S.[purchase_currency_code]
) THEN
  UPDATE SET
  T.[product_description]=S.[product_description],
  T.[component_description]=S.[component_description],
  T.[profit_center_code]=S.[profit_center_code],
  T.[moving_average_price]=S.[moving_average_price],
  T.[moving_price_unit]=S.[moving_price_unit],
  T.[moving_price_currency_code]=S.[moving_price_currency_code],
  T.[purchase_organization]=S.[purchase_organization],
  T.[purchase_group]=S.[purchase_group],
  T.[supplier_code]=S.[supplier_code],
  T.[net_purchase_price]=S.[net_purchase_price],
  T.[purchase_price_unit]=S.[purchase_price_unit],
  T.[purchase_currency_code]=S.[purchase_currency_code],
  T.[culture_code]=S.[culture_code],
  T.[remark]=S.[remark],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=@now,
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
  T.[deleted_at]=CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END,
  T.[ext_field]=S.[ext_field]
WHEN NOT MATCHED BY TARGET THEN
  INSERT (
    [id],[plant_code],[bom_level],[bom_item_code],[product_code],[line_number],[product_description],
    [component_code],[component_description],[component_quantity],
    [batch_indicator],[production_related],[purchase_type],[special_procurement_type],
    [profit_center_code],[moving_average_price],[moving_price_unit],[moving_price_currency_code],
    [purchase_organization],[purchase_group],[supplier_code],[net_purchase_price],
    [purchase_price_unit],[purchase_currency_code],[costing_date],
    [tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[bom_level],S.[bom_item_code],S.[product_code],S.[line_number],S.[product_description],
    S.[component_code],S.[component_description],S.[component_quantity],
    S.[batch_indicator],S.[production_related],S.[purchase_type],S.[special_procurement_type],
    S.[profit_center_code],S.[moving_average_price],S.[moving_price_unit],S.[moving_price_currency_code],
    S.[purchase_organization],S.[purchase_group],S.[supplier_code],S.[net_purchase_price],
    S.[purchase_price_unit],S.[purchase_currency_code],S.[costing_date],
    S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
    S.[updated_by],COALESCE(S.[created_at], @now),S.[updated_by],@now,
    S.[is_deleted],
    CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
  )
OUTPUT $action INTO #merge_action ([oper_type]);

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #merge_action WHERE [oper_type] = N'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #merge_action WHERE [oper_type] = N'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;

-- ④ 软删：目标有效且源无（同样裸等值）
IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT NOT NULL,
  [plant_code] NVARCHAR(4) NULL,
  [product_code] NVARCHAR(20) NULL,
  [component_code] NVARCHAR(20) NULL
);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[product_code],
  INSERTED.[component_code]
INTO #soft_deleted_rows ([id], [plant_code], [product_code], [component_code])
FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] T
WHERE T.[is_deleted] = 0
  AND EXISTS (SELECT 1 FROM #st_source S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[tenant_code] = T.[tenant_code]
      AND S.[company_code] = T.[company_code]
      AND S.[plant_code] = T.[plant_code]
      AND S.[bom_level] = T.[bom_level]
      AND S.[bom_item_code] = T.[bom_item_code]
      AND S.[product_code] = T.[product_code]
      AND S.[line_number] = T.[line_number]
      AND S.[component_code] = T.[component_code]
      AND S.[component_quantity] = T.[component_quantity]
      AND S.[batch_indicator] = T.[batch_indicator]
      AND S.[production_related] = T.[production_related]
      AND S.[purchase_type] = T.[purchase_type]
      AND S.[special_procurement_type] = T.[special_procurement_type]
      AND S.[costing_date] = T.[costing_date]
  );

DECLARE @delete_count INT = @@ROWCOUNT;

DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
      CAST([id] AS NVARCHAR(30)), N'|',
      ISNULL([plant_code], N''), N'/',
      ISNULL([product_code], N''), N'/',
      ISNULL([component_code], N'')
    ) AS NVARCHAR(MAX)
  ),
  N'; '
)
FROM (
  SELECT TOP (100) [id], [plant_code], [product_code], [component_code]
  FROM #soft_deleted_rows
  ORDER BY [id]
) SoftSample;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
IF @delete_count > 100
  SET @soft_deleted_keys = CONCAT(@soft_deleted_keys, N'; ...(+', CAST(@delete_count - 100 AS NVARCHAR(20)), N')');

DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @source_active_count INT = (
  SELECT COUNT(*) FROM #st_source WHERE [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost_item] T
  WHERE EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost_item] T
  WHERE T.[is_deleted]=1
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

IF @target_count <> @source_active_count
BEGIN
  DECLARE @count_msg NVARCHAR(200) = CONCAT(
    N'有效行数不一致: source_active=', @source_active_count, N', target_active=', @target_count);
  THROW 50002, @count_msg, 1;
END;

DECLARE @json_result NVARCHAR(MAX) =
  N'{"sap_raw":' + CAST(@sap_raw_count AS NVARCHAR(20))
  + N',"source":' + CAST(@source_count AS NVARCHAR(20))
  + N',"sap_keys":' + CAST(@sap_key_count AS NVARCHAR(20))
  + N',"dedupe_dropped":' + CAST(@dedupe_dropped AS NVARCHAR(20))
  + N',"target_before":' + CAST(@target_before AS NVARCHAR(20))
  + N',"target_after":' + CAST(@target_count AS NVARCHAR(20))
  + N',"target_physical":' + CAST(@target_physical AS NVARCHAR(20))
  + N',"soft_deleted":' + CAST(@soft_deleted AS NVARCHAR(20))
  + N',"insert":' + CAST(@insert_count AS NVARCHAR(20))
  + N',"update":' + CAST(@update_count AS NVARCHAR(20))
  + N',"unchanged":' + CAST(@unchanged_count AS NVARCHAR(20))
  + N',"soft_delete_this_run":' + CAST(@delete_count AS NVARCHAR(20))
  + N',"soft_delete_keys":"' + REPLACE(ISNULL(@soft_deleted_keys, N''), N'"', N'''') + N'"}';

INSERT INTO [dbo].[takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[plant_code],[culture_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC',
  N'SYNC',
  N'BOM物料成本明细',
  N'exec_sql_merge_eq',
  'SQL',
  N'/sync/bom-material-cost-item',
  CONCAT(N'batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,N'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @sap_raw_count AS [source_raw_count],
  @source_count AS [source_count],
  @sap_key_count AS [sap_key_count],
  @dedupe_dropped AS [dedupe_dropped],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @target_physical AS [target_physical],
  @soft_deleted AS [soft_deleted],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count],
  @soft_deleted_keys AS [soft_deleted_keys];
