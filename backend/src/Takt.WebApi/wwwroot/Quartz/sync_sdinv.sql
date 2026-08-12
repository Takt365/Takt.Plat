SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体一致
-- {{SourceDatabase}}.dbo.takt_logistics_sales_invoice[_item] → 当前租户库同名表
-- 主表唯一键：Tenant+Company+billing_document_code（无 fiscal year）
-- 明细唯一键：sales_invoice_id+LineNumber（源侧 billing_document_code+line_number）

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#hdr_soft') IS NOT NULL DROP TABLE #hdr_soft;
IF OBJECT_ID('tempdb..#item_soft') IS NOT NULL DROP TABLE #item_soft;

CREATE TABLE #hdr (
  [rn] INT,
  [id] BIGINT,
  [billing_document_code] NVARCHAR(10),
  [billing_type] NVARCHAR(4),
  [billing_category] NVARCHAR(1),
  [document_category] NVARCHAR(1),
  [currency_code] NVARCHAR(3),
  [sales_organization] NVARCHAR(4),
  [distribution_channel] NVARCHAR(2),
  [pricing_procedure] NVARCHAR(6),
  [condition_code] NVARCHAR(10),
  [shipping_conditions] NVARCHAR(2),
  [billing_date] DATETIME,
  [customer_group] NVARCHAR(2),
  [price_list_type] NVARCHAR(2),
  [incoterms1] NVARCHAR(3),
  [incoterms2] NVARCHAR(28),
  [export_flag] NVARCHAR(1),
  [posting_status] NVARCHAR(1),
  [accounting_exchange_rate] DECIMAL(18,5),
  [fixed_exchange_rate_flag] NVARCHAR(1),
  [payment_terms] NVARCHAR(4),
  [payment_method] NVARCHAR(1),
  [account_assignment_group] NVARCHAR(2),
  [country_code] NVARCHAR(3),
  [region] NVARCHAR(3),
  [customer_tax_class1] NVARCHAR(1),
  [net_amount] DECIMAL(18,2),
  [combination_criteria] NVARCHAR(40),
  [posted_by] NVARCHAR(12),
  [update_group] NVARCHAR(6),
  [payer_code] NVARCHAR(10),
  [customer_code] NVARCHAR(10),
  [dunning_area] NVARCHAR(2),
  [statistics_currency_code] NVARCHAR(3),
  [foreign_trade_code] NVARCHAR(10),
  [cancelled_billing_document] NVARCHAR(10),
  [agreement_code] NVARCHAR(10),
  [invoice_list_type] NVARCHAR(4),
  [invoice_list_date] DATETIME,
  [exchange_rate_type] NVARCHAR(4),
  [dunning_key] NVARCHAR(1),
  [dunning_block] NVARCHAR(1),
  [division] NVARCHAR(2),
  [credit_control_area] NVARCHAR(4),
  [credit_account] NVARCHAR(10),
  [credit_currency_code] NVARCHAR(3),
  [credit_exchange_rate] DECIMAL(18,5),
  [hierarchy_type_pricing] NVARCHAR(1),
  [customer_purchase_order] NVARCHAR(35),
  [trading_partner] NVARCHAR(6),
  [tax_departure_country] NVARCHAR(3),
  [organization_sales_tax_number] NVARCHAR(20),
  [country_sales_tax_number] NVARCHAR(20),
  [reference_code] NVARCHAR(16),
  [assignment] NVARCHAR(18),
  [tax_amount] DECIMAL(18,2),
  [logical_system] NVARCHAR(10),
  [cancelled_flag] NVARCHAR(1),
  [exchange_rate_date] DATETIME,
  [payment_reference] NVARCHAR(30),
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [is_deleted] INT,
  [updated_by] BIGINT
);

CREATE TABLE #item (
  [rn] INT,
  [id] BIGINT,
  [sales_invoice_id] BIGINT,
  [plant_code] NVARCHAR(4),
  [billing_document_code] NVARCHAR(10),
  [line_number] INT,
  [billing_quantity] DECIMAL(18,3),
  [sales_unit] NVARCHAR(3),
  [numerator] DECIMAL(18,0),
  [denominator] DECIMAL(18,0),
  [base_unit] NVARCHAR(3),
  [scale_quantity] DECIMAL(18,3),
  [billing_quantity_sku] DECIMAL(18,3),
  [required_quantity] DECIMAL(18,3),
  [net_weight] DECIMAL(18,3),
  [gross_weight] DECIMAL(18,3),
  [weight_unit] NVARCHAR(3),
  [volume] DECIMAL(18,3),
  [volume_unit] NVARCHAR(3),
  [business_area_code] NVARCHAR(4),
  [pricing_date] DATETIME,
  [service_rendered_date] DATETIME,
  [pricing_exchange_rate] DECIMAL(18,5),
  [net_amount] DECIMAL(18,2),
  [origin_document_code] NVARCHAR(10),
  [origin_document_item] INT,
  [reference_document_code] NVARCHAR(10),
  [reference_document_item] INT,
  [reference_document_category] NVARCHAR(1),
  [sales_document_code] NVARCHAR(20),
  [sales_document_item] INT,
  [sales_document_reference_flag] NVARCHAR(1),
  [material_code] NVARCHAR(20),
  [material_description] NVARCHAR(40),
  [material_group] NVARCHAR(9),
  [sales_item_category] NVARCHAR(4),
  [item_type] NVARCHAR(1),
  [product_hierarchy] NVARCHAR(18),
  [shipping_point] NVARCHAR(4),
  [replacement_part_flag] NVARCHAR(1),
  [division] NVARCHAR(2),
  [partner_item] INT,
  [departure_country] NVARCHAR(3),
  [plant_region] NVARCHAR(3),
  [statistical_value_flag] NVARCHAR(1),
  [pricing_flag] NVARCHAR(1),
  [cash_discount_flag] NVARCHAR(1),
  [cash_discount_base] DECIMAL(18,2),
  [cost_center_code] NVARCHAR(10),
  [sales_office] NVARCHAR(4),
  [division_for_order] NVARCHAR(2),
  [debit_credit_indicator] NVARCHAR(1),
  [posted_by] NVARCHAR(12),
  [valuation_type] NVARCHAR(10),
  [warehouse_code] NVARCHAR(4),
  [update_group] NVARCHAR(6),
  [cost_amount] DECIMAL(18,2),
  [subtotal1] DECIMAL(18,2),
  [subtotal2] DECIMAL(18,2),
  [subtotal3] DECIMAL(18,2),
  [subtotal4] DECIMAL(18,2),
  [subtotal5] DECIMAL(18,2),
  [subtotal6] DECIMAL(18,2),
  [statistics_exchange_rate] DECIMAL(18,5),
  [international_article_number] NVARCHAR(18),
  [profit_center_code] NVARCHAR(10),
  [material_group4] NVARCHAR(3),
  [entered_material_code] NVARCHAR(20),
  [controlling_area_code] NVARCHAR(4),
  [profitability_segment] NVARCHAR(10),
  [credit_price] DECIMAL(18,2),
  [credit_active_flag] NVARCHAR(1),
  [customer_group_sales_order] NVARCHAR(2),
  [destination_country_order] NVARCHAR(3),
  [manual_pricing_flag] NVARCHAR(1),
  [price_list_order] NVARCHAR(2),
  [region_order] NVARCHAR(3),
  [sales_organization_order] NVARCHAR(4),
  [distribution_channel_order] NVARCHAR(2),
  [document_category] NVARCHAR(1),
  [tax_amount] DECIMAL(18,2),
  [order_reason] NVARCHAR(3),
  [payment_guarantee_form] NVARCHAR(2),
  [gross_amount] DECIMAL(18,2),
  [exchange_rate_date] DATETIME,
  [is_obsolete] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [is_deleted] INT,
  [updated_by] BIGINT
);

CREATE TABLE #hdr_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [billing_document_code] NVARCHAR(10)
);
CREATE TABLE #item_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [billing_document_code] NVARCHAR(10), [line_number] INT
);
CREATE TABLE #hdr_soft (
  [id] BIGINT,
  [billing_document_code] NVARCHAR(10)
);
CREATE TABLE #item_soft (
  [id] BIGINT,
  [billing_document_code] NVARCHAR(10),
  [line_number] INT
);

INSERT INTO #hdr
SELECT
  S.rn,
  @base_id + S.rn,
  S.[billing_document_code],
  S.[billing_type],
  S.[billing_category],
  S.[document_category],
  S.[currency_code],
  S.[sales_organization],
  S.[distribution_channel],
  S.[pricing_procedure],
  S.[condition_code],
  S.[shipping_conditions],
  S.[billing_date],
  S.[customer_group],
  S.[price_list_type],
  S.[incoterms1],
  S.[incoterms2],
  S.[export_flag],
  S.[posting_status],
  S.[accounting_exchange_rate],
  S.[fixed_exchange_rate_flag],
  S.[payment_terms],
  S.[payment_method],
  S.[account_assignment_group],
  S.[country_code],
  S.[region],
  S.[customer_tax_class1],
  S.[net_amount],
  S.[combination_criteria],
  S.[posted_by],
  S.[update_group],
  S.[payer_code],
  S.[customer_code],
  S.[dunning_area],
  S.[statistics_currency_code],
  S.[foreign_trade_code],
  S.[cancelled_billing_document],
  S.[agreement_code],
  S.[invoice_list_type],
  S.[invoice_list_date],
  S.[exchange_rate_type],
  S.[dunning_key],
  S.[dunning_block],
  S.[division],
  S.[credit_control_area],
  S.[credit_account],
  S.[credit_currency_code],
  S.[credit_exchange_rate],
  S.[hierarchy_type_pricing],
  S.[customer_purchase_order],
  S.[trading_partner],
  S.[tax_departure_country],
  S.[organization_sales_tax_number],
  S.[country_sales_tax_number],
  S.[reference_code],
  S.[assignment],
  S.[tax_amount],
  S.[logical_system],
  S.[cancelled_flag],
  S.[exchange_rate_date],
  S.[payment_reference],
  @tenant_code,
  @company_code,
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[billing_document_code]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N''), N'') AS [billing_document_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[billing_type])), 4), N'') AS [billing_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[billing_category])), 1), N'') AS [billing_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_category])), 1), N'') AS [document_category],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N''), N'CNY') AS [currency_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_organization])), 4), N'') AS [sales_organization],
      NULLIF(LEFT(LTRIM(RTRIM(R.[distribution_channel])), 2), N'') AS [distribution_channel],
      NULLIF(LEFT(LTRIM(RTRIM(R.[pricing_procedure])), 6), N'') AS [pricing_procedure],
      NULLIF(LEFT(LTRIM(RTRIM(R.[condition_code])), 10), N'') AS [condition_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[shipping_conditions])), 2), N'') AS [shipping_conditions],
      ISNULL(TRY_CAST(R.[billing_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [billing_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[customer_group])), 2), N'') AS [customer_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[price_list_type])), 2), N'') AS [price_list_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[incoterms1])), 3), N'') AS [incoterms1],
      NULLIF(LEFT(LTRIM(RTRIM(R.[incoterms2])), 28), N'') AS [incoterms2],
      NULLIF(LEFT(LTRIM(RTRIM(R.[export_flag])), 1), N'') AS [export_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[posting_status])), 1), N'') AS [posting_status],
      ROUND(TRY_CAST(R.[accounting_exchange_rate] AS DECIMAL(18,5)), 5) AS [accounting_exchange_rate],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fixed_exchange_rate_flag])), 1), N'') AS [fixed_exchange_rate_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payment_terms])), 4), N'') AS [payment_terms],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payment_method])), 1), N'') AS [payment_method],
      NULLIF(LEFT(LTRIM(RTRIM(R.[account_assignment_group])), 2), N'') AS [account_assignment_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[country_code])), 3), N'') AS [country_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[region])), 3), N'') AS [region],
      NULLIF(LEFT(LTRIM(RTRIM(R.[customer_tax_class1])), 1), N'') AS [customer_tax_class1],
      ISNULL(ROUND(TRY_CAST(R.[net_amount] AS DECIMAL(18,2)), 2), 0) AS [net_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[combination_criteria])), 40), N'') AS [combination_criteria],
      NULLIF(LEFT(LTRIM(RTRIM(R.[posted_by])), 12), N'') AS [posted_by],
      NULLIF(LEFT(LTRIM(RTRIM(R.[update_group])), 6), N'') AS [update_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payer_code])), 10), N'') AS [payer_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[customer_code])), 10), N''), N'') AS [customer_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[dunning_area])), 2), N'') AS [dunning_area],
      NULLIF(LEFT(LTRIM(RTRIM(R.[statistics_currency_code])), 3), N'') AS [statistics_currency_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[foreign_trade_code])), 10), N'') AS [foreign_trade_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cancelled_billing_document])), 10), N'') AS [cancelled_billing_document],
      NULLIF(LEFT(LTRIM(RTRIM(R.[agreement_code])), 10), N'') AS [agreement_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[invoice_list_type])), 4), N'') AS [invoice_list_type],
      TRY_CAST(R.[invoice_list_date] AS DATETIME) AS [invoice_list_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[exchange_rate_type])), 4), N'') AS [exchange_rate_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[dunning_key])), 1), N'') AS [dunning_key],
      NULLIF(LEFT(LTRIM(RTRIM(R.[dunning_block])), 1), N'') AS [dunning_block],
      NULLIF(LEFT(LTRIM(RTRIM(R.[division])), 2), N'') AS [division],
      NULLIF(LEFT(LTRIM(RTRIM(R.[credit_control_area])), 4), N'') AS [credit_control_area],
      NULLIF(LEFT(LTRIM(RTRIM(R.[credit_account])), 10), N'') AS [credit_account],
      NULLIF(LEFT(LTRIM(RTRIM(R.[credit_currency_code])), 3), N'') AS [credit_currency_code],
      ROUND(TRY_CAST(R.[credit_exchange_rate] AS DECIMAL(18,5)), 5) AS [credit_exchange_rate],
      NULLIF(LEFT(LTRIM(RTRIM(R.[hierarchy_type_pricing])), 1), N'') AS [hierarchy_type_pricing],
      NULLIF(LEFT(LTRIM(RTRIM(R.[customer_purchase_order])), 35), N'') AS [customer_purchase_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[trading_partner])), 6), N'') AS [trading_partner],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_departure_country])), 3), N'') AS [tax_departure_country],
      NULLIF(LEFT(LTRIM(RTRIM(R.[organization_sales_tax_number])), 20), N'') AS [organization_sales_tax_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[country_sales_tax_number])), 20), N'') AS [country_sales_tax_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_code])), 16), N'') AS [reference_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[assignment])), 18), N'') AS [assignment],
      ROUND(TRY_CAST(R.[tax_amount] AS DECIMAL(18,2)), 2) AS [tax_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[logical_system])), 10), N'') AS [logical_system],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cancelled_flag])), 1), N'') AS [cancelled_flag],
      TRY_CAST(R.[exchange_rate_date] AS DATETIME) AS [exchange_rate_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payment_reference])), 30), N'') AS [payment_reference],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY LTRIM(RTRIM(R.[billing_document_code]))
        ORDER BY LTRIM(RTRIM(R.[billing_document_code]))
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice] R
    WHERE LTRIM(RTRIM(ISNULL(R.[billing_document_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @hdr_source INT = (SELECT COUNT(*) FROM #hdr);
DECLARE @hdr_sap_keys INT = (
  SELECT COUNT(*)
  FROM (
    SELECT LTRIM(RTRIM(R.[billing_document_code])) AS [billing_document_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice] R
    WHERE LTRIM(RTRIM(ISNULL(R.[billing_document_code], N''))) <> N''
    GROUP BY LTRIM(RTRIM(R.[billing_document_code]))
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
LEFT JOIN [takt_logistics_sales_invoice] T
  ON T.[tenant_code] = @tenant_code
 AND T.[company_code] = @company_code
 AND LTRIM(RTRIM(T.[billing_document_code])) = S.[billing_document_code];

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_sales_invoice] AS T
USING #hdr AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[billing_document_code])) = S.[billing_document_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR EXISTS (
    SELECT
      S.[billing_type],
      S.[billing_category],
      S.[document_category],
      S.[currency_code],
      S.[sales_organization],
      S.[distribution_channel],
      S.[pricing_procedure],
      S.[condition_code],
      S.[shipping_conditions],
      S.[billing_date],
      S.[customer_group],
      S.[price_list_type],
      S.[incoterms1],
      S.[incoterms2],
      S.[export_flag],
      S.[posting_status],
      S.[accounting_exchange_rate],
      S.[fixed_exchange_rate_flag],
      S.[payment_terms],
      S.[payment_method],
      S.[account_assignment_group],
      S.[country_code],
      S.[region],
      S.[customer_tax_class1],
      S.[net_amount],
      S.[combination_criteria],
      S.[posted_by],
      S.[update_group],
      S.[payer_code],
      S.[customer_code],
      S.[dunning_area],
      S.[statistics_currency_code],
      S.[foreign_trade_code],
      S.[cancelled_billing_document],
      S.[agreement_code],
      S.[invoice_list_type],
      S.[invoice_list_date],
      S.[exchange_rate_type],
      S.[dunning_key],
      S.[dunning_block],
      S.[division],
      S.[credit_control_area],
      S.[credit_account],
      S.[credit_currency_code],
      S.[credit_exchange_rate],
      S.[hierarchy_type_pricing],
      S.[customer_purchase_order],
      S.[trading_partner],
      S.[tax_departure_country],
      S.[organization_sales_tax_number],
      S.[country_sales_tax_number],
      S.[reference_code],
      S.[assignment],
      S.[tax_amount],
      S.[logical_system],
      S.[cancelled_flag],
      S.[exchange_rate_date],
      S.[payment_reference]
    EXCEPT
    SELECT
      T.[billing_type],
      T.[billing_category],
      T.[document_category],
      T.[currency_code],
      T.[sales_organization],
      T.[distribution_channel],
      T.[pricing_procedure],
      T.[condition_code],
      T.[shipping_conditions],
      T.[billing_date],
      T.[customer_group],
      T.[price_list_type],
      T.[incoterms1],
      T.[incoterms2],
      T.[export_flag],
      T.[posting_status],
      T.[accounting_exchange_rate],
      T.[fixed_exchange_rate_flag],
      T.[payment_terms],
      T.[payment_method],
      T.[account_assignment_group],
      T.[country_code],
      T.[region],
      T.[customer_tax_class1],
      T.[net_amount],
      T.[combination_criteria],
      T.[posted_by],
      T.[update_group],
      T.[payer_code],
      T.[customer_code],
      T.[dunning_area],
      T.[statistics_currency_code],
      T.[foreign_trade_code],
      T.[cancelled_billing_document],
      T.[agreement_code],
      T.[invoice_list_type],
      T.[invoice_list_date],
      T.[exchange_rate_type],
      T.[dunning_key],
      T.[dunning_block],
      T.[division],
      T.[credit_control_area],
      T.[credit_account],
      T.[credit_currency_code],
      T.[credit_exchange_rate],
      T.[hierarchy_type_pricing],
      T.[customer_purchase_order],
      T.[trading_partner],
      T.[tax_departure_country],
      T.[organization_sales_tax_number],
      T.[country_sales_tax_number],
      T.[reference_code],
      T.[assignment],
      T.[tax_amount],
      T.[logical_system],
      T.[cancelled_flag],
      T.[exchange_rate_date],
      T.[payment_reference]
  )
) THEN
  UPDATE SET
    T.[billing_type] = S.[billing_type],
    T.[billing_category] = S.[billing_category],
    T.[document_category] = S.[document_category],
    T.[currency_code] = S.[currency_code],
    T.[sales_organization] = S.[sales_organization],
    T.[distribution_channel] = S.[distribution_channel],
    T.[pricing_procedure] = S.[pricing_procedure],
    T.[condition_code] = S.[condition_code],
    T.[shipping_conditions] = S.[shipping_conditions],
    T.[billing_date] = S.[billing_date],
    T.[customer_group] = S.[customer_group],
    T.[price_list_type] = S.[price_list_type],
    T.[incoterms1] = S.[incoterms1],
    T.[incoterms2] = S.[incoterms2],
    T.[export_flag] = S.[export_flag],
    T.[posting_status] = S.[posting_status],
    T.[accounting_exchange_rate] = S.[accounting_exchange_rate],
    T.[fixed_exchange_rate_flag] = S.[fixed_exchange_rate_flag],
    T.[payment_terms] = S.[payment_terms],
    T.[payment_method] = S.[payment_method],
    T.[account_assignment_group] = S.[account_assignment_group],
    T.[country_code] = S.[country_code],
    T.[region] = S.[region],
    T.[customer_tax_class1] = S.[customer_tax_class1],
    T.[net_amount] = S.[net_amount],
    T.[combination_criteria] = S.[combination_criteria],
    T.[posted_by] = S.[posted_by],
    T.[update_group] = S.[update_group],
    T.[payer_code] = S.[payer_code],
    T.[customer_code] = S.[customer_code],
    T.[dunning_area] = S.[dunning_area],
    T.[statistics_currency_code] = S.[statistics_currency_code],
    T.[foreign_trade_code] = S.[foreign_trade_code],
    T.[cancelled_billing_document] = S.[cancelled_billing_document],
    T.[agreement_code] = S.[agreement_code],
    T.[invoice_list_type] = S.[invoice_list_type],
    T.[invoice_list_date] = S.[invoice_list_date],
    T.[exchange_rate_type] = S.[exchange_rate_type],
    T.[dunning_key] = S.[dunning_key],
    T.[dunning_block] = S.[dunning_block],
    T.[division] = S.[division],
    T.[credit_control_area] = S.[credit_control_area],
    T.[credit_account] = S.[credit_account],
    T.[credit_currency_code] = S.[credit_currency_code],
    T.[credit_exchange_rate] = S.[credit_exchange_rate],
    T.[hierarchy_type_pricing] = S.[hierarchy_type_pricing],
    T.[customer_purchase_order] = S.[customer_purchase_order],
    T.[trading_partner] = S.[trading_partner],
    T.[tax_departure_country] = S.[tax_departure_country],
    T.[organization_sales_tax_number] = S.[organization_sales_tax_number],
    T.[country_sales_tax_number] = S.[country_sales_tax_number],
    T.[reference_code] = S.[reference_code],
    T.[assignment] = S.[assignment],
    T.[tax_amount] = S.[tax_amount],
    T.[logical_system] = S.[logical_system],
    T.[cancelled_flag] = S.[cancelled_flag],
    T.[exchange_rate_date] = S.[exchange_rate_date],
    T.[payment_reference] = S.[payment_reference],
        T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT ([id],[billing_document_code],[billing_type],[billing_category],[document_category],[currency_code],[sales_organization],[distribution_channel],[pricing_procedure],[condition_code],[shipping_conditions],[billing_date],[customer_group],[price_list_type],[incoterms1],[incoterms2],[export_flag],[posting_status],[accounting_exchange_rate],[fixed_exchange_rate_flag],[payment_terms],[payment_method],[account_assignment_group],[country_code],[region],[customer_tax_class1],[net_amount],[combination_criteria],[posted_by],[update_group],[payer_code],[customer_code],[dunning_area],[statistics_currency_code],[foreign_trade_code],[cancelled_billing_document],[agreement_code],[invoice_list_type],[invoice_list_date],[exchange_rate_type],[dunning_key],[dunning_block],[division],[credit_control_area],[credit_account],[credit_currency_code],[credit_exchange_rate],[hierarchy_type_pricing],[customer_purchase_order],[trading_partner],[tax_departure_country],[organization_sales_tax_number],[country_sales_tax_number],[reference_code],[assignment],[tax_amount],[logical_system],[cancelled_flag],[exchange_rate_date],[payment_reference],[tenant_code],[company_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[billing_document_code],S.[billing_type],S.[billing_category],S.[document_category],S.[currency_code],S.[sales_organization],S.[distribution_channel],S.[pricing_procedure],S.[condition_code],S.[shipping_conditions],S.[billing_date],S.[customer_group],S.[price_list_type],S.[incoterms1],S.[incoterms2],S.[export_flag],S.[posting_status],S.[accounting_exchange_rate],S.[fixed_exchange_rate_flag],S.[payment_terms],S.[payment_method],S.[account_assignment_group],S.[country_code],S.[region],S.[customer_tax_class1],S.[net_amount],S.[combination_criteria],S.[posted_by],S.[update_group],S.[payer_code],S.[customer_code],S.[dunning_area],S.[statistics_currency_code],S.[foreign_trade_code],S.[cancelled_billing_document],S.[agreement_code],S.[invoice_list_type],S.[invoice_list_date],S.[exchange_rate_type],S.[dunning_key],S.[dunning_block],S.[division],S.[credit_control_area],S.[credit_account],S.[credit_currency_code],S.[credit_exchange_rate],S.[hierarchy_type_pricing],S.[customer_purchase_order],S.[trading_partner],S.[tax_departure_country],S.[organization_sales_tax_number],S.[country_sales_tax_number],S.[reference_code],S.[assignment],S.[tax_amount],S.[logical_system],S.[cancelled_flag],S.[exchange_rate_date],S.[payment_reference],S.[tenant_code],S.[company_code],N'{}',N'',S.[updated_by],@now,S.[updated_by],@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END)
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[billing_document_code]
INTO #hdr_delta (rn, oper_type, id, [billing_document_code]);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[billing_document_code]
INTO #hdr_soft ([id], [billing_document_code])
FROM [takt_logistics_sales_invoice] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1 FROM #hdr S
    WHERE S.[billing_document_code] = LTRIM(RTRIM(T.[billing_document_code]))
  );

DECLARE @hdr_delete INT = @@ROWCOUNT;

INSERT INTO #item
SELECT
  S.rn,
  @base_id + 1000000000 + S.rn,
  0,
  S.[plant_code],
  S.[billing_document_code],
  S.[line_number],
  S.[billing_quantity],
  S.[sales_unit],
  S.[numerator],
  S.[denominator],
  S.[base_unit],
  S.[scale_quantity],
  S.[billing_quantity_sku],
  S.[required_quantity],
  S.[net_weight],
  S.[gross_weight],
  S.[weight_unit],
  S.[volume],
  S.[volume_unit],
  S.[business_area_code],
  S.[pricing_date],
  S.[service_rendered_date],
  S.[pricing_exchange_rate],
  S.[net_amount],
  S.[origin_document_code],
  S.[origin_document_item],
  S.[reference_document_code],
  S.[reference_document_item],
  S.[reference_document_category],
  S.[sales_document_code],
  S.[sales_document_item],
  S.[sales_document_reference_flag],
  S.[material_code],
  S.[material_description],
  S.[material_group],
  S.[sales_item_category],
  S.[item_type],
  S.[product_hierarchy],
  S.[shipping_point],
  S.[replacement_part_flag],
  S.[division],
  S.[partner_item],
  S.[departure_country],
  S.[plant_region],
  S.[statistical_value_flag],
  S.[pricing_flag],
  S.[cash_discount_flag],
  S.[cash_discount_base],
  S.[cost_center_code],
  S.[sales_office],
  S.[division_for_order],
  S.[debit_credit_indicator],
  S.[posted_by],
  S.[valuation_type],
  S.[warehouse_code],
  S.[update_group],
  S.[cost_amount],
  S.[subtotal1],
  S.[subtotal2],
  S.[subtotal3],
  S.[subtotal4],
  S.[subtotal5],
  S.[subtotal6],
  S.[statistics_exchange_rate],
  S.[international_article_number],
  S.[profit_center_code],
  S.[material_group4],
  S.[entered_material_code],
  S.[controlling_area_code],
  S.[profitability_segment],
  S.[credit_price],
  S.[credit_active_flag],
  S.[customer_group_sales_order],
  S.[destination_country_order],
  S.[manual_pricing_flag],
  S.[price_list_order],
  S.[region_order],
  S.[sales_organization_order],
  S.[distribution_channel_order],
  S.[document_category],
  S.[tax_amount],
  S.[order_reason],
  S.[payment_guarantee_form],
  S.[gross_amount],
  S.[exchange_rate_date],
  S.[is_obsolete],
  @tenant_code,
  @company_code,
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[billing_document_code], N.[line_number]) AS rn
  FROM (
    SELECT
      NULLIF(LEFT(LTRIM(RTRIM(R.[plant_code])), 4), N'') AS [plant_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N''), N'') AS [billing_document_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number],
      ROUND(TRY_CAST(R.[billing_quantity] AS DECIMAL(18,3)), 3) AS [billing_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_unit])), 3), N'') AS [sales_unit],
      ROUND(TRY_CAST(R.[numerator] AS DECIMAL(18,0)), 0) AS [numerator],
      ROUND(TRY_CAST(R.[denominator] AS DECIMAL(18,0)), 0) AS [denominator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[base_unit])), 3), N'') AS [base_unit],
      ROUND(TRY_CAST(R.[scale_quantity] AS DECIMAL(18,3)), 3) AS [scale_quantity],
      ROUND(TRY_CAST(R.[billing_quantity_sku] AS DECIMAL(18,3)), 3) AS [billing_quantity_sku],
      ROUND(TRY_CAST(R.[required_quantity] AS DECIMAL(18,3)), 3) AS [required_quantity],
      ROUND(TRY_CAST(R.[net_weight] AS DECIMAL(18,3)), 3) AS [net_weight],
      ROUND(TRY_CAST(R.[gross_weight] AS DECIMAL(18,3)), 3) AS [gross_weight],
      NULLIF(LEFT(LTRIM(RTRIM(R.[weight_unit])), 3), N'') AS [weight_unit],
      ROUND(TRY_CAST(R.[volume] AS DECIMAL(18,3)), 3) AS [volume],
      NULLIF(LEFT(LTRIM(RTRIM(R.[volume_unit])), 3), N'') AS [volume_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[business_area_code])), 4), N'') AS [business_area_code],
      TRY_CAST(R.[pricing_date] AS DATETIME) AS [pricing_date],
      TRY_CAST(R.[service_rendered_date] AS DATETIME) AS [service_rendered_date],
      ROUND(TRY_CAST(R.[pricing_exchange_rate] AS DECIMAL(18,5)), 5) AS [pricing_exchange_rate],
      ROUND(TRY_CAST(R.[net_amount] AS DECIMAL(18,2)), 2) AS [net_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[origin_document_code])), 10), N'') AS [origin_document_code],
      TRY_CAST(R.[origin_document_item] AS INT) AS [origin_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_code])), 10), N'') AS [reference_document_code],
      TRY_CAST(R.[reference_document_item] AS INT) AS [reference_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_category])), 1), N'') AS [reference_document_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_document_code])), 20), N'') AS [sales_document_code],
      TRY_CAST(R.[sales_document_item] AS INT) AS [sales_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_document_reference_flag])), 1), N'') AS [sales_document_reference_flag],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N''), N'') AS [material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_description])), 40), N'') AS [material_description],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_group])), 9), N'') AS [material_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_item_category])), 4), N'') AS [sales_item_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[item_type])), 1), N'') AS [item_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[product_hierarchy])), 18), N'') AS [product_hierarchy],
      NULLIF(LEFT(LTRIM(RTRIM(R.[shipping_point])), 4), N'') AS [shipping_point],
      NULLIF(LEFT(LTRIM(RTRIM(R.[replacement_part_flag])), 1), N'') AS [replacement_part_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[division])), 2), N'') AS [division],
      TRY_CAST(R.[partner_item] AS INT) AS [partner_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[departure_country])), 3), N'') AS [departure_country],
      NULLIF(LEFT(LTRIM(RTRIM(R.[plant_region])), 3), N'') AS [plant_region],
      NULLIF(LEFT(LTRIM(RTRIM(R.[statistical_value_flag])), 1), N'') AS [statistical_value_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[pricing_flag])), 1), N'') AS [pricing_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cash_discount_flag])), 1), N'') AS [cash_discount_flag],
      ROUND(TRY_CAST(R.[cash_discount_base] AS DECIMAL(18,2)), 2) AS [cash_discount_base],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cost_center_code])), 10), N'') AS [cost_center_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_office])), 4), N'') AS [sales_office],
      NULLIF(LEFT(LTRIM(RTRIM(R.[division_for_order])), 2), N'') AS [division_for_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[debit_credit_indicator])), 1), N'') AS [debit_credit_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[posted_by])), 12), N'') AS [posted_by],
      NULLIF(LEFT(LTRIM(RTRIM(R.[valuation_type])), 10), N'') AS [valuation_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[warehouse_code])), 4), N'') AS [warehouse_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[update_group])), 6), N'') AS [update_group],
      ROUND(TRY_CAST(R.[cost_amount] AS DECIMAL(18,2)), 2) AS [cost_amount],
      ROUND(TRY_CAST(R.[subtotal1] AS DECIMAL(18,2)), 2) AS [subtotal1],
      ROUND(TRY_CAST(R.[subtotal2] AS DECIMAL(18,2)), 2) AS [subtotal2],
      ROUND(TRY_CAST(R.[subtotal3] AS DECIMAL(18,2)), 2) AS [subtotal3],
      ROUND(TRY_CAST(R.[subtotal4] AS DECIMAL(18,2)), 2) AS [subtotal4],
      ROUND(TRY_CAST(R.[subtotal5] AS DECIMAL(18,2)), 2) AS [subtotal5],
      ROUND(TRY_CAST(R.[subtotal6] AS DECIMAL(18,2)), 2) AS [subtotal6],
      ROUND(TRY_CAST(R.[statistics_exchange_rate] AS DECIMAL(18,5)), 5) AS [statistics_exchange_rate],
      NULLIF(LEFT(LTRIM(RTRIM(R.[international_article_number])), 18), N'') AS [international_article_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[profit_center_code])), 10), N'') AS [profit_center_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_group4])), 3), N'') AS [material_group4],
      NULLIF(LEFT(LTRIM(RTRIM(R.[entered_material_code])), 20), N'') AS [entered_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[controlling_area_code])), 4), N'') AS [controlling_area_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[profitability_segment])), 10), N'') AS [profitability_segment],
      ROUND(TRY_CAST(R.[credit_price] AS DECIMAL(18,2)), 2) AS [credit_price],
      NULLIF(LEFT(LTRIM(RTRIM(R.[credit_active_flag])), 1), N'') AS [credit_active_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[customer_group_sales_order])), 2), N'') AS [customer_group_sales_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[destination_country_order])), 3), N'') AS [destination_country_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[manual_pricing_flag])), 1), N'') AS [manual_pricing_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[price_list_order])), 2), N'') AS [price_list_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[region_order])), 3), N'') AS [region_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_organization_order])), 4), N'') AS [sales_organization_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[distribution_channel_order])), 2), N'') AS [distribution_channel_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_category])), 1), N'') AS [document_category],
      ROUND(TRY_CAST(R.[tax_amount] AS DECIMAL(18,2)), 2) AS [tax_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[order_reason])), 3), N'') AS [order_reason],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payment_guarantee_form])), 2), N'') AS [payment_guarantee_form],
      ROUND(TRY_CAST(R.[gross_amount] AS DECIMAL(18,2)), 2) AS [gross_amount],
      TRY_CAST(R.[exchange_rate_date] AS DATETIME) AS [exchange_rate_date],
      ISNULL(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[billing_document_code])),
          COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
        ORDER BY COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice_item] R
    WHERE LTRIM(RTRIM(ISNULL(R.[billing_document_code], N''))) <> N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0) > 0
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

UPDATE I
SET I.[sales_invoice_id] = H.[id],
    I.[id] = COALESCE(T.[id], I.[id])
FROM #item I
INNER JOIN #hdr H
  ON H.[billing_document_code] = I.[billing_document_code]
LEFT JOIN [takt_logistics_sales_invoice_item] T
  ON T.[tenant_code] = @tenant_code
 AND T.[company_code] = @company_code
 AND T.[sales_invoice_id] = H.[id]
 AND T.[line_number] = I.[line_number];

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item WHERE [sales_invoice_id] <> 0);
DECLARE @item_sap_keys INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(R.[billing_document_code])) AS [billing_document_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice_item] R
    INNER JOIN #hdr H
      ON LTRIM(RTRIM(H.[billing_document_code])) = LTRIM(RTRIM(R.[billing_document_code]))
    WHERE LTRIM(RTRIM(ISNULL(R.[billing_document_code], N''))) <> N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0) > 0
    GROUP BY
      LTRIM(RTRIM(R.[billing_document_code])),
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
  ) K
);
IF @item_source <> @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(
    N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;

DELETE FROM #item WHERE [sales_invoice_id] = 0;

DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice_item]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_sales_invoice_item] AS T
USING #item AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND T.[sales_invoice_id] = S.[sales_invoice_id]
AND T.[line_number] = S.[line_number]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR EXISTS (
    SELECT
      S.[billing_quantity],
      S.[sales_unit],
      S.[numerator],
      S.[denominator],
      S.[base_unit],
      S.[scale_quantity],
      S.[billing_quantity_sku],
      S.[required_quantity],
      S.[net_weight],
      S.[gross_weight],
      S.[weight_unit],
      S.[volume],
      S.[volume_unit],
      S.[business_area_code],
      S.[pricing_date],
      S.[service_rendered_date],
      S.[pricing_exchange_rate],
      S.[net_amount],
      S.[origin_document_code],
      S.[origin_document_item],
      S.[reference_document_code],
      S.[reference_document_item],
      S.[reference_document_category],
      S.[sales_document_code],
      S.[sales_document_item],
      S.[sales_document_reference_flag],
      S.[material_code],
      S.[material_description],
      S.[material_group],
      S.[sales_item_category],
      S.[item_type],
      S.[product_hierarchy],
      S.[shipping_point],
      S.[replacement_part_flag],
      S.[division],
      S.[partner_item],
      S.[plant_code],
      S.[departure_country],
      S.[plant_region],
      S.[statistical_value_flag],
      S.[pricing_flag],
      S.[cash_discount_flag],
      S.[cash_discount_base],
      S.[cost_center_code],
      S.[sales_office],
      S.[division_for_order],
      S.[debit_credit_indicator],
      S.[posted_by],
      S.[valuation_type],
      S.[warehouse_code],
      S.[update_group],
      S.[cost_amount],
      S.[subtotal1],
      S.[subtotal2],
      S.[subtotal3],
      S.[subtotal4],
      S.[subtotal5],
      S.[subtotal6],
      S.[statistics_exchange_rate],
      S.[international_article_number],
      S.[profit_center_code],
      S.[material_group4],
      S.[entered_material_code],
      S.[controlling_area_code],
      S.[profitability_segment],
      S.[credit_price],
      S.[credit_active_flag],
      S.[customer_group_sales_order],
      S.[destination_country_order],
      S.[manual_pricing_flag],
      S.[price_list_order],
      S.[region_order],
      S.[sales_organization_order],
      S.[distribution_channel_order],
      S.[document_category],
      S.[tax_amount],
      S.[order_reason],
      S.[payment_guarantee_form],
      S.[gross_amount],
      S.[exchange_rate_date],
      S.[is_obsolete]
    EXCEPT
    SELECT
      T.[billing_quantity],
      T.[sales_unit],
      T.[numerator],
      T.[denominator],
      T.[base_unit],
      T.[scale_quantity],
      T.[billing_quantity_sku],
      T.[required_quantity],
      T.[net_weight],
      T.[gross_weight],
      T.[weight_unit],
      T.[volume],
      T.[volume_unit],
      T.[business_area_code],
      T.[pricing_date],
      T.[service_rendered_date],
      T.[pricing_exchange_rate],
      T.[net_amount],
      T.[origin_document_code],
      T.[origin_document_item],
      T.[reference_document_code],
      T.[reference_document_item],
      T.[reference_document_category],
      T.[sales_document_code],
      T.[sales_document_item],
      T.[sales_document_reference_flag],
      T.[material_code],
      T.[material_description],
      T.[material_group],
      T.[sales_item_category],
      T.[item_type],
      T.[product_hierarchy],
      T.[shipping_point],
      T.[replacement_part_flag],
      T.[division],
      T.[partner_item],
      T.[plant_code],
      T.[departure_country],
      T.[plant_region],
      T.[statistical_value_flag],
      T.[pricing_flag],
      T.[cash_discount_flag],
      T.[cash_discount_base],
      T.[cost_center_code],
      T.[sales_office],
      T.[division_for_order],
      T.[debit_credit_indicator],
      T.[posted_by],
      T.[valuation_type],
      T.[warehouse_code],
      T.[update_group],
      T.[cost_amount],
      T.[subtotal1],
      T.[subtotal2],
      T.[subtotal3],
      T.[subtotal4],
      T.[subtotal5],
      T.[subtotal6],
      T.[statistics_exchange_rate],
      T.[international_article_number],
      T.[profit_center_code],
      T.[material_group4],
      T.[entered_material_code],
      T.[controlling_area_code],
      T.[profitability_segment],
      T.[credit_price],
      T.[credit_active_flag],
      T.[customer_group_sales_order],
      T.[destination_country_order],
      T.[manual_pricing_flag],
      T.[price_list_order],
      T.[region_order],
      T.[sales_organization_order],
      T.[distribution_channel_order],
      T.[document_category],
      T.[tax_amount],
      T.[order_reason],
      T.[payment_guarantee_form],
      T.[gross_amount],
      T.[exchange_rate_date],
      T.[is_obsolete]
  )
) THEN
  UPDATE SET
    T.[billing_document_code] = S.[billing_document_code],
    T.[billing_quantity] = S.[billing_quantity],
    T.[sales_unit] = S.[sales_unit],
    T.[numerator] = S.[numerator],
    T.[denominator] = S.[denominator],
    T.[base_unit] = S.[base_unit],
    T.[scale_quantity] = S.[scale_quantity],
    T.[billing_quantity_sku] = S.[billing_quantity_sku],
    T.[required_quantity] = S.[required_quantity],
    T.[net_weight] = S.[net_weight],
    T.[gross_weight] = S.[gross_weight],
    T.[weight_unit] = S.[weight_unit],
    T.[volume] = S.[volume],
    T.[volume_unit] = S.[volume_unit],
    T.[business_area_code] = S.[business_area_code],
    T.[pricing_date] = S.[pricing_date],
    T.[service_rendered_date] = S.[service_rendered_date],
    T.[pricing_exchange_rate] = S.[pricing_exchange_rate],
    T.[net_amount] = S.[net_amount],
    T.[origin_document_code] = S.[origin_document_code],
    T.[origin_document_item] = S.[origin_document_item],
    T.[reference_document_code] = S.[reference_document_code],
    T.[reference_document_item] = S.[reference_document_item],
    T.[reference_document_category] = S.[reference_document_category],
    T.[sales_document_code] = S.[sales_document_code],
    T.[sales_document_item] = S.[sales_document_item],
    T.[sales_document_reference_flag] = S.[sales_document_reference_flag],
    T.[material_code] = S.[material_code],
    T.[material_description] = S.[material_description],
    T.[material_group] = S.[material_group],
    T.[sales_item_category] = S.[sales_item_category],
    T.[item_type] = S.[item_type],
    T.[product_hierarchy] = S.[product_hierarchy],
    T.[shipping_point] = S.[shipping_point],
    T.[replacement_part_flag] = S.[replacement_part_flag],
    T.[division] = S.[division],
    T.[partner_item] = S.[partner_item],
    T.[plant_code] = S.[plant_code],
    T.[departure_country] = S.[departure_country],
    T.[plant_region] = S.[plant_region],
    T.[statistical_value_flag] = S.[statistical_value_flag],
    T.[pricing_flag] = S.[pricing_flag],
    T.[cash_discount_flag] = S.[cash_discount_flag],
    T.[cash_discount_base] = S.[cash_discount_base],
    T.[cost_center_code] = S.[cost_center_code],
    T.[sales_office] = S.[sales_office],
    T.[division_for_order] = S.[division_for_order],
    T.[debit_credit_indicator] = S.[debit_credit_indicator],
    T.[posted_by] = S.[posted_by],
    T.[valuation_type] = S.[valuation_type],
    T.[warehouse_code] = S.[warehouse_code],
    T.[update_group] = S.[update_group],
    T.[cost_amount] = S.[cost_amount],
    T.[subtotal1] = S.[subtotal1],
    T.[subtotal2] = S.[subtotal2],
    T.[subtotal3] = S.[subtotal3],
    T.[subtotal4] = S.[subtotal4],
    T.[subtotal5] = S.[subtotal5],
    T.[subtotal6] = S.[subtotal6],
    T.[statistics_exchange_rate] = S.[statistics_exchange_rate],
    T.[international_article_number] = S.[international_article_number],
    T.[profit_center_code] = S.[profit_center_code],
    T.[material_group4] = S.[material_group4],
    T.[entered_material_code] = S.[entered_material_code],
    T.[controlling_area_code] = S.[controlling_area_code],
    T.[profitability_segment] = S.[profitability_segment],
    T.[credit_price] = S.[credit_price],
    T.[credit_active_flag] = S.[credit_active_flag],
    T.[customer_group_sales_order] = S.[customer_group_sales_order],
    T.[destination_country_order] = S.[destination_country_order],
    T.[manual_pricing_flag] = S.[manual_pricing_flag],
    T.[price_list_order] = S.[price_list_order],
    T.[region_order] = S.[region_order],
    T.[sales_organization_order] = S.[sales_organization_order],
    T.[distribution_channel_order] = S.[distribution_channel_order],
    T.[document_category] = S.[document_category],
    T.[tax_amount] = S.[tax_amount],
    T.[order_reason] = S.[order_reason],
    T.[payment_guarantee_form] = S.[payment_guarantee_form],
    T.[gross_amount] = S.[gross_amount],
    T.[exchange_rate_date] = S.[exchange_rate_date],
    T.[is_obsolete] = S.[is_obsolete],
        T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT ([id],[sales_invoice_id],[plant_code],[billing_document_code],[line_number],[billing_quantity],[sales_unit],[numerator],[denominator],[base_unit],[scale_quantity],[billing_quantity_sku],[required_quantity],[net_weight],[gross_weight],[weight_unit],[volume],[volume_unit],[business_area_code],[pricing_date],[service_rendered_date],[pricing_exchange_rate],[net_amount],[origin_document_code],[origin_document_item],[reference_document_code],[reference_document_item],[reference_document_category],[sales_document_code],[sales_document_item],[sales_document_reference_flag],[material_code],[material_description],[material_group],[sales_item_category],[item_type],[product_hierarchy],[shipping_point],[replacement_part_flag],[division],[partner_item],[departure_country],[plant_region],[statistical_value_flag],[pricing_flag],[cash_discount_flag],[cash_discount_base],[cost_center_code],[sales_office],[division_for_order],[debit_credit_indicator],[posted_by],[valuation_type],[warehouse_code],[update_group],[cost_amount],[subtotal1],[subtotal2],[subtotal3],[subtotal4],[subtotal5],[subtotal6],[statistics_exchange_rate],[international_article_number],[profit_center_code],[material_group4],[entered_material_code],[controlling_area_code],[profitability_segment],[credit_price],[credit_active_flag],[customer_group_sales_order],[destination_country_order],[manual_pricing_flag],[price_list_order],[region_order],[sales_organization_order],[distribution_channel_order],[document_category],[tax_amount],[order_reason],[payment_guarantee_form],[gross_amount],[exchange_rate_date],[is_obsolete],[tenant_code],[company_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[sales_invoice_id],S.[plant_code],S.[billing_document_code],S.[line_number],S.[billing_quantity],S.[sales_unit],S.[numerator],S.[denominator],S.[base_unit],S.[scale_quantity],S.[billing_quantity_sku],S.[required_quantity],S.[net_weight],S.[gross_weight],S.[weight_unit],S.[volume],S.[volume_unit],S.[business_area_code],S.[pricing_date],S.[service_rendered_date],S.[pricing_exchange_rate],S.[net_amount],S.[origin_document_code],S.[origin_document_item],S.[reference_document_code],S.[reference_document_item],S.[reference_document_category],S.[sales_document_code],S.[sales_document_item],S.[sales_document_reference_flag],S.[material_code],S.[material_description],S.[material_group],S.[sales_item_category],S.[item_type],S.[product_hierarchy],S.[shipping_point],S.[replacement_part_flag],S.[division],S.[partner_item],S.[departure_country],S.[plant_region],S.[statistical_value_flag],S.[pricing_flag],S.[cash_discount_flag],S.[cash_discount_base],S.[cost_center_code],S.[sales_office],S.[division_for_order],S.[debit_credit_indicator],S.[posted_by],S.[valuation_type],S.[warehouse_code],S.[update_group],S.[cost_amount],S.[subtotal1],S.[subtotal2],S.[subtotal3],S.[subtotal4],S.[subtotal5],S.[subtotal6],S.[statistics_exchange_rate],S.[international_article_number],S.[profit_center_code],S.[material_group4],S.[entered_material_code],S.[controlling_area_code],S.[profitability_segment],S.[credit_price],S.[credit_active_flag],S.[customer_group_sales_order],S.[destination_country_order],S.[manual_pricing_flag],S.[price_list_order],S.[region_order],S.[sales_organization_order],S.[distribution_channel_order],S.[document_category],S.[tax_amount],S.[order_reason],S.[payment_guarantee_form],S.[gross_amount],S.[exchange_rate_date],S.[is_obsolete],S.[tenant_code],S.[company_code],N'{}',N'',S.[updated_by],@now,S.[updated_by],@now,S.[is_deleted],CASE WHEN S.[is_deleted]=1 THEN S.[updated_by] ELSE NULL END,CASE WHEN S.[is_deleted]=1 THEN @now ELSE NULL END)
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[billing_document_code], INSERTED.[line_number]
INTO #item_delta (rn, oper_type, id, [billing_document_code], [line_number]);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[billing_document_code], INSERTED.[line_number]
INTO #item_soft ([id], [billing_document_code], [line_number])
FROM [takt_logistics_sales_invoice_item] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1 FROM #item S
    WHERE S.[sales_invoice_id] = T.[sales_invoice_id]
      AND S.[line_number] = T.[line_number]
  );

DECLARE @item_delete INT = @@ROWCOUNT;

DECLARE @hdr_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);
DECLARE @item_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice_item]
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
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([billing_document_code], N''))
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
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([billing_document_code], N''), N'/', CAST([line_number] AS NVARCHAR(20)))
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

