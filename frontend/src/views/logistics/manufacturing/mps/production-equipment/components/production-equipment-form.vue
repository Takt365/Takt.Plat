<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mps/production-equipment/components -->
<!-- 文件名称：production-equipment-form.vue -->
<!-- 功能描述：生产设备主数据维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="production-equipment-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.productionEquipmentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentCategory')"
                name="equipmentCategory"
              >
                <TaktSelect
                  v-model:value="formState.equipmentCategory"
                  dict-type="logistics_equipment_category"
                  :placeholder="pi.ph('equipmentCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionEquipmentCode')"
                name="productionEquipmentCode"
              >
                <a-input
                  v-model:value="formState.productionEquipmentCode"
                  :placeholder="pi.ph('productionEquipmentCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.productionEquipmentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionEquipmentName')"
                name="productionEquipmentName"
              >
                <a-input
                  v-model:value="formState.productionEquipmentName"
                  :placeholder="pi.ph('productionEquipmentName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturer')"
                name="manufacturer"
              >
                <a-input
                  v-model:value="formState.manufacturer"
                  :placeholder="pi.ph('manufacturer')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentBrand')"
                name="equipmentBrand"
              >
                <a-input
                  v-model:value="formState.equipmentBrand"
                  :placeholder="pi.ph('equipmentBrand')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('machineType')"
                name="machineType"
              >
                <a-input
                  v-model:value="formState.machineType"
                  :placeholder="pi.ph('machineType')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('modelNo')"
                name="modelNo"
              >
                <a-input
                  v-model:value="formState.modelNo"
                  :placeholder="pi.ph('modelNo')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNo')"
                name="serialNo"
              >
                <a-input
                  v-model:value="formState.serialNo"
                  :placeholder="pi.ph('serialNo')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturingDate')"
                name="manufacturingDate"
              >
                <a-date-picker
                  v-model:value="formState.manufacturingDate"
                  :placeholder="pi.ph('manufacturingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentSpecification')"
                name="equipmentSpecification"
              >
                <a-input
                  v-model:value="formState.equipmentSpecification"
                  :placeholder="pi.ph('equipmentSpecification')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stdCycleTimeSeconds')"
                name="stdCycleTimeSeconds"
              >
                <a-input-number
                  v-model:value="formState.stdCycleTimeSeconds"
                  :placeholder="pi.ph('stdCycleTimeSeconds')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stdMinutesPerUnit')"
                name="stdMinutesPerUnit"
              >
                <a-input-number
                  v-model:value="formState.stdMinutesPerUnit"
                  :placeholder="pi.ph('stdMinutesPerUnit')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stdMinutesPerCycle')"
                name="stdMinutesPerCycle"
              >
                <a-input-number
                  v-model:value="formState.stdMinutesPerCycle"
                  :placeholder="pi.ph('stdMinutesPerCycle')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('theoreticalSpm')"
                name="theoreticalSpm"
              >
                <a-input-number
                  v-model:value="formState.theoreticalSpm"
                  :placeholder="pi.ph('theoreticalSpm')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('theoreticalCycleTimeSeconds')"
                name="theoreticalCycleTimeSeconds"
              >
                <a-input-number
                  v-model:value="formState.theoreticalCycleTimeSeconds"
                  :placeholder="pi.ph('theoreticalCycleTimeSeconds')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stdEquipmentHourlyCapacity')"
                name="stdEquipmentHourlyCapacity"
              >
                <a-input-number
                  v-model:value="formState.stdEquipmentHourlyCapacity"
                  :placeholder="pi.ph('stdEquipmentHourlyCapacity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('availabilityRate')"
                name="availabilityRate"
              >
                <a-input-number
                  v-model:value="formState.availabilityRate"
                  :placeholder="pi.ph('availabilityRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('performanceRate')"
                name="performanceRate"
              >
                <a-input-number
                  v-model:value="formState.performanceRate"
                  :placeholder="pi.ph('performanceRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('setupMinutes')"
                name="setupMinutes"
              >
                <a-input-number
                  v-model:value="formState.setupMinutes"
                  :placeholder="pi.ph('setupMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('moldChangeMinutes')"
                name="moldChangeMinutes"
              >
                <a-input-number
                  v-model:value="formState.moldChangeMinutes"
                  :placeholder="pi.ph('moldChangeMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialChangeMinutes')"
                name="materialChangeMinutes"
              >
                <a-input-number
                  v-model:value="formState.materialChangeMinutes"
                  :placeholder="pi.ph('materialChangeMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mtbfHours')"
                name="mtbfHours"
              >
                <a-input-number
                  v-model:value="formState.mtbfHours"
                  :placeholder="pi.ph('mtbfHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mttrHours')"
                name="mttrHours"
              >
                <a-input-number
                  v-model:value="formState.mttrHours"
                  :placeholder="pi.ph('mttrHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('repeatabilityAccuracy')"
                name="repeatabilityAccuracy"
              >
                <a-input-number
                  v-model:value="formState.repeatabilityAccuracy"
                  :placeholder="pi.ph('repeatabilityAccuracy')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shutHeightAccuracy')"
                name="shutHeightAccuracy"
              >
                <a-input-number
                  v-model:value="formState.shutHeightAccuracy"
                  :placeholder="pi.ph('shutHeightAccuracy')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('injectionAccuracy')"
                name="injectionAccuracy"
              >
                <a-input-number
                  v-model:value="formState.injectionAccuracy"
                  :placeholder="pi.ph('injectionAccuracy')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('temperatureControlAccuracy')"
                name="temperatureControlAccuracy"
              >
                <a-input-number
                  v-model:value="formState.temperatureControlAccuracy"
                  :placeholder="pi.ph('temperatureControlAccuracy')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pressureControlAccuracy')"
                name="pressureControlAccuracy"
              >
                <a-input-number
                  v-model:value="formState.pressureControlAccuracy"
                  :placeholder="pi.ph('pressureControlAccuracy')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('processCapabilityCpk')"
                name="processCapabilityCpk"
              >
                <a-input-number
                  v-model:value="formState.processCapabilityCpk"
                  :placeholder="pi.ph('processCapabilityCpk')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maxDimensionalTolerance')"
                name="maxDimensionalTolerance"
              >
                <a-input-number
                  v-model:value="formState.maxDimensionalTolerance"
                  :placeholder="pi.ph('maxDimensionalTolerance')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maxMoldDimension')"
                name="maxMoldDimension"
              >
                <a-input
                  v-model:value="formState.maxMoldDimension"
                  :placeholder="pi.ph('maxMoldDimension')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('minMoldDimension')"
                name="minMoldDimension"
              >
                <a-input
                  v-model:value="formState.minMoldDimension"
                  :placeholder="pi.ph('minMoldDimension')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maxMoldWeightTon')"
                name="maxMoldWeightTon"
              >
                <a-input-number
                  v-model:value="formState.maxMoldWeightTon"
                  :placeholder="pi.ph('maxMoldWeightTon')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('moldHeightRange')"
                name="moldHeightRange"
              >
                <a-input
                  v-model:value="formState.moldHeightRange"
                  :placeholder="pi.ph('moldHeightRange')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ejectionType')"
                name="ejectionType"
              >
                <TaktSelect
                  v-model:value="formState.ejectionType"
                  dict-type="logistics_ejection_type"
                  :placeholder="pi.ph('ejectionType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ejectionStrokeMm')"
                name="ejectionStrokeMm"
              >
                <a-input-number
                  v-model:value="formState.ejectionStrokeMm"
                  :placeholder="pi.ph('ejectionStrokeMm')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cavityCount')"
                name="cavityCount"
              >
                <a-input-number
                  v-model:value="formState.cavityCount"
                  :placeholder="pi.ph('cavityCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quickMoldChange')"
                name="quickMoldChange"
              >
                <TaktSelect
                  v-model:value="formState.quickMoldChange"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('quickMoldChange')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('moldCode')"
                name="moldCode"
              >
                <a-input
                  v-model:value="formState.moldCode"
                  :placeholder="pi.ph('moldCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.productionEquipmentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ratedTonnage')"
                name="ratedTonnage"
              >
                <a-input-number
                  v-model:value="formState.ratedTonnage"
                  :placeholder="pi.ph('ratedTonnage')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clampingForceKn')"
                name="clampingForceKn"
              >
                <a-input-number
                  v-model:value="formState.clampingForceKn"
                  :placeholder="pi.ph('clampingForceKn')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maxStrokeMm')"
                name="maxStrokeMm"
              >
                <a-input-number
                  v-model:value="formState.maxStrokeMm"
                  :placeholder="pi.ph('maxStrokeMm')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('openStrokeMm')"
                name="openStrokeMm"
              >
                <a-input-number
                  v-model:value="formState.openStrokeMm"
                  :placeholder="pi.ph('openStrokeMm')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('platenSize')"
                name="platenSize"
              >
                <a-input
                  v-model:value="formState.platenSize"
                  :placeholder="pi.ph('platenSize')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ratedVoltage')"
                name="ratedVoltage"
              >
                <a-input-number
                  v-model:value="formState.ratedVoltage"
                  :placeholder="pi.ph('ratedVoltage')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ratedPowerKw')"
                name="ratedPowerKw"
              >
                <a-input-number
                  v-model:value="formState.ratedPowerKw"
                  :placeholder="pi.ph('ratedPowerKw')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('airConsumptionLpm')"
                name="airConsumptionLpm"
              >
                <a-input-number
                  v-model:value="formState.airConsumptionLpm"
                  :placeholder="pi.ph('airConsumptionLpm')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('coolingWaterFlowLpm')"
                name="coolingWaterFlowLpm"
              >
                <a-input-number
                  v-model:value="formState.coolingWaterFlowLpm"
                  :placeholder="pi.ph('coolingWaterFlowLpm')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('operatorCount')"
                name="operatorCount"
              >
                <a-input-number
                  v-model:value="formState.operatorCount"
                  :placeholder="pi.ph('operatorCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isCriticalResource')"
                name="isCriticalResource"
              >
                <TaktSelect
                  v-model:value="formState.isCriticalResource"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isCriticalResource')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('parallelCapacity')"
                name="parallelCapacity"
              >
                <a-input-number
                  v-model:value="formState.parallelCapacity"
                  :placeholder="pi.ph('parallelCapacity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowRushOrder')"
                name="allowRushOrder"
              >
                <TaktSelect
                  v-model:value="formState.allowRushOrder"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('allowRushOrder')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('warmupMinutes')"
                name="warmupMinutes"
              >
                <a-input-number
                  v-model:value="formState.warmupMinutes"
                  :placeholder="pi.ph('warmupMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('operatingTempRange')"
                name="operatingTempRange"
              >
                <a-input
                  v-model:value="formState.operatingTempRange"
                  :placeholder="pi.ph('operatingTempRange')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('operatingHumidityRange')"
                name="operatingHumidityRange"
              >
                <a-input
                  v-model:value="formState.operatingHumidityRange"
                  :placeholder="pi.ph('operatingHumidityRange')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('noiseLevelDb')"
                name="noiseLevelDb"
              >
                <a-input-number
                  v-model:value="formState.noiseLevelDb"
                  :placeholder="pi.ph('noiseLevelDb')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentRunStatus')"
                name="equipmentRunStatus"
              >
                <TaktSelect
                  v-model:value="formState.equipmentRunStatus"
                  dict-type="logistics_equipment_run_status"
                  :placeholder="pi.ph('equipmentRunStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceIntervalHours')"
                name="maintenanceIntervalHours"
              >
                <a-input-number
                  v-model:value="formState.maintenanceIntervalHours"
                  :placeholder="pi.ph('maintenanceIntervalHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cumulativeRunHours')"
                name="cumulativeRunHours"
              >
                <a-input-number
                  v-model:value="formState.cumulativeRunHours"
                  :placeholder="pi.ph('cumulativeRunHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('interfaceType')"
                name="interfaceType"
              >
                <a-input
                  v-model:value="formState.interfaceType"
                  :placeholder="pi.ph('interfaceType')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('commissioningDate')"
                name="commissioningDate"
              >
                <a-date-picker
                  v-model:value="formState.commissioningDate"
                  :placeholder="pi.ph('commissioningDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('decommissioningDate')"
                name="decommissioningDate"
              >
                <a-date-picker
                  v-model:value="formState.decommissioningDate"
                  :placeholder="pi.ph('decommissioningDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scrapDate')"
                name="scrapDate"
              >
                <a-date-picker
                  v-model:value="formState.scrapDate"
                  :placeholder="pi.ph('scrapDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('storageLocation')"
                name="storageLocation"
              >
                <a-input
                  v-model:value="formState.storageLocation"
                  :placeholder="pi.ph('storageLocation')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentAdministrator')"
                name="equipmentAdministrator"
              >
                <TaktSelect
                  v-model:value="formState.equipmentAdministrator"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('equipmentAdministrator')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionEquipmentStatus')"
                name="productionEquipmentStatus"
              >
                <TaktSelect
                  v-model:value="formState.productionEquipmentStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('productionEquipmentStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-6"
        :tab="t('common.page.form.tabs.basicinfo') + ' (7/7)'"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 生产设备主数据维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/mps/production-equipment/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useProductionEquipmentI18n } from '../composables/use-production-equipment-i18n'

/** 实体字段 i18n */
const pi = useProductionEquipmentI18n()
import type { ProductionEquipmentCreate } from '@/types/logistics/manufacturing/mps/production-equipment'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.companyDefaultCulture) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ProductionEquipmentCreate & { productionEquipmentId?: string }> | null
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
  productionEquipmentStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 productionEquipmentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.productionEquipmentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    if (!props.formData?.productionEquipmentId) {
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
  equipmentCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('equipmentCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('equipmentCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  productionEquipmentCode: [
    {
      required: true,
      message: pi.ph('productionEquipmentCode'),
      trigger: 'blur'
    }
  ],
  productionEquipmentName: [
    {
      required: true,
      message: pi.ph('productionEquipmentName'),
      trigger: 'blur'
    }
  ],
  machineType: [
    {
      required: true,
      message: pi.ph('machineType'),
      trigger: 'blur'
    }
  ],
  stdCycleTimeSeconds: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdCycleTimeSeconds'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdCycleTimeSeconds'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdMinutesPerUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdMinutesPerUnit'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdMinutesPerUnit'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdMinutesPerCycle: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdMinutesPerCycle'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdMinutesPerCycle'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  theoreticalSpm: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('theoreticalSpm'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('theoreticalSpm'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  theoreticalCycleTimeSeconds: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('theoreticalCycleTimeSeconds'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('theoreticalCycleTimeSeconds'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdEquipmentHourlyCapacity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdEquipmentHourlyCapacity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdEquipmentHourlyCapacity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  availabilityRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('availabilityRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('availabilityRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  performanceRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('performanceRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('performanceRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  setupMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('setupMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('setupMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  moldChangeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('moldChangeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('moldChangeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialChangeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('materialChangeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('materialChangeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  mtbfHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('mtbfHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('mtbfHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  mttrHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('mttrHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('mttrHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  cavityCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('cavityCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('cavityCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  quickMoldChange: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('quickMoldChange'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('quickMoldChange'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  operatorCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('operatorCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('operatorCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCriticalResource: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isCriticalResource'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isCriticalResource'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  parallelCapacity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('parallelCapacity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('parallelCapacity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  allowRushOrder: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('allowRushOrder'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('allowRushOrder'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  warmupMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('warmupMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('warmupMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  equipmentRunStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('equipmentRunStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('equipmentRunStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceIntervalHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceIntervalHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceIntervalHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  cumulativeRunHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('cumulativeRunHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('cumulativeRunHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  productionEquipmentStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('productionEquipmentStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('productionEquipmentStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('equipmentCategory' in payload) {
    const rawequipmentCategory = payload.equipmentCategory
    payload.equipmentCategory = typeof rawequipmentCategory === 'number' ? rawequipmentCategory : Number(rawequipmentCategory)
  }
  if ('stdCycleTimeSeconds' in payload) {
    const rawstdCycleTimeSeconds = payload.stdCycleTimeSeconds
    payload.stdCycleTimeSeconds = typeof rawstdCycleTimeSeconds === 'number' ? rawstdCycleTimeSeconds : Number(rawstdCycleTimeSeconds)
  }
  if ('stdMinutesPerUnit' in payload) {
    const rawstdMinutesPerUnit = payload.stdMinutesPerUnit
    payload.stdMinutesPerUnit = typeof rawstdMinutesPerUnit === 'number' ? rawstdMinutesPerUnit : Number(rawstdMinutesPerUnit)
  }
  if ('stdMinutesPerCycle' in payload) {
    const rawstdMinutesPerCycle = payload.stdMinutesPerCycle
    payload.stdMinutesPerCycle = typeof rawstdMinutesPerCycle === 'number' ? rawstdMinutesPerCycle : Number(rawstdMinutesPerCycle)
  }
  if ('theoreticalSpm' in payload) {
    const rawtheoreticalSpm = payload.theoreticalSpm
    payload.theoreticalSpm = typeof rawtheoreticalSpm === 'number' ? rawtheoreticalSpm : Number(rawtheoreticalSpm)
  }
  if ('theoreticalCycleTimeSeconds' in payload) {
    const rawtheoreticalCycleTimeSeconds = payload.theoreticalCycleTimeSeconds
    payload.theoreticalCycleTimeSeconds = typeof rawtheoreticalCycleTimeSeconds === 'number' ? rawtheoreticalCycleTimeSeconds : Number(rawtheoreticalCycleTimeSeconds)
  }
  if ('stdEquipmentHourlyCapacity' in payload) {
    const rawstdEquipmentHourlyCapacity = payload.stdEquipmentHourlyCapacity
    payload.stdEquipmentHourlyCapacity = typeof rawstdEquipmentHourlyCapacity === 'number' ? rawstdEquipmentHourlyCapacity : Number(rawstdEquipmentHourlyCapacity)
  }
  if ('availabilityRate' in payload) {
    const rawavailabilityRate = payload.availabilityRate
    payload.availabilityRate = typeof rawavailabilityRate === 'number' ? rawavailabilityRate : Number(rawavailabilityRate)
  }
  if ('performanceRate' in payload) {
    const rawperformanceRate = payload.performanceRate
    payload.performanceRate = typeof rawperformanceRate === 'number' ? rawperformanceRate : Number(rawperformanceRate)
  }
  if ('setupMinutes' in payload) {
    const rawsetupMinutes = payload.setupMinutes
    payload.setupMinutes = typeof rawsetupMinutes === 'number' ? rawsetupMinutes : Number(rawsetupMinutes)
  }
  if ('moldChangeMinutes' in payload) {
    const rawmoldChangeMinutes = payload.moldChangeMinutes
    payload.moldChangeMinutes = typeof rawmoldChangeMinutes === 'number' ? rawmoldChangeMinutes : Number(rawmoldChangeMinutes)
  }
  if ('materialChangeMinutes' in payload) {
    const rawmaterialChangeMinutes = payload.materialChangeMinutes
    payload.materialChangeMinutes = typeof rawmaterialChangeMinutes === 'number' ? rawmaterialChangeMinutes : Number(rawmaterialChangeMinutes)
  }
  if ('mtbfHours' in payload) {
    const rawmtbfHours = payload.mtbfHours
    payload.mtbfHours = typeof rawmtbfHours === 'number' ? rawmtbfHours : Number(rawmtbfHours)
  }
  if ('mttrHours' in payload) {
    const rawmttrHours = payload.mttrHours
    payload.mttrHours = typeof rawmttrHours === 'number' ? rawmttrHours : Number(rawmttrHours)
  }
  if ('repeatabilityAccuracy' in payload) {
    const rawrepeatabilityAccuracy = payload.repeatabilityAccuracy
    payload.repeatabilityAccuracy = typeof rawrepeatabilityAccuracy === 'number' ? rawrepeatabilityAccuracy : Number(rawrepeatabilityAccuracy)
  }
  if ('shutHeightAccuracy' in payload) {
    const rawshutHeightAccuracy = payload.shutHeightAccuracy
    payload.shutHeightAccuracy = typeof rawshutHeightAccuracy === 'number' ? rawshutHeightAccuracy : Number(rawshutHeightAccuracy)
  }
  if ('injectionAccuracy' in payload) {
    const rawinjectionAccuracy = payload.injectionAccuracy
    payload.injectionAccuracy = typeof rawinjectionAccuracy === 'number' ? rawinjectionAccuracy : Number(rawinjectionAccuracy)
  }
  if ('temperatureControlAccuracy' in payload) {
    const rawtemperatureControlAccuracy = payload.temperatureControlAccuracy
    payload.temperatureControlAccuracy = typeof rawtemperatureControlAccuracy === 'number' ? rawtemperatureControlAccuracy : Number(rawtemperatureControlAccuracy)
  }
  if ('pressureControlAccuracy' in payload) {
    const rawpressureControlAccuracy = payload.pressureControlAccuracy
    payload.pressureControlAccuracy = typeof rawpressureControlAccuracy === 'number' ? rawpressureControlAccuracy : Number(rawpressureControlAccuracy)
  }
  if ('processCapabilityCpk' in payload) {
    const rawprocessCapabilityCpk = payload.processCapabilityCpk
    payload.processCapabilityCpk = typeof rawprocessCapabilityCpk === 'number' ? rawprocessCapabilityCpk : Number(rawprocessCapabilityCpk)
  }
  if ('maxDimensionalTolerance' in payload) {
    const rawmaxDimensionalTolerance = payload.maxDimensionalTolerance
    payload.maxDimensionalTolerance = typeof rawmaxDimensionalTolerance === 'number' ? rawmaxDimensionalTolerance : Number(rawmaxDimensionalTolerance)
  }
  if ('maxMoldWeightTon' in payload) {
    const rawmaxMoldWeightTon = payload.maxMoldWeightTon
    payload.maxMoldWeightTon = typeof rawmaxMoldWeightTon === 'number' ? rawmaxMoldWeightTon : Number(rawmaxMoldWeightTon)
  }
  if ('ejectionType' in payload) {
    const rawejectionType = payload.ejectionType
    payload.ejectionType = typeof rawejectionType === 'number' ? rawejectionType : Number(rawejectionType)
  }
  if ('ejectionStrokeMm' in payload) {
    const rawejectionStrokeMm = payload.ejectionStrokeMm
    payload.ejectionStrokeMm = typeof rawejectionStrokeMm === 'number' ? rawejectionStrokeMm : Number(rawejectionStrokeMm)
  }
  if ('cavityCount' in payload) {
    const rawcavityCount = payload.cavityCount
    payload.cavityCount = typeof rawcavityCount === 'number' ? rawcavityCount : Number(rawcavityCount)
  }
  if ('quickMoldChange' in payload) {
    const rawquickMoldChange = payload.quickMoldChange
    payload.quickMoldChange = typeof rawquickMoldChange === 'number' ? rawquickMoldChange : Number(rawquickMoldChange)
  }
  if ('ratedTonnage' in payload) {
    const rawratedTonnage = payload.ratedTonnage
    payload.ratedTonnage = typeof rawratedTonnage === 'number' ? rawratedTonnage : Number(rawratedTonnage)
  }
  if ('clampingForceKn' in payload) {
    const rawclampingForceKn = payload.clampingForceKn
    payload.clampingForceKn = typeof rawclampingForceKn === 'number' ? rawclampingForceKn : Number(rawclampingForceKn)
  }
  if ('maxStrokeMm' in payload) {
    const rawmaxStrokeMm = payload.maxStrokeMm
    payload.maxStrokeMm = typeof rawmaxStrokeMm === 'number' ? rawmaxStrokeMm : Number(rawmaxStrokeMm)
  }
  if ('openStrokeMm' in payload) {
    const rawopenStrokeMm = payload.openStrokeMm
    payload.openStrokeMm = typeof rawopenStrokeMm === 'number' ? rawopenStrokeMm : Number(rawopenStrokeMm)
  }
  if ('ratedVoltage' in payload) {
    const rawratedVoltage = payload.ratedVoltage
    payload.ratedVoltage = typeof rawratedVoltage === 'number' ? rawratedVoltage : Number(rawratedVoltage)
  }
  if ('ratedPowerKw' in payload) {
    const rawratedPowerKw = payload.ratedPowerKw
    payload.ratedPowerKw = typeof rawratedPowerKw === 'number' ? rawratedPowerKw : Number(rawratedPowerKw)
  }
  if ('airConsumptionLpm' in payload) {
    const rawairConsumptionLpm = payload.airConsumptionLpm
    payload.airConsumptionLpm = typeof rawairConsumptionLpm === 'number' ? rawairConsumptionLpm : Number(rawairConsumptionLpm)
  }
  if ('coolingWaterFlowLpm' in payload) {
    const rawcoolingWaterFlowLpm = payload.coolingWaterFlowLpm
    payload.coolingWaterFlowLpm = typeof rawcoolingWaterFlowLpm === 'number' ? rawcoolingWaterFlowLpm : Number(rawcoolingWaterFlowLpm)
  }
  if ('operatorCount' in payload) {
    const rawoperatorCount = payload.operatorCount
    payload.operatorCount = typeof rawoperatorCount === 'number' ? rawoperatorCount : Number(rawoperatorCount)
  }
  if ('isCriticalResource' in payload) {
    const rawisCriticalResource = payload.isCriticalResource
    payload.isCriticalResource = typeof rawisCriticalResource === 'number' ? rawisCriticalResource : Number(rawisCriticalResource)
  }
  if ('parallelCapacity' in payload) {
    const rawparallelCapacity = payload.parallelCapacity
    payload.parallelCapacity = typeof rawparallelCapacity === 'number' ? rawparallelCapacity : Number(rawparallelCapacity)
  }
  if ('allowRushOrder' in payload) {
    const rawallowRushOrder = payload.allowRushOrder
    payload.allowRushOrder = typeof rawallowRushOrder === 'number' ? rawallowRushOrder : Number(rawallowRushOrder)
  }
  if ('warmupMinutes' in payload) {
    const rawwarmupMinutes = payload.warmupMinutes
    payload.warmupMinutes = typeof rawwarmupMinutes === 'number' ? rawwarmupMinutes : Number(rawwarmupMinutes)
  }
  if ('noiseLevelDb' in payload) {
    const rawnoiseLevelDb = payload.noiseLevelDb
    payload.noiseLevelDb = typeof rawnoiseLevelDb === 'number' ? rawnoiseLevelDb : Number(rawnoiseLevelDb)
  }
  if ('equipmentRunStatus' in payload) {
    const rawequipmentRunStatus = payload.equipmentRunStatus
    payload.equipmentRunStatus = typeof rawequipmentRunStatus === 'number' ? rawequipmentRunStatus : Number(rawequipmentRunStatus)
  }
  if ('maintenanceIntervalHours' in payload) {
    const rawmaintenanceIntervalHours = payload.maintenanceIntervalHours
    payload.maintenanceIntervalHours = typeof rawmaintenanceIntervalHours === 'number' ? rawmaintenanceIntervalHours : Number(rawmaintenanceIntervalHours)
  }
  if ('cumulativeRunHours' in payload) {
    const rawcumulativeRunHours = payload.cumulativeRunHours
    payload.cumulativeRunHours = typeof rawcumulativeRunHours === 'number' ? rawcumulativeRunHours : Number(rawcumulativeRunHours)
  }
  if ('productionEquipmentStatus' in payload) {
    const rawproductionEquipmentStatus = payload.productionEquipmentStatus
    payload.productionEquipmentStatus = typeof rawproductionEquipmentStatus === 'number' ? rawproductionEquipmentStatus : Number(rawproductionEquipmentStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.productionEquipmentId)

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
