<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/components -->
<!-- 文件名称：supplier-evaluation-form.vue -->
<!-- 功能描述：供应商评价考核主表实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="supplier-evaluation-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
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
                :label="t('entity.supplierEvaluation.code')"
                name="supplierEvaluationCode"
              >
                <a-input
                  v-model:value="formState.supplierEvaluationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.supplierid')"
                name="supplierId"
              >
                <a-input
                  v-model:value="formState.supplierId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.supplierid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.suppliername')"
                name="supplierName"
              >
                <a-input
                  v-model:value="formState.supplierName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.suppliercode')"
                name="supplierCode"
              >
                <a-input
                  v-model:value="formState.supplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationdate')"
                name="evaluationDate"
              >
                <a-date-picker
                  v-model:value="formState.evaluationDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationperiod')"
                name="evaluationPeriod"
              >
                <a-input-number
                  v-model:value="formState.evaluationPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationperiod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationtype')"
                name="evaluationType"
              >
                <a-input-number
                  v-model:value="formState.evaluationType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationtype') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluatorby')"
                name="evaluatorBy"
              >
                <a-input
                  v-model:value="formState.evaluatorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluatorby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationdept')"
                name="evaluationDept"
              >
                <a-input
                  v-model:value="formState.evaluationDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationdept') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.overallrating')"
                name="overallRating"
              >
                <a-input-number
                  v-model:value="formState.overallRating"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.overallrating') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.totalscore')"
                name="totalScore"
              >
                <a-input-number
                  v-model:value="formState.totalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.totalscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.qualityscore')"
                name="qualityScore"
              >
                <a-input-number
                  v-model:value="formState.qualityScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.qualityscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.deliveryscore')"
                name="deliveryScore"
              >
                <a-input-number
                  v-model:value="formState.deliveryScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.deliveryscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.pricescore')"
                name="priceScore"
              >
                <a-input-number
                  v-model:value="formState.priceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.pricescore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.servicescore')"
                name="serviceScore"
              >
                <a-input-number
                  v-model:value="formState.serviceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.servicescore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.technicalscore')"
                name="technicalScore"
              >
                <a-input-number
                  v-model:value="formState.technicalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.technicalscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.mainstrengths')"
                name="mainStrengths"
              >
                <a-input
                  v-model:value="formState.mainStrengths"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.mainstrengths') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.mainissues')"
                name="mainIssues"
              >
                <a-input
                  v-model:value="formState.mainIssues"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.mainissues') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.improvementrequirements')"
                name="improvementRequirements"
              >
                <a-input
                  v-model:value="formState.improvementRequirements"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.improvementrequirements') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationconclusion')"
                name="evaluationConclusion"
              >
                <a-input-number
                  v-model:value="formState.evaluationConclusion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationconclusion') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.rectificationdeadline')"
                name="rectificationDeadline"
              >
                <a-input
                  v-model:value="formState.rectificationDeadline"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.rectificationdeadline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationstatus')"
                name="evaluationStatus"
              >
                <a-input-number
                  v-model:value="formState.evaluationStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.rectificationstatus')"
                name="rectificationStatus"
              >
                <a-input-number
                  v-model:value="formState.rectificationStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.rectificationstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.sortorder') })"
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
      <!-- 子表：supplierEvaluationItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.supplierEvaluationItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddSupplierEvaluationItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.supplierEvaluationItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="supplierEvaluationItemFormColumns"
          :data-source="childSupplierEvaluationItemRows"
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
            <template v-else-if="column.key === 'evaluationId'">
              <a-input
                v-model:value="record.evaluationId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.evaluationid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'categoryType'">
              <a-input-number
                v-model:value="record.categoryType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.categorytype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'itemName'">
              <a-input
                v-model:value="record.itemName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.itemname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'itemDescription'">
              <a-textarea
                v-model:value="record.itemDescription"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplierEvaluationItem.itemdescription') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'weight'">
              <a-input-number
                v-model:value="record.weight"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.weight') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'scoringStandard'">
              <a-input
                v-model:value="record.scoringStandard"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.scoringstandard') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'score'">
              <a-input-number
                v-model:value="record.score"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.score') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveSupplierEvaluationItemRow(index)">
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
 * 供应商评价考核主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/supplier-evaluation/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SupplierEvaluationCreate, SupplierEvaluationItemCreate, SupplierEvaluationItem } from '@/types/logistics/quality/complaint/supplier-evaluation'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","supplierEvaluationCode","supplierId","supplierName","supplierCode","evaluationDate","evaluationPeriod","evaluationType","evaluatorBy","evaluationDept","overallRating","totalScore","qualityScore","deliveryScore","priceScore","serviceScore","technicalScore","mainStrengths","mainIssues","improvementRequirements","evaluationConclusion","rectificationDeadline","evaluationStatus","rectificationStatus","relatedPlant","sortOrder","extFieldJson","remark"]

/** supplierEvaluationItem 子表行（表单 Tab 内嵌） */
const childSupplierEvaluationItemRows = ref<Record<string, unknown>[]>([])

/** 子表 supplierEvaluationItem 表单列定义 */
const supplierEvaluationItemFormColumns = computed(() => [
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
    title: t('entity.supplierEvaluationItem.evaluationid'),
    dataIndex: 'evaluationId',
    key: 'evaluationId',
    width: 140,
  },
  {
    title: t('entity.supplierEvaluationItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.supplierEvaluationItem.categorytype'),
    dataIndex: 'categoryType',
    key: 'categoryType',
    width: 140,
  },
  {
    title: t('entity.supplierEvaluationItem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 140,
  },
  {
    title: t('entity.supplierEvaluationItem.itemdescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    width: 140,
  },
  {
    title: t('entity.supplierEvaluationItem.weight'),
    dataIndex: 'weight',
    key: 'weight',
    width: 140,
  },
  {
    title: t('entity.supplierEvaluationItem.scoringstandard'),
    dataIndex: 'scoringStandard',
    key: 'scoringStandard',
    width: 140,
  },
  {
    title: t('entity.supplierEvaluationItem.score'),
    dataIndex: 'score',
    key: 'score',
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
function syncChildRowsFromFormData(val: Partial<SupplierEvaluationCreate & { supplierEvaluationId?: string }> | null | undefined) {
  childSupplierEvaluationItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.supplierEvaluationItemId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 supplierEvaluationItem 行 */
function handleAddSupplierEvaluationItemRow() {
  childSupplierEvaluationItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      evaluationId: '',
      lineNumber: 0,
      categoryType: 0,
      itemName: '',
      itemDescription: '',
      weight: 0,
      scoringStandard: '',
      score: 0,
  })
}

/** 表单 Tab 内删除 supplierEvaluationItem 行 */
function handleRemoveSupplierEvaluationItemRow(index: number) {
  childSupplierEvaluationItemRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childSupplierEvaluationItemRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SupplierEvaluationCreate & { supplierEvaluationId?: string }> | null
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
    delete (next as any).items
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
    const isCreate = !props.formData?.supplierEvaluationId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  supplierEvaluationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.code') }),
      trigger: 'blur'
    }
  ],
  supplierId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.supplierid') }),
      trigger: 'blur'
    }
  ],
  supplierName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliername') }),
      trigger: 'blur'
    }
  ],
  evaluationDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationdate') }),
      trigger: 'change'
    }
  ],
  evaluationPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationperiod') }),
      trigger: 'change'
    }
  ],
  evaluationType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationtype') }),
      trigger: 'change'
    }
  ],
  overallRating: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.overallrating') }),
      trigger: 'change'
    }
  ],
  evaluationConclusion: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationconclusion') }),
      trigger: 'change'
    }
  ],
  evaluationStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationstatus') }),
      trigger: 'change'
    }
  ],
  rectificationStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.rectificationstatus') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.sortorder') }),
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
  childSupplierEvaluationItemRows.value = []
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
