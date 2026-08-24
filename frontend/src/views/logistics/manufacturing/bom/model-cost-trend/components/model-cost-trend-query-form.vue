<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/model-cost-trend/components -->
<!-- 文件名称：model-cost-trend-query-form.vue -->
<!-- 功能描述：机种成本推移查询栏（工厂 → 期间 → 物料类型先拉全量再默认 FERT → 机种/物料） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar model-cost-trend-query-bar">
    <div class="model-cost-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model="plantCode"
        :api-url="plantOptionsUrl"
        class="model-cost-trend-query-bar__control model-cost-trend-query-bar__control--plant"
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
        class="model-cost-trend-query-bar__control model-cost-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`)]"
        @change="handlePeriodChange"
      />
      <TaktSelect
        v-model:value="materialType"
        :options="materialTypeOptions"
        class="model-cost-trend-query-bar__control model-cost-trend-query-bar__control--type"
        :allow-clear="false"
        show-search
        :disabled="!canSelectType || materialTypeOptionsLoading"
        :placeholder="t('entity.bommaterialcost.materialtype')"
        @change="handleMaterialTypeChange"
      />
      <TaktSelect
        :key="`model-${modelSelectKey}-${materialType || ''}-${periodKey}`"
        v-model="modelCodes"
        :api-url="modelOptionsUrl"
        :api-params="modelApiParams"
        class="model-cost-trend-query-bar__control model-cost-trend-query-bar__control--model"
        multiple
        allow-clear
        show-search
        :disabled="!canSelectModel"
        :placeholder="t(`${localePrefix}.modelCodesOptional`)"
        @change="handleModelCodesChange"
      />
      <TaktSelect
        :key="`component-${componentSelectKey}-${periodKey}-${modelCodesKey}`"
        v-model="componentCodes"
        :api-url="componentOptionsUrl"
        :api-params="componentApiParams"
        class="model-cost-trend-query-bar__control model-cost-trend-query-bar__control--component"
        multiple
        allow-clear
        show-search
        remote-search
        virtual
        :disabled="!canSelectComponent"
        :placeholder="t(`${localePrefix}.componentCodesOptional`)"
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
 * 机种成本推移查询栏：
 * - 物料类型：先 get material-type-options 全量，再默认选中 FERT（仅影响机种 options）
 * - 机种：工厂 + 整个期间 + 物料类型
 * - 物料：工厂 + 整个期间 + X+F 去重；remote-search + virtual
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getBomCostOptionMaterialOptionsUrl,
  getBomCostOptionModelOptionsUrl,
  getBomCostOptionPlantOptionsUrl,
} from '@/api/logistics/manufacturing/bom/cost-option'
import type { TaktSelectOption } from '@/types/common'
import { buildBomCostOptionParams, hasBomCostOptionPeriod } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-cost-option-params'
import { isCostingPeriodMonthDisabled } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import {
  loadBomMaterialTypeOptionsWithDefault,
  pickDefaultBomMaterialType,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-type-options'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 物料类型（本表 MaterialType；加载后默认 FERT） */
const materialType = defineModel<string | undefined>('materialType')
/** 机种编码多选（空=期间最后月全部） */
const modelCodes = defineModel<string[]>('modelCodes', { default: () => [] })
/** 物料/组件编码多选（空=期间最后月全部） */
const componentCodes = defineModel<string[]>('componentCodes', { default: () => [] })
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
const localePrefix = 'logistics.manufacturing.bom.model-cost-trend.page'
/** 机种下拉刷新键 */
const modelSelectKey = ref(0)
/** 物料下拉刷新键 */
const componentSelectKey = ref(0)
/** 本表物料类型全量选项 */
const materialTypeOptions = ref<TaktSelectOption[]>([])
/** 物料类型选项 loading */
const materialTypeOptionsLoading = ref(false)
/** 当前 options 对应的工厂 */
const materialTypeOptionsPlant = ref('')
/** 选项请求序号 */
let materialTypeLoadToken = 0
const plantOptionsUrl = getBomCostOptionPlantOptionsUrl()
const modelOptionsUrl = getBomCostOptionModelOptionsUrl()
const componentOptionsUrl = getBomCostOptionMaterialOptionsUrl()
/** 期间键（驱动下拉刷新） */
const periodKey = computed(
  () => `${periodRange.value?.[0] || ''}_${periodRange.value?.[1] || ''}`,
)
/** 已选机种键（空=全部机种，物料不过滤机种） */
const modelCodesKey = computed(
  () => (modelCodes.value ?? []).map((c) => c.trim()).filter(Boolean).sort().join(','),
)
/** 期间是否可解析 */
const hasPeriod = computed(() => hasBomCostOptionPeriod(periodRange.value))
/** 类型：工厂 + 期间 */
const canSelectType = computed(() => !!plantCode.value?.trim() && hasPeriod.value)
/** 机种：工厂 + 期间 + 类型；机种可空 */
const canSelectModel = computed(
  () => canSelectType.value && !!materialType.value?.trim() && !materialTypeOptionsLoading.value,
)
/** 物料：工厂 + 期间即可；机种可空=不过滤 */
const canSelectComponent = computed(
  () => !!plantCode.value?.trim() && hasPeriod.value,
)

/** 机种下拉参数（工厂 + 整个期间 + 类型） */
const modelApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    periodRange: periodRange.value,
    materialType: materialType.value,
  }),
)

/** 物料下拉参数（工厂 + 期间；机种多选可空过滤） */
const componentApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    periodRange: periodRange.value,
    modelCodes: modelCodes.value,
  }),
)

/** 刷新机种下拉 */
function refreshModelSelect() {
  modelSelectKey.value += 1
}

/** 刷新物料下拉 */
function refreshComponentSelect() {
  componentSelectKey.value += 1
}

/** 刷新机种+物料下拉 */
function refreshModelComponentSelects() {
  refreshModelSelect()
  refreshComponentSelect()
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

/** 机种变更：清空物料并按新机种（可空=全部）重拉物料 */
function handleModelCodesChange() {
  componentCodes.value = []
  refreshComponentSelect()
}

/** 工厂变更：清空机种/物料（类型由 watch 重拉并默认 FERT） */
function handlePlantChange() {
  modelCodes.value = []
  componentCodes.value = []
  refreshModelComponentSelects()
}

/** 期间变更：选项跟整个期间走，重拉类型并清空已选机种/物料 */
async function handlePeriodChange() {
  modelCodes.value = []
  componentCodes.value = []
  const p = plantCode.value?.trim()
  if (p && hasBomCostOptionPeriod(periodRange.value)) {
    const defaultType = await ensureMaterialTypeOptions(p)
    materialType.value = defaultType
  }
  refreshModelComponentSelects()
}

/** 物料类型变更：清空机种；机种空则物料不过滤机种并重拉 */
function handleMaterialTypeChange() {
  applyDefaultMaterialTypeIfEmpty()
  modelCodes.value = []
  componentCodes.value = []
  refreshModelComponentSelects()
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
      modelCodes.value = []
      componentCodes.value = []
      refreshModelComponentSelects()
      return
    }
    modelCodes.value = []
    componentCodes.value = []
    const defaultType = await ensureMaterialTypeOptions(p)
    materialType.value = defaultType
    refreshModelComponentSelects()
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

.model-cost-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.model-cost-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.model-cost-trend-query-bar__control--type {
  width: 9rem;
  min-width: 7rem;
}

/* 多选：基准宽=普通单选（工厂）宽，控件自动 ×2 并 responsive 溢出 */
.model-cost-trend-query-bar__control--model,
.model-cost-trend-query-bar__control--component {
  --takt-select-base-width: 10rem;
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
