<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection-detail/components -->
<!-- 文件名称：pcba-inspection-form.vue -->
<!-- 功能描述：PCBA检查日报实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-inspection-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-inspection-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.tenantcode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companycode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companydefaultculture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbainspection.plantcode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.plantcode') })"
                  :disabled="!!formData?.pcbaInspectionId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbainspection.prodcategory')"
                name="prodCategory"
              >
                <TaktSelect
                  v-model:value="formState.prodCategory"
                  dict-type="logistics_prod_category"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.prodcategory') })"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbainspection.proddate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.proddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbainspection.prodordercode')"
                name="prodOrderCode"
              >
                <TaktSelect
                  v-model:value="formState.prodOrderCode"
                  :options="productionOrderOptions"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.prodordercode') })"
                  :disabled="loading || !formState.plantCode || !!formData?.pcbaInspectionId"
                  @change="handleProductionOrderSelectChange"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbainspection.prodorderqty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspection.prodorderqty') })"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbainspection.modelcode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspection.modelcode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbainspection.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspection.batchno') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.pcbainspection.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspection.materialcode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t('common.page.entity.extfield') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 pcbaInspectionDetails -->
    <TaktEditableTable
      ref="pcbaInspectionDetailTableRef"
      v-model="childPcbaInspectionDetailRows"
      :columns="pcbaInspectionDetailFormColumns"
      :title="t('entity.pcbainspectiondetail._self')"
      :add-button-entity="t('entity.pcbainspectiondetail._self')"
      id-field="pcbaInspectionDetailId"
      :default-row="createDefaultPcbaInspectionDetailRow"
      :disabled="loading"
      section-border
    >
      <template #cell-pcbaBoardType="{ record }">
        <TaktSelect
          v-model:value="record.pcbaBoardType"
          dict-type="logistics_pcba_panel_category"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspectiondetail.pcbaboardtype') })"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-visualInspectionLine="{ record }">
        <TaktSelect
          v-model:value="record.visualInspectionLine"
          dict-type="logistics_visual_inspection_line_category"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspectiondetail.visualinspectionline') })"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-aoiLine="{ record }">
        <TaktSelect
          v-model:value="record.aoiLine"
          dict-type="logistics_aoi_inspection_line_category"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspectiondetail.aoiline') })"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-shiftNo="{ record }">
        <TaktSelect
          v-model:value="record.shiftNo"
          dict-type="logistics_shift_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspectiondetail.shiftno') })"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * PCBA检查日报实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/pcba-inspection-detail/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PcbaInspectionCreate } from '@/types/logistics/manufacturing/defect/pcba-inspection-detail'
import type { TaktSelectOption } from '@/types/common'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { getProductionOrderOptions } from '@/api/logistics/manufacturing/output/production-order'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 生产工单下拉选项（GetProductionOrderOptionsAsync，按工厂加载） */
const productionOrderOptions = ref<TaktSelectOption[]>([])

/**
 * 加载生产工单选项（按工厂过滤）
 * @param plantCode 工厂代码
 */
async function loadProductionOrderOptions(plantCode?: string) {
  productionOrderOptions.value = plantCode
    ? await getProductionOrderOptions(plantCode)
    : []
}

/**
 * 从工单选项标签解析物料编码（DictLabel = ProdOrderCode + MaterialCode）
 * @param dictLabel 选项标签
 * @param prodOrderCode 生产工单号
 * @returns 物料编码
 */
function parseMaterialCodeFromProductionOrderLabel(dictLabel: string, prodOrderCode: string): string {
  if (!dictLabel || !prodOrderCode) {
    return ''
  }
  if (dictLabel === prodOrderCode) {
    return ''
  }
  if (dictLabel.startsWith(prodOrderCode)) {
    return dictLabel.slice(prodOrderCode.length)
  }
  return ''
}

/**
 * 生产工单下拉变更：回填物料、订单数量
 * @param _value 选中工单号
 * @param option 选中项
 */
function handleProductionOrderSelectChange(
  _value: string | number | (string | number)[] | undefined,
  option: Record<string, unknown> | Record<string, unknown>[] | null
) {
  const selected = Array.isArray(option) ? option[0] : option
  if (!selected) {
    formState.materialCode = undefined
    formState.prodOrderQty = undefined
    formState.modelCode = undefined
    return
  }
  const prodOrderCode = String(formState.prodOrderCode ?? selected.dictValue ?? '')
  const materialCode = parseMaterialCodeFromProductionOrderLabel(String(selected.dictLabel ?? ''), prodOrderCode)
  if (materialCode) {
    formState.materialCode = materialCode
  }
  if (selected.extValue !== undefined && selected.extValue !== null && selected.extValue !== '') {
    formState.prodOrderQty = Number(selected.extValue)
  }
}

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** 表单挂载时预加载字典与生产工单选项 */
onMounted(async () => {
  void dictDataStore.loadAllDictDataAsync()
  if (formState.plantCode) {
    await loadProductionOrderOptions(String(formState.plantCode))
  }
})

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodCategory","prodDate","prodOrderCode","prodOrderQty","modelCode","batchNo","materialCode","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childPcbaInspectionDetailRows = ref<Record<string, unknown>[]>([])
const pcbaInspectionDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 pcbaInspectionDetail 可编辑列 */
const pcbaInspectionDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'prodOrderCode',
    title: t('entity.pcbainspectiondetail.prodordercode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: t('entity.pcbainspectiondetail.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'pcbaBoardType',
    title: t('entity.pcbainspectiondetail.pcbaboardtype'),
    width: 140,
  },
  {
    key: 'visualInspectionLine',
    title: t('entity.pcbainspectiondetail.visualinspectionline'),
    width: 140,
  },
  {
    key: 'aoiLine',
    title: t('entity.pcbainspectiondetail.aoiline'),
    width: 140,
  },
  {
    key: 'bSideAssemblyDate',
    title: t('entity.pcbainspectiondetail.bsideassemblydate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'tSideAssemblyDate',
    title: t('entity.pcbainspectiondetail.tsideassemblydate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'shiftNo',
    title: t('entity.pcbainspectiondetail.shiftno'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PcbaInspectionCreate & { pcbaInspectionId?: string }> | null | undefined) {
  childPcbaInspectionDetailRows.value = ((val as any)?.pcbaInspectionDetails ?? []) as Record<string, unknown>[]
}

function createDefaultPcbaInspectionDetailRow(): Record<string, unknown> {
  return {
    prodOrderCode: '',
    lineNumber: (childPcbaInspectionDetailRows.value.length + 1) * 10,
    pcbaBoardType: '',
    visualInspectionLine: '',
    aoiLine: '',
    bSideAssemblyDate: '',
    tSideAssemblyDate: '',
    shiftNo: 1,
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaInspectionCreate & { pcbaInspectionId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}


/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.pcbaInspectionId ?? ''
  return {
    ...formState,
    pcbaInspectionDetails: pcbaInspectionDetailTableRef.value?.getRows?.() ?? childPcbaInspectionDetailRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      pcbaInspectionId: masterId,
    })),
  }
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaInspectionId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaInspectionId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).pcbaInspectionDetails
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      if (formState.plantCode) {
        void loadProductionOrderOptions(String(formState.plantCode))
      }
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      productionOrderOptions.value = []
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.pcbaInspectionId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 工厂变更时，重载工单选项并清理无效工单 */
watch(
  () => formState.plantCode,
  async (plantCode, prevPlantCode) => {
    if (props.formData?.pcbaInspectionId) {
      if (plantCode) {
        await loadProductionOrderOptions(String(plantCode))
      }
      return
    }
    if (!plantCode) {
      formState.prodOrderCode = undefined
      formState.materialCode = undefined
      formState.prodOrderQty = undefined
      formState.modelCode = undefined
      productionOrderOptions.value = []
      return
    }
    if (prevPlantCode && prevPlantCode !== plantCode) {
      formState.prodOrderCode = undefined
      formState.materialCode = undefined
      formState.prodOrderQty = undefined
      formState.modelCode = undefined
    }
    await loadProductionOrderOptions(String(plantCode))
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.plantcode') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  prodCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.prodcategory') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  prodDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.proddate') }),
      trigger: 'change'
    }
  ],
  prodOrderCode: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.prodordercode') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  prodOrderQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.prodorderqty') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbainspection.prodorderqty') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  modelCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbainspection.modelcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbainspection.materialcode') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await pcbaInspectionDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('prodOrderQty' in payload) {
    const rawprodOrderQty = payload.prodOrderQty
    payload.prodOrderQty = typeof rawprodOrderQty === 'number' ? rawprodOrderQty : Number(rawprodOrderQty)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaInspectionId)
  childPcbaInspectionDetailRows.value = []
  pcbaInspectionDetailTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
