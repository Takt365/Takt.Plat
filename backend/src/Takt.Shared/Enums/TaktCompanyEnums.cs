// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktCompanyEnums.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：公司（TaktCompany）相关枚举
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 公司类型
/// </summary>
public enum TaktCompanyType
{
    /// <summary>
    /// 制造工厂
    /// </summary>
    [Display(Name = "制造工厂")]
    Manufacturing = 1,

    /// <summary>
    /// 销售公司
    /// </summary>
    [Display(Name = "销售公司")]
    Sales = 2,

    /// <summary>
    /// 研发中心
    /// </summary>
    [Display(Name = "研发中心")]
    Research = 3,

    /// <summary>
    /// 总部
    /// </summary>
    [Display(Name = "总部")]
    Headquarters = 4,

    /// <summary>
    /// 其他
    /// </summary>
    [Display(Name = "其他")]
    Other = 5,
}

/// <summary>
/// 企业性质（统计用登记注册类型）
/// 依据：《关于划分企业登记注册类型的规定》（国统字〔1998〕200号）；存库值为标准 3 位代码
/// </summary>
public enum TaktEnterpriseNature
{
    /// <summary>100 内资企业</summary>
    [Display(Name = "100 内资企业")]
    Domestic = 100,

    /// <summary>110 国有企业</summary>
    [Display(Name = "110 国有企业")]
    DomesticStateOwned = 110,

    /// <summary>120 集体企业</summary>
    [Display(Name = "120 集体企业")]
    DomesticCollective = 120,

    /// <summary>130 股份合作企业</summary>
    [Display(Name = "130 股份合作企业")]
    DomesticShareCooperative = 130,

    /// <summary>140 联营企业</summary>
    [Display(Name = "140 联营企业")]
    DomesticJointOperation = 140,

    /// <summary>150 有限责任公司</summary>
    [Display(Name = "150 有限责任公司")]
    DomesticLimitedLiability = 150,

    /// <summary>151 国有独资公司</summary>
    [Display(Name = "151 国有独资公司")]
    DomesticStateOwnedSole = 151,

    /// <summary>160 股份有限公司</summary>
    [Display(Name = "160 股份有限公司")]
    DomesticShareLimited = 160,

    /// <summary>170 私营企业</summary>
    [Display(Name = "170 私营企业")]
    DomesticPrivate = 170,

    /// <summary>190 其他内资企业</summary>
    [Display(Name = "190 其他内资企业")]
    DomesticOther = 190,

    /// <summary>200 港、澳、台商投资企业</summary>
    [Display(Name = "200 港、澳、台商投资企业")]
    HmtInvested = 200,

    /// <summary>210 合资经营企业（港、澳、台）</summary>
    [Display(Name = "210 合资经营企业（港、澳、台）")]
    HmtJointVenture = 210,

    /// <summary>220 合作经营企业（港、澳、台）</summary>
    [Display(Name = "220 合作经营企业（港、澳、台）")]
    HmtCooperative = 220,

    /// <summary>230 港、澳、台商独资经营企业</summary>
    [Display(Name = "230 港、澳、台商独资经营企业")]
    HmtWhollyOwned = 230,

    /// <summary>240 港、澳、台商投资股份有限公司</summary>
    [Display(Name = "240 港、澳、台商投资股份有限公司")]
    HmtShareLimited = 240,

    /// <summary>290 其他港、澳、台商投资企业</summary>
    [Display(Name = "290 其他港、澳、台商投资企业")]
    HmtOther = 290,

    /// <summary>300 外商投资企业</summary>
    [Display(Name = "300 外商投资企业")]
    ForeignInvested = 300,

    /// <summary>310 中外合资经营企业</summary>
    [Display(Name = "310 中外合资经营企业")]
    ForeignJointVenture = 310,

    /// <summary>320 中外合作经营企业</summary>
    [Display(Name = "320 中外合作经营企业")]
    ForeignCooperative = 320,

    /// <summary>330 外资企业</summary>
    [Display(Name = "330 外资企业")]
    ForeignWhollyOwned = 330,

    /// <summary>340 外商投资股份有限公司</summary>
    [Display(Name = "340 外商投资股份有限公司")]
    ForeignShareLimited = 340,

    /// <summary>390 其他外商投资企业</summary>
    [Display(Name = "390 其他外商投资企业")]
    ForeignOther = 390,
}

/// <summary>
/// 行业属性（国民经济行业分类门类）
/// 依据：GB/T 4754-2017《国民经济行业分类》；存库值为门类序号（A=1 … T=20）
/// </summary>
public enum TaktIndustryAttribute
{
    /// <summary>A 农、林、牧、渔业</summary>
    [Display(Name = "A 农、林、牧、渔业")]
    A = 1,

    /// <summary>B 采矿业</summary>
    [Display(Name = "B 采矿业")]
    B = 2,

    /// <summary>C 制造业</summary>
    [Display(Name = "C 制造业")]
    C = 3,

    /// <summary>D 电力、热力、燃气及水生产和供应业</summary>
    [Display(Name = "D 电力、热力、燃气及水生产和供应业")]
    D = 4,

    /// <summary>E 建筑业</summary>
    [Display(Name = "E 建筑业")]
    E = 5,

    /// <summary>F 批发和零售业</summary>
    [Display(Name = "F 批发和零售业")]
    F = 6,

    /// <summary>G 交通运输、仓储和邮政业</summary>
    [Display(Name = "G 交通运输、仓储和邮政业")]
    G = 7,

    /// <summary>H 住宿和餐饮业</summary>
    [Display(Name = "H 住宿和餐饮业")]
    H = 8,

    /// <summary>I 信息传输、软件和信息技术服务业</summary>
    [Display(Name = "I 信息传输、软件和信息技术服务业")]
    I = 9,

    /// <summary>J 金融业</summary>
    [Display(Name = "J 金融业")]
    J = 10,

    /// <summary>K 房地产业</summary>
    [Display(Name = "K 房地产业")]
    K = 11,

    /// <summary>L 租赁和商务服务业</summary>
    [Display(Name = "L 租赁和商务服务业")]
    L = 12,

    /// <summary>M 科学研究和技术服务业</summary>
    [Display(Name = "M 科学研究和技术服务业")]
    M = 13,

    /// <summary>N 水利、环境和公共设施管理业</summary>
    [Display(Name = "N 水利、环境和公共设施管理业")]
    N = 14,

    /// <summary>O 居民服务、修理和其他服务业</summary>
    [Display(Name = "O 居民服务、修理和其他服务业")]
    O = 15,

    /// <summary>P 教育</summary>
    [Display(Name = "P 教育")]
    P = 16,

    /// <summary>Q 卫生和社会工作</summary>
    [Display(Name = "Q 卫生和社会工作")]
    Q = 17,

    /// <summary>R 文化、体育和娱乐业</summary>
    [Display(Name = "R 文化、体育和娱乐业")]
    R = 18,

    /// <summary>S 公共管理、社会保障和社会组织</summary>
    [Display(Name = "S 公共管理、社会保障和社会组织")]
    S = 19,

    /// <summary>T 国际组织</summary>
    [Display(Name = "T 国际组织")]
    T = 20,
}

/// <summary>
/// 企业规模（统计上大中小微型划分）
/// 依据：《统计上大中小微型企业划分办法》(2017)；《中小企业划型标准规定》(工信部联企业〔2011〕300号)；存库值为统计代码 1–4
/// </summary>
public enum TaktEnterpriseScale
{
    /// <summary>1 大型企业</summary>
    [Display(Name = "1 大型企业")]
    Large = 1,

    /// <summary>2 中型企业</summary>
    [Display(Name = "2 中型企业")]
    Medium = 2,

    /// <summary>3 小型企业</summary>
    [Display(Name = "3 小型企业")]
    Small = 3,

    /// <summary>4 微型企业</summary>
    [Display(Name = "4 微型企业")]
    Micro = 4,
}

/// <summary>
/// 公司存续状态（市场主体登记状态）
/// 依据：《市场主体登记管理条例》及国家企业信用信息公示系统登记状态分类；存库值为标准代码 1–10
/// </summary>
public enum TaktCompanyExistenceStatus
{
    /// <summary>1 存续（在营、开业、在册）</summary>
    [Display(Name = "1 存续")]
    Subsisting = 1,

    /// <summary>2 在业</summary>
    [Display(Name = "2 在业")]
    Operating = 2,

    /// <summary>3 吊销</summary>
    [Display(Name = "3 吊销")]
    Revoked = 3,

    /// <summary>4 注销</summary>
    [Display(Name = "4 注销")]
    Cancelled = 4,

    /// <summary>5 迁出</summary>
    [Display(Name = "5 迁出")]
    MovedOut = 5,

    /// <summary>6 停业</summary>
    [Display(Name = "6 停业")]
    Suspended = 6,

    /// <summary>7 清算</summary>
    [Display(Name = "7 清算")]
    Liquidating = 7,

    /// <summary>8 歇业</summary>
    [Display(Name = "8 歇业")]
    Closed = 8,

    /// <summary>9 责令关闭</summary>
    [Display(Name = "9 责令关闭")]
    OrderedToClose = 9,

    /// <summary>10 撤销</summary>
    [Display(Name = "10 撤销")]
    RegistrationRevoked = 10,
}
