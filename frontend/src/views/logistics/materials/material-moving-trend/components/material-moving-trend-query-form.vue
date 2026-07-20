<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-moving-trend/components -->
<!-- 文件名称：material-moving-trend-query-form.vue -->
<!-- 功能描述：物料移动价格推移查询栏（工厂/期间/评估/物料） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar material-moving-trend-query-bar">
    <div class="material-moving-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="material-moving-trend-query-bar__control material-moving-trend-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.materialmovingprice.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="material-moving-trend-query-bar__control material-moving-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`),
        ]"
      />
      <TaktSelect
        v-model:value="valuation"
        dict-type="logistics_valuation_class_category"
        class="material-moving-trend-query-bar__control material-moving-trend-query-bar__control--valuation"
        allow-clear
        :placeholder="t('entity.materialmovingprice.valuation')"
      />
      <a-input
        v-model:value="materialCode"
        class="material-moving-trend-query-bar__control material-moving-trend-query-bar__control--material"
        allow-clear
        :placeholder="t(`${localePrefix}.materialCode`)"
        @press-enter="emit('search')"
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
 * 物料移动价格推移查询栏：工厂 + 期间 + 评估类别 + 物料编码
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 评估类别 */
const valuation = defineModel<string | undefined>('valuation')
/** 物料编码 */
const materialCode = defineModel<string>('materialCode', { default: '' })
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
const localePrefix = 'logistics.materials.material-moving-trend.page'
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

.material-moving-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.material-moving-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.material-moving-trend-query-bar__control--valuation {
  width: 10rem;
  min-width: 8rem;
}

.material-moving-trend-query-bar__control--material {
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
