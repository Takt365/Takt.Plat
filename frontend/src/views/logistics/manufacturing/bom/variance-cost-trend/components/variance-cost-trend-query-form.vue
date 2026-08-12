<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/variance-cost-trend/components -->
<!-- 文件名称：variance-cost-trend-query-form.vue -->
<!-- 功能描述：差异成本推移查询栏（工厂、期间、物料类型、机种可选多选空=全部、产品可选与机种联动） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar variance-cost-trend-query-bar">
    <div class="variance-cost-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model="plantCode"
        :api-url="plantOptionsUrl"
        class="variance-cost-trend-query-bar__control variance-cost-trend-query-bar__control--plant"
        allow-clear
        show-search
        :placeholder="t('entity.bommaterialcost.plantcode')"
        @change="handlePlantChange"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        :disabled-date="isCostingPeriodMonthDisabled"
        class="variance-cost-trend-query-bar__control variance-cost-trend-query-bar__control--period"
        :placeholder="[t(`${localePrefix}.periodRange`), t(`${localePrefix}.periodRange`)]"
        @change="handlePeriodChange"
      />
      <TaktSelect
        v-model:value="materialType"
        :options="materialTypeOptions"
        class="variance-cost-trend-query-bar__control variance-cost-trend-query-bar__control--type"
        :allow-clear="false"
        show-search
        :disabled="!plantCode || materialTypeOptionsLoading"
        :placeholder="t('entity.bommaterialcost.materialtype')"
        @change="handleMaterialTypeChange"
      />
      <TaktSelect
        :key="`model-${modelSelectKey}-${materialType || ''}-${focusPeriod}`"
        v-model="modelCodes"
        :api-url="modelOptionsUrl"
        :api-params="modelApiParams"
        class="variance-cost-trend-query-bar__control variance-cost-trend-query-bar__control--model"
        multiple
        allow-clear
        show-search
        :disabled="!canSelectModel"
        :placeholder="t(`${localePrefix}.modelCodesOptional`)"
        @change="handleModelChange"
      />
      <TaktSelect
        :key="`product-${productSelectKey}-${materialType || ''}-${focusPeriod}-${modelCodesKey}`"
        v-model="productCodes"
        :api-url="productOptionsUrl"
        :api-params="productApiParams"
        class="variance-cost-trend-query-bar__control variance-cost-trend-query-bar__control--product"
        multiple
        allow-clear
        show-search
        :disabled="!canSelectProduct"
        :placeholder="t(`${localePrefix}.productCodesOptional`)"
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
 * 差异成本推移查询栏：工厂 → 期间 → 物料类型（默认 FERT）→ 机种（可选多选，空=全部）→ 产品（可选，随机种联动）
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import { getBomMaterialCostAnalysisPlantOptionsUrl } from '@/api/logistics/manufacturing/bom/material-cost-analysis'
import {
  getBomVarianceCostTrendModelOptionsUrl,
  getBomVarianceCostTrendProductOptionsUrl,
} from '@/api/logistics/manufacturing/bom/variance-cost-trend'
import type { TaktSelectOption } from '@/types/common'
import { isCostingPeriodMonthDisabled } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import {
  loadBomMaterialTypeOptionsWithDefault,
  pickDefaultBomMaterialType,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-type-options'

/** 工厂 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 物料类型 */
const materialType = defineModel<string | undefined>('materialType')
/** 机种（多选可选；空=对比月全部机种） */
const modelCodes = defineModel<string[]>('modelCodes', { default: () => [] })
/** 产品（多选可选；随已选机种联动） */
const productCodes = defineModel<string[]>('productCodes', { default: () => [] })
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
const localePrefix = 'logistics.manufacturing.bom.variance-cost-trend.page'
const modelSelectKey = ref(0)
const productSelectKey = ref(0)
const materialTypeOptions = ref<TaktSelectOption[]>([])
const materialTypeOptionsLoading = ref(false)
const materialTypeOptionsPlant = ref('')
let materialTypeLoadToken = 0
const plantOptionsUrl = getBomMaterialCostAnalysisPlantOptionsUrl()
const modelOptionsUrl = getBomVarianceCostTrendModelOptionsUrl()
const productOptionsUrl = getBomVarianceCostTrendProductOptionsUrl()

/** 期间最后月 */
const focusPeriod = computed(() => periodRange.value?.[1]?.trim() || periodRange.value?.[0]?.trim() || '')

/** 已选机种（去空白） */
const selectedModelCodes = computed(() =>
  (modelCodes.value ?? []).map((c) => String(c).trim()).filter(Boolean),
)

/** 机种 key（驱动产品下拉刷新） */
const modelCodesKey = computed(() => selectedModelCodes.value.join(','))

/** 可选机种（须工厂 + 期间 + 物料类型） */
const canSelectModel = computed(
  () =>
    !!plantCode.value?.trim()
    && !!focusPeriod.value
    && !!materialType.value?.trim()
    && !materialTypeOptionsLoading.value,
)

/** 可选产品（须已选至少一机种） */
const canSelectProduct = computed(
  () => canSelectModel.value && selectedModelCodes.value.length > 0,
)

/** 机种下拉参数（工厂 + 期间最后月 + 必选物料类型） */
const modelApiParams = computed(() => {
  if (!canSelectModel.value) {
    return { plantCode: '', focusPeriod: '' }
  }
  return {
    plantCode: plantCode.value!.trim(),
    focusPeriod: focusPeriod.value,
    materialType: materialType.value!.trim(),
  }
})

/** 产品下拉参数（工厂 + 期间最后月 + 物料类型 + 已选机种） */
const productApiParams = computed(() => {
  if (!canSelectProduct.value) {
    return { plantCode: '', focusPeriod: '', modelCodes: '' }
  }
  return {
    plantCode: plantCode.value!.trim(),
    focusPeriod: focusPeriod.value,
    materialType: materialType.value!.trim(),
    modelCodes: selectedModelCodes.value.join(','),
  }
})

/**
 * 拉取物料类型并默认 FERT
 * @param {string} plant 工厂
 * @returns {Promise<string | undefined>} 默认类型
 */
async function loadMaterialTypes(plant: string): Promise<string | undefined> {
  const token = ++materialTypeLoadToken
  materialTypeOptionsLoading.value = true
  try {
    const { options, defaultType } = await loadBomMaterialTypeOptionsWithDefault(plant)
    if (token !== materialTypeLoadToken) {
      return undefined
    }
    materialTypeOptions.value = options
    materialTypeOptionsPlant.value = plant
    return defaultType ?? pickDefaultBomMaterialType(options)
  } finally {
    if (token === materialTypeLoadToken) {
      materialTypeOptionsLoading.value = false
    }
  }
}

/** 刷新机种/产品下拉 */
function refreshModelProductSelects() {
  modelSelectKey.value += 1
  productSelectKey.value += 1
}

/** 工厂变更 */
async function handlePlantChange() {
  modelCodes.value = []
  productCodes.value = []
  refreshModelProductSelects()
  const plant = plantCode.value?.trim()
  if (!plant) {
    materialType.value = undefined
    materialTypeOptions.value = []
    materialTypeOptionsPlant.value = ''
    return
  }
  const def = await loadMaterialTypes(plant)
  materialType.value = def
  refreshModelProductSelects()
}

/** 期间变更 */
function handlePeriodChange() {
  modelCodes.value = []
  productCodes.value = []
  refreshModelProductSelects()
}

/** 物料类型变更：不可空，清空时回填默认 FERT */
function handleMaterialTypeChange() {
  if (!materialType.value?.trim()) {
    const def = pickDefaultBomMaterialType(materialTypeOptions.value)
    if (def) {
      materialType.value = def
    }
  }
  modelCodes.value = []
  productCodes.value = []
  refreshModelProductSelects()
}

/** 机种变更：清空产品并刷新产品选项（联动） */
function handleModelChange() {
  productCodes.value = []
  productSelectKey.value += 1
}

watch(
  () => plantCode.value?.trim() || '',
  async (plant) => {
    if (!plant || plant === materialTypeOptionsPlant.value) {
      return
    }
    const def = await loadMaterialTypes(plant)
    if (!materialType.value?.trim() && def) {
      materialType.value = def
      refreshModelProductSelects()
    }
  },
  { immediate: true },
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

.variance-cost-trend-query-bar__fields {
  min-width: 0;
}

.variance-cost-trend-query-bar__control--plant {
  width: 9rem;
  min-width: 7rem;
}

.variance-cost-trend-query-bar__control--period {
  width: 14rem;
  min-width: 12rem;
}

.variance-cost-trend-query-bar__control--type {
  width: 7rem;
  min-width: 6rem;
}

/* 多选：基准宽=普通单选（工厂）宽，控件自动 ×2 并 responsive 溢出 */
.variance-cost-trend-query-bar__control--model,
.variance-cost-trend-query-bar__control--product {
  --takt-select-base-width: 9rem;
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
