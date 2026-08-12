<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/components -->
<!-- 文件名称：material-cost-item-analysis-query-form.vue -->
<!-- 功能描述：产品成本推移查询栏：工厂 → 期间 → 物料类型(先拉全量再默认 FERT) → 机种 → 产品 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar material-cost-analysis-query-bar">
    <div class="material-cost-analysis-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        :api-url="plantOptionsUrl"
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--plant"
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
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`)]"
      />
      <TaktSelect
        v-model:value="materialType"
        :options="materialTypeOptions"
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--type"
        :allow-clear="false"
        show-search
        :disabled="!plantCode || materialTypeOptionsLoading"
        :placeholder="t('entity.bommaterialcost.materialtype')"
        @change="handleMaterialTypeChange"
      />
      <TaktSelect
        :key="`model-${modelSelectKey}-${materialType || ''}`"
        v-model:value="modelCode"
        :api-url="modelOptionsUrl"
        :api-params="modelApiParams"
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--model"
        allow-clear
        show-search
        :disabled="!canSelectModelOrProduct"
        :placeholder="t('entity.bommaterialcost.modelcode')"
        @change="handleModelChange"
      />
      <TaktSelect
        :key="`product-${productSelectKey}-${materialType || ''}-${modelCode || ''}`"
        v-model:value="productCode"
        :api-url="productOptionsUrl"
        :api-params="productApiParams"
        class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--product"
        allow-clear
        show-search
        :disabled="!canSelectModelOrProduct"
        :placeholder="t('entity.bommaterialcost.productcode')"
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
 * 产品成本推移查询栏：
 * - 物料类型：先 get material-type-options 全量，再默认选中 FERT
 * - 机种/产品：本表 model-options / product-options
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getBomMaterialCostAnalysisModelOptionsUrl,
  getBomMaterialCostAnalysisPlantOptionsUrl,
  getBomMaterialCostAnalysisProductOptionsUrl,
} from '@/api/logistics/manufacturing/bom/material-cost-analysis'
import type { TaktSelectOption } from '@/types/common'
import { isCostingPeriodMonthDisabled } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import {
  loadBomMaterialTypeOptionsWithDefault,
  pickDefaultBomMaterialType,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-type-options'
import { MATERIAL_COST_ANALYSIS_LOCALE_PREFIX } from '../composables/use-material-cost-item-analysis'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 物料类型（本表 MaterialType；加载后默认 FERT） */
const materialType = defineModel<string | undefined>('materialType')
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
/** 机种下拉刷新键 */
const modelSelectKey = ref(0)
/** 产品下拉刷新键 */
const productSelectKey = ref(0)
/** 本表物料类型全量选项 */
const materialTypeOptions = ref<TaktSelectOption[]>([])
/** 物料类型选项 loading */
const materialTypeOptionsLoading = ref(false)
/** 当前 options 对应的工厂 */
const materialTypeOptionsPlant = ref('')
/** 选项请求序号 */
let materialTypeLoadToken = 0
const plantOptionsUrl = getBomMaterialCostAnalysisPlantOptionsUrl()
const modelOptionsUrl = getBomMaterialCostAnalysisModelOptionsUrl()
const productOptionsUrl = getBomMaterialCostAnalysisProductOptionsUrl()

/** 可选机种/产品：须工厂 + 物料类型 */
const canSelectModelOrProduct = computed(
  () => !!plantCode.value?.trim() && !!materialType.value?.trim(),
)

/** 机种下拉参数（工厂 + 必选物料类型） */
const modelApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const type = materialType.value?.trim()
  if (!plant || !type) {
    return undefined
  }
  return { plantCode: plant, materialType: type }
})

/** 产品下拉参数（工厂 + 必选物料类型；机种可选） */
const productApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const type = materialType.value?.trim()
  if (!plant || !type) {
    return undefined
  }
  const params: Record<string, string> = { plantCode: plant, materialType: type }
  const model = modelCode.value?.trim()
  if (model) {
    params.modelCode = model
  }
  return params
})

/** 刷新机种/产品下拉 */
function refreshModelProductSelects() {
  modelSelectKey.value += 1
  productSelectKey.value += 1
}

/**
 * 按工厂拉取物料类型全量选项
 * @param {string} plant 工厂
 * @returns {Promise<string | undefined>} 默认类型（优先 FERT）
 */
async function ensureMaterialTypeOptions(plant: string): Promise<string | undefined> {
  const token = ++materialTypeLoadToken
  materialTypeOptionsLoading.value = true
  try {
    const { options, defaultType } = await loadBomMaterialTypeOptionsWithDefault(plant)
    if (token !== materialTypeLoadToken) {
      return undefined
    }
    materialTypeOptions.value = options
    materialTypeOptionsPlant.value = plant
    return defaultType
  } finally {
    if (token === materialTypeLoadToken) {
      materialTypeOptionsLoading.value = false
    }
  }
}

/** 空类型时回填默认 FERT */
function applyDefaultMaterialTypeIfEmpty() {
  if (materialType.value?.trim()) {
    return
  }
  const def = pickDefaultBomMaterialType(materialTypeOptions.value)
  if (def) {
    materialType.value = def
  }
}

/** 工厂变更 */
function handlePlantChange() {
  modelCode.value = undefined
  productCode.value = undefined
  refreshModelProductSelects()
}

/** 物料类型变更 */
function handleMaterialTypeChange() {
  applyDefaultMaterialTypeIfEmpty()
  modelCode.value = undefined
  productCode.value = undefined
  refreshModelProductSelects()
}

/** 机种变更 */
function handleModelChange() {
  productCode.value = undefined
  productSelectKey.value += 1
}

watch(
  plantCode,
  async (plant) => {
    const p = plant?.trim()
    if (!p) {
      materialTypeLoadToken += 1
      materialTypeOptions.value = []
      materialTypeOptionsPlant.value = ''
      materialTypeOptionsLoading.value = false
      materialType.value = undefined
      modelCode.value = undefined
      productCode.value = undefined
      refreshModelProductSelects()
      return
    }
    modelCode.value = undefined
    productCode.value = undefined
    const defaultType = await ensureMaterialTypeOptions(p)
    materialType.value = defaultType
    refreshModelProductSelects()
  },
  { immediate: true },
)

watch(materialType, async (type) => {
  if (type?.trim()) {
    return
  }
  const p = plantCode.value?.trim()
  if (!p) {
    return
  }
  if (materialTypeOptionsPlant.value !== p) {
    const defaultType = await ensureMaterialTypeOptions(p)
    if (!materialType.value?.trim()) {
      materialType.value = defaultType
    }
    return
  }
  applyDefaultMaterialTypeIfEmpty()
})
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

.material-cost-analysis-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.material-cost-analysis-query-bar__control--type {
  width: 9rem;
  min-width: 7rem;
}

.material-cost-analysis-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.material-cost-analysis-query-bar__control--product {
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
