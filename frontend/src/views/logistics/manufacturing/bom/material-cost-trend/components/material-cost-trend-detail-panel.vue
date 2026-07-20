<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/components -->
<!-- 文件名称：material-cost-trend-detail-panel.vue -->
<!-- 功能描述：BOM 成本推移右表：选中产品的转置月涨跌明细 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-1 flex-col overflow-hidden p-2">
    <material-cost-item-component-price-panel
      ref="componentPricePanelRef"
      class="h-full min-h-0"
      :period-order="periodOrder"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 成本推移右表明细：产品转置月涨跌
 */
import MaterialCostItemComponentPricePanel from './material-cost-item-component-price-panel.vue'

defineProps<{
  /** 期间列顺序 */
  periodOrder?: string[]
}>()

/** 转置面板 */
const componentPricePanelRef = ref<{
  reload?: () => Promise<void>
  recalcTableScrollY?: () => void
} | null>(null)

/**
 * 选中产品变更后重载右表
 */
async function reload() {
  await componentPricePanelRef.value?.reload?.()
  nextTick(() => {
    componentPricePanelRef.value?.recalcTableScrollY?.()
  })
}

defineExpose({
  reload,
})
</script>
