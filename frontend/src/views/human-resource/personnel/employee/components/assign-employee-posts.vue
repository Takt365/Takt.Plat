<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：@/views/human-resource/personnel/employee/components -->
<!-- 文件名称：assign-employee-posts.vue -->
<!-- 功能描述：分配员工岗位弹窗；Transfer + getPostOptions / getEmployeePostIds / assignEmployeePosts。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.page.button.allocate') + t('entity.post._self')"
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
      <a-form-item :label="t('entity.employee._self')">
        <a-input
          :value="employeeInfo"
          disabled
        />
      </a-form-item>
      <a-form-item :label="t('entity.post._self')">
        <a-transfer
          v-model:target-keys="targetKeys"
          :data-source="dataSource"
          :list-style="{
            width: '250px',
            height: '50vh',
          }"
          :titles="[t('common.tip.transfer.unassigned'), t('common.tip.transfer.assigned')]"
          show-search
          :loading="optionsLoading"
          :render="item => item.title"
        />
      </a-form-item>
    </a-form>
  </a-modal>
</template>

<script setup lang="ts">
/**
 * 分配员工岗位弹窗：岗位 Transfer，提交 assignEmployeePosts（postId 列表）。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { getPostOptions } from '@/api/human-resource/organization/post'
import { getEmployeePostIds, assignEmployeePosts } from '@/api/identity/rbac'
import type { Employee } from '@/types/human-resource/personnel/employee'
import type { EmployeePost } from '@/types/human-resource/organization/employee-post'
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
  /** 目标员工 */
  employee?: Employee | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  employee: null
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignEmployeePosts')

/** 弹窗显隐 */
const visible = ref(false)
/** 提交 loading */
const loading = ref(false)
/** 选项 loading */
const optionsLoading = ref(false)
/** 已选 postId */
const targetKeys = ref<string[]>([])
/** 全量岗位选项 */
const allOptions = ref<TaktSelectOption[]>([])
/** 员工 id */
const employeeId = ref('')
/** 员工只读展示 */
const employeeInfo = ref('')

/** Transfer 数据源 */
const dataSource = computed(() =>
  allOptions.value.map((item) => ({
    key: String(item.dictValue),
    title: item.dictLabel ?? ''
  }))
)

watch(() => props.open, (val) => {
  visible.value = val
  if (val && props.employee) {
    loadEmployeePosts()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

/**
 * 加载岗位选项与员工已绑 postId
 * @returns {Promise<void>}
 */
async function loadEmployeePosts() {
  const employee = props.employee
  if (!employee?.employeeId) return
  try {
    loading.value = true
    optionsLoading.value = true
    employeeId.value = String(employee.employeeId)
    const name = employee.name ?? (employee as { displayName?: string }).displayName ?? ''
    const no = employee.employeeNo ?? ''
    employeeInfo.value = `${name}${no ? `（${no}）` : ''}`
    const [allPosts, employeePosts] = await Promise.all([
      getPostOptions(),
      getEmployeePostIds(employeeId.value)
    ])
    allOptions.value = allPosts
    targetKeys.value = employeePosts
      .map((row: EmployeePost) => String(row.postId || ''))
      .filter((id: string) => id)
  } catch (error: unknown) {
    logger.error('[AssignEmployeePosts] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.employee._self') + t('entity.post._self') }))
  } finally {
    loading.value = false
    optionsLoading.value = false
  }
}

/**
 * 提交 assignEmployeePosts
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!employeeId.value) {
    message.error(t('common.validation.not.found', { field: t('entity.employee._self') }))
    return
  }
  try {
    loading.value = true
    await assignEmployeePosts(employeeId.value, targetKeys.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.post._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignEmployeePosts] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.post._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  employeeId.value = ''
  targetKeys.value = []
  allOptions.value = []
  employeeInfo.value = ''
}
</script>
