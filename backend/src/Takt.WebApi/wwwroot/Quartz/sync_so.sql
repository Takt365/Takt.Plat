SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @plant_code NVARCHAR(4) = N'{{PlantCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源/目标列与实体 TaktSalesOrder / TaktSalesOrderItem 一致
-- {{SourceDatabase}}.dbo.takt_logistics_sales_order[_item] → 当前租户库同名表
-- 主表唯一键：Tenant+Company+Plant+sales_order_code
-- 明细唯一键：sales_order_id+line_number
-- 源明细 FK：先按 plant+sales_order_code 回填 sales_order_id，再 SH.id=R.sales_order_id 装入
-- tenant/company/plant/culture 取自各源表本列；任一层为空则丢弃，不回退任务参数

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#hdr_soft') IS NOT NULL DROP TABLE #hdr_soft;
IF OBJECT_ID('tempdb..#item_soft') IS NOT NULL DROP TABLE #item_soft;

CREATE TABLE #hdr (
  [rn] INT, [id] BIGINT,
  [company_code] NVARCHAR(4), [plant_code] NVARCHAR(4), [tenant_code] NVARCHAR(3), [culture_code] NVARCHAR(5),
  [sales_order_code] NVARCHAR(20),
  [customer_code] NVARCHAR(10), [customer_name1] NVARCHAR(140),
  [order_date] DATETIME, [required_delivery_date] DATETIME, [actual_delivery_date] DATETIME,
  [sales_by] NVARCHAR(50),
  [total_quantity] DECIMAL(18,4), [total_amount] DECIMAL(18,2), [discount_amount] DECIMAL(18,2),
  [currency_code] NVARCHAR(3), [exchange_rate] DECIMAL(18,5),
  [tax_code] NVARCHAR(4), [tax_rate] INT, [tax_amount] DECIMAL(18,2), [actual_amount] DECIMAL(18,2),
  [shipped_quantity] DECIMAL(18,4), [shipped_amount] DECIMAL(18,2), [received_amount] DECIMAL(18,2),
  [delivery_method] INT, [payment_method] INT, [delivery_address] NVARCHAR(500),
  [order_status] INT, [delivery_status] INT,
  [is_deleted] INT, [created_at] DATETIME, [updated_by] BIGINT
);

CREATE TABLE #item (
  [rn] INT, [id] BIGINT, [sales_order_id] BIGINT,
  [company_code] NVARCHAR(4), [plant_code] NVARCHAR(4), [tenant_code] NVARCHAR(3), [culture_code] NVARCHAR(5),
  [sales_order_code] NVARCHAR(20), [line_number] INT,
  [material_code] NVARCHAR(20), [material_description] NVARCHAR(40), [material_specification] NVARCHAR(70),
  [sales_unit] NVARCHAR(5),
  [order_quantity] DECIMAL(18,5), [shipped_quantity] DECIMAL(18,5),
  [sales_per_unit] INT, [sales_unit_price] DECIMAL(18,5),
  [discount_rate] DECIMAL(5,2), [discount_amount] DECIMAL(18,5),
  [tax_included_amount] DECIMAL(18,5), [untaxed_amount] DECIMAL(18,5), [tax_amount] DECIMAL(18,5),
  [sales_amount] DECIMAL(18,5),
  [delivery_status] INT, [is_obsolete] INT,
  [is_deleted] INT, [created_at] DATETIME, [updated_by] BIGINT
);

CREATE TABLE #hdr_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [plant_code] NVARCHAR(4), [sales_order_code] NVARCHAR(20)
);
CREATE TABLE #item_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [sales_order_code] NVARCHAR(20), [line_number] INT
);
CREATE TABLE #hdr_soft (
  [id] BIGINT, [plant_code] NVARCHAR(4), [sales_order_code] NVARCHAR(20)
);
CREATE TABLE #item_soft (
  [id] BIGINT, [sales_order_code] NVARCHAR(20), [line_number] INT
);

INSERT INTO #hdr
SELECT S.rn, @base_id + S.rn,
  S.[company_code], S.[plant_code], S.[tenant_code], S.[culture_code], S.[sales_order_code],
  S.[customer_code], S.[customer_name1],
  S.[order_date], S.[required_delivery_date], S.[actual_delivery_date],
  S.[sales_by],
  S.[total_quantity], S.[total_amount], S.[discount_amount],
  S.[currency_code], S.[exchange_rate],
  S.[tax_code], S.[tax_rate], S.[tax_amount], S.[actual_amount],
  S.[shipped_quantity], S.[shipped_amount], S.[received_amount],
  S.[delivery_method], S.[payment_method], S.[delivery_address],
  S.[order_status], S.[delivery_status],
  S.[is_deleted], S.[created_at], @sync_user_id
FROM (
  SELECT N.*, ROW_NUMBER() OVER (ORDER BY N.[plant_code], N.[sales_order_code]) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[sales_order_code])), 20), N''), N'') AS [sales_order_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[customer_code])), 10), N''), N'') AS [customer_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[customer_name1])), 140), N''), N'') AS [customer_name1],
      ISNULL(TRY_CAST(R.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS [order_date],
      TRY_CAST(R.[required_delivery_date] AS DATETIME) AS [required_delivery_date],
      TRY_CAST(R.[actual_delivery_date] AS DATETIME) AS [actual_delivery_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_by])), 50), N'') AS [sales_by],
      ROUND(COALESCE(TRY_CAST(R.[total_quantity] AS DECIMAL(18,8)), 0), 4) AS [total_quantity],
      ROUND(COALESCE(TRY_CAST(R.[total_amount] AS DECIMAL(18,8)), 0), 2) AS [total_amount],
      ROUND(COALESCE(TRY_CAST(R.[discount_amount] AS DECIMAL(18,8)), 0), 2) AS [discount_amount],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N''), N'') AS [currency_code],
      ROUND(COALESCE(TRY_CAST(R.[exchange_rate] AS DECIMAL(18,8)), 1), 5) AS [exchange_rate],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_code])), 4), N'') AS [tax_code],
      COALESCE(TRY_CAST(R.[tax_rate] AS INT), 0) AS [tax_rate],
      ROUND(COALESCE(TRY_CAST(R.[tax_amount] AS DECIMAL(18,8)), 0), 2) AS [tax_amount],
      ROUND(COALESCE(TRY_CAST(R.[actual_amount] AS DECIMAL(18,8)), 0), 2) AS [actual_amount],
      ROUND(COALESCE(TRY_CAST(R.[shipped_quantity] AS DECIMAL(18,8)), 0), 4) AS [shipped_quantity],
      ROUND(COALESCE(TRY_CAST(R.[shipped_amount] AS DECIMAL(18,8)), 0), 2) AS [shipped_amount],
      ROUND(COALESCE(TRY_CAST(R.[received_amount] AS DECIMAL(18,8)), 0), 2) AS [received_amount],
      COALESCE(TRY_CAST(R.[delivery_method] AS INT), 0) AS [delivery_method],
      COALESCE(TRY_CAST(R.[payment_method] AS INT), 0) AS [payment_method],
      NULLIF(LEFT(LTRIM(RTRIM(R.[delivery_address])), 500), N'') AS [delivery_address],
      COALESCE(TRY_CAST(R.[order_status] AS INT), 1) AS [order_status],
      COALESCE(TRY_CAST(R.[delivery_status] AS INT), 0) AS [delivery_status],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      R.[created_at] AS [created_at],
      ROW_NUMBER() OVER (
        PARTITION BY
          LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4),
          LTRIM(RTRIM(R.[sales_order_code]))
        ORDER BY R.[id]
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order] R
    WHERE LTRIM(RTRIM(ISNULL(R.[sales_order_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[customer_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @hdr_source INT = (SELECT COUNT(*) FROM #hdr);
DECLARE @hdr_sap_keys INT = (
  SELECT COUNT(*) FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LTRIM(RTRIM(R.[sales_order_code])) AS [sales_order_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order] R
    WHERE LTRIM(RTRIM(ISNULL(R.[sales_order_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[customer_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
    GROUP BY
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4),
      LTRIM(RTRIM(R.[sales_order_code]))
  ) K
);
IF @hdr_source <> @hdr_sap_keys
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(N'主表业务键装入不一致: keys=', @hdr_sap_keys, N', loaded=', @hdr_source);
  THROW 50003, @hdr_src_msg, 1;
END;

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_order] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

MERGE [takt_logistics_sales_order] AS T
USING #hdr AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code]
 AND LTRIM(RTRIM(T.[sales_order_code]))=S.[sales_order_code]
WHEN MATCHED AND (
  T.[customer_code]<>S.[customer_code] OR T.[customer_name1]<>S.[customer_name1]
  OR T.[order_date]<>S.[order_date]
  OR ISNULL(T.[required_delivery_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[required_delivery_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[actual_delivery_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[actual_delivery_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[sales_by],N'')<>ISNULL(S.[sales_by],N'')
  OR T.[total_quantity]<>S.[total_quantity] OR T.[total_amount]<>S.[total_amount] OR T.[discount_amount]<>S.[discount_amount]
  OR T.[currency_code]<>S.[currency_code] OR T.[exchange_rate]<>S.[exchange_rate]
  OR ISNULL(T.[tax_code],N'')<>ISNULL(S.[tax_code],N'') OR T.[tax_rate]<>S.[tax_rate]
  OR T.[tax_amount]<>S.[tax_amount] OR T.[actual_amount]<>S.[actual_amount]
  OR T.[shipped_quantity]<>S.[shipped_quantity] OR T.[shipped_amount]<>S.[shipped_amount] OR T.[received_amount]<>S.[received_amount]
  OR T.[delivery_method]<>S.[delivery_method] OR T.[payment_method]<>S.[payment_method]
  OR ISNULL(T.[delivery_address],N'')<>ISNULL(S.[delivery_address],N'')
  OR T.[order_status]<>S.[order_status] OR T.[delivery_status]<>S.[delivery_status]
  OR ISNULL(T.[culture_code],N'')<>ISNULL(S.[culture_code],N'') OR T.[is_deleted]<>S.[is_deleted]
) THEN UPDATE SET
  T.[customer_code]=S.[customer_code],
  T.[customer_name1]=S.[customer_name1],
  T.[order_date]=S.[order_date],
  T.[required_delivery_date]=S.[required_delivery_date],
  T.[actual_delivery_date]=S.[actual_delivery_date],
  T.[sales_by]=S.[sales_by],
  T.[total_quantity]=S.[total_quantity],
  T.[total_amount]=S.[total_amount],
  T.[discount_amount]=S.[discount_amount],
  T.[currency_code]=S.[currency_code],
  T.[exchange_rate]=S.[exchange_rate],
  T.[tax_code]=S.[tax_code],
  T.[tax_rate]=S.[tax_rate],
  T.[tax_amount]=S.[tax_amount],
  T.[actual_amount]=S.[actual_amount],
  T.[shipped_quantity]=S.[shipped_quantity],
  T.[shipped_amount]=S.[shipped_amount],
  T.[received_amount]=S.[received_amount],
  T.[delivery_method]=S.[delivery_method],
  T.[payment_method]=S.[payment_method],
  T.[delivery_address]=S.[delivery_address],
  T.[order_status]=S.[order_status],
  T.[delivery_status]=S.[delivery_status],
  T.[culture_code]=S.[culture_code],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=@now,
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,
  T.[deleted_at]=CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT ([id],[plant_code],[sales_order_code],[customer_code],[customer_name1],[order_date],[required_delivery_date],[actual_delivery_date],[sales_by],[total_quantity],[total_amount],[discount_amount],[currency_code],[exchange_rate],[tax_code],[tax_rate],[tax_amount],[actual_amount],[shipped_quantity],[shipped_amount],[received_amount],[delivery_method],[payment_method],[delivery_address],[order_status],[delivery_status],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[plant_code],S.[sales_order_code],S.[customer_code],S.[customer_name1],S.[order_date],S.[required_delivery_date],S.[actual_delivery_date],S.[sales_by],S.[total_quantity],S.[total_amount],S.[discount_amount],S.[currency_code],S.[exchange_rate],S.[tax_code],S.[tax_rate],S.[tax_amount],S.[actual_amount],S.[shipped_quantity],S.[shipped_amount],S.[received_amount],S.[delivery_method],S.[payment_method],S.[delivery_address],S.[order_status],S.[delivery_status],S.[tenant_code],S.[company_code],S.[culture_code],N'{}',N'',S.[updated_by],COALESCE(S.[created_at],@now),S.[updated_by],@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[plant_code], INSERTED.[sales_order_code]
INTO #hdr_delta (rn, oper_type, id, [plant_code], [sales_order_code]);

UPDATE S SET S.[id]=T.[id]
FROM #hdr S
INNER JOIN [takt_logistics_sales_order] T
  ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code]
 AND LTRIM(RTRIM(T.[sales_order_code]))=S.[sales_order_code];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
OUTPUT INSERTED.[id], INSERTED.[plant_code], INSERTED.[sales_order_code]
INTO #hdr_soft ([id], [plant_code], [sales_order_code])
FROM [takt_logistics_sales_order] T
WHERE T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #hdr S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (SELECT 1 FROM #hdr S WHERE S.[id]=T.[id]);
DECLARE @hdr_delete INT = @@ROWCOUNT;

UPDATE I
SET I.[sales_order_id] = H.[id]
FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order_item] AS I
INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order] AS H
  ON LTRIM(RTRIM(H.[sales_order_code])) = LTRIM(RTRIM(I.[sales_order_code]))
 AND LEFT(LTRIM(RTRIM(ISNULL(H.[plant_code], N''))), 4)
   = LEFT(LTRIM(RTRIM(ISNULL(I.[plant_code], N''))), 4)
WHERE I.[sales_order_id] IS NULL OR I.[sales_order_id] <> H.[id];

INSERT INTO #item
SELECT S.rn, @base_id+1000000000+S.rn, 0,
  S.[company_code], S.[plant_code], S.[tenant_code], S.[culture_code], S.[sales_order_code], S.[line_number],
  S.[material_code], S.[material_description], S.[material_specification],
  S.[sales_unit], S.[order_quantity], S.[shipped_quantity],
  S.[sales_per_unit], S.[sales_unit_price],
  S.[discount_rate], S.[discount_amount],
  S.[tax_included_amount], S.[untaxed_amount], S.[tax_amount], S.[sales_amount],
  S.[delivery_status], S.[is_obsolete],
  S.[is_deleted], S.[created_at], @sync_user_id
FROM (
  SELECT N.*, ROW_NUMBER() OVER (ORDER BY N.[plant_code], N.[sales_order_code], N.[line_number]) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[sales_order_code])), 20), N''), N'') AS [sales_order_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[material_code], N''))))=18 AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N''), N'')
      END AS [material_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[material_description], N''))), 40) AS [material_description],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_specification])), 70), N'') AS [material_specification],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[sales_unit])), 5), N''), N'') AS [sales_unit],
      ROUND(COALESCE(TRY_CAST(R.[order_quantity] AS DECIMAL(18,8)), 0), 5) AS [order_quantity],
      ROUND(COALESCE(TRY_CAST(R.[shipped_quantity] AS DECIMAL(18,8)), 0), 5) AS [shipped_quantity],
      COALESCE(TRY_CAST(R.[sales_per_unit] AS INT), 0) AS [sales_per_unit],
      ROUND(COALESCE(TRY_CAST(R.[sales_unit_price] AS DECIMAL(18,8)), 0), 5) AS [sales_unit_price],
      ROUND(COALESCE(TRY_CAST(R.[discount_rate] AS DECIMAL(18,8)), 0), 2) AS [discount_rate],
      ROUND(COALESCE(TRY_CAST(R.[discount_amount] AS DECIMAL(18,8)), 0), 5) AS [discount_amount],
      ROUND(COALESCE(TRY_CAST(R.[tax_included_amount] AS DECIMAL(18,8)), 0), 5) AS [tax_included_amount],
      ROUND(COALESCE(TRY_CAST(R.[untaxed_amount] AS DECIMAL(18,8)), 0), 5) AS [untaxed_amount],
      ROUND(COALESCE(TRY_CAST(R.[tax_amount] AS DECIMAL(18,8)), 0), 5) AS [tax_amount],
      ROUND(COALESCE(TRY_CAST(R.[sales_amount] AS DECIMAL(18,8)), 0), 5) AS [sales_amount],
      COALESCE(TRY_CAST(R.[delivery_status] AS INT), 0) AS [delivery_status],
      ISNULL(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
      CASE WHEN ISNULL(R.[is_deleted], 0)=0 THEN 0 ELSE 1 END AS [is_deleted],
      R.[created_at] AS [created_at],
      ROW_NUMBER() OVER (
        PARTITION BY LTRIM(RTRIM(R.[sales_order_code])), COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
        ORDER BY COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order] SH ON SH.[id]=R.[sales_order_id]
    WHERE LTRIM(RTRIM(ISNULL(R.[sales_order_code], N'')))<>N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0)>0
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N'')))<>N''
  ) N
  WHERE N.dup_rn=1
) S
WHERE @batch_size=0 OR S.rn<=@batch_size;

UPDATE I SET I.[sales_order_id]=H.[id], I.[id]=COALESCE(T.[id], I.[id]), I.[plant_code]=H.[plant_code]
FROM #item I
INNER JOIN #hdr H
  ON H.[plant_code]=I.[plant_code] AND H.[sales_order_code]=I.[sales_order_code]
LEFT JOIN [takt_logistics_sales_order_item] T
  ON T.[tenant_code]=I.[tenant_code] AND T.[company_code]=I.[company_code]
 AND T.[sales_order_id]=H.[id] AND T.[line_number]=I.[line_number];

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item WHERE [sales_order_id]<>0);
DECLARE @item_sap_keys INT = (
  SELECT COUNT(*) FROM (
    SELECT LTRIM(RTRIM(R.[sales_order_code])) AS [sales_order_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT),0) AS [line_number]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order] SH ON SH.[id]=R.[sales_order_id]
    WHERE LTRIM(RTRIM(ISNULL(R.[sales_order_code], N'')))<>N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT),0)>0
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N'')))<>N''
    GROUP BY LTRIM(RTRIM(R.[sales_order_code])), COALESCE(TRY_CAST(R.[line_number] AS INT),0)
  ) K
);
IF @item_source <> @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;
DELETE FROM #item WHERE [sales_order_id]=0;

DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_order_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

MERGE [takt_logistics_sales_order_item] AS T
USING #item AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND T.[sales_order_id]=S.[sales_order_id] AND T.[line_number]=S.[line_number]
WHEN MATCHED AND (
  T.[material_code]<>S.[material_code]
  OR ISNULL(T.[material_description],N'')<>S.[material_description]
  OR ISNULL(T.[material_specification],N'')<>ISNULL(S.[material_specification],N'')
  OR T.[sales_unit]<>S.[sales_unit]
  OR T.[order_quantity]<>S.[order_quantity] OR T.[shipped_quantity]<>S.[shipped_quantity]
  OR T.[sales_per_unit]<>S.[sales_per_unit] OR T.[sales_unit_price]<>S.[sales_unit_price]
  OR T.[discount_rate]<>S.[discount_rate] OR T.[discount_amount]<>S.[discount_amount]
  OR T.[tax_included_amount]<>S.[tax_included_amount] OR T.[untaxed_amount]<>S.[untaxed_amount]
  OR T.[tax_amount]<>S.[tax_amount] OR T.[sales_amount]<>S.[sales_amount]
  OR T.[delivery_status]<>S.[delivery_status] OR T.[is_obsolete]<>S.[is_obsolete]
  OR ISNULL(T.[plant_code],N'')<>S.[plant_code] OR ISNULL(T.[culture_code],N'')<>ISNULL(S.[culture_code],N'')
  OR T.[is_deleted]<>S.[is_deleted]
) THEN UPDATE SET
  T.[sales_order_code]=S.[sales_order_code],
  T.[material_code]=S.[material_code],
  T.[material_description]=S.[material_description],
  T.[material_specification]=S.[material_specification],
  T.[sales_unit]=S.[sales_unit],
  T.[order_quantity]=S.[order_quantity],
  T.[shipped_quantity]=S.[shipped_quantity],
  T.[sales_per_unit]=S.[sales_per_unit],
  T.[sales_unit_price]=S.[sales_unit_price],
  T.[discount_rate]=S.[discount_rate],
  T.[discount_amount]=S.[discount_amount],
  T.[tax_included_amount]=S.[tax_included_amount],
  T.[untaxed_amount]=S.[untaxed_amount],
  T.[tax_amount]=S.[tax_amount],
  T.[sales_amount]=S.[sales_amount],
  T.[delivery_status]=S.[delivery_status],
  T.[is_obsolete]=S.[is_obsolete],
  T.[plant_code]=S.[plant_code],
  T.[culture_code]=S.[culture_code],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=@now,
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,
  T.[deleted_at]=CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT ([id],[sales_order_id],[plant_code],[sales_order_code],[line_number],[material_code],[material_description],[material_specification],[sales_unit],[order_quantity],[shipped_quantity],[sales_per_unit],[sales_unit_price],[discount_rate],[discount_amount],[tax_included_amount],[untaxed_amount],[tax_amount],[sales_amount],[delivery_status],[is_obsolete],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[sales_order_id],S.[plant_code],S.[sales_order_code],S.[line_number],S.[material_code],S.[material_description],S.[material_specification],S.[sales_unit],S.[order_quantity],S.[shipped_quantity],S.[sales_per_unit],S.[sales_unit_price],S.[discount_rate],S.[discount_amount],S.[tax_included_amount],S.[untaxed_amount],S.[tax_amount],S.[sales_amount],S.[delivery_status],S.[is_obsolete],S.[tenant_code],S.[company_code],S.[culture_code],N'{}',N'',S.[updated_by],COALESCE(S.[created_at],@now),S.[updated_by],@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[sales_order_code], INSERTED.[line_number]
INTO #item_delta (rn, oper_type, id, [sales_order_code], [line_number]);

UPDATE I SET I.[id]=T.[id]
FROM #item I
INNER JOIN [takt_logistics_sales_order_item] T
  ON T.[tenant_code]=I.[tenant_code] AND T.[company_code]=I.[company_code]
 AND T.[sales_order_id]=I.[sales_order_id] AND T.[line_number]=I.[line_number];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
OUTPUT INSERTED.[id], INSERTED.[sales_order_code], INSERTED.[line_number]
INTO #item_soft ([id], [sales_order_code], [line_number])
FROM [takt_logistics_sales_order_item] T
WHERE T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #item S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (SELECT 1 FROM #item S WHERE S.[id]=T.[id]);
DECLARE @item_delete INT = @@ROWCOUNT;

DECLARE @hdr_source_active INT = (SELECT COUNT(*) FROM #hdr WHERE [is_deleted]=0);
DECLARE @item_source_active INT = (SELECT COUNT(*) FROM #item WHERE [is_deleted]=0);
DECLARE @hdr_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_order] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_order_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
IF @hdr_after <> @hdr_source_active BEGIN DECLARE @hdr_cnt NVARCHAR(200)=CONCAT(N'主表有效行数不一致: source=',@hdr_source_active,N', active=',@hdr_after); THROW 50002,@hdr_cnt,1; END;
IF @item_after <> @item_source_active BEGIN DECLARE @item_cnt NVARCHAR(200)=CONCAT(N'明细有效行数不一致: source=',@item_source_active,N', active=',@item_after); THROW 50002,@item_cnt,1; END;

DECLARE @hdr_ins INT=(SELECT COUNT(*) FROM #hdr_delta WHERE oper_type=N'INSERT');
DECLARE @hdr_upd INT=(SELECT COUNT(*) FROM #hdr_delta WHERE oper_type=N'UPDATE');
DECLARE @hdr_unchanged INT=@hdr_source-@hdr_ins-@hdr_upd;
DECLARE @item_ins INT=(SELECT COUNT(*) FROM #item_delta WHERE oper_type=N'INSERT');
DECLARE @item_upd INT=(SELECT COUNT(*) FROM #item_delta WHERE oper_type=N'UPDATE');
DECLARE @item_unchanged INT=@item_source-@item_ins-@item_upd;

DECLARE @hdr_soft_keys NVARCHAR(MAX)=N'';
SELECT @hdr_soft_keys=STRING_AGG(CAST(CONCAT(CAST([id] AS NVARCHAR(30)),N'|',ISNULL([plant_code],N''),N'/',ISNULL([sales_order_code],N'')) AS NVARCHAR(MAX)), N'; ')
FROM (SELECT TOP (100) * FROM #hdr_soft ORDER BY [id]) SoftSample;
SET @hdr_soft_keys=ISNULL(@hdr_soft_keys,N'');
IF @hdr_delete>100 SET @hdr_soft_keys=CONCAT(@hdr_soft_keys,N'; ...(+',CAST(@hdr_delete-100 AS NVARCHAR(20)),N')');

DECLARE @item_soft_keys NVARCHAR(MAX)=N'';
SELECT @item_soft_keys=STRING_AGG(CAST(CONCAT(CAST([id] AS NVARCHAR(30)),N'|',ISNULL([sales_order_code],N''),N'/',CAST([line_number] AS NVARCHAR(20))) AS NVARCHAR(MAX)), N'; ')
FROM (SELECT TOP (100) * FROM #item_soft ORDER BY [id]) SoftSample;
SET @item_soft_keys=ISNULL(@item_soft_keys,N'');
IF @item_delete>100 SET @item_soft_keys=CONCAT(@item_soft_keys,N'; ...(+',CAST(@item_delete-100 AS NVARCHAR(20)),N')');

DECLARE @hdr_sap_raw INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order]);
DECLARE @item_sap_raw INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_order_item]);
DECLARE @hdr_physical INT = (SELECT COUNT(*) FROM [takt_logistics_sales_order] T WHERE EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]));
DECLARE @item_physical INT = (SELECT COUNT(*) FROM [takt_logistics_sales_order_item] T WHERE EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]));
DECLARE @hdr_soft_total INT = (SELECT COUNT(*) FROM [takt_logistics_sales_order] T WHERE T.[is_deleted]=1 AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]));
DECLARE @item_soft_total INT = (SELECT COUNT(*) FROM [takt_logistics_sales_order_item] T WHERE T.[is_deleted]=1 AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]));

SELECT N'QUARTZ_SYNC_SUMMARY' AS [summary_tag], CAST(N'main' AS NVARCHAR(40)) AS [scope],
  @hdr_sap_raw AS [source_raw_count], @hdr_source AS [source_count],
  @hdr_before AS [target_before], @hdr_after AS [target_after],
  @hdr_physical AS [target_physical], @hdr_soft_total AS [soft_deleted],
  @hdr_ins AS [insert_count], @hdr_upd AS [update_count], @hdr_unchanged AS [unchanged_count],
  @hdr_delete AS [delete_count], @hdr_soft_keys AS [soft_deleted_keys]
UNION ALL
SELECT N'QUARTZ_SYNC_SUMMARY', CAST(N'detail' AS NVARCHAR(40)),
  @item_sap_raw, @item_source,
  @item_before, @item_after,
  @item_physical, @item_soft_total,
  @item_ins, @item_upd, @item_unchanged,
  @item_delete, @item_soft_keys;
