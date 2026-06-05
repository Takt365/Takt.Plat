// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktApprovalRepository.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：审批仓储实现，独立仓储，提供审批操作
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Enums;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 审批仓储实现（独立仓储，不继承其他仓储）
/// 适用于：请假单、报销单、采购单、合同等需要审批的实体
/// 注意：审批实体使用 TaktApprovalEntityBase 基类，包含 TenantCode 和 CompanyCode 字段
/// </summary>
/// <typeparam name="TEntity">审批实体类型</typeparam>
public class TaktApprovalRepository<TEntity> : ITaktApprovalRepository<TEntity> where TEntity : TaktApprovalEntityBase, new()
{
    /// <summary>
    /// SqlSugar 数据库上下文
    /// </summary>
    private readonly TaktSqlSugarContext _dbContext;

    /// <summary>
    /// 用户上下文
    /// </summary>
    private readonly ITaktUserContext _userContext;

    /// <summary>
    /// 主键类型配置
    /// </summary>
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// 当前租户编码
    /// </summary>
    protected string CurrentTenantCode => _userContext.TenantCode ?? string.Empty;

    /// <summary>
    /// 当前公司编码
    /// </summary>
    protected string CurrentCompanyCode => _userContext.CompanyCode ?? string.Empty;

    /// <summary>
    /// 当前用户ID
    /// </summary>
    protected long? CurrentUserId => _userContext.UserId;

    /// <summary>
    /// 数据库客户端
    /// </summary>
    protected ISqlSugarClient Db => _dbContext.Db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="primaryKeyTypeOptions">主键类型配置</param>
    public TaktApprovalRepository(
        TaktSqlSugarContext dbContext,
        ITaktUserContext userContext,
        Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
    }

    // ========================================
    // 基础查询
    // ========================================

    /// <summary>
    /// 根据ID查询实体
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Db.Queryable<TEntity>()
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .FirstAsync();
    }

    /// <summary>
    /// 根据条件查询单个实体
    /// </summary>
    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .FirstAsync();
    }

    /// <summary>
    /// 查询所有实体（带租户和公司过滤）
    /// </summary>
    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await Db.Queryable<TEntity>()
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 根据条件查询列表
    /// </summary>
    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 根据条件查询列表（带排序）
    /// </summary>
    public virtual async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>> orderBy,
        bool isDesc = true)
    {
        var query = Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0);

        return isDesc
            ? await query.OrderByDescending(orderBy).ToListAsync()
            : await query.OrderBy(orderBy).ToListAsync();
    }

    // ========================================
    // 分页查询
    // ========================================

    /// <summary>
    /// 分页查询
    /// </summary>
    public virtual async Task<(List<TEntity> Items, int Total)> GetPagedAsync(int pageIndex, int pageSize)
    {
        var filterQuery = Db.Queryable<TEntity>()
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0);

        var total = await filterQuery.CountAsync();
        var items = await filterQuery
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    /// <summary>
    /// 分页查询（带条件，默认按 CreatedAt 降序）
    /// </summary>
    public virtual Task<(List<TEntity> Items, int Total)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate) =>
        GetPagedAsync(predicate, pageIndex, pageSize, x => x.CreatedAt, isDesc: true);

    /// <summary>
    /// 分页查询（带条件）
    /// </summary>
    public virtual async Task<(List<TEntity> Items, int Total)> GetPagedAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool isDesc = true)
    {
        var filterQuery = Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0);

        var total = await filterQuery.CountAsync();

        var pageQuery = filterQuery;
        if (orderBy != null)
        {
            pageQuery = isDesc ? pageQuery.OrderByDescending(orderBy) : pageQuery.OrderBy(orderBy);
        }
        else
        {
            pageQuery = pageQuery.OrderByDescending(x => x.CreatedAt);
        }

        var items = await pageQuery
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    // ========================================
    // 新增操作
    // ========================================

    /// <summary>
    /// 创建实体
    /// </summary>
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        // 自动设置租户和公司编码(仅在未设置时才自动填充)
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = CurrentTenantCode;
        }

        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = CurrentCompanyCode;
        }

        // 自动设置审计字段
        var now = DateTime.Now;
        entity.CreatedAt = now;
        entity.UpdatedAt = now;
        entity.CreatedBy = CurrentUserId ?? 0;
        entity.UpdatedBy = CurrentUserId ?? 0;
        entity.IsDeleted = 0;

        // 根据配置的主键类型处理 ID
        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            await Db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
        }
        else
        {
            await Db.Insertable(entity).ExecuteCommandAsync();
        }

        return entity;
    }

    /// <summary>
    /// 批量创建实体
    /// </summary>
    public virtual async Task<int> CreateRangeAsync(List<TEntity> entities)
    {
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            // 自动设置租户和公司编码(仅在未设置时才自动填充)
            if (string.IsNullOrEmpty(entity.TenantCode))
            {
                entity.TenantCode = CurrentTenantCode;
            }

            if (string.IsNullOrEmpty(entity.CompanyCode))
            {
                entity.CompanyCode = CurrentCompanyCode;
            }

            entity.CreatedAt = now;
            entity.UpdatedAt = now;
            entity.CreatedBy = CurrentUserId ?? 0;
            entity.UpdatedBy = CurrentUserId ?? 0;
            entity.IsDeleted = 0;
        }

        // 根据配置的主键类型处理 ID
        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            var ids = await Db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync();
            return ids.Count;
        }
        else
        {
            return await Db.Insertable(entities).ExecuteCommandAsync();
        }
    }

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体
    /// </summary>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        // 自动设置审计字段
        entity.UpdatedAt = DateTime.Now;
        entity.UpdatedBy = CurrentUserId ?? 0;

        var rows = await Db.Updateable(entity).ExecuteCommandAsync();
        return rows > 0;
    }

    /// <summary>
    /// 批量更新实体
    /// </summary>
    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
    {
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.UpdatedAt = now;
            entity.UpdatedBy = CurrentUserId ?? 0;
        }

        return await Db.Updateable(entities).ExecuteCommandAsync();
    }

    // ========================================
    // 删除操作
    // ========================================

    /// <summary>
    /// 软删除实体
    /// </summary>
    public virtual async Task<bool> DeleteAsync(long id)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity 
            { 
                IsDeleted = 1, 
                UpdatedAt = DateTime.Now,
                UpdatedBy = CurrentUserId ?? 0
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .ExecuteCommandAsync();

        return rows > 0;
    }

    /// <summary>
    /// 根据条件软删除
    /// </summary>
    public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity 
            { 
                IsDeleted = 1, 
                UpdatedAt = DateTime.Now,
                UpdatedBy = CurrentUserId ?? 0
            })
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .ExecuteCommandAsync();
    }

    // ========================================
    // 存在性检查
    // ========================================

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    public virtual async Task<bool> ExistsAsync(long id)
    {
        return await Db.Queryable<TEntity>()
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .AnyAsync();
    }

    /// <summary>
    /// 根据条件检查是否存在
    /// </summary>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .AnyAsync();
    }

    /// <summary>
    /// 统计符合条件的记录数
    /// </summary>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = Db.Queryable<TEntity>()
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode);

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.Where(x => x.IsDeleted == 0).CountAsync();
    }

    // ========================================
    // 序列与只读脚本
    // ========================================

    /// <summary>
    /// 按条件取整型字段最大值（当前租户与公司范围内、未删除）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="fieldSelector">整型字段</param>
    /// <returns>最大值；无记录时为 0</returns>
    public virtual async Task<int> GetMaxIntAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int>> fieldSelector)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.IsDeleted == 0)
            .MaxAsync(fieldSelector);
    }

    /// <summary>
    /// 执行只读 SQL 并返回动态行
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行列表</returns>
    public virtual Task<IReadOnlyList<Dictionary<string, object>>> QueryReadOnlySqlAsync(
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default) =>
        TaktRepositoryReadOnlySql.QueryAsync(Db, sql, parameters, cancellationToken);

    // ========================================
    // 事务管理
    // ========================================

    /// <summary>
    /// 开始事务
    /// </summary>
    public void BeginTran()
    {
        Db.Ado.BeginTran();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTran()
    {
        Db.Ado.CommitTran();
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTran()
    {
        Db.Ado.RollbackTran();
    }

    // ========================================
    // 审批操作（专属方法）
    // ========================================

    /// <summary>
    /// 提交审批
    /// </summary>
    public virtual async Task<bool> SubmitForApprovalAsync(long id, long submitterId)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                ApprovalStatus = TaktApprovalStatus.InProgress,
                InitiatorId = submitterId,
                InitiatedAt = DateTime.Now,
                UpdatedBy = submitterId,
                UpdatedAt = DateTime.Now
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.ApprovalStatus == TaktApprovalStatus.Pending)
            .ExecuteCommandAsync();

        return rows > 0;
    }

    /// <summary>
    /// 审批通过
    /// </summary>
    public virtual async Task<bool> ApproveAsync(long id, long approverId, string? opinion = null)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                ApprovalStatus = TaktApprovalStatus.Approved,
                ApprovalOpinion = opinion,
                ApprovedBy = approverId,
                ApprovedAt = DateTime.Now,
                UpdatedBy = approverId,
                UpdatedAt = DateTime.Now
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.ApprovalStatus == TaktApprovalStatus.InProgress)
            .ExecuteCommandAsync();

        return rows > 0;
    }

    /// <summary>
    /// 审批驳回
    /// </summary>
    public virtual async Task<bool> RejectAsync(long id, long approverId, string opinion)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                ApprovalStatus = TaktApprovalStatus.Rejected,
                ApprovalOpinion = opinion,
                ApprovedBy = approverId,
                ApprovedAt = DateTime.Now,
                UpdatedBy = approverId,
                UpdatedAt = DateTime.Now
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.ApprovalStatus == TaktApprovalStatus.InProgress)
            .ExecuteCommandAsync();

        return rows > 0;
    }

    /// <summary>
    /// 撤销审批
    /// </summary>
    public virtual async Task<bool> CancelApprovalAsync(long id, long cancellerId, string? opinion = null)
    {
        var rows = await Db.Updateable<TEntity>()
            .SetColumns(x => new TEntity
            {
                ApprovalStatus = TaktApprovalStatus.Cancelled,
                ApprovalOpinion = opinion,
                UpdatedBy = cancellerId,
                UpdatedAt = DateTime.Now
            })
            .Where(x => x.Id == id)
            .Where(x => x.TenantCode == CurrentTenantCode)
            .Where(x => x.CompanyCode == CurrentCompanyCode)
            .Where(x => x.ApprovalStatus == TaktApprovalStatus.InProgress ||
                        x.ApprovalStatus == TaktApprovalStatus.Pending)
            .ExecuteCommandAsync();

        return rows > 0;
    }
}
