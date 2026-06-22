<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/ticket-change-log/components -->
<!-- 文件名称：ticket-form.vue -->
<!-- 功能描述：Takt工单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ticket-form flex flex-col min-h-0"
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
                :label="t('entity.ticket.no')"
                name="ticketNo"
              >
                <a-input
                  v-model:value="formState.ticketNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.no') })"
                  show-count
                  :maxlength="50"
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
                  show-count
                  :maxlength="200"
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
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.status')"
                name="ticketStatus"
              >
                <TaktSelect
                  v-model:value="formState.ticketStatus"
                  dict-type="sys_ticket_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.status') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.priority')"
                name="priority"
              >
                <TaktSelect
                  v-model:value="formState.priority"
                  dict-type="sys_priority_level_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.priority') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.urgency')"
                name="urgency"
              >
                <TaktSelect
                  v-model:value="formState.urgency"
                  dict-type="sys_urgency_level_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.urgency') })"
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
                :label="t('entity.ticket.impact')"
                name="impact"
              >
                <TaktSelect
                  v-model:value="formState.impact"
                  dict-type="sys_impact_level_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.impact') })"
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
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.ticketId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.source')"
                name="ticketSource"
              >
                <a-input-number
                  v-model:value="formState.ticketSource"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.source') })"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.firstresponsedueby')"
                name="firstResponseDueBy"
              >
                <a-input
                  v-model:value="formState.firstResponseDueBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponsedueby') })"
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.resolutiondueby')"
                name="resolutionDueBy"
              >
                <a-input
                  v-model:value="formState.resolutionDueBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.resolutiondueby') })"
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="100"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ticket.extfield')"
                name="ExtField"
              >
                <a-textarea
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ticket.extfield') })"
                  :rows="2"
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
    <!-- 下：子表 changeLogs -->
    <TaktEditableTable
      ref="ticketChangeLogTableRef"
      v-model="childTicketChangeLogRows"
      :columns="ticketChangeLogFormColumns"
      :title="t('entity.ticketchangelog._self')"
      :add-button-entity="t('entity.ticketchangelog._self')"
      id-field="ticketChangeLogId"
      :default-row="createDefaultTicketChangeLogRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt工单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/ticket-change-log/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TicketCreate } from '@/types/routine/help-desk/ticket'
import TaktSelect from '@/components/business/takt-select/index.vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","ticketNo","title","content","attachmentsJson","ticketStatus","priority","urgency","impact","categoryCode","ticketSource","submitterId","submitterName","assigneeId","assigneeName","knowledgeId","parentTicketId","firstResponseAt","firstResponseDueBy","resolvedAt","resolutionDueBy","closedAt","flowInstanceId","applicantDeptId","applicantDeptName","applicantBy","childTickets","ExtField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childTicketChangeLogRows = ref<Record<string, unknown>[]>([])
const ticketChangeLogTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 ticketChangeLog 可编辑列 */
const ticketChangeLogFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'ticketNo',
    title: t('entity.ticketchangelog.ticketno'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.ticketchangelog.ticketno') }),
  },
  {
    key: 'changeType',
    title: t('entity.ticketchangelog.changetype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'changeSummary',
    title: t('entity.ticketchangelog.changesummary'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.ticketchangelog.changesummary') }),
  },
  {
    key: 'changeFields',
    title: t('entity.ticketchangelog.changefields'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.ticketchangelog.changefields') }),
  },
  {
    key: 'changeReason',
    title: t('entity.ticketchangelog.changereason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.ticketchangelog.changereason') }),
  },
  {
    key: 'ExtField',
    title: t('entity.ticketchangelog.extfield'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.optional', { field: t('entity.ticketchangelog.extfield') }),
    width: 140,
  },
  {
    key: 'remark',
    title: t('common.page.entity.remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') }),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<TicketCreate & { ticketId?: string }> | null | undefined) {
  childTicketChangeLogRows.value = ((val as any)?.changeLogs ?? []) as Record<string, unknown>[]
}

function createDefaultTicketChangeLogRow(): Record<string, unknown> {
  return {
    ticketNo: '',
    changeType: 0,
    changeSummary: '',
    changeFields: '',
    changeReason: '',
    ExtField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.ticketId ?? ''
  return {
    ...formState,
    changeLogs: ticketChangeLogTableRef.value?.getRows?.() ?? childTicketChangeLogRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ticketId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TicketCreate & { ticketId?: string }> | null
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
  ticketStatus: 0,
  priority: 3,
  urgency: 3,
  impact: 3
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 ticketId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ticketId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).changeLogs
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
    const isCreate = !props.formData?.ticketId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
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
  ticketStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.priority') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.priority') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  urgency: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.urgency') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.urgency') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  impact: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.impact') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.impact') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ticketSource: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.source') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ticket.source') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await ticketChangeLogTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('ticketStatus' in payload) {
    const rawticketStatus = payload.ticketStatus
    payload.ticketStatus = typeof rawticketStatus === 'number' ? rawticketStatus : Number(rawticketStatus)
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    payload.priority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
  }
  if ('urgency' in payload) {
    const rawurgency = payload.urgency
    payload.urgency = typeof rawurgency === 'number' ? rawurgency : Number(rawurgency)
  }
  if ('impact' in payload) {
    const rawimpact = payload.impact
    payload.impact = typeof rawimpact === 'number' ? rawimpact : Number(rawimpact)
  }
  if ('ticketSource' in payload) {
    const rawticketSource = payload.ticketSource
    payload.ticketSource = typeof rawticketSource === 'number' ? rawticketSource : Number(rawticketSource)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ticketId)
  childTicketChangeLogRows.value = []
  ticketChangeLogTableRef.value?.resetRows?.()
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
