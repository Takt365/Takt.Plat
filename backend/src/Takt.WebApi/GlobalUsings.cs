// ========================================
// 项目名称：Takt.Plat
// 命名空间：Takt.WebApi
// 文件名称：GlobalUsings.cs
// 创建时间：2024-01-15 10:00:00
// 创建人：Davis.Cheng
// 功能描述：Takt.WebApi 项目全局 using 声明
// 
// 版权所有 (C) Takt.Plat. 保留所有权利。
// 本代码仅供内部使用，未经授权不得复制或分发。
// ========================================

// ASP.NET Core
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication;

// OpenIddict
global using OpenIddict.Abstractions;
global using OpenIddict.Server.AspNetCore;
global using OpenIddict.Validation.AspNetCore;
global using Microsoft.IdentityModel.Tokens;

// Takt Domain
global using Takt.Domain.Entities;
global using Takt.Domain.Interfaces;
global using Takt.Domain.Repositories;

// Takt Application
global using Takt.Application.Services;
global using Takt.Application.Dtos;

// Takt Infrastructure
global using Takt.Infrastructure.Data.Context;
global using Takt.Infrastructure.Repositories;

// Takt Shared
global using Takt.Shared.Constants;
global using Takt.Shared.Enums;
global using Takt.Shared.Exceptions;
global using Takt.Shared.Helpers;
global using Takt.Shared.Models;
global using Takt.Shared.Options;

// Entity Framework Core
global using Microsoft.EntityFrameworkCore;

// System 命名空间
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
