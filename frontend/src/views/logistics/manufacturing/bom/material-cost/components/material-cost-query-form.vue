<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost/components -->
<!-- 文件名称：material-cost-query-form.vue -->
<!-- 功能描述：浏览页查询栏（工厂→期间→本表物料类型→本表机种；与分析页同源 options） -->
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
        :placeholder="t('common.page.entity.plantcode')"
        @change="handlePlantChange"
      />
      <a-date-picker
        v-model:value="costingMonth"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="material-cost-query-bar__control material-cost-query-bar__control--period"
        :placeholder="t('logistics.manufacturing.bom.material-cost.page.costingMonth')"
        @change="handlePeriodChange"
      />
      <TaktSelect
        v-model:value="materialType"
        :options="materialTypeOptions"
        class="material-cost-query-bar__control material-cost-query-bar__control--type"
        allow-clear
        show-search
        :disabled="!plantCode || !costingMonth || materialTypeOptionsLoading"
        :placeholder="t('entity.bommaterialcost.materialtype')"
        @change="handleMaterialTypeChange"
      />
      <TaktSelect
        :key="`model-${modelSelectKey}-${materialType || ''}-${costingMonth || ''}`"
        v-model:value="modelCode"
        :api-url="modelOptionsUrl"
        :api-params="modelApiParams"
        class="material-cost-query-bar__control material-cost-query-bar__control--model"
        allow-clear
        show-search
        :disabled="!plantCode || !costingMonth"
        :placeholder="t('entity.bommaterialcost.modelcode')"
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
 * 浏览页查询栏：工厂 → 期间 → 物料类型（本表去重）→ 机种（本表去重，按类型过滤）
 * 与分析/推移同源 TaktBomCostOptions/*-options，避免字典/型号目的地与汇总表脱节
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import { getBomCostOptionModelOptionsUrl } from '@/api/logistics/manufacturing/bom/cost-option'
import type { TaktSelectOption } from '@/types/common'
import { buildBomCostOptionParams } from '../utils/bom-cost-option-params'
import {
  loadBomMaterialTypeOptionsWithDefault,
  pickDefaultBomMaterialType,
} from '../utils/bom-material-type-options'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 机种编码（本表 ModelCode 去重） */
const modelCode = defineModel<string | undefined>('modelCode')
/** 物料类型（本表 MaterialType；加载后默认 FERT） */
const materialType = defineModel<string | undefined>('materialType')
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
/** 本表物料类型全量选项 */
const materialTypeOptions = ref<TaktSelectOption[]>([])
/** 物料类型选项 loading */
const materialTypeOptionsLoading = ref(false)
/** 当前 options 对应的工厂 */
const materialTypeOptionsPlant = ref('')
/** 选项请求序号（防竞态） */
let materialTypeLoadToken = 0
/** 本表机种 options */
const modelOptionsUrl = getBomCostOptionModelOptionsUrl()

/**
 * 机种下拉参数（工厂 + 单月 + 已选类型）
 */
const modelApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    costingMonth: costingMonth.value,
    materialType: materialType.value,
  }),
)

/**
 * 按工厂拉取本表物料类型全量选项
 * @param {string} plant 工厂
 * @returns {Promise<string | undefined>} 默认类型
 */
async function ensureMaterialTypeOptions(plant: string): Promise<string | undefined> {
  const token = ++materialTypeLoadToken
  materialTypeOptionsLoading.value = true
  try {
    const { options, defaultType } = await loadBomMaterialTypeOptionsWithDefault(
      plant,
      undefined,
      costingMonth.value,
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

/** 类型为空时回填默认 FERT */
function applyDefaultMaterialTypeIfEmpty() {
  if (materialType.value?.trim()) {
    return
  }
  const def = pickDefaultBomMaterialType(materialTypeOptions.value)
  if (def) {
    materialType.value = def
  }
}

/** 工厂变更：清空机种/类型，重拉本表类型并默认 FERT */
async function handlePlantChange() {
  modelCode.value = undefined
  materialType.value = undefined
  modelSelectKey.value += 1
  const plant = plantCode.value?.trim()
  if (!plant || !costingMonth.value?.trim()) {
    materialTypeOptions.value = []
    materialTypeOptionsPlant.value = ''
    return
  }
  const defaultType = await ensureMaterialTypeOptions(plant)
  if (defaultType && !materialType.value?.trim()) {
    materialType.value = defaultType
  }
  modelSelectKey.value += 1
}

/** 期间变更：重拉类型并清空机种 */
async function handlePeriodChange() {
  modelCode.value = undefined
  const plant = plantCode.value?.trim()
  if (plant && costingMonth.value?.trim()) {
    const defaultType = await ensureMaterialTypeOptions(plant)
    materialType.value = defaultType
  }
  modelSelectKey.value += 1
}

/** 物料类型变更：清空机种；清空时回填 FERT */
function handleMaterialTypeChange() {
  applyDefaultMaterialTypeIfEmpty()
  modelCode.value = undefined
  modelSelectKey.value += 1
}

watch(
  () => plantCode.value?.trim() ?? '',
  async (plant) => {
    if (!plant) {
      materialTypeOptions.value = []
      materialTypeOptionsPlant.value = ''
      return
    }
    if (plant === materialTypeOptionsPlant.value && materialTypeOptions.value.length > 0) {
      applyDefaultMaterialTypeIfEmpty()
      return
    }
    const defaultType = await ensureMaterialTypeOptions(plant)
    if (defaultType && !materialType.value?.trim()) {
      materialType.value = defaultType
    }
    modelSelectKey.value += 1
  },
  { immediate: true },
)
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

.material-cost-query-bar__control--type {
  width: 9rem;
  min-width: 7rem;
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
