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
    /// 严格根据枚举定义生成，一一对应
    /// 前缀统一为 sys_，全部小写，全部内置
    /// 按 DictTypeCode 字母顺序排列
    /// </summary>
    private static List<(string DictTypeCode, string DictTypeName, string Remark, int SortOrder)> GetStandardDictTypes()
    {
        return new List<(string, string, string, int)>
        {
            ("accounting_account_category","科目类别","会计科目分类。用于财务科目管理",1),
            ("accounting_asset_category","资产类别","固定资产分类。用于资产管理",2),
            ("accounting_cost_center_category","成本中心类别","成本中心分类。用于成本核算管理",3),
            ("accounting_cost_element_category","成本要素类别","成本要素分类。用于成本要素管理",4),
            ("accounting_currency","币种","货币类型。国际贸易和财务结算通用币种",5),
            ("accounting_payment_terms","付款条件","付款条件。用于客户和供应商财务付款条款管理",6),
            ("accounting_profit_center_category","利润中心类别","利润中心分类。用于利润核算管理",7),
            ("accounting_tax_code","税码","税务编码。用于增值税等税务管理",8),
            ("accounting_tax_rate","税率","税率。用于各类税务计算",9),
            ("gen_button_category","代码生成操作后缀","对应 TaktGenTable.MenuButtonGroup；DictValue 为完整权限码第四段英文 key（前缀为三段规范化的 PermsPrefixCanonical，见 TaktGenTable.PermsPrefix 注释）；多选逗号；TaktCodeGenWorkflowService.BuildSqlMenuButtonRowsAsync 生成 basePerm:sfx 与 MenuL10nKey=common.button.*；DictLabel 为中文名。已合并原 sys_button_category。曾用类型名「按钮权限后缀」片面。原编码 gen_menu_button。",10),
            ("gen_button_style","操作按钮样式","代码生成表 TaktGenTable.FrontBtnStyle（front_btn_style）。0=文本，1=标准。原编码 sys_button_style。",11),
            ("gen_csharp_data_type","C#数据类型","代码生成列 CsharpDataType。对应 string、int、long、DateTime、decimal、bool、Guid 等。C# 数据类型。原编码 sys_csharp_type。",12),
            ("gen_display_type","显示类型","代码生成列 HtmlType/显示类型。input、select、checkbox 等。原编码 sys_display_type。",13),
            ("gen_frontend_form_layout","前端表单布局","代码生成表 FrontFormLayout。12=一行一列，24=一行两列。原编码 sys_frontend_style。",14),
            ("gen_frontend_ui","前端UI框架","代码生成表 FrontUi。1=element plus，2=ant design vue。原编码 sys_frontend_template。",15),
            ("gen_function","生成功能","生成功能。查询，新增，更新，删除，状态，排序，模板，导入，导出",16),
            ("gen_method","生成方式","代码生成方式。0=zip 压缩包，1=自定义路径，2=当前项目",17),
            ("gen_query_type","查询方式","代码生成列 QueryType。EQ/NE/GT/GTE/LT/LTE/LIKE/BETWEEN。原编码 sys_query_type。",18),
            ("gen_template_type","生成模板类型","生成模板类型。对应 TaktGenTable.GenTemplateCategory。crud=单表操作，tree=树表操作，sub=主子表操作",19),
            ("hr_attendance_correction_approval","补卡审批状态","补卡审批状态（与 TaktAttendanceCorrection.approval_status 一致）。0=草稿，1=待审，2=已通过，3=已驳回",20),
            ("hr_attendance_correction_kind","补卡类型","补卡类型（与 TaktAttendanceCorrection.correction_kind 一致）。1=上班，2=下班",21),
            ("hr_attendance_device_brand","考勤设备品牌","设备品牌（与多品牌 SDK 路由一致）。Hikvision=海康威视，Deli=得力，ZKTeco=中控",22),
            ("hr_attendance_device_status","考勤设备状态","设备状态（与 TaktAttendanceDevice.device_status 一致）。0=停用，1=正常，2=故障",23),
            ("hr_attendance_exception_handle_status","考勤异常处理状态","处理状态（与 TaktAttendanceException.handle_status 一致）。0=待处理，1=已处理，2=已忽略",24),
            ("hr_attendance_exception_type","考勤异常类型","异常类型（与 TaktAttendanceException.exception_type 一致）。1=上班缺卡，2=下班缺卡，3=迟到，4=早退，5=旷工，9=其他",25),
            ("hr_attendance_punch_source","打卡来源","打卡来源（与 TaktAttendancePunch.punch_source 一致）。0=后台录入，1=移动端，2=导入",26),
            ("hr_attendance_punch_type","打卡类型","打卡类型（与 TaktAttendancePunch.punch_type 一致）。1=上班，2=下班，3=外勤",27),
            ("hr_attendance_result_status","出勤状态","考勤日结出勤状态（与 TaktAttendanceResult.attendance_status 一致）。0=正常，1=迟到，2=早退，3=缺卡，4=旷工，5=加班",28),
            ("hr_attendance_verify_mode","考勤验证方式","验证方式（与 TaktAttendanceSource.verify_mode 一致）。0=未知，1=指纹，2=人脸，3=密码，4=卡",29),
            ("hr_delegate_mode","人事代理模式","部门/岗位/员工代理子表 delegate_mode。0=直接员工，1=部门规则，2=岗位规则",30),
            ("hr_employee_status","员工状态","员工状态（与 TaktEmployee.employee_status 一致）。1=试用期，2=正式，3=离职，4=退休",31),
            ("hr_education","学历","学历（与 TaktEmployee.education 一致）。1=高中及以下，2=大专，3=本科，4=硕士，5=博士",32),
            ("hr_ethnic_group","民族","民族（56 个民族）。DictValue=序号 1～56，与国家标准排序一致",33),
            ("hr_holiday_is_working_day","假日是否工作日","是否工作日（假日表 is_working_day）。0=非工作日，1=工作日，2=半天等；与 TaktHoliday.IsWorkingDay 一致",34),
            ("hr_holiday_type","假日类型","假日类型（假日表 holiday_type）。0=法定，1=调休，2=公司",35),
            ("hr_leave_status","请假状态","请假状态（与 TaktLeave.leave_status 一致）。0=草稿，1=审批中，2=已通过，3=已驳回，4=已撤回",36),
            ("hr_marital_status","婚姻状况","婚姻状况（与 TaktEmployee.marital_status 一致）。0=未婚，1=已婚，2=离异，3=丧偶",37),
            ("hr_native_place","籍贯","籍贯（省级行政区，GB 区划代码前两位+0000）。用于下拉选择与员工档案 native_place 对照",38),
            ("hr_overtime_status","加班状态","加班状态（与 TaktOvertime.overtime_status 一致）。0=草稿，1=已提交，2=已通过，3=已驳回",38),
            ("hr_overtime_type","加班类型","加班类型（与 TaktOvertime.overtime_type 一致）。0=工作日加班，1=休息日加班，2=法定节假日加班",39),
            ("hr_political_status","政治面貌","政治面貌（国家标准十三类）。0=群众，1=共青团员，2=中共党员，3=中共预备党员，4=民革党员，5=民盟盟员，6=民建会员，7=民进会员，8=农工党党员，9=致公党党员，10=九三学社社员，11=台盟盟员，12=无党派民主人士",40),
            ("hr_schedule_type","排班类别","排班类别（与 TaktShiftSchedule.schedule_type 一致）。0=部门，1=人员",41),
            ("hr_reassignment_status","调动审批状态","调动审批状态（与 TaktEmployeeReassignment.ApprovalStatus / TaktApprovalStatus 一致）。0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回",42),
            ("hr_reassignment_type","调动类型","员工调动类型。0=转岗，1=调岗",43),
            ("hr_talent_staffing_requirement_status","用人需求审批状态","用人需求审批状态（与 TaktTalentStaffingRequirement.ApprovalStatus 一致）。0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回",44),
            ("hr_talent_recruitment_plan_status","招聘计划审批状态","招聘计划审批状态（与 TaktTalentRecruitmentPlan.ApprovalStatus 一致）。0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回",45),
            ("hr_talent_job_posting_status","职位发布状态","职位发布状态（与 TaktTalentJobPosting.posting_status 一致）。0=草稿，1=招聘中，2=已暂停，3=已关闭",46),
            ("hr_talent_publish_channel","职位发布渠道","职位发布渠道（与 TaktTalentJobPosting.publish_channel 一致）。0=官网，1=招聘网站，2=内推，3=校园，9=其他",47),
            ("hr_talent_interview_status","面试安排状态","面试安排状态（与 TaktTalentInterview.interview_status 一致）。0=草稿，1=已安排，2=已完成，3=未通过，4=已取消",48),
            ("hr_talent_interview_round","面试轮次","面试轮次（与 TaktTalentInterview.interview_round 一致）。1=初试，2=复试，3=终试",49),
            ("hr_talent_offer_status","录用审批状态","录用审批状态（与 TaktTalentOffer.ApprovalStatus 一致）。0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回",50),
            ("hr_personnel_onboarding_status","入职待办状态","入职待办状态（与 TaktEmployeeOnboarding.todo_status 一致）。0=待办理，1=办理中，2=已完成，3=已取消",51),
            ("hr_joined_status","入职上岗审批状态","入职上岗审批状态（与 TaktEmployeeJoined.ApprovalStatus 一致）。0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回",52),
            ("hr_resignation_status","离职审批状态","离职审批状态（与 TaktEmployeeResignation.ApprovalStatus 一致）。0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回",53),
            ("hr_resignation_type","离职类型","离职类型（与 TaktEmployeeResignation.resignation_type 一致）。0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他",54),
            ("logistics_batch_management","批次管理标识","是否启用批次管理。0=否，1=是",44),
            ("logistics_bulk_material","散装物料标识","是否为散装物料。0=否，1=是",45),
            ("logistics_credit_rating","信用等级","信用等级评估。用于客户和供应商信用管理",46),
            ("logistics_customer_category","客户类别","客户分类类别。用于客户业务分类管理",47),
            ("logistics_cycle_counting","周期盘点标识","物料周期盘点类别。A=12月，B=6月，C=3月，D=1月",48),
            ("logistics_defect_level","缺点等级","缺点等级。用于质量缺陷严重程度分级",49),
            ("logistics_equipment_category","设备类别","设备分类类别。用于设备资产管理",50),
            ("logistics_grade_category","等级类别","通用等级类别。适用于供应商和客户分级管理",51),
            ("logistics_handling_plan","处理方案","问题处理方案。用于质量问题和设备故障处理",52),
            ("logistics_inhouse_production_days","自制生产天数","自制生产所需天数。2=2天，5=5天",53),
            ("logistics_inspection_category","检验类型","检验类型。用于质量检验业务分类",54),
            ("logistics_inspection_item_type","检验项目类型","检验项目类型。用于检验项目分类管理",55),
            ("logistics_inspection_method","检验方式","检验方式。用于检验执行方式管理",56),
            ("logistics_inspection_severity","检验严格度","检验严格度。用于抽样检验严格程度管理",57),
            ("logistics_inspection_tool","检验工具","检验工具类型。用于检验设备和工具管理",58),
            ("logistics_inspection_type","检验类别","物料检验类型。0=免检，1=必检",59),
            ("logistics_judgment_category","判定类别","检验判定类别。用于检验结果判定",60),
            ("logistics_maintenance_category","维护类别","设备维护类型。用于维护工单管理",61),
            ("logistics_material_group","物料组","通用物料组分类。用于物料品目组管理，包含螺丝、电容、电阻、IC、结构件、包材等分类",62),
            ("logistics_material_type","物料类型","通用物料类型。ABF=废料，CBAU=兼容设备，CH00=CH合同操作，CONT=看板容器，COUP=优惠券，DIEN=服务，EPA=设备包装，ERSA=备件，FERT=成品，FGTR=饮料，FHMI=生产资源/工具，FOOD=食品，FRIP=易腐品，HALB=半成品，HAWA=贸易货物，HERS=制造商部分，HIBE=经营供应，IBAU=维护装配，INTR=内部物料，KMAT=可配置物料，LEER=虚拟件，LEIH=可反复利用包装，LGUT=空零售，MODE=衣物，MPO=物料计划对象，NLAG=非存储物料，NOF1=非食品，PIPE=管线物料，PLAN=贸易货物计划，PROC=过程物料，PROD=产品组，ROH=原材料，UNBW=未估价物料，VERP=包装，VKHM=附加，VOLL=全部产品，WERB=产品目录，WERT=只有价值物料，WETT=竞争产品",63),
            ("logistics_planned_delivery_days","计划交货天数","供应商计划交货天数。7/30/60/90/120天",64),
            ("logistics_price_control","价格控制","物料价格控制方式。S=标准价格，V=移动平均价",65),
            ("logistics_price_type","价格类型","价格类型。用于客户和供应商价格管理",66),
            ("logistics_price_unit","价格单位","物料价格单位基数。1/10/100/1000",67),
            ("logistics_procurement_type","采购类别","物料采购类型。E=自制生产，F=外部采购，X=两种采购类型",68),
            ("logistics_sampling_scheme_type","抽样方案类型","抽样方案类型。用于抽样检验方案管理",69),
            ("logistics_special_procurement","特殊采购类别","特殊采购类型。10=寄售，30=外协加工，50=虚设品号",70),
            ("logistics_supplier_category","供应商类别","供应商分类类别。用于供应商业务分类管理",71),
            ("logistics_unit_of_measure","基本单位类别","物料基本计量单位类别。国际通用计量单位，包含SI单位及商业常用单位",72),
            ("logistics_valuation_class","评估类别","物料评估类别。7920=成品，Z300=原材料(CN)，Z790=半成品(CN)，Z792=成品(CN)",73),
            ("prod_aoi_inspection_line","AOI线别","AOI 检测线别，如 AOI1、AOI2 等。原编码 aoi_inspection_line。",74),
            ("prod_assy_location","Assy个所","组立个所。自插、部品、设计、修正、加工、手插、组立、SMT、其他",75),
            ("prod_defect_category","不良区分","组立不良区分/类别，用于不良明细分类。原编码 defect_category。",76),
            ("prod_defect_location","不良个所","不良个所/发生场所，用于不良明细发生位置。原编码 defect_location。",77),
            ("prod_ec_distinction","设变管理区分","设变管理区分。1=全仕向，2=部管，3=内部，4=技术。原编码 sys_ec_distinction。",78),
            ("prod_ec_status","设变状态","设变（ECN）改号状态。1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的。原编码 sys_ec_status。",79),
            ("prod_equipment_status","设备状态","设备状态。0=运行中，1=停机，2=维修中，3=故障，4=待报废，5=已报废",80),
            ("prod_equipment_type","设备类型","设备类型。0=生产设备，1=检测设备，2=包装设备，3=物流设备，4=辅助设备",81),
            ("prod_maintenance_type","维护类型","维护类型。0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他",82),
            ("prod_nonachievement_reason","未达成原因","未达成原因。清机、测试慢/测试修理机、修理试机、转机、人员欠缺、部品不良/欠料、ST差异大、仪器设备/设置/调试/检查/故障/切换、请假/旷工、其他、切换机种/仕向、组立慢/加工多/工程多/下机慢/作业困难/升级慢、改修、坏机多/不良多、人员借调、返工、下机慢、学习中/新人员学习/开会、正常",83),
            ("prod_pcb_location","PCB个所","PCB个所。翘脚、生锡、锡量過多、空焊、漏件、发黄、IC PIN竖立/浮高、连锡、異物付着、底下有部品、基板不良、红胶不良、反面、位置偏移、部品不良/破損、立碑、翻面、撞件、错料、侧立、反向、PCB不良、焊接不良、極性相違、多件、锡少等",84),
            ("prod_pcba_function_category","PCBA功能类别","PCBA 功能类别。A、ADOC、ANA、AUDIO、B、BOTTOM、BTICE、C、DSPL、ENC、FRONT、INPUT、IO、JACK、L、LCD、MAIN、PANEL、POWER、REAR、RMN-1、SATA、SEQ、SYS、TOP、USB。原编码 pcba_function_category。",85),
            ("prod_pcba_panel_category","PCBA板位类别","PCBA 板位类别。电源、前板、IO板等。原编码 pcba_panel_category。",86),
            ("prod_pcba_side_category","PCBA面别","PCBA 面别。B面、T面。原编码 pcba_side_category。",87),
            ("prod_shift_category","生产班别","生产班别。早、中、晚、白班、夜班",88),
            ("prod_stop_reason","停线原因","停线原因。切换停止时间、周会、其他、欠料、停电、班会、切换机种、早会、组立、学习、仪设、清洁",89),
            ("prod_visual_inspection_line","目视线别","目视检查线别，如 L1、L2 等。原编码 visual_inspection_line。",90),
            ("prod_warranty_status","保修状态","保修状态。0=无保修，1=保修期内，2=保修期外，3=延保中",91),
            ("production_work_center_type","工作中心类型","工作中心类型。按产品类别+工序类型划分，如Pro加工、Pro组立、ProSMT等",92),
            ("sys_data_scope","数据权限","数据范围。0=全部数据，1=本部门数据，2=本部门及以下数据，3=仅本人数据，4=自定义数据范围",93),
            ("sys_data_source","数据源","字典数据源（与 TaktDictType.data_source 一致）。0=系统表，1=SQL查询",94),
            ("sys_db_data_type","数据库数据类型","数据库数据类型。基于数据库的数据类型，如：varchar、int、datetime、decimal等。原编码 sys_db_type。",95),
            ("sys_dept_type","部门类型","部门类型。0=直接，1=间接",96),
            ("sys_file_category","文件分类","文件分类。0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他",97),
            ("sys_file_status","文件状态","文件状态。0=正常，1=已锁定，2=已归档，3=已删除",98),
            ("sys_flow_category","流程分类","流程分类。0=通用流程，1=业务流程，2=系统流程",99),
            ("sys_flow_status","流程状态","流程实例运行状态。0=运行中，1=已完成，2=已终止，3=已挂起，4=已撤回，5=草稿",100),
            ("sys_form_category","表单分类","表单分类。0=通用表单，1=业务表单，2=系统表单",101),
            ("sys_form_type","表单类型","表单类型。0=动态表单，1=静态表单，2=自定义表单",102),
            ("sys_ftp_provider","FTP服务提供商","FTP服务提供商类型。teac_cn=TEAC FTP中国（ftp.teac.com.cn），teac_jp=TEAC FTP日本（rosu2.teac.co.jp）",103),
            ("sys_is_builtin","是否内置","是否内置标志。1=是/内置，0=否/自定义",104),
            ("sys_is_default","是否默认","通用默认标志。1=是/默认，0=否/非默认",105),
            ("sys_is_public","是否公开","是否公开标志。0=公开，1=私有",106),
            ("sys_language_code","语言编码","语言编码。ISO 639-1/639-2，如：zh-CN、en-US",107),
            ("sys_leave_category","请假类型","请假类型（与请假表 leave_type 一致）。affair=事假，sick=病假，annual=年假，marriage=婚假，maternity=产假，paternity=陪产假，bereavement=丧假，compensatory=调休，personal=私假，other=其他，可扩展",108),
            ("sys_list_class","列表类名","列表类名。用于前端样式控制",109),
            ("sys_mail_status","邮件状态","邮件状态。0=草稿，1=已发送，2=发送失败，3=已撤回，4=定时发送中",110),
            ("sys_mail_type","邮件类型","邮件类型。0=普通邮件，1=系统邮件，2=通知邮件，3=提醒邮件",111),
            ("sys_menu_type","菜单类型","菜单类型。0=目录，1=菜单，2=按钮",112),
            ("sys_message_group","消息分组","消息分组。Chat=聊天，Notification=通知，Alert=提醒",113),
            ("sys_message_type","消息类型","消息类型。Text=文本，Image=图片，File=文件，System=系统消息",114),
            ("sys_news_category","新闻分类","新闻分类。0=公司新闻，1=行业动态，2=技术分享，3=产品发布，4=活动资讯，5=其他",115),
            ("sys_news_status","新闻状态","新闻状态。0=草稿，1=已发布，2=已撤回，3=已过期",116),
            ("sys_normal_disable","默认状态","通用默认状态。1=启用，0=禁用，2=锁定",117),
            ("sys_notice_status","公告状态","公告状态。0=草稿，1=已发布，2=已撤回，3=已过期",118),
            ("sys_notice_type","公告类型","公告类型。0=通知，1=公告，2=新闻，3=活动",119),
            ("sys_online_status","在线状态","在线状态。0=在线，1=离线，2=离开",120),
            ("sys_oper_type","操作类型","系统操作类型。1=新增，2=修改，3=删除，4=查询，5=导出，6=导入，7=授权，8=强退，9=生成代码，10=清空数据",121),
            ("sys_oss_provider","OSS提供商类型","OSS对象存储提供商类型。aliyun=阿里云OSS，tencent=腾讯云COS，huawei=华为云OBS，aws=AWS S3",122),
            ("sys_post_category","岗位类别","岗位类别。管理类、技术类、业务类、支持类",123),
            ("sys_post_level","岗位级别","岗位级别。1=初级，2=中级，3=高级，4=专家，5=资深",124),
            ("sys_priority","优先级","优先级。0=低，1=中，2=高，3=紧急",125),
            ("sys_publish_scope","发布范围","发布范围。0=全部，1=指定部门，2=指定用户，3=指定角色",126),
            ("sys_read_status","读取状态","读取状态。0=未读，1=已读",127),
            ("sys_resource_type","资源类型","资源类型。Frontend=前端，Backend=后端",128),
            ("sys_scheme_status","方案状态","流程/表单方案状态。0=草稿，1=已发布，2=已禁用",129),
            ("sys_setting_group","设置分组","设置分组。backend=后端，frontend=前端",130),
            ("sys_sort_type","排序类型","排序类型。asc=升序，desc=降序",131),
            ("sys_storage_directory","存储目录","存储目录。用于文件分类存储",132),
            ("sys_storage_naming","存储命名规则","存储命名规则。0=原文件+哈希值，1=自动生成，2=自定义",133),
            ("sys_storage_type","存储方式","存储方式。0=本地存储，1=OSS对象存储，2=FTP，3=其他",134),
            ("sys_urgency_level","紧急程度","是否紧急。0=一般，1=紧急，2=非常紧急",135),
            ("sys_user_gender","用户性别","用户性别。0=未知，1=男，2=女",136),
            ("sys_user_type","用户类型","用户类型。0=普通用户，1=管理员，2=超级管理员",137),
            ("sys_word_category","敏感词词性类别","敏感词分类（与 Takt.Domain.Entities.Foundation.TaktVocabulary.WordCategory 一致，字典 sys_word_category）。1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视",138),
            ("sys_word_filter_level","敏感词过滤等级","敏感词过滤等级（与 Takt.Domain.Entities.Foundation.TaktVocabulary.FilterLevel 一致，字典 sys_word_filter_level）。1=低，2=中，3=高",139),
            ("sys_yes_no","是否","通用布尔标志。1=是/启用，0=否/禁用",140),
            ("sys_culture_code","区域类别","区域文化编码（BCP47，对齐 TaktCulture.CultureCode；用于用户/公司/工厂 default_culture 下拉）。如 zh-CN、en-US、ja-JP、zh-HK",141),

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
                IsBuiltIn = TaktYesNo.Yes,
                DataSource = TaktDataSource.TableData,
                DictStatus = TaktCommonStatus.Enabled,
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
