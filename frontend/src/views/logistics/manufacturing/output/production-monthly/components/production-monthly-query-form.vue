<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-monthly/components -->
<!-- 文件名称：production-monthly-query-form.vue -->
<!-- 功能描述：月生产推移查询栏（工厂/期间/机种/产出类别） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar production-monthly-query-bar">
    <div class="production-monthly-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="production-monthly-query-bar__control production-monthly-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.assyoutput.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="production-monthly-query-bar__control production-monthly-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`),
        ]"
      />
      <a-input
        v-model:value="modelCode"
        class="production-monthly-query-bar__control production-monthly-query-bar__control--model"
        allow-clear
        :placeholder="t(`${localePrefix}.modelCode`)"
      />
      <a-select
        v-model:value="outputCategory"
        class="production-monthly-query-bar__control production-monthly-query-bar__control--category"
        allow-clear
        :placeholder="t(`${localePrefix}.outputCategory`)"
        :options="outputCategoryOptions"
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
 * 月生产推移查询栏：工厂 + 期间 + 机种 + 产出类别
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 机种 */
const modelCode = defineModel<string | undefined>('modelCode')
/** 产出类别 */
const outputCategory = defineModel<string | undefined>('outputCategory')
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
const localePrefix = 'logistics.manufacturing.output.production-monthly.page'

/** 产出类别选项 */
const outputCategoryOptions = computed(() => [
  { value: 'assy', label: t(`${localePrefix}.outputCategoryOptions.assy`) },
  { value: 'pcba', label: t(`${localePrefix}.outputCategoryOptions.pcba`) },
])
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

.production-monthly-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.production-monthly-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.production-monthly-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.production-monthly-query-bar__control--category {
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
