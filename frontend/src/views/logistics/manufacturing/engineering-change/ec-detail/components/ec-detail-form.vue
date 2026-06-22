<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-detail/components -->
<!-- 文件名称：ec-detail-form.vue -->
<!-- 功能描述：设变维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ec-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-detail-form-tabs"
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
                :label="t('entity.ecdetail.ecid')"
                name="ecId"
              >
                <a-input
                  v-model:value="formState.ecId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecno')"
                name="ecNo"
              >
                <a-input
                  v-model:value="formState.ecNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecno') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecmodel')"
                name="ecModel"
              >
                <a-input
                  v-model:value="formState.ecModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecmodel') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomitem')"
                name="ecBomItem"
              >
                <a-input
                  v-model:value="formState.ecBomItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomitem') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomsubitem')"
                name="ecBomSubItem"
              >
                <a-input
                  v-model:value="formState.ecBomSubItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomsubitem') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomno')"
                name="ecBomNo"
              >
                <a-input
                  v-model:value="formState.ecBomNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomno') })"
                  show-count
                  :maxlength="50"
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
                :label="t('entity.ecdetail.ecchange')"
                name="ecChange"
              >
                <a-input
                  v-model:value="formState.ecChange"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecchange') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.eclocal')"
                name="ecLocal"
              >
                <a-input
                  v-model:value="formState.ecLocal"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.eclocal') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ecdetail.ecnote')"
                name="ecNote"
              >
                <a-textarea
                  v-model:value="formState.ecNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnote') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecprocess')"
                name="ecProcess"
              >
                <a-input
                  v-model:value="formState.ecProcess"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecprocess') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomdate')"
                name="ecBomDate"
              >
                <a-date-picker
                  v-model:value="formState.ecBomDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecbomdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecentrydate')"
                name="ecEntryDate"
              >
                <a-date-picker
                  v-model:value="formState.ecEntryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecentrydate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecolditem')"
                name="ecOldItem"
              >
                <a-input
                  v-model:value="formState.ecOldItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecolditem') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecoldtext')"
                name="ecOldText"
              >
                <a-input
                  v-model:value="formState.ecOldText"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldtext') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecoldqty')"
                name="ecOldQty"
              >
                <a-input-number
                  v-model:value="formState.ecOldQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldqty') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecoldset')"
                name="ecOldSet"
              >
                <a-input
                  v-model:value="formState.ecOldSet"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldset') })"
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
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewitem')"
                name="ecNewItem"
              >
                <a-input
                  v-model:value="formState.ecNewItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewitem') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewtext')"
                name="ecNewText"
              >
                <a-input
                  v-model:value="formState.ecNewText"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewtext') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewqty')"
                name="ecNewQty"
              >
                <a-input-number
                  v-model:value="formState.ecNewQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewqty') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewset')"
                name="ecNewSet"
              >
                <a-input
                  v-model:value="formState.ecNewSet"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewset') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.isprocurement')"
                name="isProcurement"
              >
                <a-input-number
                  v-model:value="formState.isProcurement"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.isprocurement') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ischeck')"
                name="isCheck"
              >
                <a-input-number
                  v-model:value="formState.isCheck"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ischeck') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecwarehouse')"
                name="ecWarehouse"
              >
                <a-input
                  v-model:value="formState.ecWarehouse"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecwarehouse') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.isendofline')"
                name="isEndOfLine"
              >
                <a-input-number
                  v-model:value="formState.isEndOfLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.isendofline') })"
                  style="width: 100%"
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
    <!-- 下：子表 deptRecords -->
    <TaktEditableTable
      ref="ecDeptTableRef"
      v-model="childEcDeptRows"
      :columns="ecDeptFormColumns"
      :title="t('entity.ecdept._self')"
      :add-button-entity="t('entity.ecdept._self')"
      id-field="ecDeptId"
      :default-row="createDefaultEcDeptRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 设变维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-detail/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcDetailCreate } from '@/types/logistics/manufacturing/engineering-change/ec-detail'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","ecId","ecNo","lineNumber","ecModel","ecBomItem","ecBomSubItem","ecBomNo","ecChange","ecLocal","ecNote","ecProcess","ecBomDate","ecEntryDate","ecOldItem","ecOldText","ecOldQty","ecOldSet","ecNewItem","ecNewText","ecNewQty","ecNewSet","isProcurement","isCheck","ecWarehouse","isEndOfLine","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childEcDeptRows = ref<Record<string, unknown>[]>([])
const ecDeptTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 ecDept 可编辑列 */
const ecDeptFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'ecnDetailId',
    title: t('entity.ecdept.ecndetailid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'ecNo',
    title: t('entity.ecdept.ecno'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: t('entity.ecdept.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'deptCode',
    title: t('entity.ecdept.deptcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'isImplemented',
    title: t('entity.ecdept.isimplemented'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'content',
    title: t('entity.ecdept.content'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.optional', { field: t('entity.ecdept.content') }),
    width: 140,
  },
  {
    key: 'scheduledProductionDate',
    title: t('entity.ecdept.scheduledproductiondate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'scheduledBatch',
    title: t('entity.ecdept.scheduledbatch'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.ecdept.scheduledbatch') }),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<EcDetailCreate & { ecDetailId?: string }> | null | undefined) {
  childEcDeptRows.value = ((val as any)?.deptRecords ?? []) as Record<string, unknown>[]
}

function createDefaultEcDeptRow(): Record<string, unknown> {
  return {
    ecnDetailId: '',
    ecNo: '',
    lineNumber: (childEcDeptRows.value.length + 1) * 10,
    deptCode: '',
    isImplemented: 0,
    content: '',
    scheduledProductionDate: '',
    scheduledBatch: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.ecDetailId ?? ''
  return {
    ...formState,
    deptRecords: ecDeptTableRef.value?.getRows?.() ?? childEcDeptRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ecDetailId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EcDetailCreate & { ecDetailId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 ecDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecDetailId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).deptRecords
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
    const isCreate = !props.formData?.ecDetailId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ecId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecid') }),
      trigger: 'blur'
    }
  ],
  ecNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecno') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecModel: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecmodel') }),
      trigger: 'blur'
    }
  ],
  ecBomDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecbomdate') }),
      trigger: 'change'
    }
  ],
  ecEntryDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecentrydate') }),
      trigger: 'change'
    }
  ],
  isProcurement: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isprocurement') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isprocurement') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCheck: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ischeck') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ischeck') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isEndOfLine: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isendofline') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isendofline') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await ecDeptTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('ecOldQty' in payload) {
    const rawecOldQty = payload.ecOldQty
    payload.ecOldQty = typeof rawecOldQty === 'number' ? rawecOldQty : Number(rawecOldQty)
  }
  if ('ecNewQty' in payload) {
    const rawecNewQty = payload.ecNewQty
    payload.ecNewQty = typeof rawecNewQty === 'number' ? rawecNewQty : Number(rawecNewQty)
  }
  if ('isProcurement' in payload) {
    const rawisProcurement = payload.isProcurement
    payload.isProcurement = typeof rawisProcurement === 'number' ? rawisProcurement : Number(rawisProcurement)
  }
  if ('isCheck' in payload) {
    const rawisCheck = payload.isCheck
    payload.isCheck = typeof rawisCheck === 'number' ? rawisCheck : Number(rawisCheck)
  }
  if ('isEndOfLine' in payload) {
    const rawisEndOfLine = payload.isEndOfLine
    payload.isEndOfLine = typeof rawisEndOfLine === 'number' ? rawisEndOfLine : Number(rawisEndOfLine)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecDetailId)
  childEcDeptRows.value = []
  ecDeptTableRef.value?.resetRows?.()
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
