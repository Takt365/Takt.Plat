<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-price-trend/components -->
<!-- 文件名称：purchase-price-trend-query-form.vue -->
<!-- 功能描述：采购价格推移查询栏（工厂/期间/供应商/物料/价格类型） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar purchase-price-trend-query-bar">
    <div class="purchase-price-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="purchase-price-trend-query-bar__control purchase-price-trend-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.purchaseprice.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="purchase-price-trend-query-bar__control purchase-price-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`),
        ]"
      />
      <TaktSelect
        v-model:value="supplierCode"
        api-url="TaktPurchasePrices/supplier-options"
        :api-params="plantCode ? { plantCode } : undefined"
        :disabled="!plantCode?.trim()"
        class="purchase-price-trend-query-bar__control purchase-price-trend-query-bar__control--supplier"
        allow-clear
        show-search
        :placeholder="t(`${localePrefix}.supplierCode`)"
      />
      <TaktSelect
        v-model:value="materialCode"
        api-url="TaktPurchasePrices/material-options"
        :api-params="plantCode ? { plantCode } : undefined"
        :disabled="!plantCode?.trim()"
        class="purchase-price-trend-query-bar__control purchase-price-trend-query-bar__control--material"
        allow-clear
        show-search
        :placeholder="t(`${localePrefix}.materialCode`)"
      />
      <TaktSelect
        v-model:value="priceType"
        dict-type="logistics_price_type"
        class="purchase-price-trend-query-bar__control purchase-price-trend-query-bar__control--price-type"
        allow-clear
        :placeholder="t('entity.purchaseprice.pricetype')"
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
 * 采购价格推移查询栏：工厂 + 期间 + 供应商 + 物料 + 价格类型
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 供应商编码 */
const supplierCode = defineModel<string | undefined>('supplierCode')
/** 物料编码（采购价格表内去重选项） */
const materialCode = defineModel<string | undefined>('materialCode')
/** 价格类型 */
const priceType = defineModel<string | undefined>('priceType')
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
const localePrefix = 'logistics.procurement.purchase-price-trend.page'

/** 工厂变更时清空依赖工厂的维度筛选 */
watch(
  () => plantCode.value,
  () => {
    supplierCode.value = undefined
    materialCode.value = undefined
  },
)</script>

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

.purchase-price-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.purchase-price-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.purchase-price-trend-query-bar__control--supplier {
  width: 12rem;
  min-width: 9rem;
}

.purchase-price-trend-query-bar__control--material {
  width: 12rem;
  min-width: 9rem;
}

.purchase-price-trend-query-bar__control--price-type {
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
