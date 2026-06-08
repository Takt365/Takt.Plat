<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/manufacturer/components -->
<!-- 文件名称：manufacturer-form.vue -->
<!-- 功能描述：Takt制造商实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="manufacturer-form-tabs"
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
                :label="t('entity.manufacturer.code')"
                name="manufacturerCode"
              >
                <a-input
                  v-model:value="formState.manufacturerCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.name')"
                name="manufacturerName"
              >
                <a-input
                  v-model:value="formState.manufacturerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.shortname')"
                name="manufacturerShortName"
              >
                <a-input
                  v-model:value="formState.manufacturerShortName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.shortname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.type')"
                name="manufacturerType"
              >
                <a-input-number
                  v-model:value="formState.manufacturerType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.industrysector')"
                name="industrySector"
              >
                <a-input
                  v-model:value="formState.industrySector"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.industrysector') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.taxnumber')"
                name="manufacturerTaxNumber"
              >
                <a-input
                  v-model:value="formState.manufacturerTaxNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.taxnumber') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.registrationcountry')"
                name="registrationCountry"
              >
                <a-input
                  v-model:value="formState.registrationCountry"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.registrationcountry') })"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.manufacturer.registrationaddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.manufacturer.registrationaddress1') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.manufacturer.registrationaddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.manufacturer.registrationaddress2') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.manufacturer.registrationaddress3')"
                name="registrationAddress3"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress3"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.manufacturer.registrationaddress3') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.phone')"
                name="manufacturerPhone"
              >
                <a-input
                  v-model:value="formState.manufacturerPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.phone') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.fax')"
                name="manufacturerFax"
              >
                <a-input
                  v-model:value="formState.manufacturerFax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.fax') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.email')"
                name="manufacturerEmail"
              >
                <a-input
                  v-model:value="formState.manufacturerEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.email') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.website')"
                name="manufacturerWebsite"
              >
                <a-input
                  v-model:value="formState.manufacturerWebsite"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.website') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.contactperson')"
                name="contactPerson"
              >
                <a-input
                  v-model:value="formState.contactPerson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.contactperson') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.contactphone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.contactphone') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.contactemail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.contactemail') })"
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
                :label="t('entity.manufacturer.level')"
                name="manufacturerLevel"
              >
                <a-input-number
                  v-model:value="formState.manufacturerLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.level') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.qualitycertification')"
                name="qualityCertification"
              >
                <a-input-number
                  v-model:value="formState.qualityCertification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.qualitycertification') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.evaluationscore')"
                name="evaluationScore"
              >
                <a-input-number
                  v-model:value="formState.evaluationScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.evaluationscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.isqualified')"
                name="isQualified"
              >
                <a-input-number
                  v-model:value="formState.isQualified"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.isqualified') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.status')"
                name="manufacturerStatus"
              >
                <TaktSelect
                  v-model:value="formState.manufacturerStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.manufacturer.status') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.manufacturer.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.sortorder') })"
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
      <!-- 子表：manufacturerMaterial -->
      <a-tab-pane
        key="child-manufacturerMaterials"
        :tab="t('entity.manufacturerMaterial._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddManufacturerMaterialRow">
            {{ t('common.page.button.create') }}{{ t('entity.manufacturerMaterial._self') }}
          </a-button>
        </div>
        <a-table
          :columns="manufacturerMaterialFormColumns"
          :data-source="childManufacturerMaterialRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturerMaterial.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'materialType'">
              <a-input-number
                v-model:value="record.materialType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturerMaterial.materialtype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'manufacturerMaterialCode'">
              <a-input
                v-model:value="record.manufacturerMaterialCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturerMaterial.code') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'manufacturerMaterialName'">
              <a-input
                v-model:value="record.manufacturerMaterialName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturerMaterial.name') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'manufacturerMaterialSpecification'">
              <a-input
                v-model:value="record.manufacturerMaterialSpecification"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturerMaterial.specification') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialCode'">
              <a-input
                v-model:value="record.materialCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturerMaterial.materialcode') })"
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
              <a-button type="link" danger size="small" @click="handleRemoveManufacturerMaterialRow(index)">
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
 * Takt制造商实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/manufacturer/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ManufacturerCreate, ManufacturerMaterialCreate, ManufacturerMaterial } from '@/types/logistics/materials/manufacturer'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","manufacturerCode","manufacturerName","manufacturerShortName","manufacturerType","industrySector","manufacturerTaxNumber","registrationCountry","registrationAddress1","registrationAddress2","registrationAddress3","manufacturerPhone","manufacturerFax","manufacturerEmail","manufacturerWebsite","contactPerson","contactPhone","contactEmail","manufacturerLevel","qualityCertification","evaluationScore","isQualified","manufacturerStatus","sortOrder","extFieldJson","remark"]

/** manufacturerMaterial 子表行（表单 Tab 内嵌） */
const childManufacturerMaterialRows = ref<Record<string, unknown>[]>([])

/** 子表 manufacturerMaterial 表单列定义 */
const manufacturerMaterialFormColumns = computed(() => [
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
    title: t('entity.manufacturerMaterial.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.manufacturerMaterial.materialtype'),
    dataIndex: 'materialType',
    key: 'materialType',
    width: 140,
  },
  {
    title: t('entity.manufacturerMaterial.code'),
    dataIndex: 'manufacturerMaterialCode',
    key: 'manufacturerMaterialCode',
    width: 140,
  },
  {
    title: t('entity.manufacturerMaterial.name'),
    dataIndex: 'manufacturerMaterialName',
    key: 'manufacturerMaterialName',
    width: 140,
  },
  {
    title: t('entity.manufacturerMaterial.specification'),
    dataIndex: 'manufacturerMaterialSpecification',
    key: 'manufacturerMaterialSpecification',
    width: 140,
  },
  {
    title: t('entity.manufacturerMaterial.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
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
function syncChildRowsFromFormData(val: Partial<ManufacturerCreate & { manufacturerId?: string }> | null | undefined) {
  childManufacturerMaterialRows.value = ((val as any)?.manufacturerMaterials ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.manufacturerMaterialId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 manufacturerMaterial 行 */
function handleAddManufacturerMaterialRow() {
  childManufacturerMaterialRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      materialType: 0,
      manufacturerMaterialCode: '',
      manufacturerMaterialName: '',
      manufacturerMaterialSpecification: '',
      materialCode: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 manufacturerMaterial 行 */
function handleRemoveManufacturerMaterialRow(index: number) {
  childManufacturerMaterialRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    manufacturerMaterials: childManufacturerMaterialRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ManufacturerCreate & { manufacturerId?: string }> | null
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
    delete (next as any).manufacturerMaterials
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
    const isCreate = !props.formData?.manufacturerId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  manufacturerCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.manufacturer.code') }),
      trigger: 'blur'
    }
  ],
  manufacturerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.manufacturer.name') }),
      trigger: 'blur'
    }
  ],
  manufacturerType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.manufacturer.type') }),
      trigger: 'change'
    }
  ],
  manufacturerLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.manufacturer.level') }),
      trigger: 'change'
    }
  ],
  qualityCertification: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.manufacturer.qualitycertification') }),
      trigger: 'change'
    }
  ],
  evaluationScore: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.manufacturer.evaluationscore') }),
      trigger: 'change'
    }
  ],
  isQualified: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.manufacturer.isqualified') }),
      trigger: 'change'
    }
  ],
  manufacturerStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.manufacturer.status') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.manufacturer.sortorder') }),
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
  childManufacturerMaterialRows.value = []
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
