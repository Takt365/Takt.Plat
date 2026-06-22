// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktFileChunkPlanHelper.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：分片上传计划计算（总大小、MaxChunkCount、DefaultChunkSize 等纯函数）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// 分片上传计划计算（与 TaktFileUploadOptions 对齐，无 I/O）
/// </summary>
public static class TaktFileChunkPlanHelper
{
    /// <summary>
    /// 根据文件总大小与上传配置生成分片计划
    /// </summary>
    /// <param name="options">FileUpload 配置</param>
    /// <param name="totalSizeBytes">文件总大小（字节）</param>
    /// <returns>分片计划</returns>
    /// <exception cref="ArgumentNullException">options 为 null</exception>
    /// <exception cref="ArgumentOutOfRangeException">totalSizeBytes 非法或超过 MaxFileSizeBytes</exception>
    public static TaktFileChunkPlan Resolve(TaktFileUploadOptions options, long totalSizeBytes)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (totalSizeBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalSizeBytes), "文件总大小必须大于 0");
        }

        if (totalSizeBytes > options.MaxFileSizeBytes)
        {
            throw new ArgumentOutOfRangeException(nameof(totalSizeBytes), "文件总大小超过 MaxFileSizeBytes");
        }

        var maxChunkCount = Math.Max(1, options.MaxChunkCount);
        var defaultChunkSize = Math.Max(1L, options.DefaultChunkSizeBytes);
        var threshold = Math.Max(1L, options.ChunkThresholdBytes);

        if (totalSizeBytes <= threshold)
        {
            return BuildPlan(
                totalSizeBytes,
                useChunkUpload: false,
                chunkSizeBytes: totalSizeBytes,
                totalChunks: 1,
                options,
                maxChunkCount,
                defaultChunkSize,
                threshold);
        }

        var chunkSize = defaultChunkSize;
        var totalChunks = checked((int)Math.Ceiling((double)totalSizeBytes / chunkSize));
        if (totalChunks > maxChunkCount)
        {
            chunkSize = (long)Math.Ceiling((double)totalSizeBytes / maxChunkCount);
            if (chunkSize < 1)
            {
                chunkSize = 1;
            }

            totalChunks = checked((int)Math.Ceiling((double)totalSizeBytes / chunkSize));
        }

        if (totalChunks > maxChunkCount)
        {
            throw new InvalidOperationException("无法在 MaxChunkCount 限制内切分分片");
        }

        return BuildPlan(
            totalSizeBytes,
            useChunkUpload: true,
            chunkSizeBytes: chunkSize,
            totalChunks: totalChunks,
            options,
            maxChunkCount,
            defaultChunkSize,
            threshold);
    }

    /// <summary>
    /// 获取指定序号分片的期望大小（最后一片为余数）
    /// </summary>
    /// <param name="plan">分片计划</param>
    /// <param name="chunkNumber">分片序号（从 1 开始）</param>
    /// <returns>期望字节数</returns>
    /// <exception cref="ArgumentNullException">plan 为 null</exception>
    /// <exception cref="ArgumentOutOfRangeException">chunkNumber 超出范围</exception>
    public static long GetExpectedChunkSize(TaktFileChunkPlan plan, int chunkNumber)
    {
        ArgumentNullException.ThrowIfNull(plan);
        if (chunkNumber < 1 || chunkNumber > plan.TotalChunks)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkNumber));
        }

        if (chunkNumber < plan.TotalChunks)
        {
            return plan.ChunkSizeBytes;
        }

        var remainder = checked(plan.TotalSizeBytes - ((long)(plan.TotalChunks - 1) * plan.ChunkSizeBytes));
        return remainder > 0 ? remainder : plan.ChunkSizeBytes;
    }

    /// <summary>
    /// 校验客户端声明的分片元数据是否与计划一致
    /// </summary>
    /// <param name="plan">分片计划</param>
    /// <param name="totalChunks">客户端声明总分片数</param>
    /// <param name="chunkNumber">分片序号</param>
    /// <param name="declaredChunkSize">客户端声明分片大小</param>
    /// <param name="actualChunkSize">实际上传字节数</param>
    /// <returns>元数据是否与计划一致</returns>
    public static bool IsChunkMetadataValid(
        TaktFileChunkPlan plan,
        int totalChunks,
        int chunkNumber,
        long declaredChunkSize,
        long actualChunkSize)
    {
        ArgumentNullException.ThrowIfNull(plan);
        if (totalChunks != plan.TotalChunks)
        {
            return false;
        }

        if (chunkNumber < 1 || chunkNumber > plan.TotalChunks)
        {
            return false;
        }

        var expected = GetExpectedChunkSize(plan, chunkNumber);
        return declaredChunkSize == expected && actualChunkSize == expected;
    }

    private static TaktFileChunkPlan BuildPlan(
        long totalSizeBytes,
        bool useChunkUpload,
        long chunkSizeBytes,
        int totalChunks,
        TaktFileUploadOptions options,
        int maxChunkCount,
        long defaultChunkSize,
        long threshold)
    {
        return new TaktFileChunkPlan
        {
            TotalSizeBytes = totalSizeBytes,
            UseChunkUpload = useChunkUpload,
            ChunkSizeBytes = chunkSizeBytes,
            TotalChunks = totalChunks,
            MaxFileSizeBytes = options.MaxFileSizeBytes,
            MaxChunkCount = maxChunkCount,
            ChunkThresholdBytes = threshold,
            DefaultChunkSizeBytes = defaultChunkSize,
        };
    }
}
