<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-request/components -->
<!-- 文件名称：service-request-form.vue -->
<!-- 功能描述：服务请求实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form service-request-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="service-request-form-tabs"
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
                :label="t('entity.servicerequest.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.serviceRequestId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.code')"
                name="serviceRequestCode"
              >
                <a-input
                  v-model:value="formState.serviceRequestCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serviceRequestId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.clientid')"
                name="clientId"
              >
                <a-input
                  v-model:value="formState.clientId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.clientcode')"
                name="clientCode"
              >
                <a-input
                  v-model:value="formState.clientCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.serviceRequestId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.clientname')"
                name="clientName"
              >
                <a-input
                  v-model:value="formState.clientName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientname') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.servicecontractid')"
                name="serviceContractId"
              >
                <a-input
                  v-model:value="formState.serviceContractId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.servicecontractid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.servicecontractcode')"
                name="serviceContractCode"
              >
                <a-input
                  v-model:value="formState.serviceContractCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.servicecontractcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serviceRequestId"
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
                :label="t('entity.servicerequest.requestdate')"
                name="requestDate"
              >
                <a-date-picker
                  v-model:value="formState.requestDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requestdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.expectedservicedate')"
                name="expectedServiceDate"
              >
                <a-date-picker
                  v-model:value="formState.expectedServiceDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.expectedservicedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.requesttype')"
                name="requestType"
              >
                <a-input-number
                  v-model:value="formState.requestType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requesttype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.sourcechannel')"
                name="sourceChannel"
              >
                <a-input-number
                  v-model:value="formState.sourceChannel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.sourcechannel') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.priority')"
                name="priority"
              >
                <TaktSelect
                  v-model:value="formState.priority"
                  dict-type="sys_priority_level_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.priority') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.requeststatus')"
                name="requestStatus"
              >
                <a-input-number
                  v-model:value="formState.requestStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requeststatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.requestsubject')"
                name="requestSubject"
              >
                <a-input
                  v-model:value="formState.requestSubject"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requestsubject') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.requestdescription')"
                name="requestDescription"
              >
                <a-textarea
                  v-model:value="formState.requestDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.servicerequest.requestdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.contactperson')"
                name="contactPerson"
              >
                <a-input
                  v-model:value="formState.contactPerson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.contactperson') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.servicerequest.contactphone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.contactphone') })"
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
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.contactemail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.contactemail') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.serviceaddress')"
                name="serviceAddress"
              >
                <a-textarea
                  v-model:value="formState.serviceAddress"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.servicerequest.serviceaddress') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.assignedemployeeid')"
                name="assignedEmployeeId"
              >
                <a-input
                  v-model:value="formState.assignedEmployeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.assignedemployeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.assignedemployeename')"
                name="assignedEmployeeName"
              >
                <a-input
                  v-model:value="formState.assignedEmployeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.assignedemployeename') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.assignedat')"
                name="assignedAt"
              >
                <a-input
                  v-model:value="formState.assignedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.assignedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.closedat')"
                name="closedAt"
              >
                <a-input
                  v-model:value="formState.closedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.closedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
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
    <!-- 下：子表 tickets -->
    <TaktEditableTable
      ref="serviceTicketTableRef"
      v-model="childServiceTicketRows"
      :columns="serviceTicketFormColumns"
      :title="t('entity.serviceticket._self')"
      :add-button-entity="t('entity.serviceticket._self')"
      id-field="serviceTicketId"
      :default-row="createDefaultServiceTicketRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 服务请求实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/service/service-request/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ServiceRequestCreate } from '@/types/logistics/customer-service/service-request'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","serviceRequestCode","clientId","clientCode","clientName","serviceContractId","serviceContractCode","requestDate","expectedServiceDate","requestType","sourceChannel","priority","requestStatus","requestSubject","requestDescription","contactPerson","contactPhone","contactEmail","serviceAddress","assignedEmployeeId","assignedEmployeeName","assignedAt","closedAt","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childServiceTicketRows = ref<Record<string, unknown>[]>([])
const serviceTicketTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 serviceTicket 可编辑列 */
const serviceTicketFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'plantCode',
    title: t('entity.serviceticket.plantcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'serviceTicketCode',
    title: t('entity.serviceticket.code'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'clientId',
    title: t('entity.serviceticket.clientid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'clientCode',
    title: t('entity.serviceticket.clientcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'clientName',
    title: t('entity.serviceticket.clientname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'serviceOrderId',
    title: t('entity.serviceticket.serviceorderid'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.serviceticket.serviceorderid') }),
  },
  {
    key: 'serviceOrderCode',
    title: t('entity.serviceticket.serviceordercode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.serviceticket.serviceordercode') }),
  },
  {
    key: 'serviceContractId',
    title: t('entity.serviceticket.servicecontractid'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.serviceticket.servicecontractid') }),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ServiceRequestCreate & { serviceRequestId?: string }> | null | undefined) {
  childServiceTicketRows.value = ((val as any)?.tickets ?? []) as Record<string, unknown>[]
}

function createDefaultServiceTicketRow(): Record<string, unknown> {
  return {
    plantCode: '',
    serviceTicketCode: '',
    clientId: '',
    clientCode: '',
    clientName: '',
    serviceOrderId: '',
    serviceOrderCode: '',
    serviceContractId: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.serviceRequestId ?? ''
  return {
    ...formState,
    tickets: serviceTicketTableRef.value?.getRows?.() ?? childServiceTicketRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      serviceRequestId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ServiceRequestCreate & { serviceRequestId?: string }> | null
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
  priority: 3
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 serviceRequestId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.serviceRequestId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).tickets
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
    const isCreate = !props.formData?.serviceRequestId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.plantcode') }),
      trigger: 'blur'
    }
  ],
  serviceRequestCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.code') }),
      trigger: 'blur'
    }
  ],
  clientId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientid') }),
      trigger: 'blur'
    }
  ],
  clientCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientcode') }),
      trigger: 'blur'
    }
  ],
  clientName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientname') }),
      trigger: 'blur'
    }
  ],
  requestDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requestdate') }),
      trigger: 'change'
    }
  ],
  requestType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requesttype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requesttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sourceChannel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.servicerequest.sourcechannel') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.servicerequest.sourcechannel') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.servicerequest.priority') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.servicerequest.priority') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requestStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requeststatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requeststatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requestSubject: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requestsubject') }),
      trigger: 'blur'
    }
  ],
  requestDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requestdescription') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await serviceTicketTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('requestType' in payload) {
    const rawrequestType = payload.requestType
    payload.requestType = typeof rawrequestType === 'number' ? rawrequestType : Number(rawrequestType)
  }
  if ('sourceChannel' in payload) {
    const rawsourceChannel = payload.sourceChannel
    payload.sourceChannel = typeof rawsourceChannel === 'number' ? rawsourceChannel : Number(rawsourceChannel)
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    payload.priority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
  }
  if ('requestStatus' in payload) {
    const rawrequestStatus = payload.requestStatus
    payload.requestStatus = typeof rawrequestStatus === 'number' ? rawrequestStatus : Number(rawrequestStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.serviceRequestId)
  childServiceTicketRows.value = []
  serviceTicketTableRef.value?.resetRows?.()
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
