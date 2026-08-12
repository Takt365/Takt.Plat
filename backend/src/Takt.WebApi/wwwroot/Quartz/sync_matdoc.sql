SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体一致（明细无 material_document_year）
-- {{SourceDatabase}}.dbo.takt_logistics_materials_material_document[_item] → 当前租户库同名表
-- 主表唯一键：Tenant+Company+material_document_year+material_document_code
-- 明细唯一键：material_document_id+LineNumber
-- #item 临时列 year：从源主表 JOIN（SH.id = R.material_document_id）取得，不落库、不读明细

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#hdr_soft') IS NOT NULL DROP TABLE #hdr_soft;
IF OBJECT_ID('tempdb..#item_soft') IS NOT NULL DROP TABLE #item_soft;

CREATE TABLE #hdr (
  [rn] INT,
  [id] BIGINT,
  [material_document_code] NVARCHAR(10),
  [material_document_year] NVARCHAR(4),
  [transaction_event_type] NVARCHAR(2),
  [document_type] NVARCHAR(2),
  [revaluation_type] NVARCHAR(2),
  [document_date] DATETIME,
  [posting_date] DATETIME,
  [reference_code] NVARCHAR(16),
  [header_text] NVARCHAR(25),
  [transaction_code] NVARCHAR(4),
  [delivery_code] NVARCHAR(10),
  [posted_by] NVARCHAR(12),
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [is_deleted] INT,
  [updated_by] BIGINT
);

CREATE TABLE #item (
  [rn] INT,
  [id] BIGINT,
  [material_document_id] BIGINT,
  [plant_code] NVARCHAR(4),
  [material_document_code] NVARCHAR(10),
  [material_document_year] NVARCHAR(4),
  [line_number] INT,
  [line_id] NVARCHAR(6),
  [parent_line_id] NVARCHAR(6),
  [line_depth] NVARCHAR(2),
  [account_assignment_original_line] INT,
  [movement_type] NVARCHAR(3),
  [auto_created_flag] NVARCHAR(1),
  [material_code] NVARCHAR(20),
  [warehouse_code] NVARCHAR(4),
  [batch_code] NVARCHAR(10),
  [stock_type] NVARCHAR(1),
  [batch_status_key] NVARCHAR(1),
  [restricted_stock_flag] NVARCHAR(1),
  [special_stock] NVARCHAR(1),
  [supplier_code] NVARCHAR(10),
  [customer_code] NVARCHAR(10),
  [sales_order_code] NVARCHAR(20),
  [sales_order_item] INT,
  [sales_order_schedule] INT,
  [distribution_code] NVARCHAR(10),
  [debit_credit_indicator] NVARCHAR(1),
  [currency_code] NVARCHAR(3),
  [local_currency_amount] DECIMAL(13,2),
  [delivery_cost_amount] DECIMAL(13,2),
  [alternative_amount] DECIMAL(13,2),
  [revaluation_debit_credit] NVARCHAR(1),
  [revaluation_amount] DECIMAL(13,2),
  [valuation_type] NVARCHAR(10),
  [quantity] DECIMAL(13,3),
  [base_unit] NVARCHAR(3),
  [entry_quantity] DECIMAL(13,3),
  [entry_unit] NVARCHAR(3),
  [po_price_quantity] DECIMAL(13,3),
  [po_price_unit] NVARCHAR(3),
  [purchase_order_code] NVARCHAR(20),
  [purchase_order_item] INT,
  [reference_document_year] NVARCHAR(4),
  [reference_document_code] NVARCHAR(10),
  [reference_document_item] INT,
  [original_material_document_year] NVARCHAR(4),
  [original_material_document_code] NVARCHAR(10),
  [original_line_number] INT,
  [delivery_completed_flag] NVARCHAR(1),
  [item_text] NVARCHAR(50),
  [equipment_code] NVARCHAR(18),
  [goods_recipient] NVARCHAR(12),
  [unloading_point] NVARCHAR(25),
  [business_area_code] NVARCHAR(4),
  [controlling_area_code] NVARCHAR(4),
  [trading_partner_business_area] NVARCHAR(4),
  [clearing_company_code] NVARCHAR(4),
  [cost_center_code] NVARCHAR(10),
  [legacy_project_code] NVARCHAR(16),
  [production_order_code] NVARCHAR(12),
  [asset_code] NVARCHAR(12),
  [asset_sub_code] NVARCHAR(4),
  [cost_center_stat_flag] NVARCHAR(1),
  [order_stat_flag] NVARCHAR(1),
  [project_stat_flag] NVARCHAR(1),
  [profitability_stat_flag] NVARCHAR(1),
  [fiscal_year] NVARCHAR(4),
  [post_to_previous_period_flag] NVARCHAR(1),
  [post_to_previous_year_flag] NVARCHAR(1),
  [accounting_document_code] NVARCHAR(10),
  [accounting_document_item] INT,
  [revaluation_document_code] NVARCHAR(10),
  [revaluation_document_item] NVARCHAR(3),
  [reservation_code] NVARCHAR(10),
  [reservation_item] INT,
  [final_issue_flag] NVARCHAR(1),
  [reservation_quantity] DECIMAL(13,3),
  [statistics_relevant_flag] NVARCHAR(1),
  [receiving_material_code] NVARCHAR(20),
  [receiving_plant_code] NVARCHAR(4),
  [receiving_warehouse_code] NVARCHAR(4),
  [goods_receipt_slip_count] INT,
  [profit_center_code] NVARCHAR(10),
  [network_code] NVARCHAR(12),
  [routing_number] NVARCHAR(10),
  [routing_counter] NVARCHAR(8),
  [order_item_number] INT,
  [gl_account_code] NVARCHAR(10),
  [order_unit_quantity] DECIMAL(13,3),
  [order_unit] NVARCHAR(3),
  [supplying_vendor_code] NVARCHAR(10),
  [partner_profit_center_code] NVARCHAR(10),
  [stock_managed_material_code] NVARCHAR(20),
  [receiving_stock_material_code] NVARCHAR(20),
  [quantity_string] NVARCHAR(4),
  [value_string] NVARCHAR(4),
  [quantity_update_flag] NVARCHAR(1),
  [value_update_flag] NVARCHAR(1),
  [valuated_stock_quantity] DECIMAL(13,3),
  [total_valuated_stock_value] DECIMAL(13,2),
  [price_control] NVARCHAR(1),
  [original_item_line] INT,
  [manufacturer_part_material_code] NVARCHAR(40),
  [stock_type_modification] NVARCHAR(1),
  [transaction_event_type] NVARCHAR(2),
  [mkpf_reference_code] NVARCHAR(32),
  [mkpf_transaction_code2] NVARCHAR(40),
  [im_delivery_code] NVARCHAR(20),
  [im_delivery_item] INT,
  [is_obsolete] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [is_deleted] INT,
  [updated_by] BIGINT
);

CREATE TABLE #hdr_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [material_document_year] NVARCHAR(4),
  [material_document_code] NVARCHAR(10)
);
CREATE TABLE #item_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [material_document_code] NVARCHAR(40), [line_number] INT
);
CREATE TABLE #hdr_soft (
  [id] BIGINT,
  [material_document_year] NVARCHAR(4),
  [material_document_code] NVARCHAR(10)
);
CREATE TABLE #item_soft (
  [id] BIGINT,
  [material_document_code] NVARCHAR(40),
  [line_number] INT
);

INSERT INTO #hdr
SELECT
  S.rn,
  @base_id + S.rn,
  S.[material_document_code],
  S.[material_document_year],
  S.[transaction_event_type],
  S.[document_type],
  S.[revaluation_type],
  S.[document_date],
  S.[posting_date],
  S.[reference_code],
  S.[header_text],
  S.[transaction_code],
  S.[delivery_code],
  S.[posted_by],
  @tenant_code,
  @company_code,
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[material_document_year], N.[material_document_code]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_document_code])), 10), N''), N'') AS [material_document_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_document_year])), 4), N''), N'') AS [material_document_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_event_type])), 2), N'') AS [transaction_event_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_type])), 2), N'') AS [document_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[revaluation_type])), 2), N'') AS [revaluation_type],
      ISNULL(TRY_CAST(R.[document_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [document_date],
      ISNULL(TRY_CAST(R.[posting_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [posting_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_code])), 16), N'') AS [reference_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[header_text])), 25), N'') AS [header_text],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_code])), 4), N'') AS [transaction_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[delivery_code])), 10), N'') AS [delivery_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[posted_by])), 12), N'') AS [posted_by],
      0 AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY LTRIM(RTRIM(R.[material_document_year])), LTRIM(RTRIM(R.[material_document_code]))
        ORDER BY LTRIM(RTRIM(R.[material_document_year])), LTRIM(RTRIM(R.[material_document_code]))
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_document_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_document_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @hdr_source INT = (SELECT COUNT(*) FROM #hdr);
DECLARE @hdr_sap_keys INT = (
  SELECT COUNT(*)
  FROM (
    SELECT LTRIM(RTRIM(R.[material_document_year])) AS [material_document_year],
           LTRIM(RTRIM(R.[material_document_code])) AS [material_document_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_document_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_document_code], N''))) <> N''
    GROUP BY LTRIM(RTRIM(R.[material_document_year])), LTRIM(RTRIM(R.[material_document_code]))
  ) K
);
IF @hdr_source <> @hdr_sap_keys
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(
    N'主表业务键装入不一致: keys=', @hdr_sap_keys, N', loaded=', @hdr_source);
  THROW 50003, @hdr_src_msg, 1;
END;

UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #hdr S
LEFT JOIN [takt_logistics_materials_material_document] T
  ON T.[tenant_code] = @tenant_code
 AND T.[company_code] = @company_code
 AND LTRIM(RTRIM(T.[material_document_year])) = S.[material_document_year]
 AND LTRIM(RTRIM(T.[material_document_code])) = S.[material_document_code];

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_materials_material_document] AS T
USING #hdr AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[material_document_year])) = S.[material_document_year]
AND LTRIM(RTRIM(T.[material_document_code])) = S.[material_document_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR EXISTS (
    SELECT
      S.[transaction_event_type],
      S.[document_type],
      S.[revaluation_type],
      S.[document_date],
      S.[posting_date],
      S.[reference_code],
      S.[header_text],
      S.[transaction_code],
      S.[delivery_code],
      S.[posted_by]
    EXCEPT
    SELECT
      T.[transaction_event_type],
      T.[document_type],
      T.[revaluation_type],
      T.[document_date],
      T.[posting_date],
      T.[reference_code],
      T.[header_text],
      T.[transaction_code],
      T.[delivery_code],
      T.[posted_by]
  )
) THEN
  UPDATE SET
    T.[transaction_event_type] = S.[transaction_event_type],
    T.[document_type] = S.[document_type],
    T.[revaluation_type] = S.[revaluation_type],
    T.[document_date] = S.[document_date],
    T.[posting_date] = S.[posting_date],
    T.[reference_code] = S.[reference_code],
    T.[header_text] = S.[header_text],
    T.[transaction_code] = S.[transaction_code],
    T.[delivery_code] = S.[delivery_code],
    T.[posted_by] = S.[posted_by],
        T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT ([id],[material_document_code],[material_document_year],[transaction_event_type],[document_type],[revaluation_type],[document_date],[posting_date],[reference_code],[header_text],[transaction_code],[delivery_code],[posted_by],[tenant_code],[company_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[material_document_code],S.[material_document_year],S.[transaction_event_type],S.[document_type],S.[revaluation_type],S.[document_date],S.[posting_date],S.[reference_code],S.[header_text],S.[transaction_code],S.[delivery_code],S.[posted_by],S.[tenant_code],S.[company_code],N'{}',N'',S.[updated_by],@now,S.[updated_by],@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END)
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[material_document_year], INSERTED.[material_document_code]
INTO #hdr_delta (rn, oper_type, id, [material_document_year], [material_document_code]);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[material_document_year], INSERTED.[material_document_code]
INTO #hdr_soft ([id], [material_document_year], [material_document_code])
FROM [takt_logistics_materials_material_document] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1 FROM #hdr S
    WHERE S.[material_document_year] = LTRIM(RTRIM(T.[material_document_year]))
      AND S.[material_document_code] = LTRIM(RTRIM(T.[material_document_code]))
  );

DECLARE @hdr_delete INT = @@ROWCOUNT;

INSERT INTO #item
SELECT
  S.rn,
  @base_id + 1000000000 + S.rn,
  0,
  S.[plant_code],
  S.[material_document_code],
  S.[material_document_year],
  S.[line_number],
  S.[line_id],
  S.[parent_line_id],
  S.[line_depth],
  S.[account_assignment_original_line],
  S.[movement_type],
  S.[auto_created_flag],
  S.[material_code],
  S.[warehouse_code],
  S.[batch_code],
  S.[stock_type],
  S.[batch_status_key],
  S.[restricted_stock_flag],
  S.[special_stock],
  S.[supplier_code],
  S.[customer_code],
  S.[sales_order_code],
  S.[sales_order_item],
  S.[sales_order_schedule],
  S.[distribution_code],
  S.[debit_credit_indicator],
  S.[currency_code],
  S.[local_currency_amount],
  S.[delivery_cost_amount],
  S.[alternative_amount],
  S.[revaluation_debit_credit],
  S.[revaluation_amount],
  S.[valuation_type],
  S.[quantity],
  S.[base_unit],
  S.[entry_quantity],
  S.[entry_unit],
  S.[po_price_quantity],
  S.[po_price_unit],
  S.[purchase_order_code],
  S.[purchase_order_item],
  S.[reference_document_year],
  S.[reference_document_code],
  S.[reference_document_item],
  S.[original_material_document_year],
  S.[original_material_document_code],
  S.[original_line_number],
  S.[delivery_completed_flag],
  S.[item_text],
  S.[equipment_code],
  S.[goods_recipient],
  S.[unloading_point],
  S.[business_area_code],
  S.[controlling_area_code],
  S.[trading_partner_business_area],
  S.[clearing_company_code],
  S.[cost_center_code],
  S.[legacy_project_code],
  S.[production_order_code],
  S.[asset_code],
  S.[asset_sub_code],
  S.[cost_center_stat_flag],
  S.[order_stat_flag],
  S.[project_stat_flag],
  S.[profitability_stat_flag],
  S.[fiscal_year],
  S.[post_to_previous_period_flag],
  S.[post_to_previous_year_flag],
  S.[accounting_document_code],
  S.[accounting_document_item],
  S.[revaluation_document_code],
  S.[revaluation_document_item],
  S.[reservation_code],
  S.[reservation_item],
  S.[final_issue_flag],
  S.[reservation_quantity],
  S.[statistics_relevant_flag],
  S.[receiving_material_code],
  S.[receiving_plant_code],
  S.[receiving_warehouse_code],
  S.[goods_receipt_slip_count],
  S.[profit_center_code],
  S.[network_code],
  S.[routing_number],
  S.[routing_counter],
  S.[order_item_number],
  S.[gl_account_code],
  S.[order_unit_quantity],
  S.[order_unit],
  S.[supplying_vendor_code],
  S.[partner_profit_center_code],
  S.[stock_managed_material_code],
  S.[receiving_stock_material_code],
  S.[quantity_string],
  S.[value_string],
  S.[quantity_update_flag],
  S.[value_update_flag],
  S.[valuated_stock_quantity],
  S.[total_valuated_stock_value],
  S.[price_control],
  S.[original_item_line],
  S.[manufacturer_part_material_code],
  S.[stock_type_modification],
  S.[transaction_event_type],
  S.[mkpf_reference_code],
  S.[mkpf_transaction_code2],
  S.[im_delivery_code],
  S.[im_delivery_item],
  S.[is_obsolete],
  @tenant_code,
  @company_code,
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[material_document_year], N.[material_document_code], N.[line_number]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[plant_code])), 4), N''), N'') AS [plant_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_document_code])), 10), N''), N'') AS [material_document_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[material_document_year])), 4), N''), N'') AS [material_document_year],
      ISNULL(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[line_id])), 6), N'') AS [line_id],
      NULLIF(LEFT(LTRIM(RTRIM(R.[parent_line_id])), 6), N'') AS [parent_line_id],
      NULLIF(LEFT(LTRIM(RTRIM(R.[line_depth])), 2), N'') AS [line_depth],
      TRY_CAST(R.[account_assignment_original_line] AS INT) AS [account_assignment_original_line],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[movement_type])), 3), N''), N'101') AS [movement_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[auto_created_flag])), 1), N'') AS [auto_created_flag],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N''), N'') AS [material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[warehouse_code])), 4), N'') AS [warehouse_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[batch_code])), 10), N'') AS [batch_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[stock_type])), 1), N'') AS [stock_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[batch_status_key])), 1), N'') AS [batch_status_key],
      NULLIF(LEFT(LTRIM(RTRIM(R.[restricted_stock_flag])), 1), N'') AS [restricted_stock_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[special_stock])), 1), N'') AS [special_stock],
      NULLIF(LEFT(LTRIM(RTRIM(R.[supplier_code])), 10), N'') AS [supplier_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[customer_code])), 10), N'') AS [customer_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_order_code])), 20), N'') AS [sales_order_code],
      TRY_CAST(R.[sales_order_item] AS INT) AS [sales_order_item],
      TRY_CAST(R.[sales_order_schedule] AS INT) AS [sales_order_schedule],
      NULLIF(LEFT(LTRIM(RTRIM(R.[distribution_code])), 10), N'') AS [distribution_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[debit_credit_indicator])), 1), N'') AS [debit_credit_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N'') AS [currency_code],
      ISNULL(ROUND(TRY_CAST(R.[local_currency_amount] AS DECIMAL(13,2)), 2), 0) AS [local_currency_amount],
      ROUND(TRY_CAST(R.[delivery_cost_amount] AS DECIMAL(13,2)), 2) AS [delivery_cost_amount],
      ROUND(TRY_CAST(R.[alternative_amount] AS DECIMAL(13,2)), 2) AS [alternative_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[revaluation_debit_credit])), 1), N'') AS [revaluation_debit_credit],
      ROUND(TRY_CAST(R.[revaluation_amount] AS DECIMAL(13,2)), 2) AS [revaluation_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[valuation_type])), 10), N'') AS [valuation_type],
      ISNULL(ROUND(TRY_CAST(R.[quantity] AS DECIMAL(13,3)), 3), 0) AS [quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[base_unit])), 3), N'') AS [base_unit],
      ROUND(TRY_CAST(R.[entry_quantity] AS DECIMAL(13,3)), 3) AS [entry_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[entry_unit])), 3), N'') AS [entry_unit],
      ROUND(TRY_CAST(R.[po_price_quantity] AS DECIMAL(13,3)), 3) AS [po_price_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[po_price_unit])), 3), N'') AS [po_price_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_order_code])), 20), N'') AS [purchase_order_code],
      TRY_CAST(R.[purchase_order_item] AS INT) AS [purchase_order_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_year])), 4), N'') AS [reference_document_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_code])), 10), N'') AS [reference_document_code],
      TRY_CAST(R.[reference_document_item] AS INT) AS [reference_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[original_material_document_year])), 4), N'') AS [original_material_document_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[original_material_document_code])), 10), N'') AS [original_material_document_code],
      TRY_CAST(R.[original_line_number] AS INT) AS [original_line_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[delivery_completed_flag])), 1), N'') AS [delivery_completed_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[item_text])), 50), N'') AS [item_text],
      NULLIF(LEFT(LTRIM(RTRIM(R.[equipment_code])), 18), N'') AS [equipment_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[goods_recipient])), 12), N'') AS [goods_recipient],
      NULLIF(LEFT(LTRIM(RTRIM(R.[unloading_point])), 25), N'') AS [unloading_point],
      NULLIF(LEFT(LTRIM(RTRIM(R.[business_area_code])), 4), N'') AS [business_area_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[controlling_area_code])), 4), N'') AS [controlling_area_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[trading_partner_business_area])), 4), N'') AS [trading_partner_business_area],
      NULLIF(LEFT(LTRIM(RTRIM(R.[clearing_company_code])), 4), N'') AS [clearing_company_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cost_center_code])), 10), N'') AS [cost_center_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[legacy_project_code])), 16), N'') AS [legacy_project_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[production_order_code])), 12), N'') AS [production_order_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[asset_code])), 12), N'') AS [asset_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[asset_sub_code])), 4), N'') AS [asset_sub_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cost_center_stat_flag])), 1), N'') AS [cost_center_stat_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[order_stat_flag])), 1), N'') AS [order_stat_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[project_stat_flag])), 1), N'') AS [project_stat_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[profitability_stat_flag])), 1), N'') AS [profitability_stat_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiscal_year])), 4), N'') AS [fiscal_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[post_to_previous_period_flag])), 1), N'') AS [post_to_previous_period_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[post_to_previous_year_flag])), 1), N'') AS [post_to_previous_year_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[accounting_document_code])), 10), N'') AS [accounting_document_code],
      TRY_CAST(R.[accounting_document_item] AS INT) AS [accounting_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[revaluation_document_code])), 10), N'') AS [revaluation_document_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[revaluation_document_item])), 3), N'') AS [revaluation_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reservation_code])), 10), N'') AS [reservation_code],
      TRY_CAST(R.[reservation_item] AS INT) AS [reservation_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[final_issue_flag])), 1), N'') AS [final_issue_flag],
      ROUND(TRY_CAST(R.[reservation_quantity] AS DECIMAL(13,3)), 3) AS [reservation_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[statistics_relevant_flag])), 1), N'') AS [statistics_relevant_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[receiving_material_code])), 20), N'') AS [receiving_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[receiving_plant_code])), 4), N'') AS [receiving_plant_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[receiving_warehouse_code])), 4), N'') AS [receiving_warehouse_code],
      TRY_CAST(R.[goods_receipt_slip_count] AS INT) AS [goods_receipt_slip_count],
      NULLIF(LEFT(LTRIM(RTRIM(R.[profit_center_code])), 10), N'') AS [profit_center_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[network_code])), 12), N'') AS [network_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[routing_number])), 10), N'') AS [routing_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[routing_counter])), 8), N'') AS [routing_counter],
      TRY_CAST(R.[order_item_number] AS INT) AS [order_item_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[gl_account_code])), 10), N'') AS [gl_account_code],
      ROUND(TRY_CAST(R.[order_unit_quantity] AS DECIMAL(13,3)), 3) AS [order_unit_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[order_unit])), 3), N'') AS [order_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[supplying_vendor_code])), 10), N'') AS [supplying_vendor_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[partner_profit_center_code])), 10), N'') AS [partner_profit_center_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[stock_managed_material_code])), 20), N'') AS [stock_managed_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[receiving_stock_material_code])), 20), N'') AS [receiving_stock_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[quantity_string])), 4), N'') AS [quantity_string],
      NULLIF(LEFT(LTRIM(RTRIM(R.[value_string])), 4), N'') AS [value_string],
      NULLIF(LEFT(LTRIM(RTRIM(R.[quantity_update_flag])), 1), N'') AS [quantity_update_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[value_update_flag])), 1), N'') AS [value_update_flag],
      ROUND(TRY_CAST(R.[valuated_stock_quantity] AS DECIMAL(13,3)), 3) AS [valuated_stock_quantity],
      ROUND(TRY_CAST(R.[total_valuated_stock_value] AS DECIMAL(13,2)), 2) AS [total_valuated_stock_value],
      NULLIF(LEFT(LTRIM(RTRIM(R.[price_control])), 1), N'') AS [price_control],
      TRY_CAST(R.[original_item_line] AS INT) AS [original_item_line],
      NULLIF(LEFT(LTRIM(RTRIM(R.[manufacturer_part_material_code])), 40), N'') AS [manufacturer_part_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[stock_type_modification])), 1), N'') AS [stock_type_modification],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_event_type])), 2), N'') AS [transaction_event_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[mkpf_reference_code])), 32), N'') AS [mkpf_reference_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[mkpf_transaction_code2])), 40), N'') AS [mkpf_transaction_code2],
      NULLIF(LEFT(LTRIM(RTRIM(R.[im_delivery_code])), 20), N'') AS [im_delivery_code],
      TRY_CAST(R.[im_delivery_item] AS INT) AS [im_delivery_item],
      ISNULL(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
      0 AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(SH.[material_document_year])),
          LTRIM(RTRIM(R.[material_document_code])),
          COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
        ORDER BY COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] SH
      ON SH.[id] = R.[material_document_id]
    WHERE LTRIM(RTRIM(ISNULL(SH.[material_document_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_document_code], N''))) <> N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0) > 0
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

UPDATE I
SET I.[material_document_id] = H.[id]
FROM #item I
INNER JOIN #hdr H
  ON H.[material_document_year] = I.[material_document_year]
 AND H.[material_document_code] = I.[material_document_code];

DELETE FROM #item WHERE [material_document_id] = 0 OR [material_document_id] IS NULL;

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item);
DECLARE @item_sap_keys INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(SH.[material_document_year])) AS [material_document_year],
      LTRIM(RTRIM(R.[material_document_code])) AS [material_document_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] SH
      ON SH.[id] = R.[material_document_id]
    WHERE LTRIM(RTRIM(ISNULL(SH.[material_document_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_document_code], N''))) <> N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0) > 0
    GROUP BY
      LTRIM(RTRIM(SH.[material_document_year])),
      LTRIM(RTRIM(R.[material_document_code])),
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
  ) K
);
IF @item_source > @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(
    N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;

UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #item S
LEFT JOIN [takt_logistics_materials_material_document_item] T
  ON T.[tenant_code] = @tenant_code
 AND T.[company_code] = @company_code
 AND T.[material_document_id] = S.[material_document_id]
 AND T.[line_number] = S.[line_number];

DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document_item]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_materials_material_document_item] AS T
USING #item AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND T.[material_document_id] = S.[material_document_id]
AND T.[line_number] = S.[line_number]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR EXISTS (
    SELECT
      S.[plant_code],
      S.[line_id],
      S.[parent_line_id],
      S.[line_depth],
      S.[account_assignment_original_line],
      S.[movement_type],
      S.[auto_created_flag],
      S.[material_code],
      S.[warehouse_code],
      S.[batch_code],
      S.[stock_type],
      S.[batch_status_key],
      S.[restricted_stock_flag],
      S.[special_stock],
      S.[supplier_code],
      S.[customer_code],
      S.[sales_order_code],
      S.[sales_order_item],
      S.[sales_order_schedule],
      S.[distribution_code],
      S.[debit_credit_indicator],
      S.[currency_code],
      S.[local_currency_amount],
      S.[delivery_cost_amount],
      S.[alternative_amount],
      S.[revaluation_debit_credit],
      S.[revaluation_amount],
      S.[valuation_type],
      S.[quantity],
      S.[base_unit],
      S.[entry_quantity],
      S.[entry_unit],
      S.[po_price_quantity],
      S.[po_price_unit],
      S.[purchase_order_code],
      S.[purchase_order_item],
      S.[reference_document_year],
      S.[reference_document_code],
      S.[reference_document_item],
      S.[original_material_document_year],
      S.[original_material_document_code],
      S.[original_line_number],
      S.[delivery_completed_flag],
      S.[item_text],
      S.[equipment_code],
      S.[goods_recipient],
      S.[unloading_point],
      S.[business_area_code],
      S.[controlling_area_code],
      S.[trading_partner_business_area],
      S.[clearing_company_code],
      S.[cost_center_code],
      S.[legacy_project_code],
      S.[production_order_code],
      S.[asset_code],
      S.[asset_sub_code],
      S.[cost_center_stat_flag],
      S.[order_stat_flag],
      S.[project_stat_flag],
      S.[profitability_stat_flag],
      S.[fiscal_year],
      S.[post_to_previous_period_flag],
      S.[post_to_previous_year_flag],
      S.[accounting_document_code],
      S.[accounting_document_item],
      S.[revaluation_document_code],
      S.[revaluation_document_item],
      S.[reservation_code],
      S.[reservation_item],
      S.[final_issue_flag],
      S.[reservation_quantity],
      S.[statistics_relevant_flag],
      S.[receiving_material_code],
      S.[receiving_plant_code],
      S.[receiving_warehouse_code],
      S.[goods_receipt_slip_count],
      S.[profit_center_code],
      S.[network_code],
      S.[routing_number],
      S.[routing_counter],
      S.[order_item_number],
      S.[gl_account_code],
      S.[order_unit_quantity],
      S.[order_unit],
      S.[supplying_vendor_code],
      S.[partner_profit_center_code],
      S.[stock_managed_material_code],
      S.[receiving_stock_material_code],
      S.[quantity_string],
      S.[value_string],
      S.[quantity_update_flag],
      S.[value_update_flag],
      S.[valuated_stock_quantity],
      S.[total_valuated_stock_value],
      S.[price_control],
      S.[original_item_line],
      S.[manufacturer_part_material_code],
      S.[stock_type_modification],
      S.[transaction_event_type],
      S.[mkpf_reference_code],
      S.[mkpf_transaction_code2],
      S.[im_delivery_code],
      S.[im_delivery_item],
      S.[is_obsolete]
    EXCEPT
    SELECT
      T.[plant_code],
      T.[line_id],
      T.[parent_line_id],
      T.[line_depth],
      T.[account_assignment_original_line],
      T.[movement_type],
      T.[auto_created_flag],
      T.[material_code],
      T.[warehouse_code],
      T.[batch_code],
      T.[stock_type],
      T.[batch_status_key],
      T.[restricted_stock_flag],
      T.[special_stock],
      T.[supplier_code],
      T.[customer_code],
      T.[sales_order_code],
      T.[sales_order_item],
      T.[sales_order_schedule],
      T.[distribution_code],
      T.[debit_credit_indicator],
      T.[currency_code],
      T.[local_currency_amount],
      T.[delivery_cost_amount],
      T.[alternative_amount],
      T.[revaluation_debit_credit],
      T.[revaluation_amount],
      T.[valuation_type],
      T.[quantity],
      T.[base_unit],
      T.[entry_quantity],
      T.[entry_unit],
      T.[po_price_quantity],
      T.[po_price_unit],
      T.[purchase_order_code],
      T.[purchase_order_item],
      T.[reference_document_year],
      T.[reference_document_code],
      T.[reference_document_item],
      T.[original_material_document_year],
      T.[original_material_document_code],
      T.[original_line_number],
      T.[delivery_completed_flag],
      T.[item_text],
      T.[equipment_code],
      T.[goods_recipient],
      T.[unloading_point],
      T.[business_area_code],
      T.[controlling_area_code],
      T.[trading_partner_business_area],
      T.[clearing_company_code],
      T.[cost_center_code],
      T.[legacy_project_code],
      T.[production_order_code],
      T.[asset_code],
      T.[asset_sub_code],
      T.[cost_center_stat_flag],
      T.[order_stat_flag],
      T.[project_stat_flag],
      T.[profitability_stat_flag],
      T.[fiscal_year],
      T.[post_to_previous_period_flag],
      T.[post_to_previous_year_flag],
      T.[accounting_document_code],
      T.[accounting_document_item],
      T.[revaluation_document_code],
      T.[revaluation_document_item],
      T.[reservation_code],
      T.[reservation_item],
      T.[final_issue_flag],
      T.[reservation_quantity],
      T.[statistics_relevant_flag],
      T.[receiving_material_code],
      T.[receiving_plant_code],
      T.[receiving_warehouse_code],
      T.[goods_receipt_slip_count],
      T.[profit_center_code],
      T.[network_code],
      T.[routing_number],
      T.[routing_counter],
      T.[order_item_number],
      T.[gl_account_code],
      T.[order_unit_quantity],
      T.[order_unit],
      T.[supplying_vendor_code],
      T.[partner_profit_center_code],
      T.[stock_managed_material_code],
      T.[receiving_stock_material_code],
      T.[quantity_string],
      T.[value_string],
      T.[quantity_update_flag],
      T.[value_update_flag],
      T.[valuated_stock_quantity],
      T.[total_valuated_stock_value],
      T.[price_control],
      T.[original_item_line],
      T.[manufacturer_part_material_code],
      T.[stock_type_modification],
      T.[transaction_event_type],
      T.[mkpf_reference_code],
      T.[mkpf_transaction_code2],
      T.[im_delivery_code],
      T.[im_delivery_item],
      T.[is_obsolete]
  )
) THEN
  UPDATE SET
    T.[material_document_code] = S.[material_document_code],
    T.[plant_code] = S.[plant_code],
    T.[line_id] = S.[line_id],
    T.[parent_line_id] = S.[parent_line_id],
    T.[line_depth] = S.[line_depth],
    T.[account_assignment_original_line] = S.[account_assignment_original_line],
    T.[movement_type] = S.[movement_type],
    T.[auto_created_flag] = S.[auto_created_flag],
    T.[material_code] = S.[material_code],
    T.[warehouse_code] = S.[warehouse_code],
    T.[batch_code] = S.[batch_code],
    T.[stock_type] = S.[stock_type],
    T.[batch_status_key] = S.[batch_status_key],
    T.[restricted_stock_flag] = S.[restricted_stock_flag],
    T.[special_stock] = S.[special_stock],
    T.[supplier_code] = S.[supplier_code],
    T.[customer_code] = S.[customer_code],
    T.[sales_order_code] = S.[sales_order_code],
    T.[sales_order_item] = S.[sales_order_item],
    T.[sales_order_schedule] = S.[sales_order_schedule],
    T.[distribution_code] = S.[distribution_code],
    T.[debit_credit_indicator] = S.[debit_credit_indicator],
    T.[currency_code] = S.[currency_code],
    T.[local_currency_amount] = S.[local_currency_amount],
    T.[delivery_cost_amount] = S.[delivery_cost_amount],
    T.[alternative_amount] = S.[alternative_amount],
    T.[revaluation_debit_credit] = S.[revaluation_debit_credit],
    T.[revaluation_amount] = S.[revaluation_amount],
    T.[valuation_type] = S.[valuation_type],
    T.[quantity] = S.[quantity],
    T.[base_unit] = S.[base_unit],
    T.[entry_quantity] = S.[entry_quantity],
    T.[entry_unit] = S.[entry_unit],
    T.[po_price_quantity] = S.[po_price_quantity],
    T.[po_price_unit] = S.[po_price_unit],
    T.[purchase_order_code] = S.[purchase_order_code],
    T.[purchase_order_item] = S.[purchase_order_item],
    T.[reference_document_year] = S.[reference_document_year],
    T.[reference_document_code] = S.[reference_document_code],
    T.[reference_document_item] = S.[reference_document_item],
    T.[original_material_document_year] = S.[original_material_document_year],
    T.[original_material_document_code] = S.[original_material_document_code],
    T.[original_line_number] = S.[original_line_number],
    T.[delivery_completed_flag] = S.[delivery_completed_flag],
    T.[item_text] = S.[item_text],
    T.[equipment_code] = S.[equipment_code],
    T.[goods_recipient] = S.[goods_recipient],
    T.[unloading_point] = S.[unloading_point],
    T.[business_area_code] = S.[business_area_code],
    T.[controlling_area_code] = S.[controlling_area_code],
    T.[trading_partner_business_area] = S.[trading_partner_business_area],
    T.[clearing_company_code] = S.[clearing_company_code],
    T.[cost_center_code] = S.[cost_center_code],
    T.[legacy_project_code] = S.[legacy_project_code],
    T.[production_order_code] = S.[production_order_code],
    T.[asset_code] = S.[asset_code],
    T.[asset_sub_code] = S.[asset_sub_code],
    T.[cost_center_stat_flag] = S.[cost_center_stat_flag],
    T.[order_stat_flag] = S.[order_stat_flag],
    T.[project_stat_flag] = S.[project_stat_flag],
    T.[profitability_stat_flag] = S.[profitability_stat_flag],
    T.[fiscal_year] = S.[fiscal_year],
    T.[post_to_previous_period_flag] = S.[post_to_previous_period_flag],
    T.[post_to_previous_year_flag] = S.[post_to_previous_year_flag],
    T.[accounting_document_code] = S.[accounting_document_code],
    T.[accounting_document_item] = S.[accounting_document_item],
    T.[revaluation_document_code] = S.[revaluation_document_code],
    T.[revaluation_document_item] = S.[revaluation_document_item],
    T.[reservation_code] = S.[reservation_code],
    T.[reservation_item] = S.[reservation_item],
    T.[final_issue_flag] = S.[final_issue_flag],
    T.[reservation_quantity] = S.[reservation_quantity],
    T.[statistics_relevant_flag] = S.[statistics_relevant_flag],
    T.[receiving_material_code] = S.[receiving_material_code],
    T.[receiving_plant_code] = S.[receiving_plant_code],
    T.[receiving_warehouse_code] = S.[receiving_warehouse_code],
    T.[goods_receipt_slip_count] = S.[goods_receipt_slip_count],
    T.[profit_center_code] = S.[profit_center_code],
    T.[network_code] = S.[network_code],
    T.[routing_number] = S.[routing_number],
    T.[routing_counter] = S.[routing_counter],
    T.[order_item_number] = S.[order_item_number],
    T.[gl_account_code] = S.[gl_account_code],
    T.[order_unit_quantity] = S.[order_unit_quantity],
    T.[order_unit] = S.[order_unit],
    T.[supplying_vendor_code] = S.[supplying_vendor_code],
    T.[partner_profit_center_code] = S.[partner_profit_center_code],
    T.[stock_managed_material_code] = S.[stock_managed_material_code],
    T.[receiving_stock_material_code] = S.[receiving_stock_material_code],
    T.[quantity_string] = S.[quantity_string],
    T.[value_string] = S.[value_string],
    T.[quantity_update_flag] = S.[quantity_update_flag],
    T.[value_update_flag] = S.[value_update_flag],
    T.[valuated_stock_quantity] = S.[valuated_stock_quantity],
    T.[total_valuated_stock_value] = S.[total_valuated_stock_value],
    T.[price_control] = S.[price_control],
    T.[original_item_line] = S.[original_item_line],
    T.[manufacturer_part_material_code] = S.[manufacturer_part_material_code],
    T.[stock_type_modification] = S.[stock_type_modification],
    T.[transaction_event_type] = S.[transaction_event_type],
    T.[mkpf_reference_code] = S.[mkpf_reference_code],
    T.[mkpf_transaction_code2] = S.[mkpf_transaction_code2],
    T.[im_delivery_code] = S.[im_delivery_code],
    T.[im_delivery_item] = S.[im_delivery_item],
    T.[is_obsolete] = S.[is_obsolete],
        T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT ([id],[material_document_id],[plant_code],[material_document_code],[line_number],[line_id],[parent_line_id],[line_depth],[account_assignment_original_line],[movement_type],[auto_created_flag],[material_code],[warehouse_code],[batch_code],[stock_type],[batch_status_key],[restricted_stock_flag],[special_stock],[supplier_code],[customer_code],[sales_order_code],[sales_order_item],[sales_order_schedule],[distribution_code],[debit_credit_indicator],[currency_code],[local_currency_amount],[delivery_cost_amount],[alternative_amount],[revaluation_debit_credit],[revaluation_amount],[valuation_type],[quantity],[base_unit],[entry_quantity],[entry_unit],[po_price_quantity],[po_price_unit],[purchase_order_code],[purchase_order_item],[reference_document_year],[reference_document_code],[reference_document_item],[original_material_document_year],[original_material_document_code],[original_line_number],[delivery_completed_flag],[item_text],[equipment_code],[goods_recipient],[unloading_point],[business_area_code],[controlling_area_code],[trading_partner_business_area],[clearing_company_code],[cost_center_code],[legacy_project_code],[production_order_code],[asset_code],[asset_sub_code],[cost_center_stat_flag],[order_stat_flag],[project_stat_flag],[profitability_stat_flag],[fiscal_year],[post_to_previous_period_flag],[post_to_previous_year_flag],[accounting_document_code],[accounting_document_item],[revaluation_document_code],[revaluation_document_item],[reservation_code],[reservation_item],[final_issue_flag],[reservation_quantity],[statistics_relevant_flag],[receiving_material_code],[receiving_plant_code],[receiving_warehouse_code],[goods_receipt_slip_count],[profit_center_code],[network_code],[routing_number],[routing_counter],[order_item_number],[gl_account_code],[order_unit_quantity],[order_unit],[supplying_vendor_code],[partner_profit_center_code],[stock_managed_material_code],[receiving_stock_material_code],[quantity_string],[value_string],[quantity_update_flag],[value_update_flag],[valuated_stock_quantity],[total_valuated_stock_value],[price_control],[original_item_line],[manufacturer_part_material_code],[stock_type_modification],[transaction_event_type],[mkpf_reference_code],[mkpf_transaction_code2],[im_delivery_code],[im_delivery_item],[is_obsolete],[tenant_code],[company_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[material_document_id],S.[plant_code],S.[material_document_code],S.[line_number],S.[line_id],S.[parent_line_id],S.[line_depth],S.[account_assignment_original_line],S.[movement_type],S.[auto_created_flag],S.[material_code],S.[warehouse_code],S.[batch_code],S.[stock_type],S.[batch_status_key],S.[restricted_stock_flag],S.[special_stock],S.[supplier_code],S.[customer_code],S.[sales_order_code],S.[sales_order_item],S.[sales_order_schedule],S.[distribution_code],S.[debit_credit_indicator],S.[currency_code],S.[local_currency_amount],S.[delivery_cost_amount],S.[alternative_amount],S.[revaluation_debit_credit],S.[revaluation_amount],S.[valuation_type],S.[quantity],S.[base_unit],S.[entry_quantity],S.[entry_unit],S.[po_price_quantity],S.[po_price_unit],S.[purchase_order_code],S.[purchase_order_item],S.[reference_document_year],S.[reference_document_code],S.[reference_document_item],S.[original_material_document_year],S.[original_material_document_code],S.[original_line_number],S.[delivery_completed_flag],S.[item_text],S.[equipment_code],S.[goods_recipient],S.[unloading_point],S.[business_area_code],S.[controlling_area_code],S.[trading_partner_business_area],S.[clearing_company_code],S.[cost_center_code],S.[legacy_project_code],S.[production_order_code],S.[asset_code],S.[asset_sub_code],S.[cost_center_stat_flag],S.[order_stat_flag],S.[project_stat_flag],S.[profitability_stat_flag],S.[fiscal_year],S.[post_to_previous_period_flag],S.[post_to_previous_year_flag],S.[accounting_document_code],S.[accounting_document_item],S.[revaluation_document_code],S.[revaluation_document_item],S.[reservation_code],S.[reservation_item],S.[final_issue_flag],S.[reservation_quantity],S.[statistics_relevant_flag],S.[receiving_material_code],S.[receiving_plant_code],S.[receiving_warehouse_code],S.[goods_receipt_slip_count],S.[profit_center_code],S.[network_code],S.[routing_number],S.[routing_counter],S.[order_item_number],S.[gl_account_code],S.[order_unit_quantity],S.[order_unit],S.[supplying_vendor_code],S.[partner_profit_center_code],S.[stock_managed_material_code],S.[receiving_stock_material_code],S.[quantity_string],S.[value_string],S.[quantity_update_flag],S.[value_update_flag],S.[valuated_stock_quantity],S.[total_valuated_stock_value],S.[price_control],S.[original_item_line],S.[manufacturer_part_material_code],S.[stock_type_modification],S.[transaction_event_type],S.[mkpf_reference_code],S.[mkpf_transaction_code2],S.[im_delivery_code],S.[im_delivery_item],S.[is_obsolete],S.[tenant_code],S.[company_code],N'{}',N'',S.[updated_by],@now,S.[updated_by],@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END)
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[material_document_code], INSERTED.[line_number]
INTO #item_delta (rn, oper_type, id, [material_document_code], [line_number]);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[material_document_code], INSERTED.[line_number]
INTO #item_soft ([id], [material_document_code], [line_number])
FROM [takt_logistics_materials_material_document_item] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1 FROM #item S
    WHERE S.[material_document_id] = T.[material_document_id]
      AND S.[line_number] = T.[line_number]
  );

DECLARE @item_delete INT = @@ROWCOUNT;

DECLARE @hdr_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);
DECLARE @item_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document_item]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);

IF @hdr_after <> @hdr_source
BEGIN
  DECLARE @hdr_cnt NVARCHAR(200) = CONCAT(
    N'主表有效行数不一致: source=', @hdr_source, N', active=', @hdr_after);
  THROW 50002, @hdr_cnt, 1;
END;
IF @item_after <> @item_source
BEGIN
  DECLARE @item_cnt NVARCHAR(200) = CONCAT(
    N'明细有效行数不一致: source=', @item_source, N', active=', @item_after);
  THROW 50002, @item_cnt, 1;
END;

DECLARE @hdr_ins INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type = N'INSERT');
DECLARE @hdr_upd INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type = N'UPDATE');
DECLARE @hdr_unchanged INT = @hdr_source - @hdr_ins - @hdr_upd;
DECLARE @item_ins INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type = N'INSERT');
DECLARE @item_upd INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type = N'UPDATE');
DECLARE @item_unchanged INT = @item_source - @item_ins - @item_upd;

DECLARE @hdr_soft_keys NVARCHAR(MAX) = N'';
SELECT @hdr_soft_keys = STRING_AGG(
  CAST(
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([material_document_year], N''), N'/', ISNULL([material_document_code], N''))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (SELECT TOP (100) * FROM #hdr_soft ORDER BY [id]) SoftSample;
SET @hdr_soft_keys = ISNULL(@hdr_soft_keys, N'');
IF @hdr_delete > 100
  SET @hdr_soft_keys = CONCAT(@hdr_soft_keys, N'; ...(+', CAST(@hdr_delete - 100 AS NVARCHAR(20)), N')');

DECLARE @item_soft_keys NVARCHAR(MAX) = N'';
SELECT @item_soft_keys = STRING_AGG(
  CAST(
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([material_document_code], N''), N'/', CAST([line_number] AS NVARCHAR(20)))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (SELECT TOP (100) * FROM #item_soft ORDER BY [id]) SoftSample;
SET @item_soft_keys = ISNULL(@item_soft_keys, N'');
IF @item_delete > 100
  SET @item_soft_keys = CONCAT(@item_soft_keys, N'; ...(+', CAST(@item_delete - 100 AS NVARCHAR(20)), N')');

DECLARE @json_result NVARCHAR(MAX) =
  N'{"hdr_keys":' + CAST(@hdr_sap_keys AS NVARCHAR)
  + N',"hdr_source":' + CAST(@hdr_source AS NVARCHAR)
  + N',"hdr_before":' + CAST(@hdr_before AS NVARCHAR)
  + N',"hdr_after":' + CAST(@hdr_after AS NVARCHAR)
  + N',"hdr_insert":' + CAST(@hdr_ins AS NVARCHAR)
  + N',"hdr_update":' + CAST(@hdr_upd AS NVARCHAR)
  + N',"hdr_unchanged":' + CAST(@hdr_unchanged AS NVARCHAR)
  + N',"hdr_soft_delete":' + CAST(@hdr_delete AS NVARCHAR)
  + N',"item_keys":' + CAST(@item_sap_keys AS NVARCHAR)
  + N',"item_source":' + CAST(@item_source AS NVARCHAR)
  + N',"item_before":' + CAST(@item_before AS NVARCHAR)
  + N',"item_after":' + CAST(@item_after AS NVARCHAR)
  + N',"item_insert":' + CAST(@item_ins AS NVARCHAR)
  + N',"item_update":' + CAST(@item_upd AS NVARCHAR)
  + N',"item_unchanged":' + CAST(@item_unchanged AS NVARCHAR)
  + N',"item_soft_delete":' + CAST(@item_delete AS NVARCHAR)
  + N'}';



INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC', N'SYNC', N'物料凭证',
  N'exec_sql_merge', 'SQL', N'/sync/material-document',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'material_document' AS NVARCHAR(40)) AS [scope],
  @hdr_sap_keys AS [source_raw_count],
  @hdr_source AS [source_count],
  @hdr_before AS [target_before],
  @hdr_after AS [target_after],
  @hdr_after AS [target_physical],
  @hdr_delete AS [soft_deleted],
  @hdr_ins AS [insert_count],
  @hdr_upd AS [update_count],
  @hdr_unchanged AS [unchanged_count],
  @hdr_delete AS [delete_count],
  @hdr_soft_keys AS [soft_deleted_keys]
UNION ALL
SELECT
  N'QUARTZ_SYNC_SUMMARY',
  CAST(N'material_document_item' AS NVARCHAR(40)),
  @item_sap_keys,
  @item_source,
  @item_before,
  @item_after,
  @item_after,
  @item_delete,
  @item_ins,
  @item_upd,
  @item_unchanged,
  @item_delete,
  @item_soft_keys;
