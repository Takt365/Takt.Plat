// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktSeedRepository.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：种子数据专用仓储实现（三个实体基类共用），绕过用户上下文，支持精确租户控制和雪花ID
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 租户级种子数据专用仓储实现
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 根据配置处理主键类型
/// </summary>
/// <typeparam name="TEntity">实体类型（必须继承TaktTenantEntityBase）</typeparam>
public class TaktTenantSeedRepository<TEntity> : ITaktTenantSeedRepository<TEntity> where TEntity : TaktTenantEntityBase, new()
{
    /// <summary>
    /// 种子数据数据库上下文
    /// </summary>
    private readonly TaktSeedContext _dbContext;

    /// <summary>
    /// 主键类型配置
    /// </summary>
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// 数据库客户端
    /// </summary>
    protected ISqlSugarClient Db => _dbContext.Db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">种子数据上下文</param>
    /// <param name="idRuleOptions">主键类型配置</param>
    public TaktTenantSeedRepository(TaktSeedContext dbContext, Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> idRuleOptions)
    {
        _dbContext = dbContext;
        _primaryKeyTypeOptions = idRuleOptions.Value;
    }

    // ========================================
    // 新增操作（根据配置处理主键类型）
    // ========================================

    /// <summary>
    /// 创建实体（根据实体主键字段类型自动判断 ID 生成策略）
    /// </summary>
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        // 统一填充审计字段（种子数据使用 900001）
        var now = DateTime.Now;
        entity.CreatedAt = now;
        entity.CreatedBy = 900001L;
        
        // 根据实体主键字段类型自动判断 ID 生成策略
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty != null)
        {
            var idType = idProperty.PropertyType;
            
            if (idType == typeof(long) && _primaryKeyTypeOptions.Snowflake.Enabled)
            {
                // long 类型 = 雪花ID
                await Db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
            }
            else if (idType == typeof(Guid) && _primaryKeyTypeOptions.Guid.Enabled)
            {
                // Guid 类型 = GUID
                if ((Guid)idProperty.GetValue(entity)! == Guid.Empty)
                {
                    idProperty.SetValue(entity, Guid.NewGuid());
                }
                await Db.Insertable(entity).ExecuteCommandAsync();
            }
            else if ((idType == typeof(int) || idType == typeof(int?)) && _primaryKeyTypeOptions.Identity.Enabled)
            {
                // int 类型 = 数据库自增
                await Db.Insertable(entity).ExecuteCommandAsync();
            }
            else
            {
                // 默认处理
                await Db.Insertable(entity).ExecuteCommandAsync();
            }
        }
        else
        {
            await Db.Insertable(entity).ExecuteCommandAsync();
        }
        
        return entity;
    }

    /// <summary>
    /// 批量创建实体（根据配置处理主键类型和审计字段）
    /// </summary>
    public virtual async Task<int> CreateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        
        var now = DateTime.Now;
        
        // 统一填充审计字段（种子数据使用 900001）
        foreach (var entity in entities)
        {
            entity.CreatedAt = now;
            entity.CreatedBy = 900001L;
        }
        
        // 根据配置的主键类型处理 ID
        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            // 雪花ID：使用 SqlSugar 官方方法自动填充
            var ids = await Db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync();
            return ids.Count;
        }
        else
        {
            // 数据库自增或 GUID：不需要手动设置 ID
            return await Db.Insertable(entities).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 批量创建实体（大批量插入优化，根据配置处理主键类型和审计字段）
    /// </summary>
    public virtual async Task<int> CreateRangeBulkAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        
        var now = DateTime.Now;
        
        // 统一填充审计字段（种子数据使用 900001）
        foreach (var entity in entities)
        {
            entity.CreatedAt = now;
            entity.CreatedBy = 900001L;
        }
        
        // 根据配置的主键类型处理 ID
        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            // 雪花ID：使用 SqlSugar 官方方法自动填充
            var ids = await Db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync();
            return ids.Count;
        }
        else
        {
            // 数据库自增或 GUID：不需要手动设置 ID
            return await Db.Insertable(entities).ExecuteCommandAsync();
        }
    }

    // ========================================
    // 更新操作
    // ========================================

    /// <summary>
    /// 更新实体（自动填充审计字段）
    /// </summary>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        // 统一填充更新审计字段（种子数据使用 900001）
        entity.UpdatedAt = DateTime.Now;
        entity.UpdatedBy = 900001L;
        
        return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 批量更新实体（自动填充审计字段）
    /// </summary>
    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        
        var now = DateTime.Now;
        
        // 为每个实体填充更新审计字段（种子数据使用 900001）
        foreach (var entity in entities)
        {
            entity.UpdatedAt = now;
            entity.UpdatedBy = 900001L;
        }
        
        return await Db.Updateable(entities).ExecuteCommandAsync();
    }

    // ========================================
    // 查询操作（不自动添加租户过滤）
    // ========================================

    /// <summary>
    /// 根据ID查询实体（不自动添加租户过滤）
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Db.Queryable<TEntity>()
            .Where(x => x.Id == id)
            .Where(x => x.IsDeleted == 0)
            .FirstAsync();
    }

    /// <summary>
    /// 根据条件查询单个实体（不自动添加租户过滤）
    /// </summary>
    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.IsDeleted == 0)
            .FirstAsync();
    }

    /// <summary>
    /// 根据条件查询实体列表（不自动添加租户过滤）
    /// </summary>
    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.IsDeleted == 0)
            .ToListAsync();
    }

    /// <summary>
    /// 检查实体是否存在
    /// </summary>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>()
            .Where(predicate)
            .Where(x => x.IsDeleted == 0)
            .AnyAsync();
    }
}


// ========================================
// 公司级种子数据专用仓储实现
// ========================================

/// <summary>
/// 公司级种子数据专用仓储实现
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 根据配置处理主键类型
/// </summary>
/// <typeparam name="TEntity">实体类型（必须继承TaktCompanyEntityBase）</typeparam>
public class TaktCompanySeedRepository<TEntity> : ITaktCompanySeedRepository<TEntity> where TEntity : TaktCompanyEntityBase, new()
{
    private readonly TaktSeedContext _dbContext;
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;
    protected ISqlSugarClient Db => _dbContext.Db;

    public TaktCompanySeedRepository(TaktSeedContext dbContext, Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions)
    {
        _dbContext = dbContext;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var now = DateTime.Now;
        entity.CreatedAt = now;
        entity.CreatedBy = 900001L;
        
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty != null)
        {
            var idType = idProperty.PropertyType;
            if (idType == typeof(long) && _primaryKeyTypeOptions.Snowflake.Enabled)
                await Db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
            else if (idType == typeof(Guid) && _primaryKeyTypeOptions.Guid.Enabled)
            {
                if ((Guid)idProperty.GetValue(entity)! == Guid.Empty)
                    idProperty.SetValue(entity, Guid.NewGuid());
                await Db.Insertable(entity).ExecuteCommandAsync();
            }
            else if ((idType == typeof(int) || idType == typeof(int?)) && _primaryKeyTypeOptions.Identity.Enabled)
                await Db.Insertable(entity).ExecuteCommandAsync();
            else
                await Db.Insertable(entity).ExecuteCommandAsync();
        }
        else
        {
            await Db.Insertable(entity).ExecuteCommandAsync();
        }
        return entity;
    }

    public virtual async Task<int> CreateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.CreatedAt = now;
            entity.CreatedBy = 900001L;
        }
        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            var ids = await Db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync();
            return ids.Count;
        }
        return await Db.Insertable(entities).ExecuteCommandAsync();
    }

    public virtual async Task<int> CreateRangeBulkAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.CreatedAt = now;
            entity.CreatedBy = 900001L;
        }
        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            var ids = await Db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync();
            return ids.Count;
        }
        return await Db.Insertable(entities).ExecuteCommandAsync();
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        entity.UpdatedAt = DateTime.Now;
        entity.UpdatedBy = 900001L;
        return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
    }

    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.UpdatedAt = now;
            entity.UpdatedBy = 900001L;
        }
        return await Db.Updateable(entities).ExecuteCommandAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Db.Queryable<TEntity>().Where(x => x.Id == id).Where(x => x.IsDeleted == 0).FirstAsync();
    }

    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).FirstAsync();
    }

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).AnyAsync();
    }
}

// ========================================
// 审批级种子数据专用仓储实现
// ========================================

/// <summary>
/// 审批级种子数据专用仓储实现
/// 特点：
/// 1. 不依赖 ITaktUserContext（种子数据阶段无 HTTP 上下文）
/// 2. 不自动添加租户/公司过滤（由调用方显式指定）
/// 3. 支持精确控制租户和公司编码
/// 4. 根据配置处理主键类型
/// </summary>
/// <typeparam name="TEntity">实体类型（必须继承TaktApprovalEntityBase）</typeparam>
public class TaktApprovalSeedRepository<TEntity> : ITaktApprovalSeedRepository<TEntity> where TEntity : TaktApprovalEntityBase, new()
{
    private readonly TaktSeedContext _dbContext;
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;
    protected ISqlSugarClient Db => _dbContext.Db;

    public TaktApprovalSeedRepository(TaktSeedContext dbContext, Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions)
    {
        _dbContext = dbContext;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var now = DateTime.Now;
        entity.CreatedAt = now;
        entity.CreatedBy = 900001L;
        
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty != null)
        {
            var idType = idProperty.PropertyType;
            if (idType == typeof(long) && _primaryKeyTypeOptions.Snowflake.Enabled)
                await Db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
            else if (idType == typeof(Guid) && _primaryKeyTypeOptions.Guid.Enabled)
            {
                if ((Guid)idProperty.GetValue(entity)! == Guid.Empty)
                    idProperty.SetValue(entity, Guid.NewGuid());
                await Db.Insertable(entity).ExecuteCommandAsync();
            }
            else if ((idType == typeof(int) || idType == typeof(int?)) && _primaryKeyTypeOptions.Identity.Enabled)
                await Db.Insertable(entity).ExecuteCommandAsync();
            else
                await Db.Insertable(entity).ExecuteCommandAsync();
        }
        else
        {
            await Db.Insertable(entity).ExecuteCommandAsync();
        }
        return entity;
    }

    public virtual async Task<int> CreateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.CreatedAt = now;
            entity.CreatedBy = 900001L;
        }
        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            var ids = await Db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync();
            return ids.Count;
        }
        return await Db.Insertable(entities).ExecuteCommandAsync();
    }

    public virtual async Task<int> CreateRangeBulkAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.CreatedAt = now;
            entity.CreatedBy = 900001L;
        }
        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            var ids = await Db.Insertable(entities).ExecuteReturnSnowflakeIdListAsync();
            return ids.Count;
        }
        return await Db.Insertable(entities).ExecuteCommandAsync();
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        entity.UpdatedAt = DateTime.Now;
        entity.UpdatedBy = 900001L;
        return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
    }

    public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
    {
        if (entities.Count == 0) return 0;
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.UpdatedAt = now;
            entity.UpdatedBy = 900001L;
        }
        return await Db.Updateable(entities).ExecuteCommandAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Db.Queryable<TEntity>().Where(x => x.Id == id).Where(x => x.IsDeleted == 0).FirstAsync();
    }

    public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).FirstAsync();
    }

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Db.Queryable<TEntity>().Where(predicate).Where(x => x.IsDeleted == 0).AnyAsync();
    }
}