<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/model-moving-price/components -->
<!-- 文件名称：model-moving-price-query-form.vue -->
<!-- 功能描述：机种成本推移查询栏（工厂 → 机种 + 期间） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar model-moving-price-query-bar">
    <div class="model-moving-price-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="model-moving-price-query-bar__control model-moving-price-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.bommaterialcost.plantcode')"
        @change="handlePlantChange"
      />
      <TaktSelect
        :key="modelSelectKey"
        v-model:value="modelCode"
        api-url="TaktBomMaterialCosts/model-options"
        :api-params="modelApiParams"
        class="model-moving-price-query-bar__control model-moving-price-query-bar__control--model"
        allow-clear
        :disabled="!plantCode"
        :placeholder="t('entity.bommaterialcost.modelcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="model-moving-price-query-bar__control model-moving-price-query-bar__control--period"
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
 * 机种成本推移查询栏：工厂 → 机种 + 核算期间
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 机种编码 */
const modelCode = defineModel<string | undefined>('modelCode')
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
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.bom.model-moving-price.page'
/** 机种下拉刷新键 */
const modelSelectKey = ref(0)

/** 机种下拉参数 */
const modelApiParams = computed(() => ({
  plantCode: plantCode.value || undefined,
}))

/** 工厂变更：清空机种 */
function handlePlantChange() {
  modelCode.value = undefined
  modelSelectKey.value += 1
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

.model-moving-price-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.model-moving-price-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.model-moving-price-query-bar__control--period {
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
