<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-repair/components -->
<!-- 文件名称：pcba-repair-form.vue -->
<!-- 功能描述：PCBA改修日报实体 不良率维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-repair-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-repair-form-tabs"
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
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="pi.ph('plantCode')"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodCategory')"
                name="prodCategory"
              >
                <TaktSelect
                  v-model:value="formState.prodCategory"
                  dict-type="logistics_prod_category"
                  :placeholder="pi.ph('prodCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodDate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="pi.ph('prodDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodTeam')"
                name="prodTeam"
              >
                <TaktSelect
                  v-model:value="formState.prodTeam"
                  api-url="TaktProductionTeams/options"
                  :placeholder="pi.ph('prodTeam')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shiftNo')"
                name="shiftNo"
              >
                <TaktSelect
                  v-model:value="formState.shiftNo"
                  dict-type="logistics_shift_category"
                  :placeholder="pi.ph('shiftNo')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderType')"
                name="prodOrderType"
              >
                <a-input
                  v-model:value="formState.prodOrderType"
                  :placeholder="pi.ph('prodOrderType')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderCode')"
                name="prodOrderCode"
              >
                <TaktSelect
                  v-model:value="formState.prodOrderCode"
                  api-url="TaktProductionOrders/options"
                  :placeholder="pi.ph('prodOrderCode')"
                  :disabled="!!formData?.pcbaRepairId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderQty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="pi.ph('prodOrderQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('modelCode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="pi.ph('modelCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.pcbaRepairId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchNo')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="pi.ph('batchNo')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="pi.ph('materialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.pcbaRepairId"
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
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="pi.ph('companyDefaultCulture')"
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    <!-- 下：子表 pcbaRepairDetails -->
    <TaktEditableTable
      ref="pcbaRepairDetailTableRef"
      v-model="childPcbaRepairDetailRows"
      :columns="pcbaRepairDetailFormColumns"
      :title="pcbaRepairDetailPi.self()"
      :add-button-entity="pcbaRepairDetailPi.self()"
      id-field="pcbaRepairDetailId"
      :default-row="createDefaultPcbaRepairDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-pcbaBoardType="{ record }">
        <TaktSelect
          v-model:value="record.pcbaBoardType"
          dict-type="logistics_pcba_function_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.ph('pcbaBoardType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-prodTeam="{ record }">
        <TaktSelect
          v-model:value="record.prodTeam"
          api-url="TaktProductionTeams/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.queryPh('prodTeam', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectEngineering="{ record }">
        <TaktSelect
          v-model:value="record.defectEngineering"
          dict-type="logistics_defect_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.ph('defectEngineering')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectResponsibility="{ record }">
        <TaktSelect
          v-model:value="record.defectResponsibility"
          dict-type="logistics_defect_responsibility_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.ph('defectResponsibility')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectNature="{ record }">
        <TaktSelect
          v-model:value="record.defectNature"
          dict-type="logistics_defect_nature_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.ph('defectNature')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-repairOperator="{ record }">
        <TaktSelect
          v-model:value="record.repairOperator"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.queryPh('repairOperator', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * PCBA改修日报实体 不良率维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/pcba-repair/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaRepairI18n } from '../composables/use-pcba-repair-i18n'

/** 实体字段 i18n */
const pi = usePcbaRepairI18n()

import type { PcbaRepairCreate } from '@/types/logistics/manufacturing/defect/pcba-repair'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodCategory","prodDate","prodTeam","shiftNo","prodOrderType","prodOrderCode","prodOrderQty","modelCode","batchNo","materialCode","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { usePcbaRepairDetailI18n } from '../composables/use-pcba-repair-detail-i18n'

const pcbaRepairDetailPi = usePcbaRepairDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPcbaRepairDetailRows = ref<Record<string, unknown>[]>([])
const pcbaRepairDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPcbaRepairDetailRow(row: Record<string, unknown>): boolean {
  const id = row.pcbaRepairDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPcbaRepairDetailLineNumber(): number {
  const rows = pcbaRepairDetailTableRef.value?.getRows?.() ?? childPcbaRepairDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 pcbaRepairDetail 可编辑列 */
const pcbaRepairDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'prodOrderCode',
    title: pcbaRepairDetailPi.label('prodOrderCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: pcbaRepairDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'pcbaBoardType',
    title: pcbaRepairDetailPi.label('pcbaBoardType'),
    width: 140,
  },
  {
    key: 'prodActualQty',
    title: pcbaRepairDetailPi.label('prodActualQty'),
    width: 140,
  },
  {
    key: 'prodTeam',
    title: pcbaRepairDetailPi.label('prodTeam'),
    width: 140,
  },
  {
    key: 'cardNo',
    title: pcbaRepairDetailPi.label('cardNo'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaRepairDetailPi.ph('cardNo'),
  },
  {
    key: 'defectSymptom',
    title: pcbaRepairDetailPi.label('defectSymptom'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaRepairDetailPi.ph('defectSymptom'),
  },
  {
    key: 'defectEngineering',
    title: pcbaRepairDetailPi.label('defectEngineering'),
    width: 140,
  },
  {
    key: 'defectReason',
    title: pcbaRepairDetailPi.label('defectReason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaRepairDetailPi.ph('defectReason'),
  },
  {
    key: 'defectQty',
    title: pcbaRepairDetailPi.label('defectQty'),
    width: 140,
  },
  {
    key: 'defectResponsibility',
    title: pcbaRepairDetailPi.label('defectResponsibility'),
    width: 140,
  },
  {
    key: 'defectNature',
    title: pcbaRepairDetailPi.label('defectNature'),
    width: 140,
  },
  {
    key: 'repairOperator',
    title: pcbaRepairDetailPi.label('repairOperator'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: pcbaRepairDetailPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PcbaRepairCreate & { pcbaRepairId?: string }> | null | undefined) {
  const rows_pcbaRepairDetail = ((val as any)?.pcbaRepairDetails ?? []) as Record<string, unknown>[]
  childPcbaRepairDetailRows.value = rows_pcbaRepairDetail
}

function createDefaultPcbaRepairDetailRow(): Record<string, unknown> {
  return {
    prodOrderCode: '',
    lineNumber: allocateNextPcbaRepairDetailLineNumber(),
    pcbaBoardType: '',
    prodActualQty: 0,
    prodTeam: '',
    cardNo: '',
    defectSymptom: '',
    defectEngineering: '',
    defectReason: '',
    defectQty: 0,
    defectResponsibility: '',
    defectNature: '',
    repairOperator: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.pcbaRepairId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    pcbaRepairDetails: pcbaRepairDetailTableRef.value?.getRows?.() ?? childPcbaRepairDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        pcbaRepairId: masterId,
      }
      if (isUpdate && isPersistedPcbaRepairDetailRow(row)) {
        normalized.pcbaRepairDetailId = row.pcbaRepairDetailId
      } else {
        delete normalized.pcbaRepairDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaRepairCreate & { pcbaRepairId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  prodCategory: "FPP"
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaRepairId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaRepairId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).pcbaRepairDetails
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

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.pcbaRepairId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  prodCategory: [
    {
      required: true,
      message: pi.ph('prodCategory'),
      trigger: 'change'
    }
  ],
  prodDate: [
    {
      required: true,
      message: pi.ph('prodDate'),
      trigger: 'change'
    }
  ],
  prodTeam: [
    {
      required: true,
      message: pi.ph('prodTeam'),
      trigger: 'change'
    }
  ],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shiftNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shiftNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  prodOrderCode: [
    {
      required: true,
      message: pi.ph('prodOrderCode'),
      trigger: 'change'
    }
  ],
  prodOrderQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('prodOrderQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('prodOrderQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  modelCode: [
    {
      required: true,
      message: pi.ph('modelCode'),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await pcbaRepairDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaRepairId)
  childPcbaRepairDetailRows.value = []
  pcbaRepairDetailTableRef.value?.resetRows?.()
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
