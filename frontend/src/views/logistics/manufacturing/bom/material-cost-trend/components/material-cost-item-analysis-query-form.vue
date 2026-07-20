<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/components -->
<!-- 文件名称：material-cost-item-analysis-query-form.vue -->
<!-- 功能描述：BOM 成本推移查询栏：工厂 / 机种（可选过滤）/ 产品（必选）/ 期间 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar material-cost-analysis-query-bar">
    <div class="material-cost-analysis-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.bommaterialcost.plantcode')"
        @change="handlePlantChange"
      />
      <TaktSelect
        :key="modelSelectKey"
        v-model:value="modelCode"
        api-url="TaktBomMaterialCosts/model-options"
        :api-params="modelApiParams"
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--model"
        allow-clear
        :disabled="!plantCode"
        :placeholder="t('entity.bommaterialcost.modelcode')"
        @change="handleModelChange"
      />
      <TaktSelect
        :key="productSelectKey"
        v-model:value="productCode"
        :api-url="productApiUrl"
        :api-params="productApiParams"
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--product"
        allow-clear
        :disabled="!plantCode"
        :placeholder="t('entity.bommaterialcostitem.productcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`),
        ]"
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
 * BOM 成本推移查询栏：产品必选；机种仅用于缩小产品下拉
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import { MATERIAL_COST_ANALYSIS_LOCALE_PREFIX } from '../composables/use-material-cost-item-analysis'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 机种编码（可选） */
const modelCode = defineModel<string | undefined>('modelCode')
/** 产品编码（必选） */
const productCode = defineModel<string | undefined>('productCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
const props = defineProps<{
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX
const modelSelectKey = ref(0)
const productSelectKey = ref(0)

const modelApiParams = computed(() => ({
  plantCode: plantCode.value || undefined,
}))

/** 有机种走机种下产品；无机种走工厂下产品 */
const productApiUrl = computed(() =>
  modelCode.value?.trim()
    ? 'TaktBomMaterialCostItems/product-options-by-model'
    : 'TaktBomMaterialCostItems/options',
)

const productApiParams = computed(() => ({
  modelCode: modelCode.value || undefined,
  plantCode: plantCode.value || undefined,
}))

function handlePlantChange() {
  modelCode.value = undefined
  productCode.value = undefined
  modelSelectKey.value += 1
  productSelectKey.value += 1
}

function handleModelChange() {
  productCode.value = undefined
  productSelectKey.value += 1
}
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

.material-cost-analysis-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.material-cost-analysis-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.material-cost-analysis-query-bar__control--product {
  width: 14rem;
  min-width: 10rem;
}

.material-cost-analysis-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
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
