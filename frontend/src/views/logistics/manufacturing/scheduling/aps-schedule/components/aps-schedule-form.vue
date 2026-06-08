<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/scheduling/aps-schedule/components -->
<!-- 文件名称：aps-schedule-form.vue -->
<!-- 功能描述：APS排程主表维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="aps-schedule-form-tabs"
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
                :label="t('entity.apsSchedule.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulecode')"
                name="scheduleCode"
              >
                <a-input
                  v-model:value="formState.scheduleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulename')"
                name="scheduleName"
              >
                <a-input
                  v-model:value="formState.scheduleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.scheduletype')"
                name="scheduleType"
              >
                <a-input-number
                  v-model:value="formState.scheduleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.scheduletype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.plandate')"
                name="planDate"
              >
                <a-date-picker
                  v-model:value="formState.planDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.plandate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.planstarttime')"
                name="planStartTime"
              >
                <a-input
                  v-model:value="formState.planStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.planstarttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.planendtime')"
                name="planEndTime"
              >
                <a-input
                  v-model:value="formState.planEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.planendtime') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.plancycle')"
                name="planCycle"
              >
                <a-input-number
                  v-model:value="formState.planCycle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plancycle') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.workshopcode')"
                name="workshopCode"
              >
                <a-input
                  v-model:value="formState.workshopCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.workshopcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.workshopname')"
                name="workshopName"
              >
                <a-input
                  v-model:value="formState.workshopName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.workshopname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.productionlinecode')"
                name="productionLineCode"
              >
                <a-input
                  v-model:value="formState.productionLineCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.productionlinecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.productionlinename')"
                name="productionLineName"
              >
                <a-input
                  v-model:value="formState.productionLineName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.productionlinename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulestrategy')"
                name="scheduleStrategy"
              >
                <a-input-number
                  v-model:value="formState.scheduleStrategy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulestrategy') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulealgorithm')"
                name="scheduleAlgorithm"
              >
                <a-input-number
                  v-model:value="formState.scheduleAlgorithm"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulealgorithm') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.optimizationobjective')"
                name="optimizationObjective"
              >
                <a-input-number
                  v-model:value="formState.optimizationObjective"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.optimizationobjective') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulestatus')"
                name="scheduleStatus"
              >
                <a-input-number
                  v-model:value="formState.scheduleStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulestatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.plannerid')"
                name="plannerId"
              >
                <a-input
                  v-model:value="formState.plannerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plannerid') })"
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
                :label="t('entity.apsSchedule.plannername')"
                name="plannerName"
              >
                <a-input
                  v-model:value="formState.plannerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plannername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.publishtime')"
                name="publishTime"
              >
                <a-input
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.publishtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.publishuserid')"
                name="publishUserId"
              >
                <a-input
                  v-model:value="formState.publishUserId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.publishuserid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.publishusername')"
                name="publishUserName"
              >
                <a-input
                  v-model:value="formState.publishUserName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.publishusername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.apsSchedule.scheduledescription')"
                name="scheduleDescription"
              >
                <a-textarea
                  v-model:value="formState.scheduleDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.apsSchedule.scheduledescription') })"
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
      <!-- 子表：apsScheduleItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.apsScheduleItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddApsScheduleItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.apsScheduleItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="apsScheduleItemFormColumns"
          :data-source="childApsScheduleItemRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'workOrderCode'">
              <a-input
                v-model:value="record.workOrderCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.workordercode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'productCode'">
              <a-input
                v-model:value="record.productCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.productcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'productName'">
              <a-input
                v-model:value="record.productName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.productname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'workCenterCode'">
              <a-input
                v-model:value="record.workCenterCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.workcentercode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'workCenterName'">
              <a-input
                v-model:value="record.workCenterName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.workcentername') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'processCode'">
              <a-input
                v-model:value="record.processCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'processName'">
              <a-input
                v-model:value="record.processName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveApsScheduleItemRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：apsScheduleChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.apsScheduleChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddApsScheduleChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.apsScheduleChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="apsScheduleChangeLogFormColumns"
          :data-source="childApsScheduleChangeLogRows"
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
            <template v-else-if="column.key === 'changeFields'">
              <a-input
                v-model:value="record.changeFields"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeType'">
              <a-input-number
                v-model:value="record.changeType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleChangeLog.changetype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleChangeLog.changereason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeBy'">
              <a-input
                v-model:value="record.changeBy"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleChangeLog.changeby') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeTime'">
              <a-input
                v-model:value="record.changeTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleChangeLog.changetime') })"
                size="small"
                allow-clear
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
              <a-button type="link" danger size="small" @click="handleRemoveApsScheduleChangeLogRow(index)">
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
 * APS排程主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/scheduling/aps-schedule/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ApsScheduleCreate, ApsScheduleItemCreate, ApsScheduleItem, ApsScheduleChangeLogCreate, ApsScheduleChangeLog } from '@/types/logistics/manufacturing/scheduling/aps-schedule'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","scheduleCode","scheduleName","scheduleType","planDate","planStartTime","planEndTime","planCycle","workshopCode","workshopName","productionLineCode","productionLineName","scheduleStrategy","scheduleAlgorithm","optimizationObjective","scheduleStatus","plannerId","plannerName","publishTime","publishUserId","publishUserName","scheduleDescription","extFieldJson","remark"]

/** apsScheduleItem 子表行（表单 Tab 内嵌） */
const childApsScheduleItemRows = ref<Record<string, unknown>[]>([])
/** apsScheduleChangeLog 子表行（表单 Tab 内嵌） */
const childApsScheduleChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 apsScheduleItem 表单列定义 */
const apsScheduleItemFormColumns = computed(() => [
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
    title: t('entity.apsScheduleItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.apsScheduleItem.workordercode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    width: 140,
  },
  {
    title: t('entity.apsScheduleItem.productcode'),
    dataIndex: 'productCode',
    key: 'productCode',
    width: 140,
  },
  {
    title: t('entity.apsScheduleItem.productname'),
    dataIndex: 'productName',
    key: 'productName',
    width: 140,
  },
  {
    title: t('entity.apsScheduleItem.workcentercode'),
    dataIndex: 'workCenterCode',
    key: 'workCenterCode',
    width: 140,
  },
  {
    title: t('entity.apsScheduleItem.workcentername'),
    dataIndex: 'workCenterName',
    key: 'workCenterName',
    width: 140,
  },
  {
    title: t('entity.apsScheduleItem.processcode'),
    dataIndex: 'processCode',
    key: 'processCode',
    width: 140,
  },
  {
    title: t('entity.apsScheduleItem.processname'),
    dataIndex: 'processName',
    key: 'processName',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 apsScheduleChangeLog 表单列定义 */
const apsScheduleChangeLogFormColumns = computed(() => [
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
    title: t('entity.apsScheduleChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.apsScheduleChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 140,
  },
  {
    title: t('entity.apsScheduleChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 140,
  },
  {
    title: t('entity.apsScheduleChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 140,
  },
  {
    title: t('entity.apsScheduleChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
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
function syncChildRowsFromFormData(val: Partial<ApsScheduleCreate & { apsScheduleId?: string }> | null | undefined) {
  childApsScheduleItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.apsScheduleItemId ?? `new-${index}`,
  }))
  childApsScheduleChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.apsScheduleChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 apsScheduleItem 行 */
function handleAddApsScheduleItemRow() {
  childApsScheduleItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      workOrderCode: '',
      productCode: '',
      productName: '',
      workCenterCode: '',
      workCenterName: '',
      processCode: '',
      processName: '',
  })
}

/** 表单 Tab 内删除 apsScheduleItem 行 */
function handleRemoveApsScheduleItemRow(index: number) {
  childApsScheduleItemRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 apsScheduleChangeLog 行 */
function handleAddApsScheduleChangeLogRow() {
  childApsScheduleChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      changeFields: '',
      changeType: 0,
      changeReason: '',
      changeBy: '',
      changeTime: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 apsScheduleChangeLog 行 */
function handleRemoveApsScheduleChangeLogRow(index: number) {
  childApsScheduleChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childApsScheduleItemRows.value.map(({ __rowKey, ...rest }) => rest),
    changeLogs: childApsScheduleChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ApsScheduleCreate & { apsScheduleId?: string }> | null
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
    delete (next as any).changeLogs
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
    const isCreate = !props.formData?.apsScheduleId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plantcode') }),
      trigger: 'blur'
    }
  ],
  scheduleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulecode') }),
      trigger: 'blur'
    }
  ],
  scheduleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulename') }),
      trigger: 'blur'
    }
  ],
  scheduleType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.scheduletype') }),
      trigger: 'change'
    }
  ],
  planDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.plandate') }),
      trigger: 'change'
    }
  ],
  planStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.planstarttime') }),
      trigger: 'blur'
    }
  ],
  planEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.planendtime') }),
      trigger: 'blur'
    }
  ],
  planCycle: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.plancycle') }),
      trigger: 'change'
    }
  ],
  scheduleStrategy: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.schedulestrategy') }),
      trigger: 'change'
    }
  ],
  scheduleAlgorithm: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.schedulealgorithm') }),
      trigger: 'change'
    }
  ],
  optimizationObjective: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.optimizationobjective') }),
      trigger: 'change'
    }
  ],
  scheduleStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.schedulestatus') }),
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
  childApsScheduleItemRows.value = []
  childApsScheduleChangeLogRows.value = []
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
