SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

IF OBJECT_ID('tempdb..#source_main') IS NOT NULL DROP TABLE #source_main;
IF OBJECT_ID('tempdb..#source_detail') IS NOT NULL DROP TABLE #source_detail;
IF OBJECT_ID('tempdb..#main_delta') IS NOT NULL DROP TABLE #main_delta;
IF OBJECT_ID('tempdb..#detail_delta') IS NOT NULL DROP TABLE #detail_delta;

CREATE TABLE #source_main (
  [rn] INT,
  [id] BIGINT,
  [source_ec_code] NVARCHAR(100),
  [source_model] NVARCHAR(100),
  [source_title] NVARCHAR(200),
  [source_status] NVARCHAR(100),
  [source_issue_date] DATE,
  [source_tcj_owner] NVARCHAR(MAX),
  [source_tcj_dependency] NVARCHAR(MAX),
  [source_ec_meeting] NVARCHAR(MAX),
  [source_pp_code] NVARCHAR(MAX),
  [source_technical_notice_code] NVARCHAR(MAX),
  [source_implementation] NVARCHAR(MAX),
  [source_main_change_reason] NVARCHAR(MAX),
  [source_secondary_change_reason] NVARCHAR(MAX),
  [source_safety_regulation] NVARCHAR(MAX),
  [source_progress_status] NVARCHAR(MAX),
  [source_serial_number_control] NVARCHAR(MAX),
  [source_customer_approval] NVARCHAR(MAX),
  [source_service_manual_revision] NVARCHAR(MAX),
  [source_user_manual_revision] NVARCHAR(MAX),
  [source_promotion_manual_revision] NVARCHAR(MAX),
  [source_standard_document_revision] NVARCHAR(MAX),
  [source_information_release] NVARCHAR(MAX),
  [source_cost_change] NVARCHAR(MAX),
  [source_unit_cost] DECIMAL(18,2),
  [source_mold_modification_cost] DECIMAL(18,2),
  [source_related_drawing] NVARCHAR(MAX),
  [source_ec_content] NVARCHAR(MAX)
);

CREATE TABLE #source_detail (
  [rn] INT,
  [id] BIGINT,
  [source_ec_code] NVARCHAR(100),
  [source_legacy_part_code] NVARCHAR(100),
  [source_finished_product] NVARCHAR(500),
  [source_parent_part] NVARCHAR(500),
  [source_legacy_part_name] NVARCHAR(MAX),
  [source_legacy_usage] NVARCHAR(MAX),
  [source_legacy_mounting_position] NVARCHAR(MAX),
  [source_replacement_part_code] NVARCHAR(MAX),
  [source_replacement_part_name] NVARCHAR(MAX),
  [source_replacement_usage] NVARCHAR(MAX),
  [source_replacement_mounting_position] NVARCHAR(MAX),
  [source_bom_code] NVARCHAR(MAX),
  [source_compatibility] NVARCHAR(MAX),
  [source_distinction] NVARCHAR(MAX),
  [source_instruction] NVARCHAR(MAX),
  [source_legacy_part_disposition] NVARCHAR(MAX),
  [source_bom_effective_date] DATE
);

CREATE TABLE #main_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  source_ec_code NVARCHAR(100)
);

CREATE TABLE #detail_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  source_ec_id BIGINT,
  source_legacy_part_code NVARCHAR(100)
);

-- 主表源：PP_SapEcn 原样全量（空设变号除外）
INSERT INTO #source_main
SELECT
  S.rn,
  @base_id + S.rn,
  S.source_ec_code,
  ISNULL(S.[D_SAP_ZPABD_Z002], ''),
  ISNULL(S.[D_SAP_ZPABD_Z003], ''),
  ISNULL(S.[D_SAP_ZPABD_Z004], ''),
  COALESCE(TRY_CONVERT(DATE, NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPABD_Z005])), ''), 23), CAST('1900-01-01' AS DATE)),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z006] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z007] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z008] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z009] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z010] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z011] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z012] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z013] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z014] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z015] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z016] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z017] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z018] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z019] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z020] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z021] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z022] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z023] AS NVARCHAR(MAX)), ''),
  ISNULL(TRY_CAST(S.[D_SAP_ZPABD_Z024] AS DECIMAL(18,2)), 0),
  ISNULL(TRY_CAST(S.[D_SAP_ZPABD_Z025] AS DECIMAL(18,2)), 0),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z026] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z027] AS NVARCHAR(MAX)), N'')
FROM (
  SELECT *,
    LTRIM(RTRIM([D_SAP_ZPABD_Z001])) AS source_ec_code,
    ROW_NUMBER() OVER (ORDER BY LTRIM(RTRIM([D_SAP_ZPABD_Z001]))) AS rn
  FROM [Sap_Data].[dbo].[PP_SapEcn]
  WHERE LTRIM(RTRIM([D_SAP_ZPABD_Z001])) <> ''
) S;

DECLARE @main_source_count INT = (SELECT COUNT(*) FROM #source_main);
DECLARE @main_sap_raw_count INT = (
  SELECT COUNT(*)
  FROM [Sap_Data].[dbo].[PP_SapEcn]
  WHERE LTRIM(RTRIM([D_SAP_ZPABD_Z001])) <> ''
);

IF @main_source_count <> @main_sap_raw_count
BEGIN
  DECLARE @main_src_msg NVARCHAR(200) = CONCAT(
    N'主表源行数与装入不一致: source=', @main_sap_raw_count, N', loaded=', @main_source_count);
  THROW 50003, @main_src_msg, 1;
END;

IF EXISTS (
  SELECT 1 FROM #source_main GROUP BY [source_ec_code] HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @main_dup NVARCHAR(400);
  SELECT TOP 1 @main_dup = CONCAT([source_ec_code], N' x', COUNT(*))
  FROM #source_main GROUP BY [source_ec_code] HAVING COUNT(*) > 1;
  THROW 50001, @main_dup, 1;
END;

-- 子表源：仅按设变号关联主表，原样全量装入（禁止按旧件料号去重）
INSERT INTO #source_detail
SELECT
  S.rn,
  @base_id + 1000000000 + S.rn,
  S.source_ec_code,
  S.source_legacy_part_code,
  S.source_finished_product,
  S.source_parent_part,
  S.source_legacy_part_name,
  S.source_legacy_usage,
  S.source_legacy_mounting_position,
  S.source_replacement_part_code,
  S.source_replacement_part_name,
  S.source_replacement_usage,
  S.source_replacement_mounting_position,
  S.source_bom_code,
  S.source_compatibility,
  S.source_distinction,
  S.source_instruction,
  S.source_legacy_part_disposition,
  S.source_bom_effective_date
FROM (
  SELECT
    LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001])) AS source_ec_code,
    ISNULL(Sub.[D_SAP_ZPABD_S004], N'') AS source_legacy_part_code,
    ISNULL(Sub.[D_SAP_ZPABD_S002], N'') AS source_finished_product,
    ISNULL(Sub.[D_SAP_ZPABD_S003], N'') AS source_parent_part,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S005] AS NVARCHAR(MAX)), N'') AS source_legacy_part_name,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S006] AS NVARCHAR(MAX)), N'') AS source_legacy_usage,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S007] AS NVARCHAR(MAX)), N'') AS source_legacy_mounting_position,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S008] AS NVARCHAR(MAX)), N'') AS source_replacement_part_code,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S009] AS NVARCHAR(MAX)), N'') AS source_replacement_part_name,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S010] AS NVARCHAR(MAX)), N'') AS source_replacement_usage,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S011] AS NVARCHAR(MAX)), N'') AS source_replacement_mounting_position,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S012] AS NVARCHAR(MAX)), N'') AS source_bom_code,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S013] AS NVARCHAR(MAX)), N'') AS source_compatibility,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S014] AS NVARCHAR(MAX)), N'') AS source_distinction,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S015] AS NVARCHAR(MAX)), N'') AS source_instruction,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S016] AS NVARCHAR(MAX)), N'') AS source_legacy_part_disposition,
    TRY_CONVERT(DATE, NULLIF(LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S017])), N''), 23) AS source_bom_effective_date,
    ROW_NUMBER() OVER (
      ORDER BY
        LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001])),
        ISNULL(Sub.[D_SAP_ZPABD_S002], N''),
        ISNULL(Sub.[D_SAP_ZPABD_S003], N''),
        ISNULL(Sub.[D_SAP_ZPABD_S004], N''),
        ISNULL(Sub.[D_SAP_ZPABD_S008], N''),
        ISNULL(Sub.[D_SAP_ZPABD_S012], N'')
    ) AS rn
  FROM [Sap_Data].[dbo].[PP_SapEcnSub] Sub
  INNER JOIN #source_main M
    ON M.[source_ec_code] = LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001]))
  WHERE LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001])) <> N''
) S;

DECLARE @detail_source_count INT = (SELECT COUNT(*) FROM #source_detail);
DECLARE @detail_sap_raw_count INT = (
  SELECT COUNT(*)
  FROM [Sap_Data].[dbo].[PP_SapEcnSub] Sub
  WHERE LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001])) <> ''
    AND EXISTS (
      SELECT 1
      FROM [Sap_Data].[dbo].[PP_SapEcn] E
      WHERE LTRIM(RTRIM(E.[D_SAP_ZPABD_Z001])) <> ''
        AND LTRIM(RTRIM(E.[D_SAP_ZPABD_Z001])) = LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001]))
    )
);

IF @detail_source_count <> @detail_sap_raw_count
BEGIN
  DECLARE @detail_src_msg NVARCHAR(200) = CONCAT(
    N'子表源行数与装入不一致: source=', @detail_sap_raw_count, N', loaded=', @detail_source_count);
  THROW 50003, @detail_src_msg, 1;
END;

DECLARE @main_target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);

-- 主表：存在则更新（有变化或恢复软删），不存在则插入；唯一键 Tenant+Company+SourceEcNo
MERGE INTO [takt_logistics_manufacturing_ec_source] AS T
USING #source_main AS S
ON T.[tenant_code] = @tenant_code
AND T.[company_code] = @company_code
AND LTRIM(RTRIM(T.[source_ec_code])) = S.[source_ec_code]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR LTRIM(RTRIM(ISNULL(T.[source_model], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_model], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_title], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_title], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_status], N'')))
  OR ISNULL(T.[source_issue_date], '1900-01-01') <> ISNULL(S.[source_issue_date], '1900-01-01')
  OR LTRIM(RTRIM(ISNULL(T.[source_tcj_owner], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_tcj_owner], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_tcj_dependency], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_tcj_dependency], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_ec_meeting], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_ec_meeting], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_pp_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_pp_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_technical_notice_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_technical_notice_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_implementation], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_implementation], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_main_change_reason], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_main_change_reason], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_secondary_change_reason], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_secondary_change_reason], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_safety_regulation], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_safety_regulation], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_progress_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_progress_status], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_serial_number_control], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_serial_number_control], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_customer_approval], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_customer_approval], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_service_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_service_manual_revision], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_user_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_user_manual_revision], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_promotion_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_promotion_manual_revision], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_standard_document_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_standard_document_revision], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_information_release], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_information_release], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_cost_change], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_cost_change], N'')))
  OR ROUND(T.[source_unit_cost], 2) <> ROUND(S.[source_unit_cost], 2)
  OR ROUND(T.[source_mold_modification_cost], 2) <> ROUND(S.[source_mold_modification_cost], 2)
  OR LTRIM(RTRIM(ISNULL(T.[source_related_drawing], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_related_drawing], N'')))
  OR ISNULL(CAST(T.[source_ec_content] AS NVARCHAR(MAX)), N'') <> ISNULL(S.[source_ec_content], N'')
) THEN
  UPDATE SET
    T.[source_model] = S.[source_model],
    T.[source_title] = S.[source_title],
    T.[source_status] = S.[source_status],
    T.[source_issue_date] = S.[source_issue_date],
    T.[source_tcj_owner] = S.[source_tcj_owner],
    T.[source_tcj_dependency] = S.[source_tcj_dependency],
    T.[source_ec_meeting] = S.[source_ec_meeting],
    T.[source_pp_code] = S.[source_pp_code],
    T.[source_technical_notice_code] = S.[source_technical_notice_code],
    T.[source_implementation] = S.[source_implementation],
    T.[source_main_change_reason] = S.[source_main_change_reason],
    T.[source_secondary_change_reason] = S.[source_secondary_change_reason],
    T.[source_safety_regulation] = S.[source_safety_regulation],
    T.[source_progress_status] = S.[source_progress_status],
    T.[source_serial_number_control] = S.[source_serial_number_control],
    T.[source_customer_approval] = S.[source_customer_approval],
    T.[source_service_manual_revision] = S.[source_service_manual_revision],
    T.[source_user_manual_revision] = S.[source_user_manual_revision],
    T.[source_promotion_manual_revision] = S.[source_promotion_manual_revision],
    T.[source_standard_document_revision] = S.[source_standard_document_revision],
    T.[source_information_release] = S.[source_information_release],
    T.[source_cost_change] = S.[source_cost_change],
    T.[source_unit_cost] = S.[source_unit_cost],
    T.[source_mold_modification_cost] = S.[source_mold_modification_cost],
    T.[source_related_drawing] = S.[source_related_drawing],
    T.[source_ec_content] = S.[source_ec_content],
    T.[updated_by] = @sync_user_id,
    T.[updated_at] = @now,
    T.[culture_code] = @culture_code,
    T.[is_deleted] = 0,
    T.[deleted_by] = NULL,
    T.[deleted_at] = NULL
WHEN NOT MATCHED THEN
  INSERT (
    [id],[source_ec_code],[source_model],[source_title],[source_status],
    [source_issue_date],[source_tcj_owner],[source_tcj_dependency],
    [source_ec_meeting],[source_pp_code],[source_technical_notice_code],
    [source_implementation],[source_main_change_reason],
    [source_secondary_change_reason],[source_safety_regulation],
    [source_progress_status],[source_serial_number_control],
    [source_customer_approval],[source_service_manual_revision],
    [source_user_manual_revision],[source_promotion_manual_revision],
    [source_standard_document_revision],[source_information_release],
    [source_cost_change],[source_unit_cost],
    [source_mold_modification_cost],[source_related_drawing],
    [source_ec_content],[tenant_code],[company_code],[culture_code],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
  )
  VALUES (
    S.[id],S.[source_ec_code],S.[source_model],S.[source_title],S.[source_status],
    S.[source_issue_date],S.[source_tcj_owner],S.[source_tcj_dependency],
    S.[source_ec_meeting],S.[source_pp_code],S.[source_technical_notice_code],
    S.[source_implementation],S.[source_main_change_reason],
    S.[source_secondary_change_reason],S.[source_safety_regulation],
    S.[source_progress_status],S.[source_serial_number_control],
    S.[source_customer_approval],S.[source_service_manual_revision],
    S.[source_user_manual_revision],S.[source_promotion_manual_revision],
    S.[source_standard_document_revision],S.[source_information_release],
    S.[source_cost_change],S.[source_unit_cost],
    S.[source_mold_modification_cost],S.[source_related_drawing],
    S.[source_ec_content],@tenant_code,@company_code,@culture_code,
    @sync_user_id,@now,@sync_user_id,@now,0
  )
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[source_ec_code]
INTO #main_delta(rn, oper_type, id, source_ec_code);

-- 主表孤儿软删：目标有而源没有（仅 is_deleted=0）
IF OBJECT_ID('tempdb..#main_soft_deleted_rows') IS NOT NULL DROP TABLE #main_soft_deleted_rows;
CREATE TABLE #main_soft_deleted_rows (
  [id] BIGINT,
  [source_ec_code] NVARCHAR(100)
);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[source_ec_code]
INTO #main_soft_deleted_rows ([id], [source_ec_code])
FROM [takt_logistics_manufacturing_ec_source] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #source_main S
    WHERE S.[source_ec_code] = LTRIM(RTRIM(T.[source_ec_code]))
  );

DECLARE @main_delete_count INT = @@ROWCOUNT;
DECLARE @main_soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @main_soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([source_ec_code], N''))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #main_soft_deleted_rows;
SET @main_soft_deleted_keys = ISNULL(@main_soft_deleted_keys, N'');

DECLARE @main_target_count INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_ec_source]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 0
);
DECLARE @main_target_physical INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_ec_source]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code
);
DECLARE @main_soft_deleted INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_ec_source]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [is_deleted] = 1
);

DECLARE @detail_target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source_detail]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);

-- 子表：仅按设变号归属主表；原样全量装入后整批替换（不按旧件料号 MERGE）
IF OBJECT_ID('tempdb..#detail_soft_deleted_rows') IS NOT NULL DROP TABLE #detail_soft_deleted_rows;
CREATE TABLE #detail_soft_deleted_rows (
  [id] BIGINT,
  [source_ec_id] BIGINT,
  [source_legacy_part_code] NVARCHAR(100)
);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[source_ec_id], INSERTED.[source_legacy_part_code]
INTO #detail_soft_deleted_rows ([id], [source_ec_id], [source_legacy_part_code])
FROM [takt_logistics_manufacturing_ec_source_detail] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0;

DECLARE @detail_delete_count INT = @@ROWCOUNT;

INSERT INTO [takt_logistics_manufacturing_ec_source_detail] (
  [id],[source_ec_id],[source_finished_product],[source_parent_part],
  [source_legacy_part_code],[source_legacy_part_name],[source_legacy_usage],
  [source_legacy_mounting_position],[source_replacement_part_code],
  [source_replacement_part_name],[source_replacement_usage],
  [source_replacement_mounting_position],[source_bom_code],
  [source_compatibility],[source_distinction],
  [source_instruction],[source_legacy_part_disposition],
  [source_bom_effective_date],[tenant_code],[company_code],[culture_code],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
)
SELECT
  S.[id],
  M.[id],
  S.[source_finished_product],
  S.[source_parent_part],
  S.[source_legacy_part_code],
  S.[source_legacy_part_name],
  TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_legacy_usage] AS NVARCHAR(40)))), N'')),
  S.[source_legacy_mounting_position],
  S.[source_replacement_part_code],
  S.[source_replacement_part_name],
  TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_replacement_usage] AS NVARCHAR(40)))), N'')),
  S.[source_replacement_mounting_position],
  S.[source_bom_code],
  S.[source_compatibility],
  S.[source_distinction],
  S.[source_instruction],
  S.[source_legacy_part_disposition],
  S.[source_bom_effective_date],
  @tenant_code,
  @company_code,
  @culture_code,
  @sync_user_id,
  @now,
  @sync_user_id,
  @now,
  0
FROM #source_detail S
INNER JOIN [takt_logistics_manufacturing_ec_source] M
  ON M.[tenant_code] = @tenant_code
 AND M.[company_code] = @company_code
 AND M.[source_ec_code] = S.[source_ec_code]
 AND M.[is_deleted] = 0;

INSERT INTO #detail_delta(rn, oper_type, id, source_ec_id, source_legacy_part_code)
SELECT
  S.[rn],
  N'INSERT',
  S.[id],
  M.[id],
  S.[source_legacy_part_code]
FROM #source_detail S
INNER JOIN [takt_logistics_manufacturing_ec_source] M
  ON M.[tenant_code] = @tenant_code
 AND M.[company_code] = @company_code
 AND M.[source_ec_code] = S.[source_ec_code]
 AND M.[is_deleted] = 0;

DECLARE @detail_soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @detail_soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    CAST([source_ec_id] AS NVARCHAR(30)), N'/',
    ISNULL([source_legacy_part_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (
  SELECT TOP (200) [id], [source_ec_id], [source_legacy_part_code]
  FROM #detail_soft_deleted_rows
  ORDER BY [id]
) K;
SET @detail_soft_deleted_keys = ISNULL(@detail_soft_deleted_keys, N'');

DECLARE @detail_target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source_detail] T
  INNER JOIN [takt_logistics_manufacturing_ec_source] M ON M.[id] = T.[source_ec_id]
  WHERE T.[tenant_code] = @tenant_code
    AND T.[company_code] = @company_code
    AND T.[is_deleted] = 0
    AND M.[tenant_code] = @tenant_code
    AND M.[company_code] = @company_code
    AND M.[is_deleted] = 0
);
DECLARE @detail_target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source_detail]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
);
DECLARE @detail_soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source_detail]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 1
);

DECLARE @main_ins INT = (SELECT COUNT(*) FROM #main_delta WHERE oper_type = 'INSERT');
DECLARE @main_upd INT = (SELECT COUNT(*) FROM #main_delta WHERE oper_type = 'UPDATE');
DECLARE @main_unchanged_count INT = @main_source_count - @main_ins - @main_upd;
DECLARE @detail_ins INT = (SELECT COUNT(*) FROM #detail_delta WHERE oper_type = 'INSERT');
DECLARE @detail_upd INT = (SELECT COUNT(*) FROM #detail_delta WHERE oper_type = 'UPDATE');
DECLARE @detail_unchanged_count INT = @detail_source_count - @detail_ins - @detail_upd;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"main_sap_raw":' + CAST(@main_sap_raw_count AS NVARCHAR)
  + N',"main_source":' + CAST(@main_source_count AS NVARCHAR)
  + N',"main_target_before":' + CAST(@main_target_before AS NVARCHAR)
  + N',"main_target_after":' + CAST(@main_target_count AS NVARCHAR)
  + N',"main_target_physical":' + CAST(@main_target_physical AS NVARCHAR)
  + N',"main_soft_deleted":' + CAST(@main_soft_deleted AS NVARCHAR)
  + N',"main_insert":' + CAST(@main_ins AS NVARCHAR)
  + N',"main_update":' + CAST(@main_upd AS NVARCHAR)
  + N',"main_unchanged":' + CAST(@main_unchanged_count AS NVARCHAR)
  + N',"main_soft_delete_this_run":' + CAST(@main_delete_count AS NVARCHAR)
  + N',"main_soft_delete_keys":"' + REPLACE(@main_soft_deleted_keys, N'"', N'''') + N'"'
  + N',"detail_sap_raw":' + CAST(@detail_sap_raw_count AS NVARCHAR)
  + N',"detail_source":' + CAST(@detail_source_count AS NVARCHAR)
  + N',"detail_target_before":' + CAST(@detail_target_before AS NVARCHAR)
  + N',"detail_target_after":' + CAST(@detail_target_count AS NVARCHAR)
  + N',"detail_target_physical":' + CAST(@detail_target_physical AS NVARCHAR)
  + N',"detail_soft_deleted":' + CAST(@detail_soft_deleted AS NVARCHAR)
  + N',"detail_insert":' + CAST(@detail_ins AS NVARCHAR)
  + N',"detail_update":' + CAST(@detail_upd AS NVARCHAR)
  + N',"detail_unchanged":' + CAST(@detail_unchanged_count AS NVARCHAR)
  + N',"detail_soft_delete_this_run":' + CAST(@detail_delete_count AS NVARCHAR)
  + N',"detail_soft_delete_keys":"' + REPLACE(@detail_soft_deleted_keys, N'"', N'''') + N'"}';



INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC',N'SYNC',N'EC management',
  N'exec_sql_merge','SQL',N'/sync/ec','',
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@sync_user_id,@now
);

-- Quartz 执行器读取此结果集写入 ExecuteMessage / quartz-.log
SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'main' AS NVARCHAR(40)) AS [scope],
  @main_sap_raw_count AS [source_raw_count],
  @main_source_count AS [source_count],
  @main_target_before AS [target_before],
  @main_target_count AS [target_after],
  @main_target_physical AS [target_physical],
  @main_soft_deleted AS [soft_deleted],
  @main_ins AS [insert_count],
  @main_upd AS [update_count],
  @main_unchanged_count AS [unchanged_count],
  @main_delete_count AS [delete_count],
  @main_soft_deleted_keys AS [soft_deleted_keys]
UNION ALL
SELECT
  N'QUARTZ_SYNC_SUMMARY',
  CAST(N'detail' AS NVARCHAR(40)),
  @detail_sap_raw_count,
  @detail_source_count,
  @detail_target_before,
  @detail_target_count,
  @detail_target_physical,
  @detail_soft_deleted,
  @detail_ins,
  @detail_upd,
  @detail_unchanged_count,
  @detail_delete_count,
  @detail_soft_deleted_keys;
