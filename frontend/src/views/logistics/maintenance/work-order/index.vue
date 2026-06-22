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
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="logistics:maintenance:workorder:create"
      update-permission="logistics:maintenance:workorder:update"
      delete-permission="logistics:maintenance:workorder:delete"
      import-permission="logistics:maintenance:workorder:import"
      export-permission="logistics:maintenance:workorder:export"
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
      :master-total="total"
      master-entity-scope="approval"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'maintenanceCategory'">
          <TaktDictTag
            :value="getMaintenanceWorkOrderField(record, 'maintenanceCategory')"
            dict-type="logistics_maintenance_category"
          />
        </template>
        <template v-else-if="column.key === 'maintenanceType'">
          <TaktDictTag
            :value="getMaintenanceWorkOrderField(record, 'maintenanceType')"
            dict-type="logistics_maintenance_type"
          />
        </template>
        <template v-else-if="column.key === 'workOrderStatus'">
          <TaktDictTag
            :value="getMaintenanceWorkOrderField(record, 'workOrderStatus')"
            dict-type="sys_ticket_status"
          />
        </template>
        <template v-else-if="column.key === 'isHistoryArchived'">
          <TaktDictTag
            :value="getMaintenanceWorkOrderField(record, 'isHistoryArchived')"
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
      <a-form-item :label="t('entity.maintenanceworkorder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workOrderCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.workordercode')">
        <a-input
          v-model:value="advancedQueryForm.workOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.workordercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceNotificationId')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancenotificationid')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceNotificationId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancenotificationid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('notificationCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.notificationcode')">
        <a-input
          v-model:value="advancedQueryForm.notificationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.notificationcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentId')">
      <a-form-item :label="t('entity.maintenanceworkorder.equipmentid')">
        <a-input
          v-model:value="advancedQueryForm.equipmentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.equipmentcode')">
        <a-input
          v-model:value="advancedQueryForm.equipmentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentName')">
      <a-form-item :label="t('entity.maintenanceworkorder.equipmentname')">
        <a-input
          v-model:value="advancedQueryForm.equipmentName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentname') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCategory')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancecategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceCategory"
          dict-type="logistics_maintenance_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancecategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceType')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancetype')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceType"
          dict-type="logistics_maintenance_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancetype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workOrderStatus')">
      <a-form-item :label="t('entity.maintenanceworkorder.workorderstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.workOrderStatus"
          dict-type="sys_ticket_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.workorderstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priority')">
      <a-form-item :label="t('entity.maintenanceworkorder.priority')">
        <a-input-number
          v-model:value="advancedQueryForm.priority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.priority') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenter')">
      <a-form-item :label="t('entity.maintenanceworkorder.workcenter')">
        <a-input
          v-model:value="advancedQueryForm.workCenter"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.workcenter') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedTechnician')">
      <a-form-item :label="t('entity.maintenanceworkorder.assignedtechnician')">
        <a-input
          v-model:value="advancedQueryForm.assignedTechnician"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.assignedtechnician') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCompany')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancecompany')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceCompany"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancecompany') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.plannedstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.plannedstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.plannedendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.plannedendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.actualstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.actualstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.actualendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.actualendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('faultDescription')">
      <a-form-item :label="t('entity.maintenanceworkorder.faultdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.faultDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenanceworkorder.faultdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceContent')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancecontent')">
        <a-textarea
          v-model:value="advancedQueryForm.maintenanceContent"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenanceworkorder.maintenancecontent') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('solution')">
      <a-form-item :label="t('entity.maintenanceworkorder.solution')">
        <a-input
          v-model:value="advancedQueryForm.solution"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.solution') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterId')">
      <a-form-item :label="t('entity.maintenanceworkorder.costcenterid')">
        <a-input
          v-model:value="advancedQueryForm.costCenterId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costcenterid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.costcentercode')">
        <a-input
          v-model:value="advancedQueryForm.costCenterCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costcentercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementId')">
      <a-form-item :label="t('entity.maintenanceworkorder.costelementid')">
        <a-input
          v-model:value="advancedQueryForm.costElementId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costelementid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.costelementcode')">
        <a-input
          v-model:value="advancedQueryForm.costElementCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costelementcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalMaterialCost')">
      <a-form-item :label="t('entity.maintenanceworkorder.totalmaterialcost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalMaterialCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalmaterialcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalLaborCost')">
      <a-form-item :label="t('entity.maintenanceworkorder.totallaborcost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalLaborCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totallaborcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalOtherCost')">
      <a-form-item :label="t('entity.maintenanceworkorder.totalothercost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalOtherCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalothercost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalCost')">
      <a-form-item :label="t('entity.maintenanceworkorder.totalcost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementStatus')">
      <a-form-item :label="t('entity.maintenanceworkorder.settlementstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.settlementStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.settlementstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.settlementtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.settlementTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.settlementtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.settlementtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.settlementTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.settlementtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completedAtStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.completedatstart')">
        <a-input
          v-model:value="advancedQueryForm.completedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.completedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completedAtEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.completedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.completedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.completedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedBy')">
      <a-form-item :label="t('entity.maintenanceworkorder.acceptedby')">
        <a-input
          v-model:value="advancedQueryForm.acceptedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedby') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.acceptedatstart')">
        <a-input
          v-model:value="advancedQueryForm.acceptedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.acceptedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.acceptedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.acceptedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceResult')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenanceresult')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenanceresult') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.nextmaintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.nextmaintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.nextmaintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.nextmaintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCycleDays')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancecycledays')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceCycleDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancecycledays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceImages')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenanceimages')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenanceimages') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDocuments')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancedocuments')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceDocuments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancedocuments') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedSummary')">
      <a-form-item :label="t('entity.maintenanceworkorder.acceptedsummary')">
        <a-input
          v-model:value="advancedQueryForm.acceptedSummary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedsummary') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isHistoryArchived')">
      <a-form-item :label="t('entity.maintenanceworkorder.ishistoryarchived')">
        <TaktSelect
          v-model:value="advancedQueryForm.isHistoryArchived"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.ishistoryarchived') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.maintenanceworkorder.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.maintenanceworkorder.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.maintenanceworkorder.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.maintenanceworkorder.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.flowinstanceid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
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
            <span>{{ t('common.page.entity.extfield') }}</span>
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
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.maintenanceworkorder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.maintenanceworkorder._self"
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
      table-mode="single"
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
import { provideMaintenanceWorkOrderMasterContext } from './composables/use-work-order-master-context'
import { getMaintenanceWorkOrderList, getMaintenanceWorkOrderById, createMaintenanceWorkOrder, updateMaintenanceWorkOrder, deleteMaintenanceWorkOrderById, deleteMaintenanceWorkOrderBatch, getMaintenanceWorkOrderTemplate, importMaintenanceWorkOrder, exportMaintenanceWorkOrder, updateMaintenanceWorkOrderStatus } from '@/api/logistics/maintenance/work-order'
import type { MaintenanceWorkOrder, MaintenanceWorkOrderQuery } from '@/types/logistics/maintenance/work-order'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaintenanceWorkOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.maintenanceworkorder._self') })
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
const selectedRow = ref<MaintenanceWorkOrder | null>(null)
/** 表格多选行 */
const selectedRows = ref<MaintenanceWorkOrder[]>([])
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
/** 高级查询表单模型 */
const advancedQueryForm = ref({
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
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.maintenanceworkorder.plantcode') },
  { key: 'workOrderCode', label: t('entity.maintenanceworkorder.workordercode') },
  { key: 'maintenanceNotificationId', label: t('entity.maintenanceworkorder.maintenancenotificationid') },
  { key: 'notificationCode', label: t('entity.maintenanceworkorder.notificationcode') },
  { key: 'equipmentId', label: t('entity.maintenanceworkorder.equipmentid') },
  { key: 'equipmentCode', label: t('entity.maintenanceworkorder.equipmentcode') },
  { key: 'equipmentName', label: t('entity.maintenanceworkorder.equipmentname') },
  { key: 'maintenanceCategory', label: t('entity.maintenanceworkorder.maintenancecategory') },
  { key: 'maintenanceType', label: t('entity.maintenanceworkorder.maintenancetype') },
  { key: 'workOrderStatus', label: t('entity.maintenanceworkorder.workorderstatus') },
  { key: 'priority', label: t('entity.maintenanceworkorder.priority') },
  { key: 'workCenter', label: t('entity.maintenanceworkorder.workcenter') },
  { key: 'assignedTechnician', label: t('entity.maintenanceworkorder.assignedtechnician') },
  { key: 'maintenanceCompany', label: t('entity.maintenanceworkorder.maintenancecompany') },
  { key: 'plannedStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.plannedstarttime')) },
  { key: 'plannedStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.plannedstarttime')) },
  { key: 'plannedEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.plannedendtime')) },
  { key: 'plannedEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.plannedendtime')) },
  { key: 'actualStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.actualstarttime')) },
  { key: 'actualStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.actualstarttime')) },
  { key: 'actualEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.actualendtime')) },
  { key: 'actualEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.actualendtime')) },
  { key: 'faultDescription', label: t('entity.maintenanceworkorder.faultdescription') },
  { key: 'maintenanceContent', label: t('entity.maintenanceworkorder.maintenancecontent') },
  { key: 'solution', label: t('entity.maintenanceworkorder.solution') },
  { key: 'costCenterId', label: t('entity.maintenanceworkorder.costcenterid') },
  { key: 'costCenterCode', label: t('entity.maintenanceworkorder.costcentercode') },
  { key: 'costElementId', label: t('entity.maintenanceworkorder.costelementid') },
  { key: 'costElementCode', label: t('entity.maintenanceworkorder.costelementcode') },
  { key: 'totalMaterialCost', label: t('entity.maintenanceworkorder.totalmaterialcost') },
  { key: 'totalLaborCost', label: t('entity.maintenanceworkorder.totallaborcost') },
  { key: 'totalOtherCost', label: t('entity.maintenanceworkorder.totalothercost') },
  { key: 'totalCost', label: t('entity.maintenanceworkorder.totalcost') },
  { key: 'settlementStatus', label: t('entity.maintenanceworkorder.settlementstatus') },
  { key: 'settlementTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.settlementtime')) },
  { key: 'settlementTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.settlementtime')) },
  { key: 'completedAtStart', label: t('entity.maintenanceworkorder.completedatstart') },
  { key: 'completedAtEnd', label: t('entity.maintenanceworkorder.completedatend') },
  { key: 'acceptedBy', label: t('entity.maintenanceworkorder.acceptedby') },
  { key: 'acceptedAtStart', label: t('entity.maintenanceworkorder.acceptedatstart') },
  { key: 'acceptedAtEnd', label: t('entity.maintenanceworkorder.acceptedatend') },
  { key: 'maintenanceResult', label: t('entity.maintenanceworkorder.maintenanceresult') },
  { key: 'nextMaintenanceDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.nextmaintenancedate')) },
  { key: 'nextMaintenanceDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.nextmaintenancedate')) },
  { key: 'maintenanceCycleDays', label: t('entity.maintenanceworkorder.maintenancecycledays') },
  { key: 'maintenanceImages', label: t('entity.maintenanceworkorder.maintenanceimages') },
  { key: 'maintenanceDocuments', label: t('entity.maintenanceworkorder.maintenancedocuments') },
  { key: 'acceptedSummary', label: t('entity.maintenanceworkorder.acceptedsummary') },
  { key: 'isHistoryArchived', label: t('entity.maintenanceworkorder.ishistoryarchived') },
  { key: 'approvalStatus', label: t('entity.maintenanceworkorder.approvalstatus') },
  { key: 'initiatorId', label: t('entity.maintenanceworkorder.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.maintenanceworkorder.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.maintenanceworkorder.initiatedatend') },
  { key: 'approvedBy', label: t('entity.maintenanceworkorder.approvedby') },
  { key: 'approvedAtStart', label: t('entity.maintenanceworkorder.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.maintenanceworkorder.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.maintenanceworkorder.flowinstanceid') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
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
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('workOrderCode', form.workOrderCode)
  assignTrimmed('maintenanceNotificationId', form.maintenanceNotificationId)
  assignTrimmed('notificationCode', form.notificationCode)
  assignTrimmed('equipmentId', form.equipmentId)
  assignTrimmed('equipmentCode', form.equipmentCode)
  assignTrimmed('equipmentName', form.equipmentName)
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
  assignTrimmed('workCenter', form.workCenter)
  assignTrimmed('assignedTechnician', form.assignedTechnician)
  assignTrimmed('maintenanceCompany', form.maintenanceCompany)
  assignTrimmed('plannedStartTimeStart', form.plannedStartTimeStart)
  assignTrimmed('plannedStartTimeEnd', form.plannedStartTimeEnd)
  assignTrimmed('plannedEndTimeStart', form.plannedEndTimeStart)
  assignTrimmed('plannedEndTimeEnd', form.plannedEndTimeEnd)
  assignTrimmed('actualStartTimeStart', form.actualStartTimeStart)
  assignTrimmed('actualStartTimeEnd', form.actualStartTimeEnd)
  assignTrimmed('actualEndTimeStart', form.actualEndTimeStart)
  assignTrimmed('actualEndTimeEnd', form.actualEndTimeEnd)
  assignTrimmed('faultDescription', form.faultDescription)
  assignTrimmed('maintenanceContent', form.maintenanceContent)
  assignTrimmed('solution', form.solution)
  assignTrimmed('costCenterId', form.costCenterId)
  assignTrimmed('costCenterCode', form.costCenterCode)
  assignTrimmed('costElementId', form.costElementId)
  assignTrimmed('costElementCode', form.costElementCode)
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
  assignTrimmed('settlementTimeStart', form.settlementTimeStart)
  assignTrimmed('settlementTimeEnd', form.settlementTimeEnd)
  assignTrimmed('completedAtStart', form.completedAtStart)
  assignTrimmed('completedAtEnd', form.completedAtEnd)
  assignTrimmed('acceptedBy', form.acceptedBy)
  assignTrimmed('acceptedAtStart', form.acceptedAtStart)
  assignTrimmed('acceptedAtEnd', form.acceptedAtEnd)
  if (form.maintenanceResult !== undefined && form.maintenanceResult !== null) {
    query.maintenanceResult = form.maintenanceResult
  }
  assignTrimmed('nextMaintenanceDateStart', form.nextMaintenanceDateStart)
  assignTrimmed('nextMaintenanceDateEnd', form.nextMaintenanceDateEnd)
  if (form.maintenanceCycleDays !== undefined && form.maintenanceCycleDays !== null) {
    query.maintenanceCycleDays = form.maintenanceCycleDays
  }
  assignTrimmed('maintenanceImages', form.maintenanceImages)
  assignTrimmed('maintenanceDocuments', form.maintenanceDocuments)
  assignTrimmed('acceptedSummary', form.acceptedSummary)
  if (form.isHistoryArchived !== undefined && form.isHistoryArchived !== null) {
    query.isHistoryArchived = form.isHistoryArchived
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
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
function syncMasterSelection(record: MaintenanceWorkOrder | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMaintenanceWorkOrderId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as MaintenanceWorkOrder
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
async function loadMaintenanceWorkOrderDetail(record: MaintenanceWorkOrder): Promise<MaintenanceWorkOrder | null> {
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
    title: t('entity.maintenanceworkorder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.workordercode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'workOrderCode') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.maintenancenotificationid'),
    dataIndex: 'maintenanceNotificationId',
    key: 'maintenanceNotificationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceNotificationId') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.notificationcode'),
    dataIndex: 'notificationCode',
    key: 'notificationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'notificationCode') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.equipmentid'),
    dataIndex: 'equipmentId',
    key: 'equipmentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'equipmentId') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.equipmentcode'),
    dataIndex: 'equipmentCode',
    key: 'equipmentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'equipmentCode') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.equipmentname'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'equipmentName') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.maintenancecategory'),
    dataIndex: 'maintenanceCategory',
    key: 'maintenanceCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.maintenanceworkorder.maintenancetype'),
    dataIndex: 'maintenanceType',
    key: 'maintenanceType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.maintenanceworkorder.workorderstatus'),
    dataIndex: 'workOrderStatus',
    key: 'workOrderStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.maintenanceworkorder.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'priority') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.workcenter'),
    dataIndex: 'workCenter',
    key: 'workCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'workCenter') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.assignedtechnician'),
    dataIndex: 'assignedTechnician',
    key: 'assignedTechnician',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'assignedTechnician') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.maintenancecompany'),
    dataIndex: 'maintenanceCompany',
    key: 'maintenanceCompany',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceCompany') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.plannedstarttime'),
    dataIndex: 'plannedStartTime',
    key: 'plannedStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'plannedStartTime') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.plannedendtime'),
    dataIndex: 'plannedEndTime',
    key: 'plannedEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'plannedEndTime') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.actualstarttime'),
    dataIndex: 'actualStartTime',
    key: 'actualStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'actualStartTime') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.actualendtime'),
    dataIndex: 'actualEndTime',
    key: 'actualEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'actualEndTime') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.faultdescription'),
    dataIndex: 'faultDescription',
    key: 'faultDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'faultDescription') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.maintenancecontent'),
    dataIndex: 'maintenanceContent',
    key: 'maintenanceContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceContent') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.solution'),
    dataIndex: 'solution',
    key: 'solution',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'solution') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.costcenterid'),
    dataIndex: 'costCenterId',
    key: 'costCenterId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'costCenterId') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.costcentercode'),
    dataIndex: 'costCenterCode',
    key: 'costCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'costCenterCode') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.costelementid'),
    dataIndex: 'costElementId',
    key: 'costElementId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'costElementId') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.costelementcode'),
    dataIndex: 'costElementCode',
    key: 'costElementCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'costElementCode') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.totalmaterialcost'),
    dataIndex: 'totalMaterialCost',
    key: 'totalMaterialCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'totalMaterialCost') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.totallaborcost'),
    dataIndex: 'totalLaborCost',
    key: 'totalLaborCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'totalLaborCost') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.totalothercost'),
    dataIndex: 'totalOtherCost',
    key: 'totalOtherCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'totalOtherCost') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.totalcost'),
    dataIndex: 'totalCost',
    key: 'totalCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'totalCost') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.settlementstatus'),
    dataIndex: 'settlementStatus',
    key: 'settlementStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'settlementStatus') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.settlementtime'),
    dataIndex: 'settlementTime',
    key: 'settlementTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'settlementTime') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.completedat'),
    dataIndex: 'completedAt',
    key: 'completedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'completedAt') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.acceptedby'),
    dataIndex: 'acceptedBy',
    key: 'acceptedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'acceptedBy') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.acceptedat'),
    dataIndex: 'acceptedAt',
    key: 'acceptedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'acceptedAt') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.maintenanceresult'),
    dataIndex: 'maintenanceResult',
    key: 'maintenanceResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceResult') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.nextmaintenancedate'),
    dataIndex: 'nextMaintenanceDate',
    key: 'nextMaintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'nextMaintenanceDate') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.maintenancecycledays'),
    dataIndex: 'maintenanceCycleDays',
    key: 'maintenanceCycleDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceCycleDays') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.maintenanceimages'),
    dataIndex: 'maintenanceImages',
    key: 'maintenanceImages',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceImages') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.maintenancedocuments'),
    dataIndex: 'maintenanceDocuments',
    key: 'maintenanceDocuments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceDocuments') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.acceptedsummary'),
    dataIndex: 'acceptedSummary',
    key: 'acceptedSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'acceptedSummary') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.ishistoryarchived'),
    dataIndex: 'isHistoryArchived',
    key: 'isHistoryArchived',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.maintenanceworkorder.maintenancenotification'),
    dataIndex: 'maintenanceNotification',
    key: 'maintenanceNotification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'maintenanceNotification') ?? ''
  },
  {
    title: t('entity.maintenanceworkorder.equipment'),
    dataIndex: 'equipment',
    key: 'equipment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceWorkOrderField(record, 'equipment') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:workorder:update',
        onClick: (record: MaintenanceWorkOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:workorder:delete',
        onClick: (record: MaintenanceWorkOrder) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaintenanceWorkOrderId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaintenanceWorkOrderField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaintenanceWorkOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: MaintenanceWorkOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getMaintenanceWorkOrderId(selectedRow.value) === getMaintenanceWorkOrderId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaintenanceWorkOrder[]) => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.maintenanceworkorder._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: MaintenanceWorkOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.maintenanceworkorder._self') })
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.maintenanceworkorder._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.maintenanceworkorder._self') }))
    } else {
      await createMaintenanceWorkOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.maintenanceworkorder._self') }))
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

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaintenanceWorkOrder(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
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
    message.success(t('common.feedback.export.success', { target: t('entity.maintenanceworkorder._self') }))
  } catch (error: any) {
    logger.error('[MaintenanceWorkOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.maintenanceworkorder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MaintenanceWorkOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.maintenanceworkorder._self'), name: t('common.tip.this.target', { target: t('entity.maintenanceworkorder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceWorkOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.maintenanceworkorder._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.maintenanceworkorder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.maintenanceworkorder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaintenanceWorkOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.maintenanceworkorder._self') }))
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
