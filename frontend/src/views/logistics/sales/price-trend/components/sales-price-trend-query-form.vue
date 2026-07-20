<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/price-trend/components -->
<!-- 文件名称：sales-price-trend-query-form.vue -->
<!-- 功能描述：销售价格推移查询栏（工厂/期间/客户/物料/价格类型） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar sales-price-trend-query-bar">
    <div class="sales-price-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.salesprice.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`),
        ]"
      />
      <TaktSelect
        v-model:value="customerCode"
        api-url="TaktCustomers/options"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--customer"
        allow-clear
        :placeholder="t(`${localePrefix}.customerCode`)"
      />
      <a-input
        v-model:value="materialCode"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--material"
        allow-clear
        :placeholder="t(`${localePrefix}.materialCode`)"
        @press-enter="emit('search')"
      />
      <TaktSelect
        v-model:value="priceType"
        dict-type="logistics_price_type"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--price-type"
        allow-clear
        :placeholder="t('entity.salesprice.pricetype')"
      />
    </div>
    <a-space class="query-actions">
      <a-button
        class="takt-button-query"
        :loading="props.loading"
        @click="emit('search')"
      >
        <template #icon>
          <RiSearchLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.query') }}
      </a-button>
      <a-button
        class="takt-button-reset"
        :disabled="props.loading"
        @click="emit('reset')"
      >
        <template #icon>
          <RiRefreshLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.reset') }}
      </a-button>
    </a-space>
  </div>
</template>

<script setup lang="ts">
/**
 * 销售价格推移查询栏：工厂 + 期间 + 客户 + 物料 + 价格类型
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 客户编码 */
const customerCode = defineModel<string | undefined>('customerCode')
/** 物料编码关键字 */
const materialCode = defineModel<string>('materialCode', { default: '' })
/** 价格类型 */
const priceType = defineModel<number | undefined>('priceType')
const props = defineProps<{
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.sales.price-trend.page'
</script>

<style scoped>
.takt-query-bar {
  margin: 4px;
  padding: 4px;
  display: flex;
  align-items: center;
  gap: 8px;
  width: 100%;
  box-sizing: border-box;
}

.sales-price-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.sales-price-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.sales-price-trend-query-bar__control--customer {
  width: 12rem;
  min-width: 9rem;
}

.sales-price-trend-query-bar__control--material {
  width: 12rem;
  min-width: 9rem;
}

.sales-price-trend-query-bar__control--price-type {
  width: 10rem;
  min-width: 8rem;
}

.query-actions {
  flex-shrink: 0;
}

.query-actions :deep(.ant-btn) {
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.query-actions :deep(.ant-btn .anticon) {
  margin-inline-end: 0 !important;
}
</style>
