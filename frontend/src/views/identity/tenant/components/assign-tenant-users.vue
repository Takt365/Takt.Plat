<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：@/views/identity/tenant/components -->
<!-- 文件名称：assign-tenant-users.vue -->
<!-- 功能描述：分配租户用户弹窗；Transfer + getUserOptions / getTenantUserIds / assignTenantUsers。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.dialog.title.allocate', { entity: t('entity.user._self') })"
    :width="'33.333vw'"
    :confirm-loading="loading"
    :centered="true"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-form
      :label-col="{ span: 4 }"
      :wrapper-col="{ span: 20 }"
      layout="horizontal"
    >
      <a-form-item :label="t('entity.tenant._self')">
        <a-input
          :value="tenantInfo"
          disabled
        />
      </a-form-item>
      <a-row :gutter="24">
        <a-col :span="24">
          <a-form-item
            :label="t('entity.user._self')"
            :label-col="{ span: 24 }"
            :wrapper-col="{ span: 24 }"
          >
            <takt-transfer
              v-model:target-keys="targetKeys"
              :data-source="dataSource"
              :titles="[t('common.tip.transfer.unassigned'), t('common.tip.transfer.assigned')]"
              :loading="optionsLoading"
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>
  </a-modal>
</template>

<script setup lang="ts">
/**
 * 分配租户用户弹窗：用户 Transfer，提交 assignTenantUsers（userId 列表）。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { getUserOptions } from '@/api/identity/user'
import { getTenantUserIds, assignTenantUsers } from '@/api/identity/rbac'
import type { Tenant } from '@/types/identity/tenant'
import type { UserTenant } from '@/types/identity/user-tenant'
import type { TaktSelectOption } from '@/types/common'

/**
 * 从异常对象提取可展示消息
 * @param error 捕获的异常
 * @returns {string | undefined} 错误文案
 */
function getErrorMessage(error: unknown): string | undefined {
  if (error instanceof Error) return error.message
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const msg = (error as { message?: unknown }).message
    return typeof msg === 'string' ? msg : undefined
  }
  return undefined
}

/** 组件入参 */
interface Props {
  /** 是否显示对话框 */
  open?: boolean
  /** 目标租户 */
  tenant?: Tenant | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  tenant: null
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignTenantUsers')

/** 弹窗显隐 */
const visible = ref(false)
/** 提交 loading */
const loading = ref(false)
/** 选项 loading */
const optionsLoading = ref(false)
/** 已选 userId */
const targetKeys = ref<string[]>([])
/** 全量用户选项 */
const allOptions = ref<TaktSelectOption[]>([])
/** 租户编码 */
const tenantCode = ref('')
/** 租户只读展示 */
const tenantInfo = ref('')

/** Transfer 数据源 */
const dataSource = computed(() =>
  allOptions.value.map((item) => ({
    key: String(item.dictValue),
    title: item.dictLabel ?? '',
    description: String(item.dictValue ?? ''),
  }))
)

watch(() => props.open, (val) => {
  visible.value = val
  if (val && props.tenant) {
    loadTenantUsers()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

/**
 * 加载用户选项与租户已绑 userId
 * @returns {Promise<void>}
 */
async function loadTenantUsers() {
  const tenant = props.tenant
  const code = tenant?.tenantCode?.trim()
  if (!code) return
  try {
    loading.value = true
    optionsLoading.value = true
    tenantCode.value = code
    tenantInfo.value = `${tenant?.tenantName ?? ''}（${code}）`
    const [allUsers, userTenants] = await Promise.all([
      getUserOptions(),
      getTenantUserIds(code)
    ])
    allOptions.value = allUsers
    targetKeys.value = userTenants
      .map((row: UserTenant) => String(row.userId || ''))
      .filter((id: string) => id)
  } catch (error: unknown) {
    logger.error('[AssignTenantUsers] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.tenant._self') + t('entity.user._self') }))
  } finally {
    loading.value = false
    optionsLoading.value = false
  }
}

/**
 * 提交 assignTenantUsers
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!tenantCode.value) {
    message.error(t('common.validation.not.found', { field: t('entity.tenant._self') }))
    return
  }
  try {
    loading.value = true
    await assignTenantUsers(tenantCode.value, targetKeys.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.user._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignTenantUsers] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.user._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  tenantCode.value = ''
  targetKeys.value = []
  allOptions.value = []
  tenantInfo.value = ''
}
</script>
