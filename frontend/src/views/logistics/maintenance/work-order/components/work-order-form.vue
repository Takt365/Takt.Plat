<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/work-order/components -->
<!-- 文件名称：work-order-form.vue -->
<!-- 功能描述：维护工单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form work-order-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="work-order-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="pi.ph('plantCode')"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workOrderCode')"
                name="workOrderCode"
              >
                <a-input
                  v-model:value="formState.workOrderCode"
                  :placeholder="pi.ph('workOrderCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceNotificationId')"
                name="maintenanceNotificationId"
              >
                <a-input
                  v-model:value="formState.maintenanceNotificationId"
                  :placeholder="pi.ph('maintenanceNotificationId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('notificationCode')"
                name="notificationCode"
              >
                <a-input
                  v-model:value="formState.notificationCode"
                  :placeholder="pi.ph('notificationCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentId')"
                name="equipmentId"
              >
                <a-input
                  v-model:value="formState.equipmentId"
                  :placeholder="pi.ph('equipmentId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentCode')"
                name="equipmentCode"
              >
                <a-input
                  v-model:value="formState.equipmentCode"
                  :placeholder="pi.ph('equipmentCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentName')"
                name="equipmentName"
              >
                <a-input
                  v-model:value="formState.equipmentName"
                  :placeholder="pi.ph('equipmentName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceCategory')"
                name="maintenanceCategory"
              >
                <TaktSelect
                  v-model:value="formState.maintenanceCategory"
                  dict-type="logistics_maintenance_category"
                  :placeholder="pi.ph('maintenanceCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceType')"
                name="maintenanceType"
              >
                <TaktSelect
                  v-model:value="formState.maintenanceType"
                  dict-type="logistics_maintenance_type"
                  :placeholder="pi.ph('maintenanceType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workOrderStatus')"
                name="workOrderStatus"
              >
                <TaktSelect
                  v-model:value="formState.workOrderStatus"
                  dict-type="sys_ticket_status"
                  :placeholder="pi.ph('workOrderStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priority')"
                name="priority"
              >
                <a-input-number
                  v-model:value="formState.priority"
                  :placeholder="pi.ph('priority')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workCenter')"
                name="workCenter"
              >
                <a-input
                  v-model:value="formState.workCenter"
                  :placeholder="pi.ph('workCenter')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assignedTechnician')"
                name="assignedTechnician"
              >
                <a-input
                  v-model:value="formState.assignedTechnician"
                  :placeholder="pi.ph('assignedTechnician')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceCompany')"
                name="maintenanceCompany"
              >
                <a-input
                  v-model:value="formState.maintenanceCompany"
                  :placeholder="pi.ph('maintenanceCompany')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedStartTime')"
                name="plannedStartTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedStartTime"
                  :placeholder="pi.ph('plannedStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedEndTime')"
                name="plannedEndTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedEndTime"
                  :placeholder="pi.ph('plannedEndTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualStartTime')"
                name="actualStartTime"
              >
                <a-date-picker
                  v-model:value="formState.actualStartTime"
                  :placeholder="pi.ph('actualStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualEndTime')"
                name="actualEndTime"
              >
                <a-date-picker
                  v-model:value="formState.actualEndTime"
                  :placeholder="pi.ph('actualEndTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('faultDescription')"
                name="faultDescription"
              >
                <a-textarea
                  v-model:value="formState.faultDescription"
                  :placeholder="pi.ph('faultDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('maintenanceContent')"
                name="maintenanceContent"
              >
                <a-textarea
                  v-model:value="formState.maintenanceContent"
                  :placeholder="pi.ph('maintenanceContent')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('solution')"
                name="solution"
              >
                <a-input
                  v-model:value="formState.solution"
                  :placeholder="pi.ph('solution')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costCenterId')"
                name="costCenterId"
              >
                <a-input
                  v-model:value="formState.costCenterId"
                  :placeholder="pi.ph('costCenterId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costCenterCode')"
                name="costCenterCode"
              >
                <a-input
                  v-model:value="formState.costCenterCode"
                  :placeholder="pi.ph('costCenterCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costElementId')"
                name="costElementId"
              >
                <a-input
                  v-model:value="formState.costElementId"
                  :placeholder="pi.ph('costElementId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costElementCode')"
                name="costElementCode"
              >
                <a-input
                  v-model:value="formState.costElementCode"
                  :placeholder="pi.ph('costElementCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalMaterialCost')"
                name="totalMaterialCost"
              >
                <a-input-number
                  v-model:value="formState.totalMaterialCost"
                  :placeholder="pi.ph('totalMaterialCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalLaborCost')"
                name="totalLaborCost"
              >
                <a-input-number
                  v-model:value="formState.totalLaborCost"
                  :placeholder="pi.ph('totalLaborCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalOtherCost')"
                name="totalOtherCost"
              >
                <a-input-number
                  v-model:value="formState.totalOtherCost"
                  :placeholder="pi.ph('totalOtherCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalCost')"
                name="totalCost"
              >
                <a-input-number
                  v-model:value="formState.totalCost"
                  :placeholder="pi.ph('totalCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('settlementStatus')"
                name="settlementStatus"
              >
                <a-input-number
                  v-model:value="formState.settlementStatus"
                  :placeholder="pi.ph('settlementStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('settlementTime')"
                name="settlementTime"
              >
                <a-date-picker
                  v-model:value="formState.settlementTime"
                  :placeholder="pi.ph('settlementTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('completedAt')"
                name="completedAt"
              >
                <a-date-picker
                  v-model:value="formState.completedAt"
                  :placeholder="pi.ph('completedAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('acceptedBy')"
                name="acceptedBy"
              >
                <a-input
                  v-model:value="formState.acceptedBy"
                  :placeholder="pi.ph('acceptedBy')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('acceptedAt')"
                name="acceptedAt"
              >
                <a-date-picker
                  v-model:value="formState.acceptedAt"
                  :placeholder="pi.ph('acceptedAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceResult')"
                name="maintenanceResult"
              >
                <a-input-number
                  v-model:value="formState.maintenanceResult"
                  :placeholder="pi.ph('maintenanceResult')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('nextMaintenanceDate')"
                name="nextMaintenanceDate"
              >
                <a-date-picker
                  v-model:value="formState.nextMaintenanceDate"
                  :placeholder="pi.ph('nextMaintenanceDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceCycleDays')"
                name="maintenanceCycleDays"
              >
                <a-input-number
                  v-model:value="formState.maintenanceCycleDays"
                  :placeholder="pi.ph('maintenanceCycleDays')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceImages')"
                name="maintenanceImages"
              >
                <a-input
                  v-model:value="formState.maintenanceImages"
                  :placeholder="pi.ph('maintenanceImages')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceDocuments')"
                name="maintenanceDocuments"
              >
                <a-input
                  v-model:value="formState.maintenanceDocuments"
                  :placeholder="pi.ph('maintenanceDocuments')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('acceptedSummary')"
                name="acceptedSummary"
              >
                <a-input
                  v-model:value="formState.acceptedSummary"
                  :placeholder="pi.ph('acceptedSummary')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isHistoryArchived')"
                name="isHistoryArchived"
              >
                <TaktSelect
                  v-model:value="formState.isHistoryArchived"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isHistoryArchived')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/6)'"
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
    <!-- 下：子表 materials -->
    <TaktEditableTable
      ref="maintenanceWorkOrderMaterialTableRef"
      v-model="childMaintenanceWorkOrderMaterialRows"
      :columns="maintenanceWorkOrderMaterialFormColumns"
      :title="maintenanceWorkOrderMaterialPi.self()"
      :add-button-entity="maintenanceWorkOrderMaterialPi.self()"
      id-field="maintenanceWorkOrderMaterialId"
      :default-row="createDefaultMaintenanceWorkOrderMaterialRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="maintenanceWorkOrderMaterialPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 维护工单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/work-order/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaintenanceWorkOrderI18n } from '../composables/use-work-order-i18n'

/** 实体字段 i18n */
const pi = useMaintenanceWorkOrderI18n()

import type { MaintenanceWorkOrderCreate } from '@/types/logistics/maintenance/work-order'
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
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","workOrderCode","maintenanceNotificationId","notificationCode","equipmentId","equipmentCode","equipmentName","maintenanceCategory","maintenanceType","workOrderStatus","priority","workCenter","assignedTechnician","maintenanceCompany","plannedStartTime","plannedEndTime","actualStartTime","actualEndTime","faultDescription","maintenanceContent","solution","costCenterId","costCenterCode","costElementId","costElementCode","totalMaterialCost","totalLaborCost","totalOtherCost","totalCost","settlementStatus","settlementTime","completedAt","acceptedBy","acceptedAt","maintenanceResult","nextMaintenanceDate","maintenanceCycleDays","maintenanceImages","maintenanceDocuments","acceptedSummary","isHistoryArchived","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useMaintenanceWorkOrderMaterialI18n } from '../composables/use-work-order-material-i18n'

const maintenanceWorkOrderMaterialPi = useMaintenanceWorkOrderMaterialI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMaintenanceWorkOrderMaterialRows = ref<Record<string, unknown>[]>([])
const maintenanceWorkOrderMaterialTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedMaintenanceWorkOrderMaterialRow(row: Record<string, unknown>): boolean {
  const id = row.maintenanceWorkOrderMaterialId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextMaintenanceWorkOrderMaterialLineNumber(): number {
  const rows = maintenanceWorkOrderMaterialTableRef.value?.getRows?.() ?? childMaintenanceWorkOrderMaterialRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 maintenanceWorkOrderMaterial 可编辑列 */
const maintenanceWorkOrderMaterialFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'workOrderCode',
    title: maintenanceWorkOrderMaterialPi.label('workOrderCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: maintenanceWorkOrderMaterialPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialId',
    title: maintenanceWorkOrderMaterialPi.label('materialId'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialCode',
    title: maintenanceWorkOrderMaterialPi.label('materialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialName',
    title: maintenanceWorkOrderMaterialPi.label('materialName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'requiredQuantity',
    title: maintenanceWorkOrderMaterialPi.label('requiredQuantity'),
    width: 140,
  },
  {
    key: 'issuedQuantity',
    title: maintenanceWorkOrderMaterialPi.label('issuedQuantity'),
    width: 140,
  },
  {
    key: 'materialUnit',
    title: maintenanceWorkOrderMaterialPi.label('materialUnit'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'unitPrice',
    title: maintenanceWorkOrderMaterialPi.label('unitPrice'),
    width: 140,
  },
  {
    key: 'amount',
    title: maintenanceWorkOrderMaterialPi.label('amount'),
    width: 140,
  },
  {
    key: 'warehouseCode',
    title: maintenanceWorkOrderMaterialPi.label('warehouseCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: maintenanceWorkOrderMaterialPi.ph('warehouseCode'),
  },
  {
    key: 'storageLocation',
    title: maintenanceWorkOrderMaterialPi.label('storageLocation'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: maintenanceWorkOrderMaterialPi.ph('storageLocation'),
  },
  {
    key: 'issueStatus',
    title: maintenanceWorkOrderMaterialPi.label('issueStatus'),
    width: 140,
  },
  {
    key: 'issueTime',
    title: maintenanceWorkOrderMaterialPi.label('issueTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'isObsolete',
    title: maintenanceWorkOrderMaterialPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaintenanceWorkOrderCreate & { maintenanceWorkOrderId?: string }> | null | undefined) {
  const rows_maintenanceWorkOrderMaterial = ((val as any)?.materials ?? []) as Record<string, unknown>[]
  childMaintenanceWorkOrderMaterialRows.value = rows_maintenanceWorkOrderMaterial
}

function createDefaultMaintenanceWorkOrderMaterialRow(): Record<string, unknown> {
  return {
    workOrderCode: '',
    lineNumber: allocateNextMaintenanceWorkOrderMaterialLineNumber(),
    materialId: '',
    materialCode: '',
    materialName: '',
    requiredQuantity: 0,
    issuedQuantity: 0,
    materialUnit: '',
    unitPrice: 0,
    amount: 0,
    warehouseCode: '',
    storageLocation: '',
    issueStatus: 0,
    issueTime: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.maintenanceWorkOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    materials: maintenanceWorkOrderMaterialTableRef.value?.getRows?.() ?? childMaintenanceWorkOrderMaterialRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        maintenanceWorkOrderId: masterId,
      }
      if (isUpdate && isPersistedMaintenanceWorkOrderMaterialRow(row)) {
        normalized.maintenanceWorkOrderMaterialId = row.maintenanceWorkOrderMaterialId
      } else {
        delete normalized.maintenanceWorkOrderMaterialId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaintenanceWorkOrderCreate & { maintenanceWorkOrderId?: string }> | null
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
  workOrderStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 maintenanceWorkOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.maintenanceWorkOrderId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).materials
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
    const isCreate = !props.formData?.maintenanceWorkOrderId
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
      trigger: 'blur'
    }
  ],
  workOrderCode: [
    {
      required: true,
      message: pi.ph('workOrderCode'),
      trigger: 'blur'
    }
  ],
  equipmentId: [
    {
      required: true,
      message: pi.ph('equipmentId'),
      trigger: 'blur'
    }
  ],
  equipmentCode: [
    {
      required: true,
      message: pi.ph('equipmentCode'),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: pi.ph('equipmentName'),
      trigger: 'blur'
    }
  ],
  maintenanceCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workOrderStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('workOrderStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('workOrderStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('priority'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('priority'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalMaterialCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalMaterialCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalMaterialCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalLaborCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalLaborCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalLaborCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalOtherCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalOtherCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalOtherCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  settlementStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('settlementStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('settlementStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceResult: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceResult'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceResult'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceCycleDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceCycleDays'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceCycleDays'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isHistoryArchived: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isHistoryArchived'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isHistoryArchived'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await maintenanceWorkOrderMaterialTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('maintenanceCategory' in payload) {
    const rawmaintenanceCategory = payload.maintenanceCategory
    payload.maintenanceCategory = typeof rawmaintenanceCategory === 'number' ? rawmaintenanceCategory : Number(rawmaintenanceCategory)
  }
  if ('maintenanceType' in payload) {
    const rawmaintenanceType = payload.maintenanceType
    payload.maintenanceType = typeof rawmaintenanceType === 'number' ? rawmaintenanceType : Number(rawmaintenanceType)
  }
  if ('workOrderStatus' in payload) {
    const rawworkOrderStatus = payload.workOrderStatus
    payload.workOrderStatus = typeof rawworkOrderStatus === 'number' ? rawworkOrderStatus : Number(rawworkOrderStatus)
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    payload.priority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
  }
  if ('totalMaterialCost' in payload) {
    const rawtotalMaterialCost = payload.totalMaterialCost
    payload.totalMaterialCost = typeof rawtotalMaterialCost === 'number' ? rawtotalMaterialCost : Number(rawtotalMaterialCost)
  }
  if ('totalLaborCost' in payload) {
    const rawtotalLaborCost = payload.totalLaborCost
    payload.totalLaborCost = typeof rawtotalLaborCost === 'number' ? rawtotalLaborCost : Number(rawtotalLaborCost)
  }
  if ('totalOtherCost' in payload) {
    const rawtotalOtherCost = payload.totalOtherCost
    payload.totalOtherCost = typeof rawtotalOtherCost === 'number' ? rawtotalOtherCost : Number(rawtotalOtherCost)
  }
  if ('totalCost' in payload) {
    const rawtotalCost = payload.totalCost
    payload.totalCost = typeof rawtotalCost === 'number' ? rawtotalCost : Number(rawtotalCost)
  }
  if ('settlementStatus' in payload) {
    const rawsettlementStatus = payload.settlementStatus
    payload.settlementStatus = typeof rawsettlementStatus === 'number' ? rawsettlementStatus : Number(rawsettlementStatus)
  }
  if ('maintenanceResult' in payload) {
    const rawmaintenanceResult = payload.maintenanceResult
    payload.maintenanceResult = typeof rawmaintenanceResult === 'number' ? rawmaintenanceResult : Number(rawmaintenanceResult)
  }
  if ('maintenanceCycleDays' in payload) {
    const rawmaintenanceCycleDays = payload.maintenanceCycleDays
    payload.maintenanceCycleDays = typeof rawmaintenanceCycleDays === 'number' ? rawmaintenanceCycleDays : Number(rawmaintenanceCycleDays)
  }
  if ('isHistoryArchived' in payload) {
    const rawisHistoryArchived = payload.isHistoryArchived
    payload.isHistoryArchived = typeof rawisHistoryArchived === 'number' ? rawisHistoryArchived : Number(rawisHistoryArchived)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.maintenanceWorkOrderId)
  childMaintenanceWorkOrderMaterialRows.value = []
  maintenanceWorkOrderMaterialTableRef.value?.resetRows?.()
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
