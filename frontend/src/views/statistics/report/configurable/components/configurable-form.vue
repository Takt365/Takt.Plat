<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/report/configurable/components -->
<!-- 文件名称：configurable-form.vue -->
<!-- 功能描述：自定义报表主实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="configurable-form-tabs"
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
                :label="t('entity.configurable.reportcode')"
                name="reportCode"
              >
                <a-input
                  v-model:value="formState.reportCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.reportcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.reportname')"
                name="reportName"
              >
                <a-input
                  v-model:value="formState.reportName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.reportname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.reportdomain')"
                name="reportDomain"
              >
                <a-input-number
                  v-model:value="formState.reportDomain"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.reportdomain') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.reportsubcategory')"
                name="reportSubCategory"
              >
                <a-input
                  v-model:value="formState.reportSubCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.reportsubcategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.distinctrows')"
                name="distinctRows"
              >
                <a-input-number
                  v-model:value="formState.distinctRows"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.distinctrows') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.maxexportrows')"
                name="maxExportRows"
              >
                <a-input-number
                  v-model:value="formState.maxExportRows"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.maxexportrows') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.maxqueryrows')"
                name="maxQueryRows"
              >
                <a-input-number
                  v-model:value="formState.maxQueryRows"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.maxqueryrows') })"
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
                :label="t('entity.configurable.owneruserid')"
                name="ownerUserId"
              >
                <a-input
                  v-model:value="formState.ownerUserId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.owneruserid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.isbuiltin')"
                name="isBuiltIn"
              >
                <a-input-number
                  v-model:value="formState.isBuiltIn"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.isbuiltin') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.configurable.reportstatus')"
                name="reportStatus"
              >
                <TaktSelect
                  v-model:value="formState.reportStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.reportstatus') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.configurable.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.configurable.description') })"
                  :rows="2"
                  size="small"
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
      <!-- 子表：configurableSource -->
      <a-tab-pane
        key="child-sources"
        :tab="t('entity.configurableSource._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddConfigurableSourceRow">
            {{ t('common.page.button.create') }}{{ t('entity.configurableSource._self') }}
          </a-button>
        </div>
        <a-table
          :columns="configurableSourceFormColumns"
          :data-source="childConfigurableSourceRows"
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
            <template v-else-if="column.key === 'sourceAlias'">
              <a-input
                v-model:value="record.sourceAlias"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSource.sourcealias') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'tableName'">
              <a-input
                v-model:value="record.tableName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSource.tablename') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'isPrimary'">
              <a-input-number
                v-model:value="record.isPrimary"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSource.isprimary') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'sortOrder'">
              <a-input-number
                v-model:value="record.sortOrder"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSource.sortorder') })"
                size="small"
                style="width: 100%"
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
              <a-button type="link" danger size="small" @click="handleRemoveConfigurableSourceRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：configurableJoin -->
      <a-tab-pane
        key="child-joins"
        :tab="t('entity.configurableJoin._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddConfigurableJoinRow">
            {{ t('common.page.button.create') }}{{ t('entity.configurableJoin._self') }}
          </a-button>
        </div>
        <a-table
          :columns="configurableJoinFormColumns"
          :data-source="childConfigurableJoinRows"
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
            <template v-else-if="column.key === 'joinType'">
              <a-input-number
                v-model:value="record.joinType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableJoin.jointype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'leftSourceAlias'">
              <a-input
                v-model:value="record.leftSourceAlias"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableJoin.leftsourcealias') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'leftColumnName'">
              <a-input
                v-model:value="record.leftColumnName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableJoin.leftcolumnname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'rightSourceAlias'">
              <a-input
                v-model:value="record.rightSourceAlias"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableJoin.rightsourcealias') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'rightColumnName'">
              <a-input
                v-model:value="record.rightColumnName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableJoin.rightcolumnname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'sortOrder'">
              <a-input-number
                v-model:value="record.sortOrder"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableJoin.sortorder') })"
                size="small"
                style="width: 100%"
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
              <a-button type="link" danger size="small" @click="handleRemoveConfigurableJoinRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：configurableField -->
      <a-tab-pane
        key="child-fields"
        :tab="t('entity.configurableField._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddConfigurableFieldRow">
            {{ t('common.page.button.create') }}{{ t('entity.configurableField._self') }}
          </a-button>
        </div>
        <a-table
          :columns="configurableFieldFormColumns"
          :data-source="childConfigurableFieldRows"
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
            <template v-else-if="column.key === 'sourceAlias'">
              <a-input
                v-model:value="record.sourceAlias"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableField.sourcealias') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'columnName'">
              <a-input
                v-model:value="record.columnName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableField.columnname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'displayName'">
              <a-input
                v-model:value="record.displayName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableField.displayname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'outputAlias'">
              <a-input
                v-model:value="record.outputAlias"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableField.outputalias') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'aggregateFunc'">
              <a-input-number
                v-model:value="record.aggregateFunc"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableField.aggregatefunc') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'isVisible'">
              <a-input-number
                v-model:value="record.isVisible"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableField.isvisible') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'sortOrder'">
              <a-input-number
                v-model:value="record.sortOrder"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableField.sortorder') })"
                size="small"
                style="width: 100%"
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
              <a-button type="link" danger size="small" @click="handleRemoveConfigurableFieldRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：configurableSelection -->
      <a-tab-pane
        key="child-selections"
        :tab="t('entity.configurableSelection._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddConfigurableSelectionRow">
            {{ t('common.page.button.create') }}{{ t('entity.configurableSelection._self') }}
          </a-button>
        </div>
        <a-table
          :columns="configurableSelectionFormColumns"
          :data-source="childConfigurableSelectionRows"
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
            <template v-else-if="column.key === 'sourceAlias'">
              <a-input
                v-model:value="record.sourceAlias"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSelection.sourcealias') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'columnName'">
              <a-input
                v-model:value="record.columnName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSelection.columnname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'displayName'">
              <a-input
                v-model:value="record.displayName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSelection.displayname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'filterOperator'">
              <a-input-number
                v-model:value="record.filterOperator"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSelection.filteroperator') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'defaultValue'">
              <a-input
                v-model:value="record.defaultValue"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSelection.defaultvalue') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'defaultValueTo'">
              <a-input
                v-model:value="record.defaultValueTo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSelection.defaultvalueto') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'isRequired'">
              <a-input-number
                v-model:value="record.isRequired"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSelection.isrequired') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'sortOrder'">
              <a-input-number
                v-model:value="record.sortOrder"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableSelection.sortorder') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveConfigurableSelectionRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：configurableGroupBy -->
      <a-tab-pane
        key="child-groupBys"
        :tab="t('entity.configurableGroupBy._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddConfigurableGroupByRow">
            {{ t('common.page.button.create') }}{{ t('entity.configurableGroupBy._self') }}
          </a-button>
        </div>
        <a-table
          :columns="configurableGroupByFormColumns"
          :data-source="childConfigurableGroupByRows"
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
            <template v-else-if="column.key === 'sourceAlias'">
              <a-input
                v-model:value="record.sourceAlias"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableGroupBy.sourcealias') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'columnName'">
              <a-input
                v-model:value="record.columnName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableGroupBy.columnname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'sortOrder'">
              <a-input-number
                v-model:value="record.sortOrder"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableGroupBy.sortorder') })"
                size="small"
                style="width: 100%"
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
              <a-button type="link" danger size="small" @click="handleRemoveConfigurableGroupByRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：configurableOrderBy -->
      <a-tab-pane
        key="child-orderBys"
        :tab="t('entity.configurableOrderBy._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddConfigurableOrderByRow">
            {{ t('common.page.button.create') }}{{ t('entity.configurableOrderBy._self') }}
          </a-button>
        </div>
        <a-table
          :columns="configurableOrderByFormColumns"
          :data-source="childConfigurableOrderByRows"
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
            <template v-else-if="column.key === 'sourceAlias'">
              <a-input
                v-model:value="record.sourceAlias"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableOrderBy.sourcealias') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'columnName'">
              <a-input
                v-model:value="record.columnName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableOrderBy.columnname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'sortDirection'">
              <a-input-number
                v-model:value="record.sortDirection"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableOrderBy.sortdirection') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'sortOrder'">
              <a-input-number
                v-model:value="record.sortOrder"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurableOrderBy.sortorder') })"
                size="small"
                style="width: 100%"
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
              <a-button type="link" danger size="small" @click="handleRemoveConfigurableOrderByRow(index)">
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
 * 自定义报表主实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/statistics/report/configurable/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ConfigurableCreate, ConfigurableSourceCreate, ConfigurableSource, ConfigurableJoinCreate, ConfigurableJoin, ConfigurableFieldCreate, ConfigurableField, ConfigurableSelectionCreate, ConfigurableSelection, ConfigurableGroupByCreate, ConfigurableGroupBy, ConfigurableOrderByCreate, ConfigurableOrderBy } from '@/types/statistics/report/configurable'
import TaktSelect from '@/components/business/takt-select/index.vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","reportCode","reportName","reportDomain","reportSubCategory","distinctRows","maxExportRows","maxQueryRows","ownerUserId","isBuiltIn","sortOrder","reportStatus","description","extFieldJson","remark"]

/** configurableSource 子表行（表单 Tab 内嵌） */
const childConfigurableSourceRows = ref<Record<string, unknown>[]>([])
/** configurableJoin 子表行（表单 Tab 内嵌） */
const childConfigurableJoinRows = ref<Record<string, unknown>[]>([])
/** configurableField 子表行（表单 Tab 内嵌） */
const childConfigurableFieldRows = ref<Record<string, unknown>[]>([])
/** configurableSelection 子表行（表单 Tab 内嵌） */
const childConfigurableSelectionRows = ref<Record<string, unknown>[]>([])
/** configurableGroupBy 子表行（表单 Tab 内嵌） */
const childConfigurableGroupByRows = ref<Record<string, unknown>[]>([])
/** configurableOrderBy 子表行（表单 Tab 内嵌） */
const childConfigurableOrderByRows = ref<Record<string, unknown>[]>([])

/** 子表 configurableSource 表单列定义 */
const configurableSourceFormColumns = computed(() => [
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
    title: t('entity.configurableSource.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    width: 140,
  },
  {
    title: t('entity.configurableSource.tablename'),
    dataIndex: 'tableName',
    key: 'tableName',
    width: 140,
  },
  {
    title: t('entity.configurableSource.isprimary'),
    dataIndex: 'isPrimary',
    key: 'isPrimary',
    width: 140,
  },
  {
    title: t('entity.configurableSource.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
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

/** 子表 configurableJoin 表单列定义 */
const configurableJoinFormColumns = computed(() => [
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
    title: t('entity.configurableJoin.jointype'),
    dataIndex: 'joinType',
    key: 'joinType',
    width: 140,
  },
  {
    title: t('entity.configurableJoin.leftsourcealias'),
    dataIndex: 'leftSourceAlias',
    key: 'leftSourceAlias',
    width: 140,
  },
  {
    title: t('entity.configurableJoin.leftcolumnname'),
    dataIndex: 'leftColumnName',
    key: 'leftColumnName',
    width: 140,
  },
  {
    title: t('entity.configurableJoin.rightsourcealias'),
    dataIndex: 'rightSourceAlias',
    key: 'rightSourceAlias',
    width: 140,
  },
  {
    title: t('entity.configurableJoin.rightcolumnname'),
    dataIndex: 'rightColumnName',
    key: 'rightColumnName',
    width: 140,
  },
  {
    title: t('entity.configurableJoin.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
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

/** 子表 configurableField 表单列定义 */
const configurableFieldFormColumns = computed(() => [
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
    title: t('entity.configurableField.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    width: 140,
  },
  {
    title: t('entity.configurableField.columnname'),
    dataIndex: 'columnName',
    key: 'columnName',
    width: 140,
  },
  {
    title: t('entity.configurableField.displayname'),
    dataIndex: 'displayName',
    key: 'displayName',
    width: 140,
  },
  {
    title: t('entity.configurableField.outputalias'),
    dataIndex: 'outputAlias',
    key: 'outputAlias',
    width: 140,
  },
  {
    title: t('entity.configurableField.aggregatefunc'),
    dataIndex: 'aggregateFunc',
    key: 'aggregateFunc',
    width: 140,
  },
  {
    title: t('entity.configurableField.isvisible'),
    dataIndex: 'isVisible',
    key: 'isVisible',
    width: 140,
  },
  {
    title: t('entity.configurableField.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
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

/** 子表 configurableSelection 表单列定义 */
const configurableSelectionFormColumns = computed(() => [
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
    title: t('entity.configurableSelection.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    width: 140,
  },
  {
    title: t('entity.configurableSelection.columnname'),
    dataIndex: 'columnName',
    key: 'columnName',
    width: 140,
  },
  {
    title: t('entity.configurableSelection.displayname'),
    dataIndex: 'displayName',
    key: 'displayName',
    width: 140,
  },
  {
    title: t('entity.configurableSelection.filteroperator'),
    dataIndex: 'filterOperator',
    key: 'filterOperator',
    width: 140,
  },
  {
    title: t('entity.configurableSelection.defaultvalue'),
    dataIndex: 'defaultValue',
    key: 'defaultValue',
    width: 140,
  },
  {
    title: t('entity.configurableSelection.defaultvalueto'),
    dataIndex: 'defaultValueTo',
    key: 'defaultValueTo',
    width: 140,
  },
  {
    title: t('entity.configurableSelection.isrequired'),
    dataIndex: 'isRequired',
    key: 'isRequired',
    width: 140,
  },
  {
    title: t('entity.configurableSelection.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 configurableGroupBy 表单列定义 */
const configurableGroupByFormColumns = computed(() => [
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
    title: t('entity.configurableGroupBy.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    width: 140,
  },
  {
    title: t('entity.configurableGroupBy.columnname'),
    dataIndex: 'columnName',
    key: 'columnName',
    width: 140,
  },
  {
    title: t('entity.configurableGroupBy.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
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

/** 子表 configurableOrderBy 表单列定义 */
const configurableOrderByFormColumns = computed(() => [
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
    title: t('entity.configurableOrderBy.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    width: 140,
  },
  {
    title: t('entity.configurableOrderBy.columnname'),
    dataIndex: 'columnName',
    key: 'columnName',
    width: 140,
  },
  {
    title: t('entity.configurableOrderBy.sortdirection'),
    dataIndex: 'sortDirection',
    key: 'sortDirection',
    width: 140,
  },
  {
    title: t('entity.configurableOrderBy.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
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
function syncChildRowsFromFormData(val: Partial<ConfigurableCreate & { configurableId?: string }> | null | undefined) {
  childConfigurableSourceRows.value = ((val as any)?.sources ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableSourceId ?? `new-${index}`,
  }))
  childConfigurableJoinRows.value = ((val as any)?.joins ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableJoinId ?? `new-${index}`,
  }))
  childConfigurableFieldRows.value = ((val as any)?.fields ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableFieldId ?? `new-${index}`,
  }))
  childConfigurableSelectionRows.value = ((val as any)?.selections ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableSelectionId ?? `new-${index}`,
  }))
  childConfigurableGroupByRows.value = ((val as any)?.groupBys ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableGroupById ?? `new-${index}`,
  }))
  childConfigurableOrderByRows.value = ((val as any)?.orderBys ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableOrderById ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 configurableSource 行 */
function handleAddConfigurableSourceRow() {
  childConfigurableSourceRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      sourceAlias: '',
      tableName: '',
      isPrimary: 0,
      sortOrder: 0,
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 configurableSource 行 */
function handleRemoveConfigurableSourceRow(index: number) {
  childConfigurableSourceRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 configurableJoin 行 */
function handleAddConfigurableJoinRow() {
  childConfigurableJoinRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      joinType: 0,
      leftSourceAlias: '',
      leftColumnName: '',
      rightSourceAlias: '',
      rightColumnName: '',
      sortOrder: 0,
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 configurableJoin 行 */
function handleRemoveConfigurableJoinRow(index: number) {
  childConfigurableJoinRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 configurableField 行 */
function handleAddConfigurableFieldRow() {
  childConfigurableFieldRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      sourceAlias: '',
      columnName: '',
      displayName: '',
      outputAlias: '',
      aggregateFunc: 0,
      isVisible: 0,
      sortOrder: 0,
      extFieldJson: '',
  })
}

/** 表单 Tab 内删除 configurableField 行 */
function handleRemoveConfigurableFieldRow(index: number) {
  childConfigurableFieldRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 configurableSelection 行 */
function handleAddConfigurableSelectionRow() {
  childConfigurableSelectionRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      sourceAlias: '',
      columnName: '',
      displayName: '',
      filterOperator: 0,
      defaultValue: '',
      defaultValueTo: '',
      isRequired: 0,
      sortOrder: 0,
  })
}

/** 表单 Tab 内删除 configurableSelection 行 */
function handleRemoveConfigurableSelectionRow(index: number) {
  childConfigurableSelectionRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 configurableGroupBy 行 */
function handleAddConfigurableGroupByRow() {
  childConfigurableGroupByRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      sourceAlias: '',
      columnName: '',
      sortOrder: 0,
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 configurableGroupBy 行 */
function handleRemoveConfigurableGroupByRow(index: number) {
  childConfigurableGroupByRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 configurableOrderBy 行 */
function handleAddConfigurableOrderByRow() {
  childConfigurableOrderByRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      sourceAlias: '',
      columnName: '',
      sortDirection: 0,
      sortOrder: 0,
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 configurableOrderBy 行 */
function handleRemoveConfigurableOrderByRow(index: number) {
  childConfigurableOrderByRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    sources: childConfigurableSourceRows.value.map(({ __rowKey, ...rest }) => rest),
    joins: childConfigurableJoinRows.value.map(({ __rowKey, ...rest }) => rest),
    fields: childConfigurableFieldRows.value.map(({ __rowKey, ...rest }) => rest),
    selections: childConfigurableSelectionRows.value.map(({ __rowKey, ...rest }) => rest),
    groupBys: childConfigurableGroupByRows.value.map(({ __rowKey, ...rest }) => rest),
    orderBys: childConfigurableOrderByRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ConfigurableCreate & { configurableId?: string }> | null
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
    delete (next as any).sources
    delete (next as any).joins
    delete (next as any).fields
    delete (next as any).selections
    delete (next as any).groupBys
    delete (next as any).orderBys
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
    const isCreate = !props.formData?.configurableId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  reportCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.configurable.reportcode') }),
      trigger: 'blur'
    }
  ],
  reportName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.configurable.reportname') }),
      trigger: 'blur'
    }
  ],
  reportDomain: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.configurable.reportdomain') }),
      trigger: 'change'
    }
  ],
  distinctRows: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.configurable.distinctrows') }),
      trigger: 'change'
    }
  ],
  maxExportRows: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.configurable.maxexportrows') }),
      trigger: 'change'
    }
  ],
  maxQueryRows: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.configurable.maxqueryrows') }),
      trigger: 'change'
    }
  ],
  isBuiltIn: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.configurable.isbuiltin') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.configurable.sortorder') }),
      trigger: 'change'
    }
  ],
  reportStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.configurable.reportstatus') }),
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
  childConfigurableSourceRows.value = []
  childConfigurableJoinRows.value = []
  childConfigurableFieldRows.value = []
  childConfigurableSelectionRows.value = []
  childConfigurableGroupByRows.value = []
  childConfigurableOrderByRows.value = []
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
