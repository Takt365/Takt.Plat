<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-plant-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt工厂物料实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:materials:material:plant:change:log:create"
      update-permission="logistics:materials:material:plant:change:log:update"
      delete-permission="logistics:materials:material:plant:change:log:delete"
      import-permission="logistics:materials:material:plant:change:log:import"
      export-permission="logistics:materials:material:plant:change:log:export"
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
      :master-row-key="getMaterialPlantId"
      :master-row-selection="rowSelection"
      master-id-column-key="materialPlantId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'materialStatus'">
          <a-switch
            :checked="getMaterialPlantField(record, 'materialStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleMaterialStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
      <template #detail>
        <MaterialPlantChangeLogPanel
          ref="materialPlantChangeLogPanelRef"
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
      <MaterialPlantForm
        :key="formData?.materialPlantId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-materials-material-plant-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.materialplant.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.plantcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.materialplant.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.materialplant.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialname') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.materialplant.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialspecification') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="t('entity.materialplant.materialdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.materialplant.materialdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="t('entity.materialplant.industrysector')">
        <a-input
          v-model:value="advancedQueryForm.industrySector"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.industrysector') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialHierarchy')">
      <a-form-item :label="t('entity.materialplant.materialhierarchy')">
        <a-input
          v-model:value="advancedQueryForm.materialHierarchy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialhierarchy') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialGroupCode')">
      <a-form-item :label="t('entity.materialplant.materialgroupcode')">
        <a-input
          v-model:value="advancedQueryForm.materialGroupCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialgroupcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialType')">
      <a-form-item :label="t('entity.materialplant.materialtype')">
        <a-input-number
          v-model:value="advancedQueryForm.materialType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialModel')">
      <a-form-item :label="t('entity.materialplant.materialmodel')">
        <a-input
          v-model:value="advancedQueryForm.materialModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialmodel') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialBrand')">
      <a-form-item :label="t('entity.materialplant.materialbrand')">
        <a-input
          v-model:value="advancedQueryForm.materialBrand"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialbrand') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="t('entity.materialplant.baseunit')">
        <a-input
          v-model:value="advancedQueryForm.baseUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.baseunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseGroup')">
      <a-form-item :label="t('entity.materialplant.purchasegroup')">
        <a-input
          v-model:value="advancedQueryForm.purchaseGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.purchasegroup') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseType')">
      <a-form-item :label="t('entity.materialplant.purchasetype')">
        <a-input-number
          v-model:value="advancedQueryForm.purchaseType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.purchasetype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('specialProcurement')">
      <a-form-item :label="t('entity.materialplant.specialprocurement')">
        <a-input-number
          v-model:value="advancedQueryForm.specialProcurement"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.specialprocurement') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBulk')">
      <a-form-item :label="t('entity.materialplant.isbulk')">
        <a-input-number
          v-model:value="advancedQueryForm.isBulk"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.isbulk') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('minOrderQuantity')">
      <a-form-item :label="t('entity.materialplant.minorderquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.minOrderQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.minorderquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('roundingValue')">
      <a-form-item :label="t('entity.materialplant.roundingvalue')">
        <a-input-number
          v-model:value="advancedQueryForm.roundingValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.roundingvalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedDeliveryTimeDays')">
      <a-form-item :label="t('entity.materialplant.planneddeliverytimedays')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedDeliveryTimeDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.planneddeliverytimedays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inHouseProductionDays')">
      <a-form-item :label="t('entity.materialplant.inhouseproductiondays')">
        <a-input-number
          v-model:value="advancedQueryForm.inHouseProductionDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.inhouseproductiondays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturer')">
      <a-form-item :label="t('entity.materialplant.manufacturer')">
        <a-input
          v-model:value="advancedQueryForm.manufacturer"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.manufacturer') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPartNumber')">
      <a-form-item :label="t('entity.materialplant.manufacturerpartnumber')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerPartNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.manufacturerpartnumber') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="t('entity.materialplant.currencycode')">
        <a-input
          v-model:value="advancedQueryForm.currencyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.currencycode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceControl')">
      <a-form-item :label="t('entity.materialplant.pricecontrol')">
        <a-input-number
          v-model:value="advancedQueryForm.priceControl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.pricecontrol') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceUnit')">
      <a-form-item :label="t('entity.materialplant.priceunit')">
        <a-input-number
          v-model:value="advancedQueryForm.priceUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.priceunit') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('valuationCategory')">
      <a-form-item :label="t('entity.materialplant.valuationcategory')">
        <a-input
          v-model:value="advancedQueryForm.valuationCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.valuationcategory') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('differenceCode')">
      <a-form-item :label="t('entity.materialplant.differencecode')">
        <a-input
          v-model:value="advancedQueryForm.differenceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.differencecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenter')">
      <a-form-item :label="t('entity.materialplant.profitcenter')">
        <a-input
          v-model:value="advancedQueryForm.profitCenter"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.profitcenter') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('latestPurchasePrice')">
      <a-form-item :label="t('entity.materialplant.latestpurchaseprice')">
        <a-input-number
          v-model:value="advancedQueryForm.latestPurchasePrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.latestpurchaseprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPrice')">
      <a-form-item :label="t('entity.materialplant.salesprice')">
        <a-input-number
          v-model:value="advancedQueryForm.salesPrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.salesprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('safetyStock')">
      <a-form-item :label="t('entity.materialplant.safetystock')">
        <a-input-number
          v-model:value="advancedQueryForm.safetyStock"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.safetystock') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maxStock')">
      <a-form-item :label="t('entity.materialplant.maxstock')">
        <a-input-number
          v-model:value="advancedQueryForm.maxStock"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.maxstock') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('minStock')">
      <a-form-item :label="t('entity.materialplant.minstock')">
        <a-input-number
          v-model:value="advancedQueryForm.minStock"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.minstock') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currentStock')">
      <a-form-item :label="t('entity.materialplant.currentstock')">
        <a-input-number
          v-model:value="advancedQueryForm.currentStock"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.currentstock') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLocation')">
      <a-form-item :label="t('entity.materialplant.productionlocation')">
        <a-input
          v-model:value="advancedQueryForm.productionLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.productionlocation') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasingLocation')">
      <a-form-item :label="t('entity.materialplant.purchasinglocation')">
        <a-input
          v-model:value="advancedQueryForm.purchasingLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.purchasinglocation') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionRequired')">
      <a-form-item :label="t('entity.materialplant.inspectionrequired')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionRequired"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.inspectionrequired') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBatch')">
      <a-form-item :label="t('entity.materialplant.isbatch')">
        <a-input-number
          v-model:value="advancedQueryForm.isBatch"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.isbatch') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isExpiry')">
      <a-form-item :label="t('entity.materialplant.isexpiry')">
        <a-input-number
          v-model:value="advancedQueryForm.isExpiry"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.isexpiry') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDays')">
      <a-form-item :label="t('entity.materialplant.expirydays')">
        <a-input-number
          v-model:value="advancedQueryForm.expiryDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.expirydays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialStatus')">
      <a-form-item :label="t('entity.materialplant.materialstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.materialstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialAttributes')">
      <a-form-item :label="t('entity.materialplant.materialattributes')">
        <a-input
          v-model:value="advancedQueryForm.materialAttributes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialattributes') })"
          show-count
          :maxlength="4000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEndOfLife')">
      <a-form-item :label="t('entity.materialplant.isendoflife')">
        <a-input
          v-model:value="advancedQueryForm.isEndOfLife"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.isendoflife') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endOfLifeDateStart')">
      <a-form-item :label="t('entity.materialplant.endoflifedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endOfLifeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.endoflifedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endOfLifeDateEnd')">
      <a-form-item :label="t('entity.materialplant.endoflifedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.endOfLifeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.endoflifedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
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
      :title="t('common.dialog.title.import', { entity: t('entity.materialplant._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.materialplant._self"
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
      :id-column-key="'materialPlantId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt工厂物料实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-plant-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import MaterialPlantForm from './components/material-plant-form.vue'
import MaterialPlantChangeLogPanel from './components/material-plant-change-log-panel.vue'
import { provideMaterialPlantMasterContext } from './composables/use-material-plant-master-context'
import { getMaterialPlantList, getMaterialPlantById, createMaterialPlant, updateMaterialPlant, deleteMaterialPlantById, deleteMaterialPlantBatch, getMaterialPlantTemplate, importMaterialPlant, exportMaterialPlant, updateMaterialPlantStatus } from '@/api/logistics/materials/material-plant'
import type { MaterialPlant, MaterialPlantQuery } from '@/types/logistics/materials/material-plant'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterialPlant')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.materialplant._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<MaterialPlant[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<MaterialPlant | null>(null)
/** 表格多选行 */
const selectedRows = ref<MaterialPlant[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<MaterialPlant> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  purchaseGroup: '',
  purchaseType: undefined as number | undefined,
  specialProcurement: undefined as number | undefined,
  isBulk: undefined as number | undefined,
  minOrderQuantity: undefined as number | undefined,
  roundingValue: undefined as number | undefined,
  plannedDeliveryTimeDays: undefined as number | undefined,
  inHouseProductionDays: undefined as number | undefined,
  manufacturer: '',
  manufacturerPartNumber: '',
  currencyCode: '',
  priceControl: undefined as number | undefined,
  priceUnit: undefined as number | undefined,
  valuationCategory: '',
  differenceCode: '',
  profitCenter: '',
  latestPurchasePrice: undefined as number | undefined,
  salesPrice: undefined as number | undefined,
  safetyStock: undefined as number | undefined,
  maxStock: undefined as number | undefined,
  minStock: undefined as number | undefined,
  currentStock: undefined as number | undefined,
  productionLocation: '',
  purchasingLocation: '',
  inspectionRequired: undefined as number | undefined,
  isBatch: undefined as number | undefined,
  isExpiry: undefined as number | undefined,
  expiryDays: undefined as number | undefined,
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.materialplant.plantcode') },
  { key: 'materialCode', label: t('entity.materialplant.materialcode') },
  { key: 'materialName', label: t('entity.materialplant.materialname') },
  { key: 'materialSpecification', label: t('entity.materialplant.materialspecification') },
  { key: 'materialDescription', label: t('entity.materialplant.materialdescription') },
  { key: 'industrySector', label: t('entity.materialplant.industrysector') },
  { key: 'materialHierarchy', label: t('entity.materialplant.materialhierarchy') },
  { key: 'materialGroupCode', label: t('entity.materialplant.materialgroupcode') },
  { key: 'materialType', label: t('entity.materialplant.materialtype') },
  { key: 'materialModel', label: t('entity.materialplant.materialmodel') },
  { key: 'materialBrand', label: t('entity.materialplant.materialbrand') },
  { key: 'baseUnit', label: t('entity.materialplant.baseunit') },
  { key: 'purchaseGroup', label: t('entity.materialplant.purchasegroup') },
  { key: 'purchaseType', label: t('entity.materialplant.purchasetype') },
  { key: 'specialProcurement', label: t('entity.materialplant.specialprocurement') },
  { key: 'isBulk', label: t('entity.materialplant.isbulk') },
  { key: 'minOrderQuantity', label: t('entity.materialplant.minorderquantity') },
  { key: 'roundingValue', label: t('entity.materialplant.roundingvalue') },
  { key: 'plannedDeliveryTimeDays', label: t('entity.materialplant.planneddeliverytimedays') },
  { key: 'inHouseProductionDays', label: t('entity.materialplant.inhouseproductiondays') },
  { key: 'manufacturer', label: t('entity.materialplant.manufacturer') },
  { key: 'manufacturerPartNumber', label: t('entity.materialplant.manufacturerpartnumber') },
  { key: 'currencyCode', label: t('entity.materialplant.currencycode') },
  { key: 'priceControl', label: t('entity.materialplant.pricecontrol') },
  { key: 'priceUnit', label: t('entity.materialplant.priceunit') },
  { key: 'valuationCategory', label: t('entity.materialplant.valuationcategory') },
  { key: 'differenceCode', label: t('entity.materialplant.differencecode') },
  { key: 'profitCenter', label: t('entity.materialplant.profitcenter') },
  { key: 'latestPurchasePrice', label: t('entity.materialplant.latestpurchaseprice') },
  { key: 'salesPrice', label: t('entity.materialplant.salesprice') },
  { key: 'safetyStock', label: t('entity.materialplant.safetystock') },
  { key: 'maxStock', label: t('entity.materialplant.maxstock') },
  { key: 'minStock', label: t('entity.materialplant.minstock') },
  { key: 'currentStock', label: t('entity.materialplant.currentstock') },
  { key: 'productionLocation', label: t('entity.materialplant.productionlocation') },
  { key: 'purchasingLocation', label: t('entity.materialplant.purchasinglocation') },
  { key: 'inspectionRequired', label: t('entity.materialplant.inspectionrequired') },
  { key: 'isBatch', label: t('entity.materialplant.isbatch') },
  { key: 'isExpiry', label: t('entity.materialplant.isexpiry') },
  { key: 'expiryDays', label: t('entity.materialplant.expirydays') },
  { key: 'materialStatus', label: t('entity.materialplant.materialstatus') },
  { key: 'materialAttributes', label: t('entity.materialplant.materialattributes') },
  { key: 'isEndOfLife', label: t('entity.materialplant.isendoflife') },
  { key: 'endOfLifeDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.materialplant.endoflifedate')) },
  { key: 'endOfLifeDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.materialplant.endoflifedate')) },
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
const entityIdName = 'materialPlantId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideMaterialPlantMasterContext()
const materialPlantChangeLogPanelRef = ref<InstanceType<typeof MaterialPlantChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaterialPlantQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialPlantQuery>): MaterialPlantQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialPlantQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialPlantQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('materialSpecification', form.materialSpecification)
  assignTrimmed('materialDescription', form.materialDescription)
  assignTrimmed('industrySector', form.industrySector)
  assignTrimmed('materialHierarchy', form.materialHierarchy)
  assignTrimmed('materialGroupCode', form.materialGroupCode)
  if (form.materialType !== undefined && form.materialType !== null) {
    query.materialType = form.materialType
  }
  assignTrimmed('materialModel', form.materialModel)
  assignTrimmed('materialBrand', form.materialBrand)
  assignTrimmed('baseUnit', form.baseUnit)
  assignTrimmed('purchaseGroup', form.purchaseGroup)
  if (form.purchaseType !== undefined && form.purchaseType !== null) {
    query.purchaseType = form.purchaseType
  }
  if (form.specialProcurement !== undefined && form.specialProcurement !== null) {
    query.specialProcurement = form.specialProcurement
  }
  if (form.isBulk !== undefined && form.isBulk !== null) {
    query.isBulk = form.isBulk
  }
  if (form.minOrderQuantity !== undefined && form.minOrderQuantity !== null) {
    query.minOrderQuantity = form.minOrderQuantity
  }
  if (form.roundingValue !== undefined && form.roundingValue !== null) {
    query.roundingValue = form.roundingValue
  }
  if (form.plannedDeliveryTimeDays !== undefined && form.plannedDeliveryTimeDays !== null) {
    query.plannedDeliveryTimeDays = form.plannedDeliveryTimeDays
  }
  if (form.inHouseProductionDays !== undefined && form.inHouseProductionDays !== null) {
    query.inHouseProductionDays = form.inHouseProductionDays
  }
  assignTrimmed('manufacturer', form.manufacturer)
  assignTrimmed('manufacturerPartNumber', form.manufacturerPartNumber)
  assignTrimmed('currencyCode', form.currencyCode)
  if (form.priceControl !== undefined && form.priceControl !== null) {
    query.priceControl = form.priceControl
  }
  if (form.priceUnit !== undefined && form.priceUnit !== null) {
    query.priceUnit = form.priceUnit
  }
  assignTrimmed('valuationCategory', form.valuationCategory)
  assignTrimmed('differenceCode', form.differenceCode)
  assignTrimmed('profitCenter', form.profitCenter)
  if (form.latestPurchasePrice !== undefined && form.latestPurchasePrice !== null) {
    query.latestPurchasePrice = form.latestPurchasePrice
  }
  if (form.salesPrice !== undefined && form.salesPrice !== null) {
    query.salesPrice = form.salesPrice
  }
  if (form.safetyStock !== undefined && form.safetyStock !== null) {
    query.safetyStock = form.safetyStock
  }
  if (form.maxStock !== undefined && form.maxStock !== null) {
    query.maxStock = form.maxStock
  }
  if (form.minStock !== undefined && form.minStock !== null) {
    query.minStock = form.minStock
  }
  if (form.currentStock !== undefined && form.currentStock !== null) {
    query.currentStock = form.currentStock
  }
  assignTrimmed('productionLocation', form.productionLocation)
  assignTrimmed('purchasingLocation', form.purchasingLocation)
  if (form.inspectionRequired !== undefined && form.inspectionRequired !== null) {
    query.inspectionRequired = form.inspectionRequired
  }
  if (form.isBatch !== undefined && form.isBatch !== null) {
    query.isBatch = form.isBatch
  }
  if (form.isExpiry !== undefined && form.isExpiry !== null) {
    query.isExpiry = form.isExpiry
  }
  if (form.expiryDays !== undefined && form.expiryDays !== null) {
    query.expiryDays = form.expiryDays
  }
  if (form.materialStatus !== undefined && form.materialStatus !== null) {
    query.materialStatus = form.materialStatus
  }
  assignTrimmed('materialAttributes', form.materialAttributes)
  assignTrimmed('isEndOfLife', form.isEndOfLife)
  assignTrimmed('endOfLifeDateStart', form.endOfLifeDateStart)
  assignTrimmed('endOfLifeDateEnd', form.endOfLifeDateEnd)
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
function syncMasterSelection(record: MaterialPlant | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMaterialPlantId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as MaterialPlant
  const key = getMaterialPlantId(row)
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
async function loadMaterialPlantDetail(record: MaterialPlant): Promise<MaterialPlant | null> {
  const id = getMaterialPlantId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getMaterialPlantById(id)
    const index = dataSource.value.findIndex((row) => getMaterialPlantId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as MaterialPlant
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
    dataIndex: 'materialPlantId',
    key: 'materialPlantId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialPlantId') ?? ''
  },
  {
    title: t('entity.materialplant.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.materialplant.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.materialplant.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialName') ?? ''
  },
  {
    title: t('entity.materialplant.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialSpecification') ?? ''
  },
  {
    title: t('entity.materialplant.materialdescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialDescription') ?? ''
  },
  {
    title: t('entity.materialplant.industrysector'),
    dataIndex: 'industrySector',
    key: 'industrySector',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'industrySector') ?? ''
  },
  {
    title: t('entity.materialplant.materialhierarchy'),
    dataIndex: 'materialHierarchy',
    key: 'materialHierarchy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialHierarchy') ?? ''
  },
  {
    title: t('entity.materialplant.materialgroupcode'),
    dataIndex: 'materialGroupCode',
    key: 'materialGroupCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialGroupCode') ?? ''
  },
  {
    title: t('entity.materialplant.materialtype'),
    dataIndex: 'materialType',
    key: 'materialType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialType') ?? ''
  },
  {
    title: t('entity.materialplant.materialmodel'),
    dataIndex: 'materialModel',
    key: 'materialModel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialModel') ?? ''
  },
  {
    title: t('entity.materialplant.materialbrand'),
    dataIndex: 'materialBrand',
    key: 'materialBrand',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialBrand') ?? ''
  },
  {
    title: t('entity.materialplant.baseunit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'baseUnit') ?? ''
  },
  {
    title: t('entity.materialplant.purchasegroup'),
    dataIndex: 'purchaseGroup',
    key: 'purchaseGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'purchaseGroup') ?? ''
  },
  {
    title: t('entity.materialplant.purchasetype'),
    dataIndex: 'purchaseType',
    key: 'purchaseType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'purchaseType') ?? ''
  },
  {
    title: t('entity.materialplant.specialprocurement'),
    dataIndex: 'specialProcurement',
    key: 'specialProcurement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'specialProcurement') ?? ''
  },
  {
    title: t('entity.materialplant.isbulk'),
    dataIndex: 'isBulk',
    key: 'isBulk',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'isBulk') ?? ''
  },
  {
    title: t('entity.materialplant.minorderquantity'),
    dataIndex: 'minOrderQuantity',
    key: 'minOrderQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'minOrderQuantity') ?? ''
  },
  {
    title: t('entity.materialplant.roundingvalue'),
    dataIndex: 'roundingValue',
    key: 'roundingValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'roundingValue') ?? ''
  },
  {
    title: t('entity.materialplant.planneddeliverytimedays'),
    dataIndex: 'plannedDeliveryTimeDays',
    key: 'plannedDeliveryTimeDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'plannedDeliveryTimeDays') ?? ''
  },
  {
    title: t('entity.materialplant.inhouseproductiondays'),
    dataIndex: 'inHouseProductionDays',
    key: 'inHouseProductionDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'inHouseProductionDays') ?? ''
  },
  {
    title: t('entity.materialplant.manufacturer'),
    dataIndex: 'manufacturer',
    key: 'manufacturer',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'manufacturer') ?? ''
  },
  {
    title: t('entity.materialplant.manufacturerpartnumber'),
    dataIndex: 'manufacturerPartNumber',
    key: 'manufacturerPartNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'manufacturerPartNumber') ?? ''
  },
  {
    title: t('entity.materialplant.currencycode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'currencyCode') ?? ''
  },
  {
    title: t('entity.materialplant.pricecontrol'),
    dataIndex: 'priceControl',
    key: 'priceControl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'priceControl') ?? ''
  },
  {
    title: t('entity.materialplant.priceunit'),
    dataIndex: 'priceUnit',
    key: 'priceUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'priceUnit') ?? ''
  },
  {
    title: t('entity.materialplant.valuationcategory'),
    dataIndex: 'valuationCategory',
    key: 'valuationCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'valuationCategory') ?? ''
  },
  {
    title: t('entity.materialplant.differencecode'),
    dataIndex: 'differenceCode',
    key: 'differenceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'differenceCode') ?? ''
  },
  {
    title: t('entity.materialplant.profitcenter'),
    dataIndex: 'profitCenter',
    key: 'profitCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'profitCenter') ?? ''
  },
  {
    title: t('entity.materialplant.latestpurchaseprice'),
    dataIndex: 'latestPurchasePrice',
    key: 'latestPurchasePrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'latestPurchasePrice') ?? ''
  },
  {
    title: t('entity.materialplant.salesprice'),
    dataIndex: 'salesPrice',
    key: 'salesPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'salesPrice') ?? ''
  },
  {
    title: t('entity.materialplant.safetystock'),
    dataIndex: 'safetyStock',
    key: 'safetyStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'safetyStock') ?? ''
  },
  {
    title: t('entity.materialplant.maxstock'),
    dataIndex: 'maxStock',
    key: 'maxStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'maxStock') ?? ''
  },
  {
    title: t('entity.materialplant.minstock'),
    dataIndex: 'minStock',
    key: 'minStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'minStock') ?? ''
  },
  {
    title: t('entity.materialplant.currentstock'),
    dataIndex: 'currentStock',
    key: 'currentStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'currentStock') ?? ''
  },
  {
    title: t('entity.materialplant.productionlocation'),
    dataIndex: 'productionLocation',
    key: 'productionLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'productionLocation') ?? ''
  },
  {
    title: t('entity.materialplant.purchasinglocation'),
    dataIndex: 'purchasingLocation',
    key: 'purchasingLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'purchasingLocation') ?? ''
  },
  {
    title: t('entity.materialplant.inspectionrequired'),
    dataIndex: 'inspectionRequired',
    key: 'inspectionRequired',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'inspectionRequired') ?? ''
  },
  {
    title: t('entity.materialplant.isbatch'),
    dataIndex: 'isBatch',
    key: 'isBatch',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'isBatch') ?? ''
  },
  {
    title: t('entity.materialplant.isexpiry'),
    dataIndex: 'isExpiry',
    key: 'isExpiry',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'isExpiry') ?? ''
  },
  {
    title: t('entity.materialplant.expirydays'),
    dataIndex: 'expiryDays',
    key: 'expiryDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'expiryDays') ?? ''
  },
  {
    title: t('entity.materialplant.materialstatus'),
    dataIndex: 'materialStatus',
    key: 'materialStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.materialplant.materialattributes'),
    dataIndex: 'materialAttributes',
    key: 'materialAttributes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialAttributes') ?? ''
  },
  {
    title: t('entity.materialplant.isendoflife'),
    dataIndex: 'isEndOfLife',
    key: 'isEndOfLife',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'isEndOfLife') ?? ''
  },
  {
    title: t('entity.materialplant.endoflifedate'),
    dataIndex: 'endOfLifeDate',
    key: 'endOfLifeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'endOfLifeDate') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:material:plant:change:log:update',
        onClick: (record: MaterialPlant) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:material:plant:change:log:delete',
        onClick: (record: MaterialPlant) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaterialPlantId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaterialPlantField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaterialPlant[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: MaterialPlant, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getMaterialPlantId(selectedRow.value) === getMaterialPlantId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaterialPlant[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getMaterialPlantList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[MaterialPlant] 加载数据失败', { error })
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
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  purchaseGroup: '',
  purchaseType: undefined as number | undefined,
  specialProcurement: undefined as number | undefined,
  isBulk: undefined as number | undefined,
  minOrderQuantity: undefined as number | undefined,
  roundingValue: undefined as number | undefined,
  plannedDeliveryTimeDays: undefined as number | undefined,
  inHouseProductionDays: undefined as number | undefined,
  manufacturer: '',
  manufacturerPartNumber: '',
  currencyCode: '',
  priceControl: undefined as number | undefined,
  priceUnit: undefined as number | undefined,
  valuationCategory: '',
  differenceCode: '',
  profitCenter: '',
  latestPurchasePrice: undefined as number | undefined,
  salesPrice: undefined as number | undefined,
  safetyStock: undefined as number | undefined,
  maxStock: undefined as number | undefined,
  minStock: undefined as number | undefined,
  currentStock: undefined as number | undefined,
  productionLocation: '',
  purchasingLocation: '',
  inspectionRequired: undefined as number | undefined,
  isBatch: undefined as number | undefined,
  isExpiry: undefined as number | undefined,
  expiryDays: undefined as number | undefined,
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.materialplant._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: MaterialPlant) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.materialplant._self') })
  formLoading.value = true
  try {
    const detail = await loadMaterialPlantDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.materialplant._self') }))
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
      await updateMaterialPlant(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.materialplant._self') }))
    } else {
      await createMaterialPlant(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.materialplant._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  materialPlantChangeLogPanelRef.value?.reload?.()
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
  const res = await getMaterialPlantTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaterialPlant(file, sheetName)
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
    const exportMeta = await exportMaterialPlant(
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
    message.success(t('common.feedback.export.success', { target: t('entity.materialplant._self') }))
  } catch (error: any) {
    logger.error('[MaterialPlant] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.materialplant._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MaterialPlant) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.materialplant._self'), name: t('common.tip.this.target', { target: t('entity.materialplant._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialPlantById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.materialplant._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.materialplant._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.materialplant._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaterialPlantBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.materialplant._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleMaterialStatusChange(record: MaterialPlant, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getMaterialPlantField(record, 'materialStatus')
  const id = getMaterialPlantId(record)
  const row = dataSource.value.find((item) => getMaterialPlantId(item) === id)
  if (row) {
    row.materialStatus = newVal
  }
  try {
    await updateMaterialPlantStatus({ materialPlantId: id, materialStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.materialStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  purchaseGroup: '',
  purchaseType: undefined as number | undefined,
  specialProcurement: undefined as number | undefined,
  isBulk: undefined as number | undefined,
  minOrderQuantity: undefined as number | undefined,
  roundingValue: undefined as number | undefined,
  plannedDeliveryTimeDays: undefined as number | undefined,
  inHouseProductionDays: undefined as number | undefined,
  manufacturer: '',
  manufacturerPartNumber: '',
  currencyCode: '',
  priceControl: undefined as number | undefined,
  priceUnit: undefined as number | undefined,
  valuationCategory: '',
  differenceCode: '',
  profitCenter: '',
  latestPurchasePrice: undefined as number | undefined,
  salesPrice: undefined as number | undefined,
  safetyStock: undefined as number | undefined,
  maxStock: undefined as number | undefined,
  minStock: undefined as number | undefined,
  currentStock: undefined as number | undefined,
  productionLocation: '',
  purchasingLocation: '',
  inspectionRequired: undefined as number | undefined,
  isBatch: undefined as number | undefined,
  isExpiry: undefined as number | undefined,
  expiryDays: undefined as number | undefined,
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
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
