// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktMessageService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线消息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 在线消息应用服务
/// </summary>
public class TaktMessageService : TaktServiceBase, ITaktMessageService
{
    private readonly ITaktCompanyRepository<TaktMessage> _messageRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="messageRepository">在线消息仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMessageService(
        ITaktCompanyRepository<TaktMessage> messageRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _messageRepository = messageRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取在线消息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMessageDto>> GetMessageListAsync(TaktMessageQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (items, total) = await _messageRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            orderBy: x => x.CreatedAt,
            isDesc: true);
        var dtos = items.Adapt<List<TaktMessageDto>>();
        return TaktPagedResult<TaktMessageDto>.Create(dtos, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto?> GetMessageByIdAsync(long id)
    {
        var entity = await _messageRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMessageDto>();
    }

    /// <summary>
    /// 获取在线消息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMessageOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _messageRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.FromUserName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FromUserName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建在线消息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto> CreateMessageAsync(TaktMessageCreateDto dto)
    {
        var entity = dto.Adapt<TaktMessage>();
        entity = await _messageRepository.CreateAsync(entity);
        return await GetMessageByIdAsync(entity.Id) ?? entity.Adapt<TaktMessageDto>();
    }

    /// <summary>
    /// 更新在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto> UpdateMessageAsync(long id, TaktMessageUpdateDto dto)
    {
        var entity = await _messageRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("在线消息不存在");
        }
        dto.Adapt(entity);
        await _messageRepository.UpdateAsync(entity);
        return await GetMessageByIdAsync(id) ?? throw new TaktBusinessException("在线消息不存在");
    }

    /// <summary>
    /// 删除在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMessageByIdAsync(long id)
    {
        var deleted = await _messageRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("在线消息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除在线消息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMessageBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMessageByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新在线消息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto> UpdateMessageStatusAsync(TaktMessageStatusDto dto)
    {
        var entity = await _messageRepository.GetByIdAsync(dto.MessageId);
        if (entity == null)
        {
            throw new TaktBusinessException("在线消息不存在");
        }
        entity.ReadStatus = dto.ReadStatus;
        if (dto.ReadStatus == TaktMessageReadStatus.Read && !entity.ReadTime.HasValue)
        {
            entity.ReadTime = DateTime.Now;
        }
        await _messageRepository.UpdateAsync(entity);
        return await GetMessageByIdAsync(dto.MessageId) ?? throw new TaktBusinessException("在线消息不存在");
    }

    /// <summary>
    /// 标记在线消息为已读（SignalR Hub 调用）
    /// </summary>
    /// <param name="messageId">消息 ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMessageDto> UpdateMessageReadStatusAsync(long messageId)
    {
        return await UpdateMessageStatusAsync(new TaktMessageStatusDto
        {
            MessageId = messageId,
            ReadStatus = TaktMessageReadStatus.Read,
        });
    }

    /// <summary>
    /// 获取指定用户未读消息数量（SignalR Hub 调用）
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns>未读数量</returns>
    public async Task<int> GetUnreadMessageCountAsync(string userName)
    {
        EnsureThreeLayerContext();
        return await _messageRepository.CountAsync(message =>
            message.TenantCode == CurrentTenantCode
            && message.CompanyCode == CurrentCompanyCode
            && message.ToUserName == userName
            && message.ReadStatus == TaktMessageReadStatus.Unread);
    }

    /// <summary>
    /// 导出在线消息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMessageAsync(TaktMessageQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMessageQueryDto());
        var list = await _messageRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMessageExportDto>(),
                sheetName ?? "在线消息数据",
                fileName ?? "在线消息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMessageExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "在线消息数据",
            fileName ?? "在线消息导出.xlsx");
    }

    /// <summary>
    /// 获取当前登录用户在线消息统计（接收消息：总数/已读/未读）
    /// </summary>
    /// <returns>统计 DTO</returns>
    public Task<TaktMessageStatisticsDto> GetMessageStatisticsAsync()
    {
        EnsureThreeLayerContext();
        return GetMessageStatisticsByUserNameAsync(RequireCurrentUserName(), CurrentUserId);
    }

    /// <summary>
    /// 获取指定用户在线消息统计（SignalR 实时推送调用）
    /// </summary>
    /// <param name="userName">用户名（接收者）</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>统计 DTO</returns>
    public async Task<TaktMessageStatisticsDto> GetMessageStatisticsByUserNameAsync(string userName, long? userId = null)
    {
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new TaktBusinessException("用户名不能为空");
        }

        var normalizedUserName = userName.Trim();

        var totalCount = await _messageRepository.CountAsync(message =>
            message.TenantCode == CurrentTenantCode
            && message.CompanyCode == CurrentCompanyCode
            && message.ToUserName == normalizedUserName);
        var readCount = await _messageRepository.CountAsync(message =>
            message.TenantCode == CurrentTenantCode
            && message.CompanyCode == CurrentCompanyCode
            && message.ToUserName == normalizedUserName
            && message.ReadStatus == TaktMessageReadStatus.Read);
        var unreadCount = await _messageRepository.CountAsync(message =>
            message.TenantCode == CurrentTenantCode
            && message.CompanyCode == CurrentCompanyCode
            && message.ToUserName == normalizedUserName
            && message.ReadStatus == TaktMessageReadStatus.Unread);

        return new TaktMessageStatisticsDto
        {
            UserName = normalizedUserName,
            UserId = userId ?? CurrentUserId,
            TotalCount = totalCount,
            ReadCount = readCount,
            UnreadCount = unreadCount,
        };
    }

    /// <summary>
    /// 解析并校验当前登录用户名
    /// </summary>
    /// <returns>用户名</returns>
    private string RequireCurrentUserName()
    {
        if (string.IsNullOrWhiteSpace(CurrentUserName))
        {
            throw new TaktBusinessException("无法解析当前登录用户");
        }

        return CurrentUserName.Trim();
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建在线消息查询表达式
    /// </summary>
    private Expression<Func<TaktMessage, bool>> QueryExpression(TaktMessageQueryDto queryDto)
    {
        return message => message.TenantCode == CurrentTenantCode
                    && message.CompanyCode == CurrentCompanyCode
                    && (string.IsNullOrEmpty(queryDto.KeyWords)
                        || (message.FromUserName != null && message.FromUserName.Contains(queryDto.KeyWords))
                        || (message.ToUserName != null && message.ToUserName.Contains(queryDto.KeyWords))
                        || (message.MessageTitle != null && message.MessageTitle.Contains(queryDto.KeyWords))
                        || (message.MessageContent != null && message.MessageContent.Contains(queryDto.KeyWords))
                        || (message.MessageExtData != null && message.MessageExtData.Contains(queryDto.KeyWords)))
                    && (string.IsNullOrEmpty(queryDto.FromUserName) || (message.FromUserName != null && message.FromUserName.Contains(queryDto.FromUserName)))
                    && (!queryDto.FromUserId.HasValue || message.FromUserId == queryDto.FromUserId.Value)
                    && (string.IsNullOrEmpty(queryDto.ToUserName) || (message.ToUserName != null && message.ToUserName.Contains(queryDto.ToUserName)))
                    && (!queryDto.ToUserId.HasValue || message.ToUserId == queryDto.ToUserId.Value)
                    && (string.IsNullOrEmpty(queryDto.MessageTitle) || (message.MessageTitle != null && message.MessageTitle.Contains(queryDto.MessageTitle)))
                    && (string.IsNullOrEmpty(queryDto.MessageContent) || (message.MessageContent != null && message.MessageContent.Contains(queryDto.MessageContent)))
                    && (!queryDto.MessageType.HasValue || message.MessageType == queryDto.MessageType.Value)
                    && (!queryDto.MessageGroup.HasValue || message.MessageGroup == queryDto.MessageGroup.Value)
                    && (!queryDto.ReadStatus.HasValue || message.ReadStatus == queryDto.ReadStatus.Value)
                    && (string.IsNullOrEmpty(queryDto.MessageExtData) || (message.MessageExtData != null && message.MessageExtData.Contains(queryDto.MessageExtData)))
                    && (string.IsNullOrEmpty(queryDto.ExtFieldJson) || (message.ExtFieldJson != null && message.ExtFieldJson.Contains(queryDto.ExtFieldJson)))
                    && (string.IsNullOrEmpty(queryDto.Remark) || (message.Remark != null && message.Remark.Contains(queryDto.Remark)))
                    && (!queryDto.ReadTimeStart.HasValue || message.ReadTime >= queryDto.ReadTimeStart.Value)
                    && (!queryDto.ReadTimeEnd.HasValue || message.ReadTime <= queryDto.ReadTimeEnd.Value)
                    && (!queryDto.SendTimeStart.HasValue || message.SendTime >= queryDto.SendTimeStart.Value)
                    && (!queryDto.SendTimeEnd.HasValue || message.SendTime <= queryDto.SendTimeEnd.Value)
                    && (!queryDto.CreatedAtStart.HasValue || message.CreatedAt >= queryDto.CreatedAtStart.Value)
                    && (!queryDto.CreatedAtEnd.HasValue || message.CreatedAt <= queryDto.CreatedAtEnd.Value);
    }
}
