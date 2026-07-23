<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt全局物料实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      :master-row-key="getMaterialId"
      :master-row-selection="rowSelection"
      master-id-column-key="materialId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="tenant"
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
      create-permission="logistics:materials:material:create"
      update-permission="logistics:materials:material:update"
      delete-permission="logistics:materials:material:delete"
      import-permission="logistics:materials:material:import"
      export-permission="logistics:materials:material:export"
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
        <template v-if="column.key === 'materialStatus'">
          <a-switch
            :checked="getMaterialDictValue(record, 'materialStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleMaterialStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
      <template #detail>
        <MaterialDescriptionPanel
          ref="materialDescriptionPanelRef"
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
      <MaterialForm
        :key="formData?.materialId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-materials-material'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="pi.queryPh('materialCode', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completeMaintenanceStatus')">
      <a-form-item :label="pi.queryLabel('completeMaintenanceStatus')">
        <a-input
          v-model:value="advancedQueryForm.completeMaintenanceStatus"
          :placeholder="pi.queryPh('completeMaintenanceStatus', 'required')"
          show-count
          :maxlength="15"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStatus')">
      <a-form-item :label="pi.queryLabel('maintenanceStatus')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceStatus"
          :placeholder="pi.queryPh('maintenanceStatus', 'required')"
          show-count
          :maxlength="15"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientDeletionFlag')">
      <a-form-item :label="pi.queryLabel('clientDeletionFlag')">
        <a-input
          v-model:value="advancedQueryForm.clientDeletionFlag"
          :placeholder="pi.queryPh('clientDeletionFlag', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialType')">
      <a-form-item :label="pi.queryLabel('materialType')">
        <a-input
          v-model:value="advancedQueryForm.materialType"
          :placeholder="pi.queryPh('materialType', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="pi.queryLabel('industrySector')">
        <a-input
          v-model:value="advancedQueryForm.industrySector"
          :placeholder="pi.queryPh('industrySector', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialGroup')">
      <a-form-item :label="pi.queryLabel('materialGroup')">
        <a-input
          v-model:value="advancedQueryForm.materialGroup"
          :placeholder="pi.queryPh('materialGroup', 'required')"
          show-count
          :maxlength="9"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('oldMaterialNumber')">
      <a-form-item :label="pi.queryLabel('oldMaterialNumber')">
        <a-input
          v-model:value="advancedQueryForm.oldMaterialNumber"
          :placeholder="pi.queryPh('oldMaterialNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="pi.queryLabel('baseUnit')">
        <a-input
          v-model:value="advancedQueryForm.baseUnit"
          :placeholder="pi.queryPh('baseUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderUnit')">
      <a-form-item :label="pi.queryLabel('orderUnit')">
        <a-input
          v-model:value="advancedQueryForm.orderUnit"
          :placeholder="pi.queryPh('orderUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentNumber')">
      <a-form-item :label="pi.queryLabel('documentNumber')">
        <a-input
          v-model:value="advancedQueryForm.documentNumber"
          :placeholder="pi.queryPh('documentNumber', 'required')"
          show-count
          :maxlength="22"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentType')">
      <a-form-item :label="pi.queryLabel('documentType')">
        <a-input
          v-model:value="advancedQueryForm.documentType"
          :placeholder="pi.queryPh('documentType', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentVersion')">
      <a-form-item :label="pi.queryLabel('documentVersion')">
        <a-input
          v-model:value="advancedQueryForm.documentVersion"
          :placeholder="pi.queryPh('documentVersion', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentPageFormat')">
      <a-form-item :label="pi.queryLabel('documentPageFormat')">
        <a-input
          v-model:value="advancedQueryForm.documentPageFormat"
          :placeholder="pi.queryPh('documentPageFormat', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentChangeNumber')">
      <a-form-item :label="pi.queryLabel('documentChangeNumber')">
        <a-input
          v-model:value="advancedQueryForm.documentChangeNumber"
          :placeholder="pi.queryPh('documentChangeNumber', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentPageNumber')">
      <a-form-item :label="pi.queryLabel('documentPageNumber')">
        <a-input
          v-model:value="advancedQueryForm.documentPageNumber"
          :placeholder="pi.queryPh('documentPageNumber', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentSheetCount')">
      <a-form-item :label="pi.queryLabel('documentSheetCount')">
        <a-input
          v-model:value="advancedQueryForm.documentSheetCount"
          :placeholder="pi.queryPh('documentSheetCount', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionInspectionMemo')">
      <a-form-item :label="pi.queryLabel('productionInspectionMemo')">
        <a-input
          v-model:value="advancedQueryForm.productionInspectionMemo"
          :placeholder="pi.queryPh('productionInspectionMemo', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionMemoPageFormat')">
      <a-form-item :label="pi.queryLabel('productionMemoPageFormat')">
        <a-input
          v-model:value="advancedQueryForm.productionMemoPageFormat"
          :placeholder="pi.queryPh('productionMemoPageFormat', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sizeDimensions')">
      <a-form-item :label="pi.queryLabel('sizeDimensions')">
        <a-input
          v-model:value="advancedQueryForm.sizeDimensions"
          :placeholder="pi.queryPh('sizeDimensions', 'required')"
          show-count
          :maxlength="32"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('basicMaterial')">
      <a-form-item :label="pi.queryLabel('basicMaterial')">
        <a-input
          v-model:value="advancedQueryForm.basicMaterial"
          :placeholder="pi.queryPh('basicMaterial', 'required')"
          show-count
          :maxlength="48"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industryStandardDescription')">
      <a-form-item :label="pi.queryLabel('industryStandardDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.industryStandardDescription"
          :placeholder="pi.queryPh('industryStandardDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('laboratoryDesignOffice')">
      <a-form-item :label="pi.queryLabel('laboratoryDesignOffice')">
        <a-input
          v-model:value="advancedQueryForm.laboratoryDesignOffice"
          :placeholder="pi.queryPh('laboratoryDesignOffice', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasingValueKey')">
      <a-form-item :label="pi.queryLabel('purchasingValueKey')">
        <a-input
          v-model:value="advancedQueryForm.purchasingValueKey"
          :placeholder="pi.queryPh('purchasingValueKey', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grossWeight')">
      <a-form-item :label="pi.queryLabel('grossWeight')">
        <a-input-number
          v-model:value="advancedQueryForm.grossWeight"
          :placeholder="pi.queryPh('grossWeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netWeight')">
      <a-form-item :label="pi.queryLabel('netWeight')">
        <a-input-number
          v-model:value="advancedQueryForm.netWeight"
          :placeholder="pi.queryPh('netWeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weightUnit')">
      <a-form-item :label="pi.queryLabel('weightUnit')">
        <a-input
          v-model:value="advancedQueryForm.weightUnit"
          :placeholder="pi.queryPh('weightUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('volume')">
      <a-form-item :label="pi.queryLabel('volume')">
        <a-input-number
          v-model:value="advancedQueryForm.volume"
          :placeholder="pi.queryPh('volume', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('volumeUnit')">
      <a-form-item :label="pi.queryLabel('volumeUnit')">
        <a-input
          v-model:value="advancedQueryForm.volumeUnit"
          :placeholder="pi.queryPh('volumeUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('containerRequirements')">
      <a-form-item :label="pi.queryLabel('containerRequirements')">
        <a-input
          v-model:value="advancedQueryForm.containerRequirements"
          :placeholder="pi.queryPh('containerRequirements', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storageConditions')">
      <a-form-item :label="pi.queryLabel('storageConditions')">
        <a-input
          v-model:value="advancedQueryForm.storageConditions"
          :placeholder="pi.queryPh('storageConditions', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('temperatureConditions')">
      <a-form-item :label="pi.queryLabel('temperatureConditions')">
        <a-input
          v-model:value="advancedQueryForm.temperatureConditions"
          :placeholder="pi.queryPh('temperatureConditions', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lowLevelCode')">
      <a-form-item :label="pi.queryLabel('lowLevelCode')">
        <a-input
          v-model:value="advancedQueryForm.lowLevelCode"
          :placeholder="pi.queryPh('lowLevelCode', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transportationGroup')">
      <a-form-item :label="pi.queryLabel('transportationGroup')">
        <a-input
          v-model:value="advancedQueryForm.transportationGroup"
          :placeholder="pi.queryPh('transportationGroup', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('hazardousMaterialNumber')">
      <a-form-item :label="pi.queryLabel('hazardousMaterialNumber')">
        <a-input
          v-model:value="advancedQueryForm.hazardousMaterialNumber"
          :placeholder="pi.queryPh('hazardousMaterialNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('division')">
      <a-form-item :label="pi.queryLabel('division')">
        <a-input
          v-model:value="advancedQueryForm.division"
          :placeholder="pi.queryPh('division', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('competitor')">
      <a-form-item :label="pi.queryLabel('competitor')">
        <a-input
          v-model:value="advancedQueryForm.competitor"
          :placeholder="pi.queryPh('competitor', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('europeanArticleNumberObsolete')">
      <a-form-item :label="pi.queryLabel('europeanArticleNumberObsolete')">
        <a-input
          v-model:value="advancedQueryForm.europeanArticleNumberObsolete"
          :placeholder="pi.queryPh('europeanArticleNumberObsolete', 'required')"
          show-count
          :maxlength="13"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grGiSlipQuantity')">
      <a-form-item :label="pi.queryLabel('grGiSlipQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.grGiSlipQuantity"
          :placeholder="pi.queryPh('grGiSlipQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('procurementRule')">
      <a-form-item :label="pi.queryLabel('procurementRule')">
        <a-input
          v-model:value="advancedQueryForm.procurementRule"
          :placeholder="pi.queryPh('procurementRule', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceOfSupply')">
      <a-form-item :label="pi.queryLabel('sourceOfSupply')">
        <a-input
          v-model:value="advancedQueryForm.sourceOfSupply"
          :placeholder="pi.queryPh('sourceOfSupply', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonCategory')">
      <a-form-item :label="pi.queryLabel('seasonCategory')">
        <a-input
          v-model:value="advancedQueryForm.seasonCategory"
          :placeholder="pi.queryPh('seasonCategory', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('labelType')">
      <a-form-item :label="pi.queryLabel('labelType')">
        <a-input
          v-model:value="advancedQueryForm.labelType"
          :placeholder="pi.queryPh('labelType', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('labelForm')">
      <a-form-item :label="pi.queryLabel('labelForm')">
        <a-input
          v-model:value="advancedQueryForm.labelForm"
          :placeholder="pi.queryPh('labelForm', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deactivatedField')">
      <a-form-item :label="pi.queryLabel('deactivatedField')">
        <a-input
          v-model:value="advancedQueryForm.deactivatedField"
          :placeholder="pi.queryPh('deactivatedField', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('internationalArticleNumber')">
      <a-form-item :label="pi.queryLabel('internationalArticleNumber')">
        <a-input
          v-model:value="advancedQueryForm.internationalArticleNumber"
          :placeholder="pi.queryPh('internationalArticleNumber', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('eanCategory')">
      <a-form-item :label="pi.queryLabel('eanCategory')">
        <a-input
          v-model:value="advancedQueryForm.eanCategory"
          :placeholder="pi.queryPh('eanCategory', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('length')">
      <a-form-item :label="pi.queryLabel('length')">
        <a-input-number
          v-model:value="advancedQueryForm.length"
          :placeholder="pi.queryPh('length', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('width')">
      <a-form-item :label="pi.queryLabel('width')">
        <a-input-number
          v-model:value="advancedQueryForm.width"
          :placeholder="pi.queryPh('width', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('height')">
      <a-form-item :label="pi.queryLabel('height')">
        <a-input-number
          v-model:value="advancedQueryForm.height"
          :placeholder="pi.queryPh('height', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dimensionUnit')">
      <a-form-item :label="pi.queryLabel('dimensionUnit')">
        <a-input
          v-model:value="advancedQueryForm.dimensionUnit"
          :placeholder="pi.queryPh('dimensionUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productHierarchy')">
      <a-form-item :label="pi.queryLabel('productHierarchy')">
        <a-input
          v-model:value="advancedQueryForm.productHierarchy"
          :placeholder="pi.queryPh('productHierarchy', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stockTransferNetChangeCosting')">
      <a-form-item :label="pi.queryLabel('stockTransferNetChangeCosting')">
        <a-input
          v-model:value="advancedQueryForm.stockTransferNetChangeCosting"
          :placeholder="pi.queryPh('stockTransferNetChangeCosting', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cadIndicator')">
      <a-form-item :label="pi.queryLabel('cadIndicator')">
        <a-input
          v-model:value="advancedQueryForm.cadIndicator"
          :placeholder="pi.queryPh('cadIndicator', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qmInProcurement')">
      <a-form-item :label="pi.queryLabel('qmInProcurement')">
        <a-input
          v-model:value="advancedQueryForm.qmInProcurement"
          :placeholder="pi.queryPh('qmInProcurement', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowedPackagingWeight')">
      <a-form-item :label="pi.queryLabel('allowedPackagingWeight')">
        <a-input-number
          v-model:value="advancedQueryForm.allowedPackagingWeight"
          :placeholder="pi.queryPh('allowedPackagingWeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowedPackagingWeightUnit')">
      <a-form-item :label="pi.queryLabel('allowedPackagingWeightUnit')">
        <a-input
          v-model:value="advancedQueryForm.allowedPackagingWeightUnit"
          :placeholder="pi.queryPh('allowedPackagingWeightUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowedPackagingVolume')">
      <a-form-item :label="pi.queryLabel('allowedPackagingVolume')">
        <a-input-number
          v-model:value="advancedQueryForm.allowedPackagingVolume"
          :placeholder="pi.queryPh('allowedPackagingVolume', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowedPackagingVolumeUnit')">
      <a-form-item :label="pi.queryLabel('allowedPackagingVolumeUnit')">
        <a-input
          v-model:value="advancedQueryForm.allowedPackagingVolumeUnit"
          :placeholder="pi.queryPh('allowedPackagingVolumeUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('excessWeightTolerance')">
      <a-form-item :label="pi.queryLabel('excessWeightTolerance')">
        <a-input-number
          v-model:value="advancedQueryForm.excessWeightTolerance"
          :placeholder="pi.queryPh('excessWeightTolerance', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('excessVolumeTolerance')">
      <a-form-item :label="pi.queryLabel('excessVolumeTolerance')">
        <a-input-number
          v-model:value="advancedQueryForm.excessVolumeTolerance"
          :placeholder="pi.queryPh('excessVolumeTolerance', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('variablePurchaseOrderUnit')">
      <a-form-item :label="pi.queryLabel('variablePurchaseOrderUnit')">
        <a-input
          v-model:value="advancedQueryForm.variablePurchaseOrderUnit"
          :placeholder="pi.queryPh('variablePurchaseOrderUnit', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisionLevelAssigned')">
      <a-form-item :label="pi.queryLabel('revisionLevelAssigned')">
        <a-input
          v-model:value="advancedQueryForm.revisionLevelAssigned"
          :placeholder="pi.queryPh('revisionLevelAssigned', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurableMaterial')">
      <a-form-item :label="pi.queryLabel('configurableMaterial')">
        <a-input
          v-model:value="advancedQueryForm.configurableMaterial"
          :placeholder="pi.queryPh('configurableMaterial', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchManagementRequired')">
      <a-form-item :label="pi.queryLabel('batchManagementRequired')">
        <a-input
          v-model:value="advancedQueryForm.batchManagementRequired"
          :placeholder="pi.queryPh('batchManagementRequired', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packagingMaterialType')">
      <a-form-item :label="pi.queryLabel('packagingMaterialType')">
        <a-input
          v-model:value="advancedQueryForm.packagingMaterialType"
          :placeholder="pi.queryPh('packagingMaterialType', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumLevelByVolume')">
      <a-form-item :label="pi.queryLabel('maximumLevelByVolume')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumLevelByVolume"
          :placeholder="pi.queryPh('maximumLevelByVolume', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stackingFactor')">
      <a-form-item :label="pi.queryLabel('stackingFactor')">
        <a-input-number
          v-model:value="advancedQueryForm.stackingFactor"
          :placeholder="pi.queryPh('stackingFactor', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packagingMaterialGroup')">
      <a-form-item :label="pi.queryLabel('packagingMaterialGroup')">
        <a-input
          v-model:value="advancedQueryForm.packagingMaterialGroup"
          :placeholder="pi.queryPh('packagingMaterialGroup', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('authorizationGroup')">
      <a-form-item :label="pi.queryLabel('authorizationGroup')">
        <a-input
          v-model:value="advancedQueryForm.authorizationGroup"
          :placeholder="pi.queryPh('authorizationGroup', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromDateStart')">
      <a-form-item :label="pi.queryLabel('validFromDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromDateStart"
          :placeholder="pi.queryPh('validFromDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromDateEnd')">
      <a-form-item :label="pi.queryLabel('validFromDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromDateEnd"
          :placeholder="pi.queryPh('validFromDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonYear')">
      <a-form-item :label="pi.queryLabel('seasonYear')">
        <a-input
          v-model:value="advancedQueryForm.seasonYear"
          :placeholder="pi.queryPh('seasonYear', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceBandCategory')">
      <a-form-item :label="pi.queryLabel('priceBandCategory')">
        <a-input
          v-model:value="advancedQueryForm.priceBandCategory"
          :placeholder="pi.queryPh('priceBandCategory', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('emptiesBillOfMaterial')">
      <a-form-item :label="pi.queryLabel('emptiesBillOfMaterial')">
        <a-input
          v-model:value="advancedQueryForm.emptiesBillOfMaterial"
          :placeholder="pi.queryPh('emptiesBillOfMaterial', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('externalMaterialGroup')">
      <a-form-item :label="pi.queryLabel('externalMaterialGroup')">
        <a-input
          v-model:value="advancedQueryForm.externalMaterialGroup"
          :placeholder="pi.queryPh('externalMaterialGroup', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossPlantConfigurableMaterial')">
      <a-form-item :label="pi.queryLabel('crossPlantConfigurableMaterial')">
        <a-input
          v-model:value="advancedQueryForm.crossPlantConfigurableMaterial"
          :placeholder="pi.queryPh('crossPlantConfigurableMaterial', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCategory')">
      <a-form-item :label="pi.queryLabel('materialCategory')">
        <a-input
          v-model:value="advancedQueryForm.materialCategory"
          :placeholder="pi.queryPh('materialCategory', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('coProductIndicator')">
      <a-form-item :label="pi.queryLabel('coProductIndicator')">
        <a-input
          v-model:value="advancedQueryForm.coProductIndicator"
          :placeholder="pi.queryPh('coProductIndicator', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('followUpMaterialIndicator')">
      <a-form-item :label="pi.queryLabel('followUpMaterialIndicator')">
        <a-input
          v-model:value="advancedQueryForm.followUpMaterialIndicator"
          :placeholder="pi.queryPh('followUpMaterialIndicator', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingReferenceMaterial')">
      <a-form-item :label="pi.queryLabel('pricingReferenceMaterial')">
        <a-input
          v-model:value="advancedQueryForm.pricingReferenceMaterial"
          :placeholder="pi.queryPh('pricingReferenceMaterial', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossPlantMaterialStatus')">
      <a-form-item :label="pi.queryLabel('crossPlantMaterialStatus')">
        <a-input
          v-model:value="advancedQueryForm.crossPlantMaterialStatus"
          :placeholder="pi.queryPh('crossPlantMaterialStatus', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossDistributionChainStatus')">
      <a-form-item :label="pi.queryLabel('crossDistributionChainStatus')">
        <a-input
          v-model:value="advancedQueryForm.crossDistributionChainStatus"
          :placeholder="pi.queryPh('crossDistributionChainStatus', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossPlantStatusValidFromStart')">
      <a-form-item :label="pi.queryLabel('crossPlantStatusValidFromStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.crossPlantStatusValidFromStart"
          :placeholder="pi.queryPh('crossPlantStatusValidFromStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossPlantStatusValidFromEnd')">
      <a-form-item :label="pi.queryLabel('crossPlantStatusValidFromEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.crossPlantStatusValidFromEnd"
          :placeholder="pi.queryPh('crossPlantStatusValidFromEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossDistributionStatusValidFromStart')">
      <a-form-item :label="pi.queryLabel('crossDistributionStatusValidFromStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.crossDistributionStatusValidFromStart"
          :placeholder="pi.queryPh('crossDistributionStatusValidFromStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossDistributionStatusValidFromEnd')">
      <a-form-item :label="pi.queryLabel('crossDistributionStatusValidFromEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.crossDistributionStatusValidFromEnd"
          :placeholder="pi.queryPh('crossDistributionStatusValidFromEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxClassification')">
      <a-form-item :label="pi.queryLabel('taxClassification')">
        <a-input
          v-model:value="advancedQueryForm.taxClassification"
          :placeholder="pi.queryPh('taxClassification', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('catalogProfile')">
      <a-form-item :label="pi.queryLabel('catalogProfile')">
        <a-input
          v-model:value="advancedQueryForm.catalogProfile"
          :placeholder="pi.queryPh('catalogProfile', 'required')"
          show-count
          :maxlength="9"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('minimumRemainingShelfLife')">
      <a-form-item :label="pi.queryLabel('minimumRemainingShelfLife')">
        <a-input-number
          v-model:value="advancedQueryForm.minimumRemainingShelfLife"
          :placeholder="pi.queryPh('minimumRemainingShelfLife', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalShelfLife')">
      <a-form-item :label="pi.queryLabel('totalShelfLife')">
        <a-input-number
          v-model:value="advancedQueryForm.totalShelfLife"
          :placeholder="pi.queryPh('totalShelfLife', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storagePercentage')">
      <a-form-item :label="pi.queryLabel('storagePercentage')">
        <a-input-number
          v-model:value="advancedQueryForm.storagePercentage"
          :placeholder="pi.queryPh('storagePercentage', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contentUnit')">
      <a-form-item :label="pi.queryLabel('contentUnit')">
        <a-textarea
          v-model:value="advancedQueryForm.contentUnit"
          :placeholder="pi.queryPh('contentUnit', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netContents')">
      <a-form-item :label="pi.queryLabel('netContents')">
        <a-textarea
          v-model:value="advancedQueryForm.netContents"
          :placeholder="pi.queryPh('netContents', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('comparisonPriceUnit')">
      <a-form-item :label="pi.queryLabel('comparisonPriceUnit')">
        <a-input-number
          v-model:value="advancedQueryForm.comparisonPriceUnit"
          :placeholder="pi.queryPh('comparisonPriceUnit', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('labelingMaterialGrouping')">
      <a-form-item :label="pi.queryLabel('labelingMaterialGrouping')">
        <a-input
          v-model:value="advancedQueryForm.labelingMaterialGrouping"
          :placeholder="pi.queryPh('labelingMaterialGrouping', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grossContents')">
      <a-form-item :label="pi.queryLabel('grossContents')">
        <a-textarea
          v-model:value="advancedQueryForm.grossContents"
          :placeholder="pi.queryPh('grossContents', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quantityConversionMethod')">
      <a-form-item :label="pi.queryLabel('quantityConversionMethod')">
        <a-input
          v-model:value="advancedQueryForm.quantityConversionMethod"
          :placeholder="pi.queryPh('quantityConversionMethod', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('internalObjectNumber')">
      <a-form-item :label="pi.queryLabel('internalObjectNumber')">
        <a-input
          v-model:value="advancedQueryForm.internalObjectNumber"
          :placeholder="pi.queryPh('internalObjectNumber', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('environmentallyRelevant')">
      <a-form-item :label="pi.queryLabel('environmentallyRelevant')">
        <a-input
          v-model:value="advancedQueryForm.environmentallyRelevant"
          :placeholder="pi.queryPh('environmentallyRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productAllocationProcedure')">
      <a-form-item :label="pi.queryLabel('productAllocationProcedure')">
        <a-input
          v-model:value="advancedQueryForm.productAllocationProcedure"
          :placeholder="pi.queryPh('productAllocationProcedure', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('variantPricingProfile')">
      <a-form-item :label="pi.queryLabel('variantPricingProfile')">
        <a-input
          v-model:value="advancedQueryForm.variantPricingProfile"
          :placeholder="pi.queryPh('variantPricingProfile', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountInKind')">
      <a-form-item :label="pi.queryLabel('discountInKind')">
        <a-input
          v-model:value="advancedQueryForm.discountInKind"
          :placeholder="pi.queryPh('discountInKind', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPartNumber')">
      <a-form-item :label="pi.queryLabel('manufacturerPartNumber')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerPartNumber"
          :placeholder="pi.queryPh('manufacturerPartNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerNumber')">
      <a-form-item :label="pi.queryLabel('manufacturerNumber')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerNumber"
          :placeholder="pi.queryPh('manufacturerNumber', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inventoryManagedMaterialNumber')">
      <a-form-item :label="pi.queryLabel('inventoryManagedMaterialNumber')">
        <a-input
          v-model:value="advancedQueryForm.inventoryManagedMaterialNumber"
          :placeholder="pi.queryPh('inventoryManagedMaterialNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPartProfile')">
      <a-form-item :label="pi.queryLabel('manufacturerPartProfile')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerPartProfile"
          :placeholder="pi.queryPh('manufacturerPartProfile', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unitsOfMeasureUsage')">
      <a-form-item :label="pi.queryLabel('unitsOfMeasureUsage')">
        <a-input
          v-model:value="advancedQueryForm.unitsOfMeasureUsage"
          :placeholder="pi.queryPh('unitsOfMeasureUsage', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonRollout')">
      <a-form-item :label="pi.queryLabel('seasonRollout')">
        <a-input
          v-model:value="advancedQueryForm.seasonRollout"
          :placeholder="pi.queryPh('seasonRollout', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dangerousGoodsProfile')">
      <a-form-item :label="pi.queryLabel('dangerousGoodsProfile')">
        <a-input
          v-model:value="advancedQueryForm.dangerousGoodsProfile"
          :placeholder="pi.queryPh('dangerousGoodsProfile', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('highlyViscous')">
      <a-form-item :label="pi.queryLabel('highlyViscous')">
        <a-input
          v-model:value="advancedQueryForm.highlyViscous"
          :placeholder="pi.queryPh('highlyViscous', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inBulkLiquid')">
      <a-form-item :label="pi.queryLabel('inBulkLiquid')">
        <a-input
          v-model:value="advancedQueryForm.inBulkLiquid"
          :placeholder="pi.queryPh('inBulkLiquid', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialNumberExplicitness')">
      <a-form-item :label="pi.queryLabel('serialNumberExplicitness')">
        <a-input
          v-model:value="advancedQueryForm.serialNumberExplicitness"
          :placeholder="pi.queryPh('serialNumberExplicitness', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closedPackaging')">
      <a-form-item :label="pi.queryLabel('closedPackaging')">
        <a-input
          v-model:value="advancedQueryForm.closedPackaging"
          :placeholder="pi.queryPh('closedPackaging', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBatchRecordRequired')">
      <a-form-item :label="pi.queryLabel('approvedBatchRecordRequired')">
        <a-input
          v-model:value="advancedQueryForm.approvedBatchRecordRequired"
          :placeholder="pi.queryPh('approvedBatchRecordRequired', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectivityParameterOverride')">
      <a-form-item :label="pi.queryLabel('effectivityParameterOverride')">
        <a-input
          v-model:value="advancedQueryForm.effectivityParameterOverride"
          :placeholder="pi.queryPh('effectivityParameterOverride', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCompletionLevel')">
      <a-form-item :label="pi.queryLabel('materialCompletionLevel')">
        <a-input
          v-model:value="advancedQueryForm.materialCompletionLevel"
          :placeholder="pi.queryPh('materialCompletionLevel', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shelfLifePeriodIndicator')">
      <a-form-item :label="pi.queryLabel('shelfLifePeriodIndicator')">
        <a-input
          v-model:value="advancedQueryForm.shelfLifePeriodIndicator"
          :placeholder="pi.queryPh('shelfLifePeriodIndicator', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shelfLifeRoundingRule')">
      <a-form-item :label="pi.queryLabel('shelfLifeRoundingRule')">
        <a-input
          v-model:value="advancedQueryForm.shelfLifeRoundingRule"
          :placeholder="pi.queryPh('shelfLifeRoundingRule', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productCompositionOnPackaging')">
      <a-form-item :label="pi.queryLabel('productCompositionOnPackaging')">
        <a-input
          v-model:value="advancedQueryForm.productCompositionOnPackaging"
          :placeholder="pi.queryPh('productCompositionOnPackaging', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('generalItemCategoryGroup')">
      <a-form-item :label="pi.queryLabel('generalItemCategoryGroup')">
        <a-input
          v-model:value="advancedQueryForm.generalItemCategoryGroup"
          :placeholder="pi.queryPh('generalItemCategoryGroup', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('logisticalVariants')">
      <a-form-item :label="pi.queryLabel('logisticalVariants')">
        <a-input
          v-model:value="advancedQueryForm.logisticalVariants"
          :placeholder="pi.queryPh('logisticalVariants', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialLocked')">
      <a-form-item :label="pi.queryLabel('materialLocked')">
        <a-input
          v-model:value="advancedQueryForm.materialLocked"
          :placeholder="pi.queryPh('materialLocked', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurationManagementRelevant')">
      <a-form-item :label="pi.queryLabel('configurationManagementRelevant')">
        <a-input
          v-model:value="advancedQueryForm.configurationManagementRelevant"
          :placeholder="pi.queryPh('configurationManagementRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assortmentListType')">
      <a-form-item :label="pi.queryLabel('assortmentListType')">
        <a-input
          v-model:value="advancedQueryForm.assortmentListType"
          :placeholder="pi.queryPh('assortmentListType', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expirationDateType')">
      <a-form-item :label="pi.queryLabel('expirationDateType')">
        <a-date-picker
          v-model:value="advancedQueryForm.expirationDateType"
          :placeholder="pi.queryPh('expirationDateType', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('gtinVariant')">
      <a-form-item :label="pi.queryLabel('gtinVariant')">
        <a-input
          v-model:value="advancedQueryForm.gtinVariant"
          :placeholder="pi.queryPh('gtinVariant', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genericMaterialNumber')">
      <a-form-item :label="pi.queryLabel('genericMaterialNumber')">
        <a-input
          v-model:value="advancedQueryForm.genericMaterialNumber"
          :placeholder="pi.queryPh('genericMaterialNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samePackingReferenceMaterial')">
      <a-form-item :label="pi.queryLabel('samePackingReferenceMaterial')">
        <a-input
          v-model:value="advancedQueryForm.samePackingReferenceMaterial"
          :placeholder="pi.queryPh('samePackingReferenceMaterial', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('globalDataSyncRelevant')">
      <a-form-item :label="pi.queryLabel('globalDataSyncRelevant')">
        <a-input
          v-model:value="advancedQueryForm.globalDataSyncRelevant"
          :placeholder="pi.queryPh('globalDataSyncRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptanceAtOrigin')">
      <a-form-item :label="pi.queryLabel('acceptanceAtOrigin')">
        <a-input
          v-model:value="advancedQueryForm.acceptanceAtOrigin"
          :placeholder="pi.queryPh('acceptanceAtOrigin', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardHuType')">
      <a-form-item :label="pi.queryLabel('standardHuType')">
        <a-input
          v-model:value="advancedQueryForm.standardHuType"
          :placeholder="pi.queryPh('standardHuType', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pilferable')">
      <a-form-item :label="pi.queryLabel('pilferable')">
        <a-input
          v-model:value="advancedQueryForm.pilferable"
          :placeholder="pi.queryPh('pilferable', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseStorageCondition')">
      <a-form-item :label="pi.queryLabel('warehouseStorageCondition')">
        <a-input
          v-model:value="advancedQueryForm.warehouseStorageCondition"
          :placeholder="pi.queryPh('warehouseStorageCondition', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseMaterialGroup')">
      <a-form-item :label="pi.queryLabel('warehouseMaterialGroup')">
        <a-input
          v-model:value="advancedQueryForm.warehouseMaterialGroup"
          :placeholder="pi.queryPh('warehouseMaterialGroup', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingIndicator')">
      <a-form-item :label="pi.queryLabel('handlingIndicator')">
        <a-input
          v-model:value="advancedQueryForm.handlingIndicator"
          :placeholder="pi.queryPh('handlingIndicator', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('hazardousSubstancesRelevant')">
      <a-form-item :label="pi.queryLabel('hazardousSubstancesRelevant')">
        <a-input
          v-model:value="advancedQueryForm.hazardousSubstancesRelevant"
          :placeholder="pi.queryPh('hazardousSubstancesRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingUnitType')">
      <a-form-item :label="pi.queryLabel('handlingUnitType')">
        <a-input
          v-model:value="advancedQueryForm.handlingUnitType"
          :placeholder="pi.queryPh('handlingUnitType', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('variableTareWeight')">
      <a-form-item :label="pi.queryLabel('variableTareWeight')">
        <a-input
          v-model:value="advancedQueryForm.variableTareWeight"
          :placeholder="pi.queryPh('variableTareWeight', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumAllowedCapacity')">
      <a-form-item :label="pi.queryLabel('maximumAllowedCapacity')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumAllowedCapacity"
          :placeholder="pi.queryPh('maximumAllowedCapacity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overcapacityTolerance')">
      <a-form-item :label="pi.queryLabel('overcapacityTolerance')">
        <a-input-number
          v-model:value="advancedQueryForm.overcapacityTolerance"
          :placeholder="pi.queryPh('overcapacityTolerance', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumPackingLength')">
      <a-form-item :label="pi.queryLabel('maximumPackingLength')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumPackingLength"
          :placeholder="pi.queryPh('maximumPackingLength', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumPackingWidth')">
      <a-form-item :label="pi.queryLabel('maximumPackingWidth')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumPackingWidth"
          :placeholder="pi.queryPh('maximumPackingWidth', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumPackingHeight')">
      <a-form-item :label="pi.queryLabel('maximumPackingHeight')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumPackingHeight"
          :placeholder="pi.queryPh('maximumPackingHeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumPackingDimensionUnit')">
      <a-form-item :label="pi.queryLabel('maximumPackingDimensionUnit')">
        <a-input
          v-model:value="advancedQueryForm.maximumPackingDimensionUnit"
          :placeholder="pi.queryPh('maximumPackingDimensionUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countryOfOrigin')">
      <a-form-item :label="pi.queryLabel('countryOfOrigin')">
        <a-input
          v-model:value="advancedQueryForm.countryOfOrigin"
          :placeholder="pi.queryPh('countryOfOrigin', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialFreightGroup')">
      <a-form-item :label="pi.queryLabel('materialFreightGroup')">
        <a-input
          v-model:value="advancedQueryForm.materialFreightGroup"
          :placeholder="pi.queryPh('materialFreightGroup', 'required')"
          show-count
          :maxlength="8"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quarantinePeriod')">
      <a-form-item :label="pi.queryLabel('quarantinePeriod')">
        <a-input-number
          v-model:value="advancedQueryForm.quarantinePeriod"
          :placeholder="pi.queryPh('quarantinePeriod', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quarantinePeriodUnit')">
      <a-form-item :label="pi.queryLabel('quarantinePeriodUnit')">
        <a-input
          v-model:value="advancedQueryForm.quarantinePeriodUnit"
          :placeholder="pi.queryPh('quarantinePeriodUnit', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityInspectionGroup')">
      <a-form-item :label="pi.queryLabel('qualityInspectionGroup')">
        <a-input
          v-model:value="advancedQueryForm.qualityInspectionGroup"
          :placeholder="pi.queryPh('qualityInspectionGroup', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialNumberProfile')">
      <a-form-item :label="pi.queryLabel('serialNumberProfile')">
        <a-input
          v-model:value="advancedQueryForm.serialNumberProfile"
          :placeholder="pi.queryPh('serialNumberProfile', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formName')">
      <a-form-item :label="pi.queryLabel('formName')">
        <a-input
          v-model:value="advancedQueryForm.formName"
          :placeholder="pi.queryPh('formName', 'required')"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('logisticsUnitOfMeasure')">
      <a-form-item :label="pi.queryLabel('logisticsUnitOfMeasure')">
        <a-input
          v-model:value="advancedQueryForm.logisticsUnitOfMeasure"
          :placeholder="pi.queryPh('logisticsUnitOfMeasure', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('catchWeightMaterial')">
      <a-form-item :label="pi.queryLabel('catchWeightMaterial')">
        <a-input
          v-model:value="advancedQueryForm.catchWeightMaterial"
          :placeholder="pi.queryPh('catchWeightMaterial', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('catchWeightProfile')">
      <a-form-item :label="pi.queryLabel('catchWeightProfile')">
        <a-input
          v-model:value="advancedQueryForm.catchWeightProfile"
          :placeholder="pi.queryPh('catchWeightProfile', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('catchWeightToleranceGroup')">
      <a-form-item :label="pi.queryLabel('catchWeightToleranceGroup')">
        <a-input
          v-model:value="advancedQueryForm.catchWeightToleranceGroup"
          :placeholder="pi.queryPh('catchWeightToleranceGroup', 'required')"
          show-count
          :maxlength="9"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('adjustmentProfile')">
      <a-form-item :label="pi.queryLabel('adjustmentProfile')">
        <a-input
          v-model:value="advancedQueryForm.adjustmentProfile"
          :placeholder="pi.queryPh('adjustmentProfile', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('intellectualPropertyId')">
      <a-form-item :label="pi.queryLabel('intellectualPropertyId')">
        <a-input
          v-model:value="advancedQueryForm.intellectualPropertyId"
          :placeholder="pi.queryPh('intellectualPropertyId', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('variantPriceAllowed')">
      <a-form-item :label="pi.queryLabel('variantPriceAllowed')">
        <a-input
          v-model:value="advancedQueryForm.variantPriceAllowed"
          :placeholder="pi.queryPh('variantPriceAllowed', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('medium')">
      <a-form-item :label="pi.queryLabel('medium')">
        <a-input
          v-model:value="advancedQueryForm.medium"
          :placeholder="pi.queryPh('medium', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('physicalCommodity')">
      <a-form-item :label="pi.queryLabel('physicalCommodity')">
        <a-input
          v-model:value="advancedQueryForm.physicalCommodity"
          :placeholder="pi.queryPh('physicalCommodity', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('animalOrigin')">
      <a-form-item :label="pi.queryLabel('animalOrigin')">
        <a-input
          v-model:value="advancedQueryForm.animalOrigin"
          :placeholder="pi.queryPh('animalOrigin', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('textileCompositionFunction')">
      <a-form-item :label="pi.queryLabel('textileCompositionFunction')">
        <a-input
          v-model:value="advancedQueryForm.textileCompositionFunction"
          :placeholder="pi.queryPh('textileCompositionFunction', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationStructure')">
      <a-form-item :label="pi.queryLabel('segmentationStructure')">
        <a-input
          v-model:value="advancedQueryForm.segmentationStructure"
          :placeholder="pi.queryPh('segmentationStructure', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationStrategy')">
      <a-form-item :label="pi.queryLabel('segmentationStrategy')">
        <a-input
          v-model:value="advancedQueryForm.segmentationStrategy"
          :placeholder="pi.queryPh('segmentationStrategy', 'required')"
          show-count
          :maxlength="8"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationStatus')">
      <a-form-item :label="pi.queryLabel('segmentationStatus')">
        <a-input
          v-model:value="advancedQueryForm.segmentationStatus"
          :placeholder="pi.queryPh('segmentationStatus', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationScope')">
      <a-form-item :label="pi.queryLabel('segmentationScope')">
        <a-textarea
          v-model:value="advancedQueryForm.segmentationScope"
          :placeholder="pi.queryPh('segmentationScope', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationRelevant')">
      <a-form-item :label="pi.queryLabel('segmentationRelevant')">
        <a-input
          v-model:value="advancedQueryForm.segmentationRelevant"
          :placeholder="pi.queryPh('segmentationRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fashionAttribute1')">
      <a-form-item :label="pi.queryLabel('fashionAttribute1')">
        <a-input
          v-model:value="advancedQueryForm.fashionAttribute1"
          :placeholder="pi.queryPh('fashionAttribute1', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fashionAttribute2')">
      <a-form-item :label="pi.queryLabel('fashionAttribute2')">
        <a-input
          v-model:value="advancedQueryForm.fashionAttribute2"
          :placeholder="pi.queryPh('fashionAttribute2', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fashionAttribute3')">
      <a-form-item :label="pi.queryLabel('fashionAttribute3')">
        <a-input
          v-model:value="advancedQueryForm.fashionAttribute3"
          :placeholder="pi.queryPh('fashionAttribute3', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonUsageIndicator')">
      <a-form-item :label="pi.queryLabel('seasonUsageIndicator')">
        <a-input
          v-model:value="advancedQueryForm.seasonUsageIndicator"
          :placeholder="pi.queryPh('seasonUsageIndicator', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonActiveInInventory')">
      <a-form-item :label="pi.queryLabel('seasonActiveInInventory')">
        <a-input
          v-model:value="advancedQueryForm.seasonActiveInInventory"
          :placeholder="pi.queryPh('seasonActiveInInventory', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('characteristicConversionId')">
      <a-form-item :label="pi.queryLabel('characteristicConversionId')">
        <a-input
          v-model:value="advancedQueryForm.characteristicConversionId"
          :placeholder="pi.queryPh('characteristicConversionId', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('anpCode')">
      <a-form-item :label="pi.queryLabel('anpCode')">
        <a-input
          v-model:value="advancedQueryForm.anpCode"
          :placeholder="pi.queryPh('anpCode', 'required')"
          show-count
          :maxlength="9"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dangerousGoodsPackagingStatus')">
      <a-form-item :label="pi.queryLabel('dangerousGoodsPackagingStatus')">
        <a-input
          v-model:value="advancedQueryForm.dangerousGoodsPackagingStatus"
          :placeholder="pi.queryPh('dangerousGoodsPackagingStatus', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialConditionManagement')">
      <a-form-item :label="pi.queryLabel('materialConditionManagement')">
        <a-input
          v-model:value="advancedQueryForm.materialConditionManagement"
          :placeholder="pi.queryPh('materialConditionManagement', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('returnCode')">
      <a-form-item :label="pi.queryLabel('returnCode')">
        <a-input
          v-model:value="advancedQueryForm.returnCode"
          :placeholder="pi.queryPh('returnCode', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('returnToLogisticsLevel')">
      <a-form-item :label="pi.queryLabel('returnToLogisticsLevel')">
        <a-input
          v-model:value="advancedQueryForm.returnToLogisticsLevel"
          :placeholder="pi.queryPh('returnToLogisticsLevel', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('natoItemIdentificationNumber')">
      <a-form-item :label="pi.queryLabel('natoItemIdentificationNumber')">
        <a-input
          v-model:value="advancedQueryForm.natoItemIdentificationNumber"
          :placeholder="pi.queryPh('natoItemIdentificationNumber', 'required')"
          show-count
          :maxlength="9"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fffClass')">
      <a-form-item :label="pi.queryLabel('fffClass')">
        <a-input
          v-model:value="advancedQueryForm.fffClass"
          :placeholder="pi.queryPh('fffClass', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supersessionChainNumber')">
      <a-form-item :label="pi.queryLabel('supersessionChainNumber')">
        <a-input
          v-model:value="advancedQueryForm.supersessionChainNumber"
          :placeholder="pi.queryPh('supersessionChainNumber', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonalProcurementCreationStatus')">
      <a-form-item :label="pi.queryLabel('seasonalProcurementCreationStatus')">
        <a-input
          v-model:value="advancedQueryForm.seasonalProcurementCreationStatus"
          :placeholder="pi.queryPh('seasonalProcurementCreationStatus', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('colorCharacteristicInternalNumber')">
      <a-form-item :label="pi.queryLabel('colorCharacteristicInternalNumber')">
        <a-input
          v-model:value="advancedQueryForm.colorCharacteristicInternalNumber"
          :placeholder="pi.queryPh('colorCharacteristicInternalNumber', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainSizeCharacteristicInternalNumber')">
      <a-form-item :label="pi.queryLabel('mainSizeCharacteristicInternalNumber')">
        <a-input
          v-model:value="advancedQueryForm.mainSizeCharacteristicInternalNumber"
          :placeholder="pi.queryPh('mainSizeCharacteristicInternalNumber', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('secondSizeCharacteristicInternalNumber')">
      <a-form-item :label="pi.queryLabel('secondSizeCharacteristicInternalNumber')">
        <a-input
          v-model:value="advancedQueryForm.secondSizeCharacteristicInternalNumber"
          :placeholder="pi.queryPh('secondSizeCharacteristicInternalNumber', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('color')">
      <a-form-item :label="pi.queryLabel('color')">
        <a-input
          v-model:value="advancedQueryForm.color"
          :placeholder="pi.queryPh('color', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainSize')">
      <a-form-item :label="pi.queryLabel('mainSize')">
        <a-input
          v-model:value="advancedQueryForm.mainSize"
          :placeholder="pi.queryPh('mainSize', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('secondSize')">
      <a-form-item :label="pi.queryLabel('secondSize')">
        <a-input
          v-model:value="advancedQueryForm.secondSize"
          :placeholder="pi.queryPh('secondSize', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationCharacteristicValue')">
      <a-form-item :label="pi.queryLabel('evaluationCharacteristicValue')">
        <a-input
          v-model:value="advancedQueryForm.evaluationCharacteristicValue"
          :placeholder="pi.queryPh('evaluationCharacteristicValue', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('careCode')">
      <a-form-item :label="pi.queryLabel('careCode')">
        <a-input
          v-model:value="advancedQueryForm.careCode"
          :placeholder="pi.queryPh('careCode', 'required')"
          show-count
          :maxlength="16"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('brandId')">
      <a-form-item :label="pi.queryLabel('brandId')">
        <a-input
          v-model:value="advancedQueryForm.brandId"
          :placeholder="pi.queryPh('brandId', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode1')">
      <a-form-item :label="pi.queryLabel('fiberCode1')">
        <a-input
          v-model:value="advancedQueryForm.fiberCode1"
          :placeholder="pi.queryPh('fiberCode1', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart1')">
      <a-form-item :label="pi.queryLabel('fiberPart1')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart1"
          :placeholder="pi.queryPh('fiberPart1', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode2')">
      <a-form-item :label="pi.queryLabel('fiberCode2')">
        <a-input
          v-model:value="advancedQueryForm.fiberCode2"
          :placeholder="pi.queryPh('fiberCode2', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart2')">
      <a-form-item :label="pi.queryLabel('fiberPart2')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart2"
          :placeholder="pi.queryPh('fiberPart2', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode3')">
      <a-form-item :label="pi.queryLabel('fiberCode3')">
        <a-input
          v-model:value="advancedQueryForm.fiberCode3"
          :placeholder="pi.queryPh('fiberCode3', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart3')">
      <a-form-item :label="pi.queryLabel('fiberPart3')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart3"
          :placeholder="pi.queryPh('fiberPart3', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode4')">
      <a-form-item :label="pi.queryLabel('fiberCode4')">
        <a-input
          v-model:value="advancedQueryForm.fiberCode4"
          :placeholder="pi.queryPh('fiberCode4', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart4')">
      <a-form-item :label="pi.queryLabel('fiberPart4')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart4"
          :placeholder="pi.queryPh('fiberPart4', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode5')">
      <a-form-item :label="pi.queryLabel('fiberCode5')">
        <a-input
          v-model:value="advancedQueryForm.fiberCode5"
          :placeholder="pi.queryPh('fiberCode5', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart5')">
      <a-form-item :label="pi.queryLabel('fiberPart5')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart5"
          :placeholder="pi.queryPh('fiberPart5', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fashionGrade')">
      <a-form-item :label="pi.queryLabel('fashionGrade')">
        <a-input
          v-model:value="advancedQueryForm.fashionGrade"
          :placeholder="pi.queryPh('fashionGrade', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialStatus')">
      <a-form-item :label="pi.queryLabel('materialStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="pi.queryPh('materialStatus', 'select')"
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
        :entity-i18n-key="MATERIAL_SELF_I18N_KEY"
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
      :id-column-key="'materialId'"
      :action-column-key="'action'"
      entity-scope="tenant"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt全局物料实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import MaterialForm from './components/material-form.vue'
import MaterialDescriptionPanel from './components/material-description-panel.vue'
import { provideMaterialMasterContext, type MaterialRowRecord } from './composables/use-material-master-context'
import { getMaterialList, getMaterialById, createMaterial, updateMaterial, deleteMaterialById, deleteMaterialBatch, getMaterialTemplate, importMaterial, exportMaterial, updateMaterialStatus } from '@/api/logistics/materials/material'
import type { Material, MaterialQuery } from '@/types/logistics/materials/material'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useMaterialI18n,
  MATERIAL_LIST_FIELDS,
  MATERIAL_QUERY_STRING_FIELDS,
  MATERIAL_QUERY_FIELDS,
  MATERIAL_SELF_I18N_KEY,
} from './composables/use-material-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useMaterialI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterial')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Material[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<MaterialRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<MaterialRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Material> | null>(null)
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
  const form = Object.fromEntries(MATERIAL_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof MATERIAL_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    grossWeight: undefined as number | undefined,
    netWeight: undefined as number | undefined,
    volume: undefined as number | undefined,
    grGiSlipQuantity: undefined as number | undefined,
    length: undefined as number | undefined,
    width: undefined as number | undefined,
    height: undefined as number | undefined,
    allowedPackagingWeight: undefined as number | undefined,
    allowedPackagingVolume: undefined as number | undefined,
    excessWeightTolerance: undefined as number | undefined,
    excessVolumeTolerance: undefined as number | undefined,
    maximumLevelByVolume: undefined as number | undefined,
    stackingFactor: undefined as number | undefined,
    minimumRemainingShelfLife: undefined as number | undefined,
    totalShelfLife: undefined as number | undefined,
    storagePercentage: undefined as number | undefined,
    netContents: undefined as number | undefined,
    comparisonPriceUnit: undefined as number | undefined,
    grossContents: undefined as number | undefined,
    maximumAllowedCapacity: undefined as number | undefined,
    overcapacityTolerance: undefined as number | undefined,
    maximumPackingLength: undefined as number | undefined,
    maximumPackingWidth: undefined as number | undefined,
    maximumPackingHeight: undefined as number | undefined,
    quarantinePeriod: undefined as number | undefined,
    materialStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  MATERIAL_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'materialId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideMaterialMasterContext()
const materialDescriptionPanelRef = ref<InstanceType<typeof MaterialDescriptionPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaterialQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialQuery>): MaterialQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of MATERIAL_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.grossWeight !== undefined && form.grossWeight !== null) {
    query.grossWeight = form.grossWeight
  }
  if (form.netWeight !== undefined && form.netWeight !== null) {
    query.netWeight = form.netWeight
  }
  if (form.volume !== undefined && form.volume !== null) {
    query.volume = form.volume
  }
  if (form.grGiSlipQuantity !== undefined && form.grGiSlipQuantity !== null) {
    query.grGiSlipQuantity = form.grGiSlipQuantity
  }
  if (form.length !== undefined && form.length !== null) {
    query.length = form.length
  }
  if (form.width !== undefined && form.width !== null) {
    query.width = form.width
  }
  if (form.height !== undefined && form.height !== null) {
    query.height = form.height
  }
  if (form.allowedPackagingWeight !== undefined && form.allowedPackagingWeight !== null) {
    query.allowedPackagingWeight = form.allowedPackagingWeight
  }
  if (form.allowedPackagingVolume !== undefined && form.allowedPackagingVolume !== null) {
    query.allowedPackagingVolume = form.allowedPackagingVolume
  }
  if (form.excessWeightTolerance !== undefined && form.excessWeightTolerance !== null) {
    query.excessWeightTolerance = form.excessWeightTolerance
  }
  if (form.excessVolumeTolerance !== undefined && form.excessVolumeTolerance !== null) {
    query.excessVolumeTolerance = form.excessVolumeTolerance
  }
  if (form.maximumLevelByVolume !== undefined && form.maximumLevelByVolume !== null) {
    query.maximumLevelByVolume = form.maximumLevelByVolume
  }
  if (form.stackingFactor !== undefined && form.stackingFactor !== null) {
    query.stackingFactor = form.stackingFactor
  }
  if (form.minimumRemainingShelfLife !== undefined && form.minimumRemainingShelfLife !== null) {
    query.minimumRemainingShelfLife = form.minimumRemainingShelfLife
  }
  if (form.totalShelfLife !== undefined && form.totalShelfLife !== null) {
    query.totalShelfLife = form.totalShelfLife
  }
  if (form.storagePercentage !== undefined && form.storagePercentage !== null) {
    query.storagePercentage = form.storagePercentage
  }
  if (form.netContents !== undefined && form.netContents !== null) {
    query.netContents = form.netContents
  }
  if (form.comparisonPriceUnit !== undefined && form.comparisonPriceUnit !== null) {
    query.comparisonPriceUnit = form.comparisonPriceUnit
  }
  if (form.grossContents !== undefined && form.grossContents !== null) {
    query.grossContents = form.grossContents
  }
  if (form.maximumAllowedCapacity !== undefined && form.maximumAllowedCapacity !== null) {
    query.maximumAllowedCapacity = form.maximumAllowedCapacity
  }
  if (form.overcapacityTolerance !== undefined && form.overcapacityTolerance !== null) {
    query.overcapacityTolerance = form.overcapacityTolerance
  }
  if (form.maximumPackingLength !== undefined && form.maximumPackingLength !== null) {
    query.maximumPackingLength = form.maximumPackingLength
  }
  if (form.maximumPackingWidth !== undefined && form.maximumPackingWidth !== null) {
    query.maximumPackingWidth = form.maximumPackingWidth
  }
  if (form.maximumPackingHeight !== undefined && form.maximumPackingHeight !== null) {
    query.maximumPackingHeight = form.maximumPackingHeight
  }
  if (form.quarantinePeriod !== undefined && form.quarantinePeriod !== null) {
    query.quarantinePeriod = form.quarantinePeriod
  }
  if (form.materialStatus !== undefined && form.materialStatus !== null) {
    query.materialStatus = form.materialStatus
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
function syncMasterSelection(record: MaterialRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMaterialId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as MaterialRowRecord
  const key = getMaterialId(row)
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
async function loadMaterialDetail(record: MaterialRowRecord): Promise<Material | null> {
  const id = getMaterialId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getMaterialById(id)
    const index = dataSource.value.findIndex((row) => getMaterialId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Material
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
    dataIndex: 'materialId',
    key: 'materialId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialId') ?? ''
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialCode') ?? ''
  },
  {
    title: pi.label('completeMaintenanceStatus'),
    dataIndex: 'completeMaintenanceStatus',
    key: 'completeMaintenanceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'completeMaintenanceStatus') ?? ''
  },
  {
    title: pi.label('maintenanceStatus'),
    dataIndex: 'maintenanceStatus',
    key: 'maintenanceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'maintenanceStatus') ?? ''
  },
  {
    title: pi.label('clientDeletionFlag'),
    dataIndex: 'clientDeletionFlag',
    key: 'clientDeletionFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'clientDeletionFlag') ?? ''
  },
  {
    title: pi.label('materialType'),
    dataIndex: 'materialType',
    key: 'materialType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialType') ?? ''
  },
  {
    title: pi.label('industrySector'),
    dataIndex: 'industrySector',
    key: 'industrySector',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'industrySector') ?? ''
  },
  {
    title: pi.label('materialGroup'),
    dataIndex: 'materialGroup',
    key: 'materialGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialGroup') ?? ''
  },
  {
    title: pi.label('oldMaterialNumber'),
    dataIndex: 'oldMaterialNumber',
    key: 'oldMaterialNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'oldMaterialNumber') ?? ''
  },
  {
    title: pi.label('baseUnit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'baseUnit') ?? ''
  },
  {
    title: pi.label('orderUnit'),
    dataIndex: 'orderUnit',
    key: 'orderUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'orderUnit') ?? ''
  },
  {
    title: pi.label('documentNumber'),
    dataIndex: 'documentNumber',
    key: 'documentNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'documentNumber') ?? ''
  },
  {
    title: pi.label('documentType'),
    dataIndex: 'documentType',
    key: 'documentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'documentType') ?? ''
  },
  {
    title: pi.label('documentVersion'),
    dataIndex: 'documentVersion',
    key: 'documentVersion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'documentVersion') ?? ''
  },
  {
    title: pi.label('documentPageFormat'),
    dataIndex: 'documentPageFormat',
    key: 'documentPageFormat',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'documentPageFormat') ?? ''
  },
  {
    title: pi.label('documentChangeNumber'),
    dataIndex: 'documentChangeNumber',
    key: 'documentChangeNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'documentChangeNumber') ?? ''
  },
  {
    title: pi.label('documentPageNumber'),
    dataIndex: 'documentPageNumber',
    key: 'documentPageNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'documentPageNumber') ?? ''
  },
  {
    title: pi.label('documentSheetCount'),
    dataIndex: 'documentSheetCount',
    key: 'documentSheetCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'documentSheetCount') ?? ''
  },
  {
    title: pi.label('productionInspectionMemo'),
    dataIndex: 'productionInspectionMemo',
    key: 'productionInspectionMemo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'productionInspectionMemo') ?? ''
  },
  {
    title: pi.label('productionMemoPageFormat'),
    dataIndex: 'productionMemoPageFormat',
    key: 'productionMemoPageFormat',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'productionMemoPageFormat') ?? ''
  },
  {
    title: pi.label('sizeDimensions'),
    dataIndex: 'sizeDimensions',
    key: 'sizeDimensions',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'sizeDimensions') ?? ''
  },
  {
    title: pi.label('basicMaterial'),
    dataIndex: 'basicMaterial',
    key: 'basicMaterial',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'basicMaterial') ?? ''
  },
  {
    title: pi.label('industryStandardDescription'),
    dataIndex: 'industryStandardDescription',
    key: 'industryStandardDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'industryStandardDescription') ?? ''
  },
  {
    title: pi.label('laboratoryDesignOffice'),
    dataIndex: 'laboratoryDesignOffice',
    key: 'laboratoryDesignOffice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'laboratoryDesignOffice') ?? ''
  },
  {
    title: pi.label('purchasingValueKey'),
    dataIndex: 'purchasingValueKey',
    key: 'purchasingValueKey',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'purchasingValueKey') ?? ''
  },
  {
    title: pi.label('grossWeight'),
    dataIndex: 'grossWeight',
    key: 'grossWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'grossWeight') ?? ''
  },
  {
    title: pi.label('netWeight'),
    dataIndex: 'netWeight',
    key: 'netWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'netWeight') ?? ''
  },
  {
    title: pi.label('weightUnit'),
    dataIndex: 'weightUnit',
    key: 'weightUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'weightUnit') ?? ''
  },
  {
    title: pi.label('volume'),
    dataIndex: 'volume',
    key: 'volume',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'volume') ?? ''
  },
  {
    title: pi.label('volumeUnit'),
    dataIndex: 'volumeUnit',
    key: 'volumeUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'volumeUnit') ?? ''
  },
  {
    title: pi.label('containerRequirements'),
    dataIndex: 'containerRequirements',
    key: 'containerRequirements',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'containerRequirements') ?? ''
  },
  {
    title: pi.label('storageConditions'),
    dataIndex: 'storageConditions',
    key: 'storageConditions',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'storageConditions') ?? ''
  },
  {
    title: pi.label('temperatureConditions'),
    dataIndex: 'temperatureConditions',
    key: 'temperatureConditions',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'temperatureConditions') ?? ''
  },
  {
    title: pi.label('lowLevelCode'),
    dataIndex: 'lowLevelCode',
    key: 'lowLevelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'lowLevelCode') ?? ''
  },
  {
    title: pi.label('transportationGroup'),
    dataIndex: 'transportationGroup',
    key: 'transportationGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'transportationGroup') ?? ''
  },
  {
    title: pi.label('hazardousMaterialNumber'),
    dataIndex: 'hazardousMaterialNumber',
    key: 'hazardousMaterialNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'hazardousMaterialNumber') ?? ''
  },
  {
    title: pi.label('division'),
    dataIndex: 'division',
    key: 'division',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'division') ?? ''
  },
  {
    title: pi.label('competitor'),
    dataIndex: 'competitor',
    key: 'competitor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'competitor') ?? ''
  },
  {
    title: pi.label('europeanArticleNumberObsolete'),
    dataIndex: 'europeanArticleNumberObsolete',
    key: 'europeanArticleNumberObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'europeanArticleNumberObsolete') ?? ''
  },
  {
    title: pi.label('grGiSlipQuantity'),
    dataIndex: 'grGiSlipQuantity',
    key: 'grGiSlipQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'grGiSlipQuantity') ?? ''
  },
  {
    title: pi.label('procurementRule'),
    dataIndex: 'procurementRule',
    key: 'procurementRule',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'procurementRule') ?? ''
  },
  {
    title: pi.label('sourceOfSupply'),
    dataIndex: 'sourceOfSupply',
    key: 'sourceOfSupply',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'sourceOfSupply') ?? ''
  },
  {
    title: pi.label('seasonCategory'),
    dataIndex: 'seasonCategory',
    key: 'seasonCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'seasonCategory') ?? ''
  },
  {
    title: pi.label('labelType'),
    dataIndex: 'labelType',
    key: 'labelType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'labelType') ?? ''
  },
  {
    title: pi.label('labelForm'),
    dataIndex: 'labelForm',
    key: 'labelForm',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'labelForm') ?? ''
  },
  {
    title: pi.label('deactivatedField'),
    dataIndex: 'deactivatedField',
    key: 'deactivatedField',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'deactivatedField') ?? ''
  },
  {
    title: pi.label('internationalArticleNumber'),
    dataIndex: 'internationalArticleNumber',
    key: 'internationalArticleNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'internationalArticleNumber') ?? ''
  },
  {
    title: pi.label('eanCategory'),
    dataIndex: 'eanCategory',
    key: 'eanCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'eanCategory') ?? ''
  },
  {
    title: pi.label('length'),
    dataIndex: 'length',
    key: 'length',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'length') ?? ''
  },
  {
    title: pi.label('width'),
    dataIndex: 'width',
    key: 'width',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'width') ?? ''
  },
  {
    title: pi.label('height'),
    dataIndex: 'height',
    key: 'height',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'height') ?? ''
  },
  {
    title: pi.label('dimensionUnit'),
    dataIndex: 'dimensionUnit',
    key: 'dimensionUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'dimensionUnit') ?? ''
  },
  {
    title: pi.label('productHierarchy'),
    dataIndex: 'productHierarchy',
    key: 'productHierarchy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'productHierarchy') ?? ''
  },
  {
    title: pi.label('stockTransferNetChangeCosting'),
    dataIndex: 'stockTransferNetChangeCosting',
    key: 'stockTransferNetChangeCosting',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'stockTransferNetChangeCosting') ?? ''
  },
  {
    title: pi.label('cadIndicator'),
    dataIndex: 'cadIndicator',
    key: 'cadIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'cadIndicator') ?? ''
  },
  {
    title: pi.label('qmInProcurement'),
    dataIndex: 'qmInProcurement',
    key: 'qmInProcurement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'qmInProcurement') ?? ''
  },
  {
    title: pi.label('allowedPackagingWeight'),
    dataIndex: 'allowedPackagingWeight',
    key: 'allowedPackagingWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'allowedPackagingWeight') ?? ''
  },
  {
    title: pi.label('allowedPackagingWeightUnit'),
    dataIndex: 'allowedPackagingWeightUnit',
    key: 'allowedPackagingWeightUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'allowedPackagingWeightUnit') ?? ''
  },
  {
    title: pi.label('allowedPackagingVolume'),
    dataIndex: 'allowedPackagingVolume',
    key: 'allowedPackagingVolume',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'allowedPackagingVolume') ?? ''
  },
  {
    title: pi.label('allowedPackagingVolumeUnit'),
    dataIndex: 'allowedPackagingVolumeUnit',
    key: 'allowedPackagingVolumeUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'allowedPackagingVolumeUnit') ?? ''
  },
  {
    title: pi.label('excessWeightTolerance'),
    dataIndex: 'excessWeightTolerance',
    key: 'excessWeightTolerance',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'excessWeightTolerance') ?? ''
  },
  {
    title: pi.label('excessVolumeTolerance'),
    dataIndex: 'excessVolumeTolerance',
    key: 'excessVolumeTolerance',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'excessVolumeTolerance') ?? ''
  },
  {
    title: pi.label('variablePurchaseOrderUnit'),
    dataIndex: 'variablePurchaseOrderUnit',
    key: 'variablePurchaseOrderUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'variablePurchaseOrderUnit') ?? ''
  },
  {
    title: pi.label('revisionLevelAssigned'),
    dataIndex: 'revisionLevelAssigned',
    key: 'revisionLevelAssigned',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'revisionLevelAssigned') ?? ''
  },
  {
    title: pi.label('configurableMaterial'),
    dataIndex: 'configurableMaterial',
    key: 'configurableMaterial',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'configurableMaterial') ?? ''
  },
  {
    title: pi.label('batchManagementRequired'),
    dataIndex: 'batchManagementRequired',
    key: 'batchManagementRequired',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'batchManagementRequired') ?? ''
  },
  {
    title: pi.label('packagingMaterialType'),
    dataIndex: 'packagingMaterialType',
    key: 'packagingMaterialType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'packagingMaterialType') ?? ''
  },
  {
    title: pi.label('maximumLevelByVolume'),
    dataIndex: 'maximumLevelByVolume',
    key: 'maximumLevelByVolume',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'maximumLevelByVolume') ?? ''
  },
  {
    title: pi.label('stackingFactor'),
    dataIndex: 'stackingFactor',
    key: 'stackingFactor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'stackingFactor') ?? ''
  },
  {
    title: pi.label('packagingMaterialGroup'),
    dataIndex: 'packagingMaterialGroup',
    key: 'packagingMaterialGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'packagingMaterialGroup') ?? ''
  },
  {
    title: pi.label('authorizationGroup'),
    dataIndex: 'authorizationGroup',
    key: 'authorizationGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'authorizationGroup') ?? ''
  },
  {
    title: pi.label('validFromDate'),
    dataIndex: 'validFromDate',
    key: 'validFromDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'validFromDate') ?? ''
  },
  {
    title: pi.label('seasonYear'),
    dataIndex: 'seasonYear',
    key: 'seasonYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'seasonYear') ?? ''
  },
  {
    title: pi.label('priceBandCategory'),
    dataIndex: 'priceBandCategory',
    key: 'priceBandCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'priceBandCategory') ?? ''
  },
  {
    title: pi.label('emptiesBillOfMaterial'),
    dataIndex: 'emptiesBillOfMaterial',
    key: 'emptiesBillOfMaterial',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'emptiesBillOfMaterial') ?? ''
  },
  {
    title: pi.label('externalMaterialGroup'),
    dataIndex: 'externalMaterialGroup',
    key: 'externalMaterialGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'externalMaterialGroup') ?? ''
  },
  {
    title: pi.label('crossPlantConfigurableMaterial'),
    dataIndex: 'crossPlantConfigurableMaterial',
    key: 'crossPlantConfigurableMaterial',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'crossPlantConfigurableMaterial') ?? ''
  },
  {
    title: pi.label('materialCategory'),
    dataIndex: 'materialCategory',
    key: 'materialCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialCategory') ?? ''
  },
  {
    title: pi.label('coProductIndicator'),
    dataIndex: 'coProductIndicator',
    key: 'coProductIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'coProductIndicator') ?? ''
  },
  {
    title: pi.label('followUpMaterialIndicator'),
    dataIndex: 'followUpMaterialIndicator',
    key: 'followUpMaterialIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'followUpMaterialIndicator') ?? ''
  },
  {
    title: pi.label('pricingReferenceMaterial'),
    dataIndex: 'pricingReferenceMaterial',
    key: 'pricingReferenceMaterial',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'pricingReferenceMaterial') ?? ''
  },
  {
    title: pi.label('crossPlantMaterialStatus'),
    dataIndex: 'crossPlantMaterialStatus',
    key: 'crossPlantMaterialStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'crossPlantMaterialStatus') ?? ''
  },
  {
    title: pi.label('crossDistributionChainStatus'),
    dataIndex: 'crossDistributionChainStatus',
    key: 'crossDistributionChainStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'crossDistributionChainStatus') ?? ''
  },
  {
    title: pi.label('crossPlantStatusValidFrom'),
    dataIndex: 'crossPlantStatusValidFrom',
    key: 'crossPlantStatusValidFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'crossPlantStatusValidFrom') ?? ''
  },
  {
    title: pi.label('crossDistributionStatusValidFrom'),
    dataIndex: 'crossDistributionStatusValidFrom',
    key: 'crossDistributionStatusValidFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'crossDistributionStatusValidFrom') ?? ''
  },
  {
    title: pi.label('taxClassification'),
    dataIndex: 'taxClassification',
    key: 'taxClassification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'taxClassification') ?? ''
  },
  {
    title: pi.label('catalogProfile'),
    dataIndex: 'catalogProfile',
    key: 'catalogProfile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'catalogProfile') ?? ''
  },
  {
    title: pi.label('minimumRemainingShelfLife'),
    dataIndex: 'minimumRemainingShelfLife',
    key: 'minimumRemainingShelfLife',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'minimumRemainingShelfLife') ?? ''
  },
  {
    title: pi.label('totalShelfLife'),
    dataIndex: 'totalShelfLife',
    key: 'totalShelfLife',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'totalShelfLife') ?? ''
  },
  {
    title: pi.label('storagePercentage'),
    dataIndex: 'storagePercentage',
    key: 'storagePercentage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'storagePercentage') ?? ''
  },
  {
    title: pi.label('contentUnit'),
    dataIndex: 'contentUnit',
    key: 'contentUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'contentUnit') ?? ''
  },
  {
    title: pi.label('netContents'),
    dataIndex: 'netContents',
    key: 'netContents',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'netContents') ?? ''
  },
  {
    title: pi.label('comparisonPriceUnit'),
    dataIndex: 'comparisonPriceUnit',
    key: 'comparisonPriceUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'comparisonPriceUnit') ?? ''
  },
  {
    title: pi.label('labelingMaterialGrouping'),
    dataIndex: 'labelingMaterialGrouping',
    key: 'labelingMaterialGrouping',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'labelingMaterialGrouping') ?? ''
  },
  {
    title: pi.label('grossContents'),
    dataIndex: 'grossContents',
    key: 'grossContents',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'grossContents') ?? ''
  },
  {
    title: pi.label('quantityConversionMethod'),
    dataIndex: 'quantityConversionMethod',
    key: 'quantityConversionMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'quantityConversionMethod') ?? ''
  },
  {
    title: pi.label('internalObjectNumber'),
    dataIndex: 'internalObjectNumber',
    key: 'internalObjectNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'internalObjectNumber') ?? ''
  },
  {
    title: pi.label('environmentallyRelevant'),
    dataIndex: 'environmentallyRelevant',
    key: 'environmentallyRelevant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'environmentallyRelevant') ?? ''
  },
  {
    title: pi.label('productAllocationProcedure'),
    dataIndex: 'productAllocationProcedure',
    key: 'productAllocationProcedure',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'productAllocationProcedure') ?? ''
  },
  {
    title: pi.label('variantPricingProfile'),
    dataIndex: 'variantPricingProfile',
    key: 'variantPricingProfile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'variantPricingProfile') ?? ''
  },
  {
    title: pi.label('discountInKind'),
    dataIndex: 'discountInKind',
    key: 'discountInKind',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'discountInKind') ?? ''
  },
  {
    title: pi.label('manufacturerPartNumber'),
    dataIndex: 'manufacturerPartNumber',
    key: 'manufacturerPartNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'manufacturerPartNumber') ?? ''
  },
  {
    title: pi.label('manufacturerNumber'),
    dataIndex: 'manufacturerNumber',
    key: 'manufacturerNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'manufacturerNumber') ?? ''
  },
  {
    title: pi.label('inventoryManagedMaterialNumber'),
    dataIndex: 'inventoryManagedMaterialNumber',
    key: 'inventoryManagedMaterialNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'inventoryManagedMaterialNumber') ?? ''
  },
  {
    title: pi.label('manufacturerPartProfile'),
    dataIndex: 'manufacturerPartProfile',
    key: 'manufacturerPartProfile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'manufacturerPartProfile') ?? ''
  },
  {
    title: pi.label('unitsOfMeasureUsage'),
    dataIndex: 'unitsOfMeasureUsage',
    key: 'unitsOfMeasureUsage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'unitsOfMeasureUsage') ?? ''
  },
  {
    title: pi.label('seasonRollout'),
    dataIndex: 'seasonRollout',
    key: 'seasonRollout',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'seasonRollout') ?? ''
  },
  {
    title: pi.label('dangerousGoodsProfile'),
    dataIndex: 'dangerousGoodsProfile',
    key: 'dangerousGoodsProfile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'dangerousGoodsProfile') ?? ''
  },
  {
    title: pi.label('highlyViscous'),
    dataIndex: 'highlyViscous',
    key: 'highlyViscous',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'highlyViscous') ?? ''
  },
  {
    title: pi.label('inBulkLiquid'),
    dataIndex: 'inBulkLiquid',
    key: 'inBulkLiquid',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'inBulkLiquid') ?? ''
  },
  {
    title: pi.label('serialNumberExplicitness'),
    dataIndex: 'serialNumberExplicitness',
    key: 'serialNumberExplicitness',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'serialNumberExplicitness') ?? ''
  },
  {
    title: pi.label('closedPackaging'),
    dataIndex: 'closedPackaging',
    key: 'closedPackaging',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'closedPackaging') ?? ''
  },
  {
    title: pi.label('approvedBatchRecordRequired'),
    dataIndex: 'approvedBatchRecordRequired',
    key: 'approvedBatchRecordRequired',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'approvedBatchRecordRequired') ?? ''
  },
  {
    title: pi.label('effectivityParameterOverride'),
    dataIndex: 'effectivityParameterOverride',
    key: 'effectivityParameterOverride',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'effectivityParameterOverride') ?? ''
  },
  {
    title: pi.label('materialCompletionLevel'),
    dataIndex: 'materialCompletionLevel',
    key: 'materialCompletionLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialCompletionLevel') ?? ''
  },
  {
    title: pi.label('shelfLifePeriodIndicator'),
    dataIndex: 'shelfLifePeriodIndicator',
    key: 'shelfLifePeriodIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'shelfLifePeriodIndicator') ?? ''
  },
  {
    title: pi.label('shelfLifeRoundingRule'),
    dataIndex: 'shelfLifeRoundingRule',
    key: 'shelfLifeRoundingRule',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'shelfLifeRoundingRule') ?? ''
  },
  {
    title: pi.label('productCompositionOnPackaging'),
    dataIndex: 'productCompositionOnPackaging',
    key: 'productCompositionOnPackaging',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'productCompositionOnPackaging') ?? ''
  },
  {
    title: pi.label('generalItemCategoryGroup'),
    dataIndex: 'generalItemCategoryGroup',
    key: 'generalItemCategoryGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'generalItemCategoryGroup') ?? ''
  },
  {
    title: pi.label('logisticalVariants'),
    dataIndex: 'logisticalVariants',
    key: 'logisticalVariants',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'logisticalVariants') ?? ''
  },
  {
    title: pi.label('materialLocked'),
    dataIndex: 'materialLocked',
    key: 'materialLocked',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialLocked') ?? ''
  },
  {
    title: pi.label('configurationManagementRelevant'),
    dataIndex: 'configurationManagementRelevant',
    key: 'configurationManagementRelevant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'configurationManagementRelevant') ?? ''
  },
  {
    title: pi.label('assortmentListType'),
    dataIndex: 'assortmentListType',
    key: 'assortmentListType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'assortmentListType') ?? ''
  },
  {
    title: pi.label('expirationDateType'),
    dataIndex: 'expirationDateType',
    key: 'expirationDateType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'expirationDateType') ?? ''
  },
  {
    title: pi.label('gtinVariant'),
    dataIndex: 'gtinVariant',
    key: 'gtinVariant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'gtinVariant') ?? ''
  },
  {
    title: pi.label('genericMaterialNumber'),
    dataIndex: 'genericMaterialNumber',
    key: 'genericMaterialNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'genericMaterialNumber') ?? ''
  },
  {
    title: pi.label('samePackingReferenceMaterial'),
    dataIndex: 'samePackingReferenceMaterial',
    key: 'samePackingReferenceMaterial',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'samePackingReferenceMaterial') ?? ''
  },
  {
    title: pi.label('globalDataSyncRelevant'),
    dataIndex: 'globalDataSyncRelevant',
    key: 'globalDataSyncRelevant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'globalDataSyncRelevant') ?? ''
  },
  {
    title: pi.label('acceptanceAtOrigin'),
    dataIndex: 'acceptanceAtOrigin',
    key: 'acceptanceAtOrigin',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'acceptanceAtOrigin') ?? ''
  },
  {
    title: pi.label('standardHuType'),
    dataIndex: 'standardHuType',
    key: 'standardHuType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'standardHuType') ?? ''
  },
  {
    title: pi.label('pilferable'),
    dataIndex: 'pilferable',
    key: 'pilferable',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'pilferable') ?? ''
  },
  {
    title: pi.label('warehouseStorageCondition'),
    dataIndex: 'warehouseStorageCondition',
    key: 'warehouseStorageCondition',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'warehouseStorageCondition') ?? ''
  },
  {
    title: pi.label('warehouseMaterialGroup'),
    dataIndex: 'warehouseMaterialGroup',
    key: 'warehouseMaterialGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'warehouseMaterialGroup') ?? ''
  },
  {
    title: pi.label('handlingIndicator'),
    dataIndex: 'handlingIndicator',
    key: 'handlingIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'handlingIndicator') ?? ''
  },
  {
    title: pi.label('hazardousSubstancesRelevant'),
    dataIndex: 'hazardousSubstancesRelevant',
    key: 'hazardousSubstancesRelevant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'hazardousSubstancesRelevant') ?? ''
  },
  {
    title: pi.label('handlingUnitType'),
    dataIndex: 'handlingUnitType',
    key: 'handlingUnitType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'handlingUnitType') ?? ''
  },
  {
    title: pi.label('variableTareWeight'),
    dataIndex: 'variableTareWeight',
    key: 'variableTareWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'variableTareWeight') ?? ''
  },
  {
    title: pi.label('maximumAllowedCapacity'),
    dataIndex: 'maximumAllowedCapacity',
    key: 'maximumAllowedCapacity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'maximumAllowedCapacity') ?? ''
  },
  {
    title: pi.label('overcapacityTolerance'),
    dataIndex: 'overcapacityTolerance',
    key: 'overcapacityTolerance',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'overcapacityTolerance') ?? ''
  },
  {
    title: pi.label('maximumPackingLength'),
    dataIndex: 'maximumPackingLength',
    key: 'maximumPackingLength',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'maximumPackingLength') ?? ''
  },
  {
    title: pi.label('maximumPackingWidth'),
    dataIndex: 'maximumPackingWidth',
    key: 'maximumPackingWidth',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'maximumPackingWidth') ?? ''
  },
  {
    title: pi.label('maximumPackingHeight'),
    dataIndex: 'maximumPackingHeight',
    key: 'maximumPackingHeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'maximumPackingHeight') ?? ''
  },
  {
    title: pi.label('maximumPackingDimensionUnit'),
    dataIndex: 'maximumPackingDimensionUnit',
    key: 'maximumPackingDimensionUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'maximumPackingDimensionUnit') ?? ''
  },
  {
    title: pi.label('countryOfOrigin'),
    dataIndex: 'countryOfOrigin',
    key: 'countryOfOrigin',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'countryOfOrigin') ?? ''
  },
  {
    title: pi.label('materialFreightGroup'),
    dataIndex: 'materialFreightGroup',
    key: 'materialFreightGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialFreightGroup') ?? ''
  },
  {
    title: pi.label('quarantinePeriod'),
    dataIndex: 'quarantinePeriod',
    key: 'quarantinePeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'quarantinePeriod') ?? ''
  },
  {
    title: pi.label('quarantinePeriodUnit'),
    dataIndex: 'quarantinePeriodUnit',
    key: 'quarantinePeriodUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'quarantinePeriodUnit') ?? ''
  },
  {
    title: pi.label('qualityInspectionGroup'),
    dataIndex: 'qualityInspectionGroup',
    key: 'qualityInspectionGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'qualityInspectionGroup') ?? ''
  },
  {
    title: pi.label('serialNumberProfile'),
    dataIndex: 'serialNumberProfile',
    key: 'serialNumberProfile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'serialNumberProfile') ?? ''
  },
  {
    title: pi.label('formName'),
    dataIndex: 'formName',
    key: 'formName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'formName') ?? ''
  },
  {
    title: pi.label('logisticsUnitOfMeasure'),
    dataIndex: 'logisticsUnitOfMeasure',
    key: 'logisticsUnitOfMeasure',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'logisticsUnitOfMeasure') ?? ''
  },
  {
    title: pi.label('catchWeightMaterial'),
    dataIndex: 'catchWeightMaterial',
    key: 'catchWeightMaterial',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'catchWeightMaterial') ?? ''
  },
  {
    title: pi.label('catchWeightProfile'),
    dataIndex: 'catchWeightProfile',
    key: 'catchWeightProfile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'catchWeightProfile') ?? ''
  },
  {
    title: pi.label('catchWeightToleranceGroup'),
    dataIndex: 'catchWeightToleranceGroup',
    key: 'catchWeightToleranceGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'catchWeightToleranceGroup') ?? ''
  },
  {
    title: pi.label('adjustmentProfile'),
    dataIndex: 'adjustmentProfile',
    key: 'adjustmentProfile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'adjustmentProfile') ?? ''
  },
  {
    title: pi.label('intellectualPropertyId'),
    dataIndex: 'intellectualPropertyId',
    key: 'intellectualPropertyId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'intellectualPropertyId') ?? ''
  },
  {
    title: pi.label('variantPriceAllowed'),
    dataIndex: 'variantPriceAllowed',
    key: 'variantPriceAllowed',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'variantPriceAllowed') ?? ''
  },
  {
    title: pi.label('medium'),
    dataIndex: 'medium',
    key: 'medium',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'medium') ?? ''
  },
  {
    title: pi.label('physicalCommodity'),
    dataIndex: 'physicalCommodity',
    key: 'physicalCommodity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'physicalCommodity') ?? ''
  },
  {
    title: pi.label('animalOrigin'),
    dataIndex: 'animalOrigin',
    key: 'animalOrigin',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'animalOrigin') ?? ''
  },
  {
    title: pi.label('textileCompositionFunction'),
    dataIndex: 'textileCompositionFunction',
    key: 'textileCompositionFunction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'textileCompositionFunction') ?? ''
  },
  {
    title: pi.label('segmentationStructure'),
    dataIndex: 'segmentationStructure',
    key: 'segmentationStructure',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'segmentationStructure') ?? ''
  },
  {
    title: pi.label('segmentationStrategy'),
    dataIndex: 'segmentationStrategy',
    key: 'segmentationStrategy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'segmentationStrategy') ?? ''
  },
  {
    title: pi.label('segmentationStatus'),
    dataIndex: 'segmentationStatus',
    key: 'segmentationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'segmentationStatus') ?? ''
  },
  {
    title: pi.label('segmentationScope'),
    dataIndex: 'segmentationScope',
    key: 'segmentationScope',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'segmentationScope') ?? ''
  },
  {
    title: pi.label('segmentationRelevant'),
    dataIndex: 'segmentationRelevant',
    key: 'segmentationRelevant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'segmentationRelevant') ?? ''
  },
  {
    title: pi.label('fashionAttribute1'),
    dataIndex: 'fashionAttribute1',
    key: 'fashionAttribute1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fashionAttribute1') ?? ''
  },
  {
    title: pi.label('fashionAttribute2'),
    dataIndex: 'fashionAttribute2',
    key: 'fashionAttribute2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fashionAttribute2') ?? ''
  },
  {
    title: pi.label('fashionAttribute3'),
    dataIndex: 'fashionAttribute3',
    key: 'fashionAttribute3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fashionAttribute3') ?? ''
  },
  {
    title: pi.label('seasonUsageIndicator'),
    dataIndex: 'seasonUsageIndicator',
    key: 'seasonUsageIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'seasonUsageIndicator') ?? ''
  },
  {
    title: pi.label('seasonActiveInInventory'),
    dataIndex: 'seasonActiveInInventory',
    key: 'seasonActiveInInventory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'seasonActiveInInventory') ?? ''
  },
  {
    title: pi.label('characteristicConversionId'),
    dataIndex: 'characteristicConversionId',
    key: 'characteristicConversionId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'characteristicConversionId') ?? ''
  },
  {
    title: pi.label('anpCode'),
    dataIndex: 'anpCode',
    key: 'anpCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'anpCode') ?? ''
  },
  {
    title: pi.label('dangerousGoodsPackagingStatus'),
    dataIndex: 'dangerousGoodsPackagingStatus',
    key: 'dangerousGoodsPackagingStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'dangerousGoodsPackagingStatus') ?? ''
  },
  {
    title: pi.label('materialConditionManagement'),
    dataIndex: 'materialConditionManagement',
    key: 'materialConditionManagement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialConditionManagement') ?? ''
  },
  {
    title: pi.label('returnCode'),
    dataIndex: 'returnCode',
    key: 'returnCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'returnCode') ?? ''
  },
  {
    title: pi.label('returnToLogisticsLevel'),
    dataIndex: 'returnToLogisticsLevel',
    key: 'returnToLogisticsLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'returnToLogisticsLevel') ?? ''
  },
  {
    title: pi.label('natoItemIdentificationNumber'),
    dataIndex: 'natoItemIdentificationNumber',
    key: 'natoItemIdentificationNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'natoItemIdentificationNumber') ?? ''
  },
  {
    title: pi.label('fffClass'),
    dataIndex: 'fffClass',
    key: 'fffClass',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fffClass') ?? ''
  },
  {
    title: pi.label('supersessionChainNumber'),
    dataIndex: 'supersessionChainNumber',
    key: 'supersessionChainNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'supersessionChainNumber') ?? ''
  },
  {
    title: pi.label('seasonalProcurementCreationStatus'),
    dataIndex: 'seasonalProcurementCreationStatus',
    key: 'seasonalProcurementCreationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'seasonalProcurementCreationStatus') ?? ''
  },
  {
    title: pi.label('colorCharacteristicInternalNumber'),
    dataIndex: 'colorCharacteristicInternalNumber',
    key: 'colorCharacteristicInternalNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'colorCharacteristicInternalNumber') ?? ''
  },
  {
    title: pi.label('mainSizeCharacteristicInternalNumber'),
    dataIndex: 'mainSizeCharacteristicInternalNumber',
    key: 'mainSizeCharacteristicInternalNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'mainSizeCharacteristicInternalNumber') ?? ''
  },
  {
    title: pi.label('secondSizeCharacteristicInternalNumber'),
    dataIndex: 'secondSizeCharacteristicInternalNumber',
    key: 'secondSizeCharacteristicInternalNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'secondSizeCharacteristicInternalNumber') ?? ''
  },
  {
    title: pi.label('color'),
    dataIndex: 'color',
    key: 'color',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'color') ?? ''
  },
  {
    title: pi.label('mainSize'),
    dataIndex: 'mainSize',
    key: 'mainSize',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'mainSize') ?? ''
  },
  {
    title: pi.label('secondSize'),
    dataIndex: 'secondSize',
    key: 'secondSize',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'secondSize') ?? ''
  },
  {
    title: pi.label('evaluationCharacteristicValue'),
    dataIndex: 'evaluationCharacteristicValue',
    key: 'evaluationCharacteristicValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'evaluationCharacteristicValue') ?? ''
  },
  {
    title: pi.label('careCode'),
    dataIndex: 'careCode',
    key: 'careCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'careCode') ?? ''
  },
  {
    title: pi.label('brandId'),
    dataIndex: 'brandId',
    key: 'brandId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'brandId') ?? ''
  },
  {
    title: pi.label('fiberCode1'),
    dataIndex: 'fiberCode1',
    key: 'fiberCode1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberCode1') ?? ''
  },
  {
    title: pi.label('fiberPart1'),
    dataIndex: 'fiberPart1',
    key: 'fiberPart1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberPart1') ?? ''
  },
  {
    title: pi.label('fiberCode2'),
    dataIndex: 'fiberCode2',
    key: 'fiberCode2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberCode2') ?? ''
  },
  {
    title: pi.label('fiberPart2'),
    dataIndex: 'fiberPart2',
    key: 'fiberPart2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberPart2') ?? ''
  },
  {
    title: pi.label('fiberCode3'),
    dataIndex: 'fiberCode3',
    key: 'fiberCode3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberCode3') ?? ''
  },
  {
    title: pi.label('fiberPart3'),
    dataIndex: 'fiberPart3',
    key: 'fiberPart3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberPart3') ?? ''
  },
  {
    title: pi.label('fiberCode4'),
    dataIndex: 'fiberCode4',
    key: 'fiberCode4',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberCode4') ?? ''
  },
  {
    title: pi.label('fiberPart4'),
    dataIndex: 'fiberPart4',
    key: 'fiberPart4',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberPart4') ?? ''
  },
  {
    title: pi.label('fiberCode5'),
    dataIndex: 'fiberCode5',
    key: 'fiberCode5',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberCode5') ?? ''
  },
  {
    title: pi.label('fiberPart5'),
    dataIndex: 'fiberPart5',
    key: 'fiberPart5',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fiberPart5') ?? ''
  },
  {
    title: pi.label('fashionGrade'),
    dataIndex: 'fashionGrade',
    key: 'fashionGrade',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'fashionGrade') ?? ''
  },
  {
    title: pi.label('materialStatus'),
    dataIndex: 'materialStatus',
    key: 'materialStatus',
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
        permission: 'logistics:materials:material:update',
        onClick: (record: MaterialRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:material:delete',
        onClick: (record: MaterialRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaterialId = (record: MaterialRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaterialField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getMaterialDictValue = (
  record: MaterialRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 将行字段/字典值转为有限 number */
const toMaterialNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}



/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaterialRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: MaterialRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getMaterialId(selectedRow.value) === getMaterialId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaterialRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getMaterialList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Material] 加载数据失败', { error })
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
  materialCode: '',
  completeMaintenanceStatus: '',
  maintenanceStatus: '',
  clientDeletionFlag: '',
  materialType: '',
  industrySector: '',
  materialGroup: '',
  oldMaterialNumber: '',
  baseUnit: '',
  orderUnit: '',
  documentNumber: '',
  documentType: '',
  documentVersion: '',
  documentPageFormat: '',
  documentChangeNumber: '',
  documentPageNumber: '',
  documentSheetCount: '',
  productionInspectionMemo: '',
  productionMemoPageFormat: '',
  sizeDimensions: '',
  basicMaterial: '',
  industryStandardDescription: '',
  laboratoryDesignOffice: '',
  purchasingValueKey: '',
  grossWeight: undefined as number | undefined,
  netWeight: undefined as number | undefined,
  weightUnit: '',
  volume: undefined as number | undefined,
  volumeUnit: '',
  containerRequirements: '',
  storageConditions: '',
  temperatureConditions: '',
  lowLevelCode: '',
  transportationGroup: '',
  hazardousMaterialNumber: '',
  division: '',
  competitor: '',
  europeanArticleNumberObsolete: '',
  grGiSlipQuantity: undefined as number | undefined,
  procurementRule: '',
  sourceOfSupply: '',
  seasonCategory: '',
  labelType: '',
  labelForm: '',
  deactivatedField: '',
  internationalArticleNumber: '',
  eanCategory: '',
  length: undefined as number | undefined,
  width: undefined as number | undefined,
  height: undefined as number | undefined,
  dimensionUnit: '',
  productHierarchy: '',
  stockTransferNetChangeCosting: '',
  cadIndicator: '',
  qmInProcurement: '',
  allowedPackagingWeight: undefined as number | undefined,
  allowedPackagingWeightUnit: '',
  allowedPackagingVolume: undefined as number | undefined,
  allowedPackagingVolumeUnit: '',
  excessWeightTolerance: undefined as number | undefined,
  excessVolumeTolerance: undefined as number | undefined,
  variablePurchaseOrderUnit: '',
  revisionLevelAssigned: '',
  configurableMaterial: '',
  batchManagementRequired: '',
  packagingMaterialType: '',
  maximumLevelByVolume: undefined as number | undefined,
  stackingFactor: undefined as number | undefined,
  packagingMaterialGroup: '',
  authorizationGroup: '',
  validFromDateStart: '',
  validFromDateEnd: '',
  seasonYear: '',
  priceBandCategory: '',
  emptiesBillOfMaterial: '',
  externalMaterialGroup: '',
  crossPlantConfigurableMaterial: '',
  materialCategory: '',
  coProductIndicator: '',
  followUpMaterialIndicator: '',
  pricingReferenceMaterial: '',
  crossPlantMaterialStatus: '',
  crossDistributionChainStatus: '',
  crossPlantStatusValidFromStart: '',
  crossPlantStatusValidFromEnd: '',
  crossDistributionStatusValidFromStart: '',
  crossDistributionStatusValidFromEnd: '',
  taxClassification: '',
  catalogProfile: '',
  minimumRemainingShelfLife: undefined as number | undefined,
  totalShelfLife: undefined as number | undefined,
  storagePercentage: undefined as number | undefined,
  contentUnit: '',
  netContents: undefined as number | undefined,
  comparisonPriceUnit: undefined as number | undefined,
  labelingMaterialGrouping: '',
  grossContents: undefined as number | undefined,
  quantityConversionMethod: '',
  internalObjectNumber: '',
  environmentallyRelevant: '',
  productAllocationProcedure: '',
  variantPricingProfile: '',
  discountInKind: '',
  manufacturerPartNumber: '',
  manufacturerNumber: '',
  inventoryManagedMaterialNumber: '',
  manufacturerPartProfile: '',
  unitsOfMeasureUsage: '',
  seasonRollout: '',
  dangerousGoodsProfile: '',
  highlyViscous: '',
  inBulkLiquid: '',
  serialNumberExplicitness: '',
  closedPackaging: '',
  approvedBatchRecordRequired: '',
  effectivityParameterOverride: '',
  materialCompletionLevel: '',
  shelfLifePeriodIndicator: '',
  shelfLifeRoundingRule: '',
  productCompositionOnPackaging: '',
  generalItemCategoryGroup: '',
  logisticalVariants: '',
  materialLocked: '',
  configurationManagementRelevant: '',
  assortmentListType: '',
  expirationDateType: '',
  gtinVariant: '',
  genericMaterialNumber: '',
  samePackingReferenceMaterial: '',
  globalDataSyncRelevant: '',
  acceptanceAtOrigin: '',
  standardHuType: '',
  pilferable: '',
  warehouseStorageCondition: '',
  warehouseMaterialGroup: '',
  handlingIndicator: '',
  hazardousSubstancesRelevant: '',
  handlingUnitType: '',
  variableTareWeight: '',
  maximumAllowedCapacity: undefined as number | undefined,
  overcapacityTolerance: undefined as number | undefined,
  maximumPackingLength: undefined as number | undefined,
  maximumPackingWidth: undefined as number | undefined,
  maximumPackingHeight: undefined as number | undefined,
  maximumPackingDimensionUnit: '',
  countryOfOrigin: '',
  materialFreightGroup: '',
  quarantinePeriod: undefined as number | undefined,
  quarantinePeriodUnit: '',
  qualityInspectionGroup: '',
  serialNumberProfile: '',
  formName: '',
  logisticsUnitOfMeasure: '',
  catchWeightMaterial: '',
  catchWeightProfile: '',
  catchWeightToleranceGroup: '',
  adjustmentProfile: '',
  intellectualPropertyId: '',
  variantPriceAllowed: '',
  medium: '',
  physicalCommodity: '',
  animalOrigin: '',
  textileCompositionFunction: '',
  segmentationStructure: '',
  segmentationStrategy: '',
  segmentationStatus: '',
  segmentationScope: '',
  segmentationRelevant: '',
  fashionAttribute1: '',
  fashionAttribute2: '',
  fashionAttribute3: '',
  seasonUsageIndicator: '',
  seasonActiveInInventory: '',
  characteristicConversionId: '',
  anpCode: '',
  dangerousGoodsPackagingStatus: '',
  materialConditionManagement: '',
  returnCode: '',
  returnToLogisticsLevel: '',
  natoItemIdentificationNumber: '',
  fffClass: '',
  supersessionChainNumber: '',
  seasonalProcurementCreationStatus: '',
  colorCharacteristicInternalNumber: '',
  mainSizeCharacteristicInternalNumber: '',
  secondSizeCharacteristicInternalNumber: '',
  color: '',
  mainSize: '',
  secondSize: '',
  evaluationCharacteristicValue: '',
  careCode: '',
  brandId: '',
  fiberCode1: '',
  fiberPart1: '',
  fiberCode2: '',
  fiberPart2: '',
  fiberCode3: '',
  fiberPart3: '',
  fiberCode4: '',
  fiberPart4: '',
  fiberCode5: '',
  fiberPart5: '',
  fashionGrade: '',
  materialStatus: undefined as number | undefined,
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
async function handleEdit(record: MaterialRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadMaterialDetail(record)
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
      await updateMaterial(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createMaterial(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  materialDescriptionPanelRef.value?.reload?.()
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
  const res = await getMaterialTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importMaterial(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    materialDescriptionPanelRef.value?.reload?.()
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
    const exportMeta = await exportMaterial(
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
    logger.error('[Material] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MaterialRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialById((record as any)[entityIdName])
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
      await deleteMaterialBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
async function handleMaterialStatusChange(record: MaterialRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toMaterialNumber(getMaterialDictValue(record, 'materialStatus'))
  const id = getMaterialId(record)
  const row = dataSource.value.find((item) => getMaterialId(item) === id)
  if (row) {
    row.materialStatus = newVal
  }
  try {
    await updateMaterialStatus({ materialId: id, materialStatus: newVal })
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
  materialCode: '',
  completeMaintenanceStatus: '',
  maintenanceStatus: '',
  clientDeletionFlag: '',
  materialType: '',
  industrySector: '',
  materialGroup: '',
  oldMaterialNumber: '',
  baseUnit: '',
  orderUnit: '',
  documentNumber: '',
  documentType: '',
  documentVersion: '',
  documentPageFormat: '',
  documentChangeNumber: '',
  documentPageNumber: '',
  documentSheetCount: '',
  productionInspectionMemo: '',
  productionMemoPageFormat: '',
  sizeDimensions: '',
  basicMaterial: '',
  industryStandardDescription: '',
  laboratoryDesignOffice: '',
  purchasingValueKey: '',
  grossWeight: undefined as number | undefined,
  netWeight: undefined as number | undefined,
  weightUnit: '',
  volume: undefined as number | undefined,
  volumeUnit: '',
  containerRequirements: '',
  storageConditions: '',
  temperatureConditions: '',
  lowLevelCode: '',
  transportationGroup: '',
  hazardousMaterialNumber: '',
  division: '',
  competitor: '',
  europeanArticleNumberObsolete: '',
  grGiSlipQuantity: undefined as number | undefined,
  procurementRule: '',
  sourceOfSupply: '',
  seasonCategory: '',
  labelType: '',
  labelForm: '',
  deactivatedField: '',
  internationalArticleNumber: '',
  eanCategory: '',
  length: undefined as number | undefined,
  width: undefined as number | undefined,
  height: undefined as number | undefined,
  dimensionUnit: '',
  productHierarchy: '',
  stockTransferNetChangeCosting: '',
  cadIndicator: '',
  qmInProcurement: '',
  allowedPackagingWeight: undefined as number | undefined,
  allowedPackagingWeightUnit: '',
  allowedPackagingVolume: undefined as number | undefined,
  allowedPackagingVolumeUnit: '',
  excessWeightTolerance: undefined as number | undefined,
  excessVolumeTolerance: undefined as number | undefined,
  variablePurchaseOrderUnit: '',
  revisionLevelAssigned: '',
  configurableMaterial: '',
  batchManagementRequired: '',
  packagingMaterialType: '',
  maximumLevelByVolume: undefined as number | undefined,
  stackingFactor: undefined as number | undefined,
  packagingMaterialGroup: '',
  authorizationGroup: '',
  validFromDateStart: '',
  validFromDateEnd: '',
  seasonYear: '',
  priceBandCategory: '',
  emptiesBillOfMaterial: '',
  externalMaterialGroup: '',
  crossPlantConfigurableMaterial: '',
  materialCategory: '',
  coProductIndicator: '',
  followUpMaterialIndicator: '',
  pricingReferenceMaterial: '',
  crossPlantMaterialStatus: '',
  crossDistributionChainStatus: '',
  crossPlantStatusValidFromStart: '',
  crossPlantStatusValidFromEnd: '',
  crossDistributionStatusValidFromStart: '',
  crossDistributionStatusValidFromEnd: '',
  taxClassification: '',
  catalogProfile: '',
  minimumRemainingShelfLife: undefined as number | undefined,
  totalShelfLife: undefined as number | undefined,
  storagePercentage: undefined as number | undefined,
  contentUnit: '',
  netContents: undefined as number | undefined,
  comparisonPriceUnit: undefined as number | undefined,
  labelingMaterialGrouping: '',
  grossContents: undefined as number | undefined,
  quantityConversionMethod: '',
  internalObjectNumber: '',
  environmentallyRelevant: '',
  productAllocationProcedure: '',
  variantPricingProfile: '',
  discountInKind: '',
  manufacturerPartNumber: '',
  manufacturerNumber: '',
  inventoryManagedMaterialNumber: '',
  manufacturerPartProfile: '',
  unitsOfMeasureUsage: '',
  seasonRollout: '',
  dangerousGoodsProfile: '',
  highlyViscous: '',
  inBulkLiquid: '',
  serialNumberExplicitness: '',
  closedPackaging: '',
  approvedBatchRecordRequired: '',
  effectivityParameterOverride: '',
  materialCompletionLevel: '',
  shelfLifePeriodIndicator: '',
  shelfLifeRoundingRule: '',
  productCompositionOnPackaging: '',
  generalItemCategoryGroup: '',
  logisticalVariants: '',
  materialLocked: '',
  configurationManagementRelevant: '',
  assortmentListType: '',
  expirationDateType: '',
  gtinVariant: '',
  genericMaterialNumber: '',
  samePackingReferenceMaterial: '',
  globalDataSyncRelevant: '',
  acceptanceAtOrigin: '',
  standardHuType: '',
  pilferable: '',
  warehouseStorageCondition: '',
  warehouseMaterialGroup: '',
  handlingIndicator: '',
  hazardousSubstancesRelevant: '',
  handlingUnitType: '',
  variableTareWeight: '',
  maximumAllowedCapacity: undefined as number | undefined,
  overcapacityTolerance: undefined as number | undefined,
  maximumPackingLength: undefined as number | undefined,
  maximumPackingWidth: undefined as number | undefined,
  maximumPackingHeight: undefined as number | undefined,
  maximumPackingDimensionUnit: '',
  countryOfOrigin: '',
  materialFreightGroup: '',
  quarantinePeriod: undefined as number | undefined,
  quarantinePeriodUnit: '',
  qualityInspectionGroup: '',
  serialNumberProfile: '',
  formName: '',
  logisticsUnitOfMeasure: '',
  catchWeightMaterial: '',
  catchWeightProfile: '',
  catchWeightToleranceGroup: '',
  adjustmentProfile: '',
  intellectualPropertyId: '',
  variantPriceAllowed: '',
  medium: '',
  physicalCommodity: '',
  animalOrigin: '',
  textileCompositionFunction: '',
  segmentationStructure: '',
  segmentationStrategy: '',
  segmentationStatus: '',
  segmentationScope: '',
  segmentationRelevant: '',
  fashionAttribute1: '',
  fashionAttribute2: '',
  fashionAttribute3: '',
  seasonUsageIndicator: '',
  seasonActiveInInventory: '',
  characteristicConversionId: '',
  anpCode: '',
  dangerousGoodsPackagingStatus: '',
  materialConditionManagement: '',
  returnCode: '',
  returnToLogisticsLevel: '',
  natoItemIdentificationNumber: '',
  fffClass: '',
  supersessionChainNumber: '',
  seasonalProcurementCreationStatus: '',
  colorCharacteristicInternalNumber: '',
  mainSizeCharacteristicInternalNumber: '',
  secondSizeCharacteristicInternalNumber: '',
  color: '',
  mainSize: '',
  secondSize: '',
  evaluationCharacteristicValue: '',
  careCode: '',
  brandId: '',
  fiberCode1: '',
  fiberPart1: '',
  fiberCode2: '',
  fiberPart2: '',
  fiberCode3: '',
  fiberPart3: '',
  fiberCode4: '',
  fiberPart4: '',
  fiberCode5: '',
  fiberPart5: '',
  fashionGrade: '',
  materialStatus: undefined as number | undefined,
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
