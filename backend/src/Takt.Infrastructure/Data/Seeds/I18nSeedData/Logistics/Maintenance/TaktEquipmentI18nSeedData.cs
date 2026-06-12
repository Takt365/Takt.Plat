// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance
// 文件名称：TaktEquipmentI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEquipment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance;

/// <summary>
/// TaktEquipment 实体国际化翻译种子（键前缀 entity.equipment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEquipmentI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktEquipment 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 equipment 实体翻译...", tenantCode);

        foreach (var item in GetEquipmentTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktEquipment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEquipment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.equipment._self / entity.equipment.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetEquipmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.equipment._self
            new TranslationSeedItem("entity.equipment._self", "en-US", "Equipment Information", "实体名称"),
            // entity.equipment._self
            new TranslationSeedItem("entity.equipment._self", "ja-JP", "Takt工厂设备信息", "实体名称"),
            // entity.equipment._self
            new TranslationSeedItem("entity.equipment._self", "zh-CN", "Takt工厂设备信息", "实体名称"),
            // entity.equipment._self
            new TranslationSeedItem("entity.equipment._self", "zh-HK", "Takt工厂设备信息", "实体名称"),

            // entity.equipment.plantcode
            new TranslationSeedItem("entity.equipment.plantcode", "en-US", "工厂代码", "工厂代码（不可空）"),
            // entity.equipment.plantcode
            new TranslationSeedItem("entity.equipment.plantcode", "ja-JP", "工厂代码", "工厂代码（不可空）"),
            // entity.equipment.plantcode
            new TranslationSeedItem("entity.equipment.plantcode", "zh-CN", "工厂代码", "工厂代码（不可空）"),
            // entity.equipment.plantcode
            new TranslationSeedItem("entity.equipment.plantcode", "zh-HK", "工厂代码", "工厂代码（不可空）"),

            // entity.equipment.code
            new TranslationSeedItem("entity.equipment.code", "en-US", "设备编码", "设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）"),
            // entity.equipment.code
            new TranslationSeedItem("entity.equipment.code", "ja-JP", "设备编码", "设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）"),
            // entity.equipment.code
            new TranslationSeedItem("entity.equipment.code", "zh-CN", "设备编码", "设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）"),
            // entity.equipment.code
            new TranslationSeedItem("entity.equipment.code", "zh-HK", "设备编码", "设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）"),

            // entity.equipment.name
            new TranslationSeedItem("entity.equipment.name", "en-US", "设备名称", "设备名称"),
            // entity.equipment.name
            new TranslationSeedItem("entity.equipment.name", "ja-JP", "设备名称", "设备名称"),
            // entity.equipment.name
            new TranslationSeedItem("entity.equipment.name", "zh-CN", "设备名称", "设备名称"),
            // entity.equipment.name
            new TranslationSeedItem("entity.equipment.name", "zh-HK", "设备名称", "设备名称"),

            // entity.equipment.type
            new TranslationSeedItem("entity.equipment.type", "en-US", "设备类型", "设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）"),
            // entity.equipment.type
            new TranslationSeedItem("entity.equipment.type", "ja-JP", "设备类型", "设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）"),
            // entity.equipment.type
            new TranslationSeedItem("entity.equipment.type", "zh-CN", "设备类型", "设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）"),
            // entity.equipment.type
            new TranslationSeedItem("entity.equipment.type", "zh-HK", "设备类型", "设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）"),

            // entity.equipment.model
            new TranslationSeedItem("entity.equipment.model", "en-US", "设备型号", "设备型号"),
            // entity.equipment.model
            new TranslationSeedItem("entity.equipment.model", "ja-JP", "设备型号", "设备型号"),
            // entity.equipment.model
            new TranslationSeedItem("entity.equipment.model", "zh-CN", "设备型号", "设备型号"),
            // entity.equipment.model
            new TranslationSeedItem("entity.equipment.model", "zh-HK", "设备型号", "设备型号"),

            // entity.equipment.specification
            new TranslationSeedItem("entity.equipment.specification", "en-US", "设备规格", "设备规格"),
            // entity.equipment.specification
            new TranslationSeedItem("entity.equipment.specification", "ja-JP", "设备规格", "设备规格"),
            // entity.equipment.specification
            new TranslationSeedItem("entity.equipment.specification", "zh-CN", "设备规格", "设备规格"),
            // entity.equipment.specification
            new TranslationSeedItem("entity.equipment.specification", "zh-HK", "设备规格", "设备规格"),

            // entity.equipment.brand
            new TranslationSeedItem("entity.equipment.brand", "en-US", "设备品牌", "设备品牌"),
            // entity.equipment.brand
            new TranslationSeedItem("entity.equipment.brand", "ja-JP", "设备品牌", "设备品牌"),
            // entity.equipment.brand
            new TranslationSeedItem("entity.equipment.brand", "zh-CN", "设备品牌", "设备品牌"),
            // entity.equipment.brand
            new TranslationSeedItem("entity.equipment.brand", "zh-HK", "设备品牌", "设备品牌"),

            // entity.equipment.manufacturer
            new TranslationSeedItem("entity.equipment.manufacturer", "en-US", "制造商", "制造商"),
            // entity.equipment.manufacturer
            new TranslationSeedItem("entity.equipment.manufacturer", "ja-JP", "制造商", "制造商"),
            // entity.equipment.manufacturer
            new TranslationSeedItem("entity.equipment.manufacturer", "zh-CN", "制造商", "制造商"),
            // entity.equipment.manufacturer
            new TranslationSeedItem("entity.equipment.manufacturer", "zh-HK", "制造商", "制造商"),

            // entity.equipment.dealerby
            new TranslationSeedItem("entity.equipment.dealerby", "en-US", "经销商", "经销商"),
            // entity.equipment.dealerby
            new TranslationSeedItem("entity.equipment.dealerby", "ja-JP", "经销商", "经销商"),
            // entity.equipment.dealerby
            new TranslationSeedItem("entity.equipment.dealerby", "zh-CN", "经销商", "经销商"),
            // entity.equipment.dealerby
            new TranslationSeedItem("entity.equipment.dealerby", "zh-HK", "经销商", "经销商"),

            // entity.equipment.serialnumber
            new TranslationSeedItem("entity.equipment.serialnumber", "en-US", "序列号", "序列号/出厂编号"),
            // entity.equipment.serialnumber
            new TranslationSeedItem("entity.equipment.serialnumber", "ja-JP", "序列号", "序列号/出厂编号"),
            // entity.equipment.serialnumber
            new TranslationSeedItem("entity.equipment.serialnumber", "zh-CN", "序列号", "序列号/出厂编号"),
            // entity.equipment.serialnumber
            new TranslationSeedItem("entity.equipment.serialnumber", "zh-HK", "序列号", "序列号/出厂编号"),

            // entity.equipment.workshopby
            new TranslationSeedItem("entity.equipment.workshopby", "en-US", "所属车间", "所属车间"),
            // entity.equipment.workshopby
            new TranslationSeedItem("entity.equipment.workshopby", "ja-JP", "所属车间", "所属车间"),
            // entity.equipment.workshopby
            new TranslationSeedItem("entity.equipment.workshopby", "zh-CN", "所属车间", "所属车间"),
            // entity.equipment.workshopby
            new TranslationSeedItem("entity.equipment.workshopby", "zh-HK", "所属车间", "所属车间"),

            // entity.equipment.productionlineby
            new TranslationSeedItem("entity.equipment.productionlineby", "en-US", "所属产线", "所属产线"),
            // entity.equipment.productionlineby
            new TranslationSeedItem("entity.equipment.productionlineby", "ja-JP", "所属产线", "所属产线"),
            // entity.equipment.productionlineby
            new TranslationSeedItem("entity.equipment.productionlineby", "zh-CN", "所属产线", "所属产线"),
            // entity.equipment.productionlineby
            new TranslationSeedItem("entity.equipment.productionlineby", "zh-HK", "所属产线", "所属产线"),

            // entity.equipment.workstationby
            new TranslationSeedItem("entity.equipment.workstationby", "en-US", "所属工位", "所属工位"),
            // entity.equipment.workstationby
            new TranslationSeedItem("entity.equipment.workstationby", "ja-JP", "所属工位", "所属工位"),
            // entity.equipment.workstationby
            new TranslationSeedItem("entity.equipment.workstationby", "zh-CN", "所属工位", "所属工位"),
            // entity.equipment.workstationby
            new TranslationSeedItem("entity.equipment.workstationby", "zh-HK", "所属工位", "所属工位"),

            // entity.equipment.deptby
            new TranslationSeedItem("entity.equipment.deptby", "en-US", "所属部门", "所属部门"),
            // entity.equipment.deptby
            new TranslationSeedItem("entity.equipment.deptby", "ja-JP", "所属部门", "所属部门"),
            // entity.equipment.deptby
            new TranslationSeedItem("entity.equipment.deptby", "zh-CN", "所属部门", "所属部门"),
            // entity.equipment.deptby
            new TranslationSeedItem("entity.equipment.deptby", "zh-HK", "所属部门", "所属部门"),

            // entity.equipment.location
            new TranslationSeedItem("entity.equipment.location", "en-US", "设备位置", "设备位置（详细位置描述）"),
            // entity.equipment.location
            new TranslationSeedItem("entity.equipment.location", "ja-JP", "设备位置", "设备位置（详细位置描述）"),
            // entity.equipment.location
            new TranslationSeedItem("entity.equipment.location", "zh-CN", "设备位置", "设备位置（详细位置描述）"),
            // entity.equipment.location
            new TranslationSeedItem("entity.equipment.location", "zh-HK", "设备位置", "设备位置（详细位置描述）"),

            // entity.equipment.responsibleuserby
            new TranslationSeedItem("entity.equipment.responsibleuserby", "en-US", "负责人", "负责人"),
            // entity.equipment.responsibleuserby
            new TranslationSeedItem("entity.equipment.responsibleuserby", "ja-JP", "负责人", "负责人"),
            // entity.equipment.responsibleuserby
            new TranslationSeedItem("entity.equipment.responsibleuserby", "zh-CN", "负责人", "负责人"),
            // entity.equipment.responsibleuserby
            new TranslationSeedItem("entity.equipment.responsibleuserby", "zh-HK", "负责人", "负责人"),

            // entity.equipment.operatorby
            new TranslationSeedItem("entity.equipment.operatorby", "en-US", "操作人", "操作人"),
            // entity.equipment.operatorby
            new TranslationSeedItem("entity.equipment.operatorby", "ja-JP", "操作人", "操作人"),
            // entity.equipment.operatorby
            new TranslationSeedItem("entity.equipment.operatorby", "zh-CN", "操作人", "操作人"),
            // entity.equipment.operatorby
            new TranslationSeedItem("entity.equipment.operatorby", "zh-HK", "操作人", "操作人"),

            // entity.equipment.purchasedate
            new TranslationSeedItem("entity.equipment.purchasedate", "en-US", "购买日期", "购买日期"),
            // entity.equipment.purchasedate
            new TranslationSeedItem("entity.equipment.purchasedate", "ja-JP", "购买日期", "购买日期"),
            // entity.equipment.purchasedate
            new TranslationSeedItem("entity.equipment.purchasedate", "zh-CN", "购买日期", "购买日期"),
            // entity.equipment.purchasedate
            new TranslationSeedItem("entity.equipment.purchasedate", "zh-HK", "购买日期", "购买日期"),

            // entity.equipment.installationdate
            new TranslationSeedItem("entity.equipment.installationdate", "en-US", "安装日期", "安装日期"),
            // entity.equipment.installationdate
            new TranslationSeedItem("entity.equipment.installationdate", "ja-JP", "安装日期", "安装日期"),
            // entity.equipment.installationdate
            new TranslationSeedItem("entity.equipment.installationdate", "zh-CN", "安装日期", "安装日期"),
            // entity.equipment.installationdate
            new TranslationSeedItem("entity.equipment.installationdate", "zh-HK", "安装日期", "安装日期"),

            // entity.equipment.startdate
            new TranslationSeedItem("entity.equipment.startdate", "en-US", "启用日期", "启用日期"),
            // entity.equipment.startdate
            new TranslationSeedItem("entity.equipment.startdate", "ja-JP", "启用日期", "启用日期"),
            // entity.equipment.startdate
            new TranslationSeedItem("entity.equipment.startdate", "zh-CN", "启用日期", "启用日期"),
            // entity.equipment.startdate
            new TranslationSeedItem("entity.equipment.startdate", "zh-HK", "启用日期", "启用日期"),

            // entity.equipment.warrantystartdate
            new TranslationSeedItem("entity.equipment.warrantystartdate", "en-US", "保修开始日期", "保修开始日期"),
            // entity.equipment.warrantystartdate
            new TranslationSeedItem("entity.equipment.warrantystartdate", "ja-JP", "保修开始日期", "保修开始日期"),
            // entity.equipment.warrantystartdate
            new TranslationSeedItem("entity.equipment.warrantystartdate", "zh-CN", "保修开始日期", "保修开始日期"),
            // entity.equipment.warrantystartdate
            new TranslationSeedItem("entity.equipment.warrantystartdate", "zh-HK", "保修开始日期", "保修开始日期"),

            // entity.equipment.warrantyenddate
            new TranslationSeedItem("entity.equipment.warrantyenddate", "en-US", "保修结束日期", "保修结束日期"),
            // entity.equipment.warrantyenddate
            new TranslationSeedItem("entity.equipment.warrantyenddate", "ja-JP", "保修结束日期", "保修结束日期"),
            // entity.equipment.warrantyenddate
            new TranslationSeedItem("entity.equipment.warrantyenddate", "zh-CN", "保修结束日期", "保修结束日期"),
            // entity.equipment.warrantyenddate
            new TranslationSeedItem("entity.equipment.warrantyenddate", "zh-HK", "保修结束日期", "保修结束日期"),

            // entity.equipment.originalvalue
            new TranslationSeedItem("entity.equipment.originalvalue", "en-US", "设备原值", "设备原值（精确到分，存储为整数，单位为分）"),
            // entity.equipment.originalvalue
            new TranslationSeedItem("entity.equipment.originalvalue", "ja-JP", "设备原值", "设备原值（精确到分，存储为整数，单位为分）"),
            // entity.equipment.originalvalue
            new TranslationSeedItem("entity.equipment.originalvalue", "zh-CN", "设备原值", "设备原值（精确到分，存储为整数，单位为分）"),
            // entity.equipment.originalvalue
            new TranslationSeedItem("entity.equipment.originalvalue", "zh-HK", "设备原值", "设备原值（精确到分，存储为整数，单位为分）"),

            // entity.equipment.technicalparameters
            new TranslationSeedItem("entity.equipment.technicalparameters", "en-US", "设备技术参数", "设备技术参数（JSON格式，存储设备技术参数配置）"),
            // entity.equipment.technicalparameters
            new TranslationSeedItem("entity.equipment.technicalparameters", "ja-JP", "设备技术参数", "设备技术参数（JSON格式，存储设备技术参数配置）"),
            // entity.equipment.technicalparameters
            new TranslationSeedItem("entity.equipment.technicalparameters", "zh-CN", "设备技术参数", "设备技术参数（JSON格式，存储设备技术参数配置）"),
            // entity.equipment.technicalparameters
            new TranslationSeedItem("entity.equipment.technicalparameters", "zh-HK", "设备技术参数", "设备技术参数（JSON格式，存储设备技术参数配置）"),

            // entity.equipment.images
            new TranslationSeedItem("entity.equipment.images", "en-US", "设备图片", "设备图片（JSON格式，存储设备图片URL列表）"),
            // entity.equipment.images
            new TranslationSeedItem("entity.equipment.images", "ja-JP", "设备图片", "设备图片（JSON格式，存储设备图片URL列表）"),
            // entity.equipment.images
            new TranslationSeedItem("entity.equipment.images", "zh-CN", "设备图片", "设备图片（JSON格式，存储设备图片URL列表）"),
            // entity.equipment.images
            new TranslationSeedItem("entity.equipment.images", "zh-HK", "设备图片", "设备图片（JSON格式，存储设备图片URL列表）"),

            // entity.equipment.documents
            new TranslationSeedItem("entity.equipment.documents", "en-US", "设备文档", "设备文档（JSON格式，存储设备文档ID列表）"),
            // entity.equipment.documents
            new TranslationSeedItem("entity.equipment.documents", "ja-JP", "设备文档", "设备文档（JSON格式，存储设备文档ID列表）"),
            // entity.equipment.documents
            new TranslationSeedItem("entity.equipment.documents", "zh-CN", "设备文档", "设备文档（JSON格式，存储设备文档ID列表）"),
            // entity.equipment.documents
            new TranslationSeedItem("entity.equipment.documents", "zh-HK", "设备文档", "设备文档（JSON格式，存储设备文档ID列表）"),

            // entity.equipment.iscritical
            new TranslationSeedItem("entity.equipment.iscritical", "en-US", "是否关键设备", "是否关键设备（0=否，1=是）"),
            // entity.equipment.iscritical
            new TranslationSeedItem("entity.equipment.iscritical", "ja-JP", "是否关键设备", "是否关键设备（0=否，1=是）"),
            // entity.equipment.iscritical
            new TranslationSeedItem("entity.equipment.iscritical", "zh-CN", "是否关键设备", "是否关键设备（0=否，1=是）"),
            // entity.equipment.iscritical
            new TranslationSeedItem("entity.equipment.iscritical", "zh-HK", "是否关键设备", "是否关键设备（0=否，1=是）"),

            // entity.equipment.warrantystatus
            new TranslationSeedItem("entity.equipment.warrantystatus", "en-US", "保修状态", "保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）"),
            // entity.equipment.warrantystatus
            new TranslationSeedItem("entity.equipment.warrantystatus", "ja-JP", "保修状态", "保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）"),
            // entity.equipment.warrantystatus
            new TranslationSeedItem("entity.equipment.warrantystatus", "zh-CN", "保修状态", "保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）"),
            // entity.equipment.warrantystatus
            new TranslationSeedItem("entity.equipment.warrantystatus", "zh-HK", "保修状态", "保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）"),

            // entity.equipment.status
            new TranslationSeedItem("entity.equipment.status", "en-US", "设备状态", "设备状态（0=运行中，1=停机，2=维修中，3=故障，4=待报废，5=已报废）"),
            // entity.equipment.status
            new TranslationSeedItem("entity.equipment.status", "ja-JP", "设备状态", "设备状态（0=运行中，1=停机，2=维修中，3=故障，4=待报废，5=已报废）"),
            // entity.equipment.status
            new TranslationSeedItem("entity.equipment.status", "zh-CN", "设备状态", "设备状态（0=运行中，1=停机，2=维修中，3=故障，4=待报废，5=已报废）"),
            // entity.equipment.status
            new TranslationSeedItem("entity.equipment.status", "zh-HK", "设备状态", "设备状态（0=运行中，1=停机，2=维修中，3=故障，4=待报废，5=已报废）"),

            // entity.equipment.maintenancerecords
            new TranslationSeedItem("entity.equipment.maintenancerecords", "en-US", "维护记录列表", "维护记录列表（外键：子表 TaktMaintenance.EquipmentId 关联本表 Id）"),
            // entity.equipment.maintenancerecords
            new TranslationSeedItem("entity.equipment.maintenancerecords", "ja-JP", "维护记录列表", "维护记录列表（外键：子表 TaktMaintenance.EquipmentId 关联本表 Id）"),
            // entity.equipment.maintenancerecords
            new TranslationSeedItem("entity.equipment.maintenancerecords", "zh-CN", "维护记录列表", "维护记录列表（外键：子表 TaktMaintenance.EquipmentId 关联本表 Id）"),
            // entity.equipment.maintenancerecords
            new TranslationSeedItem("entity.equipment.maintenancerecords", "zh-HK", "维护记录列表", "维护记录列表（外键：子表 TaktMaintenance.EquipmentId 关联本表 Id）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = 4;
        translation.ResourceType = 0;
        translation.ContextNote = item.ContextNote;
        translation.ExtFieldJson = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
