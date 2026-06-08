<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-operation/components -->
<!-- 文件名称：quality-operation-form.vue -->
<!-- 功能描述：品质业务主表维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quality-operation-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
                :label="t('entity.qualityOperation.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityOperation.code')"
                name="qualityOperationCode"
              >
                <a-input
                  v-model:value="formState.qualityOperationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityOperation.operationmonth')"
                name="operationMonth"
              >
                <a-input
                  v-model:value="formState.operationMonth"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.operationmonth') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityOperation.customername')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.customername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityOperation.debitnoteno')"
                name="debitNoteNo"
              >
                <a-textarea
                  v-model:value="formState.debitNoteNo"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperation.debitnoteno') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityOperation.recorder')"
                name="recorder"
              >
                <a-input
                  v-model:value="formState.recorder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.recorder') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityOperation.totalqualitycost')"
                name="totalQualityCost"
              >
                <a-input-number
                  v-model:value="formState.totalQualityCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.totalqualitycost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityOperation.costcurrency')"
                name="costCurrency"
              >
                <a-input
                  v-model:value="formState.costCurrency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.costcurrency') })"
                  size="small"
                  allow-clear
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
      <!-- 子表：qualityOperationIncoming -->
      <a-tab-pane
        key="child-incomingItems"
        :tab="t('entity.qualityOperationIncoming._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityOperationIncomingRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityOperationIncoming._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityOperationIncomingFormColumns"
          :data-source="childQualityOperationIncomingRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationIncoming.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'directManpowerCostPerMinute'">
              <a-input-number
                v-model:value="record.directManpowerCostPerMinute"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationIncoming.directmanpowercostperminute') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'incomingInspectionCost'">
              <a-input-number
                v-model:value="record.incomingInspectionCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationIncoming.incominginspectioncost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'inspectionTimeMinutes'">
              <a-input-number
                v-model:value="record.inspectionTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationIncoming.inspectiontimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'travelCost'">
              <a-input-number
                v-model:value="record.travelCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationIncoming.travelcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'otherExpenses'">
              <a-input-number
                v-model:value="record.otherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationIncoming.otherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'incomingNote'">
              <a-textarea
                v-model:value="record.incomingNote"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperationIncoming.incomingnote') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityOperationIncomingRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：qualityOperationFirstArticle -->
      <a-tab-pane
        key="child-firstArticleItems"
        :tab="t('entity.qualityOperationFirstArticle._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityOperationFirstArticleRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityOperationFirstArticle._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityOperationFirstArticleFormColumns"
          :data-source="childQualityOperationFirstArticleRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationFirstArticle.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'qualificationCost'">
              <a-input-number
                v-model:value="record.qualificationCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationFirstArticle.qualificationcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'workTimeMinutes'">
              <a-input-number
                v-model:value="record.workTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationFirstArticle.worktimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'otherExpenses'">
              <a-input-number
                v-model:value="record.otherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationFirstArticle.otherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'qualificationNote'">
              <a-textarea
                v-model:value="record.qualificationNote"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperationFirstArticle.qualificationnote') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityOperationFirstArticleRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：qualityOperationCalibration -->
      <a-tab-pane
        key="child-calibrationItems"
        :tab="t('entity.qualityOperationCalibration._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityOperationCalibrationRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityOperationCalibration._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityOperationCalibrationFormColumns"
          :data-source="childQualityOperationCalibrationRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCalibration.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'calibrationCost'">
              <a-input-number
                v-model:value="record.calibrationCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCalibration.calibrationcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'workTimeMinutes'">
              <a-input-number
                v-model:value="record.workTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCalibration.worktimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'externalAgentServiceFee'">
              <a-input-number
                v-model:value="record.externalAgentServiceFee"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCalibration.externalagentservicefee') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'otherExpenses'">
              <a-input-number
                v-model:value="record.otherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCalibration.otherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'calibrationNote'">
              <a-textarea
                v-model:value="record.calibrationNote"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperationCalibration.calibrationnote') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityOperationCalibrationRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：qualityOperationOther -->
      <a-tab-pane
        key="child-otherItems"
        :tab="t('entity.qualityOperationOther._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityOperationOtherRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityOperationOther._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityOperationOtherFormColumns"
          :data-source="childQualityOperationOtherRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationOther.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'operationsCost'">
              <a-input-number
                v-model:value="record.operationsCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationOther.operationscost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'workTimeMinutes'">
              <a-input-number
                v-model:value="record.workTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationOther.worktimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'otherExpenses'">
              <a-input-number
                v-model:value="record.otherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationOther.otherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'otherNote'">
              <a-textarea
                v-model:value="record.otherNote"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperationOther.othernote') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityOperationOtherRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：qualityOperationOutgoing -->
      <a-tab-pane
        key="child-outgoingItems"
        :tab="t('entity.qualityOperationOutgoing._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityOperationOutgoingRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityOperationOutgoing._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityOperationOutgoingFormColumns"
          :data-source="childQualityOperationOutgoingRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationOutgoing.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'inspectionCost'">
              <a-input-number
                v-model:value="record.inspectionCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationOutgoing.inspectioncost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'inspectionTimeMinutes'">
              <a-input-number
                v-model:value="record.inspectionTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationOutgoing.inspectiontimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'otherExpenses'">
              <a-input-number
                v-model:value="record.otherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationOutgoing.otherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'outgoingNote'">
              <a-textarea
                v-model:value="record.outgoingNote"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperationOutgoing.outgoingnote') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityOperationOutgoingRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：qualityOperationReliability -->
      <a-tab-pane
        key="child-reliabilityItems"
        :tab="t('entity.qualityOperationReliability._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityOperationReliabilityRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityOperationReliability._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityOperationReliabilityFormColumns"
          :data-source="childQualityOperationReliabilityRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationReliability.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'testCost'">
              <a-input-number
                v-model:value="record.testCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationReliability.testcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'workTimeMinutes'">
              <a-input-number
                v-model:value="record.workTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationReliability.worktimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'otherExpenses'">
              <a-input-number
                v-model:value="record.otherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationReliability.otherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'reliabilityNote'">
              <a-textarea
                v-model:value="record.reliabilityNote"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperationReliability.reliabilitynote') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityOperationReliabilityRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：qualityOperationCustomerResponse -->
      <a-tab-pane
        key="child-customerResponseItems"
        :tab="t('entity.qualityOperationCustomerResponse._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityOperationCustomerResponseRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityOperationCustomerResponse._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityOperationCustomerResponseFormColumns"
          :data-source="childQualityOperationCustomerResponseRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCustomerResponse.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'responseCost'">
              <a-input-number
                v-model:value="record.responseCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCustomerResponse.responsecost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'workTimeMinutes'">
              <a-input-number
                v-model:value="record.workTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCustomerResponse.worktimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'otherExpenses'">
              <a-input-number
                v-model:value="record.otherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperationCustomerResponse.otherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'customerResponseNote'">
              <a-textarea
                v-model:value="record.customerResponseNote"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperationCustomerResponse.customerresponsenote') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityOperationCustomerResponseRow(index)">
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
 * 品质业务主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-operation/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityOperationCreate, QualityOperationIncomingCreate, QualityOperationIncoming, QualityOperationFirstArticleCreate, QualityOperationFirstArticle, QualityOperationCalibrationCreate, QualityOperationCalibration, QualityOperationOtherCreate, QualityOperationOther, QualityOperationOutgoingCreate, QualityOperationOutgoing, QualityOperationReliabilityCreate, QualityOperationReliability, QualityOperationCustomerResponseCreate, QualityOperationCustomerResponse } from '@/types/logistics/quality/cost/quality-operation'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","qualityOperationCode","operationMonth","customerName","debitNoteNo","recorder","totalQualityCost","costCurrency","extFieldJson","remark"]

/** qualityOperationIncoming 子表行（表单 Tab 内嵌） */
const childQualityOperationIncomingRows = ref<Record<string, unknown>[]>([])
/** qualityOperationFirstArticle 子表行（表单 Tab 内嵌） */
const childQualityOperationFirstArticleRows = ref<Record<string, unknown>[]>([])
/** qualityOperationCalibration 子表行（表单 Tab 内嵌） */
const childQualityOperationCalibrationRows = ref<Record<string, unknown>[]>([])
/** qualityOperationOther 子表行（表单 Tab 内嵌） */
const childQualityOperationOtherRows = ref<Record<string, unknown>[]>([])
/** qualityOperationOutgoing 子表行（表单 Tab 内嵌） */
const childQualityOperationOutgoingRows = ref<Record<string, unknown>[]>([])
/** qualityOperationReliability 子表行（表单 Tab 内嵌） */
const childQualityOperationReliabilityRows = ref<Record<string, unknown>[]>([])
/** qualityOperationCustomerResponse 子表行（表单 Tab 内嵌） */
const childQualityOperationCustomerResponseRows = ref<Record<string, unknown>[]>([])

/** 子表 qualityOperationIncoming 表单列定义 */
const qualityOperationIncomingFormColumns = computed(() => [
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
    title: t('entity.qualityOperationIncoming.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityOperationIncoming.directmanpowercostperminute'),
    dataIndex: 'directManpowerCostPerMinute',
    key: 'directManpowerCostPerMinute',
    width: 140,
  },
  {
    title: t('entity.qualityOperationIncoming.incominginspectioncost'),
    dataIndex: 'incomingInspectionCost',
    key: 'incomingInspectionCost',
    width: 140,
  },
  {
    title: t('entity.qualityOperationIncoming.inspectiontimeminutes'),
    dataIndex: 'inspectionTimeMinutes',
    key: 'inspectionTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityOperationIncoming.travelcost'),
    dataIndex: 'travelCost',
    key: 'travelCost',
    width: 140,
  },
  {
    title: t('entity.qualityOperationIncoming.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 140,
  },
  {
    title: t('entity.qualityOperationIncoming.incomingnote'),
    dataIndex: 'incomingNote',
    key: 'incomingNote',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 qualityOperationFirstArticle 表单列定义 */
const qualityOperationFirstArticleFormColumns = computed(() => [
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
    title: t('entity.qualityOperationFirstArticle.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityOperationFirstArticle.qualificationcost'),
    dataIndex: 'qualificationCost',
    key: 'qualificationCost',
    width: 140,
  },
  {
    title: t('entity.qualityOperationFirstArticle.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityOperationFirstArticle.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 140,
  },
  {
    title: t('entity.qualityOperationFirstArticle.qualificationnote'),
    dataIndex: 'qualificationNote',
    key: 'qualificationNote',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 qualityOperationCalibration 表单列定义 */
const qualityOperationCalibrationFormColumns = computed(() => [
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
    title: t('entity.qualityOperationCalibration.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCalibration.calibrationcost'),
    dataIndex: 'calibrationCost',
    key: 'calibrationCost',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCalibration.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCalibration.externalagentservicefee'),
    dataIndex: 'externalAgentServiceFee',
    key: 'externalAgentServiceFee',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCalibration.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCalibration.calibrationnote'),
    dataIndex: 'calibrationNote',
    key: 'calibrationNote',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 qualityOperationOther 表单列定义 */
const qualityOperationOtherFormColumns = computed(() => [
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
    title: t('entity.qualityOperationOther.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityOperationOther.operationscost'),
    dataIndex: 'operationsCost',
    key: 'operationsCost',
    width: 140,
  },
  {
    title: t('entity.qualityOperationOther.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityOperationOther.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 140,
  },
  {
    title: t('entity.qualityOperationOther.othernote'),
    dataIndex: 'otherNote',
    key: 'otherNote',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 qualityOperationOutgoing 表单列定义 */
const qualityOperationOutgoingFormColumns = computed(() => [
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
    title: t('entity.qualityOperationOutgoing.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityOperationOutgoing.inspectioncost'),
    dataIndex: 'inspectionCost',
    key: 'inspectionCost',
    width: 140,
  },
  {
    title: t('entity.qualityOperationOutgoing.inspectiontimeminutes'),
    dataIndex: 'inspectionTimeMinutes',
    key: 'inspectionTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityOperationOutgoing.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 140,
  },
  {
    title: t('entity.qualityOperationOutgoing.outgoingnote'),
    dataIndex: 'outgoingNote',
    key: 'outgoingNote',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 qualityOperationReliability 表单列定义 */
const qualityOperationReliabilityFormColumns = computed(() => [
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
    title: t('entity.qualityOperationReliability.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityOperationReliability.testcost'),
    dataIndex: 'testCost',
    key: 'testCost',
    width: 140,
  },
  {
    title: t('entity.qualityOperationReliability.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityOperationReliability.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 140,
  },
  {
    title: t('entity.qualityOperationReliability.reliabilitynote'),
    dataIndex: 'reliabilityNote',
    key: 'reliabilityNote',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 qualityOperationCustomerResponse 表单列定义 */
const qualityOperationCustomerResponseFormColumns = computed(() => [
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
    title: t('entity.qualityOperationCustomerResponse.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.responsecost'),
    dataIndex: 'responseCost',
    key: 'responseCost',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 140,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.customerresponsenote'),
    dataIndex: 'customerResponseNote',
    key: 'customerResponseNote',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
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
function syncChildRowsFromFormData(val: Partial<QualityOperationCreate & { qualityOperationId?: string }> | null | undefined) {
  childQualityOperationIncomingRows.value = ((val as any)?.incomingItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityOperationIncomingId ?? `new-${index}`,
  }))
  childQualityOperationFirstArticleRows.value = ((val as any)?.firstArticleItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityOperationFirstArticleId ?? `new-${index}`,
  }))
  childQualityOperationCalibrationRows.value = ((val as any)?.calibrationItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityOperationCalibrationId ?? `new-${index}`,
  }))
  childQualityOperationOtherRows.value = ((val as any)?.otherItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityOperationOtherId ?? `new-${index}`,
  }))
  childQualityOperationOutgoingRows.value = ((val as any)?.outgoingItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityOperationOutgoingId ?? `new-${index}`,
  }))
  childQualityOperationReliabilityRows.value = ((val as any)?.reliabilityItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityOperationReliabilityId ?? `new-${index}`,
  }))
  childQualityOperationCustomerResponseRows.value = ((val as any)?.customerResponseItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityOperationCustomerResponseId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 qualityOperationIncoming 行 */
function handleAddQualityOperationIncomingRow() {
  childQualityOperationIncomingRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      directManpowerCostPerMinute: 0,
      incomingInspectionCost: 0,
      inspectionTimeMinutes: 0,
      travelCost: 0,
      otherExpenses: 0,
      incomingNote: '',
      extFieldJson: '',
  })
}

/** 表单 Tab 内删除 qualityOperationIncoming 行 */
function handleRemoveQualityOperationIncomingRow(index: number) {
  childQualityOperationIncomingRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 qualityOperationFirstArticle 行 */
function handleAddQualityOperationFirstArticleRow() {
  childQualityOperationFirstArticleRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      qualificationCost: 0,
      workTimeMinutes: 0,
      otherExpenses: 0,
      qualificationNote: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 qualityOperationFirstArticle 行 */
function handleRemoveQualityOperationFirstArticleRow(index: number) {
  childQualityOperationFirstArticleRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 qualityOperationCalibration 行 */
function handleAddQualityOperationCalibrationRow() {
  childQualityOperationCalibrationRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      calibrationCost: 0,
      workTimeMinutes: 0,
      externalAgentServiceFee: 0,
      otherExpenses: 0,
      calibrationNote: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 qualityOperationCalibration 行 */
function handleRemoveQualityOperationCalibrationRow(index: number) {
  childQualityOperationCalibrationRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 qualityOperationOther 行 */
function handleAddQualityOperationOtherRow() {
  childQualityOperationOtherRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      operationsCost: 0,
      workTimeMinutes: 0,
      otherExpenses: 0,
      otherNote: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 qualityOperationOther 行 */
function handleRemoveQualityOperationOtherRow(index: number) {
  childQualityOperationOtherRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 qualityOperationOutgoing 行 */
function handleAddQualityOperationOutgoingRow() {
  childQualityOperationOutgoingRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      inspectionCost: 0,
      inspectionTimeMinutes: 0,
      otherExpenses: 0,
      outgoingNote: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 qualityOperationOutgoing 行 */
function handleRemoveQualityOperationOutgoingRow(index: number) {
  childQualityOperationOutgoingRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 qualityOperationReliability 行 */
function handleAddQualityOperationReliabilityRow() {
  childQualityOperationReliabilityRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      testCost: 0,
      workTimeMinutes: 0,
      otherExpenses: 0,
      reliabilityNote: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 qualityOperationReliability 行 */
function handleRemoveQualityOperationReliabilityRow(index: number) {
  childQualityOperationReliabilityRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 qualityOperationCustomerResponse 行 */
function handleAddQualityOperationCustomerResponseRow() {
  childQualityOperationCustomerResponseRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      responseCost: 0,
      workTimeMinutes: 0,
      otherExpenses: 0,
      customerResponseNote: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 qualityOperationCustomerResponse 行 */
function handleRemoveQualityOperationCustomerResponseRow(index: number) {
  childQualityOperationCustomerResponseRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    incomingItems: childQualityOperationIncomingRows.value.map(({ __rowKey, ...rest }) => rest),
    firstArticleItems: childQualityOperationFirstArticleRows.value.map(({ __rowKey, ...rest }) => rest),
    calibrationItems: childQualityOperationCalibrationRows.value.map(({ __rowKey, ...rest }) => rest),
    otherItems: childQualityOperationOtherRows.value.map(({ __rowKey, ...rest }) => rest),
    outgoingItems: childQualityOperationOutgoingRows.value.map(({ __rowKey, ...rest }) => rest),
    reliabilityItems: childQualityOperationReliabilityRows.value.map(({ __rowKey, ...rest }) => rest),
    customerResponseItems: childQualityOperationCustomerResponseRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityOperationCreate & { qualityOperationId?: string }> | null
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
    delete (next as any).incomingItems
    delete (next as any).firstArticleItems
    delete (next as any).calibrationItems
    delete (next as any).otherItems
    delete (next as any).outgoingItems
    delete (next as any).reliabilityItems
    delete (next as any).customerResponseItems
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
    const isCreate = !props.formData?.qualityOperationId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.plantcode') }),
      trigger: 'blur'
    }
  ],
  qualityOperationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.code') }),
      trigger: 'blur'
    }
  ],
  operationMonth: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.operationmonth') }),
      trigger: 'blur'
    }
  ],
  totalQualityCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityOperation.totalqualitycost') }),
      trigger: 'change'
    }
  ],
  costCurrency: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.costcurrency') }),
      trigger: 'blur'
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
  childQualityOperationIncomingRows.value = []
  childQualityOperationFirstArticleRows.value = []
  childQualityOperationCalibrationRows.value = []
  childQualityOperationOtherRows.value = []
  childQualityOperationOutgoingRows.value = []
  childQualityOperationReliabilityRows.value = []
  childQualityOperationCustomerResponseRows.value = []
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
