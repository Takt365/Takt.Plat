// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeikanI18nSeedData.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcSeikan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcSeikan 实体国际化翻译种子（键前缀 entity.ecseikan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcSeikanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcSeikan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecseikan 实体翻译...", tenantCode);

        foreach (var item in GetEcSeikanTranslations())
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

        TaktLogger.Information("TaktEcSeikan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcSeikan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecseikan._self / entity.ecseikan.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcSeikanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecseikan._self
            new TranslationSeedItem("entity.ecseikan._self", "en-US", "Ec Seikan Information_us", "实体名称"),
            // entity.ecseikan._self
            new TranslationSeedItem("entity.ecseikan._self", "ja-JP", "设变生管课信息_jp", "实体名称"),
            // entity.ecseikan._self
            new TranslationSeedItem("entity.ecseikan._self", "zh-CN", "设变生管课信息", "实体名称"),
            // entity.ecseikan._self
            new TranslationSeedItem("entity.ecseikan._self", "zh-HK", "设变生管课信息_hk", "实体名称"),

            // entity.ecseikan.ecndetailid
            new TranslationSeedItem("entity.ecseikan.ecndetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeikan 导航）"),
            // entity.ecseikan.ecndetailid
            new TranslationSeedItem("entity.ecseikan.ecndetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeikan 导航）"),
            // entity.ecseikan.ecndetailid
            new TranslationSeedItem("entity.ecseikan.ecndetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeikan 导航）"),
            // entity.ecseikan.ecndetailid
            new TranslationSeedItem("entity.ecseikan.ecndetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeikan 导航）"),

            // entity.ecseikan.eccode
            new TranslationSeedItem("entity.ecseikan.eccode", "en-US", "设变单号_us", "设变单号（冗余，便于查询）"),
            // entity.ecseikan.eccode
            new TranslationSeedItem("entity.ecseikan.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余，便于查询）"),
            // entity.ecseikan.eccode
            new TranslationSeedItem("entity.ecseikan.eccode", "zh-CN", "设变单号", "设变单号（冗余，便于查询）"),
            // entity.ecseikan.eccode
            new TranslationSeedItem("entity.ecseikan.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余，便于查询）"),

            // entity.ecseikan.linenumber
            new TranslationSeedItem("entity.ecseikan.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecseikan.linenumber
            new TranslationSeedItem("entity.ecseikan.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecseikan.linenumber
            new TranslationSeedItem("entity.ecseikan.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecseikan.linenumber
            new TranslationSeedItem("entity.ecseikan.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecseikan.ecmodelcode
            new TranslationSeedItem("entity.ecseikan.ecmodelcode", "en-US", "Model code_us", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecseikan.ecmodelcode
            new TranslationSeedItem("entity.ecseikan.ecmodelcode", "ja-JP", "機種コード_jp", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecseikan.ecmodelcode
            new TranslationSeedItem("entity.ecseikan.ecmodelcode", "zh-CN", "机种编码", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),
            // entity.ecseikan.ecmodelcode
            new TranslationSeedItem("entity.ecseikan.ecmodelcode", "zh-HK", "機種編碼_hk", "机种编码（冗余：来自 TaktEcDetail.EcModelCode）"),

            // entity.ecseikan.ecfinishedgoods
            new TranslationSeedItem("entity.ecseikan.ecfinishedgoods", "en-US", "完成品_us", "完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）"),
            // entity.ecseikan.ecfinishedgoods
            new TranslationSeedItem("entity.ecseikan.ecfinishedgoods", "ja-JP", "完成品_jp", "完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）"),
            // entity.ecseikan.ecfinishedgoods
            new TranslationSeedItem("entity.ecseikan.ecfinishedgoods", "zh-CN", "完成品", "完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）"),
            // entity.ecseikan.ecfinishedgoods
            new TranslationSeedItem("entity.ecseikan.ecfinishedgoods", "zh-HK", "完成品_hk", "完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）"),

            // entity.ecseikan.ecfinishedgoodsdescription
            new TranslationSeedItem("entity.ecseikan.ecfinishedgoodsdescription", "en-US", "完成品描述_us", "完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）"),
            // entity.ecseikan.ecfinishedgoodsdescription
            new TranslationSeedItem("entity.ecseikan.ecfinishedgoodsdescription", "ja-JP", "完成品描述_jp", "完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）"),
            // entity.ecseikan.ecfinishedgoodsdescription
            new TranslationSeedItem("entity.ecseikan.ecfinishedgoodsdescription", "zh-CN", "完成品描述", "完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）"),
            // entity.ecseikan.ecfinishedgoodsdescription
            new TranslationSeedItem("entity.ecseikan.ecfinishedgoodsdescription", "zh-HK", "完成品描述_hk", "完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）"),

            // entity.ecseikan.ecparentmaterialcode
            new TranslationSeedItem("entity.ecseikan.ecparentmaterialcode", "en-US", "上阶物料编码_us", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecseikan.ecparentmaterialcode
            new TranslationSeedItem("entity.ecseikan.ecparentmaterialcode", "ja-JP", "上阶物料编码_jp", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecseikan.ecparentmaterialcode
            new TranslationSeedItem("entity.ecseikan.ecparentmaterialcode", "zh-CN", "上阶物料编码", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),
            // entity.ecseikan.ecparentmaterialcode
            new TranslationSeedItem("entity.ecseikan.ecparentmaterialcode", "zh-HK", "上阶物料编码_hk", "上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）"),

            // entity.ecseikan.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecseikan.ecparentmaterialdescription", "en-US", "上阶物料描述_us", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecseikan.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecseikan.ecparentmaterialdescription", "ja-JP", "上阶物料描述_jp", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecseikan.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecseikan.ecparentmaterialdescription", "zh-CN", "上阶物料描述", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),
            // entity.ecseikan.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecseikan.ecparentmaterialdescription", "zh-HK", "上阶物料描述_hk", "上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）"),

            // entity.ecseikan.discontinuedstatus
            new TranslationSeedItem("entity.ecseikan.discontinuedstatus", "en-US", "完成品EOL_us", "完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseikan.discontinuedstatus
            new TranslationSeedItem("entity.ecseikan.discontinuedstatus", "ja-JP", "完成品EOL_jp", "完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseikan.discontinuedstatus
            new TranslationSeedItem("entity.ecseikan.discontinuedstatus", "zh-CN", "完成品EOL", "完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）"),
            // entity.ecseikan.discontinuedstatus
            new TranslationSeedItem("entity.ecseikan.discontinuedstatus", "zh-HK", "完成品EOL_hk", "完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）"),

            // entity.ecseikan.deptcode
            new TranslationSeedItem("entity.ecseikan.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode，5 位，如 D0420）"),
            // entity.ecseikan.deptcode
            new TranslationSeedItem("entity.ecseikan.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode，5 位，如 D0420）"),
            // entity.ecseikan.deptcode
            new TranslationSeedItem("entity.ecseikan.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode，5 位，如 D0420）"),
            // entity.ecseikan.deptcode
            new TranslationSeedItem("entity.ecseikan.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode，5 位，如 D0420）"),

            // entity.ecseikan.isimplemented
            new TranslationSeedItem("entity.ecseikan.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseikan.isimplemented
            new TranslationSeedItem("entity.ecseikan.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseikan.isimplemented
            new TranslationSeedItem("entity.ecseikan.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseikan.isimplemented
            new TranslationSeedItem("entity.ecseikan.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecseikan.execcontent
            new TranslationSeedItem("entity.ecseikan.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecseikan.execcontent
            new TranslationSeedItem("entity.ecseikan.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecseikan.execcontent
            new TranslationSeedItem("entity.ecseikan.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecseikan.execcontent
            new TranslationSeedItem("entity.ecseikan.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecseikan.scheduledproductiondate
            new TranslationSeedItem("entity.ecseikan.scheduledproductiondate", "en-US", "预计生产日期_us", "预计生产日期"),
            // entity.ecseikan.scheduledproductiondate
            new TranslationSeedItem("entity.ecseikan.scheduledproductiondate", "ja-JP", "预计生产日期_jp", "预计生产日期"),
            // entity.ecseikan.scheduledproductiondate
            new TranslationSeedItem("entity.ecseikan.scheduledproductiondate", "zh-CN", "预计生产日期", "预计生产日期"),
            // entity.ecseikan.scheduledproductiondate
            new TranslationSeedItem("entity.ecseikan.scheduledproductiondate", "zh-HK", "预计生产日期_hk", "预计生产日期"),

            // entity.ecseikan.scheduledbatch
            new TranslationSeedItem("entity.ecseikan.scheduledbatch", "en-US", "预定批次_us", "预定批次"),
            // entity.ecseikan.scheduledbatch
            new TranslationSeedItem("entity.ecseikan.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次"),
            // entity.ecseikan.scheduledbatch
            new TranslationSeedItem("entity.ecseikan.scheduledbatch", "zh-CN", "预定批次", "预定批次"),
            // entity.ecseikan.scheduledbatch
            new TranslationSeedItem("entity.ecseikan.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次"),

            // entity.ecseikan.poremainder
            new TranslationSeedItem("entity.ecseikan.poremainder", "en-US", "Po残_us", "Po残"),
            // entity.ecseikan.poremainder
            new TranslationSeedItem("entity.ecseikan.poremainder", "ja-JP", "Po残_jp", "Po残"),
            // entity.ecseikan.poremainder
            new TranslationSeedItem("entity.ecseikan.poremainder", "zh-CN", "Po残", "Po残"),
            // entity.ecseikan.poremainder
            new TranslationSeedItem("entity.ecseikan.poremainder", "zh-HK", "Po残_hk", "Po残"),

            // entity.ecseikan.balance
            new TranslationSeedItem("entity.ecseikan.balance", "en-US", "结余_us", "结余"),
            // entity.ecseikan.balance
            new TranslationSeedItem("entity.ecseikan.balance", "ja-JP", "结余_jp", "结余"),
            // entity.ecseikan.balance
            new TranslationSeedItem("entity.ecseikan.balance", "zh-CN", "结余", "结余"),
            // entity.ecseikan.balance
            new TranslationSeedItem("entity.ecseikan.balance", "zh-HK", "结余_hk", "结余"),

            // entity.ecseikan.oldproducthandling
            new TranslationSeedItem("entity.ecseikan.oldproducthandling", "en-US", "旧品处理_us", "旧品处理"),
            // entity.ecseikan.oldproducthandling
            new TranslationSeedItem("entity.ecseikan.oldproducthandling", "ja-JP", "旧品处理_jp", "旧品处理"),
            // entity.ecseikan.oldproducthandling
            new TranslationSeedItem("entity.ecseikan.oldproducthandling", "zh-CN", "旧品处理", "旧品处理"),
            // entity.ecseikan.oldproducthandling
            new TranslationSeedItem("entity.ecseikan.oldproducthandling", "zh-HK", "旧品处理_hk", "旧品处理"),

            // entity.ecseikan.isobsolete
            new TranslationSeedItem("entity.ecseikan.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseikan.isobsolete
            new TranslationSeedItem("entity.ecseikan.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseikan.isobsolete
            new TranslationSeedItem("entity.ecseikan.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseikan.isobsolete
            new TranslationSeedItem("entity.ecseikan.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "EngineeringChange";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
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
