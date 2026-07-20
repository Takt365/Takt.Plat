<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint-trend/components -->
<!-- 文件名称：customer-complaint-trend-query-form.vue -->
<!-- 功能描述：顾客投诉推移查询栏（工厂/期间/客户/投诉类型） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar customer-complaint-trend-query-bar">
    <div class="customer-complaint-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="customer-complaint-trend-query-bar__control customer-complaint-trend-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.customercomplaint.relatedplant')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="customer-complaint-trend-query-bar__control customer-complaint-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`),
        ]"
      />
      <TaktSelect
        v-model:value="customerCode"
        api-url="TaktCustomers/options"
        class="customer-complaint-trend-query-bar__control customer-complaint-trend-query-bar__control--customer"
        allow-clear
        :placeholder="t(`${localePrefix}.customerCode`)"
      />
      <TaktSelect
        v-model:value="complaintType"
        dict-type="logistics_quality_complaint_type"
        class="customer-complaint-trend-query-bar__control customer-complaint-trend-query-bar__control--complaint-type"
        allow-clear
        :placeholder="t('entity.customercomplaint.complainttype')"
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
 * 顾客投诉推移查询栏：工厂 + 期间 + 客户 + 投诉类型
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 客户编码 */
const customerCode = defineModel<string | undefined>('customerCode')
/** 投诉类型 */
const complaintType = defineModel<number | undefined>('complaintType')
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
const localePrefix = 'logistics.quality.complaint.customer-complaint-trend.page'
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

.customer-complaint-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.customer-complaint-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.customer-complaint-trend-query-bar__control--customer {
  width: 12rem;
  min-width: 9rem;
}

.customer-complaint-trend-query-bar__control--complaint-type {
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
