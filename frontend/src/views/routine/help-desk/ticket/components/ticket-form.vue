<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/ticket/components -->
<!-- 文件名称：ticket-form.vue -->
<!-- 功能描述：Takt工单实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ticket-form-tabs"
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
                :label="t('entity.ticket.no')"
                name="ticketNo"
              >
                <a-input
                  v-model:value="formState.ticketNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.no') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.title')"
                name="title"
              >
                <a-input
                  v-model:value="formState.title"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.title') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ticket.content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ticket.content') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.attachmentsjson')"
                name="attachmentsJson"
              >
                <a-input
                  v-model:value="formState.attachmentsJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.attachmentsjson') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.status')"
                name="ticketStatus"
              >
                <a-input-number
                  v-model:value="formState.ticketStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.priority')"
                name="priority"
              >
                <a-input-number
                  v-model:value="formState.priority"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.priority') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.categorycode')"
                name="categoryCode"
              >
                <a-input
                  v-model:value="formState.categoryCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.categorycode') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.source')"
                name="ticketSource"
              >
                <a-input-number
                  v-model:value="formState.ticketSource"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.source') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.submitterid')"
                name="submitterId"
              >
                <a-input
                  v-model:value="formState.submitterId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.submitterid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.submittername')"
                name="submitterName"
              >
                <a-input
                  v-model:value="formState.submitterName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.submittername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.assigneeid')"
                name="assigneeId"
              >
                <a-input
                  v-model:value="formState.assigneeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.assigneeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.assigneename')"
                name="assigneeName"
              >
                <a-input
                  v-model:value="formState.assigneeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.assigneename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.knowledgeid')"
                name="knowledgeId"
              >
                <a-input
                  v-model:value="formState.knowledgeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.knowledgeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.parentticketid')"
                name="parentTicketId"
              >
                <a-input
                  v-model:value="formState.parentTicketId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.parentticketid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.firstresponseat')"
                name="firstResponseAt"
              >
                <a-input
                  v-model:value="formState.firstResponseAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.firstresponsedueby')"
                name="firstResponseDueBy"
              >
                <a-input
                  v-model:value="formState.firstResponseDueBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponsedueby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.resolvedat')"
                name="resolvedAt"
              >
                <a-input
                  v-model:value="formState.resolvedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.resolvedat') })"
                  size="small"
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
                :label="t('entity.ticket.resolutiondueby')"
                name="resolutionDueBy"
              >
                <a-input
                  v-model:value="formState.resolutionDueBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.resolutiondueby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.closedat')"
                name="closedAt"
              >
                <a-input
                  v-model:value="formState.closedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.closedat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.applicantdeptid')"
                name="applicantDeptId"
              >
                <a-input
                  v-model:value="formState.applicantDeptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantdeptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.applicantdeptname')"
                name="applicantDeptName"
              >
                <a-input
                  v-model:value="formState.applicantDeptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantdeptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.applicantby')"
                name="applicantBy"
              >
                <a-input
                  v-model:value="formState.applicantBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.childtickets')"
                name="childTickets"
              >
                <a-input
                  v-model:value="formState.childTickets"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.childtickets') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.changelogs')"
                name="changeLogs"
              >
                <a-input
                  v-model:value="formState.changeLogs"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.changelogs') })"
                  size="small"
                  allow-clear
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
 * Takt工单实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/help-desk/ticket/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TicketCreate } from '@/types/routine/help-desk/ticket'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","ticketNo","title","content","attachmentsJson","ticketStatus","priority","categoryCode","ticketSource","submitterId","submitterName","assigneeId","assigneeName","knowledgeId","parentTicketId","firstResponseAt","firstResponseDueBy","resolvedAt","resolutionDueBy","closedAt","flowInstanceId","applicantDeptId","applicantDeptName","applicantBy","childTickets","changeLogs","extFieldJson","remark"]


interface Props {
  formData?: Partial<TicketCreate & { ticketId?: string }> | null
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
    const isCreate = !props.formData?.ticketId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  ticketNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ticket.no') }),
      trigger: 'blur'
    }
  ],
  title: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ticket.title') }),
      trigger: 'blur'
    }
  ],
  ticketStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ticket.status') }),
      trigger: 'change'
    }
  ],
  priority: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ticket.priority') }),
      trigger: 'change'
    }
  ],
  ticketSource: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ticket.source') }),
      trigger: 'change'
    }
  ],
  submitterId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ticket.submitterid') }),
      trigger: 'blur'
    }
  ],
  applicantBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantby') }),
      trigger: 'blur'
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
