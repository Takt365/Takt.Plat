// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktRbacService.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：RBAC 八张关联表统一分配服务（【查询】列表 + 【分配】全量覆盖：先查询旧关联、软删除、再新增）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using Takt.Application.Dtos.HumanResource.Organization;
using Takt.Application.Dtos.Identity;
using Takt.Domain.Entities;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Identity;

/// <summary>
/// RBAC 关联分配应用服务（八张关联表：TaktUserRole、TaktUserTenant、TaktUserCompany、TaktRoleMenu、TaktRoleCompany、TaktRoleDept、TaktEmployeeDept、TaktEmployeePost）。
/// 对外提供【查询】当前关联列表；分配接口统一为【分配】全量覆盖（【查询】旧数据 → 【删除】软删除 → 【新增】批量插入）。
/// </summary>
public class TaktRbacService : TaktServiceBase, ITaktRbacService
{
    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktTenantRepository<TaktUserRole> _userRoleRepository;
    private readonly ITaktTenantRepository<TaktRole> _roleRepository;
    private readonly ITaktTenantRepository<TaktUserTenant> _userTenantRepository;
    private readonly ITaktCompanyRepository<TaktUserCompany> _userCompanyRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyCatalogRepository;
    private readonly ITaktTenantRepository<TaktRoleMenu> _roleMenuRepository;
    private readonly ITaktCompanyRepository<TaktRoleCompany> _roleCompanyRepository;
    private readonly ITaktCompanyRepository<TaktRoleDept> _roleDeptRepository;
    private readonly ITaktTenantRepository<TaktMenu> _menuRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeDept> _employeeDeptRepository;
    private readonly ITaktCompanyRepository<TaktEmployeePost> _employeePostRepository;
    private readonly ITaktCompanyRepository<TaktDept> _deptRepository;
    private readonly ITaktCompanyRepository<TaktPost> _postRepository;
    private readonly ITaktTenantRepository<TaktTenant> _tenantRepository;

    /// <summary>
    /// 初始化 RBAC 关联分配服务
    /// </summary>
    /// <param name="userRepository">用户仓储（【查询】用户主数据，分配前校验）</param>
    /// <param name="userRoleRepository">用户-角色关联仓储（【查询】/【删除】/【新增】）</param>
    /// <param name="roleRepository">角色仓储（【查询】角色主数据，分配前校验）</param>
    /// <param name="userTenantRepository">用户-租户关联仓储（【查询】/【删除】/【新增】）</param>
    /// <param name="userCompanyRepository">用户-公司关联仓储（【查询】/【删除】/【新增】）</param>
    /// <param name="companyCatalogRepository">公司主数据仓储（【查询】校验 CompanyCode 是否存在且启用）</param>
    /// <param name="roleMenuRepository">角色-菜单关联仓储（【查询】/【删除】/【新增】）</param>
    /// <param name="roleCompanyRepository">角色-公司关联仓储（【查询】/【删除】/【新增】）</param>
    /// <param name="roleDeptRepository">角色-部门关联仓储（【查询】/【删除】/【新增】）</param>
    /// <param name="menuRepository">菜单仓储（【查询】菜单主数据，分配前校验）</param>
    /// <param name="employeeRepository">员工仓储（【查询】员工主数据，分配前校验）</param>
    /// <param name="employeeDeptRepository">员工-部门关联仓储（【查询】/【删除】/【新增】）</param>
    /// <param name="employeePostRepository">员工-岗位关联仓储（【查询】/【删除】/【新增】）</param>
    /// <param name="deptRepository">部门仓储（【查询】部门主数据，列表填充名称）</param>
    /// <param name="postRepository">岗位仓储（【查询】岗位主数据，列表填充名称）</param>
    /// <param name="tenantRepository">租户仓储（【查询】租户主数据，分配前校验）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktRbacService(
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktTenantRepository<TaktUserRole> userRoleRepository,
        ITaktTenantRepository<TaktRole> roleRepository,
        ITaktTenantRepository<TaktUserTenant> userTenantRepository,
        ITaktCompanyRepository<TaktUserCompany> userCompanyRepository,
        ITaktTenantRepository<TaktCompany> companyCatalogRepository,
        ITaktTenantRepository<TaktRoleMenu> roleMenuRepository,
        ITaktCompanyRepository<TaktRoleCompany> roleCompanyRepository,
        ITaktCompanyRepository<TaktRoleDept> roleDeptRepository,
        ITaktTenantRepository<TaktMenu> menuRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktCompanyRepository<TaktEmployeeDept> employeeDeptRepository,
        ITaktCompanyRepository<TaktEmployeePost> employeePostRepository,
        ITaktCompanyRepository<TaktDept> deptRepository,
        ITaktCompanyRepository<TaktPost> postRepository,
        ITaktTenantRepository<TaktTenant> tenantRepository,
        ITaktUserContext userContext,
        ITaktLocalizationService localizationService)
        : base(userContext, localizationService)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _userTenantRepository = userTenantRepository;
        _userCompanyRepository = userCompanyRepository;
        _companyCatalogRepository = companyCatalogRepository;
        _roleMenuRepository = roleMenuRepository;
        _roleCompanyRepository = roleCompanyRepository;
        _roleDeptRepository = roleDeptRepository;
        _menuRepository = menuRepository;
        _employeeRepository = employeeRepository;
        _employeeDeptRepository = employeeDeptRepository;
        _employeePostRepository = employeePostRepository;
        _deptRepository = deptRepository;
        _postRepository = postRepository;
        _tenantRepository = tenantRepository;
    }

    #region 用户角色授权（TaktUserRole）

    /// <summary>
    /// 【查询】获取用户角色关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户-角色关联 DTO 列表</returns>
    public async Task<List<TaktUserRoleDto>> GetUserRoleIdsAsync(long userId)
    {
        // 【校验】用户存在且属于当前租户
        var user = await EnsureUserAccessibleAsync(userId);

        // 【查询】用户-角色关联表
        var list = await _userRoleRepository.GetListAsync(x => x.UserId == userId);
        var dtos = list.Adapt<List<TaktUserRoleDto>>();

        if (dtos.Count == 0)
        {
            return dtos;
        }

        // 【查询】角色主数据，填充 RoleName
        var roleIds = dtos.Select(d => d.RoleId).Distinct().ToList();
        var roles = await _roleRepository.GetListAsync(r => roleIds.Contains(r.Id));
        var roleNameMap = roles.ToDictionary(r => r.Id, r => r.RoleName);

        foreach (var dto in dtos)
        {
            dto.UserName = user.Username;
            if (roleNameMap.TryGetValue(dto.RoleId, out var roleName))
            {
                dto.RoleName = roleName;
            }
        }

        return dtos;
    }

    /// <summary>
    /// 批量获取用户角色名称（导出用；UserId → 逗号分隔角色名）
    /// </summary>
    /// <param name="userIds">用户 ID 集合</param>
    /// <returns>用户 ID 到角色名称的映射</returns>
    public async Task<IReadOnlyDictionary<long, string>> GetUserRoleNamesMapAsync(IEnumerable<long> userIds)
    {
        var idList = userIds?.Where(id => id > 0).Distinct().ToList() ?? [];
        if (idList.Count == 0)
        {
            return new Dictionary<long, string>();
        }
        var userRoles = await _userRoleRepository.GetListAsync(x => idList.Contains(x.UserId));
        if (userRoles.Count == 0)
        {
            return new Dictionary<long, string>();
        }
        var roleIds = userRoles.Select(ur => ur.RoleId).Distinct().ToList();
        var roles = await _roleRepository.GetListAsync(r => roleIds.Contains(r.Id));
        var roleNameMap = roles.ToDictionary(r => r.Id, r => r.RoleName ?? string.Empty);
        return userRoles
            .GroupBy(ur => ur.UserId)
            .ToDictionary(
                g => g.Key,
                g => string.Join(
                    ", ",
                    g.Select(ur => roleNameMap.TryGetValue(ur.RoleId, out var name) ? name : string.Empty)
                        .Where(name => !string.IsNullOrWhiteSpace(name))));
    }

    /// <summary>
    /// 【分配】用户角色全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="roleIds">角色ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignUserRolesAsync(long userId, long[] roleIds)
    {
        // 【校验】用户存在且属于当前租户
        await EnsureUserAccessibleAsync(userId);

        var roleIdList = roleIds?.Distinct().ToList() ?? [];

        // 【查询】校验角色主数据是否全部存在
        if (roleIdList.Count > 0)
        {
            var roles = await _roleRepository.GetListAsync(r => roleIdList.Contains(r.Id));
            if (roles.Count != roleIdList.Count)
            {
                ThrowBusinessException("部分角色不存在或不可用");
            }
        }

        // 【新增】构建待插入的关联实体（实际写入在 ReplaceTenantAssociationsAsync 内完成）
        var entities = roleIdList.Select(roleId => new TaktUserRole
        {
            UserId = userId,
            RoleId = roleId,
        }).ToList();

        // 【分配】查询 → 删除 → 新增
        await ReplaceTenantAssociationsAsync(
            _userRoleRepository,
            x => x.UserId == userId,
            entities,
            $"用户角色 UserId={userId}");
        return true;
    }

    #endregion

    #region 用户租户与公司范围（TaktUserTenant、TaktUserCompany）

    /// <summary>
    /// 【查询】获取用户租户关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户-租户关联 DTO 列表</returns>
    public async Task<List<TaktUserTenantDto>> GetUserTenantIdsAsync(long userId)
    {
        // 【校验】用户存在且属于当前租户
        var user = await EnsureUserAccessibleAsync(userId);

        // 【查询】用户-租户关联表（默认租户优先排序）
        var list = await _userTenantRepository.GetListAsync(
            x => x.UserId == userId,
            x => x.IsDefault,
            true);

        var dtos = list.Adapt<List<TaktUserTenantDto>>();
        foreach (var dto in dtos)
        {
            dto.UserName = user.Username;
        }

        return dtos;
    }

    /// <summary>
    /// 【查询】获取用户公司关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户-公司关联 DTO 列表</returns>
    public async Task<List<TaktUserCompanyDto>> GetUserCompanyIdsAsync(long userId)
    {
        // 【校验】用户存在且属于当前租户
        var user = await EnsureUserAccessibleAsync(userId);

        // 【查询】用户-公司关联表（默认公司优先排序）
        var list = await _userCompanyRepository.GetListAsync(
            x => x.UserId == userId,
            x => x.IsDefault,
            true);

        var dtos = list.Adapt<List<TaktUserCompanyDto>>();
        foreach (var dto in dtos)
        {
            dto.UserName = user.Username;
        }

        return dtos;
    }

    /// <summary>
    /// 【分配】用户可访问租户全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCodes">租户编码列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignUserTenantsAsync(long userId, string[] tenantCodes)
    {
        // 【校验】用户存在且属于当前租户
        var user = await EnsureUserAccessibleAsync(userId);
        var codes = NormalizeAssociationCodes(tenantCodes);

        if (codes.Count > 0 && !codes.Contains(user.TenantCode, StringComparer.OrdinalIgnoreCase))
        {
            codes.Insert(0, user.TenantCode);
        }

        var defaultTenantCode = codes.Count == 0
            ? user.TenantCode
            : codes.FirstOrDefault(c =>
                string.Equals(c, user.TenantCode, StringComparison.OrdinalIgnoreCase)) ?? codes[0];

        // 【新增】构建待插入的关联实体
        var entities = codes.Select(code => new TaktUserTenant
        {
            UserId = userId,
            TenantCode = code,
            IsDefault = string.Equals(code, defaultTenantCode, StringComparison.OrdinalIgnoreCase)
                ? 1
                : 0,
        }).ToList();

        // 【分配】查询 → 删除 → 新增
        await ReplaceTenantAssociationsAsync(
            _userTenantRepository,
            x => x.UserId == userId,
            entities,
            $"用户租户 UserId={userId}");
        return true;
    }

    /// <summary>
    /// 【查询】获取租户用户关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>用户-租户关联 DTO 列表</returns>
    public async Task<List<TaktUserTenantDto>> GetTenantUserIdsAsync(string tenantCode)
    {
        await EnsureTenantCodeAccessibleAsync(tenantCode);
        var code = tenantCode.Trim();
        var list = await _userTenantRepository.GetListAsync(x => x.TenantCode == code);
        var dtos = list.Adapt<List<TaktUserTenantDto>>();
        if (dtos.Count == 0)
        {
            return dtos;
        }
        var userIds = dtos.Select(d => d.UserId).Distinct().ToList();
        var users = await _userRepository.GetListAsync(u => userIds.Contains(u.Id));
        var userNameMap = users.ToDictionary(u => u.Id, u => u.Username);
        foreach (var dto in dtos)
        {
            if (userNameMap.TryGetValue(dto.UserId, out var userName))
            {
                dto.UserName = userName;
            }
        }
        return dtos;
    }

    /// <summary>
    /// 【分配】租户用户全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="userIds">用户ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignTenantUsersAsync(string tenantCode, long[] userIds)
    {
        await EnsureTenantCodeAccessibleAsync(tenantCode);
        var code = tenantCode.Trim();
        var userIdList = userIds?.Distinct().ToList() ?? [];
        if (userIdList.Count > 0)
        {
            var users = await _userRepository.GetListAsync(u => userIdList.Contains(u.Id));
            if (users.Count != userIdList.Count)
            {
                ThrowBusinessException("部分用户不存在或不可用");
            }
        }
        var entities = userIdList.Select(userId => new TaktUserTenant
        {
            UserId = userId,
            TenantCode = code,
            IsDefault = 0,
        }).ToList();
        await ReplaceTenantAssociationsAsync(
            _userTenantRepository,
            x => x.TenantCode == code,
            entities,
            $"租户用户 TenantCode={code}");
        return true;
    }

    /// <summary>
    /// 【分配】用户可访问公司全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="companyCodes">公司编码列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignUserCompaniesAsync(long userId, string[] companyCodes)
    {
        // 【校验】用户存在且属于当前租户
        await EnsureUserAccessibleAsync(userId);
        var codes = NormalizeAssociationCodes(companyCodes);

        // 【查询】校验公司编码均存在且已启用
        var companyMap = await ResolveEnabledCompaniesByCodesAsync(codes);

        var defaultCompanyCode = codes.Count == 0
            ? CurrentCompanyCode
            : !string.IsNullOrWhiteSpace(CurrentCompanyCode)
              && codes.Contains(CurrentCompanyCode, StringComparer.OrdinalIgnoreCase)
                ? CurrentCompanyCode
                : codes[0];

        // 【新增】构建待插入的关联实体
        var entities = codes.Select(code =>
        {
            var company = companyMap[code];
            return new TaktUserCompany
            {
                UserId = userId,
                TenantCode = CurrentTenantCode,
                CompanyCode = company.CompanyCode,
                IsDefault = string.Equals(code, defaultCompanyCode, StringComparison.OrdinalIgnoreCase)
                    ? 1
                    : 0,
            };
        }).ToList();

        // 【分配】查询 → 删除 → 新增
        await ReplaceCompanyAssociationsAsync(
            _userCompanyRepository,
            x => x.UserId == userId,
            entities,
            $"用户公司 UserId={userId}");
        return true;
    }

    #endregion

    #region 角色菜单权限（TaktRoleMenu）

    /// <summary>
    /// 【查询】获取角色菜单关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-菜单关联 DTO 列表</returns>
    public async Task<List<TaktRoleMenuDto>> GetRoleMenuIdsAsync(long roleId)
    {
        // 【校验】角色存在且属于当前租户
        var role = await EnsureRoleAccessibleAsync(roleId);

        // 【查询】角色-菜单关联表
        var list = await _roleMenuRepository.GetListAsync(x => x.RoleId == roleId);
        var dtos = list.Adapt<List<TaktRoleMenuDto>>();

        if (dtos.Count == 0)
        {
            return dtos;
        }

        // 【查询】菜单主数据，填充 MenuName
        var menuIds = dtos.Select(d => d.MenuId).Distinct().ToList();
        var menus = await _menuRepository.GetListAsync(m => menuIds.Contains(m.Id));
        var menuNameMap = menus.ToDictionary(m => m.Id, m => m.MenuName);

        foreach (var dto in dtos)
        {
            dto.RoleName = role.RoleName;
            if (menuNameMap.TryGetValue(dto.MenuId, out var menuName))
            {
                dto.MenuName = menuName;
            }
        }

        return dtos;
    }

    /// <summary>
    /// 【分配】角色菜单全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="menuIds">菜单ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignRoleMenusAsync(long roleId, long[] menuIds)
    {
        // 【校验】角色存在且属于当前租户
        await EnsureRoleAccessibleAsync(roleId);

        var menuIdList = menuIds?.Distinct().ToList() ?? [];

        // 【查询】校验菜单主数据是否全部存在
        if (menuIdList.Count > 0)
        {
            var menus = await _menuRepository.GetListAsync(m => menuIdList.Contains(m.Id));
            if (menus.Count != menuIdList.Count)
            {
                ThrowBusinessException("部分菜单不存在或不可用");
            }
        }

        // 【新增】构建待插入的关联实体
        var entities = menuIdList.Select(menuId => new TaktRoleMenu
        {
            RoleId = roleId,
            MenuId = menuId,
        }).ToList();

        // 【分配】查询 → 删除 → 新增
        await ReplaceTenantAssociationsAsync(
            _roleMenuRepository,
            x => x.RoleId == roleId,
            entities,
            $"角色菜单 RoleId={roleId}");
        return true;
    }

    /// <summary>
    /// 【查询】获取菜单角色关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    /// <returns>角色-菜单关联 DTO 列表</returns>
    public async Task<List<TaktRoleMenuDto>> GetMenuRoleIdsAsync(long menuId)
    {
        var menu = await EnsureMenuAccessibleAsync(menuId);
        var list = await _roleMenuRepository.GetListAsync(x => x.MenuId == menuId);
        var dtos = list.Adapt<List<TaktRoleMenuDto>>();
        if (dtos.Count == 0)
        {
            return dtos;
        }
        var roleIds = dtos.Select(d => d.RoleId).Distinct().ToList();
        var roles = await _roleRepository.GetListAsync(r => roleIds.Contains(r.Id));
        var roleNameMap = roles.ToDictionary(r => r.Id, r => r.RoleName);
        foreach (var dto in dtos)
        {
            dto.MenuName = menu.MenuName;
            if (roleNameMap.TryGetValue(dto.RoleId, out var roleName))
            {
                dto.RoleName = roleName;
            }
        }
        return dtos;
    }

    /// <summary>
    /// 【分配】菜单角色全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    /// <param name="roleIds">角色ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignMenuRolesAsync(long menuId, long[] roleIds)
    {
        await EnsureMenuAccessibleAsync(menuId);
        var roleIdList = roleIds?.Distinct().ToList() ?? [];
        if (roleIdList.Count > 0)
        {
            var roles = await _roleRepository.GetListAsync(r => roleIdList.Contains(r.Id));
            if (roles.Count != roleIdList.Count)
            {
                ThrowBusinessException("部分角色不存在或不可用");
            }
        }
        var entities = roleIdList.Select(roleId => new TaktRoleMenu
        {
            RoleId = roleId,
            MenuId = menuId,
        }).ToList();
        await ReplaceTenantAssociationsAsync(
            _roleMenuRepository,
            x => x.MenuId == menuId,
            entities,
            $"菜单角色 MenuId={menuId}");
        return true;
    }

    #endregion

    #region 角色数据权限范围（TaktRoleCompany、TaktRoleDept）

    /// <summary>
    /// 【查询】获取角色公司关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-公司关联 DTO 列表</returns>
    public async Task<List<TaktRoleCompanyDto>> GetRoleCompanyIdsAsync(long roleId)
    {
        // 【校验】角色存在且属于当前租户
        var role = await EnsureRoleAccessibleAsync(roleId);

        // 【查询】角色-公司关联表
        var list = await _roleCompanyRepository.GetListAsync(x => x.RoleId == roleId);
        var dtos = list.Adapt<List<TaktRoleCompanyDto>>();

        foreach (var dto in dtos)
        {
            dto.RoleName = role.RoleName;
        }

        return dtos;
    }

    /// <summary>
    /// 【分配】角色可访问公司全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="companyCodes">公司编码列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignRoleCompaniesAsync(long roleId, string[] companyCodes)
    {
        // 【校验】角色存在且属于当前租户
        await EnsureRoleAccessibleAsync(roleId);
        var codes = NormalizeAssociationCodes(companyCodes);

        // 【查询】校验公司编码均存在且已启用
        var companyMap = await ResolveEnabledCompaniesByCodesAsync(codes);

        // 【新增】构建待插入的关联实体
        var entities = codes.Select(code =>
        {
            var company = companyMap[code];
            return new TaktRoleCompany
            {
                RoleId = roleId,
                TenantCode = CurrentTenantCode,
                CompanyCode = company.CompanyCode,
            };
        }).ToList();

        // 【分配】查询 → 删除 → 新增
        await ReplaceCompanyAssociationsAsync(
            _roleCompanyRepository,
            x => x.RoleId == roleId,
            entities,
            $"角色公司 RoleId={roleId}");
        return true;
    }

    /// <summary>
    /// 【查询】获取角色部门关联列表（数据权限自定义范围，仅未软删除记录）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-部门关联 DTO 列表</returns>
    public async Task<List<TaktRoleDeptDto>> GetRoleDeptIdsAsync(long roleId)
    {
        // 【校验】角色存在且属于当前租户
        var role = await EnsureRoleAccessibleAsync(roleId);

        // 【查询】角色-部门关联表
        var list = await _roleDeptRepository.GetListAsync(x => x.RoleId == roleId);
        var dtos = list.Adapt<List<TaktRoleDeptDto>>();

        if (dtos.Count == 0)
        {
            return dtos;
        }

        // 【查询】部门主数据，填充 DeptName
        var deptIds = dtos.Select(d => d.DeptId).Distinct().ToList();
        var depts = await _deptRepository.GetListAsync(d => deptIds.Contains(d.Id));
        var deptNameMap = depts.ToDictionary(d => d.Id, d => d.DeptName);

        foreach (var dto in dtos)
        {
            dto.RoleName = role.RoleName;
            if (deptNameMap.TryGetValue(dto.DeptId, out var deptName))
            {
                dto.DeptName = deptName;
            }
        }

        return dtos;
    }

    /// <summary>
    /// 【分配】角色部门全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="deptIds">部门ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignRoleDeptsAsync(long roleId, long[] deptIds)
    {
        // 【校验】角色存在且属于当前租户
        await EnsureRoleAccessibleAsync(roleId);

        var deptIdList = deptIds?.Distinct().ToList() ?? [];

        // 【查询】校验部门主数据是否全部存在
        if (deptIdList.Count > 0)
        {
            var depts = await _deptRepository.GetListAsync(d => deptIdList.Contains(d.Id));
            if (depts.Count != deptIdList.Count)
            {
                ThrowBusinessException("部分部门不存在或不可用");
            }
        }

        // 【新增】构建待插入的关联实体
        var entities = deptIdList.Select(deptId => new TaktRoleDept
        {
            RoleId = roleId,
            DeptId = deptId,
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
        }).ToList();

        // 【分配】查询 → 删除 → 新增
        await ReplaceCompanyAssociationsAsync(
            _roleDeptRepository,
            x => x.RoleId == roleId,
            entities,
            $"角色部门 RoleId={roleId}");
        return true;
    }

    #endregion

    #region 员工组织关系（TaktEmployeeDept、TaktEmployeePost）

    /// <summary>
    /// 【查询】获取员工部门关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <returns>员工-部门关联 DTO 列表</returns>
    public async Task<List<TaktEmployeeDeptDto>> GetEmployeeDeptIdsAsync(long employeeId)
    {
        // 【校验】员工存在且属于当前租户与公司
        var employee = await EnsureEmployeeAccessibleAsync(employeeId);

        // 【查询】员工-部门关联表
        var list = await _employeeDeptRepository.GetListAsync(x => x.EmployeeId == employeeId);
        var dtos = list.Adapt<List<TaktEmployeeDeptDto>>();

        if (dtos.Count == 0)
        {
            return dtos;
        }

        // 【查询】部门主数据，填充 DeptName
        var deptIds = dtos.Select(d => d.DeptId).Distinct().ToList();
        var depts = await _deptRepository.GetListAsync(d => deptIds.Contains(d.Id));
        var deptNameMap = depts.ToDictionary(d => d.Id, d => d.DeptName);

        foreach (var dto in dtos)
        {
            dto.EmployeeName = employee.Name;
            if (deptNameMap.TryGetValue(dto.DeptId, out var deptName))
            {
                dto.DeptName = deptName;
            }
        }

        return dtos;
    }

    /// <summary>
    /// 【查询】获取员工岗位关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <returns>员工-岗位关联 DTO 列表</returns>
    public async Task<List<TaktEmployeePostDto>> GetEmployeePostIdsAsync(long employeeId)
    {
        // 【校验】员工存在且属于当前租户与公司
        var employee = await EnsureEmployeeAccessibleAsync(employeeId);

        // 【查询】员工-岗位关联表
        var list = await _employeePostRepository.GetListAsync(x => x.EmployeeId == employeeId);
        var dtos = list.Adapt<List<TaktEmployeePostDto>>();

        if (dtos.Count == 0)
        {
            return dtos;
        }

        // 【查询】岗位主数据，填充 PostName
        var postIds = dtos.Select(d => d.PostId).Distinct().ToList();
        var posts = await _postRepository.GetListAsync(p => postIds.Contains(p.Id));
        var postNameMap = posts.ToDictionary(p => p.Id, p => p.PostName);

        foreach (var dto in dtos)
        {
            dto.EmployeeName = employee.Name;
            if (postNameMap.TryGetValue(dto.PostId, out var postName))
            {
                dto.PostName = postName;
            }
        }

        return dtos;
    }

    /// <summary>
    /// 【分配】员工部门全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <param name="deptIds">部门ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignEmployeeDeptsAsync(long employeeId, long[] deptIds)
    {
        // 【校验】员工存在且属于当前租户与公司
        await EnsureEmployeeAccessibleAsync(employeeId);

        var deptIdList = deptIds?.Distinct().ToList() ?? [];

        // 【查询】校验部门主数据是否全部存在
        if (deptIdList.Count > 0)
        {
            var depts = await _deptRepository.GetListAsync(d => deptIdList.Contains(d.Id));
            if (depts.Count != deptIdList.Count)
            {
                ThrowBusinessException("部分部门不存在或不可用");
            }
        }

        // 【新增】构建待插入的关联实体
        var entities = deptIdList.Select(deptId => new TaktEmployeeDept
        {
            EmployeeId = employeeId,
            DeptId = deptId,
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
        }).ToList();

        // 【分配】查询 → 删除 → 新增
        await ReplaceCompanyAssociationsAsync(
            _employeeDeptRepository,
            x => x.EmployeeId == employeeId,
            entities,
            $"员工部门 EmployeeId={employeeId}");
        return true;
    }

    /// <summary>
    /// 【查询】获取部门员工关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <returns>员工-部门关联 DTO 列表</returns>
    public async Task<List<TaktEmployeeDeptDto>> GetDeptEmployeeIdsAsync(long deptId)
    {
        var dept = await EnsureDeptAccessibleAsync(deptId);
        var list = await _employeeDeptRepository.GetListAsync(x => x.DeptId == deptId);
        var dtos = list.Adapt<List<TaktEmployeeDeptDto>>();
        if (dtos.Count == 0)
        {
            return dtos;
        }
        var employeeIds = dtos.Select(d => d.EmployeeId).Distinct().ToList();
        var employees = await _employeeRepository.GetListAsync(e => employeeIds.Contains(e.Id));
        var employeeNameMap = employees.ToDictionary(e => e.Id, e => e.Name);
        foreach (var dto in dtos)
        {
            dto.DeptName = dept.DeptName;
            if (employeeNameMap.TryGetValue(dto.EmployeeId, out var employeeName))
            {
                dto.EmployeeName = employeeName;
            }
        }
        return dtos;
    }

    /// <summary>
    /// 【分配】部门员工全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <param name="employeeIds">员工ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignDeptEmployeesAsync(long deptId, long[] employeeIds)
    {
        await EnsureDeptAccessibleAsync(deptId);
        var employeeIdList = employeeIds?.Distinct().ToList() ?? [];
        if (employeeIdList.Count > 0)
        {
            var employees = await _employeeRepository.GetListAsync(e => employeeIdList.Contains(e.Id));
            if (employees.Count != employeeIdList.Count)
            {
                ThrowBusinessException("部分员工不存在或不可用");
            }
        }
        var entities = employeeIdList.Select(employeeId => new TaktEmployeeDept
        {
            EmployeeId = employeeId,
            DeptId = deptId,
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
        }).ToList();
        await ReplaceCompanyAssociationsAsync(
            _employeeDeptRepository,
            x => x.DeptId == deptId,
            entities,
            $"部门员工 DeptId={deptId}");
        return true;
    }

    /// <summary>
    /// 【分配】员工岗位全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <param name="postIds">岗位ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignEmployeePostsAsync(long employeeId, long[] postIds)
    {
        // 【校验】员工存在且属于当前租户与公司
        await EnsureEmployeeAccessibleAsync(employeeId);

        var postIdList = postIds?.Distinct().ToList() ?? [];

        // 【查询】校验岗位主数据是否全部存在
        if (postIdList.Count > 0)
        {
            var posts = await _postRepository.GetListAsync(p => postIdList.Contains(p.Id));
            if (posts.Count != postIdList.Count)
            {
                ThrowBusinessException("部分岗位不存在或不可用");
            }
        }

        // 【新增】构建待插入的关联实体
        var entities = postIdList.Select(postId => new TaktEmployeePost
        {
            EmployeeId = employeeId,
            PostId = postId,
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
        }).ToList();

        // 【分配】查询 → 删除 → 新增
        await ReplaceCompanyAssociationsAsync(
            _employeePostRepository,
            x => x.EmployeeId == employeeId,
            entities,
            $"员工岗位 EmployeeId={employeeId}");
        return true;
    }

    /// <summary>
    /// 【查询】获取岗位员工关联列表（仅未软删除记录）
    /// </summary>
    /// <param name="postId">岗位ID</param>
    /// <returns>员工-岗位关联 DTO 列表</returns>
    public async Task<List<TaktEmployeePostDto>> GetPostEmployeeIdsAsync(long postId)
    {
        var post = await EnsurePostAccessibleAsync(postId);
        var list = await _employeePostRepository.GetListAsync(x => x.PostId == postId);
        var dtos = list.Adapt<List<TaktEmployeePostDto>>();
        if (dtos.Count == 0)
        {
            return dtos;
        }
        var employeeIds = dtos.Select(d => d.EmployeeId).Distinct().ToList();
        var employees = await _employeeRepository.GetListAsync(e => employeeIds.Contains(e.Id));
        var employeeNameMap = employees.ToDictionary(e => e.Id, e => e.Name);
        foreach (var dto in dtos)
        {
            dto.PostName = post.PostName;
            if (employeeNameMap.TryGetValue(dto.EmployeeId, out var employeeName))
            {
                dto.EmployeeName = employeeName;
            }
        }
        return dtos;
    }

    /// <summary>
    /// 【分配】岗位员工全量覆盖（【查询】旧关联 → 【删除】软删除 → 【新增】批量插入）
    /// </summary>
    /// <param name="postId">岗位ID</param>
    /// <param name="employeeIds">员工ID列表</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AssignPostEmployeesAsync(long postId, long[] employeeIds)
    {
        await EnsurePostAccessibleAsync(postId);
        var employeeIdList = employeeIds?.Distinct().ToList() ?? [];
        if (employeeIdList.Count > 0)
        {
            var employees = await _employeeRepository.GetListAsync(e => employeeIdList.Contains(e.Id));
            if (employees.Count != employeeIdList.Count)
            {
                ThrowBusinessException("部分员工不存在或不可用");
            }
        }
        var entities = employeeIdList.Select(employeeId => new TaktEmployeePost
        {
            EmployeeId = employeeId,
            PostId = postId,
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
        }).ToList();
        await ReplaceCompanyAssociationsAsync(
            _employeePostRepository,
            x => x.PostId == postId,
            entities,
            $"岗位员工 PostId={postId}");
        return true;
    }

    #endregion

    #region 私有辅助方法

    /// <summary>
    /// 【分配】全量覆盖租户级关联（统一三步：【查询】→【删除】→【新增】）
    /// </summary>
    /// <typeparam name="TEntity">租户级关联实体</typeparam>
    /// <param name="repository">关联仓储</param>
    /// <param name="scopePredicate">作用域条件（如 UserId、RoleId）</param>
    /// <param name="newEntities">【新增】待插入的关联实体列表</param>
    /// <param name="logContext">日志上下文</param>
    /// <returns>异步任务</returns>
    private async Task ReplaceTenantAssociationsAsync<TEntity>(
        ITaktTenantRepository<TEntity> repository,
        Expression<Func<TEntity, bool>> scopePredicate,
        List<TEntity> newEntities,
        string logContext)
        where TEntity : TaktTenantEntityBase, new()
    {
        // 【查询】按作用域获取当前未删除的旧关联
        var existing = await repository.GetListAsync(scopePredicate);

        // 【删除】对查询到的旧关联逐条软删除（IsDeleted=1）
        await SoftDeleteTenantAssociationRowsAsync(repository, existing, logContext);

        if (newEntities.Count == 0)
        {
            LogInformation("【分配】完成，无【新增】数据: {LogContext}", logContext);
            return;
        }

        // 【新增】批量插入新关联
        await repository.CreateRangeAsync(newEntities);
        LogInformation("【新增】关联 {Count} 条: {LogContext}", newEntities.Count, logContext);
    }

    /// <summary>
    /// 【分配】全量覆盖公司级关联（统一三步：【查询】→【删除】→【新增】）
    /// </summary>
    /// <typeparam name="TEntity">公司级关联实体</typeparam>
    /// <param name="repository">关联仓储</param>
    /// <param name="scopePredicate">作用域条件（如 UserId、RoleId、EmployeeId）</param>
    /// <param name="newEntities">【新增】待插入的关联实体列表</param>
    /// <param name="logContext">日志上下文</param>
    /// <returns>异步任务</returns>
    private async Task ReplaceCompanyAssociationsAsync<TEntity>(
        ITaktCompanyRepository<TEntity> repository,
        Expression<Func<TEntity, bool>> scopePredicate,
        List<TEntity> newEntities,
        string logContext)
        where TEntity : TaktCompanyEntityBase, new()
    {
        // 【查询】按作用域获取当前未删除的旧关联
        var existing = await repository.GetListAsync(scopePredicate);

        // 【删除】对查询到的旧关联逐条软删除（IsDeleted=1）
        await SoftDeleteCompanyAssociationRowsAsync(repository, existing, logContext);

        if (newEntities.Count == 0)
        {
            LogInformation("【分配】完成，无【新增】数据: {LogContext}", logContext);
            return;
        }

        // 【新增】批量插入新关联
        await repository.CreateRangeAsync(newEntities);
        LogInformation("【新增】关联 {Count} 条: {LogContext}", newEntities.Count, logContext);
    }

    /// <summary>
    /// 【删除】软删除租户级关联行（仓储 DeleteAsync 设置 IsDeleted、UpdatedAt、DeletedBy）
    /// </summary>
    /// <typeparam name="TEntity">租户级关联实体</typeparam>
    /// <param name="repository">关联仓储</param>
    /// <param name="existing">【查询】得到的旧关联列表</param>
    /// <param name="logContext">日志上下文</param>
    /// <returns>异步任务</returns>
    private async Task SoftDeleteTenantAssociationRowsAsync<TEntity>(
        ITaktTenantRepository<TEntity> repository,
        List<TEntity> existing,
        string logContext)
        where TEntity : TaktTenantEntityBase, new()
    {
        if (existing.Count == 0)
        {
            return;
        }

        foreach (var row in existing)
        {
            await repository.DeleteAsync(row.Id);
        }

        LogInformation("【删除】已软删除旧关联 {Count} 条: {LogContext}", existing.Count, logContext);
    }

    /// <summary>
    /// 【删除】软删除公司级关联行（仓储 DeleteAsync 设置 IsDeleted、UpdatedAt、DeletedBy）
    /// </summary>
    /// <typeparam name="TEntity">公司级关联实体</typeparam>
    /// <param name="repository">关联仓储</param>
    /// <param name="existing">【查询】得到的旧关联列表</param>
    /// <param name="logContext">日志上下文</param>
    /// <returns>异步任务</returns>
    private async Task SoftDeleteCompanyAssociationRowsAsync<TEntity>(
        ITaktCompanyRepository<TEntity> repository,
        List<TEntity> existing,
        string logContext)
        where TEntity : TaktCompanyEntityBase, new()
    {
        if (existing.Count == 0)
        {
            return;
        }

        foreach (var row in existing)
        {
            await repository.DeleteAsync(row.Id);
        }

        LogInformation("【删除】已软删除旧关联 {Count} 条: {LogContext}", existing.Count, logContext);
    }

    /// <summary>
    /// 【校验】用户存在且属于当前租户（分配/查询前调用）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户实体</returns>
    private async Task<TaktUser> EnsureUserAccessibleAsync(long userId)
    {
        // 【查询】用户主数据
        var user = await _userRepository.GetByIdAsync(userId);
        EnsureExists(user, "用户不存在");

        if (user!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限操作此用户");
        }

        return user;
    }

    /// <summary>
    /// 【校验】角色存在且属于当前租户（分配/查询前调用）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色实体</returns>
    private async Task<TaktRole> EnsureRoleAccessibleAsync(long roleId)
    {
        // 【查询】角色主数据
        var role = await _roleRepository.GetByIdAsync(roleId);
        EnsureExists(role, "角色不存在");

        if (role!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限操作此角色");
        }

        return role;
    }

    /// <summary>
    /// 【校验】员工存在且属于当前租户与公司（分配/查询前调用）
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <returns>员工实体</returns>
    private async Task<TaktEmployee> EnsureEmployeeAccessibleAsync(long employeeId)
    {
        // 【查询】员工主数据
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        EnsureExists(employee, "员工不存在");

        if (employee!.TenantCode != CurrentTenantCode
            || employee.CompanyCode != CurrentCompanyCode)
        {
            ThrowBusinessException("无权限操作此员工");
        }

        return employee;
    }

    /// <summary>
    /// 【校验】岗位存在且属于当前租户与公司（分配/查询前调用）
    /// </summary>
    /// <param name="postId">岗位ID</param>
    /// <returns>岗位实体</returns>
    private async Task<TaktPost> EnsurePostAccessibleAsync(long postId)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        EnsureExists(post, "岗位不存在");
        if (post!.TenantCode != CurrentTenantCode
            || post.CompanyCode != CurrentCompanyCode)
        {
            ThrowBusinessException("无权限操作此岗位");
        }
        return post;
    }

    /// <summary>
    /// 【校验】部门存在且属于当前租户与公司（分配/查询前调用）
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <returns>部门实体</returns>
    private async Task<TaktDept> EnsureDeptAccessibleAsync(long deptId)
    {
        var dept = await _deptRepository.GetByIdAsync(deptId);
        EnsureExists(dept, "部门不存在");
        if (dept!.TenantCode != CurrentTenantCode
            || dept.CompanyCode != CurrentCompanyCode)
        {
            ThrowBusinessException("无权限操作此部门");
        }
        return dept;
    }

    /// <summary>
    /// 【校验】菜单存在且属于当前租户（分配/查询前调用）
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    /// <returns>菜单实体</returns>
    private async Task<TaktMenu> EnsureMenuAccessibleAsync(long menuId)
    {
        var menu = await _menuRepository.GetByIdAsync(menuId);
        EnsureExists(menu, "菜单不存在");
        if (menu!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限操作此菜单");
        }
        return menu;
    }

    /// <summary>
    /// 【校验】租户编码存在（分配/查询前调用）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>租户实体</returns>
    private async Task<TaktTenant> EnsureTenantCodeAccessibleAsync(string tenantCode)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            ThrowBusinessException("租户编码不能为空");
        }
        var code = tenantCode.Trim();
        var tenants = await _tenantRepository.GetListAsync(t => t.TenantCode == code);
        var tenant = tenants.FirstOrDefault();
        EnsureExists(tenant, "租户不存在");
        return tenant!;
    }

    /// <summary>
    /// 规范化关联编码列表（去空、去重，分配入参预处理，不涉及数据库）
    /// </summary>
    /// <param name="codes">原始编码列表</param>
    /// <returns>规范化后的编码列表</returns>
    private static List<string> NormalizeAssociationCodes(IEnumerable<string>? codes)
    {
        return codes?
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Select(code => code.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList() ?? [];
    }

    /// <summary>
    /// 【查询】按公司编码解析已启用公司（分配前校验 CompanyCode 是否存在）
    /// </summary>
    /// <param name="companyCodes">公司编码列表</param>
    /// <returns>公司编码到公司实体的映射</returns>
    private async Task<Dictionary<string, TaktCompany>> ResolveEnabledCompaniesByCodesAsync(
        IReadOnlyCollection<string> companyCodes)
    {
        if (companyCodes.Count == 0)
        {
            return new Dictionary<string, TaktCompany>(StringComparer.OrdinalIgnoreCase);
        }

        // 【查询】当前租户下已启用的公司主数据
        var enabledCompanies = await _companyCatalogRepository.GetListAsync(
            c => c.CompanyStatus == 1);
        var companyMap = enabledCompanies.ToDictionary(
            c => c.CompanyCode,
            c => c,
            StringComparer.OrdinalIgnoreCase);

        foreach (var code in companyCodes)
        {
            if (!companyMap.ContainsKey(code))
            {
                ThrowBusinessException($"公司编码 {code} 不存在或未启用");
            }
        }

        return companyMap;
    }

    #endregion
}
