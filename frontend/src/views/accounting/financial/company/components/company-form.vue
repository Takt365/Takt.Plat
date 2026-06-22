<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/company/components -->
<!-- 文件名称：company-form.vue -->
<!-- 功能描述：公司实体 代表租户下的独立公司/工厂维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="company-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/5)'"
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
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.name')"
                name="companyName"
              >
                <a-input
                  v-model:value="formState.companyName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.shortname')"
                name="companyShortName"
              >
                <a-input
                  v-model:value="formState.companyShortName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.shortname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.type')"
                name="companyType"
              >
                <a-input-number
                  v-model:value="formState.companyType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.enterprisenature')"
                name="enterpriseNature"
              >
                <a-input-number
                  v-model:value="formState.enterpriseNature"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.enterprisenature') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.industryattribute')"
                name="industryAttribute"
              >
                <a-input-number
                  v-model:value="formState.industryAttribute"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.industryattribute') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.enterprisescale')"
                name="enterpriseScale"
              >
                <a-input-number
                  v-model:value="formState.enterpriseScale"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.enterprisescale') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.company.businessscope')"
                name="businessScope"
              >
                <a-textarea
                  v-model:value="formState.businessScope"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.businessscope') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.company.registrationaddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.registrationaddress1') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.company.registrationaddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.registrationaddress2') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.company.registrationaddress3')"
                name="registrationAddress3"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress3"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.registrationaddress3') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.registrationregion')"
                name="registrationRegion"
              >
                <a-input
                  v-model:value="formState.registrationRegion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registrationregion') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.registrationprovince')"
                name="registrationProvince"
              >
                <a-input
                  v-model:value="formState.registrationProvince"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registrationprovince') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.registrationcity')"
                name="registrationCity"
              >
                <a-input
                  v-model:value="formState.registrationCity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registrationcity') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.businessregion')"
                name="businessRegion"
              >
                <a-input
                  v-model:value="formState.businessRegion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.businessregion') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.businessprovince')"
                name="businessProvince"
              >
                <a-input
                  v-model:value="formState.businessProvince"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.businessprovince') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.businesscity')"
                name="businessCity"
              >
                <a-input
                  v-model:value="formState.businessCity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.businesscity') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.company.businessaddress1')"
                name="businessAddress1"
              >
                <a-textarea
                  v-model:value="formState.businessAddress1"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.businessaddress1') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.company.businessaddress2')"
                name="businessAddress2"
              >
                <a-textarea
                  v-model:value="formState.businessAddress2"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.businessaddress2') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.company.businessaddress3')"
                name="businessAddress3"
              >
                <a-textarea
                  v-model:value="formState.businessAddress3"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.businessaddress3') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.phone')"
                name="companyPhone"
              >
                <a-input
                  v-model:value="formState.companyPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.phone') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.email')"
                name="companyEmail"
              >
                <a-input
                  v-model:value="formState.companyEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.email') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.fax')"
                name="companyFax"
              >
                <a-input
                  v-model:value="formState.companyFax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.fax') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.website')"
                name="companyWebsite"
              >
                <a-input
                  v-model:value="formState.companyWebsite"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.website') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.unifiedsocialcreditcode')"
                name="unifiedSocialCreditCode"
              >
                <a-input
                  v-model:value="formState.unifiedSocialCreditCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.unifiedsocialcreditcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.taxregistrationnumber')"
                name="taxRegistrationNumber"
              >
                <a-input
                  v-model:value="formState.taxRegistrationNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.taxregistrationnumber') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.legalrepresentative')"
                name="legalRepresentative"
              >
                <a-input
                  v-model:value="formState.legalRepresentative"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.legalrepresentative') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.manager')"
                name="companyManager"
              >
                <a-input
                  v-model:value="formState.companyManager"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.manager') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.registeredcapital')"
                name="registeredCapital"
              >
                <a-input-number
                  v-model:value="formState.registeredCapital"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registeredcapital') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.establishmentdate')"
                name="establishmentDate"
              >
                <a-date-picker
                  v-model:value="formState.establishmentDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.company.establishmentdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.closingdate')"
                name="closingDate"
              >
                <a-date-picker
                  v-model:value="formState.closingDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.company.closingdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.existence')"
                name="companyExistence"
              >
                <a-input-number
                  v-model:value="formState.companyExistence"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.existence') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.defaultculture')"
                name="defaultCulture"
              >
                <a-input
                  v-model:value="formState.defaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.defaultculture') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.codealias')"
                name="codeAlias"
              >
                <a-input
                  v-model:value="formState.codeAlias"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.codealias') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.status')"
                name="companyStatus"
              >
                <a-input-number
                  v-model:value="formState.companyStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.roleids')"
                name="roleIds"
              >
                <a-input
                  v-model:value="formState.roleIds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.roleids') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.company.userids')"
                name="userIds"
              >
                <a-input
                  v-model:value="formState.userIds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.userids') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.ExtField')"
                name="ExtField"
              >
                <a-input
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 公司实体 代表租户下的独立公司/工厂维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/company/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CompanyCreate } from '@/types/accounting/financial/company'
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
const formFields = ["tenantCode","companyName","companyShortName","companyType","enterpriseNature","industryAttribute","enterpriseScale","businessScope","registrationAddress1","registrationAddress2","registrationAddress3","registrationRegion","registrationProvince","registrationCity","businessRegion","businessProvince","businessCity","businessAddress1","businessAddress2","businessAddress3","companyPhone","companyEmail","companyFax","companyWebsite","unifiedSocialCreditCode","taxRegistrationNumber","legalRepresentative","companyManager","registeredCapital","establishmentDate","closingDate","companyExistence","relatedPlant","defaultCulture","codeAlias","companyStatus","sortOrder","roleIds","userIds","ExtField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CompanyCreate & { companyId?: string }> | null
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

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.companyId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  companyName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.name') }),
      trigger: 'blur'
    }
  ],
  companyShortName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.shortname') }),
      trigger: 'blur'
    }
  ],
  companyType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.type') }),
      trigger: 'change'
    }
  ],
  enterpriseNature: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.enterprisenature') }),
      trigger: 'change'
    }
  ],
  industryAttribute: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.industryattribute') }),
      trigger: 'change'
    }
  ],
  enterpriseScale: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.enterprisescale') }),
      trigger: 'change'
    }
  ],
  businessScope: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.businessscope') }),
      trigger: 'blur'
    }
  ],
  registrationAddress1: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.registrationaddress1') }),
      trigger: 'blur'
    }
  ],
  registrationRegion: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.registrationregion') }),
      trigger: 'blur'
    }
  ],
  registrationProvince: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.registrationprovince') }),
      trigger: 'blur'
    }
  ],
  registrationCity: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.registrationcity') }),
      trigger: 'blur'
    }
  ],
  businessRegion: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.businessregion') }),
      trigger: 'blur'
    }
  ],
  businessProvince: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.businessprovince') }),
      trigger: 'blur'
    }
  ],
  businessCity: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.businesscity') }),
      trigger: 'blur'
    }
  ],
  businessAddress1: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.businessaddress1') }),
      trigger: 'blur'
    }
  ],
  companyPhone: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.phone') }),
      trigger: 'blur'
    }
  ],
  companyEmail: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.email') }),
      trigger: 'blur'
    }
  ],
  companyFax: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.fax') }),
      trigger: 'blur'
    }
  ],
  companyWebsite: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.website') }),
      trigger: 'blur'
    }
  ],
  unifiedSocialCreditCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.unifiedsocialcreditcode') }),
      trigger: 'blur'
    }
  ],
  taxRegistrationNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.taxregistrationnumber') }),
      trigger: 'blur'
    }
  ],
  legalRepresentative: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.legalrepresentative') }),
      trigger: 'blur'
    }
  ],
  companyManager: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.manager') }),
      trigger: 'blur'
    }
  ],
  registeredCapital: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.registeredcapital') }),
      trigger: 'change'
    }
  ],
  establishmentDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.establishmentdate') }),
      trigger: 'change'
    }
  ],
  companyExistence: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.existence') }),
      trigger: 'change'
    }
  ],
  relatedPlant: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.relatedplant') }),
      trigger: 'blur'
    }
  ],
  defaultCulture: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.defaultculture') }),
      trigger: 'blur'
    }
  ],
  codeAlias: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.company.codealias') }),
      trigger: 'blur'
    }
  ],
  companyStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.status') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.company.sortorder') }),
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
  return { ...formState }
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

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
