<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/defect-monthly/components -->
<!-- 文件名称：defect-monthly-query-form.vue -->
<!-- 功能描述：月生产不良推移查询栏（工厂/期间/机种/不良类别） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar defect-monthly-query-bar">
    <div class="defect-monthly-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="defect-monthly-query-bar__control defect-monthly-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.assydefect.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="defect-monthly-query-bar__control defect-monthly-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`),
        ]"
      />
      <a-input
        v-model:value="modelCode"
        class="defect-monthly-query-bar__control defect-monthly-query-bar__control--model"
        allow-clear
        :placeholder="t(`${localePrefix}.modelCode`)"
      />
      <a-select
        v-model:value="defectCategory"
        class="defect-monthly-query-bar__control defect-monthly-query-bar__control--category"
        allow-clear
        :placeholder="t(`${localePrefix}.defectCategory`)"
        :options="defectCategoryOptions"
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
 * 月生产不良推移查询栏：工厂 + 期间 + 机种 + 不良类别
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 机种编码 */
const modelCode = defineModel<string | undefined>('modelCode')
/** 不良类别 */
const defectCategory = defineModel<string | undefined>('defectCategory')
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
const localePrefix = 'logistics.manufacturing.defect.defect-monthly.page'

/** 不良类别选项 */
const defectCategoryOptions = computed(() => [
  { value: 'assy', label: t(`${localePrefix}.defectCategoryOptions.assy`) },
  { value: 'pcba', label: t(`${localePrefix}.defectCategoryOptions.pcba`) },
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

.defect-monthly-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.defect-monthly-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.defect-monthly-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.defect-monthly-query-bar__control--category {
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
