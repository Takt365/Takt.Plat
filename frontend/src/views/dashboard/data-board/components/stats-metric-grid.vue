<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src/views/dashboard/data-board/components -->
<!-- 文件名称：stats-metric-grid.vue -->
<!-- 功能描述：数据看板统计指标栅格（a-statistic 统一展示） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="stats-metric-grid">
    <a-spin :spinning="loading">
      <p
        v-if="periodLabel"
        class="stats-metric-grid__period"
      >
        {{ periodLabel }}
      </p>
      <a-row :gutter="gutter">
        <a-col
          v-for="item in items"
          :key="item.key"
          :xs="colXs"
          :sm="colSm"
          :md="colMd"
          :lg="colLg"
        >
          <a-statistic
            :title="item.title"
            :value="item.value"
            :prefix="item.prefix"
            :suffix="item.suffix"
            :precision="item.precision"
            :value-style="valueStyle"
          />
        </a-col>
      </a-row>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 数据看板 a-statistic 指标栅格
 */
import type { CSSProperties } from 'vue'

/** 单个统计指标 */
export interface StatsMetricItem {
  key: string
  title: string
  value: number
  prefix?: string
  suffix?: string
  precision?: number
}

const props = withDefaults(defineProps<{
  /** 指标列表 */
  items: StatsMetricItem[]
  /** 加载中 */
  loading?: boolean
  /** 周期说明（如「统计周期：本月」） */
  periodLabel?: string
  /** 栅格 gutter */
  gutter?: number | [number, number]
  /** 响应式列宽 */
  colXs?: number
  colSm?: number
  colMd?: number
  colLg?: number
  /** 数值样式 */
  valueStyle?: CSSProperties
}>(), {
  loading: false,
  periodLabel: '',
  gutter: 16,
  colXs: 12,
  colSm: 8,
  colMd: 8,
  colLg: 8,
  valueStyle: () => ({ fontSize: '24px' }),
})
</script>

<style scoped lang="css">
.stats-metric-grid {
  min-height: 80px;
}
.stats-metric-grid__period {
  margin: 0 0 8px;
  font-size: 12px;
  color: var(--ant-color-text-secondary);
}
</style>
