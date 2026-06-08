<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output/components -->
<!-- 文件名称：assy-output-form.vue -->
<!-- 功能描述：组立日报维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="assy-output-form-tabs"
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
                :label="t('entity.assyOutput.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.prodcategory')"
                name="prodCategory"
              >
                <a-input
                  v-model:value="formState.prodCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodcategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.proddate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assyOutput.proddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.prodline')"
                name="prodLine"
              >
                <a-input
                  v-model:value="formState.prodLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.directlabor')"
                name="directLabor"
              >
                <a-input-number
                  v-model:value="formState.directLabor"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.directlabor') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.indirectlabor')"
                name="indirectLabor"
              >
                <a-input-number
                  v-model:value="formState.indirectLabor"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.indirectlabor') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.shiftno') })"
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
                :label="t('entity.assyOutput.prodordertype')"
                name="prodOrderType"
              >
                <a-input
                  v-model:value="formState.prodOrderType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodordertype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.modelcode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.modelcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.batchno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.prodorderqty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodorderqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.stdminutes')"
                name="stdMinutes"
              >
                <a-input-number
                  v-model:value="formState.stdMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.stdminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.stdcapacity')"
                name="stdCapacity"
              >
                <a-input-number
                  v-model:value="formState.stdCapacity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.stdcapacity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutput.status')"
                name="status"
              >
                <a-input-number
                  v-model:value="formState.status"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.status') })"
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
      <!-- 子表：assyOutputDetail -->
      <a-tab-pane
        key="child-assyOutputDetails"
        :tab="t('entity.assyOutputDetail._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddAssyOutputDetailRow">
            {{ t('common.page.button.create') }}{{ t('entity.assyOutputDetail._self') }}
          </a-button>
        </div>
        <a-table
          :columns="assyOutputDetailFormColumns"
          :data-source="childAssyOutputDetailRows"
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
            <template v-else-if="column.key === 'prodOrderCode'">
              <a-input
                v-model:value="record.prodOrderCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.prodordercode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'timePeriod'">
              <a-input
                v-model:value="record.timePeriod"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.timeperiod') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'prodActualQty'">
              <a-input-number
                v-model:value="record.prodActualQty"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.prodactualqty') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'downtimeMinutes'">
              <a-input-number
                v-model:value="record.downtimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.downtimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'downtimeReason'">
              <a-input
                v-model:value="record.downtimeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.downtimereason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'downtimeDescription'">
              <a-textarea
                v-model:value="record.downtimeDescription"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assyOutputDetail.downtimedescription') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'unachievedReason'">
              <a-input
                v-model:value="record.unachievedReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.unachievedreason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveAssyOutputDetailRow(index)">
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
 * 组立日报维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/assy-output/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AssyOutputCreate, AssyOutputDetailCreate, AssyOutputDetail } from '@/types/logistics/manufacturing/output/assy-output'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodCategory","prodDate","prodLine","directLabor","indirectLabor","shiftNo","prodOrderType","prodOrderCode","modelCode","materialCode","batchNo","prodOrderQty","stdMinutes","stdCapacity","status","extFieldJson","remark"]

/** assyOutputDetail 子表行（表单 Tab 内嵌） */
const childAssyOutputDetailRows = ref<Record<string, unknown>[]>([])

/** 子表 assyOutputDetail 表单列定义 */
const assyOutputDetailFormColumns = computed(() => [
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
    title: t('entity.assyOutputDetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 140,
  },
  {
    title: t('entity.assyOutputDetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.assyOutputDetail.timeperiod'),
    dataIndex: 'timePeriod',
    key: 'timePeriod',
    width: 140,
  },
  {
    title: t('entity.assyOutputDetail.prodactualqty'),
    dataIndex: 'prodActualQty',
    key: 'prodActualQty',
    width: 140,
  },
  {
    title: t('entity.assyOutputDetail.downtimeminutes'),
    dataIndex: 'downtimeMinutes',
    key: 'downtimeMinutes',
    width: 140,
  },
  {
    title: t('entity.assyOutputDetail.downtimereason'),
    dataIndex: 'downtimeReason',
    key: 'downtimeReason',
    width: 140,
  },
  {
    title: t('entity.assyOutputDetail.downtimedescription'),
    dataIndex: 'downtimeDescription',
    key: 'downtimeDescription',
    width: 140,
  },
  {
    title: t('entity.assyOutputDetail.unachievedreason'),
    dataIndex: 'unachievedReason',
    key: 'unachievedReason',
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
function syncChildRowsFromFormData(val: Partial<AssyOutputCreate & { assyOutputId?: string }> | null | undefined) {
  childAssyOutputDetailRows.value = ((val as any)?.assyOutputDetails ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.assyOutputDetailId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 assyOutputDetail 行 */
function handleAddAssyOutputDetailRow() {
  childAssyOutputDetailRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      prodOrderCode: '',
      lineNumber: 0,
      timePeriod: '',
      prodActualQty: 0,
      downtimeMinutes: 0,
      downtimeReason: '',
      downtimeDescription: '',
      unachievedReason: '',
  })
}

/** 表单 Tab 内删除 assyOutputDetail 行 */
function handleRemoveAssyOutputDetailRow(index: number) {
  childAssyOutputDetailRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    assyOutputDetails: childAssyOutputDetailRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssyOutputCreate & { assyOutputId?: string }> | null
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
    delete (next as any).assyOutputDetails
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
    const isCreate = !props.formData?.assyOutputId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutput.plantcode') }),
      trigger: 'blur'
    }
  ],
  prodCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodcategory') }),
      trigger: 'blur'
    }
  ],
  prodDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutput.proddate') }),
      trigger: 'change'
    }
  ],
  prodLine: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodline') }),
      trigger: 'blur'
    }
  ],
  directLabor: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutput.directlabor') }),
      trigger: 'change'
    }
  ],
  indirectLabor: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutput.indirectlabor') }),
      trigger: 'change'
    }
  ],
  shiftNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutput.shiftno') }),
      trigger: 'change'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodordercode') }),
      trigger: 'blur'
    }
  ],
  modelCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutput.modelcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutput.materialcode') }),
      trigger: 'blur'
    }
  ],
  prodOrderQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutput.prodorderqty') }),
      trigger: 'change'
    }
  ],
  stdMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutput.stdminutes') }),
      trigger: 'change'
    }
  ],
  stdCapacity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutput.stdcapacity') }),
      trigger: 'change'
    }
  ],
  status: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutput.status') }),
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
  childAssyOutputDetailRows.value = []
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
