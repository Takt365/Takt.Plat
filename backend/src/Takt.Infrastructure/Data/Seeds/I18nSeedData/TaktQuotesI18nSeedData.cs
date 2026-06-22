// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktQuotesI18nSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：名言警句国际化翻译种子数据初始化（26条名言 × 4 种语言）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;

/// <summary>
/// 名言警句国际化翻译种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// 包含 26 条经典名言警句的英、日、中、港繁四语翻译
/// </summary>
public class TaktQuotesI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在通用翻译之后）
    /// </summary>
    public int Order => 50;

    /// <summary>
    /// 初始化名言警句国际化翻译种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化名言警句国际化翻译种子数据...");

        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过名言警句国际化翻译种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化名言警句翻译数据...", tenantCode);

        foreach (var row in GetStandardQuotesTranslations())
        {
            if (!cultureIdByCode.TryGetValue(row.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", row.CultureCode, row.I18nKey);
                continue;
            }

            var item = new TranslationSeedItem(row.I18nKey, row.CultureCode, row.TranslationText, row.ContextNote);
            var (_, i, u) = await CreateOrUpdateTranslationAsync(repository, tenantCode, cultureId, item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("名言警句国际化翻译种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取标准名言警句翻译列表
    /// 包含 26 条经典名言的英、日、中、港繁四语翻译
    /// </summary>
    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetStandardQuotesTranslations()
    {
        return new List<(string, string, string, string?)>
        {
            // ========================================
            // 名言警句 A-Z (common.page.quote.*)
            // ========================================
            
            // A
            ("common.page.quote.a", "zh-CN", "长风破浪会有时，直挂云帆济沧海。", "李白"),
            ("common.page.quote.a", "en-US", "Many hands make light work", "English Proverb"),
            ("common.page.quote.a", "ja-JP", "人が心に抱き、信じられることは、すべて実現できる。", "名言"),
            ("common.page.quote.a", "zh-HK", "長風破浪會有時，直掛雲帆濟滄海。", "李白"),
            
            // B
            ("common.page.quote.b", "zh-CN", "老骥伏枥，志在千里；烈士暮年，壮心不已。", "曹操"),
            ("common.page.quote.b", "en-US", "Strike while the iron is hot", "English Proverb"),
            ("common.page.quote.b", "ja-JP", "成功者になるためではなく、価値のある者になるために努力せよ。", "アインシュタイン"),
            ("common.page.quote.b", "zh-HK", "老驥伏櫪，志在千里；烈士暮年，壯心不已。", "曹操"),
            
            // C
            ("common.page.quote.c", "zh-CN", "博观而约取，厚积而薄发。", "苏轼"),
            ("common.page.quote.c", "en-US", "Honesty is the best policy", "Benjamin Franklin"),
            ("common.page.quote.c", "ja-JP", "私が成功した理由はほかでもない、自分にも他人にも言い訳を許さなかったからだ。", "名言"),
            ("common.page.quote.c", "zh-HK", "博觀而約取，厚積而薄發。", "苏轼"),
            
            // D
            ("common.page.quote.d", "zh-CN", "不飞则已，一飞冲天；不鸣则已，一鸣惊人。", "司马迁"),
            ("common.page.quote.d", "en-US", "The grass is always greener on the other side of the fence", "English Proverb"),
            ("common.page.quote.d", "ja-JP", "打たないシュートは100%決まらない。", "マイケル・ジョーダン"),
            ("common.page.quote.d", "zh-HK", "不飛則已，一飛沖天；不鳴則已，一鳴驚人。", "司马迁"),
            
            // E
            ("common.page.quote.e", "zh-CN", "人生如逆旅，我亦是行人。", "苏轼"),
            ("common.page.quote.e", "en-US", "Don't judge a book by its cover", "English Proverb"),
            ("common.page.quote.e", "ja-JP", "一番難しいのは行動しようと腹をくくること。あとはただ粘り強さの問題だ。", "アメリア・イアハート"),
            ("common.page.quote.e", "zh-HK", "人生如逆旅，我亦是行人。", "苏轼"),
            
            // F
            ("common.page.quote.f", "zh-CN", "粉骨碎身浑不怕，要留清白在人间。", "于谦"),
            ("common.page.quote.f", "en-US", "An apple a day keeps the doctor away", "English Proverb"),
            ("common.page.quote.f", "ja-JP", "目的が明確であることは、あらゆる偉業の出発点である。", "W・クレメント・ストーン"),
            ("common.page.quote.f", "zh-HK", "粉骨碎身渾不怕，要留清白在人間。", "于谦"),
            
            // G
            ("common.page.quote.g", "zh-CN", "花开堪折直须折，莫待无花空折枝。", "杜秋娘"),
            ("common.page.quote.g", "en-US", "Better late than never", "English Proverb"),
            ("common.page.quote.g", "ja-JP", "過去は亡霊であり、未来は夢だ。ぼくらには今しかない。", "トーマス・ジェファーソン"),
            ("common.page.quote.g", "zh-HK", "花開堪折直須折，莫待無花空折枝。", "杜秋娘"),
            
            // H
            ("common.page.quote.h", "zh-CN", "千磨万击还坚劲，任尔东西南北风。", "郑板桥"),
            ("common.page.quote.h", "en-US", "Don't bite the hand that feeds you", "English Proverb"),
            ("common.page.quote.h", "ja-JP", "人生とは、あれこれ計画を立てるのに夢中になっている間に、ぼくらの身に起きていることだ。", "ジョン・レノン"),
            ("common.page.quote.h", "zh-HK", "千磨萬擊還堅勁，任爾東西南北風。", "郑板桥"),
            
            // I
            ("common.page.quote.i", "zh-CN", "臣心一片磁针石，不指南方不肯休。", "文天祥"),
            ("common.page.quote.i", "en-US", "Rome wasn't built in a day", "English Proverb"),
            ("common.page.quote.i", "ja-JP", "私たちは自分が思ったとおりの人間になる。", "ブッダ"),
            ("common.page.quote.i", "zh-HK", "臣心一片磁針石，不指南方不肯休。", "文天祥"),
            
            // J
            ("common.page.quote.j", "zh-CN", "黑发不知勤学早，白首方悔读书迟。", "颜真卿"),
            ("common.page.quote.j", "en-US", "Curiosity killed the cat", "English Proverb"),
            ("common.page.quote.j", "ja-JP", "過去は亡霊であり、未来は夢だ。ぼくらのには今しかない。", "トーマス・ジェファーソン"),
            ("common.page.quote.j", "zh-HK", "黑髮不知勤學早，白首方悔讀書遲。", "颜真卿"),
            
            // K
            ("common.page.quote.k", "zh-CN", "不畏浮云遮望眼，只缘身在最高层。", "王安石"),
            ("common.page.quote.k", "en-US", "My hands are tied", "English Idiom"),
            ("common.page.quote.k", "ja-JP", "成功の80%はそこに行くかどうかで決まる。", "ウディ・アレン"),
            ("common.page.quote.k", "zh-HK", "不畏浮雲遮望眼，只緣身在最高層。", "王安石"),
            
            // L
            ("common.page.quote.l", "zh-CN", "花门楼前见秋草，岂能贫贱相看老。", "岑参"),
            ("common.page.quote.l", "en-US", "Out of sight, out of mind", "English Proverb"),
            ("common.page.quote.l", "ja-JP", "勝つことがすべてではなく、勝ちたいと思うことがすべてだ。", "テッド・ウィリアムズ"),
            ("common.page.quote.l", "zh-HK", "花門樓前見秋草，豈能貧賤相看老。", "岑参"),
            
            // M
            ("common.page.quote.m", "zh-CN", "花门楼前见秋草，岂能贫贱相看老。", "岑参"),
            ("common.page.quote.m", "en-US", "Easy come, easy go", "English Proverb"),
            ("common.page.quote.m", "ja-JP", "私は自らをとりまく状況の産物ではない。自らの意思決定の産物だ。", "スティーブン・コヴィー"),
            ("common.page.quote.m", "zh-HK", "花門樓前見秋草，豈能貧賤相看老。", "岑参"),
            
            // N
            ("common.page.quote.n", "zh-CN", "亦余心之所善兮，虽九死其犹未悔。", "屈原"),
            ("common.page.quote.n", "en-US", "You can't make an omelette without breaking a few eggs", "English Proverb"),
            ("common.page.quote.n", "ja-JP", "子供はみな芸術家である。問題は大人になっても、どうやって芸術家であり続けるかだ。", "ピカソ"),
            ("common.page.quote.n", "zh-HK", "亦餘心之所善兮，雖九死其猶未悔。", "屈原"),
            
            // O
            ("common.page.quote.o", "zh-CN", "人与人之间最大的信任是精诚相见", "励志名言"),
            ("common.page.quote.o", "en-US", "The forbidden fruit is always the sweetest", "English Proverb"),
            ("common.page.quote.o", "ja-JP", "あなたが一日を支配するか、一日に支配されるかのいずれかだ。", "ジム・ローン"),
            ("common.page.quote.o", "zh-HK", "人與人之間最大的信任是精誠相見", "励志名言"),
            
            // P
            ("common.page.quote.p", "zh-CN", "青春须早为，岂能长少年。", "孟郊"),
            ("common.page.quote.p", "en-US", "If you scratch my back, I'll scratch yours", "English Proverb"),
            ("common.page.quote.p", "ja-JP", "自分にはできると思うのも、できないと思うのも、いずれも正しい。", "ヘンリー・フォード"),
            ("common.page.quote.p", "zh-HK", "青春須早為，豈能長少年。", "孟郊"),
            
            // Q
            ("common.page.quote.q", "zh-CN", "靡不有初，鲜克有终。", "诗经"),
            ("common.page.quote.q", "en-US", "It's the tip of the iceberg", "English Idiom"),
            ("common.page.quote.q", "ja-JP", "人生で最も重要な日を二つ挙げるなら、それは生まれた日と、その理由を見いだした日だ。", "マーク・トウェイン"),
            ("common.page.quote.q", "zh-HK", "靡不有初，鮮克有終。", "诗经"),
            
            // R
            ("common.page.quote.r", "zh-CN", "仰天大笑出门去，我辈岂是蓬蒿人。", "李白"),
            ("common.page.quote.r", "en-US", "Learn to walk before you run", "English Proverb"),
            ("common.page.quote.r", "ja-JP", "人生は勇気次第で縮みも広がりもする。", "アナイス・ニン"),
            ("common.page.quote.r", "zh-HK", "仰天大笑出門去，我輩豈是蓬蒿人。", "李白"),
            
            // S
            ("common.page.quote.s", "zh-CN", "沉舟侧畔千帆过，病树前头万木春。", "刘禹锡"),
            ("common.page.quote.s", "en-US", "First things first", "English Proverb"),
            ("common.page.quote.s", "ja-JP", "目を引くものはいろいろあっても、心をとらえるものだけを追い求めよ。", "スティーブ・ジョブズ"),
            ("common.page.quote.s", "zh-HK", "沉舟側畔千帆過，病樹前頭萬木春。", "刘禹锡"),
            
            // T
            ("common.page.quote.t", "zh-CN", "天生我材必有用，千金散尽还复来。", "李白"),
            ("common.page.quote.t", "en-US", "Don't bite off more than you can chew", "English Proverb"),
            ("common.page.quote.t", "ja-JP", "自分ならできると信じれば、半分は終わったようなものだ。", "セオドア・ルーズベルト"),
            ("common.page.quote.t", "zh-HK", "天生我材必有用，千金散盡還復來。", "李白"),
            
            // U
            ("common.page.quote.u", "zh-CN", "夜阑卧听风吹雨，铁马冰河入梦来。", "陆游"),
            ("common.page.quote.u", "en-US", "It's better to be safe than sorry", "English Proverb"),
            ("common.page.quote.u", "ja-JP", "これまで望んだことはすべて、恐れの裏返しである。", "ネール・ドナルド・ウォルシュ"),
            ("common.page.quote.u", "zh-HK", "夜闌卧聽風吹雨，鐵馬冰河入夢來。", "陆游"),
            
            // V
            ("common.page.quote.v", "zh-CN", "黄沙百战穿金甲，不破楼兰终不还。", "王昌龄"),
            ("common.page.quote.v", "en-US", "The early bird catches the worm", "English Proverb"),
            ("common.page.quote.v", "ja-JP", "七転び八起き――日本のことわざ。", "日本谚语"),
            ("common.page.quote.v", "zh-HK", "黃沙百戰穿金甲，不破樓蘭終不還。", "王昌龄"),
            
            // W
            ("common.page.quote.w", "zh-CN", "宁为百夫长，胜作一书生。", "杨炯"),
            ("common.page.quote.w", "en-US", "Don't make a mountain out of an anthill (or molehill)", "English Proverb"),
            ("common.page.quote.w", "ja-JP", "すべてのものに美しさはあるが、すべての者に見えるわけではない。", "孔子"),
            ("common.page.quote.w", "zh-HK", "寧為百夫長，勝作一書生。", "杨炯"),

            // X
            ("common.page.quote.x", "zh-CN", "路漫漫其修远兮，吾将上下而求索。", "屈原"),
            ("common.page.quote.x", "en-US", "Where there is a will, there is a way", "English Proverb"),
            ("common.page.quote.x", "ja-JP", "千里の道も一歩から。", "老子"),
            ("common.page.quote.x", "zh-HK", "路漫漫其修遠兮，吾將上下而求索。", "屈原"),

            // Y
            ("common.page.quote.y", "zh-CN", "业精于勤，荒于嬉；行成于思，毁于随。", "韩愈"),
            ("common.page.quote.y", "en-US", "Practice makes perfect", "English Proverb"),
            ("common.page.quote.y", "ja-JP", "継続は力なり。", "日本谚语"),
            ("common.page.quote.y", "zh-HK", "業精於勤，荒於嬉；行成於思，毀於隨。", "韩愈"),

            // Z
            ("common.page.quote.z", "zh-CN", "志当存高远。", "诸葛亮"),
            ("common.page.quote.z", "en-US", "A journey of a thousand miles begins with a single step", "Lao Tzu"),
            ("common.page.quote.z", "ja-JP", "志高くんぞ、人に問わず。", "诸葛亮"),
            ("common.page.quote.z", "zh-HK", "志當存高遠。", "诸葛亮"),
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
        translation.ResourceGroup = "Foundation";
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