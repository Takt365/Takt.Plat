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
    class="takt-generated-form work-order-form flex flex-col min-h-0"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/5)'"
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.workordercode')"
                name="workOrderCode"
              >
                <a-input
                  v-model:value="formState.workOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.workordercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenancenotificationid')"
                name="maintenanceNotificationId"
              >
                <a-input
                  v-model:value="formState.maintenanceNotificationId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancenotificationid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.notificationcode')"
                name="notificationCode"
              >
                <a-input
                  v-model:value="formState.notificationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.notificationcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.equipmentid')"
                name="equipmentId"
              >
                <a-input
                  v-model:value="formState.equipmentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.equipmentcode')"
                name="equipmentCode"
              >
                <a-input
                  v-model:value="formState.equipmentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.equipmentname')"
                name="equipmentName"
              >
                <a-input
                  v-model:value="formState.equipmentName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentname') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenancecategory')"
                name="maintenanceCategory"
              >
                <TaktSelect
                  v-model:value="formState.maintenanceCategory"
                  dict-type="logistics_maintenance_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancecategory') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenancetype')"
                name="maintenanceType"
              >
                <TaktSelect
                  v-model:value="formState.maintenanceType"
                  dict-type="logistics_maintenance_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancetype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.workorderstatus')"
                name="workOrderStatus"
              >
                <TaktSelect
                  v-model:value="formState.workOrderStatus"
                  dict-type="sys_ticket_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.workorderstatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.priority')"
                name="priority"
              >
                <a-input-number
                  v-model:value="formState.priority"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.priority') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.workcenter')"
                name="workCenter"
              >
                <a-input
                  v-model:value="formState.workCenter"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.workcenter') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.assignedtechnician')"
                name="assignedTechnician"
              >
                <a-input
                  v-model:value="formState.assignedTechnician"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.assignedtechnician') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenancecompany')"
                name="maintenanceCompany"
              >
                <a-input
                  v-model:value="formState.maintenanceCompany"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancecompany') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.plannedstarttime')"
                name="plannedStartTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedStartTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedstarttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.plannedendtime')"
                name="plannedEndTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedEndTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedendtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.actualstarttime')"
                name="actualStartTime"
              >
                <a-date-picker
                  v-model:value="formState.actualStartTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualstarttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.actualendtime')"
                name="actualEndTime"
              >
                <a-date-picker
                  v-model:value="formState.actualEndTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualendtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenanceworkorder.faultdescription')"
                name="faultDescription"
              >
                <a-textarea
                  v-model:value="formState.faultDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenanceworkorder.faultdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenancecontent')"
                name="maintenanceContent"
              >
                <a-textarea
                  v-model:value="formState.maintenanceContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenanceworkorder.maintenancecontent') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.solution')"
                name="solution"
              >
                <a-input
                  v-model:value="formState.solution"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.solution') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.costcenterid')"
                name="costCenterId"
              >
                <a-input
                  v-model:value="formState.costCenterId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costcenterid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.costcentercode')"
                name="costCenterCode"
              >
                <a-input
                  v-model:value="formState.costCenterCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costcentercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.costelementid')"
                name="costElementId"
              >
                <a-input
                  v-model:value="formState.costElementId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costelementid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.costelementcode')"
                name="costElementCode"
              >
                <a-input
                  v-model:value="formState.costElementCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costelementcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.totalmaterialcost')"
                name="totalMaterialCost"
              >
                <a-input-number
                  v-model:value="formState.totalMaterialCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalmaterialcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.totallaborcost')"
                name="totalLaborCost"
              >
                <a-input-number
                  v-model:value="formState.totalLaborCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totallaborcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.totalothercost')"
                name="totalOtherCost"
              >
                <a-input-number
                  v-model:value="formState.totalOtherCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalothercost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.totalcost')"
                name="totalCost"
              >
                <a-input-number
                  v-model:value="formState.totalCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.settlementstatus')"
                name="settlementStatus"
              >
                <a-input-number
                  v-model:value="formState.settlementStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.settlementstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.settlementtime')"
                name="settlementTime"
              >
                <a-date-picker
                  v-model:value="formState.settlementTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.settlementtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.completedat')"
                name="completedAt"
              >
                <a-input
                  v-model:value="formState.completedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.completedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.acceptedby')"
                name="acceptedBy"
              >
                <a-input
                  v-model:value="formState.acceptedBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.acceptedat')"
                name="acceptedAt"
              >
                <a-input
                  v-model:value="formState.acceptedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenanceresult')"
                name="maintenanceResult"
              >
                <a-input-number
                  v-model:value="formState.maintenanceResult"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenanceresult') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.nextmaintenancedate')"
                name="nextMaintenanceDate"
              >
                <a-date-picker
                  v-model:value="formState.nextMaintenanceDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.nextmaintenancedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenancecycledays')"
                name="maintenanceCycleDays"
              >
                <a-input-number
                  v-model:value="formState.maintenanceCycleDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancecycledays') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenanceimages')"
                name="maintenanceImages"
              >
                <a-input
                  v-model:value="formState.maintenanceImages"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenanceimages') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenanceworkorder.maintenancedocuments')"
                name="maintenanceDocuments"
              >
                <a-input
                  v-model:value="formState.maintenanceDocuments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancedocuments') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenanceworkorder.acceptedsummary')"
                name="acceptedSummary"
              >
                <a-input
                  v-model:value="formState.acceptedSummary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedsummary') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenanceworkorder.ishistoryarchived')"
                name="isHistoryArchived"
              >
                <TaktSelect
                  v-model:value="formState.isHistoryArchived"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.ishistoryarchived') })"
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
                    <span>{{ t('common.page.entity.extfield') }}</span>
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
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
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
      :title="t('entity.maintenanceworkordermaterial._self')"
      :add-button-entity="t('entity.maintenanceworkordermaterial._self')"
      id-field="maintenanceWorkOrderMaterialId"
      :default-row="createDefaultMaintenanceWorkOrderMaterialRow"
      :disabled="loading"
      section-border
    />
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

const childMaintenanceWorkOrderMaterialRows = ref<Record<string, unknown>[]>([])
const maintenanceWorkOrderMaterialTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 maintenanceWorkOrderMaterial 可编辑列 */
const maintenanceWorkOrderMaterialFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'workOrderCode',
    title: t('entity.maintenanceworkordermaterial.workordercode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: t('entity.maintenanceworkordermaterial.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'materialId',
    title: t('entity.maintenanceworkordermaterial.materialid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialCode',
    title: t('entity.maintenanceworkordermaterial.materialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialName',
    title: t('entity.maintenanceworkordermaterial.materialname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'requiredQuantity',
    title: t('entity.maintenanceworkordermaterial.requiredquantity'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'issuedQuantity',
    title: t('entity.maintenanceworkordermaterial.issuedquantity'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'materialUnit',
    title: t('entity.maintenanceworkordermaterial.materialunit'),
    editor: 'input',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaintenanceWorkOrderCreate & { maintenanceWorkOrderId?: string }> | null | undefined) {
  childMaintenanceWorkOrderMaterialRows.value = ((val as any)?.materials ?? []) as Record<string, unknown>[]
}

function createDefaultMaintenanceWorkOrderMaterialRow(): Record<string, unknown> {
  return {
    workOrderCode: '',
    lineNumber: (childMaintenanceWorkOrderMaterialRows.value.length + 1) * 10,
    materialId: '',
    materialCode: '',
    materialName: '',
    requiredQuantity: 0,
    issuedQuantity: 0,
    materialUnit: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.maintenanceWorkOrderId ?? ''
  return {
    ...formState,
    materials: maintenanceWorkOrderMaterialTableRef.value?.getRows?.() ?? childMaintenanceWorkOrderMaterialRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      maintenanceWorkOrderId: masterId,
    })),
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
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.plantcode') }),
      trigger: 'blur'
    }
  ],
  workOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.workordercode') }),
      trigger: 'blur'
    }
  ],
  equipmentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentid') }),
      trigger: 'blur'
    }
  ],
  equipmentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentcode') }),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentname') }),
      trigger: 'blur'
    }
  ],
  maintenanceCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancecategory') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancecategory') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancetype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancetype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workOrderStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.workorderstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.workorderstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.priority') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.priority') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalMaterialCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.totalmaterialcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.totalmaterialcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalLaborCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.totallaborcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.totallaborcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalOtherCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.totalothercost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.totalothercost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.totalcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.totalcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  settlementStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.settlementstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.settlementstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceResult: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenanceresult') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenanceresult') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceCycleDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancecycledays') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancecycledays') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isHistoryArchived: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.ishistoryarchived') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.ishistoryarchived') }))
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
