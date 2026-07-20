/*
  Quartz 演示：字典类型只读统计（wwwroot/Quartz/demo_sql_dict_count.sql）
*/
SET NOCOUNT ON;
SELECT COUNT(*) AS dict_type_count
FROM takt_foundation_dict_type
WHERE is_deleted = 0;
