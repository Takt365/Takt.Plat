<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：pcba-output-form.vue -->
<!-- 功能描述：PCBA日报实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="pcba-output-form-tabs"
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
                :label="t('entity.pcbaOutput.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.prodcategory')"
                name="prodCategory"
              >
                <a-input
                  v-model:value="formState.prodCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodcategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.proddate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.proddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.prodline')"
                name="prodLine"
              >
                <a-input
                  v-model:value="formState.prodLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.shiftno') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.modelcode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.modelcode') })"
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
                :label="t('entity.pcbaOutput.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.batchno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.prodorderqty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodorderqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.stdminutes')"
                name="stdMinutes"
              >
                <a-input-number
                  v-model:value="formState.stdMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.stdminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.stdshorts')"
                name="stdShorts"
              >
                <a-input-number
                  v-model:value="formState.stdShorts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.stdshorts') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutput.stdcapacity')"
                name="stdCapacity"
              >
                <a-input-number
                  v-model:value="formState.stdCapacity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.stdcapacity') })"
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
      <!-- 子表：pcbaOutputDetail -->
      <a-tab-pane
        key="child-pcbaOutputDetails"
        :tab="t('entity.pcbaOutputDetail._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddPcbaOutputDetailRow">
            {{ t('common.page.button.create') }}{{ t('entity.pcbaOutputDetail._self') }}
          </a-button>
        </div>
        <a-table
          :columns="pcbaOutputDetailFormColumns"
          :data-source="childPcbaOutputDetailRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.prodordercode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'timePeriod'">
              <a-input
                v-model:value="record.timePeriod"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.timeperiod') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'shiftNo'">
              <a-input-number
                v-model:value="record.shiftNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.shiftno') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'pcbBoardType'">
              <a-input
                v-model:value="record.pcbBoardType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.pcbboardtype') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'panelSide'">
              <a-input
                v-model:value="record.panelSide"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.panelside') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'batchQty'">
              <a-input-number
                v-model:value="record.batchQty"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.batchqty') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'dailyCompletedQty'">
              <a-input-number
                v-model:value="record.dailyCompletedQty"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.dailycompletedqty') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemovePcbaOutputDetailRow(index)">
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
 * PCBA日报实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/pcba-output/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PcbaOutputCreate, PcbaOutputDetailCreate, PcbaOutputDetail } from '@/types/logistics/manufacturing/output/pcba-output'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodCategory","prodDate","prodLine","shiftNo","prodOrderCode","modelCode","batchNo","materialCode","prodOrderQty","stdMinutes","stdShorts","stdCapacity","extFieldJson","remark"]

/** pcbaOutputDetail 子表行（表单 Tab 内嵌） */
const childPcbaOutputDetailRows = ref<Record<string, unknown>[]>([])

/** 子表 pcbaOutputDetail 表单列定义 */
const pcbaOutputDetailFormColumns = computed(() => [
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
    title: t('entity.pcbaOutputDetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 140,
  },
  {
    title: t('entity.pcbaOutputDetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.pcbaOutputDetail.timeperiod'),
    dataIndex: 'timePeriod',
    key: 'timePeriod',
    width: 140,
  },
  {
    title: t('entity.pcbaOutputDetail.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 140,
  },
  {
    title: t('entity.pcbaOutputDetail.pcbboardtype'),
    dataIndex: 'pcbBoardType',
    key: 'pcbBoardType',
    width: 140,
  },
  {
    title: t('entity.pcbaOutputDetail.panelside'),
    dataIndex: 'panelSide',
    key: 'panelSide',
    width: 140,
  },
  {
    title: t('entity.pcbaOutputDetail.batchqty'),
    dataIndex: 'batchQty',
    key: 'batchQty',
    width: 140,
  },
  {
    title: t('entity.pcbaOutputDetail.dailycompletedqty'),
    dataIndex: 'dailyCompletedQty',
    key: 'dailyCompletedQty',
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
function syncChildRowsFromFormData(val: Partial<PcbaOutputCreate & { pcbaOutputId?: string }> | null | undefined) {
  childPcbaOutputDetailRows.value = ((val as any)?.pcbaOutputDetails ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.pcbaOutputDetailId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 pcbaOutputDetail 行 */
function handleAddPcbaOutputDetailRow() {
  childPcbaOutputDetailRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      prodOrderCode: '',
      lineNumber: 0,
      timePeriod: '',
      shiftNo: 0,
      pcbBoardType: '',
      panelSide: '',
      batchQty: 0,
      dailyCompletedQty: 0,
  })
}

/** 表单 Tab 内删除 pcbaOutputDetail 行 */
function handleRemovePcbaOutputDetailRow(index: number) {
  childPcbaOutputDetailRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    pcbaOutputDetails: childPcbaOutputDetailRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaOutputCreate & { pcbaOutputId?: string }> | null
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
    delete (next as any).pcbaOutputDetails
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
    const isCreate = !props.formData?.pcbaOutputId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.plantcode') }),
      trigger: 'blur'
    }
  ],
  prodCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodcategory') }),
      trigger: 'blur'
    }
  ],
  prodDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.proddate') }),
      trigger: 'change'
    }
  ],
  prodLine: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodline') }),
      trigger: 'blur'
    }
  ],
  shiftNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.shiftno') }),
      trigger: 'change'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodordercode') }),
      trigger: 'blur'
    }
  ],
  modelCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.modelcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.materialcode') }),
      trigger: 'blur'
    }
  ],
  prodOrderQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.prodorderqty') }),
      trigger: 'change'
    }
  ],
  stdMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.stdminutes') }),
      trigger: 'change'
    }
  ],
  stdShorts: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.stdshorts') }),
      trigger: 'change'
    }
  ],
  stdCapacity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.stdcapacity') }),
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
  childPcbaOutputDetailRows.value = []
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
