<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/price-delta-trend/components -->
<!-- 文件名称：price-delta-trend-query-form.vue -->
<!-- 功能描述：查询栏：工厂→期间→物料类型(默认FERT)→机种→产品（机种/产品联动） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar price-delta-trend-query-bar">
    <div class="price-delta-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        :api-url="plantOptionsUrl"
        class="price-delta-trend-query-bar__control price-delta-trend-query-bar__control--plant"
        allow-clear
        show-search
        :placeholder="t('common.page.entity.plantcode')"
        @change="handlePlantChange"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        :disabled-date="isCostingPeriodMonthDisabled"
        class="price-delta-trend-query-bar__control price-delta-trend-query-bar__control--period"
        :placeholder="[t(`${localePrefix}.periodRange`), t(`${localePrefix}.periodRange`)]"
        @change="handlePeriodChange"
      />
      <TaktSelect
        v-model:value="materialType"
        :options="materialTypeOptions"
        class="price-delta-trend-query-bar__control price-delta-trend-query-bar__control--type"
        :allow-clear="false"
        show-search
        :disabled="!canSelectType || materialTypeOptionsLoading"
        :placeholder="t('entity.bommaterialcost.materialtype')"
        @change="handleMaterialTypeChange"
      />
      <TaktSelect
        :key="`model-${modelSelectKey}-${materialType || ''}-${periodKey}`"
        v-model:value="modelCode"
        :api-url="modelOptionsUrl"
        :api-params="modelApiParams"
        class="price-delta-trend-query-bar__control price-delta-trend-query-bar__control--model"
        allow-clear
        show-search
        :disabled="!canSelectModel"
        :placeholder="t(`${localePrefix}.modelCodeOptional`)"
        @change="handleModelChange"
      />
      <TaktSelect
        :key="`product-${productSelectKey}-${materialType || ''}-${modelCode || ''}-${periodKey}`"
        v-model:value="productCode"
        :api-url="productOptionsUrl"
        :api-params="productApiParams"
        class="price-delta-trend-query-bar__control price-delta-trend-query-bar__control--product"
        allow-clear
        show-search
        :disabled="!canSelectProduct"
        :placeholder="t(`${localePrefix}.productCodeOptional`)"
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
 * 成本差异推移查询栏：
 * 工厂 + 期间 + 物料类型(默认 FERT) + 机种(可选) + 产品(可选且随机种联动)
 * → 后端按同一口径算 0价格组 / 价格差异组
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getBomCostOptionModelOptionsUrl,
  getBomCostOptionPlantOptionsUrl,
  getBomCostOptionProductOptionsUrl,
} from '@/api/logistics/manufacturing/bom/cost-option'
import type { TaktSelectOption } from '@/types/common'
import { buildBomCostOptionParams, hasBomCostOptionPeriod } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-cost-option-params'
import { isCostingPeriodMonthDisabled } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import {
  loadBomMaterialTypeOptionsWithDefault,
  pickDefaultBomMaterialType,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-type-options'

/** 工厂 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 物料类型（本表；加载后默认 FERT） */
const materialType = defineModel<string | undefined>('materialType')
/** 机种（可选） */
const modelCode = defineModel<string | undefined>('modelCode')
/** 产品（可选；随机种联动） */
const productCode = defineModel<string | undefined>('productCode')
/** 核算期间（必选） */
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
const localePrefix = 'logistics.manufacturing.bom.price-delta-trend.page'
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
/** 选项请求序号（防竞态） */
let materialTypeLoadToken = 0

const plantOptionsUrl = getBomCostOptionPlantOptionsUrl()
const modelOptionsUrl = getBomCostOptionModelOptionsUrl()
const productOptionsUrl = getBomCostOptionProductOptionsUrl()
/** 期间键（驱动下拉刷新） */
const periodKey = computed(
  () => `${periodRange.value?.[0] || ''}_${periodRange.value?.[1] || ''}`,
)

/** 期间是否可解析 */
const hasPeriod = computed(() => hasBomCostOptionPeriod(periodRange.value))
/** 类型：工厂 + 期间 */
const canSelectType = computed(() => !!plantCode.value?.trim() && hasPeriod.value)
/** 机种：工厂 + 期间 + 类型；机种可空 */
const canSelectModel = computed(
  () => canSelectType.value && !!materialType.value?.trim(),
)
/** 产品：机种可空时不过滤机种 */
const canSelectProduct = computed(() => canSelectModel.value)

/** 机种下拉参数（工厂 + 期间 + 必选物料类型） */
const modelApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    periodRange: periodRange.value,
    materialType: materialType.value,
  }),
)

/** 产品下拉参数（工厂 + 期间 + 类型；机种可选过滤） */
const productApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    periodRange: periodRange.value,
    materialType: materialType.value,
    modelCode: modelCode.value,
  }),
)

/** 刷新机种/产品下拉 */
function refreshModelProductSelects() {
  modelSelectKey.value += 1
  productSelectKey.value += 1
}

/**
 * 按工厂拉取物料类型全量选项，返回默认选中值（优先 FERT）
 * @param {string} plant 工厂
 * @returns {Promise<string | undefined>} 默认类型
 */
async function ensureMaterialTypeOptions(plant: string): Promise<string | undefined> {
  const token = ++materialTypeLoadToken
  materialTypeOptionsLoading.value = true
  try {
    const { options, defaultType } = await loadBomMaterialTypeOptionsWithDefault(
      plant,
      periodRange.value,
    )
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

/** 工厂变更：清空下游（类型由 watch 重拉并默认 FERT） */
function handlePlantChange() {
  modelCode.value = undefined
  productCode.value = undefined
  refreshModelProductSelects()
}

/** 期间变更：重拉类型并清空机种/产品 */
async function handlePeriodChange() {
  modelCode.value = undefined
  productCode.value = undefined
  const p = plantCode.value?.trim()
  if (p && hasBomCostOptionPeriod(periodRange.value)) {
    const defaultType = await ensureMaterialTypeOptions(p)
    materialType.value = defaultType
  }
  refreshModelProductSelects()
}

/** 物料类型变更：清空下游；清空时回填 FERT */
function handleMaterialTypeChange() {
  applyDefaultMaterialTypeIfEmpty()
  modelCode.value = undefined
  productCode.value = undefined
  refreshModelProductSelects()
}

/** 机种变更：清空产品并刷新产品选项（联动） */
function handleModelChange() {
  productCode.value = undefined
  productSelectKey.value += 1
}

watch(
  plantCode,
  async (plant) => {
    const p = plant?.trim()
    if (!p || !hasBomCostOptionPeriod(periodRange.value)) {
      materialTypeLoadToken += 1
      materialTypeOptions.value = []
      materialTypeOptionsPlant.value = ''
      materialTypeOptionsLoading.value = false
      if (!p) {
        materialType.value = undefined
      }
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

/** 父级重置把类型清空、工厂未变时：重新默认 FERT */
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

.price-delta-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.price-delta-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.price-delta-trend-query-bar__control--type {
  width: 9rem;
  min-width: 7rem;
}

.price-delta-trend-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.price-delta-trend-query-bar__control--product {
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
