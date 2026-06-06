// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Repositories
// 文件名称：ITaktTenantRepository.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：租户级仓储接口，仅过滤 TenantCode
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Entities;

namespace Takt.Domain.Repositories;

/// <summary>
/// 租户级仓储接口
/// 仅过滤 TenantCode，不涉及 CompanyCode
/// 适用于：用户、角色、菜单等跨公司共享的实体
/// </summary>
/// <typeparam name="TEntity">租户级实体类型</typeparam>
public interface ITaktTenantRepository<TEntity> : ITaktUniqueExistenceRepository<TEntity> where TEntity : TaktTenantEntityBase, new()
{
    // ========================================
    // 基础查询
    // ========================================

    /// <summary>
    /// 根据ID查询实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> GetByIdAsync(long id);

    /// <summary>
    /// 根据条件查询单个实体
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>实体对象</returns>
    Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 查询所有实体（带租户过滤）
    /// </summary>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// 根据条件查询列表
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 根据条件查询列表（带排序）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="orderBy">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, bool isDesc = true);

    /// <summary>
    /// 导出用条件查询（带上限行数上限，防止全表加载 OOM）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="maxRows">最大行数；为空时使用 <c>Excel:Export:MaxRowsPerRequest</c> 配置</param>
    /// <returns>实体列表</returns>
    Task<List<TEntity>> GetListForExportAsync(Expression<Func<TEntity, bool>> predicate, int? maxRows = null);

    // ========================================
    // 分页查询
    // ========================================

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="pageIndex">页码（从1开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <returns>分页结果</returns>
    Task<(List<TEntity> Items, int Total)> GetPagedAsync(int pageIndex, int pageSize);

    /// <summary>
    /// 分页查询（带条件，默认按 CreatedAt 降序）
    /// </summary>
    /// <param name="pageIndex">页码（从1开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="predicate">查询条件</param>
    /// <returns>分页结果</returns>
    Task<(List<TEntity> Items, int Total)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 分页查询（带条件）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="pageIndex">页码（从1开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="orderBy">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns>分页结果</returns>
    Task<(List<TEntity> Items, int Total)> GetPagedAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool isDesc = true);

    // ========================================
    // 新增操作
    // ========================================

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>创建的实体</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// 批量创建实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>创建的实体数量</returns>
    Task<int> CreateRangeAsync(List<TEntity> entities);

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>更新的实体数量</returns>
    Task<int> UpdateRangeAsync(List<TEntity> entities);

    /// <summary>
    /// 根据条件更新
    /// </summary>
    /// <param name="predicate">更新条件</param>
    /// <param name="columns">更新字段</param>
    /// <returns>更新的实体数量</returns>
    Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> columns);

    // ========================================
    // 删除操作
    // ========================================

    /// <summary>
    /// 软删除实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 根据条件软删除
    /// </summary>
    /// <param name="predicate">删除条件</param>
    /// <returns>删除的实体数量</returns>
    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    // ========================================
    // 存在性检查
    // ========================================

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(long id);

    /// <summary>
    /// 统计符合条件的记录数
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <returns>记录数</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

    // ========================================
    // 序列与只读脚本
    // ========================================

    /// <summary>
    /// 按条件取整型字段最大值（当前租户范围内、未删除）
    /// </summary>
    /// <param name="predicate">查询条件</param>
    /// <param name="fieldSelector">整型字段（如 SortOrder、行号字段）</param>
    /// <returns>最大值；无记录时为 0</returns>
    Task<int> GetMaxIntAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int>> fieldSelector);

    /// <summary>
    /// 执行只读 SQL 并返回动态行（调用方须先经 <see cref="Takt.Shared.Validation.TaktSqlExecutorValidator"/> 校验）
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行列表</returns>
    Task<IReadOnlyList<Dictionary<string, object>>> QueryReadOnlySqlAsync(
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default);

    // ========================================
    // 事务操作
    // ========================================

    /// <summary>
    /// 开启事务
    /// </summary>
    void BeginTran();

    /// <summary>
    /// 提交事务
    /// </summary>
    void CommitTran();

    /// <summary>
    /// 回滚事务
    /// </summary>
    void RollbackTran();
}
