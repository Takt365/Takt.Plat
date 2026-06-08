<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/talent/talent-staffing-requirement/components -->
<!-- 文件名称：talent-staffing-requirement-form.vue -->
<!-- 功能描述：用人需求维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="talent-staffing-requirement-form-tabs"
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
                :label="t('entity.talentStaffingRequirement.reqno')"
                name="reqNo"
              >
                <a-input
                  v-model:value="formState.reqNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.reqno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.postid')"
                name="postId"
              >
                <a-input
                  v-model:value="formState.postId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.postid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.jobgrade')"
                name="jobGrade"
              >
                <a-input
                  v-model:value="formState.jobGrade"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.jobgrade') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.requestqty')"
                name="requestQty"
              >
                <a-input-number
                  v-model:value="formState.requestQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.requestqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.headcounttype')"
                name="headcountType"
              >
                <a-input
                  v-model:value="formState.headcountType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.headcounttype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.reasoncode')"
                name="reasonCode"
              >
                <a-input
                  v-model:value="formState.reasonCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.reasoncode') })"
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
                :label="t('entity.talentStaffingRequirement.replaceemployeeid')"
                name="replaceEmployeeId"
              >
                <a-input
                  v-model:value="formState.replaceEmployeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.replaceemployeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.expectedonboarddate')"
                name="expectedOnboardDate"
              >
                <a-date-picker
                  v-model:value="formState.expectedOnboardDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentStaffingRequirement.expectedonboarddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.contracttype')"
                name="contractType"
              >
                <a-input
                  v-model:value="formState.contractType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.contracttype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.worklocation')"
                name="workLocation"
              >
                <a-input
                  v-model:value="formState.workLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.worklocation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.jobdesc')"
                name="jobDesc"
              >
                <a-input
                  v-model:value="formState.jobDesc"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.jobdesc') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.qualification')"
                name="qualification"
              >
                <a-input
                  v-model:value="formState.qualification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.qualification') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentStaffingRequirement.budgetyear')"
                name="budgetYear"
              >
                <a-input
                  v-model:value="formState.budgetYear"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.budgetyear') })"
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
      <!-- 子表：talentRecruitmentPlan -->
      <a-tab-pane
        key="child-talentRecruitmentPlans"
        :tab="t('entity.talentRecruitmentPlan._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddTalentRecruitmentPlanRow">
            {{ t('common.page.button.create') }}{{ t('entity.talentRecruitmentPlan._self') }}
          </a-button>
        </div>
        <a-table
          :columns="talentRecruitmentPlanFormColumns"
          :data-source="childTalentRecruitmentPlanRows"
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
            <template v-else-if="column.key === 'staffingRequirementId'">
              <a-input
                v-model:value="record.staffingRequirementId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentRecruitmentPlan.staffingrequirementid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'planNo'">
              <a-input
                v-model:value="record.planNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentRecruitmentPlan.planno') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'planDate'">
              <a-date-picker
                v-model:value="record.planDate"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentRecruitmentPlan.plandate') })"
                value-format="YYYY-MM-DD"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'planStartDate'">
              <a-date-picker
                v-model:value="record.planStartDate"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentRecruitmentPlan.planstartdate') })"
                value-format="YYYY-MM-DD"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'planEndDate'">
              <a-date-picker
                v-model:value="record.planEndDate"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentRecruitmentPlan.planenddate') })"
                value-format="YYYY-MM-DD"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'planHeadcount'">
              <a-input-number
                v-model:value="record.planHeadcount"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentRecruitmentPlan.planheadcount') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'reason'">
              <a-input
                v-model:value="record.reason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentRecruitmentPlan.reason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'talentJobPostings'">
              <a-input
                v-model:value="record.talentJobPostings"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentRecruitmentPlan.talentjobpostings') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveTalentRecruitmentPlanRow(index)">
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
 * 用人需求维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/talent/talent-staffing-requirement/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TalentStaffingRequirementCreate, TalentRecruitmentPlanCreate, TalentRecruitmentPlan } from '@/types/human-resource/talent/talent-staffing-requirement'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","reqNo","deptId","postId","jobGrade","requestQty","headcountType","reasonCode","replaceEmployeeId","expectedOnboardDate","contractType","workLocation","jobDesc","qualification","budgetYear","extFieldJson","remark"]

/** talentRecruitmentPlan 子表行（表单 Tab 内嵌） */
const childTalentRecruitmentPlanRows = ref<Record<string, unknown>[]>([])

/** 子表 talentRecruitmentPlan 表单列定义 */
const talentRecruitmentPlanFormColumns = computed(() => [
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
    title: t('entity.talentRecruitmentPlan.staffingrequirementid'),
    dataIndex: 'staffingRequirementId',
    key: 'staffingRequirementId',
    width: 140,
  },
  {
    title: t('entity.talentRecruitmentPlan.planno'),
    dataIndex: 'planNo',
    key: 'planNo',
    width: 140,
  },
  {
    title: t('entity.talentRecruitmentPlan.plandate'),
    dataIndex: 'planDate',
    key: 'planDate',
    width: 140,
  },
  {
    title: t('entity.talentRecruitmentPlan.planstartdate'),
    dataIndex: 'planStartDate',
    key: 'planStartDate',
    width: 140,
  },
  {
    title: t('entity.talentRecruitmentPlan.planenddate'),
    dataIndex: 'planEndDate',
    key: 'planEndDate',
    width: 140,
  },
  {
    title: t('entity.talentRecruitmentPlan.planheadcount'),
    dataIndex: 'planHeadcount',
    key: 'planHeadcount',
    width: 140,
  },
  {
    title: t('entity.talentRecruitmentPlan.reason'),
    dataIndex: 'reason',
    key: 'reason',
    width: 140,
  },
  {
    title: t('entity.talentRecruitmentPlan.talentjobpostings'),
    dataIndex: 'talentJobPostings',
    key: 'talentJobPostings',
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
function syncChildRowsFromFormData(val: Partial<TalentStaffingRequirementCreate & { talentStaffingRequirementId?: string }> | null | undefined) {
  childTalentRecruitmentPlanRows.value = ((val as any)?.talentRecruitmentPlans ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.talentRecruitmentPlanId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 talentRecruitmentPlan 行 */
function handleAddTalentRecruitmentPlanRow() {
  childTalentRecruitmentPlanRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      staffingRequirementId: '',
      planNo: '',
      planDate: '',
      planStartDate: '',
      planEndDate: '',
      planHeadcount: 0,
      reason: '',
      talentJobPostings: '',
  })
}

/** 表单 Tab 内删除 talentRecruitmentPlan 行 */
function handleRemoveTalentRecruitmentPlanRow(index: number) {
  childTalentRecruitmentPlanRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    talentRecruitmentPlans: childTalentRecruitmentPlanRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TalentStaffingRequirementCreate & { talentStaffingRequirementId?: string }> | null
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
    delete (next as any).talentRecruitmentPlans
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
    const isCreate = !props.formData?.talentStaffingRequirementId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  reqNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.reqno') }),
      trigger: 'blur'
    }
  ],
  deptId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.deptid') }),
      trigger: 'blur'
    }
  ],
  postId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.postid') }),
      trigger: 'blur'
    }
  ],
  requestQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.talentStaffingRequirement.requestqty') }),
      trigger: 'change'
    }
  ],
  headcountType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.headcounttype') }),
      trigger: 'blur'
    }
  ],
  reasonCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.reasoncode') }),
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
  childTalentRecruitmentPlanRows.value = []
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
