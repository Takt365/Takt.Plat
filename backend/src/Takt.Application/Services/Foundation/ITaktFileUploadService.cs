// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktFileUploadService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：文件上传应用服务接口（运行时上传/下载，编排 ITaktFileUploadEngine + TaktFile 元数据）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 文件上传应用服务（运行时能力，与 <see cref="ITaktFileService"/> CRUD 分离）
/// </summary>
public interface ITaktFileUploadService
{
    /// <summary>
    /// 整文件上传（引擎 I/O + 元数据落库）
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="fileName">原始文件名</param>
    /// <param name="contentType">MIME 类型</param>
    /// <param name="meta">可选业务元数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>文件 DTO</returns>
    Task<TaktFileDto> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadMetaDto? meta = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查分片是否已上传
    /// </summary>
    /// <param name="dto">检查参数</param>
    /// <returns>是否存在</returns>
    Task<TaktFileChunkCheckResultDto> CheckFileChunkAsync(TaktFileChunkCheckDto dto);

    /// <summary>
    /// 上传单个分片
    /// </summary>
    /// <param name="chunkStream">分片流</param>
    /// <param name="dto">分片元数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task UploadFileChunkAsync(
        Stream chunkStream,
        TaktFileChunkUploadDto dto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 合并分片并写入文件元数据
    /// </summary>
    /// <param name="dto">合并参数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>文件 DTO</returns>
    Task<TaktFileDto> MergeFileChunksAsync(
        TaktFileChunkMergeDto dto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 下载文件（更新下载次数）
    /// </summary>
    /// <param name="fileId">文件 ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>下载流与文件名</returns>
    Task<TaktFileDownloadResultDto> DownloadFileAsync(
        long fileId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新文件公开范围
    /// </summary>
    /// <param name="fileId">文件 ID</param>
    /// <param name="dto">公开范围</param>
    /// <returns>文件 DTO</returns>
    Task<TaktFileDto> ChangeFilePublicAccessAsync(long fileId, TaktFilePublicAccessDto dto);
}
