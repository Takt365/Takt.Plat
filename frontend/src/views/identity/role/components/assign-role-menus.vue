<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：@/views/identity/role/components -->
<!-- 文件名称：assign-role-menus.vue -->
<!-- 功能描述：分配角色菜单弹窗；树形 Transfer + getMenuTreeOptions / getRoleMenuIds / assignRoleMenus。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.page.button.allocate') + t('entity.rolemenu._self')"
    :width="'50vw'"
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
      <a-form-item :label="t('entity.role._self')">
        <a-input
          :value="roleInfo"
          disabled
        />
      </a-form-item>
      <a-form-item :label="t('entity.menu._self')">
        <a-transfer
          v-model:target-keys="selectedMenuIds"
          class="tree-transfer"
          :data-source="transferDataSource"
          :list-style="{
            width: '250px',
            height: '50vh',
          }"
          :render="item => item.title"
          :show-select-all="false"
          :loading="menuOptionsLoading"
          :disabled="!roleId"
        >
          <template #children="{ direction, selectedKeys, onItemSelect }">
            <a-tree
              v-if="direction === 'left'"
              block-node
              checkable
              :check-strictly="false"
              default-expand-all
              :checked-keys="[...selectedKeys.map((k: string | number) => String(k)), ...selectedMenuIds]"
              :tree-data="treeData"
              :field-names="{ title: 'dictLabel', key: 'dictValue', children: 'children' }"
              @check="(checked: unknown, info: { node?: unknown }) => {
                if (!info) return
                const checkedKeys = Array.isArray(checked) ? checked : (checked as { checked?: string[] }).checked || []
                const allKeys = [...selectedKeys.map((k: string | number) => String(k)), ...selectedMenuIds]
                const newKeys = checkedKeys.filter((k: string | number) => !allKeys.includes(String(k)))
                const removedKeys = allKeys.filter((k: string) => !checkedKeys.map((ck: string | number) => String(ck)).includes(k))
                newKeys.forEach((key: string | number) => onItemSelect(String(key), true))
                removedKeys.forEach((key: string) => onItemSelect(key, false))
              }"
            />
            <a-tree
              v-else-if="direction === 'right'"
              block-node
              checkable
              :check-strictly="false"
              default-expand-all
              :checked-keys="selectedMenuIds"
              :tree-data="rightTreeData"
              :field-names="{ title: 'dictLabel', key: 'dictValue', children: 'children' }"
              @check="handleRightTreeCheck"
            />
          </template>
        </a-transfer>
      </a-form-item>
    </a-form>
  </a-modal>
</template>

<script setup lang="ts">
/**
 * 分配角色菜单弹窗：按 roleId 加载菜单树并提交 assignRoleMenus。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { TransferProps } from 'ant-design-vue'
import { getRoleMenuIds, assignRoleMenus } from '@/api/identity/rbac'
import { getMenuTreeOptions } from '@/api/identity/menu'
import type { Role } from '@/types/identity/role'
import type { TaktTreeSelectOption } from '@/types/common'

/** 树节点（Transfer / Tree 共用） */
interface MenuTreeNode {
  key: string
  title: string
  dictValue: string | number
  dictLabel: string
  disabled?: boolean
  children?: MenuTreeNode[]
}

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
  /** 目标角色 */
  role?: Role | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  role: null
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignRoleMenus')

/** 弹窗显隐 */
const visible = ref(false)
/** 提交 loading */
const loading = ref(false)
/** 菜单树 loading */
const menuOptionsLoading = ref(false)
/** 当前角色 id */
const roleId = ref('')
/** Transfer 右侧已选菜单 id */
const selectedMenuIds = ref<string[]>([])
/** 全量菜单树 */
const allMenuOptions = ref<TaktTreeSelectOption[]>([])
/** Transfer 展平数据源 */
const transferDataSource = ref<TransferProps['dataSource']>([])
/** 角色只读展示 */
const roleInfo = ref('')

/**
 * 展平菜单树为 Transfer 行
 * @param list 菜单树
 */
function flattenMenuTree(list: TaktTreeSelectOption[] = []) {
  if (!transferDataSource.value) {
    transferDataSource.value = []
  }
  list.forEach((item) => {
    transferDataSource.value!.push({
      key: String(item.dictValue),
      title: item.dictLabel || '',
    })
    if (item.children?.length) {
      flattenMenuTree(item.children)
    }
  })
}

/**
 * 构建左侧菜单树
 * @param treeNodes 菜单树
 * @returns {MenuTreeNode[]} 树节点
 */
function buildMenuTreeNodes(treeNodes: TaktTreeSelectOption[]): MenuTreeNode[] {
  return treeNodes.map(({ children, ...node }) => ({
    key: String(node.dictValue),
    title: node.dictLabel || '',
    dictValue: node.dictValue,
    dictLabel: node.dictLabel || '',
    disabled: false,
    children: buildMenuTreeNodes(children ?? [])
  }))
}

/**
 * 按已选 key 过滤右侧菜单子树
 * @param treeNodes 菜单树
 * @param selectedKeys 已选 menuId
 * @returns {MenuTreeNode[]} 过滤后的树
 */
function filterMenuTreeBySelectedKeys(treeNodes: TaktTreeSelectOption[], selectedKeys: string[]): MenuTreeNode[] {
  return treeNodes.map(({ children, ...node }) => {
    const key = String(node.dictValue)
    const filteredChildren = children?.length ? filterMenuTreeBySelectedKeys(children, selectedKeys) : []
    const isSelected = selectedKeys.includes(key)
    const hasSelectedChildren = filteredChildren.length > 0
    if (isSelected || hasSelectedChildren) {
      return {
        key,
        title: node.dictLabel || '',
        dictValue: node.dictValue,
        dictLabel: node.dictLabel || '',
        children: filteredChildren
      }
    }
    return null
  }).filter(Boolean) as MenuTreeNode[]
}

/** 左侧菜单树 */
const treeData = computed(() => buildMenuTreeNodes(allMenuOptions.value))

/** 右侧已选菜单树 */
const rightTreeData = computed(() => {
  if (selectedMenuIds.value.length === 0 || allMenuOptions.value.length === 0) return []
  return filterMenuTreeBySelectedKeys(allMenuOptions.value, selectedMenuIds.value)
})

/**
 * 右侧树取消勾选
 * @param checked 勾选结果
 */
function handleRightTreeCheck(checked: unknown) {
  const checkedKeys = Array.isArray(checked) ? checked : (checked as { checked?: string[] }).checked || []
  const removedKeys = selectedMenuIds.value.filter((k) => !checkedKeys.map((ck: string | number) => String(ck)).includes(k))
  removedKeys.forEach((key) => {
    const index = selectedMenuIds.value.indexOf(key)
    if (index > -1) selectedMenuIds.value.splice(index, 1)
  })
}

watch(() => props.open, (val) => {
  visible.value = val
  if (val && props.role) {
    loadRoleMenus()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

/**
 * 加载角色信息与菜单分配
 * @returns {Promise<void>}
 */
async function loadRoleMenus() {
  const role = props.role
  if (!role?.roleId) {
    message.error(t('common.validation.not.found', { field: `${t('entity.role._self')} ID` }))
    return
  }
  try {
    menuOptionsLoading.value = true
    roleId.value = String(role.roleId)
    roleInfo.value = `${role.roleName ?? ''}（${role.roleCode ?? ''}）`
    const [allMenus, roleMenus] = await Promise.all([
      getMenuTreeOptions(),
      getRoleMenuIds(roleId.value)
    ])
    allMenuOptions.value = allMenus
    transferDataSource.value = []
    flattenMenuTree(allMenus)
    selectedMenuIds.value = roleMenus.map((item) => String(item.menuId))
  } catch (error: unknown) {
    logger.error('[AssignRoleMenus] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.rolemenu._self') }))
  } finally {
    menuOptionsLoading.value = false
  }
}

/**
 * 提交 assignRoleMenus
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!roleId.value) {
    message.error(t('common.validation.not.found', { field: t('entity.role._self') }))
    return
  }
  try {
    loading.value = true
    await assignRoleMenus(roleId.value, selectedMenuIds.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.rolemenu._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignRoleMenus] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.rolemenu._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  roleId.value = ''
  selectedMenuIds.value = []
  transferDataSource.value = []
  allMenuOptions.value = []
  roleInfo.value = ''
}
</script>

<style scoped lang="css">
.tree-transfer :deep(.ant-transfer-list:first-child) {
  width: 50%;
  flex: none;
}
</style>
