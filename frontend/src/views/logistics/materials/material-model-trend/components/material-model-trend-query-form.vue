<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-model-trend/components -->
<!-- 文件名称：material-model-trend-query-form.vue -->
<!-- 功能描述：物料机种推移查询栏（工厂/期间/物料类型/评估/物料） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar material-model-trend-query-bar">
    <div class="material-model-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        :api-url="plantOptionsUrl"
        class="material-model-trend-query-bar__control material-model-trend-query-bar__control--plant"
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
        class="material-model-trend-query-bar__control material-model-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`)]"
      />
      <TaktSelect
        v-model:value="materialType"
        :options="materialTypeOptions"
        class="material-model-trend-query-bar__control material-model-trend-query-bar__control--type"
        :allow-clear="false"
        show-search
        :disabled="!plantCode?.trim() || materialTypeOptionsLoading"
        :placeholder="t('entity.bommaterialcost.materialtype')"
        @change="handleMaterialTypeChange"
      />
      <TaktSelect
        v-model:value="valuation"
        :api-url="valuationOptionsUrl"
        :api-params="valuationApiParams"
        :disabled="!plantCode?.trim()"
        class="material-model-trend-query-bar__control material-model-trend-query-bar__control--valuation"
        allow-clear
        show-search
        :placeholder="t('entity.materialmovingprice.valuation')"
      />
      <TaktSelect
        v-model:value="materialCode"
        :api-url="materialOptionsUrl"
        :api-params="materialApiParams"
        :disabled="!plantCode?.trim() || !valuation?.trim()"
        class="material-model-trend-query-bar__control material-model-trend-query-bar__control--material"
        allow-clear
        show-search
        :placeholder="t(`${localePrefix}.materialCode`)"
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
 * 物料机种推移查询栏：工厂 → 物料类型 → 评估类别 → 物料
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getMaterialModelTrendMaterialOptionsUrl,
  getMaterialModelTrendPlantOptionsUrl,
  getMaterialModelTrendValuationOptionsUrl,
} from '@/api/logistics/materials/material-model-trend'
import type { TaktSelectOption } from '@/types/common'
import { isCostingPeriodMonthDisabled } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import {
  loadBomMaterialTypeOptionsWithDefault,
  pickDefaultBomMaterialType,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-type-options'

/** 工厂代码（第 1 级，必选） */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 产品物料类型（必选） */
const materialType = defineModel<string | undefined>('materialType')
/** 评估类别（必选） */
const valuation = defineModel<string | undefined>('valuation')
/** 物料编码（可空） */
const materialCode = defineModel<string | undefined>('materialCode')
const props = defineProps<{
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 静态 locales 前缀（复用移动价格推移文案） */
const localePrefix = 'logistics.materials.material-moving-trend.page'
/** 推移本表级联选项 URL（TaktMaterialModelTrends） */
const plantOptionsUrl = getMaterialModelTrendPlantOptionsUrl()
const valuationOptionsUrl = getMaterialModelTrendValuationOptionsUrl()
const materialOptionsUrl = getMaterialModelTrendMaterialOptionsUrl()
/** 物料类型选项 */
const materialTypeOptions = ref<TaktSelectOption[]>([])
const materialTypeOptionsLoading = ref(false)
const materialTypeOptionsPlant = ref('')
let materialTypeLoadToken = 0

/** 工厂 → 评估类别 */
const valuationApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 工厂 + 评估类别 → 物料（可选） */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const val = valuation.value?.trim()
  if (!plant || !val) {
    return undefined
  }
  return { plantCode: plant, valuation: val }
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

/** 清空物料类型选项缓存 */
function clearMaterialTypeOptions() {
  materialTypeLoadToken += 1
  materialType.value = undefined
  materialTypeOptions.value = []
  materialTypeOptionsPlant.value = ''
  materialTypeOptionsLoading.value = false
}

/** 工厂变更：清空下游并重拉物料类型 */
async function handlePlantChange() {
  valuation.value = undefined
  materialCode.value = undefined
  const plant = plantCode.value?.trim()
  if (!plant) {
    clearMaterialTypeOptions()
    return
  }
  materialType.value = await loadMaterialTypes(plant)
}

/** 物料类型不可空：清空时回填默认 */
function handleMaterialTypeChange() {
  if (!materialType.value?.trim()) {
    const def = pickDefaultBomMaterialType(materialTypeOptions.value)
    if (def) {
      materialType.value = def
    }
  }
}

/** 评估类别变更：清空物料 */
watch(
  () => valuation.value,
  () => {
    materialCode.value = undefined
  },
)

watch(
  () => plantCode.value?.trim() || '',
  async (plant) => {
    if (!plant || plant === materialTypeOptionsPlant.value) {
      return
    }
    const def = await loadMaterialTypes(plant)
    if (!materialType.value?.trim() && def) {
      materialType.value = def
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

.material-model-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.material-model-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.material-model-trend-query-bar__control--type {
  width: 7rem;
  min-width: 6rem;
}

.material-model-trend-query-bar__control--valuation {
  width: 10rem;
  min-width: 8rem;
}

.material-model-trend-query-bar__control--material {
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
