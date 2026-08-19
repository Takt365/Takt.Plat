<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/monthly-trend/components -->
<!-- 文件名称：sales-monthly-trend-query-form.vue -->
<!-- 功能描述：月销售推移查询栏（工厂→客户 + 期间） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar sales-monthly-trend-query-bar">
    <div class="sales-monthly-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        :api-url="plantOptionsUrl"
        class="sales-monthly-trend-query-bar__control sales-monthly-trend-query-bar__control--plant"
        allow-clear
        show-search
        :placeholder="t('common.page.entity.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="sales-monthly-trend-query-bar__control sales-monthly-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`)]"
      />
      <TaktSelect
        :key="customerSelectKey"
        v-model:value="customerCode"
        :api-url="customerOptionsUrl"
        :api-params="customerApiParams"
        :disabled="!plantCode?.trim()"
        class="sales-monthly-trend-query-bar__control sales-monthly-trend-query-bar__control--customer"
        allow-clear
        show-search
        :placeholder="t('entity.salesorder.customercode')"
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
 * 月销售推移查询栏：工厂 → 客户（客户可空；选项来自销售订单本表）
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getSalesMonthlyTrendCustomerOptionsUrl,
  getSalesMonthlyTrendPlantOptionsUrl,
} from '@/api/logistics/sales/monthly-trend'

/** 工厂代码（第 1 级，必选） */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 客户编码（第 2 级，可空） */
const customerCode = defineModel<string | undefined>('customerCode')
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
const localePrefix = 'logistics.sales.monthly-trend.page'
/** 客户下拉刷新键 */
const customerSelectKey = ref(0)
/** 推移本表级联选项 URL（TaktSalesMonthlyTrends） */
const plantOptionsUrl = getSalesMonthlyTrendPlantOptionsUrl()
const customerOptionsUrl = getSalesMonthlyTrendCustomerOptionsUrl()

/** 第 2 级：工厂 → 客户 */
const customerApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 工厂变更：清空客户 */
watch(
  () => plantCode.value,
  () => {
    customerCode.value = undefined
    customerSelectKey.value += 1
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

.sales-monthly-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.sales-monthly-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.sales-monthly-trend-query-bar__control--customer {
  width: 14rem;
  min-width: 10rem;
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
