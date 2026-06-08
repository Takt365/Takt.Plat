// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Code.Generator
// 文件名称：TaktGenTableDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：GenTable 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktGenTable 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Code.Generator;

// ========================================
// GenTable 响应 DTO
// ========================================

/// <summary>
/// Takt代码生成表配置实体
/// 对应前端 TaktGenTableDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktGenTableDto : TaktTenantDtoBase
{
    /// <summary>
    /// GenTableID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）
    /// </summary>
    public string DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表名（用于主子表）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 本表关联父表的外键名（用于主子表）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码字段（用于树形结构）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码字段（用于树形结构）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称字段（用于树形结构）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否在数据库中（1=是库表，0=不是库表）
    /// </summary>
    public int InDatabase { get; set; } = 0;

    /// <summary>
    /// 生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）
    /// </summary>
    public string GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    public string PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 是否生成仓储层（1=是，0=否）
    /// </summary>
    public int IsRepository { get; set; } = 0;

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能，JSON 格式。对象形式：{"查看":"View","新增":"Create","更新":"Update","删除":"Delete",...}，键为中文功能名、值为英文标识；也支持数组 ["查询","新增",...] 或逗号分隔。 <para><b>核心设计</b>：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：</para> <para>- Query → QueryDto（查询传输对象）</para> <para>- Create → CreateDto（创建传输对象）</para> <para>- Update → UpdateDto（更新传输对象）</para> <para>- Status → StatusDto（状态传输对象）</para> <para>- Sort → SortDto（排序传输对象）</para> <para>- Import → TemplateDto + ImportDto（模板+导入传输对象）</para> <para>- Export → ExportDto（导出传输对象）</para> <para>- 所有功能 → Dto（基础传输对象，包含所有字段）</para>
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）
    /// </summary>
    public int GenMethod { get; set; } = 0;

    /// <summary>
    /// 生成路径（默认为项目根目录）
    /// </summary>
    public string GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否生成菜单（1=是，0=否）
    /// </summary>
    public int IsGenMenu { get; set; } = 0;

    /// <summary>
    /// 上级菜单ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMenuId { get; set; }

    /// <summary>
    /// 上级菜单名称（填充字段）
    /// </summary>
    public string? ParentMenuName { get; set; }

    /// <summary>
    /// 是否生成翻译（1=是，0=否）
    /// </summary>
    public int IsGenTranslation { get; set; } = 0;

    /// <summary>
    /// 排序字段
    /// </summary>
    public string SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序类型（asc=升序，desc=降序）
    /// </summary>
    public string SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（1=element plus，2=ant design vue）
    /// </summary>
    public int FrontUi { get; set; } = 0;

    /// <summary>
    /// 前端表单布局（12=一行一列，24=一行两列）
    /// </summary>
    public int FrontFormLayout { get; set; } = 0;

    /// <summary>
    /// 前端操作按钮样式（0=文本，1=标准）
    /// </summary>
    public int FrontBtnStyle { get; set; } = 0;

    /// <summary>
    /// 是否生成代码（1=是，0=否）
    /// </summary>
    public int IsGenCode { get; set; } = 0;

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int GenCodeCount { get; set; } = 0;

    /// <summary>
    /// 是否使用tabs（1=是，0=否）
    /// </summary>
    public int IsUseTabs { get; set; } = 0;

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int TabsFieldCount { get; set; } = 0;

    /// <summary>
    /// 作者
    /// </summary>
    public string GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）
    /// （子表：TaktGenTableColumn）
    /// </summary>
    public List<TaktGenTableColumnDto>? Columns { get; set; }

}

// ========================================
// GenTable 查询 DTO
// ========================================

/// <summary>
/// GenTable 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktGenTableQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）
    /// </summary>
    public string? DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表名（用于主子表）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 本表关联父表的外键名（用于主子表）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码字段（用于树形结构）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码字段（用于树形结构）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称字段（用于树形结构）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否在数据库中（1=是库表，0=不是库表）
    /// </summary>
    public int? InDatabase { get; set; }

    /// <summary>
    /// 生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）
    /// </summary>
    public string? GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string? GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    public string? PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string? EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 是否生成仓储层（1=是，0=否）
    /// </summary>
    public int? IsRepository { get; set; }

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能，JSON 格式。对象形式：{"查看":"View","新增":"Create","更新":"Update","删除":"Delete",...}，键为中文功能名、值为英文标识；也支持数组 ["查询","新增",...] 或逗号分隔。 <para><b>核心设计</b>：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：</para> <para>- Query → QueryDto（查询传输对象）</para> <para>- Create → CreateDto（创建传输对象）</para> <para>- Update → UpdateDto（更新传输对象）</para> <para>- Status → StatusDto（状态传输对象）</para> <para>- Sort → SortDto（排序传输对象）</para> <para>- Import → TemplateDto + ImportDto（模板+导入传输对象）</para> <para>- Export → ExportDto（导出传输对象）</para> <para>- 所有功能 → Dto（基础传输对象，包含所有字段）</para>
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）
    /// </summary>
    public int? GenMethod { get; set; }

    /// <summary>
    /// 生成路径（默认为项目根目录）
    /// </summary>
    public string? GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否生成菜单（1=是，0=否）
    /// </summary>
    public int? IsGenMenu { get; set; }

    /// <summary>
    /// 上级菜单ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentMenuId { get; set; }

    /// <summary>
    /// 是否生成翻译（1=是，0=否）
    /// </summary>
    public int? IsGenTranslation { get; set; }

    /// <summary>
    /// 排序字段
    /// </summary>
    public string? SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序类型（asc=升序，desc=降序）
    /// </summary>
    public string? SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（1=element plus，2=ant design vue）
    /// </summary>
    public int? FrontUi { get; set; }

    /// <summary>
    /// 前端表单布局（12=一行一列，24=一行两列）
    /// </summary>
    public int? FrontFormLayout { get; set; }

    /// <summary>
    /// 前端操作按钮样式（0=文本，1=标准）
    /// </summary>
    public int? FrontBtnStyle { get; set; }

    /// <summary>
    /// 是否生成代码（1=是，0=否）
    /// </summary>
    public int? IsGenCode { get; set; }

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int? GenCodeCount { get; set; }

    /// <summary>
    /// 是否使用tabs（1=是，0=否）
    /// </summary>
    public int? IsUseTabs { get; set; }

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int? TabsFieldCount { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    public string? GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建GenTable DTO
// ========================================

/// <summary>
/// 创建GenTable DTO
/// </summary>
public class TaktGenTableCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）
    /// </summary>
    [Required(ErrorMessage = "数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）不能为空")]
    public string DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）
    /// </summary>
    [Required(ErrorMessage = "数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）不能为空")]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表名（用于主子表）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 本表关联父表的外键名（用于主子表）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码字段（用于树形结构）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码字段（用于树形结构）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称字段（用于树形结构）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否在数据库中（1=是库表，0=不是库表）
    /// </summary>
    public int InDatabase { get; set; } = 0;

    /// <summary>
    /// 生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）
    /// </summary>
    [Required(ErrorMessage = "生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）不能为空")]
    public string GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    [Required(ErrorMessage = "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）不能为空")]
    public string GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    [Required(ErrorMessage = "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。不能为空")]
    public string PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    [Required(ErrorMessage = "实体类名称（首字母大写，驼峰命名）不能为空")]
    public string EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 是否生成仓储层（1=是，0=否）
    /// </summary>
    public int IsRepository { get; set; } = 0;

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能，JSON 格式。对象形式：{"查看":"View","新增":"Create","更新":"Update","删除":"Delete",...}，键为中文功能名、值为英文标识；也支持数组 ["查询","新增",...] 或逗号分隔。 <para><b>核心设计</b>：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：</para> <para>- Query → QueryDto（查询传输对象）</para> <para>- Create → CreateDto（创建传输对象）</para> <para>- Update → UpdateDto（更新传输对象）</para> <para>- Status → StatusDto（状态传输对象）</para> <para>- Sort → SortDto（排序传输对象）</para> <para>- Import → TemplateDto + ImportDto（模板+导入传输对象）</para> <para>- Export → ExportDto（导出传输对象）</para> <para>- 所有功能 → Dto（基础传输对象，包含所有字段）</para>
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）
    /// </summary>
    public int GenMethod { get; set; } = 0;

    /// <summary>
    /// 生成路径（默认为项目根目录）
    /// </summary>
    [Required(ErrorMessage = "生成路径（默认为项目根目录）不能为空")]
    public string GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否生成菜单（1=是，0=否）
    /// </summary>
    public int IsGenMenu { get; set; } = 0;

    /// <summary>
    /// 上级菜单ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMenuId { get; set; }

    /// <summary>
    /// 是否生成翻译（1=是，0=否）
    /// </summary>
    public int IsGenTranslation { get; set; } = 0;

    /// <summary>
    /// 排序字段
    /// </summary>
    [Required(ErrorMessage = "排序字段不能为空")]
    public string SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序类型（asc=升序，desc=降序）
    /// </summary>
    [Required(ErrorMessage = "排序类型（asc=升序，desc=降序）不能为空")]
    public string SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（1=element plus，2=ant design vue）
    /// </summary>
    public int FrontUi { get; set; } = 0;

    /// <summary>
    /// 前端表单布局（12=一行一列，24=一行两列）
    /// </summary>
    public int FrontFormLayout { get; set; } = 0;

    /// <summary>
    /// 前端操作按钮样式（0=文本，1=标准）
    /// </summary>
    public int FrontBtnStyle { get; set; } = 0;

    /// <summary>
    /// 是否生成代码（1=是，0=否）
    /// </summary>
    public int IsGenCode { get; set; } = 0;

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int GenCodeCount { get; set; } = 0;

    /// <summary>
    /// 是否使用tabs（1=是，0=否）
    /// </summary>
    public int IsUseTabs { get; set; } = 0;

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int TabsFieldCount { get; set; } = 0;

    /// <summary>
    /// 作者
    /// </summary>
    [Required(ErrorMessage = "作者不能为空")]
    public string GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
    /// </summary>
    public List<TaktGenTableColumnCreateDto>? Columns { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新GenTable DTO
// ========================================

/// <summary>
/// 更新GenTable DTO
/// 继承 TaktGenTableCreateDto，添加 GenTableId 字段
/// </summary>
public class TaktGenTableUpdateDto : TaktGenTableCreateDto
{
    /// <summary>
    /// GenTableID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// GenTable 导入模板行 DTO
/// </summary>
public class TaktGenTableTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）
    /// </summary>
    public string? DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表名（用于主子表）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 本表关联父表的外键名（用于主子表）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码字段（用于树形结构）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码字段（用于树形结构）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称字段（用于树形结构）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否在数据库中（1=是库表，0=不是库表）
    /// </summary>
    public int? InDatabase { get; set; }

    /// <summary>
    /// 生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）
    /// </summary>
    public string? GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string? GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// GenTable 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktGenTableImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）
    /// </summary>
    public string? DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表名（用于主子表）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 本表关联父表的外键名（用于主子表）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码字段（用于树形结构）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码字段（用于树形结构）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称字段（用于树形结构）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否在数据库中（1=是库表，0=不是库表）
    /// </summary>
    public int? InDatabase { get; set; }

    /// <summary>
    /// 生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）
    /// </summary>
    public string? GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string? GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// GenTable 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktGenTableExportDto
{
    /// <summary>
    /// GenTableID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）
    /// </summary>
    public string DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表名（用于主子表）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 本表关联父表的外键名（用于主子表）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码字段（用于树形结构）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码字段（用于树形结构）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称字段（用于树形结构）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否在数据库中（1=是库表，0=不是库表）
    /// </summary>
    public int InDatabase { get; set; } = 0;

    /// <summary>
    /// 生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）
    /// </summary>
    public string GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    public string PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 是否生成仓储层（1=是，0=否）
    /// </summary>
    public int IsRepository { get; set; } = 0;

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能，JSON 格式。对象形式：{"查看":"View","新增":"Create","更新":"Update","删除":"Delete",...}，键为中文功能名、值为英文标识；也支持数组 ["查询","新增",...] 或逗号分隔。 <para><b>核心设计</b>：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：</para> <para>- Query → QueryDto（查询传输对象）</para> <para>- Create → CreateDto（创建传输对象）</para> <para>- Update → UpdateDto（更新传输对象）</para> <para>- Status → StatusDto（状态传输对象）</para> <para>- Sort → SortDto（排序传输对象）</para> <para>- Import → TemplateDto + ImportDto（模板+导入传输对象）</para> <para>- Export → ExportDto（导出传输对象）</para> <para>- 所有功能 → Dto（基础传输对象，包含所有字段）</para>
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）
    /// </summary>
    public int GenMethod { get; set; } = 0;

    /// <summary>
    /// 生成路径（默认为项目根目录）
    /// </summary>
    public string GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否生成菜单（1=是，0=否）
    /// </summary>
    public int IsGenMenu { get; set; } = 0;

    /// <summary>
    /// 上级菜单ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMenuId { get; set; }

    /// <summary>
    /// 是否生成翻译（1=是，0=否）
    /// </summary>
    public int IsGenTranslation { get; set; } = 0;

    /// <summary>
    /// 排序字段
    /// </summary>
    public string SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序类型（asc=升序，desc=降序）
    /// </summary>
    public string SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（1=element plus，2=ant design vue）
    /// </summary>
    public int FrontUi { get; set; } = 0;

    /// <summary>
    /// 前端表单布局（12=一行一列，24=一行两列）
    /// </summary>
    public int FrontFormLayout { get; set; } = 0;

    /// <summary>
    /// 前端操作按钮样式（0=文本，1=标准）
    /// </summary>
    public int FrontBtnStyle { get; set; } = 0;

    /// <summary>
    /// 是否生成代码（1=是，0=否）
    /// </summary>
    public int IsGenCode { get; set; } = 0;

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int GenCodeCount { get; set; } = 0;

    /// <summary>
    /// 是否使用tabs（1=是，0=否）
    /// </summary>
    public int IsUseTabs { get; set; } = 0;

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int TabsFieldCount { get; set; } = 0;

    /// <summary>
    /// 作者
    /// </summary>
    public string GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
