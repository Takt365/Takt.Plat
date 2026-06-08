<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/inspection-standard/components -->
<!-- 文件名称：inspection-standard-form.vue -->
<!-- 功能描述：检验标准实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="inspection-standard-form-tabs"
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
                :label="t('entity.inspectionStandard.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandard.standardcode')"
                name="standardCode"
              >
                <a-input
                  v-model:value="formState.standardCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.standardcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandard.standardname')"
                name="standardName"
              >
                <a-input
                  v-model:value="formState.standardName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.standardname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandard.inspectiontype')"
                name="inspectionType"
              >
                <a-input-number
                  v-model:value="formState.inspectionType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.inspectiontype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandard.materialcategorycode')"
                name="materialCategoryCode"
              >
                <a-input
                  v-model:value="formState.materialCategoryCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.materialcategorycode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandard.materialcategoryname')"
                name="materialCategoryName"
              >
                <a-input
                  v-model:value="formState.materialCategoryName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.materialcategoryname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandard.samplingschemecode')"
                name="samplingSchemeCode"
              >
                <a-input
                  v-model:value="formState.samplingSchemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.samplingschemecode') })"
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
                :label="t('entity.inspectionStandard.samplingschemename')"
                name="samplingSchemeName"
              >
                <a-input
                  v-model:value="formState.samplingSchemeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.samplingschemename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandard.isenabled')"
                name="isEnabled"
              >
                <a-input-number
                  v-model:value="formState.isEnabled"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.isenabled') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandard.standardstatus')"
                name="standardStatus"
              >
                <a-input-number
                  v-model:value="formState.standardStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.standardstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.inspectionStandard.standarddescription')"
                name="standardDescription"
              >
                <a-textarea
                  v-model:value="formState.standardDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.inspectionStandard.standarddescription') })"
                  :rows="2"
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
      <!-- 子表：inspectionStandardItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.inspectionStandardItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddInspectionStandardItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.inspectionStandardItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="inspectionStandardItemFormColumns"
          :data-source="childInspectionStandardItemRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'itemCode'">
              <a-input
                v-model:value="record.itemCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.itemcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'itemName'">
              <a-input
                v-model:value="record.itemName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.itemname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'itemType'">
              <a-input-number
                v-model:value="record.itemType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.itemtype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'defectLevel'">
              <a-input
                v-model:value="record.defectLevel"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.defectlevel') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'inspectionMode'">
              <a-input-number
                v-model:value="record.inspectionMode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.inspectionmode') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'standardValue'">
              <a-input
                v-model:value="record.standardValue"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.standardvalue') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'upperLimit'">
              <a-input
                v-model:value="record.upperLimit"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.upperlimit') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveInspectionStandardItemRow(index)">
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
 * 检验标准实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/inspection-standard/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { InspectionStandardCreate, InspectionStandardItemCreate, InspectionStandardItem } from '@/types/logistics/quality/operation/inspection-standard'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","standardCode","standardName","inspectionType","materialCategoryCode","materialCategoryName","samplingSchemeCode","samplingSchemeName","isEnabled","standardStatus","standardDescription","extFieldJson","remark"]

/** inspectionStandardItem 子表行（表单 Tab 内嵌） */
const childInspectionStandardItemRows = ref<Record<string, unknown>[]>([])

/** 子表 inspectionStandardItem 表单列定义 */
const inspectionStandardItemFormColumns = computed(() => [
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
    title: t('entity.inspectionStandardItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.inspectionStandardItem.itemcode'),
    dataIndex: 'itemCode',
    key: 'itemCode',
    width: 140,
  },
  {
    title: t('entity.inspectionStandardItem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 140,
  },
  {
    title: t('entity.inspectionStandardItem.itemtype'),
    dataIndex: 'itemType',
    key: 'itemType',
    width: 140,
  },
  {
    title: t('entity.inspectionStandardItem.defectlevel'),
    dataIndex: 'defectLevel',
    key: 'defectLevel',
    width: 140,
  },
  {
    title: t('entity.inspectionStandardItem.inspectionmode'),
    dataIndex: 'inspectionMode',
    key: 'inspectionMode',
    width: 140,
  },
  {
    title: t('entity.inspectionStandardItem.standardvalue'),
    dataIndex: 'standardValue',
    key: 'standardValue',
    width: 140,
  },
  {
    title: t('entity.inspectionStandardItem.upperlimit'),
    dataIndex: 'upperLimit',
    key: 'upperLimit',
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
function syncChildRowsFromFormData(val: Partial<InspectionStandardCreate & { inspectionStandardId?: string }> | null | undefined) {
  childInspectionStandardItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.inspectionStandardItemId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 inspectionStandardItem 行 */
function handleAddInspectionStandardItemRow() {
  childInspectionStandardItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      itemCode: '',
      itemName: '',
      itemType: 0,
      defectLevel: '',
      inspectionMode: 0,
      standardValue: '',
      upperLimit: '',
  })
}

/** 表单 Tab 内删除 inspectionStandardItem 行 */
function handleRemoveInspectionStandardItemRow(index: number) {
  childInspectionStandardItemRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childInspectionStandardItemRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<InspectionStandardCreate & { inspectionStandardId?: string }> | null
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
    const isCreate = !props.formData?.inspectionStandardId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  standardCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.standardcode') }),
      trigger: 'blur'
    }
  ],
  standardName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.standardname') }),
      trigger: 'blur'
    }
  ],
  inspectionType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.inspectionStandard.inspectiontype') }),
      trigger: 'change'
    }
  ],
  materialCategoryCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.materialcategorycode') }),
      trigger: 'blur'
    }
  ],
  materialCategoryName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.materialcategoryname') }),
      trigger: 'blur'
    }
  ],
  isEnabled: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.inspectionStandard.isenabled') }),
      trigger: 'change'
    }
  ],
  standardStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.inspectionStandard.standardstatus') }),
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
  childInspectionStandardItemRows.value = []
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
