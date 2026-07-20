<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-document/components -->
<!-- 文件名称：material-document-form.vue -->
<!-- 功能描述：Takt物料凭证主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-document-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-document-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.materialDocumentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterials/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.materialDocumentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialDocumentCode')"
                name="materialDocumentCode"
              >
                <a-input
                  v-model:value="formState.materialDocumentCode"
                  :placeholder="pi.ph('materialDocumentCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.materialDocumentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('postedBy')"
                name="postedBy"
              >
                <TaktSelect
                  v-model:value="formState.postedBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('postedBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialDocumentStatus')"
                name="materialDocumentStatus"
              >
                <a-input-number
                  v-model:value="formState.materialDocumentStatus"
                  :placeholder="pi.ph('materialDocumentStatus')"
                  style="width: 100%"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="materialDocumentItemTableRef"
      v-model="childMaterialDocumentItemRows"
      :columns="materialDocumentItemFormColumns"
      :title="materialDocumentItemPi.self()"
      :add-button-entity="materialDocumentItemPi.self()"
      id-field="materialDocumentItemId"
      :default-row="createDefaultMaterialDocumentItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-warehouseCode="{ record }">
        <TaktSelect
          v-model:value="record.warehouseCode"
          api-url="TaktWarehouses/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('warehouseCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-movementType="{ record }">
        <TaktSelect
          v-model:value="record.movementType"
          dict-type="logistics_movement_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('movementType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-specialStock="{ record }">
        <TaktSelect
          v-model:value="record.specialStock"
          dict-type="logistics_special_stock_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('specialStock')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-customerCode="{ record }">
        <TaktSelect
          v-model:value="record.customerCode"
          api-url="TaktCustomers/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('customerCode', 'select')"
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
          :placeholder="materialDocumentItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt物料凭证主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-document/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaterialDocumentI18n } from '../composables/use-material-document-i18n'

/** 实体字段 i18n */
const pi = useMaterialDocumentI18n()

import type { MaterialDocumentCreate } from '@/types/logistics/materials/material-document'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","materialCode","materialDocumentCode","postedBy","materialDocumentStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useMaterialDocumentItemI18n } from '../composables/use-material-document-item-i18n'

const materialDocumentItemPi = useMaterialDocumentItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMaterialDocumentItemRows = ref<Record<string, unknown>[]>([])
const materialDocumentItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedMaterialDocumentItemRow(row: Record<string, unknown>): boolean {
  const id = row.materialDocumentItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextMaterialDocumentItemLineNumber(): number {
  const rows = materialDocumentItemTableRef.value?.getRows?.() ?? childMaterialDocumentItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 materialDocumentItem 可编辑列 */
const materialDocumentItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: materialDocumentItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'warehouseCode',
    title: materialDocumentItemPi.label('warehouseCode'),
    width: 140,
  },
  {
    key: 'movementType',
    title: materialDocumentItemPi.label('movementType'),
    width: 140,
  },
  {
    key: 'postingDate',
    title: materialDocumentItemPi.label('postingDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'quantity',
    title: materialDocumentItemPi.label('quantity'),
    width: 140,
  },
  {
    key: 'specialStock',
    title: materialDocumentItemPi.label('specialStock'),
    width: 140,
  },
  {
    key: 'purchaseOrderCode',
    title: materialDocumentItemPi.label('purchaseOrderCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('purchaseOrderCode'),
  },
  {
    key: 'productionOrderCode',
    title: materialDocumentItemPi.label('productionOrderCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('productionOrderCode'),
  },
  {
    key: 'projectCode',
    title: materialDocumentItemPi.label('projectCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('projectCode'),
  },
  {
    key: 'localCurrencyAmount',
    title: materialDocumentItemPi.label('localCurrencyAmount'),
    width: 140,
  },
  {
    key: 'documentDate',
    title: materialDocumentItemPi.label('documentDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'referenceDocumentCode',
    title: materialDocumentItemPi.label('referenceDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('referenceDocumentCode'),
  },
  {
    key: 'customerCode',
    title: materialDocumentItemPi.label('customerCode'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: materialDocumentItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaterialDocumentCreate & { materialDocumentId?: string }> | null | undefined) {
  const rows_materialDocumentItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childMaterialDocumentItemRows.value = rows_materialDocumentItem
}

function createDefaultMaterialDocumentItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextMaterialDocumentItemLineNumber(),
    warehouseCode: '',
    movementType: '',
    postingDate: '',
    quantity: 0,
    specialStock: '',
    purchaseOrderCode: '',
    productionOrderCode: '',
    projectCode: '',
    localCurrencyAmount: 0,
    documentDate: '',
    referenceDocumentCode: '',
    customerCode: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.materialDocumentId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: materialDocumentItemTableRef.value?.getRows?.() ?? childMaterialDocumentItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        materialDocumentId: masterId,
      }
      if (isUpdate && isPersistedMaterialDocumentItemRow(row)) {
        normalized.materialDocumentItemId = row.materialDocumentItemId
      } else {
        delete normalized.materialDocumentItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialDocumentCreate & { materialDocumentId?: string }> | null
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 materialDocumentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialDocumentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
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
    const isCreate = !props.formData?.materialDocumentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  materialDocumentCode: [
    {
      required: true,
      message: pi.ph('materialDocumentCode'),
      trigger: 'blur'
    }
  ],
  materialDocumentStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('materialDocumentStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('materialDocumentStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await materialDocumentItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('materialDocumentStatus' in payload) {
    const rawmaterialDocumentStatus = payload.materialDocumentStatus
    payload.materialDocumentStatus = typeof rawmaterialDocumentStatus === 'number' ? rawmaterialDocumentStatus : Number(rawmaterialDocumentStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.materialDocumentId)
  childMaterialDocumentItemRows.value = []
  materialDocumentItemTableRef.value?.resetRows?.()
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
