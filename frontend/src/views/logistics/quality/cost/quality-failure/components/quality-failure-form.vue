<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-failure/components -->
<!-- 文件名称：quality-failure-form.vue -->
<!-- 功能描述：品质问题应对主表维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quality-failure-form-tabs"
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
                :label="t('entity.qualityFailure.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.code')"
                name="qualityFailureCode"
              >
                <a-input
                  v-model:value="formState.qualityFailureCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.failuredate')"
                name="failureDate"
              >
                <a-date-picker
                  v-model:value="formState.failureDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.qualityFailure.failuredate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.model')"
                name="model"
              >
                <a-input
                  v-model:value="formState.model"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.model') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.lot')"
                name="lot"
              >
                <a-input
                  v-model:value="formState.lot"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.lot') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.qualityproblemsresponse')"
                name="qualityProblemsResponse"
              >
                <a-input
                  v-model:value="formState.qualityProblemsResponse"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.qualityproblemsresponse') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.reworkduetodefects')"
                name="reworkDueToDefects"
              >
                <a-input
                  v-model:value="formState.reworkDueToDefects"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.reworkduetodefects') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.qualityFailure.needrework')"
                name="needRework"
              >
                <a-input
                  v-model:value="formState.needRework"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.needrework') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.totaltimeminutes')"
                name="totalTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.totalTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.totaltimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.totalcost')"
                name="totalCost"
              >
                <a-input-number
                  v-model:value="formState.totalCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.totalcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailure.costcurrency')"
                name="costCurrency"
              >
                <a-input
                  v-model:value="formState.costCurrency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.costcurrency') })"
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
      <!-- 子表：qualityFailureMeeting -->
      <a-tab-pane
        key="child-meetingItems"
        :tab="t('entity.qualityFailureMeeting._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityFailureMeetingRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityFailureMeeting._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityFailureMeetingFormColumns"
          :data-source="childQualityFailureMeetingRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'directManpowerCostPerMinute'">
              <a-input-number
                v-model:value="record.directManpowerCostPerMinute"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.directmanpowercostperminute') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'indirectManpowerCostPerMinute'">
              <a-input-number
                v-model:value="record.indirectManpowerCostPerMinute"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.indirectmanpowercostperminute') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'meetingInvestigationContent'">
              <a-textarea
                v-model:value="record.meetingInvestigationContent"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityFailureMeeting.meetinginvestigationcontent') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'meetingInvestigationCost'">
              <a-input-number
                v-model:value="record.meetingInvestigationCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.meetinginvestigationcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'meetingTimeMinutes'">
              <a-input-number
                v-model:value="record.meetingTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.meetingtimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'directParticipantCount'">
              <a-input-number
                v-model:value="record.directParticipantCount"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.directparticipantcount') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'indirectParticipantCount'">
              <a-input-number
                v-model:value="record.indirectParticipantCount"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.indirectparticipantcount') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityFailureMeetingRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：qualityFailureAssyRework -->
      <a-tab-pane
        key="child-assyReworkItems"
        :tab="t('entity.qualityFailureAssyRework._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityFailureAssyReworkRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityFailureAssyRework._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityFailureAssyReworkFormColumns"
          :data-source="childQualityFailureAssyReworkRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureAssyRework.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'assyDefectParts'">
              <a-input
                v-model:value="record.assyDefectParts"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureAssyRework.assydefectparts') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'assyReworkCost'">
              <a-input-number
                v-model:value="record.assyReworkCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureAssyRework.assyreworkcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'assyReworkTimeMinutes'">
              <a-input-number
                v-model:value="record.assyReworkTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureAssyRework.assyreworktimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'assyReinspectionTimeMinutes'">
              <a-input-number
                v-model:value="record.assyReinspectionTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureAssyRework.assyreinspectiontimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'assyTravelCost'">
              <a-input-number
                v-model:value="record.assyTravelCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureAssyRework.assytravelcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'assyWarehouseCost'">
              <a-input-number
                v-model:value="record.assyWarehouseCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureAssyRework.assywarehousecost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'assyOtherExpenses'">
              <a-input-number
                v-model:value="record.assyOtherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureAssyRework.assyotherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityFailureAssyReworkRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：qualityFailurePcbaRework -->
      <a-tab-pane
        key="child-pcbaReworkItems"
        :tab="t('entity.qualityFailurePcbaRework._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQualityFailurePcbaReworkRow">
            {{ t('common.page.button.create') }}{{ t('entity.qualityFailurePcbaRework._self') }}
          </a-button>
        </div>
        <a-table
          :columns="qualityFailurePcbaReworkFormColumns"
          :data-source="childQualityFailurePcbaReworkRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailurePcbaRework.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'pcbaDefectParts'">
              <a-input
                v-model:value="record.pcbaDefectParts"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailurePcbaRework.pcbadefectparts') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'pcbaReworkCost'">
              <a-input-number
                v-model:value="record.pcbaReworkCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailurePcbaRework.pcbareworkcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'pcbaReworkTimeMinutes'">
              <a-input-number
                v-model:value="record.pcbaReworkTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailurePcbaRework.pcbareworktimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'pcbaReinspectionTimeMinutes'">
              <a-input-number
                v-model:value="record.pcbaReinspectionTimeMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailurePcbaRework.pcbareinspectiontimeminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'pcbaTravelCost'">
              <a-input-number
                v-model:value="record.pcbaTravelCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailurePcbaRework.pcbatravelcost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'pcbaWarehouseCost'">
              <a-input-number
                v-model:value="record.pcbaWarehouseCost"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailurePcbaRework.pcbawarehousecost') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'pcbaOtherExpenses'">
              <a-input-number
                v-model:value="record.pcbaOtherExpenses"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailurePcbaRework.pcbaotherexpenses') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQualityFailurePcbaReworkRow(index)">
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
 * 品质问题应对主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-failure/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityFailureCreate, QualityFailureMeetingCreate, QualityFailureMeeting, QualityFailureAssyReworkCreate, QualityFailureAssyRework, QualityFailurePcbaReworkCreate, QualityFailurePcbaRework } from '@/types/logistics/quality/cost/quality-failure'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","qualityFailureCode","failureDate","model","lot","qualityProblemsResponse","reworkDueToDefects","needRework","totalTimeMinutes","totalCost","costCurrency","extFieldJson","remark"]

/** qualityFailureMeeting 子表行（表单 Tab 内嵌） */
const childQualityFailureMeetingRows = ref<Record<string, unknown>[]>([])
/** qualityFailureAssyRework 子表行（表单 Tab 内嵌） */
const childQualityFailureAssyReworkRows = ref<Record<string, unknown>[]>([])
/** qualityFailurePcbaRework 子表行（表单 Tab 内嵌） */
const childQualityFailurePcbaReworkRows = ref<Record<string, unknown>[]>([])

/** 子表 qualityFailureMeeting 表单列定义 */
const qualityFailureMeetingFormColumns = computed(() => [
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
    title: t('entity.qualityFailureMeeting.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityFailureMeeting.directmanpowercostperminute'),
    dataIndex: 'directManpowerCostPerMinute',
    key: 'directManpowerCostPerMinute',
    width: 140,
  },
  {
    title: t('entity.qualityFailureMeeting.indirectmanpowercostperminute'),
    dataIndex: 'indirectManpowerCostPerMinute',
    key: 'indirectManpowerCostPerMinute',
    width: 140,
  },
  {
    title: t('entity.qualityFailureMeeting.meetinginvestigationcontent'),
    dataIndex: 'meetingInvestigationContent',
    key: 'meetingInvestigationContent',
    width: 140,
  },
  {
    title: t('entity.qualityFailureMeeting.meetinginvestigationcost'),
    dataIndex: 'meetingInvestigationCost',
    key: 'meetingInvestigationCost',
    width: 140,
  },
  {
    title: t('entity.qualityFailureMeeting.meetingtimeminutes'),
    dataIndex: 'meetingTimeMinutes',
    key: 'meetingTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityFailureMeeting.directparticipantcount'),
    dataIndex: 'directParticipantCount',
    key: 'directParticipantCount',
    width: 140,
  },
  {
    title: t('entity.qualityFailureMeeting.indirectparticipantcount'),
    dataIndex: 'indirectParticipantCount',
    key: 'indirectParticipantCount',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 qualityFailureAssyRework 表单列定义 */
const qualityFailureAssyReworkFormColumns = computed(() => [
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
    title: t('entity.qualityFailureAssyRework.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityFailureAssyRework.assydefectparts'),
    dataIndex: 'assyDefectParts',
    key: 'assyDefectParts',
    width: 140,
  },
  {
    title: t('entity.qualityFailureAssyRework.assyreworkcost'),
    dataIndex: 'assyReworkCost',
    key: 'assyReworkCost',
    width: 140,
  },
  {
    title: t('entity.qualityFailureAssyRework.assyreworktimeminutes'),
    dataIndex: 'assyReworkTimeMinutes',
    key: 'assyReworkTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityFailureAssyRework.assyreinspectiontimeminutes'),
    dataIndex: 'assyReinspectionTimeMinutes',
    key: 'assyReinspectionTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityFailureAssyRework.assytravelcost'),
    dataIndex: 'assyTravelCost',
    key: 'assyTravelCost',
    width: 140,
  },
  {
    title: t('entity.qualityFailureAssyRework.assywarehousecost'),
    dataIndex: 'assyWarehouseCost',
    key: 'assyWarehouseCost',
    width: 140,
  },
  {
    title: t('entity.qualityFailureAssyRework.assyotherexpenses'),
    dataIndex: 'assyOtherExpenses',
    key: 'assyOtherExpenses',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 qualityFailurePcbaRework 表单列定义 */
const qualityFailurePcbaReworkFormColumns = computed(() => [
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
    title: t('entity.qualityFailurePcbaRework.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbadefectparts'),
    dataIndex: 'pcbaDefectParts',
    key: 'pcbaDefectParts',
    width: 140,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbareworkcost'),
    dataIndex: 'pcbaReworkCost',
    key: 'pcbaReworkCost',
    width: 140,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbareworktimeminutes'),
    dataIndex: 'pcbaReworkTimeMinutes',
    key: 'pcbaReworkTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbareinspectiontimeminutes'),
    dataIndex: 'pcbaReinspectionTimeMinutes',
    key: 'pcbaReinspectionTimeMinutes',
    width: 140,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbatravelcost'),
    dataIndex: 'pcbaTravelCost',
    key: 'pcbaTravelCost',
    width: 140,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbawarehousecost'),
    dataIndex: 'pcbaWarehouseCost',
    key: 'pcbaWarehouseCost',
    width: 140,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbaotherexpenses'),
    dataIndex: 'pcbaOtherExpenses',
    key: 'pcbaOtherExpenses',
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
function syncChildRowsFromFormData(val: Partial<QualityFailureCreate & { qualityFailureId?: string }> | null | undefined) {
  childQualityFailureMeetingRows.value = ((val as any)?.meetingItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityFailureMeetingId ?? `new-${index}`,
  }))
  childQualityFailureAssyReworkRows.value = ((val as any)?.assyReworkItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityFailureAssyReworkId ?? `new-${index}`,
  }))
  childQualityFailurePcbaReworkRows.value = ((val as any)?.pcbaReworkItems ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.qualityFailurePcbaReworkId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 qualityFailureMeeting 行 */
function handleAddQualityFailureMeetingRow() {
  childQualityFailureMeetingRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      directManpowerCostPerMinute: 0,
      indirectManpowerCostPerMinute: 0,
      meetingInvestigationContent: '',
      meetingInvestigationCost: 0,
      meetingTimeMinutes: 0,
      directParticipantCount: 0,
      indirectParticipantCount: 0,
  })
}

/** 表单 Tab 内删除 qualityFailureMeeting 行 */
function handleRemoveQualityFailureMeetingRow(index: number) {
  childQualityFailureMeetingRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 qualityFailureAssyRework 行 */
function handleAddQualityFailureAssyReworkRow() {
  childQualityFailureAssyReworkRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      assyDefectParts: '',
      assyReworkCost: 0,
      assyReworkTimeMinutes: 0,
      assyReinspectionTimeMinutes: 0,
      assyTravelCost: 0,
      assyWarehouseCost: 0,
      assyOtherExpenses: 0,
  })
}

/** 表单 Tab 内删除 qualityFailureAssyRework 行 */
function handleRemoveQualityFailureAssyReworkRow(index: number) {
  childQualityFailureAssyReworkRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 qualityFailurePcbaRework 行 */
function handleAddQualityFailurePcbaReworkRow() {
  childQualityFailurePcbaReworkRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      pcbaDefectParts: '',
      pcbaReworkCost: 0,
      pcbaReworkTimeMinutes: 0,
      pcbaReinspectionTimeMinutes: 0,
      pcbaTravelCost: 0,
      pcbaWarehouseCost: 0,
      pcbaOtherExpenses: 0,
  })
}

/** 表单 Tab 内删除 qualityFailurePcbaRework 行 */
function handleRemoveQualityFailurePcbaReworkRow(index: number) {
  childQualityFailurePcbaReworkRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    meetingItems: childQualityFailureMeetingRows.value.map(({ __rowKey, ...rest }) => rest),
    assyReworkItems: childQualityFailureAssyReworkRows.value.map(({ __rowKey, ...rest }) => rest),
    pcbaReworkItems: childQualityFailurePcbaReworkRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityFailureCreate & { qualityFailureId?: string }> | null
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
    delete (next as any).meetingItems
    delete (next as any).assyReworkItems
    delete (next as any).pcbaReworkItems
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
    const isCreate = !props.formData?.qualityFailureId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.plantcode') }),
      trigger: 'blur'
    }
  ],
  qualityFailureCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.code') }),
      trigger: 'blur'
    }
  ],
  failureDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailure.failuredate') }),
      trigger: 'change'
    }
  ],
  model: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.model') }),
      trigger: 'blur'
    }
  ],
  lot: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.lot') }),
      trigger: 'blur'
    }
  ],
  totalTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailure.totaltimeminutes') }),
      trigger: 'change'
    }
  ],
  totalCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailure.totalcost') }),
      trigger: 'change'
    }
  ],
  costCurrency: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.costcurrency') }),
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
  childQualityFailureMeetingRows.value = []
  childQualityFailureAssyReworkRows.value = []
  childQualityFailurePcbaReworkRows.value = []
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
