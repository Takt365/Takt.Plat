<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mps/production-team/components -->
<!-- 文件名称：production-team-form.vue -->
<!-- 功能描述：生产班组实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form production-team-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="production-team-form-tabs"
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
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
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
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    <!-- 下：子表 teamEquipmentList -->
    <TaktEditableTable
      ref="productionTeamEquipmentTableRef"
      v-model="childProductionTeamEquipmentRows"
      :columns="productionTeamEquipmentFormColumns"
      :title="productionTeamEquipmentPi.self()"
      :add-button-entity="productionTeamEquipmentPi.self()"
      id-field="productionTeamEquipmentId"
      :default-row="createDefaultProductionTeamEquipmentRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-plantCode="{ record }">
        <TaktSelect
          v-model:value="record.plantCode"
          api-url="TaktPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="productionTeamEquipmentPi.queryPh('plantCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-teamEquipStatus="{ record }">
        <TaktSelect
          v-model:value="record.teamEquipStatus"
          dict-type="sys_normal_disable"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="productionTeamEquipmentPi.ph('teamEquipStatus')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="productionTeamEquipmentPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 生产班组实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/mps/production-team/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useProductionTeamI18n } from '../composables/use-production-team-i18n'

/** 实体字段 i18n */
const pi = useProductionTeamI18n()

import type { ProductionTeamCreate } from '@/types/logistics/manufacturing/mps/production-team'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
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
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","teamCode","teamName","teamCategory","teamLeaderName","shiftNo","teamStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useProductionTeamEquipmentI18n } from '../composables/use-production-team-equipment-i18n'

const productionTeamEquipmentPi = useProductionTeamEquipmentI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childProductionTeamEquipmentRows = ref<Record<string, unknown>[]>([])
const productionTeamEquipmentTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedProductionTeamEquipmentRow(row: Record<string, unknown>): boolean {
  const id = row.productionTeamEquipmentId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextProductionTeamEquipmentLineNumber(): number {
  const rows = productionTeamEquipmentTableRef.value?.getRows?.() ?? childProductionTeamEquipmentRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 productionTeamEquipment 可编辑列 */
const productionTeamEquipmentFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'plantCode',
    title: productionTeamEquipmentPi.label('plantCode'),
    width: 140,
  },
  {
    key: 'teamCode',
    title: productionTeamEquipmentPi.label('teamCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: productionTeamEquipmentPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'prodEquipId',
    title: productionTeamEquipmentPi.label('prodEquipId'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'prodEquipCode',
    title: productionTeamEquipmentPi.label('prodEquipCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'equipQuantity',
    title: productionTeamEquipmentPi.label('equipQuantity'),
    width: 140,
  },
  {
    key: 'teamEquipStatus',
    title: productionTeamEquipmentPi.label('teamEquipStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: productionTeamEquipmentPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ProductionTeamCreate & { productionTeamId?: string }> | null | undefined) {
  const rows_productionTeamEquipment = ((val as any)?.teamEquipmentList ?? []) as Record<string, unknown>[]
  childProductionTeamEquipmentRows.value = rows_productionTeamEquipment
}

function createDefaultProductionTeamEquipmentRow(): Record<string, unknown> {
  return {
    plantCode: '',
    teamCode: '',
    lineNumber: allocateNextProductionTeamEquipmentLineNumber(),
    prodEquipId: '',
    prodEquipCode: '',
    equipQuantity: 0,
    teamEquipStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.productionTeamId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    teamEquipmentList: productionTeamEquipmentTableRef.value?.getRows?.() ?? childProductionTeamEquipmentRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        prodTeamId: masterId,
      }
      if (isUpdate && isPersistedProductionTeamEquipmentRow(row)) {
        normalized.productionTeamEquipmentId = row.productionTeamEquipmentId
      } else {
        delete normalized.productionTeamEquipmentId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ProductionTeamCreate & { productionTeamId?: string }> | null
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
  teamStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 productionTeamId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.productionTeamId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).teamEquipmentList
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
    const isCreate = !props.formData?.productionTeamId
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
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  teamCode: [
    {
      required: true,
      message: pi.ph('teamCode'),
      trigger: 'blur'
    }
  ],
  teamName: [
    {
      required: true,
      message: pi.ph('teamName'),
      trigger: 'blur'
    }
  ],
  teamCategory: [
    {
      required: true,
      message: pi.ph('teamCategory'),
      trigger: 'change'
    }
  ],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shiftNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shiftNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  teamStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('teamStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('teamStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await productionTeamEquipmentTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('teamStatus' in payload) {
    const rawteamStatus = payload.teamStatus
    payload.teamStatus = typeof rawteamStatus === 'number' ? rawteamStatus : Number(rawteamStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.productionTeamId)
  childProductionTeamEquipmentRows.value = []
  productionTeamEquipmentTableRef.value?.resetRows?.()
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
