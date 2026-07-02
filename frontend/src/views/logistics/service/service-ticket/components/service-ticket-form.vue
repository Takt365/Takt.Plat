<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-ticket/components -->
<!-- 文件名称：service-ticket-form.vue -->
<!-- 功能描述：服务工单实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="service-ticket-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="t('entity.serviceticket.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.serviceTicketId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.code')"
                name="serviceTicketCode"
              >
                <a-input
                  v-model:value="formState.serviceTicketCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serviceTicketId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.clientid')"
                name="clientId"
              >
                <a-input
                  v-model:value="formState.clientId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.clientcode')"
                name="clientCode"
              >
                <a-input
                  v-model:value="formState.clientCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.serviceTicketId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.clientname')"
                name="clientName"
              >
                <a-input
                  v-model:value="formState.clientName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientname') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.servicerequestid')"
                name="serviceRequestId"
              >
                <a-input
                  v-model:value="formState.serviceRequestId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicerequestid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.servicerequestcode')"
                name="serviceRequestCode"
              >
                <a-input
                  v-model:value="formState.serviceRequestCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicerequestcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serviceTicketId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.serviceorderid')"
                name="serviceOrderId"
              >
                <a-input
                  v-model:value="formState.serviceOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.serviceorderid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.serviceordercode')"
                name="serviceOrderCode"
              >
                <a-input
                  v-model:value="formState.serviceOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.serviceordercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serviceTicketId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.servicecontractid')"
                name="serviceContractId"
              >
                <a-input
                  v-model:value="formState.serviceContractId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicecontractid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.servicecontractcode')"
                name="serviceContractCode"
              >
                <a-input
                  v-model:value="formState.serviceContractCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicecontractcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serviceTicketId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.tickettype')"
                name="ticketType"
              >
                <a-input-number
                  v-model:value="formState.ticketType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.tickettype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.priority')"
                name="priority"
              >
                <TaktSelect
                  v-model:value="formState.priority"
                  dict-type="sys_priority_level_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.priority') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.ticketstatus')"
                name="ticketStatus"
              >
                <TaktSelect
                  v-model:value="formState.ticketStatus"
                  dict-type="sys_ticket_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.ticketstatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.ticketsubject')"
                name="ticketSubject"
              >
                <a-input
                  v-model:value="formState.ticketSubject"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.ticketsubject') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serviceticket.faultdescription')"
                name="faultDescription"
              >
                <a-textarea
                  v-model:value="formState.faultDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceticket.faultdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serviceticket.solutiondescription')"
                name="solutionDescription"
              >
                <a-textarea
                  v-model:value="formState.solutionDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceticket.solutiondescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.servicelocation')"
                name="serviceLocation"
              >
                <a-input
                  v-model:value="formState.serviceLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicelocation') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.assignedemployeeid')"
                name="assignedEmployeeId"
              >
                <a-input
                  v-model:value="formState.assignedEmployeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.assignedemployeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.assignedemployeename')"
                name="assignedEmployeeName"
              >
                <a-input
                  v-model:value="formState.assignedEmployeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.assignedemployeename') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.scheduledstarttime')"
                name="scheduledStartTime"
              >
                <a-date-picker
                  v-model:value="formState.scheduledStartTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledstarttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.scheduledendtime')"
                name="scheduledEndTime"
              >
                <a-date-picker
                  v-model:value="formState.scheduledEndTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledendtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.actualstarttime')"
                name="actualStartTime"
              >
                <a-date-picker
                  v-model:value="formState.actualStartTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualstarttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.actualendtime')"
                name="actualEndTime"
              >
                <a-date-picker
                  v-model:value="formState.actualEndTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualendtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.acceptanceresult')"
                name="acceptanceResult"
              >
                <a-input-number
                  v-model:value="formState.acceptanceResult"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.acceptanceresult') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.acceptedby')"
                name="acceptedBy"
              >
                <a-input
                  v-model:value="formState.acceptedBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.acceptedby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceticket.acceptedat')"
                name="acceptedAt"
              >
                <a-date-picker
                  v-model:value="formState.acceptedAt"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.acceptedat') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 服务工单实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/service/service-ticket/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ServiceTicketCreate } from '@/types/logistics/customer-service/service-ticket'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","serviceTicketCode","clientId","clientCode","clientName","serviceRequestId","serviceRequestCode","serviceOrderId","serviceOrderCode","serviceContractId","serviceContractCode","ticketType","priority","ticketStatus","ticketSubject","faultDescription","solutionDescription","serviceLocation","assignedEmployeeId","assignedEmployeeName","scheduledStartTime","scheduledEndTime","actualStartTime","actualEndTime","acceptanceResult","acceptedBy","acceptedAt","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ServiceTicketCreate & { serviceTicketId?: string }> | null
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
  priority: 3,
  ticketStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 serviceTicketId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.serviceTicketId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    const isCreate = !props.formData?.serviceTicketId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceticket.plantcode') }),
      trigger: 'blur'
    }
  ],
  serviceTicketCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceticket.code') }),
      trigger: 'blur'
    }
  ],
  clientId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientid') }),
      trigger: 'blur'
    }
  ],
  clientCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientcode') }),
      trigger: 'blur'
    }
  ],
  clientName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientname') }),
      trigger: 'blur'
    }
  ],
  ticketType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serviceticket.tickettype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serviceticket.tickettype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serviceticket.priority') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serviceticket.priority') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ticketStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serviceticket.ticketstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serviceticket.ticketstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ticketSubject: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceticket.ticketsubject') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('ticketType' in payload) {
    const rawticketType = payload.ticketType
    payload.ticketType = typeof rawticketType === 'number' ? rawticketType : Number(rawticketType)
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    payload.priority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
  }
  if ('ticketStatus' in payload) {
    const rawticketStatus = payload.ticketStatus
    payload.ticketStatus = typeof rawticketStatus === 'number' ? rawticketStatus : Number(rawticketStatus)
  }
  if ('acceptanceResult' in payload) {
    const rawacceptanceResult = payload.acceptanceResult
    payload.acceptanceResult = typeof rawacceptanceResult === 'number' ? rawacceptanceResult : Number(rawacceptanceResult)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.serviceTicketId)

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
