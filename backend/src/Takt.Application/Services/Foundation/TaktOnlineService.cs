// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktOnlineService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线用户应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
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
/// 在线用户应用服务
/// </summary>
public class TaktOnlineService : TaktServiceBase, ITaktOnlineService
{
    private readonly ITaktCompanyRepository<TaktOnline> _onlineRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="onlineRepository">在线用户仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktOnlineService(
        ITaktCompanyRepository<TaktOnline> onlineRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _onlineRepository = onlineRepository;
    }

    /// <summary>
    /// 获取在线用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktOnlineDto>> GetOnlineListAsync(TaktOnlineQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (items, total) = await _onlineRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            orderBy: x => x.CreatedAt,
            isDesc: true);
        var dtos = items.Adapt<List<TaktOnlineDto>>();
        return TaktPagedResult<TaktOnlineDto>.Create(dtos, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktOnlineDto?> GetOnlineByIdAsync(long id)
    {
        var entity = await _onlineRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktOnlineDto>();
    }

    /// <summary>
    /// 获取在线用户选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOnlineOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _onlineRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.UserName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.UserName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 注册 SignalR 在线会话（同租户+公司+用户复用主记录；其它仍 Online 的会话标为离线）
    /// </summary>
    /// <param name="dto">连接信息</param>
    /// <returns>在线用户 DTO</returns>
    public async Task<TaktOnlineDto> RegisterOnlineSessionAsync(TaktOnlineCreateDto dto)
    {
        EnsureThreeLayerContext();
        if (!dto.UserId.HasValue || dto.UserId.Value <= 0)
        {
            throw new TaktBusinessException("无法解析在线用户 ID");
        }

        if (string.IsNullOrWhiteSpace(dto.ConnectionId))
        {
            throw new TaktBusinessException("ConnectionId 不能为空");
        }

        var userId = dto.UserId.Value;
        var userRecords = await _onlineRepository.GetListAsync(online =>
            online.TenantCode == CurrentTenantCode
            && online.CompanyCode == CurrentCompanyCode
            && online.UserId == userId);

        var connectLocation = TaktLocationHelper.ResolveIpLocationForLogOrKeep(dto.ConnectIp, dto.ConnectLocation);
        TaktOnline entity;

        if (userRecords.Count > 0)
        {
            entity = userRecords
                .OrderByDescending(online => online.OnlineStatus == 0)
                .ThenByDescending(online => online.ConnectTime)
                .First();

            await MarkStaleOnlineSessionsOfflineAsync(userRecords, entity.Id);

            entity.ConnectionId = dto.ConnectionId.Trim();
            entity.UserName = dto.UserName.Trim();
            entity.OnlineStatus = 0;
            entity.ConnectIp = dto.ConnectIp;
            entity.ConnectLocation = connectLocation;
            entity.UserAgent = dto.UserAgent;
            entity.DeviceType = dto.DeviceType;
            entity.BrowserType = dto.BrowserType;
            entity.OperatingSystem = dto.OperatingSystem;
            entity.ConnectTime = dto.ConnectTime;
            entity.LastActiveTime = dto.ConnectTime;
            entity.DisconnectTime = null;
            entity.ConnectionDuration = null;
            await _onlineRepository.UpdateAsync(entity);
        }
        else
        {
            entity = dto.Adapt<TaktOnline>();
            entity.ConnectLocation = connectLocation;
            entity = await _onlineRepository.CreateAsync(entity);
        }

        return await GetOnlineByIdAsync(entity.Id) ?? entity.Adapt<TaktOnlineDto>();
    }

    /// <summary>
    /// 删除在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>任务</returns>
    public async Task DeleteOnlineByIdAsync(long id)
    {
        EnsureThreeLayerContext();
        var entity = await _onlineRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("在线用户不存在或已删除");
        }

        ApplyOfflineSessionState(entity, DateTime.Now);
        await _onlineRepository.UpdateAsync(entity);

        var deleted = await _onlineRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("在线用户不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除在线用户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteOnlineBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteOnlineByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新在线用户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOnlineDto> UpdateOnlineStatusAsync(TaktOnlineStatusDto dto)
    {
        var entity = await _onlineRepository.GetByIdAsync(dto.OnlineId);
        if (entity == null)
        {
            throw new TaktBusinessException("在线用户不存在");
        }
        entity.OnlineStatus = dto.OnlineStatus;
        await _onlineRepository.UpdateAsync(entity);
        return await GetOnlineByIdAsync(dto.OnlineId) ?? throw new TaktBusinessException("在线用户不存在");
    }

    /// <summary>
    /// 导出在线用户
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportOnlineAsync(TaktOnlineQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktOnlineQueryDto());
        var list = await _onlineRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktOnlineExportDto>(),
                sheetName ?? "在线用户数据",
                fileName ?? "在线用户导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktOnlineExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "在线用户数据",
            fileName ?? "在线用户导出.xlsx");
    }

    /// <summary>
    /// 获取当前登录用户在线统计（在线连接数、在线时长：当前/当天/当月）
    /// </summary>
    /// <returns>统计 DTO</returns>
    public Task<TaktOnlineStatisticsDto> GetOnlineStatisticsAsync()
    {
        EnsureThreeLayerContext();
        return GetOnlineStatisticsByUserNameAsync(RequireCurrentUserName(), CurrentUserId);
    }

    /// <summary>
    /// 获取指定用户在线统计（SignalR 实时推送调用）
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>统计 DTO</returns>
    public async Task<TaktOnlineStatisticsDto> GetOnlineStatisticsByUserNameAsync(string userName, long? userId = null)
    {
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new TaktBusinessException("用户名不能为空");
        }

        var normalizedUserName = userName.Trim();
        var now = DateTime.Now;
        var todayStart = DateTime.Today;
        var monthStart = new DateTime(now.Year, now.Month, 1);

        var onlineUsers = await _onlineRepository.GetListAsync(online =>
            online.TenantCode == CurrentTenantCode
            && online.CompanyCode == CurrentCompanyCode
            && online.UserName == normalizedUserName
            && online.OnlineStatus == 0);

        var durationRecords = await _onlineRepository.GetListAsync(online =>
            online.TenantCode == CurrentTenantCode
            && online.CompanyCode == CurrentCompanyCode
            && online.UserName == normalizedUserName
            && (online.OnlineStatus == 0
                || (online.DisconnectTime.HasValue && online.DisconnectTime.Value >= monthStart)));

        var currentDurationSeconds = onlineUsers.Sum(online => (long)(now - online.ConnectTime).TotalSeconds);

        return new TaktOnlineStatisticsDto
        {
            UserName = normalizedUserName,
            UserId = userId ?? CurrentUserId,
            OnlineCount = onlineUsers.Count,
            CurrentDurationSeconds = currentDurationSeconds,
            TodayDurationSeconds = SumOnlineDurationSeconds(durationRecords, todayStart, now),
            MonthDurationSeconds = SumOnlineDurationSeconds(durationRecords, monthStart, now),
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

    /// <summary>
    /// 将同用户除主记录外的仍在线会话标为离线
    /// </summary>
    /// <param name="userRecords">同用户全部在线记录</param>
    /// <param name="primaryRecordId">当前复用的主记录 ID</param>
    /// <returns>任务</returns>
    private async Task MarkStaleOnlineSessionsOfflineAsync(IReadOnlyList<TaktOnline> userRecords, long primaryRecordId)
    {
        var disconnectTime = DateTime.Now;
        foreach (var stale in userRecords.Where(online => online.Id != primaryRecordId))
        {
            if (stale.OnlineStatus != 0)
            {
                continue;
            }

            ApplyOfflineSessionState(stale, disconnectTime);
            await _onlineRepository.UpdateAsync(stale);
        }
    }

    /// <summary>
    /// 将会话标记为离线
    /// </summary>
    /// <param name="online">在线用户实体</param>
    /// <param name="disconnectTime">断开时间</param>
    private static void ApplyOfflineSessionState(TaktOnline online, DateTime disconnectTime)
    {
        if (online.OnlineStatus != 0)
        {
            return;
        }

        online.DisconnectTime = disconnectTime;
        online.ConnectionDuration = (int)(disconnectTime - online.ConnectTime).TotalSeconds;
        online.OnlineStatus = 1;
    }

    /// <summary>
    /// 累计指定时间范围内各会话的有效在线时长（秒）
    /// </summary>
    /// <param name="records">在线用户记录</param>
    /// <param name="rangeStart">统计区间起点（含）</param>
    /// <param name="now">统计截止时刻</param>
    /// <returns>累计时长（秒）</returns>
    private static long SumOnlineDurationSeconds(IEnumerable<TaktOnline> records, DateTime rangeStart, DateTime now)
    {
        long totalSeconds = 0;
        foreach (var online in records)
        {
            var sessionEnd = online.OnlineStatus == 0
                ? now
                : online.DisconnectTime ?? now;
            if (sessionEnd <= rangeStart)
            {
                continue;
            }

            var effectiveStart = online.ConnectTime > rangeStart ? online.ConnectTime : rangeStart;
            totalSeconds += (long)(sessionEnd - effectiveStart).TotalSeconds;
        }

        return totalSeconds;
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建在线用户查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktOnline, bool>> QueryExpression(TaktOnlineQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktOnline>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ConnectionId != null && x.ConnectionId.Contains(keywords))
                || (x.UserName != null && x.UserName.Contains(keywords))
                || SqlFunc.ToString(x.UserId).Contains(keywords)
                || SqlFunc.ToString(x.OnlineStatus).Contains(keywords)
                || (x.ConnectIp != null && x.ConnectIp.Contains(keywords))
                || (x.ConnectLocation != null && x.ConnectLocation.Contains(keywords))
                || (x.UserAgent != null && x.UserAgent.Contains(keywords))
                || SqlFunc.ToString(x.DeviceType).Contains(keywords)
                || SqlFunc.ToString(x.BrowserType).Contains(keywords)
                || SqlFunc.ToString(x.OperatingSystem).Contains(keywords)
                || SqlFunc.ToString(x.ConnectionDuration).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ConnectTime).Contains(keywords)
                || SqlFunc.ToString(x.LastActiveTime).Contains(keywords)
                || SqlFunc.ToString(x.DisconnectTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ConnectionId))
        {
            exp = exp.And(x => x.ConnectionId != null && x.ConnectionId.Contains(queryDto.ConnectionId));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (queryDto?.UserId.HasValue == true)
        {
            exp = exp.And(x => x.UserId == queryDto.UserId);
        }

        if (queryDto?.OnlineStatus.HasValue == true)
        {
            exp = exp.And(x => x.OnlineStatus == queryDto.OnlineStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ConnectIp))
        {
            exp = exp.And(x => x.ConnectIp != null && x.ConnectIp.Contains(queryDto.ConnectIp));
        }

        if (!string.IsNullOrEmpty(queryDto?.ConnectLocation))
        {
            exp = exp.And(x => x.ConnectLocation != null && x.ConnectLocation.Contains(queryDto.ConnectLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserAgent))
        {
            exp = exp.And(x => x.UserAgent != null && x.UserAgent.Contains(queryDto.UserAgent));
        }

        if (queryDto?.DeviceType.HasValue == true)
        {
            exp = exp.And(x => x.DeviceType == queryDto.DeviceType);
        }

        if (queryDto?.BrowserType.HasValue == true)
        {
            exp = exp.And(x => x.BrowserType == queryDto.BrowserType);
        }

        if (queryDto?.OperatingSystem.HasValue == true)
        {
            exp = exp.And(x => x.OperatingSystem == queryDto.OperatingSystem);
        }

        if (queryDto?.ConnectionDuration.HasValue == true)
        {
            exp = exp.And(x => x.ConnectionDuration == queryDto.ConnectionDuration);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ConnectTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ConnectTime >= queryDto.ConnectTimeStart);
        }

        if (queryDto?.ConnectTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ConnectTime <= queryDto.ConnectTimeEnd);
        }

        if (queryDto?.LastActiveTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.LastActiveTime >= queryDto.LastActiveTimeStart);
        }

        if (queryDto?.LastActiveTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastActiveTime <= queryDto.LastActiveTimeEnd);
        }

        if (queryDto?.DisconnectTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.DisconnectTime >= queryDto.DisconnectTimeStart);
        }

        if (queryDto?.DisconnectTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.DisconnectTime <= queryDto.DisconnectTimeEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
