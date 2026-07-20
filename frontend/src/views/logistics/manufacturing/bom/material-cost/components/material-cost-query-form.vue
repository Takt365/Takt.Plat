<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost/components -->
<!-- 文件名称：material-cost-query-form.vue -->
<!-- 功能描述：浏览页查询栏（TaktQueryBar 同款横排样式：工厂/机种/核算单月 + 查询/重置） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar material-cost-query-bar">
    <div class="material-cost-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="material-cost-query-bar__control material-cost-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.bommaterialcost.plantcode')"
        @change="handlePlantChange"
      />
      <TaktSelect
        :key="modelSelectKey"
        v-model:value="modelCode"
        api-url="TaktBomMaterialCosts/model-options"
        :api-params="modelApiParams"
        class="material-cost-query-bar__control material-cost-query-bar__control--model"
        allow-clear
        :disabled="!plantCode"
        :placeholder="t('entity.bommaterialcost.modelcode')"
      />
      <a-date-picker
        v-model:value="costingMonth"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="material-cost-query-bar__control material-cost-query-bar__control--period"
        :placeholder="t('logistics.manufacturing.bom.material-cost.page.costingMonth')"
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
 * 浏览页查询栏：工厂 → 机种 → 核算单月（视觉对齐 TaktQueryBar）
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 机种编码 */
const modelCode = defineModel<string | undefined>('modelCode')
/** 核算单月 yyyy-MM */
const costingMonth = defineModel<string | null>('costingMonth')
const props = defineProps<{
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 机种下拉刷新键 */
const modelSelectKey = ref(0)

/** 机种下拉参数 */
const modelApiParams = computed(() => ({
  plantCode: plantCode.value || undefined,
}))

/** 工厂变更：清空机种并刷新下拉 */
function handlePlantChange() {
  modelCode.value = undefined
  modelSelectKey.value += 1
}
</script>

<style scoped>
/* 与 components/business/takt-query-bar 同款壳体 */
.takt-query-bar {
  margin: 4px;
  padding: 4px;
  display: flex;
  align-items: center;
  gap: 8px;
  width: 100%;
  box-sizing: border-box;
}

.material-cost-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.material-cost-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.material-cost-query-bar__control--period {
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
