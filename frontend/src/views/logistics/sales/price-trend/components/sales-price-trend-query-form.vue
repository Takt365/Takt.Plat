<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/price-trend/components -->
<!-- 文件名称：sales-price-trend-query-form.vue -->
<!-- 功能描述：销售价格推移查询栏（四级：工厂→条件类型→客户→物料；物料可空） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar sales-price-trend-query-bar">
    <div class="sales-price-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        :api-url="plantOptionsUrl"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--plant"
        allow-clear
        show-search
        :placeholder="t('common.page.entity.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`)]"
      />
      <TaktSelect
        v-model:value="priceType"
        :api-url="priceTypeOptionsUrl"
        :api-params="priceTypeApiParams"
        :disabled="!plantCode?.trim()"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--price-type"
        allow-clear
        show-search
        :placeholder="t('entity.salesprice.pricetype')"
      />
      <TaktSelect
        v-model:value="customerCode"
        :api-url="customerOptionsUrl"
        :api-params="customerApiParams"
        :disabled="!plantCode?.trim() || !priceType?.trim()"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--customer"
        allow-clear
        show-search
        :placeholder="t('entity.salesprice.customercode')"
      />
      <TaktSelect
        v-model:value="materialCode"
        :api-url="materialOptionsUrl"
        :api-params="materialApiParams"
        :disabled="!plantCode?.trim() || !priceType?.trim() || !customerCode?.trim()"
        class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--material"
        allow-clear
        show-search
        :placeholder="t('entity.salesprice.materialcode')"
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
 * 销售价格推移查询栏：工厂 → 条件类型 → 客户 → 物料（物料可空）
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getSalesPriceTrendCustomerOptionsUrl,
  getSalesPriceTrendMaterialOptionsUrl,
  getSalesPriceTrendPlantOptionsUrl,
  getSalesPriceTrendPriceTypeOptionsUrl,
} from '@/api/logistics/sales/price-trend'

/** 工厂代码（第 1 级，必选） */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 条件类型（第 2 级，必选） */
const priceType = defineModel<string | undefined>('priceType')
/** 客户编码（第 3 级，必选） */
const customerCode = defineModel<string | undefined>('customerCode')
/** 物料编码（第 4 级，可空） */
const materialCode = defineModel<string | undefined>('materialCode')
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
/** 推移本表级联选项 URL（TaktSalesPriceTrends） */
const plantOptionsUrl = getSalesPriceTrendPlantOptionsUrl()
const priceTypeOptionsUrl = getSalesPriceTrendPriceTypeOptionsUrl()
const customerOptionsUrl = getSalesPriceTrendCustomerOptionsUrl()
const materialOptionsUrl = getSalesPriceTrendMaterialOptionsUrl()

/** 第 2 级：工厂 → 条件类型 */
const priceTypeApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 第 3 级：工厂 + 条件类型 → 客户 */
const customerApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const type = priceType.value?.trim()
  if (!plant || !type) {
    return undefined
  }
  return { plantCode: plant, priceType: type }
})

/** 第 4 级：工厂 + 条件类型 + 客户 → 物料（可选） */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const type = priceType.value?.trim()
  const customer = customerCode.value?.trim()
  if (!plant || !type || !customer) {
    return undefined
  }
  return { plantCode: plant, priceType: type, customerCode: customer }
})

/** 工厂变更：清空第 2～4 级 */
watch(
  () => plantCode.value,
  () => {
    priceType.value = undefined
    customerCode.value = undefined
    materialCode.value = undefined
  },
)

/** 条件类型变更：清空第 3～4 级 */
watch(
  () => priceType.value,
  () => {
    customerCode.value = undefined
    materialCode.value = undefined
  },
)

/** 客户变更：清空第 4 级 */
watch(
  () => customerCode.value,
  () => {
    materialCode.value = undefined
  },
)
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

.sales-price-trend-query-bar__control--price-type {
  width: 10rem;
  min-width: 8rem;
}

.sales-price-trend-query-bar__control--customer {
  width: 12rem;
  min-width: 9rem;
}

.sales-price-trend-query-bar__control--material {
  width: 12rem;
  min-width: 9rem;
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
