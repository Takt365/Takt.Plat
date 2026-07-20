<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-monthly-trend/components -->
<!-- 文件名称：ec-monthly-trend-query-form.vue -->
<!-- 功能描述：月设变/月实施推移查询栏（工厂/期间 + Tab 条件筛选） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar ec-monthly-trend-query-bar">
    <div class="ec-monthly-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        api-url="TaktPlants/options"
        class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--plant"
        allow-clear
        :placeholder="t('entity.ecgijutsu.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`),
        ]"
      />
      <template v-if="props.activeTab === 'issue'">
        <TaktSelect
          v-model:value="ecDistinction"
          dict-type="logistics_ec_distinction_category"
          class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--distinction"
          allow-clear
          :placeholder="t('entity.ec.distinction')"
        />
        <TaktSelect
          v-model:value="changeStatus"
          dict-type="logistics_ec_status"
          class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--status"
          allow-clear
          :placeholder="t('entity.ecgijutsu.changestatus')"
        />
        <TaktSelect
          v-model:value="ecStatus"
          dict-type="logistics_ec_gijutsu_status"
          class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--ec-status"
          allow-clear
          :placeholder="t('entity.ecgijutsu.ecstatus')"
        />
      </template>
      <a-input
        v-else
        v-model:value="deptCode"
        class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--dept"
        allow-clear
        :placeholder="t(`${localePrefix}.deptCode`)"
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
 * 月设变/月实施推移查询栏：工厂 + 期间 + Tab 条件筛选
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 区分 */
const ecDistinction = defineModel<number | undefined>('ecDistinction')
/** 变更状态 */
const changeStatus = defineModel<number | undefined>('changeStatus')
/** 设变状态 */
const ecStatus = defineModel<number | undefined>('ecStatus')
/** 部门编码 */
const deptCode = defineModel<string>('deptCode', { default: '' })
const props = defineProps<{
  /** 当前 Tab */
  activeTab: 'issue' | 'implement'
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.engineering-change.ec-monthly-trend.page'
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

.ec-monthly-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.ec-monthly-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.ec-monthly-trend-query-bar__control--distinction,
.ec-monthly-trend-query-bar__control--status,
.ec-monthly-trend-query-bar__control--ec-status,
.ec-monthly-trend-query-bar__control--dept {
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
