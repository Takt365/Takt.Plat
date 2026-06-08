<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/equipment/components -->
<!-- 文件名称：equipment-form.vue -->
<!-- 功能描述：Takt工厂设备实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="equipment-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="t('entity.equipment.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.code')"
                name="equipmentCode"
              >
                <a-input
                  v-model:value="formState.equipmentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.name')"
                name="equipmentName"
              >
                <a-input
                  v-model:value="formState.equipmentName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.type')"
                name="equipmentType"
              >
                <a-input-number
                  v-model:value="formState.equipmentType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.model')"
                name="equipmentModel"
              >
                <a-input
                  v-model:value="formState.equipmentModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.model') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.specification')"
                name="equipmentSpecification"
              >
                <a-input
                  v-model:value="formState.equipmentSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.specification') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.brand')"
                name="equipmentBrand"
              >
                <a-input
                  v-model:value="formState.equipmentBrand"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.brand') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.manufacturer')"
                name="manufacturer"
              >
                <a-input
                  v-model:value="formState.manufacturer"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.manufacturer') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.dealerby')"
                name="dealerBy"
              >
                <a-input
                  v-model:value="formState.dealerBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.dealerby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.serialnumber')"
                name="serialNumber"
              >
                <a-input
                  v-model:value="formState.serialNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.serialnumber') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.workshopby')"
                name="workshopBy"
              >
                <a-input
                  v-model:value="formState.workshopBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workshopby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.productionlineby')"
                name="productionLineBy"
              >
                <a-input
                  v-model:value="formState.productionLineBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.productionlineby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.workstationby')"
                name="workstationBy"
              >
                <a-input
                  v-model:value="formState.workstationBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workstationby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.deptby')"
                name="deptBy"
              >
                <a-input
                  v-model:value="formState.deptBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.deptby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.location')"
                name="equipmentLocation"
              >
                <a-input
                  v-model:value="formState.equipmentLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.location') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.responsibleuserby')"
                name="responsibleUserBy"
              >
                <a-input
                  v-model:value="formState.responsibleUserBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.responsibleuserby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.operatorby')"
                name="operatorBy"
              >
                <a-input
                  v-model:value="formState.operatorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.operatorby') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.purchasedate')"
                name="purchaseDate"
              >
                <a-date-picker
                  v-model:value="formState.purchaseDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.purchasedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.installationdate')"
                name="installationDate"
              >
                <a-date-picker
                  v-model:value="formState.installationDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.installationdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.startdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.warrantystartdate')"
                name="warrantyStartDate"
              >
                <a-date-picker
                  v-model:value="formState.warrantyStartDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystartdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.warrantyenddate')"
                name="warrantyEndDate"
              >
                <a-date-picker
                  v-model:value="formState.warrantyEndDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantyenddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.originalvalue')"
                name="equipmentOriginalValue"
              >
                <a-input-number
                  v-model:value="formState.equipmentOriginalValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.originalvalue') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.technicalparameters')"
                name="technicalParameters"
              >
                <a-input
                  v-model:value="formState.technicalParameters"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.technicalparameters') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.images')"
                name="equipmentImages"
              >
                <a-input
                  v-model:value="formState.equipmentImages"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.images') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.documents')"
                name="equipmentDocuments"
              >
                <a-input
                  v-model:value="formState.equipmentDocuments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.documents') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.iscritical')"
                name="isCritical"
              >
                <a-input-number
                  v-model:value="formState.isCritical"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.iscritical') })"
                  size="small"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.warrantystatus')"
                name="warrantyStatus"
              >
                <a-input-number
                  v-model:value="formState.warrantyStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.warrantystatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.status')"
                name="equipmentStatus"
              >
                <a-input-number
                  v-model:value="formState.equipmentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.status') })"
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
      <!-- 子表：maintenance -->
      <a-tab-pane
        key="child-maintenanceRecords"
        :tab="t('entity.maintenance._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddMaintenanceRow">
            {{ t('common.page.button.create') }}{{ t('entity.maintenance._self') }}
          </a-button>
        </div>
        <a-table
          :columns="maintenanceFormColumns"
          :data-source="childMaintenanceRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'maintenanceType'">
              <a-input-number
                v-model:value="record.maintenanceType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.type') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'maintenanceCompany'">
              <a-input
                v-model:value="record.maintenanceCompany"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.company') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'maintenanceTechnician'">
              <a-input
                v-model:value="record.maintenanceTechnician"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.technician') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'maintenanceDate'">
              <a-date-picker
                v-model:value="record.maintenanceDate"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.date') })"
                value-format="YYYY-MM-DD"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'maintenanceStartTime'">
              <a-input
                v-model:value="record.maintenanceStartTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.starttime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'maintenanceEndTime'">
              <a-input
                v-model:value="record.maintenanceEndTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.endtime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'maintenanceContent'">
              <a-textarea
                v-model:value="record.maintenanceContent"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenance.content') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveMaintenanceRow(index)">
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
 * Takt工厂设备实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/equipment/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EquipmentCreate, MaintenanceCreate, Maintenance } from '@/types/logistics/maintenance/equipment'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","equipmentCode","equipmentName","equipmentType","equipmentModel","equipmentSpecification","equipmentBrand","manufacturer","dealerBy","serialNumber","workshopBy","productionLineBy","workstationBy","deptBy","equipmentLocation","responsibleUserBy","operatorBy","purchaseDate","installationDate","startDate","warrantyStartDate","warrantyEndDate","equipmentOriginalValue","technicalParameters","equipmentImages","equipmentDocuments","isCritical","warrantyStatus","equipmentStatus","extFieldJson","remark"]

/** maintenance 子表行（表单 Tab 内嵌） */
const childMaintenanceRows = ref<Record<string, unknown>[]>([])

/** 子表 maintenance 表单列定义 */
const maintenanceFormColumns = computed(() => [
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
    title: t('entity.maintenance.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.maintenance.type'),
    dataIndex: 'maintenanceType',
    key: 'maintenanceType',
    width: 140,
  },
  {
    title: t('entity.maintenance.company'),
    dataIndex: 'maintenanceCompany',
    key: 'maintenanceCompany',
    width: 140,
  },
  {
    title: t('entity.maintenance.technician'),
    dataIndex: 'maintenanceTechnician',
    key: 'maintenanceTechnician',
    width: 140,
  },
  {
    title: t('entity.maintenance.date'),
    dataIndex: 'maintenanceDate',
    key: 'maintenanceDate',
    width: 140,
  },
  {
    title: t('entity.maintenance.starttime'),
    dataIndex: 'maintenanceStartTime',
    key: 'maintenanceStartTime',
    width: 140,
  },
  {
    title: t('entity.maintenance.endtime'),
    dataIndex: 'maintenanceEndTime',
    key: 'maintenanceEndTime',
    width: 140,
  },
  {
    title: t('entity.maintenance.content'),
    dataIndex: 'maintenanceContent',
    key: 'maintenanceContent',
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
function syncChildRowsFromFormData(val: Partial<EquipmentCreate & { equipmentId?: string }> | null | undefined) {
  childMaintenanceRows.value = ((val as any)?.maintenanceRecords ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.maintenanceId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 maintenance 行 */
function handleAddMaintenanceRow() {
  childMaintenanceRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      maintenanceType: 0,
      maintenanceCompany: '',
      maintenanceTechnician: '',
      maintenanceDate: '',
      maintenanceStartTime: '',
      maintenanceEndTime: '',
      maintenanceContent: '',
  })
}

/** 表单 Tab 内删除 maintenance 行 */
function handleRemoveMaintenanceRow(index: number) {
  childMaintenanceRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    maintenanceRecords: childMaintenanceRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EquipmentCreate & { equipmentId?: string }> | null
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
    delete (next as any).maintenanceRecords
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
    const isCreate = !props.formData?.equipmentId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.equipment.plantcode') }),
      trigger: 'blur'
    }
  ],
  equipmentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipment.code') }),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipment.name') }),
      trigger: 'blur'
    }
  ],
  equipmentType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.type') }),
      trigger: 'change'
    }
  ],
  equipmentOriginalValue: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.originalvalue') }),
      trigger: 'change'
    }
  ],
  isCritical: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.iscritical') }),
      trigger: 'change'
    }
  ],
  warrantyStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystatus') }),
      trigger: 'change'
    }
  ],
  equipmentStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.status') }),
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
  childMaintenanceRows.value = []
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
