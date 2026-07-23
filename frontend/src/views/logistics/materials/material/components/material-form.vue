<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material/components -->
<!-- 文件名称：material-form.vue -->
<!-- 功能描述：Takt全局物料实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="pi.ph('materialCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.materialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('completeMaintenanceStatus')"
                name="completeMaintenanceStatus"
              >
                <a-input
                  v-model:value="formState.completeMaintenanceStatus"
                  :placeholder="pi.ph('completeMaintenanceStatus')"
                  show-count
                  :maxlength="15"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceStatus')"
                name="maintenanceStatus"
              >
                <a-input
                  v-model:value="formState.maintenanceStatus"
                  :placeholder="pi.ph('maintenanceStatus')"
                  show-count
                  :maxlength="15"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientDeletionFlag')"
                name="clientDeletionFlag"
              >
                <a-input
                  v-model:value="formState.clientDeletionFlag"
                  :placeholder="pi.ph('clientDeletionFlag')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialType')"
                name="materialType"
              >
                <a-input
                  v-model:value="formState.materialType"
                  :placeholder="pi.ph('materialType')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('industrySector')"
                name="industrySector"
              >
                <a-input
                  v-model:value="formState.industrySector"
                  :placeholder="pi.ph('industrySector')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialGroup')"
                name="materialGroup"
              >
                <a-input
                  v-model:value="formState.materialGroup"
                  :placeholder="pi.ph('materialGroup')"
                  show-count
                  :maxlength="9"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('oldMaterialNumber')"
                name="oldMaterialNumber"
              >
                <a-input
                  v-model:value="formState.oldMaterialNumber"
                  :placeholder="pi.ph('oldMaterialNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('baseUnit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="pi.ph('baseUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderUnit')"
                name="orderUnit"
              >
                <a-input
                  v-model:value="formState.orderUnit"
                  :placeholder="pi.ph('orderUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentNumber')"
                name="documentNumber"
              >
                <a-input
                  v-model:value="formState.documentNumber"
                  :placeholder="pi.ph('documentNumber')"
                  show-count
                  :maxlength="22"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentType')"
                name="documentType"
              >
                <a-input
                  v-model:value="formState.documentType"
                  :placeholder="pi.ph('documentType')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentVersion')"
                name="documentVersion"
              >
                <a-input
                  v-model:value="formState.documentVersion"
                  :placeholder="pi.ph('documentVersion')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentPageFormat')"
                name="documentPageFormat"
              >
                <a-input
                  v-model:value="formState.documentPageFormat"
                  :placeholder="pi.ph('documentPageFormat')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentChangeNumber')"
                name="documentChangeNumber"
              >
                <a-input
                  v-model:value="formState.documentChangeNumber"
                  :placeholder="pi.ph('documentChangeNumber')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentPageNumber')"
                name="documentPageNumber"
              >
                <a-input
                  v-model:value="formState.documentPageNumber"
                  :placeholder="pi.ph('documentPageNumber')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentSheetCount')"
                name="documentSheetCount"
              >
                <a-input
                  v-model:value="formState.documentSheetCount"
                  :placeholder="pi.ph('documentSheetCount')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionInspectionMemo')"
                name="productionInspectionMemo"
              >
                <a-input
                  v-model:value="formState.productionInspectionMemo"
                  :placeholder="pi.ph('productionInspectionMemo')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionMemoPageFormat')"
                name="productionMemoPageFormat"
              >
                <a-input
                  v-model:value="formState.productionMemoPageFormat"
                  :placeholder="pi.ph('productionMemoPageFormat')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sizeDimensions')"
                name="sizeDimensions"
              >
                <a-input
                  v-model:value="formState.sizeDimensions"
                  :placeholder="pi.ph('sizeDimensions')"
                  show-count
                  :maxlength="32"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('basicMaterial')"
                name="basicMaterial"
              >
                <a-input
                  v-model:value="formState.basicMaterial"
                  :placeholder="pi.ph('basicMaterial')"
                  show-count
                  :maxlength="48"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('industryStandardDescription')"
                name="industryStandardDescription"
              >
                <a-textarea
                  v-model:value="formState.industryStandardDescription"
                  :placeholder="pi.ph('industryStandardDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('laboratoryDesignOffice')"
                name="laboratoryDesignOffice"
              >
                <a-input
                  v-model:value="formState.laboratoryDesignOffice"
                  :placeholder="pi.ph('laboratoryDesignOffice')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchasingValueKey')"
                name="purchasingValueKey"
              >
                <a-input
                  v-model:value="formState.purchasingValueKey"
                  :placeholder="pi.ph('purchasingValueKey')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossWeight')"
                name="grossWeight"
              >
                <a-input-number
                  v-model:value="formState.grossWeight"
                  :placeholder="pi.ph('grossWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netWeight')"
                name="netWeight"
              >
                <a-input-number
                  v-model:value="formState.netWeight"
                  :placeholder="pi.ph('netWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('weightUnit')"
                name="weightUnit"
              >
                <a-input
                  v-model:value="formState.weightUnit"
                  :placeholder="pi.ph('weightUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('volume')"
                name="volume"
              >
                <a-input-number
                  v-model:value="formState.volume"
                  :placeholder="pi.ph('volume')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('volumeUnit')"
                name="volumeUnit"
              >
                <a-input
                  v-model:value="formState.volumeUnit"
                  :placeholder="pi.ph('volumeUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('containerRequirements')"
                name="containerRequirements"
              >
                <a-input
                  v-model:value="formState.containerRequirements"
                  :placeholder="pi.ph('containerRequirements')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('storageConditions')"
                name="storageConditions"
              >
                <a-input
                  v-model:value="formState.storageConditions"
                  :placeholder="pi.ph('storageConditions')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('temperatureConditions')"
                name="temperatureConditions"
              >
                <a-input
                  v-model:value="formState.temperatureConditions"
                  :placeholder="pi.ph('temperatureConditions')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lowLevelCode')"
                name="lowLevelCode"
              >
                <a-input
                  v-model:value="formState.lowLevelCode"
                  :placeholder="pi.ph('lowLevelCode')"
                  show-count
                  :maxlength="3"
                  allow-clear
                  :disabled="!!formData?.materialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('transportationGroup')"
                name="transportationGroup"
              >
                <a-input
                  v-model:value="formState.transportationGroup"
                  :placeholder="pi.ph('transportationGroup')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('hazardousMaterialNumber')"
                name="hazardousMaterialNumber"
              >
                <a-input
                  v-model:value="formState.hazardousMaterialNumber"
                  :placeholder="pi.ph('hazardousMaterialNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('division')"
                name="division"
              >
                <a-input
                  v-model:value="formState.division"
                  :placeholder="pi.ph('division')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('competitor')"
                name="competitor"
              >
                <a-input
                  v-model:value="formState.competitor"
                  :placeholder="pi.ph('competitor')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('europeanArticleNumberObsolete')"
                name="europeanArticleNumberObsolete"
              >
                <a-input
                  v-model:value="formState.europeanArticleNumberObsolete"
                  :placeholder="pi.ph('europeanArticleNumberObsolete')"
                  show-count
                  :maxlength="13"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grGiSlipQuantity')"
                name="grGiSlipQuantity"
              >
                <a-input-number
                  v-model:value="formState.grGiSlipQuantity"
                  :placeholder="pi.ph('grGiSlipQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('procurementRule')"
                name="procurementRule"
              >
                <a-input
                  v-model:value="formState.procurementRule"
                  :placeholder="pi.ph('procurementRule')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceOfSupply')"
                name="sourceOfSupply"
              >
                <a-input
                  v-model:value="formState.sourceOfSupply"
                  :placeholder="pi.ph('sourceOfSupply')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonCategory')"
                name="seasonCategory"
              >
                <a-input
                  v-model:value="formState.seasonCategory"
                  :placeholder="pi.ph('seasonCategory')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('labelType')"
                name="labelType"
              >
                <a-input
                  v-model:value="formState.labelType"
                  :placeholder="pi.ph('labelType')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('labelForm')"
                name="labelForm"
              >
                <a-input
                  v-model:value="formState.labelForm"
                  :placeholder="pi.ph('labelForm')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deactivatedField')"
                name="deactivatedField"
              >
                <a-input
                  v-model:value="formState.deactivatedField"
                  :placeholder="pi.ph('deactivatedField')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('internationalArticleNumber')"
                name="internationalArticleNumber"
              >
                <a-input
                  v-model:value="formState.internationalArticleNumber"
                  :placeholder="pi.ph('internationalArticleNumber')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('eanCategory')"
                name="eanCategory"
              >
                <a-input
                  v-model:value="formState.eanCategory"
                  :placeholder="pi.ph('eanCategory')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('length')"
                name="length"
              >
                <a-input-number
                  v-model:value="formState.length"
                  :placeholder="pi.ph('length')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('width')"
                name="width"
              >
                <a-input-number
                  v-model:value="formState.width"
                  :placeholder="pi.ph('width')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('height')"
                name="height"
              >
                <a-input-number
                  v-model:value="formState.height"
                  :placeholder="pi.ph('height')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dimensionUnit')"
                name="dimensionUnit"
              >
                <a-input
                  v-model:value="formState.dimensionUnit"
                  :placeholder="pi.ph('dimensionUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productHierarchy')"
                name="productHierarchy"
              >
                <a-input
                  v-model:value="formState.productHierarchy"
                  :placeholder="pi.ph('productHierarchy')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stockTransferNetChangeCosting')"
                name="stockTransferNetChangeCosting"
              >
                <a-input
                  v-model:value="formState.stockTransferNetChangeCosting"
                  :placeholder="pi.ph('stockTransferNetChangeCosting')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cadIndicator')"
                name="cadIndicator"
              >
                <a-input
                  v-model:value="formState.cadIndicator"
                  :placeholder="pi.ph('cadIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('qmInProcurement')"
                name="qmInProcurement"
              >
                <a-input
                  v-model:value="formState.qmInProcurement"
                  :placeholder="pi.ph('qmInProcurement')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowedPackagingWeight')"
                name="allowedPackagingWeight"
              >
                <a-input-number
                  v-model:value="formState.allowedPackagingWeight"
                  :placeholder="pi.ph('allowedPackagingWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowedPackagingWeightUnit')"
                name="allowedPackagingWeightUnit"
              >
                <a-input
                  v-model:value="formState.allowedPackagingWeightUnit"
                  :placeholder="pi.ph('allowedPackagingWeightUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowedPackagingVolume')"
                name="allowedPackagingVolume"
              >
                <a-input-number
                  v-model:value="formState.allowedPackagingVolume"
                  :placeholder="pi.ph('allowedPackagingVolume')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowedPackagingVolumeUnit')"
                name="allowedPackagingVolumeUnit"
              >
                <a-input
                  v-model:value="formState.allowedPackagingVolumeUnit"
                  :placeholder="pi.ph('allowedPackagingVolumeUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('excessWeightTolerance')"
                name="excessWeightTolerance"
              >
                <a-input-number
                  v-model:value="formState.excessWeightTolerance"
                  :placeholder="pi.ph('excessWeightTolerance')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-6"
        :tab="t('common.page.form.tabs.basicinfo') + ' (7/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('excessVolumeTolerance')"
                name="excessVolumeTolerance"
              >
                <a-input-number
                  v-model:value="formState.excessVolumeTolerance"
                  :placeholder="pi.ph('excessVolumeTolerance')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('variablePurchaseOrderUnit')"
                name="variablePurchaseOrderUnit"
              >
                <a-input
                  v-model:value="formState.variablePurchaseOrderUnit"
                  :placeholder="pi.ph('variablePurchaseOrderUnit')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('revisionLevelAssigned')"
                name="revisionLevelAssigned"
              >
                <a-input
                  v-model:value="formState.revisionLevelAssigned"
                  :placeholder="pi.ph('revisionLevelAssigned')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('configurableMaterial')"
                name="configurableMaterial"
              >
                <a-input
                  v-model:value="formState.configurableMaterial"
                  :placeholder="pi.ph('configurableMaterial')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchManagementRequired')"
                name="batchManagementRequired"
              >
                <a-input
                  v-model:value="formState.batchManagementRequired"
                  :placeholder="pi.ph('batchManagementRequired')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('packagingMaterialType')"
                name="packagingMaterialType"
              >
                <a-input
                  v-model:value="formState.packagingMaterialType"
                  :placeholder="pi.ph('packagingMaterialType')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumLevelByVolume')"
                name="maximumLevelByVolume"
              >
                <a-input-number
                  v-model:value="formState.maximumLevelByVolume"
                  :placeholder="pi.ph('maximumLevelByVolume')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stackingFactor')"
                name="stackingFactor"
              >
                <a-input-number
                  v-model:value="formState.stackingFactor"
                  :placeholder="pi.ph('stackingFactor')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('packagingMaterialGroup')"
                name="packagingMaterialGroup"
              >
                <a-input
                  v-model:value="formState.packagingMaterialGroup"
                  :placeholder="pi.ph('packagingMaterialGroup')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('authorizationGroup')"
                name="authorizationGroup"
              >
                <a-input
                  v-model:value="formState.authorizationGroup"
                  :placeholder="pi.ph('authorizationGroup')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-7"
        :tab="t('common.page.form.tabs.basicinfo') + ' (8/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('validFromDate')"
                name="validFromDate"
              >
                <a-date-picker
                  v-model:value="formState.validFromDate"
                  :placeholder="pi.ph('validFromDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonYear')"
                name="seasonYear"
              >
                <a-input
                  v-model:value="formState.seasonYear"
                  :placeholder="pi.ph('seasonYear')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priceBandCategory')"
                name="priceBandCategory"
              >
                <a-input
                  v-model:value="formState.priceBandCategory"
                  :placeholder="pi.ph('priceBandCategory')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('emptiesBillOfMaterial')"
                name="emptiesBillOfMaterial"
              >
                <a-input
                  v-model:value="formState.emptiesBillOfMaterial"
                  :placeholder="pi.ph('emptiesBillOfMaterial')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('externalMaterialGroup')"
                name="externalMaterialGroup"
              >
                <a-input
                  v-model:value="formState.externalMaterialGroup"
                  :placeholder="pi.ph('externalMaterialGroup')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossPlantConfigurableMaterial')"
                name="crossPlantConfigurableMaterial"
              >
                <a-input
                  v-model:value="formState.crossPlantConfigurableMaterial"
                  :placeholder="pi.ph('crossPlantConfigurableMaterial')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCategory')"
                name="materialCategory"
              >
                <a-input
                  v-model:value="formState.materialCategory"
                  :placeholder="pi.ph('materialCategory')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('coProductIndicator')"
                name="coProductIndicator"
              >
                <a-input
                  v-model:value="formState.coProductIndicator"
                  :placeholder="pi.ph('coProductIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('followUpMaterialIndicator')"
                name="followUpMaterialIndicator"
              >
                <a-input
                  v-model:value="formState.followUpMaterialIndicator"
                  :placeholder="pi.ph('followUpMaterialIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingReferenceMaterial')"
                name="pricingReferenceMaterial"
              >
                <a-input
                  v-model:value="formState.pricingReferenceMaterial"
                  :placeholder="pi.ph('pricingReferenceMaterial')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-8"
        :tab="t('common.page.form.tabs.basicinfo') + ' (9/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossPlantMaterialStatus')"
                name="crossPlantMaterialStatus"
              >
                <a-input
                  v-model:value="formState.crossPlantMaterialStatus"
                  :placeholder="pi.ph('crossPlantMaterialStatus')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossDistributionChainStatus')"
                name="crossDistributionChainStatus"
              >
                <a-input
                  v-model:value="formState.crossDistributionChainStatus"
                  :placeholder="pi.ph('crossDistributionChainStatus')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossPlantStatusValidFrom')"
                name="crossPlantStatusValidFrom"
              >
                <a-date-picker
                  v-model:value="formState.crossPlantStatusValidFrom"
                  :placeholder="pi.ph('crossPlantStatusValidFrom')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossDistributionStatusValidFrom')"
                name="crossDistributionStatusValidFrom"
              >
                <a-date-picker
                  v-model:value="formState.crossDistributionStatusValidFrom"
                  :placeholder="pi.ph('crossDistributionStatusValidFrom')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxClassification')"
                name="taxClassification"
              >
                <a-input
                  v-model:value="formState.taxClassification"
                  :placeholder="pi.ph('taxClassification')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('catalogProfile')"
                name="catalogProfile"
              >
                <a-input
                  v-model:value="formState.catalogProfile"
                  :placeholder="pi.ph('catalogProfile')"
                  show-count
                  :maxlength="9"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('minimumRemainingShelfLife')"
                name="minimumRemainingShelfLife"
              >
                <a-input-number
                  v-model:value="formState.minimumRemainingShelfLife"
                  :placeholder="pi.ph('minimumRemainingShelfLife')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalShelfLife')"
                name="totalShelfLife"
              >
                <a-input-number
                  v-model:value="formState.totalShelfLife"
                  :placeholder="pi.ph('totalShelfLife')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('storagePercentage')"
                name="storagePercentage"
              >
                <a-input-number
                  v-model:value="formState.storagePercentage"
                  :placeholder="pi.ph('storagePercentage')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('contentUnit')"
                name="contentUnit"
              >
                <a-textarea
                  v-model:value="formState.contentUnit"
                  :placeholder="pi.ph('contentUnit')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-9"
        :tab="t('common.page.form.tabs.basicinfo') + ' (10/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('netContents')"
                name="netContents"
              >
                <a-textarea
                  v-model:value="formState.netContents"
                  :placeholder="pi.ph('netContents')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('comparisonPriceUnit')"
                name="comparisonPriceUnit"
              >
                <a-input-number
                  v-model:value="formState.comparisonPriceUnit"
                  :placeholder="pi.ph('comparisonPriceUnit')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('labelingMaterialGrouping')"
                name="labelingMaterialGrouping"
              >
                <a-input
                  v-model:value="formState.labelingMaterialGrouping"
                  :placeholder="pi.ph('labelingMaterialGrouping')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('grossContents')"
                name="grossContents"
              >
                <a-textarea
                  v-model:value="formState.grossContents"
                  :placeholder="pi.ph('grossContents')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quantityConversionMethod')"
                name="quantityConversionMethod"
              >
                <a-input
                  v-model:value="formState.quantityConversionMethod"
                  :placeholder="pi.ph('quantityConversionMethod')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('internalObjectNumber')"
                name="internalObjectNumber"
              >
                <a-input
                  v-model:value="formState.internalObjectNumber"
                  :placeholder="pi.ph('internalObjectNumber')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('environmentallyRelevant')"
                name="environmentallyRelevant"
              >
                <a-input
                  v-model:value="formState.environmentallyRelevant"
                  :placeholder="pi.ph('environmentallyRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productAllocationProcedure')"
                name="productAllocationProcedure"
              >
                <a-input
                  v-model:value="formState.productAllocationProcedure"
                  :placeholder="pi.ph('productAllocationProcedure')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('variantPricingProfile')"
                name="variantPricingProfile"
              >
                <a-input
                  v-model:value="formState.variantPricingProfile"
                  :placeholder="pi.ph('variantPricingProfile')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discountInKind')"
                name="discountInKind"
              >
                <a-input
                  v-model:value="formState.discountInKind"
                  :placeholder="pi.ph('discountInKind')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-10"
        :tab="t('common.page.form.tabs.basicinfo') + ' (11/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerPartNumber')"
                name="manufacturerPartNumber"
              >
                <a-input
                  v-model:value="formState.manufacturerPartNumber"
                  :placeholder="pi.ph('manufacturerPartNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerNumber')"
                name="manufacturerNumber"
              >
                <a-input
                  v-model:value="formState.manufacturerNumber"
                  :placeholder="pi.ph('manufacturerNumber')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inventoryManagedMaterialNumber')"
                name="inventoryManagedMaterialNumber"
              >
                <a-input
                  v-model:value="formState.inventoryManagedMaterialNumber"
                  :placeholder="pi.ph('inventoryManagedMaterialNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerPartProfile')"
                name="manufacturerPartProfile"
              >
                <a-input
                  v-model:value="formState.manufacturerPartProfile"
                  :placeholder="pi.ph('manufacturerPartProfile')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unitsOfMeasureUsage')"
                name="unitsOfMeasureUsage"
              >
                <a-input
                  v-model:value="formState.unitsOfMeasureUsage"
                  :placeholder="pi.ph('unitsOfMeasureUsage')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonRollout')"
                name="seasonRollout"
              >
                <a-input
                  v-model:value="formState.seasonRollout"
                  :placeholder="pi.ph('seasonRollout')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dangerousGoodsProfile')"
                name="dangerousGoodsProfile"
              >
                <a-input
                  v-model:value="formState.dangerousGoodsProfile"
                  :placeholder="pi.ph('dangerousGoodsProfile')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('highlyViscous')"
                name="highlyViscous"
              >
                <a-input
                  v-model:value="formState.highlyViscous"
                  :placeholder="pi.ph('highlyViscous')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inBulkLiquid')"
                name="inBulkLiquid"
              >
                <a-input
                  v-model:value="formState.inBulkLiquid"
                  :placeholder="pi.ph('inBulkLiquid')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNumberExplicitness')"
                name="serialNumberExplicitness"
              >
                <a-input
                  v-model:value="formState.serialNumberExplicitness"
                  :placeholder="pi.ph('serialNumberExplicitness')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-11"
        :tab="t('common.page.form.tabs.basicinfo') + ' (12/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('closedPackaging')"
                name="closedPackaging"
              >
                <a-input
                  v-model:value="formState.closedPackaging"
                  :placeholder="pi.ph('closedPackaging')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('approvedBatchRecordRequired')"
                name="approvedBatchRecordRequired"
              >
                <a-input
                  v-model:value="formState.approvedBatchRecordRequired"
                  :placeholder="pi.ph('approvedBatchRecordRequired')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('effectivityParameterOverride')"
                name="effectivityParameterOverride"
              >
                <a-input
                  v-model:value="formState.effectivityParameterOverride"
                  :placeholder="pi.ph('effectivityParameterOverride')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCompletionLevel')"
                name="materialCompletionLevel"
              >
                <a-input
                  v-model:value="formState.materialCompletionLevel"
                  :placeholder="pi.ph('materialCompletionLevel')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shelfLifePeriodIndicator')"
                name="shelfLifePeriodIndicator"
              >
                <a-input
                  v-model:value="formState.shelfLifePeriodIndicator"
                  :placeholder="pi.ph('shelfLifePeriodIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shelfLifeRoundingRule')"
                name="shelfLifeRoundingRule"
              >
                <a-input
                  v-model:value="formState.shelfLifeRoundingRule"
                  :placeholder="pi.ph('shelfLifeRoundingRule')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productCompositionOnPackaging')"
                name="productCompositionOnPackaging"
              >
                <a-input
                  v-model:value="formState.productCompositionOnPackaging"
                  :placeholder="pi.ph('productCompositionOnPackaging')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('generalItemCategoryGroup')"
                name="generalItemCategoryGroup"
              >
                <a-input
                  v-model:value="formState.generalItemCategoryGroup"
                  :placeholder="pi.ph('generalItemCategoryGroup')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('logisticalVariants')"
                name="logisticalVariants"
              >
                <a-input
                  v-model:value="formState.logisticalVariants"
                  :placeholder="pi.ph('logisticalVariants')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialLocked')"
                name="materialLocked"
              >
                <a-input
                  v-model:value="formState.materialLocked"
                  :placeholder="pi.ph('materialLocked')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-12"
        :tab="t('common.page.form.tabs.basicinfo') + ' (13/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('configurationManagementRelevant')"
                name="configurationManagementRelevant"
              >
                <a-input
                  v-model:value="formState.configurationManagementRelevant"
                  :placeholder="pi.ph('configurationManagementRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assortmentListType')"
                name="assortmentListType"
              >
                <a-input
                  v-model:value="formState.assortmentListType"
                  :placeholder="pi.ph('assortmentListType')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expirationDateType')"
                name="expirationDateType"
              >
                <a-date-picker
                  v-model:value="formState.expirationDateType"
                  :placeholder="pi.ph('expirationDateType')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('gtinVariant')"
                name="gtinVariant"
              >
                <a-input
                  v-model:value="formState.gtinVariant"
                  :placeholder="pi.ph('gtinVariant')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genericMaterialNumber')"
                name="genericMaterialNumber"
              >
                <a-input
                  v-model:value="formState.genericMaterialNumber"
                  :placeholder="pi.ph('genericMaterialNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('samePackingReferenceMaterial')"
                name="samePackingReferenceMaterial"
              >
                <a-input
                  v-model:value="formState.samePackingReferenceMaterial"
                  :placeholder="pi.ph('samePackingReferenceMaterial')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('globalDataSyncRelevant')"
                name="globalDataSyncRelevant"
              >
                <a-input
                  v-model:value="formState.globalDataSyncRelevant"
                  :placeholder="pi.ph('globalDataSyncRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('acceptanceAtOrigin')"
                name="acceptanceAtOrigin"
              >
                <a-input
                  v-model:value="formState.acceptanceAtOrigin"
                  :placeholder="pi.ph('acceptanceAtOrigin')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('standardHuType')"
                name="standardHuType"
              >
                <a-input
                  v-model:value="formState.standardHuType"
                  :placeholder="pi.ph('standardHuType')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pilferable')"
                name="pilferable"
              >
                <a-input
                  v-model:value="formState.pilferable"
                  :placeholder="pi.ph('pilferable')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-13"
        :tab="t('common.page.form.tabs.basicinfo') + ' (14/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('warehouseStorageCondition')"
                name="warehouseStorageCondition"
              >
                <a-input
                  v-model:value="formState.warehouseStorageCondition"
                  :placeholder="pi.ph('warehouseStorageCondition')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('warehouseMaterialGroup')"
                name="warehouseMaterialGroup"
              >
                <a-input
                  v-model:value="formState.warehouseMaterialGroup"
                  :placeholder="pi.ph('warehouseMaterialGroup')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('handlingIndicator')"
                name="handlingIndicator"
              >
                <a-input
                  v-model:value="formState.handlingIndicator"
                  :placeholder="pi.ph('handlingIndicator')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('hazardousSubstancesRelevant')"
                name="hazardousSubstancesRelevant"
              >
                <a-input
                  v-model:value="formState.hazardousSubstancesRelevant"
                  :placeholder="pi.ph('hazardousSubstancesRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('handlingUnitType')"
                name="handlingUnitType"
              >
                <a-input
                  v-model:value="formState.handlingUnitType"
                  :placeholder="pi.ph('handlingUnitType')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('variableTareWeight')"
                name="variableTareWeight"
              >
                <a-input
                  v-model:value="formState.variableTareWeight"
                  :placeholder="pi.ph('variableTareWeight')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumAllowedCapacity')"
                name="maximumAllowedCapacity"
              >
                <a-input-number
                  v-model:value="formState.maximumAllowedCapacity"
                  :placeholder="pi.ph('maximumAllowedCapacity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('overcapacityTolerance')"
                name="overcapacityTolerance"
              >
                <a-input-number
                  v-model:value="formState.overcapacityTolerance"
                  :placeholder="pi.ph('overcapacityTolerance')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumPackingLength')"
                name="maximumPackingLength"
              >
                <a-input-number
                  v-model:value="formState.maximumPackingLength"
                  :placeholder="pi.ph('maximumPackingLength')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumPackingWidth')"
                name="maximumPackingWidth"
              >
                <a-input-number
                  v-model:value="formState.maximumPackingWidth"
                  :placeholder="pi.ph('maximumPackingWidth')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-14"
        :tab="t('common.page.form.tabs.basicinfo') + ' (15/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumPackingHeight')"
                name="maximumPackingHeight"
              >
                <a-input-number
                  v-model:value="formState.maximumPackingHeight"
                  :placeholder="pi.ph('maximumPackingHeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumPackingDimensionUnit')"
                name="maximumPackingDimensionUnit"
              >
                <a-input
                  v-model:value="formState.maximumPackingDimensionUnit"
                  :placeholder="pi.ph('maximumPackingDimensionUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countryOfOrigin')"
                name="countryOfOrigin"
              >
                <a-input
                  v-model:value="formState.countryOfOrigin"
                  :placeholder="pi.ph('countryOfOrigin')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialFreightGroup')"
                name="materialFreightGroup"
              >
                <a-input
                  v-model:value="formState.materialFreightGroup"
                  :placeholder="pi.ph('materialFreightGroup')"
                  show-count
                  :maxlength="8"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quarantinePeriod')"
                name="quarantinePeriod"
              >
                <a-input-number
                  v-model:value="formState.quarantinePeriod"
                  :placeholder="pi.ph('quarantinePeriod')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quarantinePeriodUnit')"
                name="quarantinePeriodUnit"
              >
                <a-input
                  v-model:value="formState.quarantinePeriodUnit"
                  :placeholder="pi.ph('quarantinePeriodUnit')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('qualityInspectionGroup')"
                name="qualityInspectionGroup"
              >
                <a-input
                  v-model:value="formState.qualityInspectionGroup"
                  :placeholder="pi.ph('qualityInspectionGroup')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNumberProfile')"
                name="serialNumberProfile"
              >
                <a-input
                  v-model:value="formState.serialNumberProfile"
                  :placeholder="pi.ph('serialNumberProfile')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('formName')"
                name="formName"
              >
                <a-input
                  v-model:value="formState.formName"
                  :placeholder="pi.ph('formName')"
                  show-count
                  :maxlength="30"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('logisticsUnitOfMeasure')"
                name="logisticsUnitOfMeasure"
              >
                <a-input
                  v-model:value="formState.logisticsUnitOfMeasure"
                  :placeholder="pi.ph('logisticsUnitOfMeasure')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-15"
        :tab="t('common.page.form.tabs.basicinfo') + ' (16/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('catchWeightMaterial')"
                name="catchWeightMaterial"
              >
                <a-input
                  v-model:value="formState.catchWeightMaterial"
                  :placeholder="pi.ph('catchWeightMaterial')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('catchWeightProfile')"
                name="catchWeightProfile"
              >
                <a-input
                  v-model:value="formState.catchWeightProfile"
                  :placeholder="pi.ph('catchWeightProfile')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('catchWeightToleranceGroup')"
                name="catchWeightToleranceGroup"
              >
                <a-input
                  v-model:value="formState.catchWeightToleranceGroup"
                  :placeholder="pi.ph('catchWeightToleranceGroup')"
                  show-count
                  :maxlength="9"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('adjustmentProfile')"
                name="adjustmentProfile"
              >
                <a-input
                  v-model:value="formState.adjustmentProfile"
                  :placeholder="pi.ph('adjustmentProfile')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('intellectualPropertyId')"
                name="intellectualPropertyId"
              >
                <a-input
                  v-model:value="formState.intellectualPropertyId"
                  :placeholder="pi.ph('intellectualPropertyId')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('variantPriceAllowed')"
                name="variantPriceAllowed"
              >
                <a-input
                  v-model:value="formState.variantPriceAllowed"
                  :placeholder="pi.ph('variantPriceAllowed')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('medium')"
                name="medium"
              >
                <a-input
                  v-model:value="formState.medium"
                  :placeholder="pi.ph('medium')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('physicalCommodity')"
                name="physicalCommodity"
              >
                <a-input
                  v-model:value="formState.physicalCommodity"
                  :placeholder="pi.ph('physicalCommodity')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('animalOrigin')"
                name="animalOrigin"
              >
                <a-input
                  v-model:value="formState.animalOrigin"
                  :placeholder="pi.ph('animalOrigin')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('textileCompositionFunction')"
                name="textileCompositionFunction"
              >
                <a-input
                  v-model:value="formState.textileCompositionFunction"
                  :placeholder="pi.ph('textileCompositionFunction')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-16"
        :tab="t('common.page.form.tabs.basicinfo') + ' (17/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationStructure')"
                name="segmentationStructure"
              >
                <a-input
                  v-model:value="formState.segmentationStructure"
                  :placeholder="pi.ph('segmentationStructure')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationStrategy')"
                name="segmentationStrategy"
              >
                <a-input
                  v-model:value="formState.segmentationStrategy"
                  :placeholder="pi.ph('segmentationStrategy')"
                  show-count
                  :maxlength="8"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationStatus')"
                name="segmentationStatus"
              >
                <a-input
                  v-model:value="formState.segmentationStatus"
                  :placeholder="pi.ph('segmentationStatus')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('segmentationScope')"
                name="segmentationScope"
              >
                <a-textarea
                  v-model:value="formState.segmentationScope"
                  :placeholder="pi.ph('segmentationScope')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationRelevant')"
                name="segmentationRelevant"
              >
                <a-input
                  v-model:value="formState.segmentationRelevant"
                  :placeholder="pi.ph('segmentationRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fashionAttribute1')"
                name="fashionAttribute1"
              >
                <a-input
                  v-model:value="formState.fashionAttribute1"
                  :placeholder="pi.ph('fashionAttribute1')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fashionAttribute2')"
                name="fashionAttribute2"
              >
                <a-input
                  v-model:value="formState.fashionAttribute2"
                  :placeholder="pi.ph('fashionAttribute2')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fashionAttribute3')"
                name="fashionAttribute3"
              >
                <a-input
                  v-model:value="formState.fashionAttribute3"
                  :placeholder="pi.ph('fashionAttribute3')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonUsageIndicator')"
                name="seasonUsageIndicator"
              >
                <a-input
                  v-model:value="formState.seasonUsageIndicator"
                  :placeholder="pi.ph('seasonUsageIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonActiveInInventory')"
                name="seasonActiveInInventory"
              >
                <a-input
                  v-model:value="formState.seasonActiveInInventory"
                  :placeholder="pi.ph('seasonActiveInInventory')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-17"
        :tab="t('common.page.form.tabs.basicinfo') + ' (18/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('characteristicConversionId')"
                name="characteristicConversionId"
              >
                <a-input
                  v-model:value="formState.characteristicConversionId"
                  :placeholder="pi.ph('characteristicConversionId')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('anpCode')"
                name="anpCode"
              >
                <a-input
                  v-model:value="formState.anpCode"
                  :placeholder="pi.ph('anpCode')"
                  show-count
                  :maxlength="9"
                  allow-clear
                  :disabled="!!formData?.materialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dangerousGoodsPackagingStatus')"
                name="dangerousGoodsPackagingStatus"
              >
                <a-input
                  v-model:value="formState.dangerousGoodsPackagingStatus"
                  :placeholder="pi.ph('dangerousGoodsPackagingStatus')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialConditionManagement')"
                name="materialConditionManagement"
              >
                <a-input
                  v-model:value="formState.materialConditionManagement"
                  :placeholder="pi.ph('materialConditionManagement')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('returnCode')"
                name="returnCode"
              >
                <a-input
                  v-model:value="formState.returnCode"
                  :placeholder="pi.ph('returnCode')"
                  show-count
                  :maxlength="1"
                  allow-clear
                  :disabled="!!formData?.materialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('returnToLogisticsLevel')"
                name="returnToLogisticsLevel"
              >
                <a-input
                  v-model:value="formState.returnToLogisticsLevel"
                  :placeholder="pi.ph('returnToLogisticsLevel')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('natoItemIdentificationNumber')"
                name="natoItemIdentificationNumber"
              >
                <a-input
                  v-model:value="formState.natoItemIdentificationNumber"
                  :placeholder="pi.ph('natoItemIdentificationNumber')"
                  show-count
                  :maxlength="9"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fffClass')"
                name="fffClass"
              >
                <a-input
                  v-model:value="formState.fffClass"
                  :placeholder="pi.ph('fffClass')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supersessionChainNumber')"
                name="supersessionChainNumber"
              >
                <a-input
                  v-model:value="formState.supersessionChainNumber"
                  :placeholder="pi.ph('supersessionChainNumber')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonalProcurementCreationStatus')"
                name="seasonalProcurementCreationStatus"
              >
                <a-input
                  v-model:value="formState.seasonalProcurementCreationStatus"
                  :placeholder="pi.ph('seasonalProcurementCreationStatus')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-18"
        :tab="t('common.page.form.tabs.basicinfo') + ' (19/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('colorCharacteristicInternalNumber')"
                name="colorCharacteristicInternalNumber"
              >
                <a-input
                  v-model:value="formState.colorCharacteristicInternalNumber"
                  :placeholder="pi.ph('colorCharacteristicInternalNumber')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mainSizeCharacteristicInternalNumber')"
                name="mainSizeCharacteristicInternalNumber"
              >
                <a-input
                  v-model:value="formState.mainSizeCharacteristicInternalNumber"
                  :placeholder="pi.ph('mainSizeCharacteristicInternalNumber')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('secondSizeCharacteristicInternalNumber')"
                name="secondSizeCharacteristicInternalNumber"
              >
                <a-input
                  v-model:value="formState.secondSizeCharacteristicInternalNumber"
                  :placeholder="pi.ph('secondSizeCharacteristicInternalNumber')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('color')"
                name="color"
              >
                <a-input
                  v-model:value="formState.color"
                  :placeholder="pi.ph('color')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mainSize')"
                name="mainSize"
              >
                <a-input
                  v-model:value="formState.mainSize"
                  :placeholder="pi.ph('mainSize')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('secondSize')"
                name="secondSize"
              >
                <a-input
                  v-model:value="formState.secondSize"
                  :placeholder="pi.ph('secondSize')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluationCharacteristicValue')"
                name="evaluationCharacteristicValue"
              >
                <a-input
                  v-model:value="formState.evaluationCharacteristicValue"
                  :placeholder="pi.ph('evaluationCharacteristicValue')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('careCode')"
                name="careCode"
              >
                <a-input
                  v-model:value="formState.careCode"
                  :placeholder="pi.ph('careCode')"
                  show-count
                  :maxlength="16"
                  allow-clear
                  :disabled="!!formData?.materialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('brandId')"
                name="brandId"
              >
                <a-input
                  v-model:value="formState.brandId"
                  :placeholder="pi.ph('brandId')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode1')"
                name="fiberCode1"
              >
                <a-input
                  v-model:value="formState.fiberCode1"
                  :placeholder="pi.ph('fiberCode1')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-19"
        :tab="t('common.page.form.tabs.basicinfo') + ' (20/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart1')"
                name="fiberPart1"
              >
                <a-input
                  v-model:value="formState.fiberPart1"
                  :placeholder="pi.ph('fiberPart1')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode2')"
                name="fiberCode2"
              >
                <a-input
                  v-model:value="formState.fiberCode2"
                  :placeholder="pi.ph('fiberCode2')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart2')"
                name="fiberPart2"
              >
                <a-input
                  v-model:value="formState.fiberPart2"
                  :placeholder="pi.ph('fiberPart2')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode3')"
                name="fiberCode3"
              >
                <a-input
                  v-model:value="formState.fiberCode3"
                  :placeholder="pi.ph('fiberCode3')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart3')"
                name="fiberPart3"
              >
                <a-input
                  v-model:value="formState.fiberPart3"
                  :placeholder="pi.ph('fiberPart3')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode4')"
                name="fiberCode4"
              >
                <a-input
                  v-model:value="formState.fiberCode4"
                  :placeholder="pi.ph('fiberCode4')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart4')"
                name="fiberPart4"
              >
                <a-input
                  v-model:value="formState.fiberPart4"
                  :placeholder="pi.ph('fiberPart4')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode5')"
                name="fiberCode5"
              >
                <a-input
                  v-model:value="formState.fiberCode5"
                  :placeholder="pi.ph('fiberCode5')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart5')"
                name="fiberPart5"
              >
                <a-input
                  v-model:value="formState.fiberPart5"
                  :placeholder="pi.ph('fiberPart5')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fashionGrade')"
                name="fashionGrade"
              >
                <a-input
                  v-model:value="formState.fashionGrade"
                  :placeholder="pi.ph('fashionGrade')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-20"
        :tab="t('common.page.form.tabs.basicinfo') + ' (21/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialStatus')"
                name="materialStatus"
              >
                <TaktSelect
                  v-model:value="formState.materialStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="pi.ph('materialStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-21"
        :tab="t('common.page.form.tabs.basicinfo') + ' (22/22)'"
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
    <!-- 下：子表 descriptions -->
    <TaktEditableTable
      ref="materialDescriptionTableRef"
      v-model="childMaterialDescriptionRows"
      :columns="materialDescriptionFormColumns"
      :title="materialDescriptionPi.self()"
      :add-button-entity="materialDescriptionPi.self()"
      id-field="materialDescriptionId"
      :default-row="createDefaultMaterialDescriptionRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-cultureCode="{ record }">
        <TaktSelect
          v-model:value="record.cultureCode"
          api-url="TaktCultures/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDescriptionPi.queryPh('cultureCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt全局物料实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaterialI18n } from '../composables/use-material-i18n'

/** 实体字段 i18n */
const pi = useMaterialI18n()

import type { MaterialCreate } from '@/types/logistics/materials/material'
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
const formFields = ["tenantCode","materialCode","completeMaintenanceStatus","maintenanceStatus","clientDeletionFlag","materialType","industrySector","materialGroup","oldMaterialNumber","baseUnit","orderUnit","documentNumber","documentType","documentVersion","documentPageFormat","documentChangeNumber","documentPageNumber","documentSheetCount","productionInspectionMemo","productionMemoPageFormat","sizeDimensions","basicMaterial","industryStandardDescription","laboratoryDesignOffice","purchasingValueKey","grossWeight","netWeight","weightUnit","volume","volumeUnit","containerRequirements","storageConditions","temperatureConditions","lowLevelCode","transportationGroup","hazardousMaterialNumber","division","competitor","europeanArticleNumberObsolete","grGiSlipQuantity","procurementRule","sourceOfSupply","seasonCategory","labelType","labelForm","deactivatedField","internationalArticleNumber","eanCategory","length","width","height","dimensionUnit","productHierarchy","stockTransferNetChangeCosting","cadIndicator","qmInProcurement","allowedPackagingWeight","allowedPackagingWeightUnit","allowedPackagingVolume","allowedPackagingVolumeUnit","excessWeightTolerance","excessVolumeTolerance","variablePurchaseOrderUnit","revisionLevelAssigned","configurableMaterial","batchManagementRequired","packagingMaterialType","maximumLevelByVolume","stackingFactor","packagingMaterialGroup","authorizationGroup","validFromDate","seasonYear","priceBandCategory","emptiesBillOfMaterial","externalMaterialGroup","crossPlantConfigurableMaterial","materialCategory","coProductIndicator","followUpMaterialIndicator","pricingReferenceMaterial","crossPlantMaterialStatus","crossDistributionChainStatus","crossPlantStatusValidFrom","crossDistributionStatusValidFrom","taxClassification","catalogProfile","minimumRemainingShelfLife","totalShelfLife","storagePercentage","contentUnit","netContents","comparisonPriceUnit","labelingMaterialGrouping","grossContents","quantityConversionMethod","internalObjectNumber","environmentallyRelevant","productAllocationProcedure","variantPricingProfile","discountInKind","manufacturerPartNumber","manufacturerNumber","inventoryManagedMaterialNumber","manufacturerPartProfile","unitsOfMeasureUsage","seasonRollout","dangerousGoodsProfile","highlyViscous","inBulkLiquid","serialNumberExplicitness","closedPackaging","approvedBatchRecordRequired","effectivityParameterOverride","materialCompletionLevel","shelfLifePeriodIndicator","shelfLifeRoundingRule","productCompositionOnPackaging","generalItemCategoryGroup","logisticalVariants","materialLocked","configurationManagementRelevant","assortmentListType","expirationDateType","gtinVariant","genericMaterialNumber","samePackingReferenceMaterial","globalDataSyncRelevant","acceptanceAtOrigin","standardHuType","pilferable","warehouseStorageCondition","warehouseMaterialGroup","handlingIndicator","hazardousSubstancesRelevant","handlingUnitType","variableTareWeight","maximumAllowedCapacity","overcapacityTolerance","maximumPackingLength","maximumPackingWidth","maximumPackingHeight","maximumPackingDimensionUnit","countryOfOrigin","materialFreightGroup","quarantinePeriod","quarantinePeriodUnit","qualityInspectionGroup","serialNumberProfile","formName","logisticsUnitOfMeasure","catchWeightMaterial","catchWeightProfile","catchWeightToleranceGroup","adjustmentProfile","intellectualPropertyId","variantPriceAllowed","medium","physicalCommodity","animalOrigin","textileCompositionFunction","segmentationStructure","segmentationStrategy","segmentationStatus","segmentationScope","segmentationRelevant","fashionAttribute1","fashionAttribute2","fashionAttribute3","seasonUsageIndicator","seasonActiveInInventory","characteristicConversionId","anpCode","dangerousGoodsPackagingStatus","materialConditionManagement","returnCode","returnToLogisticsLevel","natoItemIdentificationNumber","fffClass","supersessionChainNumber","seasonalProcurementCreationStatus","colorCharacteristicInternalNumber","mainSizeCharacteristicInternalNumber","secondSizeCharacteristicInternalNumber","color","mainSize","secondSize","evaluationCharacteristicValue","careCode","brandId","fiberCode1","fiberPart1","fiberCode2","fiberPart2","fiberCode3","fiberPart3","fiberCode4","fiberPart4","fiberCode5","fiberPart5","fashionGrade","materialStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useMaterialDescriptionI18n } from '../composables/use-material-description-i18n'

const materialDescriptionPi = useMaterialDescriptionI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMaterialDescriptionRows = ref<Record<string, unknown>[]>([])
const materialDescriptionTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 materialDescription 可编辑列 */
const materialDescriptionFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'description',
    title: materialDescriptionPi.label('description'),
    editor: 'textarea',
    rows: 1,
    placeholder: materialDescriptionPi.ph('description'),
    width: 180,
  },
  {
    key: 'cultureCode',
    title: materialDescriptionPi.label('cultureCode'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaterialCreate & { materialId?: string }> | null | undefined) {
  const rows_materialDescription = ((val as any)?.descriptions ?? []) as Record<string, unknown>[]
  childMaterialDescriptionRows.value = rows_materialDescription
}

function createDefaultMaterialDescriptionRow(): Record<string, unknown> {
  return {
    description: '',
    cultureCode: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.materialId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    descriptions: materialDescriptionTableRef.value?.getRows?.() ?? childMaterialDescriptionRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      materialId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialCreate & { materialId?: string }> | null
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
  materialStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 materialId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).descriptions
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
    const isCreate = !props.formData?.materialId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'blur'
    }
  ],
  materialType: [
    {
      required: true,
      message: pi.ph('materialType'),
      trigger: 'blur'
    }
  ],
  industrySector: [
    {
      required: true,
      message: pi.ph('industrySector'),
      trigger: 'blur'
    }
  ],
  materialGroup: [
    {
      required: true,
      message: pi.ph('materialGroup'),
      trigger: 'blur'
    }
  ],
  baseUnit: [
    {
      required: true,
      message: pi.ph('baseUnit'),
      trigger: 'blur'
    }
  ],
  materialStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('materialStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('materialStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await materialDescriptionTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('grossWeight' in payload) {
    const rawgrossWeight = payload.grossWeight
    payload.grossWeight = typeof rawgrossWeight === 'number' ? rawgrossWeight : Number(rawgrossWeight)
  }
  if ('netWeight' in payload) {
    const rawnetWeight = payload.netWeight
    payload.netWeight = typeof rawnetWeight === 'number' ? rawnetWeight : Number(rawnetWeight)
  }
  if ('volume' in payload) {
    const rawvolume = payload.volume
    payload.volume = typeof rawvolume === 'number' ? rawvolume : Number(rawvolume)
  }
  if ('grGiSlipQuantity' in payload) {
    const rawgrGiSlipQuantity = payload.grGiSlipQuantity
    payload.grGiSlipQuantity = typeof rawgrGiSlipQuantity === 'number' ? rawgrGiSlipQuantity : Number(rawgrGiSlipQuantity)
  }
  if ('length' in payload) {
    const rawlength = payload.length
    payload.length = typeof rawlength === 'number' ? rawlength : Number(rawlength)
  }
  if ('width' in payload) {
    const rawwidth = payload.width
    payload.width = typeof rawwidth === 'number' ? rawwidth : Number(rawwidth)
  }
  if ('height' in payload) {
    const rawheight = payload.height
    payload.height = typeof rawheight === 'number' ? rawheight : Number(rawheight)
  }
  if ('allowedPackagingWeight' in payload) {
    const rawallowedPackagingWeight = payload.allowedPackagingWeight
    payload.allowedPackagingWeight = typeof rawallowedPackagingWeight === 'number' ? rawallowedPackagingWeight : Number(rawallowedPackagingWeight)
  }
  if ('allowedPackagingVolume' in payload) {
    const rawallowedPackagingVolume = payload.allowedPackagingVolume
    payload.allowedPackagingVolume = typeof rawallowedPackagingVolume === 'number' ? rawallowedPackagingVolume : Number(rawallowedPackagingVolume)
  }
  if ('excessWeightTolerance' in payload) {
    const rawexcessWeightTolerance = payload.excessWeightTolerance
    payload.excessWeightTolerance = typeof rawexcessWeightTolerance === 'number' ? rawexcessWeightTolerance : Number(rawexcessWeightTolerance)
  }
  if ('excessVolumeTolerance' in payload) {
    const rawexcessVolumeTolerance = payload.excessVolumeTolerance
    payload.excessVolumeTolerance = typeof rawexcessVolumeTolerance === 'number' ? rawexcessVolumeTolerance : Number(rawexcessVolumeTolerance)
  }
  if ('maximumLevelByVolume' in payload) {
    const rawmaximumLevelByVolume = payload.maximumLevelByVolume
    payload.maximumLevelByVolume = typeof rawmaximumLevelByVolume === 'number' ? rawmaximumLevelByVolume : Number(rawmaximumLevelByVolume)
  }
  if ('stackingFactor' in payload) {
    const rawstackingFactor = payload.stackingFactor
    payload.stackingFactor = typeof rawstackingFactor === 'number' ? rawstackingFactor : Number(rawstackingFactor)
  }
  if ('minimumRemainingShelfLife' in payload) {
    const rawminimumRemainingShelfLife = payload.minimumRemainingShelfLife
    payload.minimumRemainingShelfLife = typeof rawminimumRemainingShelfLife === 'number' ? rawminimumRemainingShelfLife : Number(rawminimumRemainingShelfLife)
  }
  if ('totalShelfLife' in payload) {
    const rawtotalShelfLife = payload.totalShelfLife
    payload.totalShelfLife = typeof rawtotalShelfLife === 'number' ? rawtotalShelfLife : Number(rawtotalShelfLife)
  }
  if ('storagePercentage' in payload) {
    const rawstoragePercentage = payload.storagePercentage
    payload.storagePercentage = typeof rawstoragePercentage === 'number' ? rawstoragePercentage : Number(rawstoragePercentage)
  }
  if ('netContents' in payload) {
    const rawnetContents = payload.netContents
    payload.netContents = typeof rawnetContents === 'number' ? rawnetContents : Number(rawnetContents)
  }
  if ('comparisonPriceUnit' in payload) {
    const rawcomparisonPriceUnit = payload.comparisonPriceUnit
    payload.comparisonPriceUnit = typeof rawcomparisonPriceUnit === 'number' ? rawcomparisonPriceUnit : Number(rawcomparisonPriceUnit)
  }
  if ('grossContents' in payload) {
    const rawgrossContents = payload.grossContents
    payload.grossContents = typeof rawgrossContents === 'number' ? rawgrossContents : Number(rawgrossContents)
  }
  if ('maximumAllowedCapacity' in payload) {
    const rawmaximumAllowedCapacity = payload.maximumAllowedCapacity
    payload.maximumAllowedCapacity = typeof rawmaximumAllowedCapacity === 'number' ? rawmaximumAllowedCapacity : Number(rawmaximumAllowedCapacity)
  }
  if ('overcapacityTolerance' in payload) {
    const rawovercapacityTolerance = payload.overcapacityTolerance
    payload.overcapacityTolerance = typeof rawovercapacityTolerance === 'number' ? rawovercapacityTolerance : Number(rawovercapacityTolerance)
  }
  if ('maximumPackingLength' in payload) {
    const rawmaximumPackingLength = payload.maximumPackingLength
    payload.maximumPackingLength = typeof rawmaximumPackingLength === 'number' ? rawmaximumPackingLength : Number(rawmaximumPackingLength)
  }
  if ('maximumPackingWidth' in payload) {
    const rawmaximumPackingWidth = payload.maximumPackingWidth
    payload.maximumPackingWidth = typeof rawmaximumPackingWidth === 'number' ? rawmaximumPackingWidth : Number(rawmaximumPackingWidth)
  }
  if ('maximumPackingHeight' in payload) {
    const rawmaximumPackingHeight = payload.maximumPackingHeight
    payload.maximumPackingHeight = typeof rawmaximumPackingHeight === 'number' ? rawmaximumPackingHeight : Number(rawmaximumPackingHeight)
  }
  if ('quarantinePeriod' in payload) {
    const rawquarantinePeriod = payload.quarantinePeriod
    payload.quarantinePeriod = typeof rawquarantinePeriod === 'number' ? rawquarantinePeriod : Number(rawquarantinePeriod)
  }
  if ('materialStatus' in payload) {
    const rawmaterialStatus = payload.materialStatus
    payload.materialStatus = typeof rawmaterialStatus === 'number' ? rawmaterialStatus : Number(rawmaterialStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.materialId)
  childMaterialDescriptionRows.value = []
  materialDescriptionTableRef.value?.resetRows?.()
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
