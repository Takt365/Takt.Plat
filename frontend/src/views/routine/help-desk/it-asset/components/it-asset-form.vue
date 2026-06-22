<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/it-asset/components -->
<!-- 文件名称：it-asset-form.vue -->
<!-- 功能描述：服务台 IT 设备保修扩展实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="it-asset-form-tabs"
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
                :label="t('entity.itAsset.assetcode')"
                name="assetCode"
              >
                <a-input
                  v-model:value="formState.assetCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.assetcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.warrantytype')"
                name="warrantyType"
              >
                <a-input-number
                  v-model:value="formState.warrantyType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.warrantytype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.warrantystartdate')"
                name="warrantyStartDate"
              >
                <a-date-picker
                  v-model:value="formState.warrantyStartDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.warrantystartdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.warrantyexpirydate')"
                name="warrantyExpiryDate"
              >
                <a-date-picker
                  v-model:value="formState.warrantyExpiryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.warrantyexpirydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.warrantyprovider')"
                name="warrantyProvider"
              >
                <a-input
                  v-model:value="formState.warrantyProvider"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.warrantyprovider') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.warrantycontractno')"
                name="warrantyContractNo"
              >
                <a-input
                  v-model:value="formState.warrantyContractNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.warrantycontractno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.servicehotline')"
                name="serviceHotline"
              >
                <a-input
                  v-model:value="formState.serviceHotline"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.servicehotline') })"
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
                :label="t('entity.itAsset.serviceemail')"
                name="serviceEmail"
              >
                <a-input
                  v-model:value="formState.serviceEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.serviceemail') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.maintenanceexpirydate')"
                name="maintenanceExpiryDate"
              >
                <a-date-picker
                  v-model:value="formState.maintenanceExpiryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.maintenanceexpirydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.lastmaintenancedate')"
                name="lastMaintenanceDate"
              >
                <a-date-picker
                  v-model:value="formState.lastMaintenanceDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.lastmaintenancedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.itAsset.nextmaintenancedate')"
                name="nextMaintenanceDate"
              >
                <a-date-picker
                  v-model:value="formState.nextMaintenanceDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.nextmaintenancedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.itAsset.warrantyremark')"
                name="warrantyRemark"
              >
                <a-textarea
                  v-model:value="formState.warrantyRemark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.itAsset.warrantyremark') })"
                  :rows="2"
                  size="small"
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
      <!-- 子表：itAssetChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.itAssetChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddItAssetChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.itAssetChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="itAssetChangeLogFormColumns"
          :data-source="childItAssetChangeLogRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">

            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveItAssetChangeLogRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：ticket -->
      <a-tab-pane
        key="child-tickets"
        :tab="t('entity.ticket._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddTicketRow">
            {{ t('common.page.button.create') }}{{ t('entity.ticket._self') }}
          </a-button>
        </div>
        <a-table
          :columns="ticketFormColumns"
          :data-source="childTicketRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.no') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'title'">
              <a-input
                v-model:value="record.title"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.title') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'content'">
              <a-textarea
                v-model:value="record.content"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ticket.content') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'attachmentsJson'">
              <a-input
                v-model:value="record.attachmentsJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.attachmentsjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'ticketStatus'">
              <a-input-number
                v-model:value="record.ticketStatus"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.status') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'priority'">
              <a-input-number
                v-model:value="record.priority"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.priority') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'categoryCode'">
              <a-input
                v-model:value="record.categoryCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.categorycode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'ticketSource'">
              <a-input-number
                v-model:value="record.ticketSource"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.source') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveTicketRow(index)">
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
 * 服务台 IT 设备保修扩展实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/it-asset/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ItAssetCreate, ItAssetChangeLogCreate, ItAssetChangeLog, TicketCreate, Ticket } from '@/types/routine/help-desk/it-asset'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","assetCode","warrantyType","warrantyStartDate","warrantyExpiryDate","warrantyProvider","warrantyContractNo","serviceHotline","serviceEmail","maintenanceExpiryDate","lastMaintenanceDate","nextMaintenanceDate","warrantyRemark","ExtField","remark"]

/** itAssetChangeLog 子表行（表单 Tab 内嵌） */
const childItAssetChangeLogRows = ref<Record<string, unknown>[]>([])
/** ticket 子表行（表单 Tab 内嵌） */
const childTicketRows = ref<Record<string, unknown>[]>([])

/** 子表 itAssetChangeLog 表单列定义 */
const itAssetChangeLogFormColumns = computed(() => [

  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 ticket 表单列定义 */
const ticketFormColumns = computed(() => [
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
    title: t('entity.ticket.no'),
    dataIndex: 'ticketNo',
    key: 'ticketNo',
    width: 140,
  },
  {
    title: t('entity.ticket.title'),
    dataIndex: 'title',
    key: 'title',
    width: 140,
  },
  {
    title: t('entity.ticket.content'),
    dataIndex: 'content',
    key: 'content',
    width: 140,
  },
  {
    title: t('entity.ticket.attachmentsjson'),
    dataIndex: 'attachmentsJson',
    key: 'attachmentsJson',
    width: 140,
  },
  {
    title: t('entity.ticket.status'),
    dataIndex: 'ticketStatus',
    key: 'ticketStatus',
    width: 140,
  },
  {
    title: t('entity.ticket.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 140,
  },
  {
    title: t('entity.ticket.categorycode'),
    dataIndex: 'categoryCode',
    key: 'categoryCode',
    width: 140,
  },
  {
    title: t('entity.ticket.source'),
    dataIndex: 'ticketSource',
    key: 'ticketSource',
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
function syncChildRowsFromFormData(val: Partial<ItAssetCreate & { itAssetId?: string }> | null | undefined) {
  childItAssetChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.itAssetChangeLogId ?? `new-${index}`,
  }))
  childTicketRows.value = ((val as any)?.tickets ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.ticketId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 itAssetChangeLog 行 */
function handleAddItAssetChangeLogRow() {
  childItAssetChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,

  })
}

/** 表单 Tab 内删除 itAssetChangeLog 行 */
function handleRemoveItAssetChangeLogRow(index: number) {
  childItAssetChangeLogRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 ticket 行 */
function handleAddTicketRow() {
  childTicketRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ticketNo: '',
      title: '',
      content: '',
      attachmentsJson: '',
      ticketStatus: 0,
      priority: 0,
      categoryCode: '',
      ticketSource: 0,
  })
}

/** 表单 Tab 内删除 ticket 行 */
function handleRemoveTicketRow(index: number) {
  childTicketRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    changeLogs: childItAssetChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
    tickets: childTicketRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ItAssetCreate & { itAssetId?: string }> | null
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
    delete (next as any).tickets
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
    const isCreate = !props.formData?.itAssetId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  assetCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.itAsset.assetcode') }),
      trigger: 'blur'
    }
  ],
  warrantyType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.itAsset.warrantytype') }),
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
  childItAssetChangeLogRows.value = []
  childTicketRows.value = []
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
