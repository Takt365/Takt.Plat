<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-ticket/components -->
<!-- 文件名称：service-ticket-form.vue -->
<!-- 功能描述：服务工单实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
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
                  size="small"
                  readonly
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
                  size="small"
                  readonly
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
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.code')"
                name="serviceTicketCode"
              >
                <a-input
                  v-model:value="formState.serviceTicketCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.clientid')"
                name="clientId"
              >
                <a-input
                  v-model:value="formState.clientId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.clientid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.clientcode')"
                name="clientCode"
              >
                <a-input
                  v-model:value="formState.clientCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.clientcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.clientname')"
                name="clientName"
              >
                <a-input
                  v-model:value="formState.clientName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.clientname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.servicerequestid')"
                name="serviceRequestId"
              >
                <a-input
                  v-model:value="formState.serviceRequestId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.servicerequestid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.servicerequestcode')"
                name="serviceRequestCode"
              >
                <a-input
                  v-model:value="formState.serviceRequestCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.servicerequestcode') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.serviceTicket.serviceorderid')"
                name="serviceOrderId"
              >
                <a-input
                  v-model:value="formState.serviceOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.serviceorderid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.serviceordercode')"
                name="serviceOrderCode"
              >
                <a-input
                  v-model:value="formState.serviceOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.serviceordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.servicecontractid')"
                name="serviceContractId"
              >
                <a-input
                  v-model:value="formState.serviceContractId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.servicecontractid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.servicecontractcode')"
                name="serviceContractCode"
              >
                <a-input
                  v-model:value="formState.serviceContractCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.servicecontractcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.tickettype')"
                name="ticketType"
              >
                <a-input-number
                  v-model:value="formState.ticketType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.tickettype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.priority')"
                name="priority"
              >
                <a-input-number
                  v-model:value="formState.priority"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.priority') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.ticketstatus')"
                name="ticketStatus"
              >
                <a-input-number
                  v-model:value="formState.ticketStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.ticketstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.ticketsubject')"
                name="ticketSubject"
              >
                <a-input
                  v-model:value="formState.ticketSubject"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.ticketsubject') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serviceTicket.faultdescription')"
                name="faultDescription"
              >
                <a-textarea
                  v-model:value="formState.faultDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceTicket.faultdescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serviceTicket.solutiondescription')"
                name="solutionDescription"
              >
                <a-textarea
                  v-model:value="formState.solutionDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceTicket.solutiondescription') })"
                  :rows="2"
                  size="small"
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
                :label="t('entity.serviceTicket.servicelocation')"
                name="serviceLocation"
              >
                <a-input
                  v-model:value="formState.serviceLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.servicelocation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.assignedemployeeid')"
                name="assignedEmployeeId"
              >
                <a-input
                  v-model:value="formState.assignedEmployeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.assignedemployeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.assignedemployeename')"
                name="assignedEmployeeName"
              >
                <a-input
                  v-model:value="formState.assignedEmployeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.assignedemployeename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.scheduledstarttime')"
                name="scheduledStartTime"
              >
                <a-input
                  v-model:value="formState.scheduledStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.scheduledstarttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.scheduledendtime')"
                name="scheduledEndTime"
              >
                <a-input
                  v-model:value="formState.scheduledEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.scheduledendtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.actualstarttime')"
                name="actualStartTime"
              >
                <a-input
                  v-model:value="formState.actualStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.actualstarttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.actualendtime')"
                name="actualEndTime"
              >
                <a-input
                  v-model:value="formState.actualEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.actualendtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.acceptanceresult')"
                name="acceptanceResult"
              >
                <a-input-number
                  v-model:value="formState.acceptanceResult"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.acceptanceresult') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.acceptedby')"
                name="acceptedBy"
              >
                <a-input
                  v-model:value="formState.acceptedBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.acceptedby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.acceptedat')"
                name="acceptedAt"
              >
                <a-input
                  v-model:value="formState.acceptedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.acceptedat') })"
                  size="small"
                  allow-clear
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceTicket.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                  size="small"
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
                  :rows="2"
                  size="small"
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
 * 服务工单实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/service/service-ticket/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ServiceTicketCreate } from '@/types/logistics/customer-service/service-ticket'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

const { t } = useI18n()

const tenantStore = useTenantStore()
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
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","serviceTicketCode","clientId","clientCode","clientName","serviceRequestId","serviceRequestCode","serviceOrderId","serviceOrderCode","serviceContractId","serviceContractCode","ticketType","priority","ticketStatus","ticketSubject","faultDescription","solutionDescription","serviceLocation","assignedEmployeeId","assignedEmployeeName","scheduledStartTime","scheduledEndTime","actualStartTime","actualEndTime","acceptanceResult","acceptedBy","acceptedAt","sortOrder","extFieldJson","remark"]


interface Props {
  formData?: Partial<ServiceTicketCreate & { serviceTicketId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.serviceTicketId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.plantcode') }),
      trigger: 'blur'
    }
  ],
  serviceTicketCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.code') }),
      trigger: 'blur'
    }
  ],
  clientId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.clientid') }),
      trigger: 'blur'
    }
  ],
  clientCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.clientcode') }),
      trigger: 'blur'
    }
  ],
  clientName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.clientname') }),
      trigger: 'blur'
    }
  ],
  ticketType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceTicket.tickettype') }),
      trigger: 'change'
    }
  ],
  priority: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceTicket.priority') }),
      trigger: 'change'
    }
  ],
  ticketStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceTicket.ticketstatus') }),
      trigger: 'change'
    }
  ],
  ticketSubject: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceTicket.ticketsubject') }),
      trigger: 'blur'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceTicket.sortorder') }),
      trigger: 'change'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

  activeTab.value = 'tab-0'
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
