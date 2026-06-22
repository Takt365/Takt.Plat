<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/ticket/components -->
<!-- 文件名称：ticket-form.vue -->
<!-- 功能描述：Takt工单实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      <!-- 主表 -->
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
                  :placeholder="isEditMode ? t('common.page.form.placeholder.required', { field: t('entity.ticket.no') }) : t('routine.help-desk.ticket.page.ticket.no.auto')"
                  size="small"
                  :disabled="!isEditMode"
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
                <TaktSelect
                  v-model:value="formState.ticketStatus"
                  dict-type="sys_ticket_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.status') })"
                  size="small"
                  disabled
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
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.impact')"
                name="impact"
              >
                <TaktSelect
                  v-model:value="formState.impact"
                  dict-type="sys_impact_level_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.impact') })"
                  size="small"
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
                  size="small"
                  disabled
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ticket.assetcode')"
                name="assetCode"
              >
                <TaktSelect
                  v-model:value="formState.assetCode"
                  :options="assetOptions"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.assetcode') })"
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
                <TaktSelect
                  v-model:value="formState.ticketSource"
                  dict-type="routine_ticket_source_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.source') })"
                  size="small"
                  :disabled="isEditMode"
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
                  readonly
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
                  readonly
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
                  readonly
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
                :label="t('common.page.entity.ExtField')"
                name="ExtField"
              >
                <a-input
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
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
      <!-- 子表：ticketChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.ticketChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddTicketChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.ticketChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="ticketChangeLogFormColumns"
          :data-source="childTicketChangeLogRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'ticketNo'">
              <a-input
                v-model:value="record.ticketNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticketChangeLog.ticketno') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeType'">
              <a-input-number
                v-model:value="record.changeType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticketChangeLog.changetype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'changeSummary'">
              <a-input
                v-model:value="record.changeSummary"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticketChangeLog.changesummary') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeFields'">
              <a-input
                v-model:value="record.changeFields"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticketChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticketChangeLog.changereason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'ExtField'">
              <a-input
                v-model:value="record.ExtField"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveTicketChangeLogRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt工单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/ticket/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TicketCreate, TicketChangeLogCreate, TicketChangeLog } from '@/types/routine/help-desk/ticket'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { getAssetList } from '@/api/accounting/financial/asset'
import { resolveTicketPriority } from '@/utils/takt-ticket-priority'

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

/**
 * 新增态默认：Open 状态、门户来源、当前用户为提交人
 * @param target 表单数据
 */
function applyNewTicketDefaults(target: Record<string, unknown>) {
  if (target.ticketId) {
    return
  }
  if (target.ticketStatus == null) {
    target.ticketStatus = 0
  }
  if (target.ticketSource == null) {
    target.ticketSource = 0
  }
  if (target.urgency == null) {
    target.urgency = 3
  }
  if (target.impact == null) {
    target.impact = 3
  }
  target.priority = resolveTicketPriority(
    target.urgency as number | undefined,
    target.impact as number | undefined,
  )
  if (!target.submitterId && userStore.userId) {
    target.submitterId = userStore.userId
  }
  if (!target.submitterName && userStore.userInfo?.userName) {
    target.submitterName = userStore.userInfo.userName
  }
  if (!target.applicantBy && userStore.userId) {
    target.applicantBy = userStore.userId
  }
}

/** 是否编辑态（有 ticketId） */
const isEditMode = computed(() => !!props.formData?.ticketId)
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","ticketNo","title","content","attachmentsJson","ticketStatus","urgency","impact","priority","categoryCode","assetCode","ticketSource","submitterId","submitterName","assigneeId","assigneeName","knowledgeId","parentTicketId","firstResponseAt","firstResponseDueBy","resolvedAt","resolutionDueBy","closedAt","flowInstanceId","applicantDeptId","applicantDeptName","applicantBy","childTickets","ExtField","remark"]

/** 资产号码下拉选项（value=AssetCode） */
const assetOptions = ref<Array<{ label: string; value: string }>>([])

/** 加载固定资产选项 */
async function loadAssetOptions() {
  try {
    const res = await getAssetList({ pageIndex: 1, pageSize: 500 })
    assetOptions.value = (res.data ?? [])
      .filter((item) => item.assetCode)
      .map((item) => ({
        label: item.assetName ? `${item.assetName} (${item.assetCode})` : (item.assetCode ?? ''),
        value: item.assetCode ?? '',
      }))
  } catch {
    assetOptions.value = []
  }
}

/** ticketChangeLog 子表行（表单 Tab 内嵌） */
const childTicketChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 ticketChangeLog 表单列定义 */
const ticketChangeLogFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.ticketChangeLog.ticketno'),
    dataIndex: 'ticketNo',
    key: 'ticketNo',
    width: 140,
  },
  {
    title: t('entity.ticketChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 140,
  },
  {
    title: t('entity.ticketChangeLog.changesummary'),
    dataIndex: 'changeSummary',
    key: 'changeSummary',
    width: 140,
  },
  {
    title: t('entity.ticketChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.ticketChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 140,
  },
  {
    title: t('common.page.entity.ExtField'),
    dataIndex: 'ExtField',
    key: 'ExtField',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<TicketCreate & { ticketId?: string }> | null | undefined) {
  childTicketChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.ticketChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 ticketChangeLog 行 */
function handleAddTicketChangeLogRow() {
  childTicketChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ticketNo: '',
      changeType: 0,
      changeSummary: '',
      changeFields: '',
      changeReason: '',
      ExtField: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 ticketChangeLog 行 */
function handleRemoveTicketChangeLogRow(index: number) {
  childTicketChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    changeLogs: childTicketChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TicketCreate & { ticketId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).changeLogs
    applyScopeDefaults(next)
    applyNewTicketDefaults(next)
    Object.assign(formState, next)
    syncChildRowsFromFormData(val)
  },
  { immediate: true, deep: true }
)

/** 紧急度/影响范围变更时本地预览优先级（与服务端矩阵一致） */
watch(
  () => [formState.urgency, formState.impact] as const,
  ([urgency, impact]) => {
    formState.priority = resolveTicketPriority(urgency, impact)
  },
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.ticketId
    if (isCreate) {
      applyScopeDefaults(formState, true)
      applyNewTicketDefaults(formState)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ticketNo: isEditMode.value
    ? [
        {
          required: true,
          message: t('common.page.form.placeholder.required', { field: t('entity.ticket.no') }),
          trigger: 'blur',
        },
      ]
    : [],
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
  urgency: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ticket.urgency') }),
      trigger: 'change'
    }
  ],
  impact: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ticket.impact') }),
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

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  return buildSubmitPayload()
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  childTicketChangeLogRows.value = []
  activeTab.value = 'tab-0'
}

defineExpose({ validate, getValues, resetFields })

onMounted(() => {
  loadAssetOptions()
})
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
