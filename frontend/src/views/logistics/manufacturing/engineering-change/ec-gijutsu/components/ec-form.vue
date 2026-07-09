<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：ec-form.vue -->
<!-- 功能描述：设变维护弹窗内嵌表单；主表仅 ecLeader/ecDistinction/ecEntryDate/ecContent/ecStatus/remark 可编辑；附件 Tab 工具栏增删改（来源导入无预置行，须手工维护） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ec-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
                :label="t('entity.ec.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.plantcode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.no')"
                name="ecNo"
              >
                <a-input
                  v-model:value="formState.ecNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.no') })"
                  show-count
                  :maxlength="10"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.issuedate')"
                name="ecIssueDate"
              >
                <a-date-picker
                  v-model:value="formState.ecIssueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.issuedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.changestatus')"
                name="changeStatus"
              >
                <TaktSelect
                  v-model:value="formState.changeStatus"
                  dict-type="logistics_ec_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.changestatus') })"
                  allow-clear
                  class="w-full"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.title')"
                name="ecTitle"
              >
                <a-input
                  v-model:value="formState.ecTitle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.title') })"
                  show-count
                  :maxlength="500"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.leader')"
                name="ecLeader"
              >
                <TaktSelect
                  v-model:value="formState.ecLeader"
                  api-url="TaktEmployees/options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.leader') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.distinction')"
                name="ecDistinction"
              >
                <TaktSelect
                  v-model:value="formState.ecDistinction"
                  dict-type="logistics_ec_distinction_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.distinction') })"
                  allow-clear
                  class="w-full"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.entrydate')"
                name="ecEntryDate"
              >
                <a-date-picker
                  v-model:value="formState.ecEntryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.entrydate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ec.content')"
                name="ecContent"
              >
                <a-textarea
                  v-model:value="formState.ecContent"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.content') })"
                  :rows="8"
                  allow-clear
                  :disabled="loading"
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
                :label="t('entity.ec.lossamount')"
                name="ecLossAmount"
              >
                <a-input-number
                  v-model:value="formState.ecLossAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.lossamount') })"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ec.status')"
                name="ecStatus"
              >
                <TaktSelect
                  v-model:value="formState.ecStatus"
                  dict-type="logistics_ec_gijutsu_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.status') })"
                  allow-clear
                  class="w-full"
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
                  disabled
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
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('entity.ecdetail._self')"
        force-render
      >
        <div class="ec-form-sub-table-wrap min-h-0 flex-1">
          <TaktSingleTable
            class="h-full min-h-0"
            entity-scope="company"
            :columns="ecDetailTableColumns"
            :data-source="childEcDetailRows"
            :loading="loading"
            :stripe="true"
            :row-key="getEcDetailRowKey"
            :show-row-selection="false"
            :include-audit-fields="false"
            scroll-layout="editable"
            table-mode="single"
            :show-pagination="false"
          />
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('entity.ecattachment._self')"
        force-render
      >
        <TaktToolsBar
          create-permission="logistics:manufacturing:engineering:change:gijutsu:create"
          update-permission="logistics:manufacturing:engineering:change:gijutsu:update"
          delete-permission="logistics:manufacturing:engineering:change:gijutsu:delete"
          :show-create="true"
          :show-update="true"
          :show-delete="true"
          :show-import="false"
          :show-export="false"
          :show-expand="false"
          :show-refresh="false"
          :show-advanced-query="false"
          :show-column-setting="false"
          :show-fullscreen="false"
          :create-disabled="loading"
          :update-disabled="loading || attachmentUpdateDisabled"
          :delete-disabled="loading || attachmentDeleteDisabled"
          :create-loading="loading"
          :update-loading="loading"
          :delete-loading="loading"
          @create="handleAttachmentCreate"
          @update="handleAttachmentUpdate"
          @delete="handleAttachmentDelete"
        />
        <div class="ec-form-sub-table-wrap min-h-0 flex-1">
          <TaktSingleTable
            class="h-full min-h-0"
            entity-scope="company"
            :columns="ecAttachmentTableColumns"
            :data-source="childEcAttachmentRows"
            :loading="loading"
            :stripe="true"
            :row-key="getEcAttachmentTableRowKey"
            :row-selection="attachmentRowSelection"
            :custom-row="onAttachmentClickRow"
            :show-row-selection="true"
            :include-audit-fields="false"
            scroll-layout="editable"
            table-mode="single"
            :show-pagination="false"
          />
        </div>
        <TaktModal
          v-model:open="attachmentFormVisible"
          :title="attachmentFormTitle"
          width="720px"
          :confirm-loading="attachmentFormLoading"
          @ok="handleAttachmentFormSubmit"
          @cancel="handleAttachmentFormCancel"
        >
          <EcAttachmentForm
            :key="String(attachmentFormData?.ecAttachmentId ?? attachmentFormData?.__rowKey ?? 'create')"
            ref="attachmentFormRef"
            :form-data="attachmentFormData"
            :master-id="masterEcIdForAttachment"
            :loading="attachmentFormLoading || loading"
          />
        </TaktModal>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 设变维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { reactive, watch, computed, ref, nextTick, h } from 'vue'
import { useI18n } from 'vue-i18n'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcGijutsuFormData } from '@/types/logistics/manufacturing/engineering-change/ec-gijutsu'
import { RiQuestionLine, RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import EcAttachmentForm from './ec-attachment-form.vue'
import TaktDictTag from '@/components/common/takt-dict-tag/index.vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","ecNo","ecIssueDate","changeStatus","ecTitle","ecContent","ecLeader","ecLossAmount","ecDistinction","ecEntryDate","ecStatus","extField","remark"]

const childEcDetailRows = ref<Record<string, unknown>[]>([])
const childEcAttachmentRows = ref<Record<string, unknown>[]>([])
/** 附件子表选中行 */
const attachmentSelectedRowKeys = ref<(string | number)[]>([])
const attachmentSelectedRows = ref<Record<string, unknown>[]>([])
const attachmentSelectedRow = ref<Record<string, unknown> | null>(null)
/** 附件子弹窗 */
const attachmentFormVisible = ref(false)
const attachmentFormTitle = ref('')
const attachmentFormData = ref<Record<string, unknown>>({})
const attachmentFormLoading = ref(false)
const attachmentFormRef = ref()

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: EcGijutsuFormData | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 来源设变导入草稿模式：明细只读，附件须上传后保存 */
  sourceImportMode?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  sourceImportMode: false,
})

/** 主表 Id（附件 form 外键；未保存时为空） */
const masterEcIdForAttachment = computed(() => props.formData?.ecGijutsuId ?? '')
const attachmentUpdateDisabled = computed(() => attachmentSelectedRows.value.length !== 1)
const attachmentDeleteDisabled = computed(() => attachmentSelectedRows.value.length === 0)

/** 明细子表列（只读展示） */
const ecDetailTableColumns = computed<TableColumnsType>(() => [
  {
    title: t('entity.ecdetail.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 140,
    ellipsis: true,
    customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'ecNo'),
  },
  {
    title: t('entity.ecdetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 100,
    ellipsis: true,
    customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'lineNumber'),
  },
  {
    title: t('entity.ecdetail.ecmodel'),
    dataIndex: 'ecModel',
    key: 'ecModel',
    width: 140,
    ellipsis: true,
    customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'ecModel'),
  },
  {
    title: t('entity.ecdetail.ecbomitem'),
    dataIndex: 'ecBomItem',
    key: 'ecBomItem',
    width: 140,
    ellipsis: true,
    customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'ecBomItem'),
  },
  {
    title: t('entity.ecdetail.ecbomsubitem'),
    dataIndex: 'ecBomSubItem',
    key: 'ecBomSubItem',
    width: 140,
    ellipsis: true,
    customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'ecBomSubItem'),
  },
])

/** 附件子表列（只读展示 + 操作列） */
const ecAttachmentTableColumns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.ecattachment.linenumber'),
      dataIndex: 'lineNumber',
      key: 'lineNumber',
      width: 90,
      ellipsis: true,
      customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'lineNumber'),
    },
    {
      title: t('entity.ecattachment.attachmenttype'),
      dataIndex: 'attachmentType',
      key: 'attachmentType',
      width: 120,
      ellipsis: true,
      customRender: ({ record }) => h(TaktDictTag, {
        dictType: 'logistics_ec_attachment_type',
        value: String((record as Record<string, unknown>).attachmentType ?? ''),
      }),
    },
    {
      title: t('entity.ecattachment.docno'),
      dataIndex: 'docNo',
      key: 'docNo',
      width: 140,
      ellipsis: true,
      customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'docNo'),
    },
    {
      title: t('entity.ecattachment.filename'),
      dataIndex: 'fileName',
      key: 'fileName',
      width: 140,
      ellipsis: true,
      customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'fileName'),
    },
    {
      title: t('entity.ecattachment.accessurl'),
      dataIndex: 'accessUrl',
      key: 'accessUrl',
      width: 200,
      ellipsis: true,
      customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'accessUrl'),
    },
  ]
  const actions = [
    {
      key: 'update',
      label: t('common.page.button.edit'),
      shape: 'plain' as const,
      icon: RiEditLine,
      permission: 'logistics:manufacturing:engineering:change:gijutsu:update',
      onClick: (record: Record<string, unknown>) => void handleAttachmentEdit(record),
    },
    {
      key: 'delete',
      label: t('common.page.button.delete'),
      shape: 'plain' as const,
      icon: RiDeleteBinLine,
      permission: 'logistics:manufacturing:engineering:change:gijutsu:delete',
      onClick: (record: Record<string, unknown>) => void handleAttachmentDeleteOne(record),
    },
  ]
  cols.push(CreateActionColumn({ actions }))
  return cols
})

/**
 * 明细行 row-key
 * @param record 明细行
 * @param index 行索引
 * @returns 行 key
 */
function getEcDetailRowKey(record: Record<string, unknown>, index?: number): string {
  return String(record.ecDetailId ?? record.__rowKey ?? `detail-row-${index ?? 0}`)
}

/**
 * 附件行 row-key（TaktSingleTable）
 * @param record 附件行
 * @param index 行索引
 * @returns 行 key
 */
function getEcAttachmentTableRowKey(record: Record<string, unknown>, index?: number): string {
  return getAttachmentRowKey(record, index ?? 0)
}

/**
 * 子表单元格展示文本
 * @param record 行数据
 * @param key 字段名
 * @returns 展示文本
 */
function formatSubTableCell(record: Record<string, unknown>, key: string): string {
  const value = record[key]
  if (value === undefined || value === null || value === '') {
    return '-'
  }
  return String(value)
}

/** 规范化附件行（补全 __rowKey） */
function normalizeAttachmentRows(rows: Record<string, unknown>[]): Record<string, unknown>[] {
  return rows.map((row, index) => ({
    ...row,
    __rowKey: row.__rowKey ?? row.ecAttachmentId ?? `row-${index}`,
  }))
}

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: EcGijutsuFormData | null | undefined) {
  childEcDetailRows.value = ((val as any)?.ecDetails ?? []) as Record<string, unknown>[]
  childEcAttachmentRows.value = normalizeAttachmentRows(((val as any)?.attachments ?? []) as Record<string, unknown>[])
  clearAttachmentSelection()
}

/**
 * 附件行唯一键
 * @param record 附件行
 * @param index 行索引
 * @returns 行 key
 */
function getAttachmentRowKey(record: Record<string, unknown>, index: number): string {
  return String(record.__rowKey ?? record.ecAttachmentId ?? `row-${index}`)
}

/** 清空附件子表选中态 */
function clearAttachmentSelection() {
  attachmentSelectedRowKeys.value = []
  attachmentSelectedRows.value = []
  attachmentSelectedRow.value = null
}

/**
 * 是否同一附件行
 * @param row 列表行
 * @param target 目标行
 * @param index 列表索引
 */
function isSameAttachmentRow(
  row: Record<string, unknown>,
  target: Record<string, unknown>,
  index: number,
): boolean {
  const rowKey = getAttachmentRowKey(row, index)
  const targetKey = getAttachmentRowKey(target, 0)
  if (rowKey === targetKey) {
    return true
  }
  const rowId = String(row.ecAttachmentId ?? '')
  const targetId = String(target.ecAttachmentId ?? '')
  return rowId.length > 0 && rowId === targetId
}

const attachmentRowSelection = computed(() => ({
  selectedRowKeys: attachmentSelectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Record<string, unknown>[]) => {
    attachmentSelectedRowKeys.value = keys
    attachmentSelectedRows.value = rows
    attachmentSelectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Record<string, unknown>, selected: boolean) => {
    if (selected) {
      attachmentSelectedRow.value = record
    } else if (
      attachmentSelectedRow.value != null
      && isSameAttachmentRow(record, attachmentSelectedRow.value, 0)
    ) {
      attachmentSelectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, rows: Record<string, unknown>[]) => {
    attachmentSelectedRow.value = selected && rows.length === 1 ? (rows[0] ?? null) : null
  },
}))

/**
 * 附件行点击选中
 * @param record 行数据
 */
function onAttachmentClickRow(record: Record<string, unknown>) {
  const index = childEcAttachmentRows.value.findIndex((row, i) => isSameAttachmentRow(row, record, i))
  const key = getAttachmentRowKey(record, index >= 0 ? index : 0)
  return {
    onClick: () => {
      attachmentSelectedRowKeys.value = [key]
      attachmentSelectedRows.value = [record]
      attachmentSelectedRow.value = record
    },
    class: attachmentSelectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/** 打开新增附件弹窗 */
function handleAttachmentCreate() {
  attachmentFormTitle.value = t('common.dialog.title.create', { entity: t('entity.ecattachment._self') })
  attachmentFormData.value = { ecNo: String(formState.ecNo ?? '') }
  attachmentFormVisible.value = true
}

/** 工具栏编辑：打开当前单选附件行 */
function handleAttachmentUpdate() {
  if (attachmentSelectedRow.value) {
    handleAttachmentEdit(attachmentSelectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: t('entity.ecattachment._self'),
    }))
  }
}

/**
 * 打开编辑附件弹窗
 * @param record 附件行
 */
function handleAttachmentEdit(record: Record<string, unknown>) {
  attachmentFormTitle.value = t('common.dialog.title.edit', { entity: t('entity.ecattachment._self') })
  attachmentFormData.value = { ...record }
  attachmentFormVisible.value = true
}

/** 提交附件子弹窗（内存增改，随主表一并保存） */
async function handleAttachmentFormSubmit() {
  const refInst = attachmentFormRef.value
  if (!refInst?.validate) {
    return
  }
  try {
    await refInst.validate()
  } catch {
    return
  }
  attachmentFormLoading.value = true
  try {
    const values = refInst.getValues?.() ?? {}
    const editing = attachmentFormData.value
    const editingId = editing?.ecAttachmentId
    const editingClientKey = editing?.__rowKey
    const isEdit = !!(editingId || editingClientKey)
    if (isEdit) {
      childEcAttachmentRows.value = childEcAttachmentRows.value.map((row, index) => {
        if (isSameAttachmentRow(row, editing, index)) {
          return {
            ...row,
            ...values,
            ecAttachmentId: row.ecAttachmentId,
            __rowKey: row.__rowKey ?? editingClientKey,
          }
        }
        return row
      })
    } else {
      const rows = childEcAttachmentRows.value
      const maxLine = rows.reduce((max, row) => {
        const line = Number(row.lineNumber)
        return Number.isFinite(line) ? Math.max(max, line) : max
      }, 0)
      const lineNumber = values.lineNumber ?? (maxLine > 0 ? maxLine + 10 : 10)
      childEcAttachmentRows.value = [
        ...rows,
        {
          __rowKey: `client-${crypto.randomUUID()}`,
          ...values,
          lineNumber,
          ecNo: String(formState.ecNo ?? ''),
        },
      ]
    }
    attachmentFormVisible.value = false
    attachmentFormData.value = {}
    nextTick(() => attachmentFormRef.value?.resetFields?.())
    clearAttachmentSelection()
  } finally {
    attachmentFormLoading.value = false
  }
}

/** 关闭附件子弹窗 */
function handleAttachmentFormCancel() {
  attachmentFormVisible.value = false
  attachmentFormData.value = {}
  nextTick(() => attachmentFormRef.value?.resetFields?.())
}

/**
 * 从内存列表移除附件行
 * @param records 待删行
 */
function removeAttachmentRows(records: Record<string, unknown>[]) {
  if (records.length === 0) {
    return
  }
  childEcAttachmentRows.value = childEcAttachmentRows.value.filter(
    (row, index) => !records.some((target) => isSameAttachmentRow(row, target, index)),
  )
  clearAttachmentSelection()
}

/** 工具栏批删附件行 */
function handleAttachmentDelete() {
  if (attachmentSelectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.ecattachment._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.ecattachment._self'),
      count: attachmentSelectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: () => {
      removeAttachmentRows([...attachmentSelectedRows.value])
      message.success(t('common.feedback.deleted', { target: t('entity.ecattachment._self') }))
    },
  })
}

/**
 * 行内删除单条附件
 * @param record 附件行
 */
function handleAttachmentDeleteOne(record: Record<string, unknown>) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.ecattachment._self'),
      name: t('common.tip.this.target', { target: t('entity.ecattachment._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: () => {
      removeAttachmentRows([record])
      message.success(t('common.feedback.deleted', { target: t('entity.ecattachment._self') }))
    },
  })
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.ecGijutsuId ?? ''
  const payload: Record<string, unknown> = {
    ...formState,
    ecDetails: childEcDetailRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ecId: masterId,
    })),
    attachments: childEcAttachmentRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ecId: masterId,
      ecNo: formState.ecNo,
    })),
  }
  return payload
}

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 ecId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecGijutsuId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      delete (next as any).ecDetails
      delete (next as any).attachments
      applyScopeDefaults(next)
      Object.assign(formState, next)
      syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        delete (next as any).ecDetails
        delete (next as any).attachments
        Object.assign(formState, next)
        syncChildRowsFromFormData(val)
      } else {
        childEcDetailRows.value = []
        childEcAttachmentRows.value = []
        clearAttachmentSelection()
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
    const isCreate = !props.formData?.ecGijutsuId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.plantcode') }),
      trigger: 'blur'
    }
  ],
  ecNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.no') }),
      trigger: 'blur'
    }
  ],
  ecIssueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ec.issuedate') }),
      trigger: 'change'
    }
  ],
  changeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ec.changestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ec.changestatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecTitle: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.title') }),
      trigger: 'blur'
    }
  ],
  ecContent: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.content') }),
      trigger: 'blur'
    }
  ],
  ecLeader: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ec.leader') }),
      trigger: 'change'
    }
  ],
  ecLossAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ec.lossamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ec.lossamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecDistinction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ec.distinction') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ec.distinction') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecEntryDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ec.entrydate') }),
      trigger: 'change'
    }
  ],
  ecStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ec.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ec.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  const attachmentRows = childEcAttachmentRows.value
  if (props.sourceImportMode && attachmentRows.length === 0) {
    activeTab.value = 'tab-3'
    const msg = t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.attachmentRequired')
    message.warning(msg)
    throw new Error(msg)
  }
  for (let i = 0; i < attachmentRows.length; i += 1) {
    const row = attachmentRows[i]
    const url = String(row.accessUrl ?? '').trim()
    const fileName = String(row.fileName ?? '').trim()
    const attachmentType = String(row.attachmentType ?? '').trim()
    const docNo = String(row.docNo ?? '').trim()
    if (!props.sourceImportMode && (!attachmentType || !docNo)) {
      activeTab.value = 'tab-3'
      const msg = t('common.page.form.placeholder.required', { field: t('entity.ecattachment._self') })
      message.warning(msg)
      throw new Error(msg)
    }
    if (!url || url === '-' || !fileName) {
      activeTab.value = 'tab-3'
      const msg = t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.attachmentUploadRequired', { row: i + 1 })
      message.warning(msg)
      throw new Error(msg)
    }
  }
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('changeStatus' in payload) {
    const rawchangeStatus = payload.changeStatus
    payload.changeStatus = typeof rawchangeStatus === 'number' ? rawchangeStatus : Number(rawchangeStatus)
  }
  if ('ecLossAmount' in payload) {
    const rawecLossAmount = payload.ecLossAmount
    payload.ecLossAmount = typeof rawecLossAmount === 'number' ? rawecLossAmount : Number(rawecLossAmount)
  }
  if ('ecStatus' in payload) {
    const rawecStatus = payload.ecStatus
    payload.ecStatus = typeof rawecStatus === 'number' ? rawecStatus : Number(rawecStatus)
  }
  if ('ecDistinction' in payload) {
    const rawEcDistinction = payload.ecDistinction
    payload.ecDistinction = typeof rawEcDistinction === 'number' ? rawEcDistinction : Number(rawEcDistinction)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecGijutsuId)
  childEcDetailRows.value = []
  childEcAttachmentRows.value = []
  clearAttachmentSelection()
  attachmentFormVisible.value = false
  attachmentFormData.value = {}
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
