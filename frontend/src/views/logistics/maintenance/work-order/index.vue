<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/work-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：维护工单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getMaintenanceWorkOrderId"
      :master-row-selection="rowSelection"
      master-id-column-key="maintenanceWorkOrderId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="approval"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
        <TaktToolsBar
      create-permission="logistics:maintenance:equipment:create"
      update-permission="logistics:maintenance:equipment:update"
      delete-permission="logistics:maintenance:equipment:delete"
      import-permission="logistics:maintenance:equipment:import"
      export-permission="logistics:maintenance:equipment:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
        />
      </template>
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'maintenanceCategory'">
          <TaktDictTag
            :value="getMaintenanceWorkOrderDictValue(record, 'maintenanceCategory')"
            dict-type="logistics_maintenance_category"
          />
        </template>
        <template v-else-if="column.key === 'maintenanceType'">
          <TaktDictTag
            :value="getMaintenanceWorkOrderDictValue(record, 'maintenanceType')"
            dict-type="logistics_maintenance_type"
          />
        </template>
        <template v-else-if="column.key === 'workOrderStatus'">
          <TaktDictTag
            :value="getMaintenanceWorkOrderDictValue(record, 'workOrderStatus')"
            dict-type="sys_ticket_status"
          />
        </template>
        <template v-else-if="column.key === 'isHistoryArchived'">
          <TaktDictTag
            :value="getMaintenanceWorkOrderDictValue(record, 'isHistoryArchived')"
            dict-type="sys_yes_no_type"
          />
        </template>
      </template>
      <template #detail>
        <MaintenanceWorkOrderMaterialPanel
          ref="maintenanceWorkOrderMaterialPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="1100px"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MaintenanceWorkOrderForm
        :key="formData?.maintenanceWorkOrderId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-logistics-maintenance-work-order'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="pi.queryPh('plantCode', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workOrderCode')">
      <a-form-item :label="pi.queryLabel('workOrderCode')">
        <a-input
          v-model:value="advancedQueryForm.workOrderCode"
          :placeholder="pi.queryPh('workOrderCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceNotificationId')">
      <a-form-item :label="pi.queryLabel('maintenanceNotificationId')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceNotificationId"
          :placeholder="pi.queryPh('maintenanceNotificationId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('notificationCode')">
      <a-form-item :label="pi.queryLabel('notificationCode')">
        <a-input
          v-model:value="advancedQueryForm.notificationCode"
          :placeholder="pi.queryPh('notificationCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentId')">
      <a-form-item :label="pi.queryLabel('equipmentId')">
        <a-input
          v-model:value="advancedQueryForm.equipmentId"
          :placeholder="pi.queryPh('equipmentId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentCode')">
      <a-form-item :label="pi.queryLabel('equipmentCode')">
        <a-input
          v-model:value="advancedQueryForm.equipmentCode"
          :placeholder="pi.queryPh('equipmentCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentName')">
      <a-form-item :label="pi.queryLabel('equipmentName')">
        <a-input
          v-model:value="advancedQueryForm.equipmentName"
          :placeholder="pi.queryPh('equipmentName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCategory')">
      <a-form-item :label="pi.queryLabel('maintenanceCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceCategory"
          dict-type="logistics_maintenance_category"
          :placeholder="pi.queryPh('maintenanceCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceType')">
      <a-form-item :label="pi.queryLabel('maintenanceType')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceType"
          dict-type="logistics_maintenance_type"
          :placeholder="pi.queryPh('maintenanceType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workOrderStatus')">
      <a-form-item :label="pi.queryLabel('workOrderStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.workOrderStatus"
          dict-type="sys_ticket_status"
          :placeholder="pi.queryPh('workOrderStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priority')">
      <a-form-item :label="pi.queryLabel('priority')">
        <a-input-number
          v-model:value="advancedQueryForm.priority"
          :placeholder="pi.queryPh('priority', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenter')">
      <a-form-item :label="pi.queryLabel('workCenter')">
        <a-input
          v-model:value="advancedQueryForm.workCenter"
          :placeholder="pi.queryPh('workCenter', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedTechnician')">
      <a-form-item :label="pi.queryLabel('assignedTechnician')">
        <a-input
          v-model:value="advancedQueryForm.assignedTechnician"
          :placeholder="pi.queryPh('assignedTechnician', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCompany')">
      <a-form-item :label="pi.queryLabel('maintenanceCompany')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceCompany"
          :placeholder="pi.queryPh('maintenanceCompany', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeStart')">
      <a-form-item :label="pi.queryLabel('plannedStartTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeStart"
          :placeholder="pi.queryPh('plannedStartTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeEnd')">
      <a-form-item :label="pi.queryLabel('plannedStartTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeEnd"
          :placeholder="pi.queryPh('plannedStartTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeStart')">
      <a-form-item :label="pi.queryLabel('plannedEndTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeStart"
          :placeholder="pi.queryPh('plannedEndTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeEnd')">
      <a-form-item :label="pi.queryLabel('plannedEndTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeEnd"
          :placeholder="pi.queryPh('plannedEndTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeStart')">
      <a-form-item :label="pi.queryLabel('actualStartTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeStart"
          :placeholder="pi.queryPh('actualStartTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeEnd')">
      <a-form-item :label="pi.queryLabel('actualStartTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeEnd"
          :placeholder="pi.queryPh('actualStartTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeStart')">
      <a-form-item :label="pi.queryLabel('actualEndTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeStart"
          :placeholder="pi.queryPh('actualEndTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeEnd')">
      <a-form-item :label="pi.queryLabel('actualEndTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeEnd"
          :placeholder="pi.queryPh('actualEndTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('faultDescription')">
      <a-form-item :label="pi.queryLabel('faultDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.faultDescription"
          :placeholder="pi.queryPh('faultDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceContent')">
      <a-form-item :label="pi.queryLabel('maintenanceContent')">
        <a-textarea
          v-model:value="advancedQueryForm.maintenanceContent"
          :placeholder="pi.queryPh('maintenanceContent', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('solution')">
      <a-form-item :label="pi.queryLabel('solution')">
        <a-input
          v-model:value="advancedQueryForm.solution"
          :placeholder="pi.queryPh('solution', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterId')">
      <a-form-item :label="pi.queryLabel('costCenterId')">
        <a-input
          v-model:value="advancedQueryForm.costCenterId"
          :placeholder="pi.queryPh('costCenterId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterCode')">
      <a-form-item :label="pi.queryLabel('costCenterCode')">
        <a-input
          v-model:value="advancedQueryForm.costCenterCode"
          :placeholder="pi.queryPh('costCenterCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementId')">
      <a-form-item :label="pi.queryLabel('costElementId')">
        <a-input
          v-model:value="advancedQueryForm.costElementId"
          :placeholder="pi.queryPh('costElementId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementCode')">
      <a-form-item :label="pi.queryLabel('costElementCode')">
        <a-input
          v-model:value="advancedQueryForm.costElementCode"
          :placeholder="pi.queryPh('costElementCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalMaterialCost')">
      <a-form-item :label="pi.queryLabel('totalMaterialCost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalMaterialCost"
          :placeholder="pi.queryPh('totalMaterialCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalLaborCost')">
      <a-form-item :label="pi.queryLabel('totalLaborCost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalLaborCost"
          :placeholder="pi.queryPh('totalLaborCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalOtherCost')">
      <a-form-item :label="pi.queryLabel('totalOtherCost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalOtherCost"
          :placeholder="pi.queryPh('totalOtherCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalCost')">
      <a-form-item :label="pi.queryLabel('totalCost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalCost"
          :placeholder="pi.queryPh('totalCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementStatus')">
      <a-form-item :label="pi.queryLabel('settlementStatus')">
        <a-input-number
          v-model:value="advancedQueryForm.settlementStatus"
          :placeholder="pi.queryPh('settlementStatus', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementTimeStart')">
      <a-form-item :label="pi.queryLabel('settlementTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.settlementTimeStart"
          :placeholder="pi.queryPh('settlementTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementTimeEnd')">
      <a-form-item :label="pi.queryLabel('settlementTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.settlementTimeEnd"
          :placeholder="pi.queryPh('settlementTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completedAtStart')">
      <a-form-item :label="pi.queryLabel('completedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.completedAtStart"
          :placeholder="pi.queryPh('completedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completedAtEnd')">
      <a-form-item :label="pi.queryLabel('completedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.completedAtEnd"
          :placeholder="pi.queryPh('completedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedBy')">
      <a-form-item :label="pi.queryLabel('acceptedBy')">
        <a-input
          v-model:value="advancedQueryForm.acceptedBy"
          :placeholder="pi.queryPh('acceptedBy', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtStart')">
      <a-form-item :label="pi.queryLabel('acceptedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.acceptedAtStart"
          :placeholder="pi.queryPh('acceptedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtEnd')">
      <a-form-item :label="pi.queryLabel('acceptedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.acceptedAtEnd"
          :placeholder="pi.queryPh('acceptedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceResult')">
      <a-form-item :label="pi.queryLabel('maintenanceResult')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceResult"
          :placeholder="pi.queryPh('maintenanceResult', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateStart')">
      <a-form-item :label="pi.queryLabel('nextMaintenanceDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateStart"
          :placeholder="pi.queryPh('nextMaintenanceDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateEnd')">
      <a-form-item :label="pi.queryLabel('nextMaintenanceDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateEnd"
          :placeholder="pi.queryPh('nextMaintenanceDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCycleDays')">
      <a-form-item :label="pi.queryLabel('maintenanceCycleDays')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceCycleDays"
          :placeholder="pi.queryPh('maintenanceCycleDays', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceImages')">
      <a-form-item :label="pi.queryLabel('maintenanceImages')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceImages"
          :placeholder="pi.queryPh('maintenanceImages', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDocuments')">
      <a-form-item :label="pi.queryLabel('maintenanceDocuments')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceDocuments"
          :placeholder="pi.queryPh('maintenanceDocuments', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedSummary')">
      <a-form-item :label="pi.queryLabel('acceptedSummary')">
        <a-input
          v-model:value="advancedQueryForm.acceptedSummary"
          :placeholder="pi.queryPh('acceptedSummary', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isHistoryArchived')">
      <a-form-item :label="pi.queryLabel('isHistoryArchived')">
        <TaktSelect
          v-model:value="advancedQueryForm.isHistoryArchived"
          dict-type="sys_yes_no_type"
          :placeholder="pi.queryPh('isHistoryArchived', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="pi.queryLabel('approvalStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="pi.queryPh('approvalStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="pi.queryLabel('initiatorId')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="pi.queryPh('initiatorId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="pi.queryLabel('initiatedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="pi.queryPh('initiatedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="pi.queryLabel('initiatedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="pi.queryPh('initiatedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="pi.queryLabel('approvedBy')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="pi.queryPh('approvedBy', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="pi.queryLabel('approvedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="pi.queryPh('approvedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="pi.queryLabel('approvedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="pi.queryPh('approvedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="pi.queryLabel('flowInstanceId')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="pi.queryPh('flowInstanceId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="MAINTENANCEWORKORDER_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'maintenanceWorkOrderId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 维护工单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/work-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import MaintenanceWorkOrderForm from './components/work-order-form.vue'
import MaintenanceWorkOrderMaterialPanel from './components/work-order-material-panel.vue'
import { provideMaintenanceWorkOrderMasterContext, type MaintenanceWorkOrderRowRecord } from './composables/use-work-order-master-context'
import { getMaintenanceWorkOrderList, getMaintenanceWorkOrderById, createMaintenanceWorkOrder, updateMaintenanceWorkOrder, deleteMaintenanceWorkOrderById, deleteMaintenanceWorkOrderBatch, getMaintenanceWorkOrderTemplate, importMaintenanceWorkOrder, exportMaintenanceWorkOrder, updateMaintenanceWorkOrderStatus } from '@/api/logistics/maintenance/work-order'
import type { MaintenanceWorkOrder, MaintenanceWorkOrderQuery } from '@/types/logistics/maintenance/work-order'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useMaintenanceWorkOrderI18n,
  MAINTENANCEWORKORDER_LIST_FIELDS,
  MAINTENANCEWORKORDER_QUERY_STRING_FIELDS,
  MAINTENANCEWORKORDER_QUERY_FIELDS,
  MAINTENANCEWORKORDER_SELF_I18N_KEY,
} from './composables/use-work-order-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useMaintenanceWorkOrderI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaintenanceWorkOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<MaintenanceWorkOrder[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<MaintenanceWorkOrderRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<MaintenanceWorkOrderRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<MaintenanceWorkOrder> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(MAINTENANCEWORKORDER_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof MAINTENANCEWORKORDER_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    maintenanceCategory: undefined as number | undefined,
    maintenanceType: undefined as number | undefined,
    workOrderStatus: undefined as number | undefined,
    priority: undefined as number | undefined,
    totalMaterialCost: undefined as number | undefined,
    totalLaborCost: undefined as number | undefined,
    totalOtherCost: undefined as number | undefined,
    totalCost: undefined as number | undefined,
    settlementStatus: undefined as number | undefined,
    maintenanceResult: undefined as number | undefined,
    maintenanceCycleDays: undefined as number | undefined,
    isHistoryArchived: undefined as number | undefined,
    approvalStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  MAINTENANCEWORKORDER_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'maintenanceWorkOrderId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideMaintenanceWorkOrderMasterContext()
const maintenanceWorkOrderMaterialPanelRef = ref<InstanceType<typeof MaintenanceWorkOrderMaterialPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaintenanceWorkOrderQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaintenanceWorkOrderQuery>): MaintenanceWorkOrderQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaintenanceWorkOrderQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaintenanceWorkOrderQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of MAINTENANCEWORKORDER_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.maintenanceCategory !== undefined && form.maintenanceCategory !== null) {
    query.maintenanceCategory = form.maintenanceCategory
  }
  if (form.maintenanceType !== undefined && form.maintenanceType !== null) {
    query.maintenanceType = form.maintenanceType
  }
  if (form.workOrderStatus !== undefined && form.workOrderStatus !== null) {
    query.workOrderStatus = form.workOrderStatus
  }
  if (form.priority !== undefined && form.priority !== null) {
    query.priority = form.priority
  }
  if (form.totalMaterialCost !== undefined && form.totalMaterialCost !== null) {
    query.totalMaterialCost = form.totalMaterialCost
  }
  if (form.totalLaborCost !== undefined && form.totalLaborCost !== null) {
    query.totalLaborCost = form.totalLaborCost
  }
  if (form.totalOtherCost !== undefined && form.totalOtherCost !== null) {
    query.totalOtherCost = form.totalOtherCost
  }
  if (form.totalCost !== undefined && form.totalCost !== null) {
    query.totalCost = form.totalCost
  }
  if (form.settlementStatus !== undefined && form.settlementStatus !== null) {
    query.settlementStatus = form.settlementStatus
  }
  if (form.maintenanceResult !== undefined && form.maintenanceResult !== null) {
    query.maintenanceResult = form.maintenanceResult
  }
  if (form.maintenanceCycleDays !== undefined && form.maintenanceCycleDays !== null) {
    query.maintenanceCycleDays = form.maintenanceCycleDays
  }
  if (form.isHistoryArchived !== undefined && form.isHistoryArchived !== null) {
    query.isHistoryArchived = form.isHistoryArchived
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: MaintenanceWorkOrderRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMaintenanceWorkOrderId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as MaintenanceWorkOrderRowRecord
  const key = getMaintenanceWorkOrderId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 加载主表详情并回填当前页 dataSource */
async function loadMaintenanceWorkOrderDetail(record: MaintenanceWorkOrderRowRecord): Promise<MaintenanceWorkOrder | null> {
  const id = getMaintenanceWorkOrderId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getMaintenanceWorkOrderById(id)
    const index = dataSource.value.findIndex((row) => getMaintenanceWorkOrderId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as MaintenanceWorkOrder
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'maintenanceWorkOrderId',
    key: 'maintenanceWorkOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceWorkOrderId') ?? ''
  },
  {
    title: pi.label('plantCode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'plantCode') ?? ''
  },
  {
    title: pi.label('workOrderCode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'workOrderCode') ?? ''
  },
  {
    title: pi.label('maintenanceNotificationId'),
    dataIndex: 'maintenanceNotificationId',
    key: 'maintenanceNotificationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceNotificationId') ?? ''
  },
  {
    title: pi.label('notificationCode'),
    dataIndex: 'notificationCode',
    key: 'notificationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'notificationCode') ?? ''
  },
  {
    title: pi.label('equipmentId'),
    dataIndex: 'equipmentId',
    key: 'equipmentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'equipmentId') ?? ''
  },
  {
    title: pi.label('equipmentCode'),
    dataIndex: 'equipmentCode',
    key: 'equipmentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'equipmentCode') ?? ''
  },
  {
    title: pi.label('equipmentName'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'equipmentName') ?? ''
  },
  {
    title: pi.label('maintenanceCategory'),
    dataIndex: 'maintenanceCategory',
    key: 'maintenanceCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('maintenanceType'),
    dataIndex: 'maintenanceType',
    key: 'maintenanceType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('workOrderStatus'),
    dataIndex: 'workOrderStatus',
    key: 'workOrderStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'priority') ?? ''
  },
  {
    title: pi.label('workCenter'),
    dataIndex: 'workCenter',
    key: 'workCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'workCenter') ?? ''
  },
  {
    title: pi.label('assignedTechnician'),
    dataIndex: 'assignedTechnician',
    key: 'assignedTechnician',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'assignedTechnician') ?? ''
  },
  {
    title: pi.label('maintenanceCompany'),
    dataIndex: 'maintenanceCompany',
    key: 'maintenanceCompany',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceCompany') ?? ''
  },
  {
    title: pi.label('plannedStartTime'),
    dataIndex: 'plannedStartTime',
    key: 'plannedStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'plannedStartTime') ?? ''
  },
  {
    title: pi.label('plannedEndTime'),
    dataIndex: 'plannedEndTime',
    key: 'plannedEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'plannedEndTime') ?? ''
  },
  {
    title: pi.label('actualStartTime'),
    dataIndex: 'actualStartTime',
    key: 'actualStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'actualStartTime') ?? ''
  },
  {
    title: pi.label('actualEndTime'),
    dataIndex: 'actualEndTime',
    key: 'actualEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'actualEndTime') ?? ''
  },
  {
    title: pi.label('faultDescription'),
    dataIndex: 'faultDescription',
    key: 'faultDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'faultDescription') ?? ''
  },
  {
    title: pi.label('maintenanceContent'),
    dataIndex: 'maintenanceContent',
    key: 'maintenanceContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceContent') ?? ''
  },
  {
    title: pi.label('solution'),
    dataIndex: 'solution',
    key: 'solution',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'solution') ?? ''
  },
  {
    title: pi.label('costCenterId'),
    dataIndex: 'costCenterId',
    key: 'costCenterId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'costCenterId') ?? ''
  },
  {
    title: pi.label('costCenterCode'),
    dataIndex: 'costCenterCode',
    key: 'costCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'costCenterCode') ?? ''
  },
  {
    title: pi.label('costElementId'),
    dataIndex: 'costElementId',
    key: 'costElementId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'costElementId') ?? ''
  },
  {
    title: pi.label('costElementCode'),
    dataIndex: 'costElementCode',
    key: 'costElementCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'costElementCode') ?? ''
  },
  {
    title: pi.label('totalMaterialCost'),
    dataIndex: 'totalMaterialCost',
    key: 'totalMaterialCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'totalMaterialCost') ?? ''
  },
  {
    title: pi.label('totalLaborCost'),
    dataIndex: 'totalLaborCost',
    key: 'totalLaborCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'totalLaborCost') ?? ''
  },
  {
    title: pi.label('totalOtherCost'),
    dataIndex: 'totalOtherCost',
    key: 'totalOtherCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'totalOtherCost') ?? ''
  },
  {
    title: pi.label('totalCost'),
    dataIndex: 'totalCost',
    key: 'totalCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'totalCost') ?? ''
  },
  {
    title: pi.label('settlementStatus'),
    dataIndex: 'settlementStatus',
    key: 'settlementStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'settlementStatus') ?? ''
  },
  {
    title: pi.label('settlementTime'),
    dataIndex: 'settlementTime',
    key: 'settlementTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'settlementTime') ?? ''
  },
  {
    title: pi.label('completedAt'),
    dataIndex: 'completedAt',
    key: 'completedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'completedAt') ?? ''
  },
  {
    title: pi.label('acceptedBy'),
    dataIndex: 'acceptedBy',
    key: 'acceptedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'acceptedBy') ?? ''
  },
  {
    title: pi.label('acceptedAt'),
    dataIndex: 'acceptedAt',
    key: 'acceptedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'acceptedAt') ?? ''
  },
  {
    title: pi.label('maintenanceResult'),
    dataIndex: 'maintenanceResult',
    key: 'maintenanceResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceResult') ?? ''
  },
  {
    title: pi.label('nextMaintenanceDate'),
    dataIndex: 'nextMaintenanceDate',
    key: 'nextMaintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'nextMaintenanceDate') ?? ''
  },
  {
    title: pi.label('maintenanceCycleDays'),
    dataIndex: 'maintenanceCycleDays',
    key: 'maintenanceCycleDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceCycleDays') ?? ''
  },
  {
    title: pi.label('maintenanceImages'),
    dataIndex: 'maintenanceImages',
    key: 'maintenanceImages',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceImages') ?? ''
  },
  {
    title: pi.label('maintenanceDocuments'),
    dataIndex: 'maintenanceDocuments',
    key: 'maintenanceDocuments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceDocuments') ?? ''
  },
  {
    title: pi.label('acceptedSummary'),
    dataIndex: 'acceptedSummary',
    key: 'acceptedSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'acceptedSummary') ?? ''
  },
  {
    title: pi.label('isHistoryArchived'),
    dataIndex: 'isHistoryArchived',
    key: 'isHistoryArchived',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:equipment:update',
        onClick: (record: MaintenanceWorkOrderRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:equipment:delete',
        onClick: (record: MaintenanceWorkOrderRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaintenanceWorkOrderId = (record: MaintenanceWorkOrderRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaintenanceWorkOrderField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getMaintenanceWorkOrderDictValue = (
  record: MaintenanceWorkOrderRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}



/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaintenanceWorkOrderRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: MaintenanceWorkOrderRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getMaintenanceWorkOrderId(selectedRow.value) === getMaintenanceWorkOrderId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaintenanceWorkOrderRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getMaintenanceWorkOrderList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[MaintenanceWorkOrder] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  plantCode: '',
  workOrderCode: '',
  maintenanceNotificationId: '',
  notificationCode: '',
  equipmentId: '',
  equipmentCode: '',
  equipmentName: '',
  maintenanceCategory: undefined as number | undefined,
  maintenanceType: undefined as number | undefined,
  workOrderStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  workCenter: '',
  assignedTechnician: '',
  maintenanceCompany: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  faultDescription: '',
  maintenanceContent: '',
  solution: '',
  costCenterId: '',
  costCenterCode: '',
  costElementId: '',
  costElementCode: '',
  totalMaterialCost: undefined as number | undefined,
  totalLaborCost: undefined as number | undefined,
  totalOtherCost: undefined as number | undefined,
  totalCost: undefined as number | undefined,
  settlementStatus: undefined as number | undefined,
  settlementTimeStart: '',
  settlementTimeEnd: '',
  completedAtStart: '',
  completedAtEnd: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  maintenanceResult: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceImages: '',
  maintenanceDocuments: '',
  acceptedSummary: '',
  isHistoryArchived: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: MaintenanceWorkOrderRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadMaintenanceWorkOrderDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}
/** 提交新增/编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateMaintenanceWorkOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createMaintenanceWorkOrder(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  maintenanceWorkOrderMaterialPanelRef.value?.reload?.()
    }
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getMaintenanceWorkOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importMaintenanceWorkOrder(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    maintenanceWorkOrderMaterialPanelRef.value?.reload?.()
      }
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportMaintenanceWorkOrder(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[MaintenanceWorkOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MaintenanceWorkOrderRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceWorkOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaintenanceWorkOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  plantCode: '',
  workOrderCode: '',
  maintenanceNotificationId: '',
  notificationCode: '',
  equipmentId: '',
  equipmentCode: '',
  equipmentName: '',
  maintenanceCategory: undefined as number | undefined,
  maintenanceType: undefined as number | undefined,
  workOrderStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  workCenter: '',
  assignedTechnician: '',
  maintenanceCompany: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  faultDescription: '',
  maintenanceContent: '',
  solution: '',
  costCenterId: '',
  costCenterCode: '',
  costElementId: '',
  costElementCode: '',
  totalMaterialCost: undefined as number | undefined,
  totalLaborCost: undefined as number | undefined,
  totalOtherCost: undefined as number | undefined,
  totalCost: undefined as number | undefined,
  settlementStatus: undefined as number | undefined,
  settlementTimeStart: '',
  settlementTimeEnd: '',
  completedAtStart: '',
  completedAtEnd: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  maintenanceResult: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceImages: '',
  maintenanceDocuments: '',
  acceptedSummary: '',
  isHistoryArchived: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
</script>
