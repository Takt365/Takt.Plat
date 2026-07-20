<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item/components -->
<!-- 文件名称：bill-of-material-item-form.vue -->
<!-- 功能描述：Takt物料清单明细实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form bill-of-material-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="bill-of-material-item-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
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
                :label="t('entity.billofmaterialitem.billofmaterialid')"
                name="billOfMaterialId"
              >
                <a-input
                  v-model:value="formState.billOfMaterialId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.billofmaterialid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.bomcode')"
                name="bomCode"
              >
                <a-input
                  v-model:value="formState.bomCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.bomcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.billOfMaterialItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.materialid')"
                name="materialId"
              >
                <TaktSelect
                  v-model:value="formState.materialId"
                  :options="filteredMaterialPlantOptions"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.materialid') })"
                  :disabled="loading || !resolvedPlantCode"
                  allow-clear
                  @change="handleMaterialChange"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.billOfMaterialItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.usagequantity')"
                name="usageQuantity"
              >
                <a-input-number
                  v-model:value="formState.usageQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.usagequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.materialunit')"
                name="materialUnit"
              >
                <TaktSelect
                  v-model:value="formState.materialUnit"
                  dict-type="logistics_unit_of_measure_code"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.materialunit') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.scraprate')"
                name="scrapRate"
              >
                <a-input-number
                  v-model:value="formState.scrapRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.scraprate') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.actualusagequantity')"
                name="actualUsageQuantity"
              >
                <a-input-number
                  v-model:value="formState.actualUsageQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.actualusagequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.operationseq')"
                name="operationSeq"
              >
                <a-input-number
                  v-model:value="formState.operationSeq"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.operationseq') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.workcenter')"
                name="workCenter"
              >
                <TaktSelect
                  v-model:value="formState.workCenter"
                  :options="filteredWorkCenterOptions"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.workcenter') })"
                  :disabled="loading || !resolvedPlantCode"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.position')"
                name="position"
              >
                <a-input
                  v-model:value="formState.position"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.position') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.substitutegroup')"
                name="substituteGroup"
              >
                <a-input
                  v-model:value="formState.substituteGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.substitutegroup') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.substitutepriority')"
                name="substitutePriority"
              >
                <a-input-number
                  v-model:value="formState.substitutePriority"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.substitutepriority') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.isoptional')"
                name="isOptional"
              >
                <TaktSelect
                  v-model:value="formState.isOptional"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.isoptional') })"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialitem.isphantom')"
                name="isPhantom"
              >
                <TaktSelect
                  v-model:value="formState.isPhantom"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.isphantom') })"
                  :disabled="loading"
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
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
    <!-- 下：子表 substitutes -->
    <TaktEditableTable
      ref="billOfMaterialSubstituteTableRef"
      v-model="childBillOfMaterialSubstituteRows"
      :columns="billOfMaterialSubstituteFormColumns"
      :title="t('entity.billofmaterialsubstitute._self')"
      :add-button-entity="t('entity.billofmaterialsubstitute._self')"
      id-field="billOfMaterialSubstituteId"
      :default-row="createDefaultBillOfMaterialSubstituteRow"
      :disabled="loading"
      section-border
    >
      <template #cell-substituteMaterialId="{ record }">
        <TaktSelect
          v-model:value="record.substituteMaterialId"
          :options="filteredMaterialPlantOptions"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.substitutematerialid') })"
          :disabled="loading || !resolvedPlantCode"
          allow-clear
          @change="(val: string) => handleSubstituteMaterialChange(record, val)"
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt物料清单明细实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/bill-of-material-item/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { BillOfMaterialItemCreate } from '@/types/logistics/manufacturing/bom/bill-of-material-item'
import type { TaktSelectOption } from '@/types/common'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { getMaterialPlantOptions, getMaterialPlantById } from '@/api/logistics/materials/material-plant'
import { getWorkCenterOptions } from '@/api/logistics/manufacturing/aps/work-center'
import { getBillOfMaterialById } from '@/api/logistics/manufacturing/bom/bill-of-material'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** 工厂物料下拉全量选项 */
const materialPlantOptions = ref<TaktSelectOption[]>([])
/** 工作中心下拉全量选项 */
const workCenterOptions = ref<TaktSelectOption[]>([])
/** 由 BOM 头解析的工厂代码（用于过滤物料/工作中心） */
const resolvedPlantCode = ref('')
/** 按工厂过滤的工厂物料选项 */
const filteredMaterialPlantOptions = computed(() => {
  const plantCode = resolvedPlantCode.value || formState.plantCode
  if (!plantCode) {
    return materialPlantOptions.value
  }
  return materialPlantOptions.value.filter((item) => String(item.extValue ?? '') === String(plantCode))
})
/** 按工厂过滤的工作中心选项 */
const filteredWorkCenterOptions = computed(() => {
  const plantCode = resolvedPlantCode.value || formState.plantCode
  if (!plantCode) {
    return []
  }
  return workCenterOptions.value.filter((item) => String(item.extValue ?? '') === String(plantCode))
})

/** 加载下拉选项 */
async function loadSelectOptions() {
  const [mp, wc] = await Promise.all([getMaterialPlantOptions(), getWorkCenterOptions()])
  materialPlantOptions.value = mp
  workCenterOptions.value = wc
}

/** 根据 BOM 头 Id 解析工厂代码 */
async function resolvePlantCodeFromBom(billOfMaterialId: string | undefined) {
  if (!billOfMaterialId) {
    resolvedPlantCode.value = ''
    return
  }
  try {
    const bom = await getBillOfMaterialById(billOfMaterialId)
    resolvedPlantCode.value = bom?.plantCode ?? ''
  } catch {
    resolvedPlantCode.value = ''
  }
}

/** 子项物料选择变更 */
async function handleMaterialChange(materialId: string | number | undefined) {
  if (materialId == null || materialId === '') {
    formState.materialCode = ''
    return
  }
  const option = materialPlantOptions.value.find((item) => String(item.dictValue) === String(materialId))
  if (option?.extLabel) {
    formState.materialCode = option.extLabel
  }
  try {
    const mp = await getMaterialPlantById(String(materialId))
    if (mp?.materialCode) {
      formState.materialCode = mp.materialCode
    }
    if (mp?.baseUnit && !formState.materialUnit) {
      formState.materialUnit = mp.baseUnit.toLowerCase()
    }
  } catch {
    /* 选项冗余字段已回填 */
  }
}

/** 替代料子表行物料选择变更 */
async function handleSubstituteMaterialChange(record: Record<string, unknown>, materialId: string | number | undefined) {
  if (materialId == null || materialId === '') {
    record.substituteMaterialCode = ''
    return
  }
  const option = materialPlantOptions.value.find((item) => String(item.dictValue) === String(materialId))
  if (option?.extLabel) {
    record.substituteMaterialCode = option.extLabel
  }
  try {
    const mp = await getMaterialPlantById(String(materialId))
    if (mp?.materialCode) {
      record.substituteMaterialCode = mp.materialCode
    }
  } catch {
    /* 选项冗余字段已回填 */
  }
}

onMounted(() => {
  void loadSelectOptions()
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","billOfMaterialId","bomCode","lineNumber","materialId","materialCode","usageQuantity","materialUnit","scrapRate","actualUsageQuantity","operationSeq","workCenter","position","substituteGroup","substitutePriority","isOptional","isPhantom","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childBillOfMaterialSubstituteRows = ref<Record<string, unknown>[]>([])
const billOfMaterialSubstituteTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 billOfMaterialSubstitute 可编辑列 */
const billOfMaterialSubstituteFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'billOfMaterialId',
    title: t('entity.billofmaterialsubstitute.billofmaterialid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'bomCode',
    title: t('entity.billofmaterialsubstitute.bomcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'primaryMaterialCode',
    title: t('entity.billofmaterialsubstitute.primarymaterialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: t('entity.billofmaterialsubstitute.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'substituteMaterialId',
    title: t('entity.billofmaterialsubstitute.substitutematerialid'),
    width: 180,
  },
  {
    key: 'substituteMaterialCode',
    title: t('entity.billofmaterialsubstitute.substitutematerialcode'),
    editor: 'readonly',
    width: 140,
  },
  {
    key: 'substituteGroup',
    title: t('entity.billofmaterialsubstitute.substitutegroup'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.billofmaterialsubstitute.substitutegroup') }),
  },
  {
    key: 'substitutePriority',
    title: t('entity.billofmaterialsubstitute.substitutepriority'),
    editor: 'inputNumber',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<BillOfMaterialItemCreate & { billOfMaterialItemId?: string }> | null | undefined) {
  childBillOfMaterialSubstituteRows.value = ((val as any)?.substitutes ?? []) as Record<string, unknown>[]
}

function createDefaultBillOfMaterialSubstituteRow(): Record<string, unknown> {
  return {
    billOfMaterialId: '',
    bomCode: '',
    primaryMaterialCode: '',
    lineNumber: (childBillOfMaterialSubstituteRows.value.length + 1) * 10,
    substituteMaterialId: '',
    substituteMaterialCode: '',
    substituteGroup: '',
    substitutePriority: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.billOfMaterialItemId ?? ''
  return {
    ...formState,
    substitutes: billOfMaterialSubstituteTableRef.value?.getRows?.() ?? childBillOfMaterialSubstituteRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      billOfMaterialItemId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BillOfMaterialItemCreate & { billOfMaterialItemId?: string }> | null
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
/** 表单字段默认值 */
function applyFormDefaults(target: Record<string, unknown>) {
  if (target.isOptional === undefined || target.isOptional === null || target.isOptional === '') {
    target.isOptional = 0
  }
  if (target.isPhantom === undefined || target.isPhantom === null || target.isPhantom === '') {
    target.isPhantom = 0
  }
  if (!target.materialUnit) {
    target.materialUnit = 'PC'
  }
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 billOfMaterialItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.billOfMaterialItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).substitutes
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** BOM 头变更时解析工厂代码 */
watch(
  () => formState.billOfMaterialId as string | undefined,
  (id) => {
    void resolvePlantCodeFromBom(id)
  },
  { immediate: true },
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.billOfMaterialItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  billOfMaterialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.billofmaterialid') }),
      trigger: 'blur'
    }
  ],
  bomCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.bomcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialid') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialcode') }),
      trigger: 'blur'
    }
  ],
  usageQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.usagequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.usagequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialunit') }),
      trigger: 'blur'
    }
  ],
  scrapRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.scraprate') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.scraprate') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualUsageQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.actualusagequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.actualusagequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  operationSeq: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.operationseq') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.operationseq') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  substitutePriority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.substitutepriority') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.substitutepriority') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isOptional: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.isoptional') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.isoptional') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isPhantom: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.isphantom') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.isphantom') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await billOfMaterialSubstituteTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('usageQuantity' in payload) {
    const rawusageQuantity = payload.usageQuantity
    payload.usageQuantity = typeof rawusageQuantity === 'number' ? rawusageQuantity : Number(rawusageQuantity)
  }
  if ('scrapRate' in payload) {
    const rawscrapRate = payload.scrapRate
    payload.scrapRate = typeof rawscrapRate === 'number' ? rawscrapRate : Number(rawscrapRate)
  }
  if ('actualUsageQuantity' in payload) {
    const rawactualUsageQuantity = payload.actualUsageQuantity
    payload.actualUsageQuantity = typeof rawactualUsageQuantity === 'number' ? rawactualUsageQuantity : Number(rawactualUsageQuantity)
  }
  if ('operationSeq' in payload) {
    const rawoperationSeq = payload.operationSeq
    payload.operationSeq = typeof rawoperationSeq === 'number' ? rawoperationSeq : Number(rawoperationSeq)
  }
  if ('substitutePriority' in payload) {
    const rawsubstitutePriority = payload.substitutePriority
    payload.substitutePriority = typeof rawsubstitutePriority === 'number' ? rawsubstitutePriority : Number(rawsubstitutePriority)
  }
  if ('isOptional' in payload) {
    const rawisOptional = payload.isOptional
    payload.isOptional = typeof rawisOptional === 'number' ? rawisOptional : Number(rawisOptional)
  }
  if ('isPhantom' in payload) {
    const rawisPhantom = payload.isPhantom
    payload.isPhantom = typeof rawisPhantom === 'number' ? rawisPhantom : Number(rawisPhantom)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.billOfMaterialItemId)
  childBillOfMaterialSubstituteRows.value = []
  billOfMaterialSubstituteTableRef.value?.resetRows?.()
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
