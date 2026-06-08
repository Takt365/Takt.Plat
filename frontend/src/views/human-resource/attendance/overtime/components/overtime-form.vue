<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/attendance/overtime/components -->
<!-- 文件名称：overtime-form.vue -->
<!-- 功能描述：加班申请维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="overtime-form-tabs"
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
                :label="t('entity.overtime.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.deptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.date')"
                name="overtimeDate"
              >
                <a-date-picker
                  v-model:value="formState.overtimeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.date') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.plannedstarttime')"
                name="plannedStartTime"
              >
                <a-input
                  v-model:value="formState.plannedStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.plannedstarttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.plannedendtime')"
                name="plannedEndTime"
              >
                <a-input
                  v-model:value="formState.plannedEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.plannedendtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.totalemployees')"
                name="totalEmployees"
              >
                <a-input-number
                  v-model:value="formState.totalEmployees"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalemployees') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.totalplannedhours')"
                name="totalPlannedHours"
              >
                <a-input-number
                  v-model:value="formState.totalPlannedHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalplannedhours') })"
                  size="small"
                  style="width: 100%"
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
                :label="t('entity.overtime.totalactualhours')"
                name="totalActualHours"
              >
                <a-input-number
                  v-model:value="formState.totalActualHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalactualhours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.type')"
                name="overtimeType"
              >
                <TaktSelect
                  v-model:value="formState.overtimeType"
                  dict-type="hr_overtime_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.type') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.reason')"
                name="reason"
              >
                <a-input
                  v-model:value="formState.reason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.reason') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.handlingby')"
                name="handlingBy"
              >
                <a-input
                  v-model:value="formState.handlingBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.handlingat')"
                name="handlingAt"
              >
                <a-input
                  v-model:value="formState.handlingAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.handlingcomment')"
                name="handlingComment"
              >
                <a-input
                  v-model:value="formState.handlingComment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingcomment') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.status')"
                name="overtimeStatus"
              >
                <TaktSelect
                  v-model:value="formState.overtimeStatus"
                  dict-type="hr_overtime_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.status') })"
                  size="small"
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
      <!-- 子表：overtimeItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.overtimeItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddOvertimeItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.overtimeItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="overtimeItemFormColumns"
          :data-source="childOvertimeItemRows"
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
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'employeeId'">
              <a-input
                v-model:value="record.employeeId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeItem.employeeid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'employeeName'">
              <a-input
                v-model:value="record.employeeName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeItem.employeename') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'plannedHours'">
              <a-input-number
                v-model:value="record.plannedHours"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeItem.plannedhours') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'actualStartTime'">
              <a-input
                v-model:value="record.actualStartTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeItem.actualstarttime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'actualEndTime'">
              <a-input
                v-model:value="record.actualEndTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeItem.actualendtime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'actualHours'">
              <a-input-number
                v-model:value="record.actualHours"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeItem.actualhours') })"
                size="small"
                style="width: 100%"
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
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveOvertimeItemRow(index)">
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
 * 加班申请维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/attendance/overtime/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { OvertimeCreate, OvertimeItemCreate, OvertimeItem } from '@/types/human-resource/attendance/overtime'
import TaktSelect from '@/components/business/takt-select/index.vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","deptId","deptName","overtimeDate","plannedStartTime","plannedEndTime","totalEmployees","totalPlannedHours","totalActualHours","overtimeType","reason","relatedPlant","flowInstanceId","handlingBy","handlingAt","handlingComment","overtimeStatus","extFieldJson","remark"]

/** overtimeItem 子表行（表单 Tab 内嵌） */
const childOvertimeItemRows = ref<Record<string, unknown>[]>([])

/** 子表 overtimeItem 表单列定义 */
const overtimeItemFormColumns = computed(() => [
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
    title: t('entity.overtimeItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.overtimeItem.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 140,
  },
  {
    title: t('entity.overtimeItem.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 140,
  },
  {
    title: t('entity.overtimeItem.plannedhours'),
    dataIndex: 'plannedHours',
    key: 'plannedHours',
    width: 140,
  },
  {
    title: t('entity.overtimeItem.actualstarttime'),
    dataIndex: 'actualStartTime',
    key: 'actualStartTime',
    width: 140,
  },
  {
    title: t('entity.overtimeItem.actualendtime'),
    dataIndex: 'actualEndTime',
    key: 'actualEndTime',
    width: 140,
  },
  {
    title: t('entity.overtimeItem.actualhours'),
    dataIndex: 'actualHours',
    key: 'actualHours',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
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
function syncChildRowsFromFormData(val: Partial<OvertimeCreate & { overtimeId?: string }> | null | undefined) {
  childOvertimeItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.overtimeItemId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 overtimeItem 行 */
function handleAddOvertimeItemRow() {
  childOvertimeItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      employeeId: '',
      employeeName: '',
      plannedHours: 0,
      actualStartTime: '',
      actualEndTime: '',
      actualHours: 0,
      extFieldJson: '',
  })
}

/** 表单 Tab 内删除 overtimeItem 行 */
function handleRemoveOvertimeItemRow(index: number) {
  childOvertimeItemRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childOvertimeItemRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<OvertimeCreate & { overtimeId?: string }> | null
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
    delete (next as any).items
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
    const isCreate = !props.formData?.overtimeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  deptId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtime.deptid') }),
      trigger: 'blur'
    }
  ],
  overtimeDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.overtime.date') }),
      trigger: 'change'
    }
  ],
  plannedStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtime.plannedstarttime') }),
      trigger: 'blur'
    }
  ],
  plannedEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtime.plannedendtime') }),
      trigger: 'blur'
    }
  ],
  totalEmployees: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.overtime.totalemployees') }),
      trigger: 'change'
    }
  ],
  totalPlannedHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.overtime.totalplannedhours') }),
      trigger: 'change'
    }
  ],
  totalActualHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.overtime.totalactualhours') }),
      trigger: 'change'
    }
  ],
  overtimeType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.overtime.type') }),
      trigger: 'change'
    }
  ],
  handlingBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingby') }),
      trigger: 'blur'
    }
  ],
  overtimeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.overtime.status') }),
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
  childOvertimeItemRows.value = []
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
