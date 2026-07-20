<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/components -->
<!-- 文件名称：schedule-form.vue -->
<!-- 功能描述：APS排程主表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form schedule-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="schedule-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialRequirementsPlanningId')"
                name="materialRequirementsPlanningId"
              >
                <a-input
                  v-model:value="formState.materialRequirementsPlanningId"
                  :placeholder="pi.ph('materialRequirementsPlanningId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialRequirementsPlanningCode')"
                name="materialRequirementsPlanningCode"
              >
                <a-input
                  v-model:value="formState.materialRequirementsPlanningCode"
                  :placeholder="pi.ph('materialRequirementsPlanningCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.apsScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.apsScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scheduleCode')"
                name="scheduleCode"
              >
                <a-input
                  v-model:value="formState.scheduleCode"
                  :placeholder="pi.ph('scheduleCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.apsScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scheduleName')"
                name="scheduleName"
              >
                <a-input
                  v-model:value="formState.scheduleName"
                  :placeholder="pi.ph('scheduleName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scheduleType')"
                name="scheduleType"
              >
                <a-input-number
                  v-model:value="formState.scheduleType"
                  :placeholder="pi.ph('scheduleType')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planDate')"
                name="planDate"
              >
                <a-date-picker
                  v-model:value="formState.planDate"
                  :placeholder="pi.ph('planDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planStartTime')"
                name="planStartTime"
              >
                <a-date-picker
                  v-model:value="formState.planStartTime"
                  :placeholder="pi.ph('planStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planEndTime')"
                name="planEndTime"
              >
                <a-date-picker
                  v-model:value="formState.planEndTime"
                  :placeholder="pi.ph('planEndTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planCycle')"
                name="planCycle"
              >
                <a-input-number
                  v-model:value="formState.planCycle"
                  :placeholder="pi.ph('planCycle')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workshopCode')"
                name="workshopCode"
              >
                <a-input
                  v-model:value="formState.workshopCode"
                  :placeholder="pi.ph('workshopCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.apsScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workshopName')"
                name="workshopName"
              >
                <a-input
                  v-model:value="formState.workshopName"
                  :placeholder="pi.ph('workshopName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionLineCode')"
                name="productionLineCode"
              >
                <a-input
                  v-model:value="formState.productionLineCode"
                  :placeholder="pi.ph('productionLineCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.apsScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionLineName')"
                name="productionLineName"
              >
                <a-input
                  v-model:value="formState.productionLineName"
                  :placeholder="pi.ph('productionLineName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scheduleStrategy')"
                name="scheduleStrategy"
              >
                <a-input-number
                  v-model:value="formState.scheduleStrategy"
                  :placeholder="pi.ph('scheduleStrategy')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scheduleAlgorithm')"
                name="scheduleAlgorithm"
              >
                <a-input-number
                  v-model:value="formState.scheduleAlgorithm"
                  :placeholder="pi.ph('scheduleAlgorithm')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('optimizationObjective')"
                name="optimizationObjective"
              >
                <a-input-number
                  v-model:value="formState.optimizationObjective"
                  :placeholder="pi.ph('optimizationObjective')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scheduleStatus')"
                name="scheduleStatus"
              >
                <a-input-number
                  v-model:value="formState.scheduleStatus"
                  :placeholder="pi.ph('scheduleStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannerId')"
                name="plannerId"
              >
                <TaktSelect
                  v-model:value="formState.plannerId"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('plannerId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannerName')"
                name="plannerName"
              >
                <a-input
                  v-model:value="formState.plannerName"
                  :placeholder="pi.ph('plannerName')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('publishTime')"
                name="publishTime"
              >
                <a-date-picker
                  v-model:value="formState.publishTime"
                  :placeholder="pi.ph('publishTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('publishUserId')"
                name="publishUserId"
              >
                <TaktSelect
                  v-model:value="formState.publishUserId"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('publishUserId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('publishUserName')"
                name="publishUserName"
              >
                <a-input
                  v-model:value="formState.publishUserName"
                  :placeholder="pi.ph('publishUserName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('scheduleDescription')"
                name="scheduleDescription"
              >
                <a-textarea
                  v-model:value="formState.scheduleDescription"
                  :placeholder="pi.ph('scheduleDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="pi.ph('companyDefaultCulture')"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="apsScheduleItemTableRef"
      v-model="childApsScheduleItemRows"
      :columns="apsScheduleItemFormColumns"
      :title="apsScheduleItemPi.self()"
      :add-button-entity="apsScheduleItemPi.self()"
      id-field="apsScheduleItemId"
      :default-row="createDefaultApsScheduleItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-apsOrderId="{ record }">
        <TaktSelect
          v-model:value="record.apsOrderId"
          api-url="TaktApsOrders/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsScheduleItemPi.queryPh('apsOrderId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-apsOperationId="{ record }">
        <TaktSelect
          v-model:value="record.apsOperationId"
          api-url="TaktApsOperations/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsScheduleItemPi.queryPh('apsOperationId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-routingItemId="{ record }">
        <TaktSelect
          v-model:value="record.routingItemId"
          api-url="TaktRoutingItems/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsScheduleItemPi.queryPh('routingItemId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-workOrderCode="{ record }">
        <TaktSelect
          v-model:value="record.workOrderCode"
          api-url="TaktProductionOrders/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsScheduleItemPi.queryPh('workOrderCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-productCode="{ record }">
        <TaktSelect
          v-model:value="record.productCode"
          api-url="TaktMaterials/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsScheduleItemPi.queryPh('productCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-workCenterCode="{ record }">
        <TaktSelect
          v-model:value="record.workCenterCode"
          api-url="TaktWorkCenters/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsScheduleItemPi.queryPh('workCenterCode', 'select')"
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
          :placeholder="apsScheduleItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * APS排程主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/aps/aps-schedule/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useApsScheduleI18n } from '../composables/use-schedule-i18n'

/** 实体字段 i18n */
const pi = useApsScheduleI18n()

import type { ApsScheduleCreate } from '@/types/logistics/manufacturing/aps/schedule'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","materialRequirementsPlanningId","materialRequirementsPlanningCode","plantCode","scheduleCode","scheduleName","scheduleType","planDate","planStartTime","planEndTime","planCycle","workshopCode","workshopName","productionLineCode","productionLineName","scheduleStrategy","scheduleAlgorithm","optimizationObjective","scheduleStatus","plannerId","plannerName","publishTime","publishUserId","publishUserName","scheduleDescription","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useApsScheduleItemI18n } from '../composables/use-schedule-item-i18n'

const apsScheduleItemPi = useApsScheduleItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childApsScheduleItemRows = ref<Record<string, unknown>[]>([])
const apsScheduleItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedApsScheduleItemRow(row: Record<string, unknown>): boolean {
  const id = row.apsScheduleItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextApsScheduleItemLineNumber(): number {
  const rows = apsScheduleItemTableRef.value?.getRows?.() ?? childApsScheduleItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 apsScheduleItem 可编辑列 */
const apsScheduleItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'apsOrderId',
    title: apsScheduleItemPi.label('apsOrderId'),
    width: 140,
  },
  {
    key: 'apsOperationId',
    title: apsScheduleItemPi.label('apsOperationId'),
    width: 140,
  },
  {
    key: 'routingItemId',
    title: apsScheduleItemPi.label('routingItemId'),
    width: 140,
  },
  {
    key: 'lineNumber',
    title: apsScheduleItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'workOrderCode',
    title: apsScheduleItemPi.label('workOrderCode'),
    width: 140,
  },
  {
    key: 'productCode',
    title: apsScheduleItemPi.label('productCode'),
    width: 140,
  },
  {
    key: 'productName',
    title: apsScheduleItemPi.label('productName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'workCenterCode',
    title: apsScheduleItemPi.label('workCenterCode'),
    width: 140,
  },
  {
    key: 'workCenterName',
    title: apsScheduleItemPi.label('workCenterName'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: apsScheduleItemPi.ph('workCenterName'),
  },
  {
    key: 'processCode',
    title: apsScheduleItemPi.label('processCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'processName',
    title: apsScheduleItemPi.label('processName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'processSequence',
    title: apsScheduleItemPi.label('processSequence'),
    width: 140,
  },
  {
    key: 'processStandardST',
    title: apsScheduleItemPi.label('processStandardST'),
    width: 140,
  },
  {
    key: 'processStandardSTUnit',
    title: apsScheduleItemPi.label('processStandardSTUnit'),
    width: 140,
  },
  {
    key: 'extraMinutes',
    title: apsScheduleItemPi.label('extraMinutes'),
    width: 140,
  },
  {
    key: 'planQuantity',
    title: apsScheduleItemPi.label('planQuantity'),
    width: 140,
  },
  {
    key: 'planStartTime',
    title: apsScheduleItemPi.label('planStartTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'planEndTime',
    title: apsScheduleItemPi.label('planEndTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'actualStartTime',
    title: apsScheduleItemPi.label('actualStartTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'actualEndTime',
    title: apsScheduleItemPi.label('actualEndTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'processStatus',
    title: apsScheduleItemPi.label('processStatus'),
    width: 140,
  },
  {
    key: 'priority',
    title: apsScheduleItemPi.label('priority'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: apsScheduleItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ApsScheduleCreate & { apsScheduleId?: string }> | null | undefined) {
  const rows_apsScheduleItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childApsScheduleItemRows.value = rows_apsScheduleItem
}

function createDefaultApsScheduleItemRow(): Record<string, unknown> {
  return {
    apsOrderId: '',
    apsOperationId: '',
    routingItemId: '',
    lineNumber: allocateNextApsScheduleItemLineNumber(),
    workOrderCode: '',
    productCode: '',
    productName: '',
    workCenterCode: '',
    workCenterName: '',
    processCode: '',
    processName: '',
    processSequence: 0,
    processStandardST: 0,
    processStandardSTUnit: 0,
    extraMinutes: 0,
    planQuantity: 0,
    planStartTime: '',
    planEndTime: '',
    actualStartTime: '',
    actualEndTime: '',
    processStatus: 0,
    priority: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.apsScheduleId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: apsScheduleItemTableRef.value?.getRows?.() ?? childApsScheduleItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        apsScheduleId: masterId,
      }
      if (isUpdate && isPersistedApsScheduleItemRow(row)) {
        normalized.apsScheduleItemId = row.apsScheduleItemId
      } else {
        delete normalized.apsScheduleItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ApsScheduleCreate & { apsScheduleId?: string }> | null
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 apsScheduleId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.apsScheduleId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
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
    const isCreate = !props.formData?.apsScheduleId
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
  scheduleCode: [
    {
      required: true,
      message: pi.ph('scheduleCode'),
      trigger: 'blur'
    }
  ],
  scheduleName: [
    {
      required: true,
      message: pi.ph('scheduleName'),
      trigger: 'blur'
    }
  ],
  scheduleType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scheduleType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scheduleType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  planDate: [
    {
      required: true,
      message: pi.ph('planDate'),
      trigger: 'change'
    }
  ],
  planStartTime: [
    {
      required: true,
      message: pi.ph('planStartTime'),
      trigger: 'change'
    }
  ],
  planEndTime: [
    {
      required: true,
      message: pi.ph('planEndTime'),
      trigger: 'change'
    }
  ],
  planCycle: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('planCycle'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('planCycle'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduleStrategy: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scheduleStrategy'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scheduleStrategy'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduleAlgorithm: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scheduleAlgorithm'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scheduleAlgorithm'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  optimizationObjective: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('optimizationObjective'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('optimizationObjective'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduleStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scheduleStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scheduleStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await apsScheduleItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('scheduleType' in payload) {
    const rawscheduleType = payload.scheduleType
    payload.scheduleType = typeof rawscheduleType === 'number' ? rawscheduleType : Number(rawscheduleType)
  }
  if ('planCycle' in payload) {
    const rawplanCycle = payload.planCycle
    payload.planCycle = typeof rawplanCycle === 'number' ? rawplanCycle : Number(rawplanCycle)
  }
  if ('scheduleStrategy' in payload) {
    const rawscheduleStrategy = payload.scheduleStrategy
    payload.scheduleStrategy = typeof rawscheduleStrategy === 'number' ? rawscheduleStrategy : Number(rawscheduleStrategy)
  }
  if ('scheduleAlgorithm' in payload) {
    const rawscheduleAlgorithm = payload.scheduleAlgorithm
    payload.scheduleAlgorithm = typeof rawscheduleAlgorithm === 'number' ? rawscheduleAlgorithm : Number(rawscheduleAlgorithm)
  }
  if ('optimizationObjective' in payload) {
    const rawoptimizationObjective = payload.optimizationObjective
    payload.optimizationObjective = typeof rawoptimizationObjective === 'number' ? rawoptimizationObjective : Number(rawoptimizationObjective)
  }
  if ('scheduleStatus' in payload) {
    const rawscheduleStatus = payload.scheduleStatus
    payload.scheduleStatus = typeof rawscheduleStatus === 'number' ? rawscheduleStatus : Number(rawscheduleStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.apsScheduleId)
  childApsScheduleItemRows.value = []
  apsScheduleItemTableRef.value?.resetRows?.()
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
