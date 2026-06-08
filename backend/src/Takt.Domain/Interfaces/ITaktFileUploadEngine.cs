// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktFileUploadEngine.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：通用文件上传下载引擎（整文件/分片/合并/读流/删物理文件；与业务元数据解耦）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 通用文件上传下载引擎。
/// 负责物理存储 I/O（本地分片、合并、读流）；业务元数据落库由各模块应用服务编排（如 <c>TaktFileUploadService</c>）。
/// </summary>
public interface ITaktFileUploadEngine
{
    /// <summary>
    /// 整文件上传
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="fileName">原始文件名</param>
    /// <param name="contentType">MIME 类型</param>
    /// <param name="scope">隔离范围；为空时从 <see cref="ITaktUserContext"/> 解析</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存储结果</returns>
    Task<TaktStoredFileResult> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查分片是否已上传（断点续传）
    /// </summary>
    /// <param name="request">检查参数</param>
    /// <param name="scope">隔离范围</param>
    /// <returns>是否存在</returns>
    Task<TaktFileChunkCheckResult> CheckChunkAsync(
        TaktFileChunkCheckRequest request,
        TaktFileUploadScope? scope = null);

    /// <summary>
    /// 上传单个分片
    /// </summary>
    /// <param name="chunkStream">分片流</param>
    /// <param name="request">分片元数据</param>
    /// <param name="scope">隔离范围</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task UploadChunkAsync(
        Stream chunkStream,
        TaktFileChunkUploadRequest request,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 合并分片为完整文件
    /// </summary>
    /// <param name="request">合并参数</param>
    /// <param name="scope">隔离范围</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存储结果</returns>
    Task<TaktStoredFileResult> MergeChunksAsync(
        TaktFileChunkMergeRequest request,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 按存储描述符打开只读流
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="downloadFileName">下载文件名；为空时使用路径文件名</param>
    /// <param name="contentType">MIME；为空时按扩展名推断</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>流与内容类型</returns>
    Task<TaktFileDownloadStreamResult> OpenReadAsync(
        TaktFileStorageDescriptor descriptor,
        string? downloadFileName = null,
        string? contentType = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除物理文件（本地存储）
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task DeleteStoredFileAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken = default);
}
