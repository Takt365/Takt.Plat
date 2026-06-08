<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec/components -->
<!-- 文件名称：ec-form.vue -->
<!-- 功能描述：设变维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ec-form-tabs"
    >
      <!-- 主表 -->
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
                :label="t('entity.ec.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.plantcode') })"
                  size="small"
                  allow-clear
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
                  size="small"
                  allow-clear
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
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.changestatus')"
                name="changeStatus"
              >
                <a-input-number
                  v-model:value="formState.changeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.changestatus') })"
                  size="small"
                  style="width: 100%"
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
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.detailtext')"
                name="ecDetailText"
              >
                <a-input
                  v-model:value="formState.ecDetailText"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.detailtext') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.leader')"
                name="ecLeader"
              >
                <a-input
                  v-model:value="formState.ecLeader"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.leader') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.lossamount')"
                name="ecLossAmount"
              >
                <a-input-number
                  v-model:value="formState.ecLossAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.lossamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.distinction')"
                name="ecDistinction"
              >
                <a-input
                  v-model:value="formState.ecDistinction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.distinction') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.effectivedate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.effectivedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
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
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ec.status')"
                name="ecStatus"
              >
                <a-input-number
                  v-model:value="formState.ecStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.status') })"
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
      <!-- 子表：ecDetail -->
      <a-tab-pane
        key="child-ecDetails"
        :tab="t('entity.ecDetail._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddEcDetailRow">
            {{ t('common.page.button.create') }}{{ t('entity.ecDetail._self') }}
          </a-button>
        </div>
        <a-table
          :columns="ecDetailFormColumns"
          :data-source="childEcDetailRows"
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
            <template v-else-if="column.key === 'ecNo'">
              <a-input
                v-model:value="record.ecNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecno') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'ecModel'">
              <a-input
                v-model:value="record.ecModel"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecmodel') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'ecBomItem'">
              <a-input
                v-model:value="record.ecBomItem"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecbomitem') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'ecBomSubItem'">
              <a-input
                v-model:value="record.ecBomSubItem"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecbomsubitem') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'ecBomNo'">
              <a-input
                v-model:value="record.ecBomNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecbomno') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'ecChange'">
              <a-input
                v-model:value="record.ecChange"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecchange') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'ecLocal'">
              <a-input
                v-model:value="record.ecLocal"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.eclocal') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveEcDetailRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：ecAttachment -->
      <a-tab-pane
        key="child-attachments"
        :tab="t('entity.ecAttachment._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddEcAttachmentRow">
            {{ t('common.page.button.create') }}{{ t('entity.ecAttachment._self') }}
          </a-button>
        </div>
        <a-table
          :columns="ecAttachmentFormColumns"
          :data-source="childEcAttachmentRows"
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
            <template v-else-if="column.key === 'ecNo'">
              <a-input
                v-model:value="record.ecNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecAttachment.ecno') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecAttachment.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'attachmentType'">
              <a-input
                v-model:value="record.attachmentType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecAttachment.attachmenttype') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'docNo'">
              <a-input
                v-model:value="record.docNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecAttachment.docno') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileName'">
              <a-input
                v-model:value="record.fileName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecAttachment.filename') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'accessUrl'">
              <a-input
                v-model:value="record.accessUrl"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecAttachment.accessurl') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
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
              <a-button type="link" danger size="small" @click="handleRemoveEcAttachmentRow(index)">
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
 * 设变维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcCreate, EcDetailCreate, EcDetail, EcAttachmentCreate, EcAttachment } from '@/types/logistics/manufacturing/engineering-change/ec'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","ecNo","ecIssueDate","changeStatus","ecTitle","ecDetailText","ecLeader","ecLossAmount","ecDistinction","effectiveDate","ecEntryDate","flowInstanceId","ecStatus","extFieldJson","remark"]

/** ecDetail 子表行（表单 Tab 内嵌） */
const childEcDetailRows = ref<Record<string, unknown>[]>([])
/** ecAttachment 子表行（表单 Tab 内嵌） */
const childEcAttachmentRows = ref<Record<string, unknown>[]>([])

/** 子表 ecDetail 表单列定义 */
const ecDetailFormColumns = computed(() => [
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
    title: t('entity.ecDetail.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 140,
  },
  {
    title: t('entity.ecDetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.ecDetail.ecmodel'),
    dataIndex: 'ecModel',
    key: 'ecModel',
    width: 140,
  },
  {
    title: t('entity.ecDetail.ecbomitem'),
    dataIndex: 'ecBomItem',
    key: 'ecBomItem',
    width: 140,
  },
  {
    title: t('entity.ecDetail.ecbomsubitem'),
    dataIndex: 'ecBomSubItem',
    key: 'ecBomSubItem',
    width: 140,
  },
  {
    title: t('entity.ecDetail.ecbomno'),
    dataIndex: 'ecBomNo',
    key: 'ecBomNo',
    width: 140,
  },
  {
    title: t('entity.ecDetail.ecchange'),
    dataIndex: 'ecChange',
    key: 'ecChange',
    width: 140,
  },
  {
    title: t('entity.ecDetail.eclocal'),
    dataIndex: 'ecLocal',
    key: 'ecLocal',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 ecAttachment 表单列定义 */
const ecAttachmentFormColumns = computed(() => [
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
    title: t('entity.ecAttachment.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 140,
  },
  {
    title: t('entity.ecAttachment.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.ecAttachment.attachmenttype'),
    dataIndex: 'attachmentType',
    key: 'attachmentType',
    width: 140,
  },
  {
    title: t('entity.ecAttachment.docno'),
    dataIndex: 'docNo',
    key: 'docNo',
    width: 140,
  },
  {
    title: t('entity.ecAttachment.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 140,
  },
  {
    title: t('entity.ecAttachment.accessurl'),
    dataIndex: 'accessUrl',
    key: 'accessUrl',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
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
function syncChildRowsFromFormData(val: Partial<EcCreate & { ecId?: string }> | null | undefined) {
  childEcDetailRows.value = ((val as any)?.ecDetails ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.ecDetailId ?? `new-${index}`,
  }))
  childEcAttachmentRows.value = ((val as any)?.attachments ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.ecAttachmentId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 ecDetail 行 */
function handleAddEcDetailRow() {
  childEcDetailRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ecNo: '',
      lineNumber: 0,
      ecModel: '',
      ecBomItem: '',
      ecBomSubItem: '',
      ecBomNo: '',
      ecChange: '',
      ecLocal: '',
  })
}

/** 表单 Tab 内删除 ecDetail 行 */
function handleRemoveEcDetailRow(index: number) {
  childEcDetailRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 ecAttachment 行 */
function handleAddEcAttachmentRow() {
  childEcAttachmentRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ecNo: '',
      lineNumber: 0,
      attachmentType: '',
      docNo: '',
      fileName: '',
      accessUrl: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 ecAttachment 行 */
function handleRemoveEcAttachmentRow(index: number) {
  childEcAttachmentRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    ecDetails: childEcDetailRows.value.map(({ __rowKey, ...rest }) => rest),
    attachments: childEcAttachmentRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EcCreate & { ecId?: string }> | null
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
    delete (next as any).ecDetails
    delete (next as any).attachments
    applyScopeDefaults(next)
    Object.assign(formState, next)
    syncChildRowsFromFormData(val)
  },
  { immediate: true, deep: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.ecId
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
  changeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ec.changestatus') }),
      trigger: 'change'
    }
  ],
  ecTitle: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.title') }),
      trigger: 'blur'
    }
  ],
  ecDetailText: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.detailtext') }),
      trigger: 'blur'
    }
  ],
  ecLeader: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.leader') }),
      trigger: 'blur'
    }
  ],
  ecLossAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ec.lossamount') }),
      trigger: 'change'
    }
  ],
  ecDistinction: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.distinction') }),
      trigger: 'blur'
    }
  ],
  effectiveDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ec.effectivedate') }),
      trigger: 'change'
    }
  ],
  ecEntryDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ec.entrydate') }),
      trigger: 'change'
    }
  ],
  flowInstanceId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ec.flowinstanceid') }),
      trigger: 'blur'
    }
  ],
  ecStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ec.status') }),
      trigger: 'change'
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
  childEcDetailRows.value = []
  childEcAttachmentRows.value = []
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
