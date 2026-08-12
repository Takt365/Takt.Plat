<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：pcba-output-form.vue -->
<!-- 功能描述：PCBA日报实体 达成率维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-output-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-output-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  :placeholder="pi.ph('cultureCode')"
                  show-count
                  :maxlength="20"
                  disabled
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
    <!-- 下：子表 pcbaOutputDetails -->
    <TaktEditableTable
      ref="pcbaOutputDetailTableRef"
      v-model="childPcbaOutputDetailRows"
      :columns="pcbaOutputDetailFormColumns"
      :title="pcbaOutputDetailPi.self()"
      :add-button-entity="pcbaOutputDetailPi.self()"
      id-field="pcbaOutputDetailId"
      :default-row="createDefaultPcbaOutputDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-teamCode="{ record }">
        <TaktSelect
          v-model:value="record.teamCode"
          api-url="TaktProductionTeams/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.queryPh('teamCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-prodEquipCode="{ record }">
        <TaktSelect
          v-model:value="record.prodEquipCode"
          api-url="TaktProductionEquipments/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.queryPh('prodEquipCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-shiftNo="{ record }">
        <TaktSelect
          v-model:value="record.shiftNo"
          dict-type="logistics_shift_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.ph('shiftNo')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-panelSide="{ record }">
        <TaktSelect
          v-model:value="record.panelSide"
          dict-type="logistics_pcba_side_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.ph('panelSide')"
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
          :placeholder="pcbaOutputDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * PCBA日报实体 达成率维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/pcba-output/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaOutputI18n } from '../composables/use-pcba-output-i18n'

/** 实体字段 i18n */
const pi = usePcbaOutputI18n()

import type { PcbaOutputCreate } from '@/types/logistics/manufacturing/output/pcba-output'
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
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","prodCategory","prodDate","prodOrderType","prodOrderCode","modelCode","materialCode","batchCode","prodOrderQty","serialCode","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { usePcbaOutputDetailI18n } from '../composables/use-pcba-output-detail-i18n'

const pcbaOutputDetailPi = usePcbaOutputDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPcbaOutputDetailRows = ref<Record<string, unknown>[]>([])
const pcbaOutputDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPcbaOutputDetailRow(row: Record<string, unknown>): boolean {
  const id = row.pcbaOutputDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPcbaOutputDetailLineNumber(): number {
  const rows = pcbaOutputDetailTableRef.value?.getRows?.() ?? childPcbaOutputDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 pcbaOutputDetail 可编辑列 */
const pcbaOutputDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'prodOrderCode',
    title: pcbaOutputDetailPi.label('prodOrderCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: pcbaOutputDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'timePeriod',
    title: pcbaOutputDetailPi.label('timePeriod'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'teamCode',
    title: pcbaOutputDetailPi.label('teamCode'),
    width: 140,
  },
  {
    key: 'prodEquipCode',
    title: pcbaOutputDetailPi.label('prodEquipCode'),
    width: 140,
  },
  {
    key: 'directLabor',
    title: pcbaOutputDetailPi.label('directLabor'),
    width: 140,
  },
  {
    key: 'indirectLabor',
    title: pcbaOutputDetailPi.label('indirectLabor'),
    width: 140,
  },
  {
    key: 'shiftNo',
    title: pcbaOutputDetailPi.label('shiftNo'),
    width: 140,
  },
  {
    key: 'stdShorts',
    title: pcbaOutputDetailPi.label('stdShorts'),
    width: 140,
  },
  {
    key: 'pcbBoardType',
    title: pcbaOutputDetailPi.label('pcbBoardType'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'panelSide',
    title: pcbaOutputDetailPi.label('panelSide'),
    width: 140,
  },
  {
    key: 'batchQty',
    title: pcbaOutputDetailPi.label('batchQty'),
    width: 140,
  },
  {
    key: 'dailyCompletedQty',
    title: pcbaOutputDetailPi.label('dailyCompletedQty'),
    width: 140,
  },
  {
    key: 'serialCode',
    title: pcbaOutputDetailPi.label('serialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'defectCount',
    title: pcbaOutputDetailPi.label('defectCount'),
    width: 140,
  },
  {
    key: 'downtimeMinutes',
    title: pcbaOutputDetailPi.label('downtimeMinutes'),
    width: 140,
  },
  {
    key: 'downtimeReason',
    title: pcbaOutputDetailPi.label('downtimeReason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaOutputDetailPi.ph('downtimeReason'),
  },
  {
    key: 'downtimeDescription',
    title: pcbaOutputDetailPi.label('downtimeDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: pcbaOutputDetailPi.ph('downtimeDescription'),
    width: 180,
  },
  {
    key: 'repairMinutes',
    title: pcbaOutputDetailPi.label('repairMinutes'),
    width: 140,
  },
  {
    key: 'switchCount',
    title: pcbaOutputDetailPi.label('switchCount'),
    width: 140,
  },
  {
    key: 'switchTime',
    title: pcbaOutputDetailPi.label('switchTime'),
    width: 140,
  },
  {
    key: 'stopTime',
    title: pcbaOutputDetailPi.label('stopTime'),
    width: 140,
  },
  {
    key: 'totalMinutes',
    title: pcbaOutputDetailPi.label('totalMinutes'),
    width: 140,
  },
  {
    key: 'unachievedReason',
    title: pcbaOutputDetailPi.label('unachievedReason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaOutputDetailPi.ph('unachievedReason'),
  },
  {
    key: 'unachievedDescription',
    title: pcbaOutputDetailPi.label('unachievedDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: pcbaOutputDetailPi.ph('unachievedDescription'),
    width: 180,
  },
  {
    key: 'confirmMinutes',
    title: pcbaOutputDetailPi.label('confirmMinutes'),
    width: 140,
  },
  {
    key: 'mixedProd',
    title: pcbaOutputDetailPi.label('mixedProd'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: pcbaOutputDetailPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PcbaOutputCreate & { pcbaOutputId?: string }> | null | undefined) {
  const rows_pcbaOutputDetail = ((val as any)?.pcbaOutputDetails ?? []) as Record<string, unknown>[]
  childPcbaOutputDetailRows.value = rows_pcbaOutputDetail
}

function createDefaultPcbaOutputDetailRow(): Record<string, unknown> {
  return {
    prodOrderCode: '',
    lineNumber: allocateNextPcbaOutputDetailLineNumber(),
    timePeriod: '',
    teamCode: '',
    prodEquipCode: '',
    directLabor: 0,
    indirectLabor: 0,
    shiftNo: 0,
    stdShorts: 0,
    pcbBoardType: '',
    panelSide: '',
    batchQty: 0,
    dailyCompletedQty: 0,
    serialCode: '',
    defectCount: 0,
    downtimeMinutes: 0,
    downtimeReason: '',
    downtimeDescription: '',
    repairMinutes: 0,
    switchCount: 0,
    switchTime: 0,
    stopTime: 0,
    totalMinutes: 0,
    unachievedReason: '',
    unachievedDescription: '',
    confirmMinutes: 0,
    mixedProd: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.pcbaOutputId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    pcbaOutputDetails: pcbaOutputDetailTableRef.value?.getRows?.() ?? childPcbaOutputDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        pcbaOutputId: masterId,
      }
      if (isUpdate && isPersistedPcbaOutputDetailRow(row)) {
        normalized.pcbaOutputDetailId = row.pcbaOutputDetailId
      } else {
        delete normalized.pcbaOutputDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaOutputCreate & { pcbaOutputId?: string }> | null
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
  prodCategory: "FPP"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaOutputId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaOutputId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).pcbaOutputDetails
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
    const isCreate = !props.formData?.pcbaOutputId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  prodCategory: [
    {
      required: true,
      message: pi.ph('prodCategory'),
      trigger: 'change'
    }
  ],
  prodDate: [
    {
      required: true,
      message: pi.ph('prodDate'),
      trigger: 'change'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: pi.ph('prodOrderCode'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await pcbaOutputDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaOutputId)
  childPcbaOutputDetailRows.value = []
  pcbaOutputDetailTableRef.value?.resetRows?.()
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
