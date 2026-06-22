// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktDictTypeSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：字典类型种子数据初始化
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 字典类型种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// 注意：每个租户数据库只初始化自己的字典类型数据
/// Program.cs 会为每个租户数据库调用此方法，因此只需为当前租户初始化字典类型
/// </summary>
public class TaktDictTypeSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序
    /// </summary>
    public int Order => 45;

    /// <summary>
    /// 初始化字典类型种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化字典类型种子数据...");

        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过字典类型种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktDictType>>();

        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化字典类型数据...", tenantCode);

        // 为当前租户初始化标准字典类型
        var dictTypes = GetStandardDictTypes();
        
        foreach (var dictTypeData in dictTypes)
        {
            var (dictType, i, u) = await CreateOrUpdateDictTypeAsync(
                repository,
                tenantCode,
                dictTypeData.DictTypeCode,
                dictTypeData.DictTypeName,
                dictTypeData.Remark,
                dictTypeData.SortOrder);
            
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("字典类型种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取标准字典类型列表
    /// DictTypeCode 命名（强制）：{业务领域}_{业务项}_后缀；小写蛇形；至少 3 段
    /// 后缀仅允许：_code/_category/_status/_type/_config/_param
    /// 示例：sys_numbering_date_format_config、logistics_nonachievement_reason_category
    /// Category=业务聚合；Type=行为分支；Status=生命周期；Code=字典值/lookup；Config/Param=配置与参数
    /// </summary>
    private static List<(string DictTypeCode, string DictTypeName, string Remark, int SortOrder)> GetStandardDictTypes()
    {
        return new List<(string, string, string, int)>
        {
            ("accounting_account_category","科目类别","会计科目分类。用于财务科目管理",1),
            ("accounting_asset_category","资产大类","固定资产大类分类。用于资产管理",2),
            ("accounting_asset_type","资产类型","资产类型与品种。用于资产管理",3),
            ("accounting_cost_center_category","成本中心类别","成本中心分类。用于成本核算管理",4),
            ("accounting_cost_element_category","成本要素类别","成本要素分类。用于成本要素管理",5),
            ("accounting_currency_code","币种","货币类型。国际贸易和财务结算通用币种",6),
            ("accounting_payment_terms_param","付款条件","付款条件。用于客户和供应商财务付款条款管理",7),
            ("accounting_profit_center_category","利润中心类别","利润中心分类。用于利润核算管理",8),
            ("accounting_tax_code","税码","税务编码。用于增值税等税务管理",9),
            ("accounting_tax_rate_param","税率","税率。用于各类税务计算",10),
            ("gen_button_category","代码生成操作后缀","对应 TaktGenTable.MenuButtonGroup；DictValue 为完整权限码第四段英文 key（前缀为三段规范化的 PermsPrefixCanonical，见 TaktGenTable.PermsPrefix 注释）；多选逗号；TaktCodeGenWorkflowService.BuildSqlMenuButtonRowsAsync 生成 basePerm:sfx 与 MenuL10nKey=common.page.button.*；DictLabel 为中文名。已合并原 sys_button_category。曾用类型名「按钮权限后缀」片面。原编码 gen_menu_button。",10),
            ("gen_button_style_config","操作按钮样式","代码生成表 TaktGenTable.FrontBtnStyle（front_btn_style）。0=文本，1=标准。原编码 sys_button_style。",11),
            ("gen_csharp_data_type","C#数据类型","代码生成列 CsharpDataType。对应 string、int、long、DateTime、decimal、bool、Guid 等。C# 数据类型。原编码 sys_csharp_type。",12),
            ("gen_display_type","显示类型","代码生成列 HtmlType/显示类型。input、select、checkbox 等。原编码 sys_display_type。",13),
            ("gen_frontend_form_layout_config","前端表单布局","代码生成表 FrontFormLayout。12=一行一列，24=一行两列。原编码 sys_frontend_style。",14),
            ("gen_frontend_ui_type","前端UI框架","代码生成表 FrontUi。1=element plus，2=ant design vue。原编码 sys_frontend_template。",15),
            ("gen_function_type","生成功能","生成功能。查询，新增，更新，删除，状态，排序，模板，导入，导出",16),
            ("gen_method_type","生成方式","代码生成方式。0=zip 压缩包，1=自定义路径，2=当前项目",17),
            ("gen_query_type","查询方式","代码生成列 QueryType。EQ/NE/GT/GTE/LT/LTE/LIKE/BETWEEN。原编码 sys_query_type。",18),
            ("gen_template_type","生成模板类型","生成模板类型。对应 TaktGenTable.GenTemplateCategory。crud=单表操作，tree=树表操作，sub=主子表操作",19),
            ("hr_attendance_correction_type","补卡类型","补卡类型（与 TaktAttendanceCorrection.correction_kind 一致）。1=上班，2=下班；approval_status 共用字典 sys_approval_status",20),
            ("hr_attendance_device_brand_category","考勤设备品牌","设备品牌（与多品牌 SDK 路由一致）。Hikvision=海康威视，Deli=得力，ZKTeco=中控",22),
            ("hr_attendance_exception_handle_status","考勤异常处理状态","处理状态（与 TaktAttendanceException.handle_status 一致）。0=待处理，1=已处理，2=已忽略",24),
            ("hr_attendance_exception_type","考勤异常类型","异常类型（与 TaktAttendanceException.exception_type 一致）。1=上班缺卡，2=下班缺卡，3=迟到，4=早退，5=旷工，9=其他",25),
            ("hr_attendance_punch_source_type","打卡来源","打卡来源（与 TaktAttendancePunch.punch_source 一致）。0=后台录入，1=移动端，2=导入",26),
            ("hr_attendance_punch_type","打卡类型","打卡类型（与 TaktAttendancePunch.punch_type 一致）。1=上班，2=下班，3=外勤",27),
            ("hr_attendance_result_status","出勤状态","考勤日结出勤状态（与 TaktAttendanceResult.attendance_status 一致）。0=正常，1=迟到，2=早退，3=缺卡，4=旷工，5=加班",28),
            ("hr_attendance_verify_type","考勤验证方式","验证方式（与 TaktAttendanceSource.verify_mode 一致）。0=未知，1=指纹，2=人脸，3=密码，4=卡",29),
            ("hr_delegate_type","人事代理模式","部门/岗位/员工代理子表 delegate_mode。0=直接员工，1=部门规则，2=岗位规则",30),
            ("hr_employee_status","员工状态","员工状态（与 TaktEmployee.employee_status 一致）。1=试用期，2=正式，3=离职，4=退休",31),
            ("hr_education_level_category","学历","学历（与 TaktEmployee.education 一致）。1=高中及以下，2=大专，3=本科，4=硕士，5=博士",32),
            ("hr_ethnic_code","民族","民族（56 个民族）。DictValue=序号 1～56，与国家标准排序一致",33),
            ("hr_holiday_working_day_type","假日是否工作日","是否工作日（假日表 is_working_day）。0=非工作日，1=工作日，2=半天等；与 TaktHoliday.IsWorkingDay 一致",34),
            ("hr_holiday_category","假日类别","假日类别（列 holiday_type；字典 hr_holiday_category）。0=法定，1=调休，2=公司",35),
            ("hr_marital_status","婚姻状况","婚姻状况（与 TaktEmployee.marital_status 一致）。0=未婚，1=已婚，2=离异，3=丧偶",37),
            ("hr_native_place_code","籍贯","籍贯（省级行政区 6 位 GB 区划代码，如 110000）。用于下拉选择与员工档案 native_place 对照",38),
            ("hr_overtime_type","加班类型","加班类型（与 TaktOvertime.overtime_type 一致）。0=工作日加班，1=休息日加班，2=法定节假日加班",39),
            ("hr_political_status","政治面貌","政治面貌（国家标准十三类）。0=群众，1=共青团员，2=中共党员，3=中共预备党员，4=民革党员，5=民盟盟员，6=民建会员，7=民进会员，8=农工党党员，9=致公党党员，10=九三学社社员，11=台盟盟员，12=无党派民主人士",40),
            ("hr_schedule_type","排班类别","排班类别（与 TaktShiftSchedule.schedule_type 一致）。0=部门，1=人员",41),
            ("hr_reassignment_type","调动类型","员工调动类型。0=转岗，1=调岗",43),
            ("hr_talent_job_posting_status","职位发布状态","职位发布状态（与 TaktTalentJobPosting.posting_status 一致）。0=草稿，1=招聘中，2=已暂停，3=已关闭",46),
            ("hr_talent_publish_channel_type","职位发布渠道","职位发布渠道（与 TaktTalentJobPosting.publish_channel 一致）。0=官网，1=招聘网站，2=内推，3=校园，9=其他",47),
            ("hr_personnel_onboarding_status","入职待办状态","入职待办状态（与 TaktEmployeeOnboarding.todo_status 一致）。0=待办理，1=办理中，2=已完成，3=已取消",51),
            ("hr_resignation_category","离职类别","离职类别（列 resignation_type；字典 hr_resignation_category）。0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他",54),
            ("logistics_batch_management_type","批次管理标识","是否启用批次管理。0=否，1=是",44),
            ("logistics_bulk_material_type","散装物料标识","是否为散装物料。0=否，1=是",45),
            ("logistics_credit_rating_category","信用等级","信用等级。0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级",46),
            ("logistics_cycle_counting_category","周期盘点标识","物料周期盘点类别。A=12月，B=6月，C=3月，D=1月",48),
            ("logistics_defect_level_category","缺点等级","缺点等级。用于质量缺陷严重程度分级",49),
            ("logistics_equipment_category","设备类别","设备分类类别。用于设备资产管理",50),
            ("logistics_grade_category","等级类别","通用伙伴等级（供货商/经销商/客户等级共用；TaktSupplier.SupplierLevel、TaktVendor.VendorLevel 等）。0=普通，1=优选，2=战略，3=临时",51),
            ("logistics_handling_plan_category","处理方案","问题处理方案。用于质量问题和设备故障处理",52),
            ("logistics_inhouse_production_days_param","自制生产天数","自制生产所需天数。2=2天，5=5天",53),
            ("logistics_inspection_category","检验类别","检验业务归类。用于质量检验分类管理",54),
            ("logistics_inspection_item_type","检验项目类型","检验项目类型。用于检验项目分类管理",55),
            ("logistics_inspection_method_type","检验方式","检验方式。用于检验执行方式管理",56),
            ("logistics_inspection_severity_category","检验严格度","检验严格度。用于抽样检验严格程度管理",57),
            ("logistics_inspection_tool_category","检验工具","检验工具类型。用于检验设备和工具管理",58),
            ("logistics_inspection_type","检验类型","物料检验处理方式（影响检验流程）。0=免检，1=必检",59),
            ("logistics_judgment_category","判定类别","检验判定类别。用于检验结果判定",60),
            ("logistics_maintenance_category","维护类别","设备维护类型。用于维护工单管理",61),
            ("logistics_material_type","物料类型","通用物料类型。ABF=废料，CBAU=兼容设备，CH00=CH合同操作，CONT=看板容器，COUP=优惠券，DIEN=服务，EPA=设备包装，ERSA=备件，FERT=成品，FGTR=饮料，FHMI=生产资源/工具，FOOD=食品，FRIP=易腐品，HALB=半成品，HAWA=贸易货物，HERS=制造商部分，HIBE=经营供应，IBAU=维护装配，INTR=内部物料，KMAT=可配置物料，LEER=虚拟件，LEIH=可反复利用包装，LGUT=空零售，MODE=衣物，MPO=物料计划对象，NLAG=非存储物料，NOF1=非食品，PIPE=管线物料，PLAN=贸易货物计划，PROC=过程物料，PROD=产品组，ROH=原材料，UNBW=未估价物料，VERP=包装，VKHM=附加，VOLL=全部产品，WERB=产品目录，WERT=只有价值物料，WETT=竞争产品",63),
            ("logistics_planned_delivery_days_param","计划交货天数","供应商计划交货天数。7/30/60/90/120天",64),
            ("logistics_price_control_type","价格控制","物料价格控制方式。S=标准价格，V=移动平均价",65),
            ("logistics_price_type","价格类型","价格类型。0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格，5=客户价格，6=促销价格，7=成本价，8=批发价，9=零售价，10=协议价",66),
            ("logistics_price_unit_param","价格单位","物料价格单位基数。1/10/100/1000",67),
            ("logistics_procurement_type","采购类别","物料采购类型。E=自制生产，F=外部采购，X=两种采购类型",68),
            ("logistics_sampling_scheme_type","抽样方案类型","抽样方案类型。用于抽样检验方案管理",69),
            ("logistics_special_procurement_type","特殊采购类别","特殊采购类型。10=寄售，30=外协加工，50=虚设品号",70),
            ("logistics_unit_of_measure_code","基本单位类别","物料基本计量单位类别。国际通用计量单位，包含SI单位及商业常用单位",72),
            ("logistics_valuation_class_category","评估类别","物料评估类别。7920=成品，Z300=原材料(CN)，Z790=半成品(CN)，Z792=成品(CN)",73),
            ("logistics_inbound_type","入库类型","入库类型。0=采购入库，1=生产入库，2=退货入库，3=调拨入库，4=序列号入库，5=其他",152),
            ("logistics_outbound_type","出库类型","出库类型。0=销售出库，1=生产领料，2=退货出库，3=调拨出库，4=报废出库，5=序列号出库，6=其他",153),
            ("logistics_shipping_method_type","运输方式","运输方式。0=海运，1=空运，2=陆运，3=铁路，4=快递，5=其他",154),
            ("logistics_delivery_status","交货状态","交货状态。0=未交货，1=部分交货，2=全部交货",155),
            ("logistics_payment_method_type","支付/收款方式","支付方式/收款方式。0=现金，1=银行转账，2=支票，3=信用证，4=其他",156),
            ("logistics_discount_rate_param","折扣率","折扣率预设（百分比）。0~100",157),
            ("logistics_tax_rate_param","税费率","税费率预设（百分比）。常见增值税税率",158),
            ("logistics_supplier_category","供货商类别","供货商业务归类。0=生产商，1=代理商，2=经销商，3=贸易商，4=其他",159),
            ("logistics_payment_terms_param","付款条件","付款条件。0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他",160),
            ("logistics_vendor_category","经销商类别","经销商业务归类。0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他",162),
            ("logistics_client_category","客户端类别","客户端业务归类。0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他",163),
            ("logistics_sales_channel_type","销售渠道","销售渠道。0=直销，1=经销，2=代销，3=电商，4=其他",164),
            ("logistics_customer_level_category","客户等级","客户/客户端等级。0=普通，1=重要，2=VIP，3=战略",165),
            ("logistics_customer_category","客户类别","客户业务归类。0=企业客户，1=个人客户，2=政府机构，3=其他",168),
            ("logistics_invoice_status","发票状态","发票状态。0=草稿，1=已开票，2=已收款，3=已作废",169),
            ("logistics_delivery_method_type","交货方式","交货方式。0=自提，1=送货上门，2=物流配送，3=快递",170),
            ("logistics_sales_price_type","销售价格类型","销售价格类型。0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格",171),
            ("logistics_quotation_status","报价状态","报价状态。0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废",172),
            ("logistics_aoi_inspection_line_category","AOI线别","AOI 检测线别，如 AOI1、AOI2 等。原编码 aoi_inspection_line。",74),
            ("logistics_assy_location_category","Assy个所","组立个所。自插、部品、设计、修正、加工、手插、组立、SMT、其他",75),
            ("logistics_defect_category","不良区分","组立不良区分/类别，用于不良明细分类。原编码 defect_category。",76),
            ("logistics_defect_location_category","不良个所","不良个所/发生场所，用于不良明细发生位置。原编码 defect_location。",77),
            ("logistics_ec_distinction_category","设变管理区分","设变管理区分。1=全仕向，2=部管，3=内部，4=技术。原编码 sys_ec_distinction。",78),
            ("logistics_ec_status","设变状态","设变（ECN）改号状态。1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的。原编码 sys_ec_status。",79),
            ("sys_equipment_status","设备状态","通用设备状态（字典 sys_equipment_status；TaktEquipment.EquipmentStatus、考勤设备 device_status 等共用）。0=运行中，1=停机，2=维修中，3=故障，4=待报废，5=已报废",80),
            ("logistics_equipment_type","设备类型","设备类型。0=生产设备，1=检测设备，2=包装设备，3=物流设备，4=辅助设备",81),
            ("logistics_maintenance_type","维护类型","维护类型。0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他",82),
            ("logistics_nonachievement_reason_category","未达成原因","未达成原因。清机、测试慢/测试修理机、修理试机、转机、人员欠缺、部品不良/欠料、ST差异大、仪器设备/设置/调试/检查/故障/切换、请假/旷工、其他、切换机种/仕向、组立慢/加工多/工程多/下机慢/作业困难/升级慢、改修、坏机多/不良多、人员借调、返工、下机慢、学习中/新人员学习/开会、正常",83),
            ("logistics_pcb_location_category","PCB个所","PCB个所。翘脚、生锡、锡量過多、空焊、漏件、发黄、IC PIN竖立/浮高、连锡、異物付着、底下有部品、基板不良、红胶不良、反面、位置偏移、部品不良/破損、立碑、翻面、撞件、错料、侧立、反向、PCB不良、焊接不良、極性相違、多件、锡少等",84),
            ("logistics_pcba_function_category","PCBA功能类别","PCBA 功能类别。A、ADOC、ANA、AUDIO、B、BOTTOM、BTICE、C、DSPL、ENC、FRONT、INPUT、IO、JACK、L、LCD、MAIN、PANEL、POWER、REAR、RMN-1、SATA、SEQ、SYS、TOP、USB。原编码 pcba_function_category。",85),
            ("logistics_pcba_panel_category","PCBA板位类别","PCBA 板位类别。电源、前板、IO板等。原编码 pcba_panel_category。",86),
            ("logistics_pcba_side_category","PCBA面别","PCBA 面别。B面、T面。原编码 pcba_side_category。",87),
            ("logistics_shift_category","生产班别","生产班别。早、中、晚、白班、夜班",88),
            ("logistics_stop_reason_category","停线原因","停线原因。切换停止时间、周会、其他、欠料、停电、班会、切换机种、早会、组立、学习、仪设、清洁",89),
            ("logistics_visual_inspection_line_category","目视线别","目视检查线别，如 L1、L2 等。原编码 visual_inspection_line。",90),
            ("logistics_warranty_status","保修状态","保修状态。0=无保修，1=保修期内，2=保修期外，3=延保中",91),
            ("logistics_work_center_type","工作中心类型","工作中心类型。按产品类别+工序类型划分，如Pro加工、Pro组立、ProSMT等",92),
            ("sys_culture_code","区域文化编码","BCP47 区域文化编码（对齐 TaktCulture.CultureCode；用户/公司/工厂 default_culture 下拉）。共 33 项，如 af-ZA、zh-CN、en-US、ja-JP",93),
            ("sys_data_scope_type","数据权限","数据范围。0=全部数据，1=本部门数据，2=本部门及以下数据，3=仅本人数据，4=自定义数据范围",94),
            ("sys_data_source_type","数据源","字典数据源（与 TaktDictType.data_source 一致）。0=系统表，1=SQL查询",95),
            ("sys_db_data_type","数据库数据类型","数据库数据类型。基于数据库的数据类型，如：varchar、int、datetime、decimal等。原编码 sys_db_type。",96),
            ("sys_dept_type","部门类型","部门类型。0=直接，1=间接",97),
            ("sys_flow_category","流程分类","流程分类。0=通用流程，1=业务流程，2=系统流程",98),
            ("sys_flow_status","流程状态","流程实例运行状态。0=运行中，1=已完成，2=已终止，3=已挂起，4=已撤回，5=草稿",99),
            ("sys_form_category","表单分类","表单分类。0=通用表单，1=业务表单，2=系统表单",100),
            ("sys_form_type","表单类型","表单类型。0=动态表单，1=静态表单，2=自定义表单",101),
            ("sys_ftp_provider_type","FTP服务提供商","FTP服务提供商类型。teac_cn=TEAC FTP中国（ftp.teac.com.cn），teac_jp=TEAC FTP日本（rosu2.teac.co.jp）",102),
            ("sys_is_builtin_type","是否内置","是否内置标志。1=是/内置，0=否/自定义",103),
            ("sys_is_default_type","是否默认","通用默认标志。1=是/默认，0=否/非默认",104),
            ("sys_is_public_type","公开","公开标志。0=公开，1=私有",105),
            ("sys_leave_type","请假类型","请假类型（列 leave_type；影响审批/薪酬逻辑；字典 sys_leave_type）。affair=事假，sick=病假，annual=年假，marriage=婚假，maternity=产假，paternity=陪产假，bereavement=丧假，compensatory=调休，personal=私假，other=其他，可扩展",106),
            ("sys_list_class_config","列表类名","列表类名。用于前端样式控制",107),
            ("sys_mail_status","邮件状态","邮件状态。0=草稿，1=已发送，2=发送失败，3=已撤回，4=定时发送中",108),
            ("sys_mail_type","邮件类型","邮件类型。0=普通邮件，1=系统邮件，2=通知邮件，3=提醒邮件",109),
            ("sys_menu_type","菜单类型","菜单类型。0=目录，1=菜单，2=按钮",110),
            ("sys_message_group_category","消息分组","消息分组。Collaboration=协同，OfficialDocument=公文，Document=文档，Announcement=公告，Other=其他，Message=消息，Reminder=提醒",111),
            ("sys_message_type","消息类型","消息类型。Text=文本，Image=图片，File=文件，System=系统消息",112),
            ("sys_news_category","新闻分类","新闻分类。0=公司新闻，1=行业动态，2=技术分享，3=产品发布，4=活动资讯，5=其他",113),
            ("sys_publish_status","发布状态","内容发布生命周期（字典 sys_publish_status；TaktNews.NewsStatus、TaktAnnouncement.AnnouncementStatus 等共用）。0=草稿，1=已发布，2=已撤回，3=已过期",114),
            ("sys_approval_status","审批状态","通用审批状态（与 TaktApprovalEntityBase.ApprovalStatus / TaktApprovalStatus 一致；各业务 ApprovalStatus、TaktLeave.LeaveStatus 等共用）。0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回，5=已终止",115),
            ("sys_ticket_status","工单状态","通用工单状态（字典 sys_ticket_status；TaktTicket.TicketStatus、TaktServiceTicket.TicketStatus、TaktMaintenanceWorkOrder.WorkOrderStatus 等共用）。0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开",116),
            ("sys_normal_disable_status","默认状态","通用默认状态。1=启用，0=禁用，2=锁定",117),
            ("sys_announcement_category","公告类别","公告业务归类（列 announcement_type；字典 sys_announcement_category）。1=紧急通知，2=公告，3=通知，4=决议，5=活动，6=安全通告，7=运维通知，8=系统公告",117),
            ("sys_online_status","在线状态","在线状态。0=在线，1=离线，2=离开",118),
            ("sys_oper_type","操作类型","系统操作类型。1=新增，2=修改，3=删除，4=查询，5=导出，6=导入，7=授权，8=强退，9=生成代码，10=清空数据",119),
            ("sys_oss_provider_type","OSS提供商类型","OSS对象存储提供商类型。aliyun=阿里云OSS，tencent=腾讯云COS，huawei=华为云OBS，aws=AWS S3",120),
            ("sys_post_category","岗位类别","岗位类别（与 TaktPost.PostCategory 一致）。MGT=管理岗，PRO=专业岗，TEC=技术岗，SUP=支持岗，OPS=操作岗",121),
            ("sys_post_level_category","岗位职级","岗位职级（与 TaktPost.PostLevel 一致）。P1~P4 专业序列，M1~M5 管理序列",122),
            ("sys_priority_level_category","优先级","优先级（字典 sys_priority_level_category）。1=最高，2=高，3=普通，4=低",123),
            ("sys_publish_scope_type","发布范围","发布范围。0=全部，1=指定部门，2=指定用户，3=指定角色",124),
            ("sys_read_status","读取状态","读取状态。0=未读，1=已读",125),
            ("sys_reset_period_config","重置周期","编号规则流水号重置周期（与 date_format 粒度匹配，排序：none=不重置，year=按年，month=按月，day=按日，hour=按时）",126),
            ("sys_resource_type","资源类型","资源类型（与 TaktTranslation.ResourceType 一致，字典 sys_resource_type）。frontend=前端，backend=后端",127),
            ("sys_scheme_status","方案状态","流程/表单方案状态。0=草稿，1=已发布，2=已禁用",128),
            ("sys_setting_group_category","设置分组","设置分组。backend=后端，frontend=前端",129),
            ("sys_sort_type","排序类型","排序类型。asc=升序，desc=降序",130),
            ("sys_storage_naming_config","存储命名规则","存储命名规则。0=原文件+哈希值，1=自动生成，2=自定义",131),
            ("sys_storage_type","存储方式","存储方式。0=本地存储，1=OSS对象存储，2=FTP，3=其他",132),
            ("sys_urgency_level_category","紧急度","紧急度 Urgency（字典 sys_urgency_level_category）。1=High/高，2=Medium/中，3=Low/低",133),
            ("sys_impact_level_category","影响范围","影响范围 Impact（字典 sys_impact_level_category）。1=High/高，2=Medium/中，3=Low/低",134),
            ("sys_user_gender_category","用户性别","用户性别。0=未知，1=男，2=女",135),
            ("sys_user_type","用户类型","用户类型。0=普通用户，1=管理员，2=超级管理员",136),
            ("sys_word_category","敏感词词性类别","敏感词分类（与 Takt.Domain.Entities.Foundation.TaktVocabulary.WordCategory 一致，字典 sys_word_category）。1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视",137),
            ("sys_word_filter_level_category","敏感词过滤等级","敏感词过滤等级（与 Takt.Domain.Entities.Foundation.TaktVocabulary.FilterLevel 一致，字典 sys_word_filter_level_category）。1=低，2=中，3=高",138),
            ("sys_yes_no_type","是否","通用布尔标志。1=是/启用，0=否/禁用",139),
            ("sys_numbering_date_format_config","编号日期格式","编号规则日期格式（与 TaktNumbering.date_format 一致）。none=不使用；yyyy/yyyyMM/yyyyMMdd/yyyyMMddHH 须与 reset_period 按年/月/日/时匹配",140),
            ("routine_ticket_source_type","工单来源","服务台工单来源。0=门户，1=邮件，2=电话，3=API",142),
            ("sys_warranty_type","保修类型","保修类型（与 TaktWarrantyType 一致）。0=原厂保修，1=延长保修，2=上门保修，3=寄修保修，4=维保合同，5=付费保养",143),
            ("sys_lifecycle_status","生命周期状态","通用文档/版本生命周期状态（SOP等共用）。1=编制中，2=审核中，3=已生效，4=已废止",144),
            ("sys_attachment_file_type","附件文件类型","通用多媒体附件类型（字典 sys_attachment_file_type；SOP/新闻/公告/通知/文管等共用）。1=图片，2=视频，3=文档",146),
            ("sys_workstation_type","工位类型","通用制造工位类型。1=装配，2=检验，3=包装，4=测试，5=其他",147),
            ("sys_iso_code_category","ISO编码类别","ISO编码类别（与 TaktIsoCode.IsoCodeCategory 一致，字典 sys_iso_code_category）。1=部门，2=公司，3=产品，4=通用",148),

            ("hr_benefit_category","福利大类","福利大类。1=保险，2=补贴，3=休假，4=其他",149),
            ("hr_benefit_type","福利类型","福利类型。1=社保，2=公积金，3=商业保险，4=年假额度，5=餐补，6=培训补贴，7=员工折扣",150),
            ("hr_benefit_payment_cycle_type","福利发放周期","福利发放周期。1=月度，2=季度，3=年度，4=一次性",151),
            ("hr_emp_benefit_plan_status","员工福利方案状态","员工福利方案状态。0=待生效，1=生效中，2=已失效",152),
            ("hr_comp_bonus_type","奖金类型","奖金类型。1=绩效奖金，2=项目奖金，3=年终奖金，4=专项奖金",153),
            ("hr_comp_bonus_calc_method_type","奖金计算方式","奖金计算方式。1=固定金额，2=按比例，3=按公式",154),
            ("hr_salary_item_type","薪资项目类型","薪资项目类型。1=基本工资，2=岗位工资，3=津贴，4=奖金，5=股权激励",155),
            ("hr_salary_calc_method_type","薪资计算方式","薪资计算方式。1=固定金额，2=按比例，3=按公式",156),
            ("hr_salary_formula_step_type","薪资公式步骤","薪资公式步骤。1=应发，2=社保个人，3=公积金个人，4=个税，5=实发",157),
            ("hr_social_insurance_pay_status","社保缴纳状态","社保缴纳状态。0=待缴纳，1=已缴纳，2=已补缴",158),
            ("logistics_process_segment_type","工艺段类型","工艺段类型。1=SMT，2=自插，3=手插，4=修正，5=总装",159),
            ("logistics_sop_andon_type","SOP安灯呼叫类型","SOP安灯呼叫类型。1=班长，2=维修，3=品质",160),
            ("logistics_sop_andon_status","SOP安灯呼叫状态","SOP安灯呼叫状态。1=待响应，2=已响应，3=已关闭",161),
            ("logistics_sop_exec_status","SOP执行状态","SOP执行状态。1=进行中，2=完成，3=中断",162),
            ("logistics_sop_check_result_type","SOP检验结果","SOP检验/工步结果。1=合格，2=不合格，3=不适用/跳过",163),
            ("logistics_sop_scan_result_type","SOP扫码结果","SOP扫码结果。1=PASS，2=NG",164),


        };
    }

    /// <summary>
    /// 创建或更新字典类型
    /// </summary>
    private static async Task<(TaktDictType DictType, int InsertCount, int UpdateCount)> CreateOrUpdateDictTypeAsync(
        ITaktTenantSeedRepository<TaktDictType> repository,
        string tenantCode,
        string dictTypeCode,
        string dictTypeName,
        string remark,
        int sortOrder)
    {
        var dictType = await repository.FirstAsync(d => d.TenantCode == tenantCode && d.DictTypeCode == dictTypeCode);
        
        if (dictType == null)
        {
            // 不存在：创建新记录（仓储会自动生成雪花ID和审计字段）
            dictType = new TaktDictType
            {
                TenantCode = tenantCode,
                DictTypeCode = dictTypeCode,
                DictTypeName = dictTypeName,
                Remark = remark,
                IsBuiltIn = 1,
                DataSource = 0,
                DictStatus = 1,
                SortOrder = sortOrder
            };
            dictType = await repository.CreateAsync(dictType);
            return (dictType, 1, 0);
        }
        else
        {
            // 存在：更新记录
            dictType.DictTypeName = dictTypeName;
            dictType.Remark = remark;
            dictType.SortOrder = sortOrder;

            await repository.UpdateAsync(dictType);
            return (dictType, 0, 1);
        }
    }
}
