SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体 TaktMaterialMovingPrice 一致
-- {{SourceDatabase}}.dbo.takt_logistics_materials_material_moving_price → 当前租户库同名表
-- 业务唯一键：Plant+ValuationPeriod+Material+Valuation

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [valuation_period] NVARCHAR(7),
  [material_code] NVARCHAR(20),
  [valuation] NVARCHAR(4),
  [stock_quantity] DECIMAL(18,4),
  [stock_amount] DECIMAL(18,2),
  [price_control] NVARCHAR(1),
  [moving_price] DECIMAL(18,5),
  [price_unit] INT,
  [currency_code] NVARCHAR(3),
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [is_deleted] INT,
  [updated_by] BIGINT
);

-- 源表装入：
-- 1) 物料码含全角括号（　）的不参与同步
-- 2) 18 位纯数字物料截末 10 位
-- 3) 按唯一键 Plant+Period+Material+Valuation 去重（库存金额→移动价→数量 较大者优先）
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[valuation_period],
  S.[material_code],
  S.[valuation],
  S.[stock_quantity],
  S.[stock_amount],
  S.[price_control],
  S.[moving_price],
  S.[price_unit],
  S.[currency_code],
  @tenant_code,
  @company_code,
  '{}',
  '',
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY N.[plant_code], N.[valuation_period], N.[material_code], N.[valuation]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))), 7) AS [valuation_period],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code],
      LTRIM(RTRIM(R.[valuation])) AS [valuation],
      COALESCE(TRY_CAST(R.[stock_quantity] AS DECIMAL(18,4)), 0) AS [stock_quantity],
      COALESCE(TRY_CAST(R.[stock_amount] AS DECIMAL(18,2)), 0) AS [stock_amount],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[price_control])), ''), 'V') AS [price_control],
      COALESCE(TRY_CAST(R.[moving_price] AS DECIMAL(18,5)), 0) AS [moving_price],
      COALESCE(TRY_CAST(R.[price_unit] AS INT), 1000) AS [price_unit],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[currency_code])), ''), 'CNY') AS [currency_code],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[plant_code])),
          LEFT(LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))), 7),
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
              AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
            ELSE LTRIM(RTRIM(R.[material_code]))
          END,
          LTRIM(RTRIM(R.[valuation]))
        ORDER BY
          COALESCE(TRY_CAST(R.[stock_amount] AS DECIMAL(18,2)), 0) DESC,
          COALESCE(TRY_CAST(R.[moving_price] AS DECIMAL(18,5)), 0) DESC,
          COALESCE(TRY_CAST(R.[stock_quantity] AS DECIMAL(18,4)), 0) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
      AND LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #st_source);
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price]);
DECLARE @sap_excluded_paren INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price] R
  WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) LIKE N'%（%'
     OR LTRIM(RTRIM(ISNULL(R.[material_code], N''))) LIKE N'%）%'
);
DECLARE @sap_eligible_count INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price] R
  WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
    AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
    AND LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))) <> N''
);
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))), 7) AS [valuation_period],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code],
      LTRIM(RTRIM(R.[valuation])) AS [valuation]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
      AND LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))) <> N''
    GROUP BY
      LTRIM(RTRIM(R.[plant_code])),
      LEFT(LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))), 7),
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END,
      LTRIM(RTRIM(R.[valuation]))
  ) K
);
DECLARE @dedupe_dropped INT = @sap_eligible_count - @sap_key_count;

-- 装入行数须等于「排除全角括号后」业务键去重行数
IF @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count, N', excluded_paren=', @sap_excluded_paren);
  THROW 50003, @src_msg, 1;
END;

-- 唯一键 = Plant+ValuationPeriod+Material+Valuation（去重后仍重复则失败）
IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [plant_code], [valuation_period], [material_code], [valuation]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT(
      [plant_code], N' / ',
      [valuation_period], N' / ',
      [material_code], N' / ', [valuation], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [plant_code], [valuation_period], [material_code], [valuation]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(4),
  material_code NVARCHAR(20),
  valuation NVARCHAR(4),
  tenant_code NVARCHAR(3),
  company_code NVARCHAR(4),
  change_by BIGINT,
  stock_quantity_old DECIMAL(18,4),
  stock_quantity_new DECIMAL(18,4),
  stock_amount_old DECIMAL(18,2),
  stock_amount_new DECIMAL(18,2),
  moving_price_old DECIMAL(18,5),
  moving_price_new DECIMAL(18,5),
  price_unit_old INT,
  price_unit_new INT,
  valuation_period_old NVARCHAR(7),
  valuation_period_new NVARCHAR(7)
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_moving_price]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_materials_material_moving_price] AS T
USING #st_source AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND T.[valuation_period] = S.[valuation_period]
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
AND LTRIM(RTRIM(T.[valuation])) = S.[valuation]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR ROUND(T.[stock_quantity], 4) <> ROUND(S.[stock_quantity], 4)
  OR ROUND(T.[stock_amount], 2) <> ROUND(S.[stock_amount], 2)
  OR LTRIM(RTRIM(ISNULL(T.[price_control], N''))) <> LTRIM(RTRIM(ISNULL(S.[price_control], N'')))
  OR ROUND(T.[moving_price], 5) <> ROUND(S.[moving_price], 5)
  OR T.[price_unit] <> S.[price_unit]
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[currency_code], N'')))
) THEN
  UPDATE SET
    T.[stock_quantity] = S.[stock_quantity],
    T.[stock_amount] = S.[stock_amount],
    T.[price_control] = S.[price_control],
    T.[moving_price] = S.[moving_price],
    T.[price_unit] = S.[price_unit],
    T.[currency_code] = S.[currency_code],
    T.[ext_field] = S.[ext_field],
    T.[remark] = S.[remark],
        T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[valuation_period],[material_code],[valuation],
    [stock_quantity],[stock_amount],[price_control],[moving_price],[price_unit],[currency_code],[tenant_code],[company_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[valuation_period],S.[material_code],S.[valuation],
    S.[stock_quantity],S.[stock_amount],S.[price_control],S.[moving_price],S.[price_unit],S.[currency_code],S.[tenant_code],S.[company_code],S.[ext_field],S.[remark],
    S.[updated_by],@now,S.[updated_by],@now,
    S.[is_deleted],
    CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[material_code],
  INSERTED.[valuation],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[stock_quantity], INSERTED.[stock_quantity],
  DELETED.[stock_amount], INSERTED.[stock_amount],
  DELETED.[moving_price], INSERTED.[moving_price],
  DELETED.[price_unit], INSERTED.[price_unit],
  DELETED.[valuation_period], INSERTED.[valuation_period]
INTO #delta(
  rn, oper_type, id, plant_code, material_code, valuation,
  tenant_code, company_code, change_by,
  stock_quantity_old, stock_quantity_new,
  stock_amount_old, stock_amount_new,
  moving_price_old, moving_price_new,
  price_unit_old, price_unit_new,
  valuation_period_old, valuation_period_new
);

IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [material_code] NVARCHAR(20),
  [valuation] NVARCHAR(4)
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
  INSERTED.[material_code],
  INSERTED.[valuation]
INTO #soft_deleted_rows ([id], [plant_code], [material_code], [valuation])
FROM [takt_logistics_materials_material_moving_price] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[valuation_period] = T.[valuation_period]
      AND S.[material_code] = LTRIM(RTRIM(T.[material_code]))
      AND S.[valuation] = LTRIM(RTRIM(T.[valuation]))
  );

DECLARE @delete_count INT = @@ROWCOUNT;
DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    ISNULL([plant_code], N''), N'/',
    ISNULL([material_code], N''), N'/',
    ISNULL([valuation], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_moving_price]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_moving_price]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_moving_price]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 1
);

IF @target_count <> @source_count
BEGIN
  DECLARE @count_msg NVARCHAR(200) = CONCAT(
    N'有效行数不一致: source=', @source_count, N', active=', @target_count);
  THROW 50002, @count_msg, 1;
END;

INSERT INTO [takt_statistics_logging_delta_log] (
  [id],[oper_type],[table_name],[primary_key_id],
  [before_data],[after_data],[diff_data],[sql_statement],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[tenant_code],[company_code],
  [ext_field],[remark],[created_by],[created_at]
)
SELECT
  @base_id + d.rn,
  d.oper_type,
  N'takt_logistics_materials_material_moving_price',
  d.id,
  ISNULL((
    SELECT
      d.stock_quantity_old AS [stock_quantity],
      d.stock_amount_old AS [stock_amount],
      d.moving_price_old AS [moving_price],
      d.price_unit_old AS [price_unit]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  (
    SELECT
      d.stock_quantity_new AS [stock_quantity],
      d.stock_amount_new AS [stock_amount],
      d.moving_price_new AS [moving_price],
      d.price_unit_new AS [price_unit]
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  ISNULL((
    SELECT
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.moving_price_old AS NVARCHAR), 'null') END AS [moving_price.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.moving_price_new AS NVARCHAR), 'null') END AS [moving_price.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.stock_quantity_old AS NVARCHAR), 'null') END AS [stock_quantity.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.stock_quantity_new AS NVARCHAR), 'null') END AS [stock_quantity.new]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  N'MERGE MaterialMovingPrice Sync',
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,0,
  d.tenant_code,d.company_code,'{}',N'SYNC',d.change_by,@now
FROM #delta d;

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"sap_raw":' + CAST(@sap_raw_count AS NVARCHAR)
  + N',"excluded_paren":' + CAST(@sap_excluded_paren AS NVARCHAR)
  + N',"sap_eligible":' + CAST(@sap_eligible_count AS NVARCHAR)
  + N',"sap_keys":' + CAST(@sap_key_count AS NVARCHAR)
  + N',"dedupe_dropped":' + CAST(@dedupe_dropped AS NVARCHAR)
  + N',"source":' + CAST(@source_count AS NVARCHAR)
  + N',"target_before":' + CAST(@target_before AS NVARCHAR)
  + N',"target_after":' + CAST(@target_count AS NVARCHAR)
  + N',"target_physical":' + CAST(@target_physical AS NVARCHAR)
  + N',"soft_deleted":' + CAST(@soft_deleted AS NVARCHAR)
  + N',"insert":' + CAST(@insert_count AS NVARCHAR)
  + N',"update":' + CAST(@update_count AS NVARCHAR)
  + N',"unchanged":' + CAST(@unchanged_count AS NVARCHAR)
  + N',"soft_delete_this_run":' + CAST(@delete_count AS NVARCHAR)
  + N',"soft_delete_keys":"' + REPLACE(@soft_deleted_keys, N'"', N'''') + N'"}';



INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC',
  N'SYNC',
  N'移动价格',
  N'exec_sql_merge',
  'SQL',
  N'/sync/material-moving-price',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @sap_raw_count AS [source_raw_count],
  @sap_excluded_paren AS [excluded_paren],
  @sap_eligible_count AS [sap_eligible_count],
  @sap_key_count AS [sap_key_count],
  @dedupe_dropped AS [dedupe_dropped],
  @source_count AS [source_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @target_physical AS [target_physical],
  @soft_deleted AS [soft_deleted],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count],
  @soft_deleted_keys AS [soft_deleted_keys];
