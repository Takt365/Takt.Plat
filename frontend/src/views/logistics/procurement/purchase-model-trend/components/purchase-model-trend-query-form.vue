<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-model-trend/components -->
<!-- 文件名称：purchase-model-trend-query-form.vue -->
<!-- 功能描述：采购价格推移查询栏（四级：工厂→条件类型→供应商→物料；物料可空） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar purchase-model-trend-query-bar">
    <div class="purchase-model-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-2">
      <TaktSelect
        v-model:value="plantCode"
        :api-url="plantOptionsUrl"
        class="purchase-model-trend-query-bar__control purchase-model-trend-query-bar__control--plant"
        allow-clear
        show-search
        :placeholder="t('common.page.entity.plantcode')"
      />
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        :disabled-date="isCostingPeriodMonthDisabled"
        class="purchase-model-trend-query-bar__control purchase-model-trend-query-bar__control--period"
        :placeholder="[
          t(`${localePrefix}.periodRange`),
          t(`${localePrefix}.periodRange`)]"
      />
      <TaktSelect
        v-model:value="materialType"
        :options="materialTypeOptions"
        class="purchase-model-trend-query-bar__control purchase-model-trend-query-bar__control--type"
        :allow-clear="false"
        show-search
        :disabled="!plantCode?.trim() || materialTypeOptionsLoading"
        :placeholder="t('entity.bommaterialcost.materialtype')"
        @change="handleMaterialTypeChange"
      />
      <TaktSelect
        :key="priceTypeSelectKey"
        v-model:value="priceType"
        :api-url="priceTypeOptionsUrl"
        :api-params="priceTypeApiParams"
        :disabled="!plantCode?.trim()"
        class="purchase-model-trend-query-bar__control purchase-model-trend-query-bar__control--price-type"
        allow-clear
        show-search
        :placeholder="t('entity.purchaseprice.pricetype')"
      />
      <TaktSelect
        :key="supplierSelectKey"
        v-model:value="supplierCode"
        :api-url="supplierOptionsUrl"
        :api-params="supplierApiParams"
        :disabled="!plantCode?.trim() || !priceType?.trim()"
        class="purchase-model-trend-query-bar__control purchase-model-trend-query-bar__control--supplier"
        allow-clear
        show-search
        :placeholder="t('entity.purchaseprice.suppliercode')"
      />
      <TaktSelect
        :key="materialSelectKey"
        v-model:value="materialCode"
        :api-url="materialOptionsUrl"
        :api-params="materialApiParams"
        :disabled="!plantCode?.trim() || !priceType?.trim() || !supplierCode?.trim()"
        class="purchase-model-trend-query-bar__control purchase-model-trend-query-bar__control--material"
        allow-clear
        show-search
        :placeholder="t('entity.purchaseprice.materialcode')"
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
 * 采购价格推移查询栏：工厂 → 物料类型(必选) → 条件类型 → 供应商 → 物料（物料可空）
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getPurchaseModelTrendMaterialOptionsUrl,
  getPurchaseModelTrendPlantOptionsUrl,
  getPurchaseModelTrendPriceTypeOptionsUrl,
  getPurchaseModelTrendSupplierOptionsUrl,
} from '@/api/logistics/procurement/purchase-model-trend'
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
/** 产品物料类型（必选，默认 FERT；机种推移用） */
const materialType = defineModel<string | undefined>('materialType')
/** 条件类型（第 2 级，必选） */
const priceType = defineModel<string | undefined>('priceType')
/** 供应商编码（第 3 级，必选） */
const supplierCode = defineModel<string | undefined>('supplierCode')
/** 物料编码（第 4 级，可空） */
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
/** 静态 locales 前缀 */
const localePrefix = 'logistics.procurement.purchase-model-trend.page'
/** 条件类型下拉刷新键（工厂变更强制重挂载，避免陈旧选项） */
const priceTypeSelectKey = ref(0)
/** 供应商下拉刷新键 */
const supplierSelectKey = ref(0)
/** 物料下拉刷新键 */
const materialSelectKey = ref(0)
/** 推移本表级联选项 URL（TaktPurchaseModelTrends） */
const plantOptionsUrl = getPurchaseModelTrendPlantOptionsUrl()
const priceTypeOptionsUrl = getPurchaseModelTrendPriceTypeOptionsUrl()
const supplierOptionsUrl = getPurchaseModelTrendSupplierOptionsUrl()
const materialOptionsUrl = getPurchaseModelTrendMaterialOptionsUrl()
/** 物料类型选项 */
const materialTypeOptions = ref<TaktSelectOption[]>([])
const materialTypeOptionsLoading = ref(false)
const materialTypeOptionsPlant = ref('')
let materialTypeLoadToken = 0

/** 第 2 级：工厂 → 条件类型 */
const priceTypeApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 第 3 级：工厂 + 条件类型 → 供应商（仅该厂价目供应商） */
const supplierApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const type = priceType.value?.trim()
  if (!plant || !type) {
    return undefined
  }
  return { plantCode: plant, priceType: type }
})

/** 第 4 级：工厂 + 条件类型 + 供应商 → 物料（可选） */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const type = priceType.value?.trim()
  const supplier = supplierCode.value?.trim()
  if (!plant || !type || !supplier) {
    return undefined
  }
  return { plantCode: plant, priceType: type, supplierCode: supplier }
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

/** 物料类型不可空：清空时回填默认 */
function handleMaterialTypeChange() {
  if (!materialType.value?.trim()) {
    const def = pickDefaultBomMaterialType(materialTypeOptions.value)
    if (def) {
      materialType.value = def
    }
  }
}

/** 工厂变更：清空第 2～4 级并强制重挂载；重拉物料类型 */
watch(
  () => plantCode.value,
  async () => {
    priceType.value = undefined
    supplierCode.value = undefined
    materialCode.value = undefined
    priceTypeSelectKey.value += 1
    supplierSelectKey.value += 1
    materialSelectKey.value += 1
    const plant = plantCode.value?.trim()
    if (!plant) {
      materialTypeLoadToken += 1
      materialType.value = undefined
      materialTypeOptions.value = []
      materialTypeOptionsPlant.value = ''
      materialTypeOptionsLoading.value = false
      return
    }
    materialType.value = await loadMaterialTypes(plant)
  },
)

/** 条件类型变更：清空第 3～4 级并强制重挂载 */
watch(
  () => priceType.value,
  () => {
    supplierCode.value = undefined
    materialCode.value = undefined
    supplierSelectKey.value += 1
    materialSelectKey.value += 1
  },
)

/** 供应商变更：清空第 4 级并强制重挂载 */
watch(
  () => supplierCode.value,
  () => {
    materialCode.value = undefined
    materialSelectKey.value += 1
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

.purchase-model-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.purchase-model-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.purchase-model-trend-query-bar__control--type {
  width: 7rem;
  min-width: 6rem;
}

.purchase-model-trend-query-bar__control--price-type {
  width: 10rem;
  min-width: 8rem;
}

.purchase-model-trend-query-bar__control--supplier {
  width: 14rem;
  min-width: 10rem;
}

.purchase-model-trend-query-bar__control--material {
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
