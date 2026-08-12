SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体一致（明细无 fiscal_year）
-- {{SourceDatabase}}.dbo.takt_logistics_procurement_purchase_invoice[_item] → 当前租户库同名表
-- 主表唯一键：Tenant+Company+fiscal_year+purchase_invoice_code
-- 明细唯一键：purchase_invoice_id+LineNumber
-- #item 临时列 fiscal_year：从源主表 JOIN（SH.id = R.purchase_invoice_id）取得

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#hdr_soft') IS NOT NULL DROP TABLE #hdr_soft;
IF OBJECT_ID('tempdb..#item_soft') IS NOT NULL DROP TABLE #item_soft;

CREATE TABLE #hdr (
  [rn] INT,
  [id] BIGINT,
  [purchase_invoice_code] NVARCHAR(10),
  [fiscal_year] NVARCHAR(4),
  [document_type] NVARCHAR(2),
  [document_date] DATETIME,
  [posting_date] DATETIME,
  [posted_by] NVARCHAR(12),
  [transaction_code] NVARCHAR(20),
  [transaction_event_type] NVARCHAR(2),
  [reference_code] NVARCHAR(16),
  [supplier_code] NVARCHAR(10),
  [currency_code] NVARCHAR(3),
  [exchange_rate] DECIMAL(18,5),
  [gross_amount] DECIMAL(18,2),
  [vat_amount] DECIMAL(18,2),
  [tax_code] NVARCHAR(2),
  [payment_terms] NVARCHAR(4),
  [invoice_flag] NVARCHAR(1),
  [header_text] NVARCHAR(25),
  [calculate_tax_flag] NVARCHAR(1),
  [reversal_document_code] NVARCHAR(10),
  [reversal_fiscal_year] NVARCHAR(4),
  [invoice_verification_category] NVARCHAR(1),
  [invoice_verification_type] NVARCHAR(1),
  [invoice_status] NVARCHAR(1),
  [supplying_country] NVARCHAR(3),
  [scb_indicator] NVARCHAR(3),
  [tax_exchange_rate] DECIMAL(18,5),
  [payment_method] NVARCHAR(1),
  [baseline_date] DATETIME,
  [entered_by] NVARCHAR(12),
  [branch_account] NVARCHAR(10),
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [is_deleted] INT,
  [updated_by] BIGINT
);

CREATE TABLE #item (
  [rn] INT,
  [id] BIGINT,
  [purchase_invoice_id] BIGINT,
  [fiscal_year] NVARCHAR(4),
  [purchase_invoice_code] NVARCHAR(10),
  [line_number] INT,
  [plant_code] NVARCHAR(4),
  [purchase_order_code] NVARCHAR(20),
  [purchase_order_item] INT,
  [account_assignment_seq] NVARCHAR(2),
  [material_code] NVARCHAR(20),
  [valuation_area] NVARCHAR(4),
  [amount] DECIMAL(18,2),
  [debit_credit_indicator] NVARCHAR(1),
  [tax_code] NVARCHAR(2),
  [quantity] DECIMAL(18,3),
  [order_unit] NVARCHAR(3),
  [po_price_quantity] DECIMAL(18,3),
  [po_price_unit] NVARCHAR(3),
  [valuated_stock_quantity] DECIMAL(18,3),
  [previous_period_stock] DECIMAL(18,3),
  [base_unit] NVARCHAR(3),
  [item_category] NVARCHAR(1),
  [account_assignment_category] NVARCHAR(1),
  [valuation_class] NVARCHAR(4),
  [final_invoice_flag] NVARCHAR(1),
  [update_po_history_flag] NVARCHAR(1),
  [subsequent_debit_credit] NVARCHAR(1),
  [block_reason_quantity] NVARCHAR(1),
  [value_string] NVARCHAR(4),
  [reference_code] NVARCHAR(16),
  [return_posting_flag] NVARCHAR(1),
  [delivery_cost_share] DECIMAL(18,2),
  [total_valuated_stock_value] DECIMAL(18,2),
  [previous_period_value] DECIMAL(18,2),
  [reference_document_code] NVARCHAR(10),
  [reference_document_year] NVARCHAR(4),
  [reference_document_item] INT,
  [stock_managed_material_code] NVARCHAR(20),
  [material_document_item] INT,
  [is_obsolete] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [is_deleted] INT,
  [updated_by] BIGINT
);

CREATE TABLE #hdr_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [fiscal_year] NVARCHAR(4),
  [purchase_invoice_code] NVARCHAR(10)
);
CREATE TABLE #item_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [purchase_invoice_code] NVARCHAR(10), [line_number] INT
);
CREATE TABLE #hdr_soft (
  [id] BIGINT,
  [fiscal_year] NVARCHAR(4),
  [purchase_invoice_code] NVARCHAR(10)
);
CREATE TABLE #item_soft (
  [id] BIGINT,
  [purchase_invoice_code] NVARCHAR(10),
  [line_number] INT
);

INSERT INTO #hdr
SELECT
  S.rn,
  @base_id + S.rn,
  S.[purchase_invoice_code],
  S.[fiscal_year],
  S.[document_type],
  S.[document_date],
  S.[posting_date],
  S.[posted_by],
  S.[transaction_code],
  S.[transaction_event_type],
  S.[reference_code],
  S.[supplier_code],
  S.[currency_code],
  S.[exchange_rate],
  S.[gross_amount],
  S.[vat_amount],
  S.[tax_code],
  S.[payment_terms],
  S.[invoice_flag],
  S.[header_text],
  S.[calculate_tax_flag],
  S.[reversal_document_code],
  S.[reversal_fiscal_year],
  S.[invoice_verification_category],
  S.[invoice_verification_type],
  S.[invoice_status],
  S.[supplying_country],
  S.[scb_indicator],
  S.[tax_exchange_rate],
  S.[payment_method],
  S.[baseline_date],
  S.[entered_by],
  S.[branch_account],
  @tenant_code,
  @company_code,
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[fiscal_year], N.[purchase_invoice_code]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_invoice_code])), 10), N''), N'') AS [purchase_invoice_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[fiscal_year])), 4), N''), N'') AS [fiscal_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_type])), 2), N'') AS [document_type],
      ISNULL(TRY_CAST(R.[document_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [document_date],
      ISNULL(TRY_CAST(R.[posting_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [posting_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[posted_by])), 12), N'') AS [posted_by],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_code])), 20), N'') AS [transaction_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_event_type])), 2), N'') AS [transaction_event_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_code])), 16), N'') AS [reference_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[supplier_code])), 10), N''), N'') AS [supplier_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N''), N'CNY') AS [currency_code],
      ROUND(TRY_CAST(R.[exchange_rate] AS DECIMAL(18,5)), 5) AS [exchange_rate],
      ISNULL(ROUND(TRY_CAST(R.[gross_amount] AS DECIMAL(18,2)), 2), 0) AS [gross_amount],
      ROUND(TRY_CAST(R.[vat_amount] AS DECIMAL(18,2)), 2) AS [vat_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_code])), 2), N'') AS [tax_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payment_terms])), 4), N'') AS [payment_terms],
      NULLIF(LEFT(LTRIM(RTRIM(R.[invoice_flag])), 1), N'') AS [invoice_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[header_text])), 25), N'') AS [header_text],
      NULLIF(LEFT(LTRIM(RTRIM(R.[calculate_tax_flag])), 1), N'') AS [calculate_tax_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reversal_document_code])), 10), N'') AS [reversal_document_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reversal_fiscal_year])), 4), N'') AS [reversal_fiscal_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[invoice_verification_category])), 1), N'') AS [invoice_verification_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[invoice_verification_type])), 1), N'') AS [invoice_verification_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[invoice_status])), 1), N'') AS [invoice_status],
      NULLIF(LEFT(LTRIM(RTRIM(R.[supplying_country])), 3), N'') AS [supplying_country],
      NULLIF(LEFT(LTRIM(RTRIM(R.[scb_indicator])), 3), N'') AS [scb_indicator],
      ROUND(TRY_CAST(R.[tax_exchange_rate] AS DECIMAL(18,5)), 5) AS [tax_exchange_rate],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payment_method])), 1), N'') AS [payment_method],
      TRY_CAST(R.[baseline_date] AS DATETIME) AS [baseline_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[entered_by])), 12), N'') AS [entered_by],
      NULLIF(LEFT(LTRIM(RTRIM(R.[branch_account])), 10), N'') AS [branch_account],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY LTRIM(RTRIM(R.[fiscal_year])), LTRIM(RTRIM(R.[purchase_invoice_code]))
        ORDER BY LTRIM(RTRIM(R.[fiscal_year])), LTRIM(RTRIM(R.[purchase_invoice_code]))
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] R
    WHERE LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[purchase_invoice_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @hdr_source INT = (SELECT COUNT(*) FROM #hdr);
DECLARE @hdr_sap_keys INT = (
  SELECT COUNT(*)
  FROM (
    SELECT LTRIM(RTRIM(R.[fiscal_year])) AS [fiscal_year],
           LTRIM(RTRIM(R.[purchase_invoice_code])) AS [purchase_invoice_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] R
    WHERE LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[purchase_invoice_code], N''))) <> N''
    GROUP BY LTRIM(RTRIM(R.[fiscal_year])), LTRIM(RTRIM(R.[purchase_invoice_code]))
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
LEFT JOIN [takt_logistics_procurement_purchase_invoice] T
  ON T.[tenant_code] = @tenant_code
 AND T.[company_code] = @company_code
 AND LTRIM(RTRIM(T.[fiscal_year])) = S.[fiscal_year]
 AND LTRIM(RTRIM(T.[purchase_invoice_code])) = S.[purchase_invoice_code];

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_procurement_purchase_invoice] AS T
USING #hdr AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[fiscal_year])) = S.[fiscal_year]
AND LTRIM(RTRIM(T.[purchase_invoice_code])) = S.[purchase_invoice_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR EXISTS (
    SELECT
      S.[document_type],
      S.[document_date],
      S.[posting_date],
      S.[posted_by],
      S.[transaction_code],
      S.[transaction_event_type],
      S.[reference_code],
      S.[supplier_code],
      S.[currency_code],
      S.[exchange_rate],
      S.[gross_amount],
      S.[vat_amount],
      S.[tax_code],
      S.[payment_terms],
      S.[invoice_flag],
      S.[header_text],
      S.[calculate_tax_flag],
      S.[reversal_document_code],
      S.[reversal_fiscal_year],
      S.[invoice_verification_category],
      S.[invoice_verification_type],
      S.[invoice_status],
      S.[supplying_country],
      S.[scb_indicator],
      S.[tax_exchange_rate],
      S.[payment_method],
      S.[baseline_date],
      S.[entered_by],
      S.[branch_account]
    EXCEPT
    SELECT
      T.[document_type],
      T.[document_date],
      T.[posting_date],
      T.[posted_by],
      T.[transaction_code],
      T.[transaction_event_type],
      T.[reference_code],
      T.[supplier_code],
      T.[currency_code],
      T.[exchange_rate],
      T.[gross_amount],
      T.[vat_amount],
      T.[tax_code],
      T.[payment_terms],
      T.[invoice_flag],
      T.[header_text],
      T.[calculate_tax_flag],
      T.[reversal_document_code],
      T.[reversal_fiscal_year],
      T.[invoice_verification_category],
      T.[invoice_verification_type],
      T.[invoice_status],
      T.[supplying_country],
      T.[scb_indicator],
      T.[tax_exchange_rate],
      T.[payment_method],
      T.[baseline_date],
      T.[entered_by],
      T.[branch_account]
  )
) THEN
  UPDATE SET
    T.[document_type] = S.[document_type],
    T.[document_date] = S.[document_date],
    T.[posting_date] = S.[posting_date],
    T.[posted_by] = S.[posted_by],
    T.[transaction_code] = S.[transaction_code],
    T.[transaction_event_type] = S.[transaction_event_type],
    T.[reference_code] = S.[reference_code],
    T.[supplier_code] = S.[supplier_code],
    T.[currency_code] = S.[currency_code],
    T.[exchange_rate] = S.[exchange_rate],
    T.[gross_amount] = S.[gross_amount],
    T.[vat_amount] = S.[vat_amount],
    T.[tax_code] = S.[tax_code],
    T.[payment_terms] = S.[payment_terms],
    T.[invoice_flag] = S.[invoice_flag],
    T.[header_text] = S.[header_text],
    T.[calculate_tax_flag] = S.[calculate_tax_flag],
    T.[reversal_document_code] = S.[reversal_document_code],
    T.[reversal_fiscal_year] = S.[reversal_fiscal_year],
    T.[invoice_verification_category] = S.[invoice_verification_category],
    T.[invoice_verification_type] = S.[invoice_verification_type],
    T.[invoice_status] = S.[invoice_status],
    T.[supplying_country] = S.[supplying_country],
    T.[scb_indicator] = S.[scb_indicator],
    T.[tax_exchange_rate] = S.[tax_exchange_rate],
    T.[payment_method] = S.[payment_method],
    T.[baseline_date] = S.[baseline_date],
    T.[entered_by] = S.[entered_by],
    T.[branch_account] = S.[branch_account],
        T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT ([id],[purchase_invoice_code],[fiscal_year],[document_type],[document_date],[posting_date],[posted_by],[transaction_code],[transaction_event_type],[reference_code],[supplier_code],[currency_code],[exchange_rate],[gross_amount],[vat_amount],[tax_code],[payment_terms],[invoice_flag],[header_text],[calculate_tax_flag],[reversal_document_code],[reversal_fiscal_year],[invoice_verification_category],[invoice_verification_type],[invoice_status],[supplying_country],[scb_indicator],[tax_exchange_rate],[payment_method],[baseline_date],[entered_by],[branch_account],[tenant_code],[company_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[purchase_invoice_code],S.[fiscal_year],S.[document_type],S.[document_date],S.[posting_date],S.[posted_by],S.[transaction_code],S.[transaction_event_type],S.[reference_code],S.[supplier_code],S.[currency_code],S.[exchange_rate],S.[gross_amount],S.[vat_amount],S.[tax_code],S.[payment_terms],S.[invoice_flag],S.[header_text],S.[calculate_tax_flag],S.[reversal_document_code],S.[reversal_fiscal_year],S.[invoice_verification_category],S.[invoice_verification_type],S.[invoice_status],S.[supplying_country],S.[scb_indicator],S.[tax_exchange_rate],S.[payment_method],S.[baseline_date],S.[entered_by],S.[branch_account],S.[tenant_code],S.[company_code],N'{}',N'',S.[updated_by],@now,S.[updated_by],@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END)
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[fiscal_year], INSERTED.[purchase_invoice_code]
INTO #hdr_delta (rn, oper_type, id, [fiscal_year], [purchase_invoice_code]);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[fiscal_year], INSERTED.[purchase_invoice_code]
INTO #hdr_soft ([id], [fiscal_year], [purchase_invoice_code])
FROM [takt_logistics_procurement_purchase_invoice] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1 FROM #hdr S
    WHERE S.[fiscal_year] = LTRIM(RTRIM(T.[fiscal_year]))
      AND S.[purchase_invoice_code] = LTRIM(RTRIM(T.[purchase_invoice_code]))
  );

DECLARE @hdr_delete INT = @@ROWCOUNT;

INSERT INTO #item
SELECT
  S.rn,
  @base_id + 1000000000 + S.rn,
  0,
  S.[fiscal_year],
  S.[purchase_invoice_code],
  S.[line_number],
  S.[plant_code],
  S.[purchase_order_code],
  S.[purchase_order_item],
  S.[account_assignment_seq],
  S.[material_code],
  S.[valuation_area],
  S.[amount],
  S.[debit_credit_indicator],
  S.[tax_code],
  S.[quantity],
  S.[order_unit],
  S.[po_price_quantity],
  S.[po_price_unit],
  S.[valuated_stock_quantity],
  S.[previous_period_stock],
  S.[base_unit],
  S.[item_category],
  S.[account_assignment_category],
  S.[valuation_class],
  S.[final_invoice_flag],
  S.[update_po_history_flag],
  S.[subsequent_debit_credit],
  S.[block_reason_quantity],
  S.[value_string],
  S.[reference_code],
  S.[return_posting_flag],
  S.[delivery_cost_share],
  S.[total_valuated_stock_value],
  S.[previous_period_value],
  S.[reference_document_code],
  S.[reference_document_year],
  S.[reference_document_item],
  S.[stock_managed_material_code],
  S.[material_document_item],
  S.[is_obsolete],
  @tenant_code,
  @company_code,
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[fiscal_year], N.[purchase_invoice_code], N.[line_number]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[fiscal_year])), 4), N''), N'') AS [fiscal_year],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_invoice_code])), 10), N''), N'') AS [purchase_invoice_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[plant_code])), 4), N'') AS [plant_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_order_code])), 20), N'') AS [purchase_order_code],
      TRY_CAST(R.[purchase_order_item] AS INT) AS [purchase_order_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[account_assignment_seq])), 2), N'') AS [account_assignment_seq],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N'') AS [material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[valuation_area])), 4), N'') AS [valuation_area],
      ROUND(TRY_CAST(R.[amount] AS DECIMAL(18,2)), 2) AS [amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[debit_credit_indicator])), 1), N'') AS [debit_credit_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_code])), 2), N'') AS [tax_code],
      ROUND(TRY_CAST(R.[quantity] AS DECIMAL(18,3)), 3) AS [quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[order_unit])), 3), N'') AS [order_unit],
      ROUND(TRY_CAST(R.[po_price_quantity] AS DECIMAL(18,3)), 3) AS [po_price_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[po_price_unit])), 3), N'') AS [po_price_unit],
      ROUND(TRY_CAST(R.[valuated_stock_quantity] AS DECIMAL(18,3)), 3) AS [valuated_stock_quantity],
      ROUND(TRY_CAST(R.[previous_period_stock] AS DECIMAL(18,3)), 3) AS [previous_period_stock],
      NULLIF(LEFT(LTRIM(RTRIM(R.[base_unit])), 3), N'') AS [base_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[item_category])), 1), N'') AS [item_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[account_assignment_category])), 1), N'') AS [account_assignment_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[valuation_class])), 4), N'') AS [valuation_class],
      NULLIF(LEFT(LTRIM(RTRIM(R.[final_invoice_flag])), 1), N'') AS [final_invoice_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[update_po_history_flag])), 1), N'') AS [update_po_history_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[subsequent_debit_credit])), 1), N'') AS [subsequent_debit_credit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[block_reason_quantity])), 1), N'') AS [block_reason_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[value_string])), 4), N'') AS [value_string],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_code])), 16), N'') AS [reference_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[return_posting_flag])), 1), N'') AS [return_posting_flag],
      ROUND(TRY_CAST(R.[delivery_cost_share] AS DECIMAL(18,2)), 2) AS [delivery_cost_share],
      ROUND(TRY_CAST(R.[total_valuated_stock_value] AS DECIMAL(18,2)), 2) AS [total_valuated_stock_value],
      ROUND(TRY_CAST(R.[previous_period_value] AS DECIMAL(18,2)), 2) AS [previous_period_value],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_code])), 10), N'') AS [reference_document_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_year])), 4), N'') AS [reference_document_year],
      TRY_CAST(R.[reference_document_item] AS INT) AS [reference_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[stock_managed_material_code])), 20), N'') AS [stock_managed_material_code],
      TRY_CAST(R.[material_document_item] AS INT) AS [material_document_item],
      ISNULL(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(SH.[fiscal_year])),
          LTRIM(RTRIM(R.[purchase_invoice_code])),
          COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
        ORDER BY COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] SH
      ON SH.[id] = R.[purchase_invoice_id]
    WHERE LTRIM(RTRIM(ISNULL(SH.[fiscal_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[purchase_invoice_code], N''))) <> N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0) > 0
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

UPDATE I
SET I.[purchase_invoice_id] = H.[id],
    I.[id] = COALESCE(T.[id], I.[id])
FROM #item I
INNER JOIN #hdr H
  ON H.[fiscal_year] = I.[fiscal_year]
 AND H.[purchase_invoice_code] = I.[purchase_invoice_code]
LEFT JOIN [takt_logistics_procurement_purchase_invoice_item] T
  ON T.[tenant_code] = @tenant_code
 AND T.[company_code] = @company_code
 AND T.[purchase_invoice_id] = H.[id]
 AND T.[line_number] = I.[line_number];

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item WHERE [purchase_invoice_id] <> 0);
DECLARE @item_sap_keys INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(SH.[fiscal_year])) AS [fiscal_year],
      LTRIM(RTRIM(R.[purchase_invoice_code])) AS [purchase_invoice_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] SH
      ON SH.[id] = R.[purchase_invoice_id]
    WHERE LTRIM(RTRIM(ISNULL(SH.[fiscal_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[purchase_invoice_code], N''))) <> N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0) > 0
    GROUP BY
      LTRIM(RTRIM(SH.[fiscal_year])),
      LTRIM(RTRIM(R.[purchase_invoice_code])),
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
  ) K
);
IF @item_source <> @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(
    N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;

DELETE FROM #item WHERE [purchase_invoice_id] = 0;

DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice_item]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_procurement_purchase_invoice_item] AS T
USING #item AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND T.[purchase_invoice_id] = S.[purchase_invoice_id]
AND T.[line_number] = S.[line_number]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR EXISTS (
    SELECT
      S.[plant_code],
      S.[purchase_order_code],
      S.[purchase_order_item],
      S.[account_assignment_seq],
      S.[material_code],
      S.[valuation_area],
      S.[amount],
      S.[debit_credit_indicator],
      S.[tax_code],
      S.[quantity],
      S.[order_unit],
      S.[po_price_quantity],
      S.[po_price_unit],
      S.[valuated_stock_quantity],
      S.[previous_period_stock],
      S.[base_unit],
      S.[item_category],
      S.[account_assignment_category],
      S.[valuation_class],
      S.[final_invoice_flag],
      S.[update_po_history_flag],
      S.[subsequent_debit_credit],
      S.[block_reason_quantity],
      S.[value_string],
      S.[reference_code],
      S.[return_posting_flag],
      S.[delivery_cost_share],
      S.[total_valuated_stock_value],
      S.[previous_period_value],
      S.[reference_document_code],
      S.[reference_document_year],
      S.[reference_document_item],
      S.[stock_managed_material_code],
      S.[material_document_item],
      S.[is_obsolete]
    EXCEPT
    SELECT
      T.[plant_code],
      T.[purchase_order_code],
      T.[purchase_order_item],
      T.[account_assignment_seq],
      T.[material_code],
      T.[valuation_area],
      T.[amount],
      T.[debit_credit_indicator],
      T.[tax_code],
      T.[quantity],
      T.[order_unit],
      T.[po_price_quantity],
      T.[po_price_unit],
      T.[valuated_stock_quantity],
      T.[previous_period_stock],
      T.[base_unit],
      T.[item_category],
      T.[account_assignment_category],
      T.[valuation_class],
      T.[final_invoice_flag],
      T.[update_po_history_flag],
      T.[subsequent_debit_credit],
      T.[block_reason_quantity],
      T.[value_string],
      T.[reference_code],
      T.[return_posting_flag],
      T.[delivery_cost_share],
      T.[total_valuated_stock_value],
      T.[previous_period_value],
      T.[reference_document_code],
      T.[reference_document_year],
      T.[reference_document_item],
      T.[stock_managed_material_code],
      T.[material_document_item],
      T.[is_obsolete]
  )
) THEN
  UPDATE SET
    T.[purchase_invoice_code] = S.[purchase_invoice_code],
    T.[plant_code] = S.[plant_code],
    T.[purchase_order_code] = S.[purchase_order_code],
    T.[purchase_order_item] = S.[purchase_order_item],
    T.[account_assignment_seq] = S.[account_assignment_seq],
    T.[material_code] = S.[material_code],
    T.[valuation_area] = S.[valuation_area],
    T.[amount] = S.[amount],
    T.[debit_credit_indicator] = S.[debit_credit_indicator],
    T.[tax_code] = S.[tax_code],
    T.[quantity] = S.[quantity],
    T.[order_unit] = S.[order_unit],
    T.[po_price_quantity] = S.[po_price_quantity],
    T.[po_price_unit] = S.[po_price_unit],
    T.[valuated_stock_quantity] = S.[valuated_stock_quantity],
    T.[previous_period_stock] = S.[previous_period_stock],
    T.[base_unit] = S.[base_unit],
    T.[item_category] = S.[item_category],
    T.[account_assignment_category] = S.[account_assignment_category],
    T.[valuation_class] = S.[valuation_class],
    T.[final_invoice_flag] = S.[final_invoice_flag],
    T.[update_po_history_flag] = S.[update_po_history_flag],
    T.[subsequent_debit_credit] = S.[subsequent_debit_credit],
    T.[block_reason_quantity] = S.[block_reason_quantity],
    T.[value_string] = S.[value_string],
    T.[reference_code] = S.[reference_code],
    T.[return_posting_flag] = S.[return_posting_flag],
    T.[delivery_cost_share] = S.[delivery_cost_share],
    T.[total_valuated_stock_value] = S.[total_valuated_stock_value],
    T.[previous_period_value] = S.[previous_period_value],
    T.[reference_document_code] = S.[reference_document_code],
    T.[reference_document_year] = S.[reference_document_year],
    T.[reference_document_item] = S.[reference_document_item],
    T.[stock_managed_material_code] = S.[stock_managed_material_code],
    T.[material_document_item] = S.[material_document_item],
    T.[is_obsolete] = S.[is_obsolete],
        T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT ([id],[purchase_invoice_id],[purchase_invoice_code],[line_number],[plant_code],[purchase_order_code],[purchase_order_item],[account_assignment_seq],[material_code],[valuation_area],[amount],[debit_credit_indicator],[tax_code],[quantity],[order_unit],[po_price_quantity],[po_price_unit],[valuated_stock_quantity],[previous_period_stock],[base_unit],[item_category],[account_assignment_category],[valuation_class],[final_invoice_flag],[update_po_history_flag],[subsequent_debit_credit],[block_reason_quantity],[value_string],[reference_code],[return_posting_flag],[delivery_cost_share],[total_valuated_stock_value],[previous_period_value],[reference_document_code],[reference_document_year],[reference_document_item],[stock_managed_material_code],[material_document_item],[is_obsolete],[tenant_code],[company_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[purchase_invoice_id],S.[purchase_invoice_code],S.[line_number],S.[plant_code],S.[purchase_order_code],S.[purchase_order_item],S.[account_assignment_seq],S.[material_code],S.[valuation_area],S.[amount],S.[debit_credit_indicator],S.[tax_code],S.[quantity],S.[order_unit],S.[po_price_quantity],S.[po_price_unit],S.[valuated_stock_quantity],S.[previous_period_stock],S.[base_unit],S.[item_category],S.[account_assignment_category],S.[valuation_class],S.[final_invoice_flag],S.[update_po_history_flag],S.[subsequent_debit_credit],S.[block_reason_quantity],S.[value_string],S.[reference_code],S.[return_posting_flag],S.[delivery_cost_share],S.[total_valuated_stock_value],S.[previous_period_value],S.[reference_document_code],S.[reference_document_year],S.[reference_document_item],S.[stock_managed_material_code],S.[material_document_item],S.[is_obsolete],S.[tenant_code],S.[company_code],N'{}',N'',S.[updated_by],@now,S.[updated_by],@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END)
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[purchase_invoice_code], INSERTED.[line_number]
INTO #item_delta (rn, oper_type, id, [purchase_invoice_code], [line_number]);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[purchase_invoice_code], INSERTED.[line_number]
INTO #item_soft ([id], [purchase_invoice_code], [line_number])
FROM [takt_logistics_procurement_purchase_invoice_item] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1 FROM #item S
    WHERE S.[purchase_invoice_id] = T.[purchase_invoice_id]
      AND S.[line_number] = T.[line_number]
  );

DECLARE @item_delete INT = @@ROWCOUNT;

DECLARE @hdr_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);
DECLARE @item_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice_item]
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
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([fiscal_year], N''), N'/', ISNULL([purchase_invoice_code], N''))
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
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([purchase_invoice_code], N''), N'/', CAST([line_number] AS NVARCHAR(20)))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (SELECT TOP (100) * FROM #item_soft ORDER BY [id]) SoftSample;
SET @item_soft_keys = ISNULL(@item_soft_keys, N'');
IF @item_delete > 100
  SET @item_soft_keys = CONCAT(@item_soft_keys, N'; ...(+', CAST(@item_delete - 100 AS NVARCHAR(20)), N')');

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  @hdr_before AS [hdr_before],
  @hdr_source AS [hdr_source],
  @hdr_ins AS [hdr_insert],
  @hdr_upd AS [hdr_update],
  @hdr_unchanged AS [hdr_unchanged],
  @hdr_delete AS [hdr_soft_delete],
  @hdr_after AS [hdr_after],
  @hdr_soft_keys AS [hdr_soft_sample],
  @item_before AS [item_before],
  @item_source AS [item_source],
  @item_ins AS [item_insert],
  @item_upd AS [item_update],
  @item_unchanged AS [item_unchanged],
  @item_delete AS [item_soft_delete],
  @item_after AS [item_after],
  @item_soft_keys AS [item_soft_sample];

