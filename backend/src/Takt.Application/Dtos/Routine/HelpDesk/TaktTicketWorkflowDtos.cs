// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktTicketWorkflowDtos.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：工单 ITSM 工作流动作 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.HelpDesk;

/// <summary>
/// 门户/用户提交工单 DTO
/// </summary>
public class TaktTicketSubmitDto
{
    /// <summary>
    /// 工单标题
    /// </summary>
    [Required(ErrorMessage = "工单标题不能为空")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 工单内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 附件 JSON
    /// </summary>
    public string? AttachmentsJson { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { get; set; } = 1;

    /// <summary>
    /// 分类编码
    /// </summary>
    public string? CategoryCode { get; set; }

    /// <summary>
    /// 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    public string? AssetCode { get; set; }

    /// <summary>
    /// IT 设备保修扩展 ID（关联 TaktItAsset.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }

    /// <summary>
    /// 关联知识 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 邮件/API 建单 DTO
/// </summary>
public class TaktTicketCreateFromChannelDto : TaktTicketSubmitDto
{
    /// <summary>
    /// 工单来源
    /// </summary>
    public int TicketSource { get; set; } = 1;

    /// <summary>
    /// 外部邮件 Message-Id 或 API 幂等键（可选）
    /// </summary>
    public string? ExternalMessageId { get; set; }

    /// <summary>
    /// 提交人 ID（邮件/API 场景可指定；门户由当前用户填充）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SubmitterId { get; set; }

    /// <summary>
    /// 提交人姓名
    /// </summary>
    public string? SubmitterName { get; set; }
}

/// <summary>
/// 指派/领取工单 DTO
/// </summary>
public class TaktTicketAssignDto
{
    /// <summary>
    /// 工单 ID
    /// </summary>
    [Required]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 处理人 ID（为空则领取到当前用户）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人姓名
    /// </summary>
    public string? AssigneeName { get; set; }

    /// <summary>
    /// 领取后立即进入处理中
    /// </summary>
    public bool StartImmediately { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 工单通用动作 DTO（开始/等待/解决/确认/重开）
/// </summary>
public class TaktTicketWorkflowActionDto
{
    /// <summary>
    /// 工单 ID
    /// </summary>
    [Required]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 备注或原因
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 工单回复 DTO
/// </summary>
public class TaktTicketReplyCreateDto
{
    /// <summary>
    /// 工单 ID
    /// </summary>
    [Required]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 回复内容
    /// </summary>
    [Required(ErrorMessage = "回复内容不能为空")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 附件 JSON
    /// </summary>
    public string? AttachmentsJson { get; set; }

    /// <summary>
    /// 是否内部备注（仅客服）
    /// </summary>
    public bool IsInternal { get; set; }
}

/// <summary>
/// 工单回复响应 DTO
/// </summary>
public class TaktTicketReplyDto : TaktCompanyDtoBase
{
    /// <summary>
    /// 回复 ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketReplyId { get; set; }

    /// <summary>
    /// 工单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 作者类型
    /// </summary>
    public int AuthorType { get; set; }

    /// <summary>
    /// 作者 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AuthorId { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    public string? AuthorName { get; set; }

    /// <summary>
    /// 回复内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 附件 JSON
    /// </summary>
    public string? AttachmentsJson { get; set; }

    /// <summary>
    /// 是否内部备注
    /// </summary>
    public bool IsInternal { get; set; }
}

/// <summary>
/// 工单回复分页查询
/// </summary>
public class TaktTicketReplyQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 是否包含内部备注（客服可见）
    /// </summary>
    public bool IncludeInternal { get; set; } = true;
}

/// <summary>
/// 我的资产汇总 DTO（按工单 AssetCode 聚合，关联 TaktAsset）
/// </summary>
public class TaktTicketMyAssetDto
{
    /// <summary>
    /// 资产号码
    /// </summary>
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产名称（来自 TaktAsset）
    /// </summary>
    public string? AssetName { get; set; }

    /// <summary>
    /// 关联工单数量
    /// </summary>
    public int TicketCount { get; set; }

    /// <summary>
    /// 最近工单时间
    /// </summary>
    public DateTime? LastTicketAt { get; set; }
}

/// <summary>
/// 我的资产分页查询
/// </summary>
public class TaktTicketMyAssetQueryDto : TaktPagedQuery
{
}
