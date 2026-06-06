<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/plant/components -->
<!-- 文件名称：plant-form.vue -->
<!-- 功能描述：Takt工厂实体 代表租户下的独立工厂维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="plant-form-tabs"
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
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.code')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.name')"
                name="plantName"
              >
                <a-input
                  v-model:value="formState.plantName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.shortname')"
                name="plantShortName"
              >
                <a-input
                  v-model:value="formState.plantShortName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.shortname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.codealias')"
                name="codeAlias"
              >
                <a-input
                  v-model:value="formState.codeAlias"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.codealias') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.defaultculture')"
                name="defaultCulture"
              >
                <a-input
                  v-model:value="formState.defaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.defaultculture') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.type')"
                name="plantType"
              >
                <a-input-number
                  v-model:value="formState.plantType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.relatedcompany')"
                name="relatedCompany"
              >
                <a-input
                  v-model:value="formState.relatedCompany"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.relatedcompany') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.enterprisenature')"
                name="enterpriseNature"
              >
                <a-input-number
                  v-model:value="formState.enterpriseNature"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.enterprisenature') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.industryattribute')"
                name="industryAttribute"
              >
                <a-input-number
                  v-model:value="formState.industryAttribute"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.industryattribute') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.enterprisescale')"
                name="enterpriseScale"
              >
                <a-input-number
                  v-model:value="formState.enterpriseScale"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.enterprisescale') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.businessscope')"
                name="businessScope"
              >
                <a-textarea
                  v-model:value="formState.businessScope"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.businessscope') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.registrationaddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.registrationaddress1') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.registrationaddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.registrationaddress2') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.registrationaddress3')"
                name="registrationAddress3"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress3"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.registrationaddress3') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.registrationregion')"
                name="registrationRegion"
              >
                <a-input
                  v-model:value="formState.registrationRegion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.registrationregion') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.registrationprovince')"
                name="registrationProvince"
              >
                <a-input
                  v-model:value="formState.registrationProvince"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.registrationprovince') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.registrationcity')"
                name="registrationCity"
              >
                <a-input
                  v-model:value="formState.registrationCity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.registrationcity') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.businessregion')"
                name="businessRegion"
              >
                <a-input
                  v-model:value="formState.businessRegion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.businessregion') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.businessprovince')"
                name="businessProvince"
              >
                <a-input
                  v-model:value="formState.businessProvince"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.businessprovince') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.businesscity')"
                name="businessCity"
              >
                <a-input
                  v-model:value="formState.businessCity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.businesscity') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.businessaddress1')"
                name="businessAddress1"
              >
                <a-textarea
                  v-model:value="formState.businessAddress1"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.businessaddress1') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.businessaddress2')"
                name="businessAddress2"
              >
                <a-textarea
                  v-model:value="formState.businessAddress2"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.businessaddress2') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.businessaddress3')"
                name="businessAddress3"
              >
                <a-textarea
                  v-model:value="formState.businessAddress3"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.businessaddress3') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.address1')"
                name="plantAddress1"
              >
                <a-textarea
                  v-model:value="formState.plantAddress1"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.address1') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.address2')"
                name="plantAddress2"
              >
                <a-textarea
                  v-model:value="formState.plantAddress2"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.address2') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.plant.address3')"
                name="plantAddress3"
              >
                <a-textarea
                  v-model:value="formState.plantAddress3"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.plant.address3') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.phone')"
                name="plantPhone"
              >
                <a-input
                  v-model:value="formState.plantPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.phone') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.email')"
                name="plantEmail"
              >
                <a-input
                  v-model:value="formState.plantEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.email') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.fax')"
                name="plantFax"
              >
                <a-input
                  v-model:value="formState.plantFax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.fax') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.plant.website')"
                name="plantWebsite"
              >
                <a-input
                  v-model:value="formState.plantWebsite"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.website') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.unifiedsocialcreditcode')"
                name="unifiedSocialCreditCode"
              >
                <a-input
                  v-model:value="formState.unifiedSocialCreditCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.unifiedsocialcreditcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.taxregistrationnumber')"
                name="taxRegistrationNumber"
              >
                <a-input
                  v-model:value="formState.taxRegistrationNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.taxregistrationnumber') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.legalrepresentative')"
                name="legalRepresentative"
              >
                <a-input
                  v-model:value="formState.legalRepresentative"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.legalrepresentative') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.manager')"
                name="plantManager"
              >
                <a-input
                  v-model:value="formState.plantManager"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.manager') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.registeredcapital')"
                name="registeredCapital"
              >
                <a-input-number
                  v-model:value="formState.registeredCapital"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.registeredcapital') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.establishmentdate')"
                name="establishmentDate"
              >
                <a-date-picker
                  v-model:value="formState.establishmentDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.plant.establishmentdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.closingdate')"
                name="closingDate"
              >
                <a-date-picker
                  v-model:value="formState.closingDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.plant.closingdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.existence')"
                name="plantExistence"
              >
                <a-input-number
                  v-model:value="formState.plantExistence"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.existence') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.status')"
                name="plantStatus"
              >
                <a-input-number
                  v-model:value="formState.plantStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.status') })"
                  size="small"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.plant.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.plant.sortorder') })"
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

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt工厂实体 代表租户下的独立工厂维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/materials/plant/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PlantCreate } from '@/types/logistics/materials/plant'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

const { t } = useI18n()

const tenantStore = useTenantStore()
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
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","plantCode","plantName","plantShortName","codeAlias","defaultCulture","plantType","relatedCompany","enterpriseNature","industryAttribute","enterpriseScale","businessScope","registrationAddress1","registrationAddress2","registrationAddress3","registrationRegion","registrationProvince","registrationCity","businessRegion","businessProvince","businessCity","businessAddress1","businessAddress2","businessAddress3","plantAddress1","plantAddress2","plantAddress3","plantPhone","plantEmail","plantFax","plantWebsite","unifiedSocialCreditCode","taxRegistrationNumber","legalRepresentative","plantManager","registeredCapital","establishmentDate","closingDate","plantExistence","plantStatus","sortOrder","extFieldJson","remark"]


interface Props {
  formData?: Partial<PlantCreate & { plantId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

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

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.plantId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.code') }),
      trigger: 'blur'
    }
  ],
  plantName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.name') }),
      trigger: 'blur'
    }
  ],
  plantShortName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.shortname') }),
      trigger: 'blur'
    }
  ],
  codeAlias: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.codealias') }),
      trigger: 'blur'
    }
  ],
  defaultCulture: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.defaultculture') }),
      trigger: 'blur'
    }
  ],
  plantType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.type') }),
      trigger: 'change'
    }
  ],
  relatedCompany: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.relatedcompany') }),
      trigger: 'blur'
    }
  ],
  enterpriseNature: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.enterprisenature') }),
      trigger: 'change'
    }
  ],
  industryAttribute: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.industryattribute') }),
      trigger: 'change'
    }
  ],
  enterpriseScale: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.enterprisescale') }),
      trigger: 'change'
    }
  ],
  businessScope: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.businessscope') }),
      trigger: 'blur'
    }
  ],
  registrationAddress1: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.registrationaddress1') }),
      trigger: 'blur'
    }
  ],
  registrationRegion: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.registrationregion') }),
      trigger: 'blur'
    }
  ],
  registrationProvince: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.registrationprovince') }),
      trigger: 'blur'
    }
  ],
  registrationCity: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.registrationcity') }),
      trigger: 'blur'
    }
  ],
  businessRegion: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.businessregion') }),
      trigger: 'blur'
    }
  ],
  businessProvince: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.businessprovince') }),
      trigger: 'blur'
    }
  ],
  businessCity: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.businesscity') }),
      trigger: 'blur'
    }
  ],
  businessAddress1: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.businessaddress1') }),
      trigger: 'blur'
    }
  ],
  plantPhone: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.phone') }),
      trigger: 'blur'
    }
  ],
  plantEmail: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.email') }),
      trigger: 'blur'
    }
  ],
  plantFax: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.fax') }),
      trigger: 'blur'
    }
  ],
  plantWebsite: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.website') }),
      trigger: 'blur'
    }
  ],
  unifiedSocialCreditCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.unifiedsocialcreditcode') }),
      trigger: 'blur'
    }
  ],
  taxRegistrationNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.taxregistrationnumber') }),
      trigger: 'blur'
    }
  ],
  legalRepresentative: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.legalrepresentative') }),
      trigger: 'blur'
    }
  ],
  plantManager: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.plant.manager') }),
      trigger: 'blur'
    }
  ],
  registeredCapital: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.registeredcapital') }),
      trigger: 'change'
    }
  ],
  establishmentDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.establishmentdate') }),
      trigger: 'change'
    }
  ],
  plantExistence: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.existence') }),
      trigger: 'change'
    }
  ],
  plantStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.status') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.plant.sortorder') }),
      trigger: 'change'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

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
