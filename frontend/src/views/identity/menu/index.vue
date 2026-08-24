<!-- ======================================== -->
<!-- 项目名称：节节拍工厂·Takt Plat  -->
<!-- 命名空间：@/views/identity/menu -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：菜单管理页面，包含菜单列表、查询、新增、编辑、删除、导入、导出等 -->
<!-- ======================================== -->

<template>
  <div class="identity-menu">
    <!-- 第一行：左 1/4 树查询栏 | 右 3/4 表查询栏 -->
    <div class="menu-query-row">
      <TaktTreeLeftQueryBar
        v-model="treeQueryKeyword"
        @search="handleTreeQuerySearch"
      />
      <TaktTreeRightQueryBar
        v-model="queryKeyword"
        :placeholder="t('common.page.form.placeholder.search', { keyword: t('entity.menu.name') + t('common.tip.or') + t('entity.menu.code') })"
        :loading="loading"
        @search="handleSearch"
        @reset="handleReset"
      />
    </div>

    <!-- 第二行：左 1/4 树工具栏 | 右 3/4 表工具栏 -->
    <div class="menu-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadMenuTree"
      />
      <TaktTreeRightToolsBar
        create-permission="identity:menu:create"
        update-permission="identity:menu:update"
        delete-permission="identity:menu:delete"
        import-permission="identity:menu:import"
        export-permission="identity:menu:export"
        :show-create="true"
        :show-update="true"
        :show-delete="true"
        :show-import="true"
        :show-export="true"
        :show-advanced-query="true"
        :show-column-setting="true"
        :show-fullscreen="true"
        :show-refresh="true"
        :show-expand="true"
        :update-disabled="!selectedRow"
        :delete-disabled="!selectedRow && selectedRows.length === 0"
        :create-loading="loading"
        :update-loading="loading"
        :delete-loading="loading"
        :refresh-loading="loading"
        :expanded="tableExpanded"
        @create="handleCreate"
        @update="handleUpdate"
        @delete="handleDelete"
        @import="handleImport"
        @export="handleExport"
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @refresh="handleRefresh"
        @update:expanded="(v: boolean) => (tableExpanded = v)"
      />
    </div>

    <!-- 第三行：左 1/4 树 | 右 3/4 树表 -->
    <div class="menu-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selected-keys="selectedTreeKeys"
        :tree-data="filteredMenuTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="true"
        :draggable="true"
        @tree-select="handleTreeSelect"
        @tree-drop="handleMenuTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="tenant"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'menuId'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="tableFilteredTree"
        v-model:expanded-row-keys="tableExpandedRowKeys"
        :loading="loading"
        :row-key="getMenuId"
        :stripe="true"
        :row-selection="rowSelection"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'menuName'">
            {{ getMenuField(record, 'menuName') }}
          </template>
          <template v-else-if="column.key === 'icon'">
            <span
              class="inline-flex items-center gap-1"
              :title="String(getMenuField(record, 'icon') ?? '')"
            >
              <takt-remix-icon
                :name="String(getMenuField(record, 'icon') ?? '')"
                :size="18"
                :show-placeholder="!!getMenuField(record, 'icon')"
              />
            </span>
          </template>
          <template v-else-if="column.key === 'menuType'">
            <TaktDictTag
              :value="getMenuField(record, 'menuType')"
              dict-type="sys_menu_type"
            />
          </template>
          <template v-else-if="column.key === 'menuStatus'">
            <a-switch
              :checked="getMenuField(record, 'menuStatus') === 1"
              :disabled="getMenuField(record, 'isBuiltIn') === 1"
              :checked-children="t('common.page.button.enable')"
              :un-checked-children="t('common.page.button.disable')"
              :loading="Boolean(switchLoadingMap.get(getMenuId(record) + ':menuStatus'))"
              @change="(checked: unknown) => handleMenuStatusSwitch(record, Boolean(checked) ? 1 : 0)"
            />
          </template>
          <template v-else-if="column.key === 'isVisible'">
            <a-switch
              :checked="getMenuField(record, 'isVisible') === 1"
              :checked-children="t('common.page.button.enable')"
              :un-checked-children="t('common.page.button.disable')"
              :loading="Boolean(switchLoadingMap.get(getMenuId(record) + ':isVisible'))"
              @change="(checked: unknown) => handleMenuSwitch(record, 'isVisible', Boolean(checked) ? 1 : 0)"
            />
          </template>
          <template v-else-if="column.key === 'isCached'">
            <a-switch
              :checked="getMenuField(record, 'isCached') === 1"
              :checked-children="t('common.page.button.enable')"
              :un-checked-children="t('common.page.button.disable')"
              :loading="Boolean(switchLoadingMap.get(getMenuId(record) + ':isCached'))"
              @change="(checked: unknown) => handleMenuSwitch(record, 'isCached', Boolean(checked) ? 1 : 0)"
            />
          </template>
          <template v-else-if="column.key === 'isExternal'">
            <a-switch
              :checked="getMenuField(record, 'isExternal') === 1"
              :checked-children="t('common.page.button.enable')"
              :un-checked-children="t('common.page.button.disable')"
              :loading="Boolean(switchLoadingMap.get(getMenuId(record) + ':isExternal'))"
              @change="(checked: unknown) => handleMenuSwitch(record, 'isExternal', Boolean(checked) ? 1 : 0)"
            />
          </template>
        </template>
      </TaktTreeRightTable>
    </div>

    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MenuForm
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.menu.name')">
        <a-input v-model:value="advancedQueryForm.menuName" />
      </a-form-item>
      <a-form-item :label="t('entity.menu.code')">
        <a-input v-model:value="advancedQueryForm.menuCode" />
      </a-form-item>
      <a-form-item :label="t('entity.menu.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.menuType"
          dict-type="sys_menu_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.type') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.menu.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.menuStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.status') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.menu._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.menu._self"
        file-type="xlsx"
        :sheet-name="menuExcelNames.sheet"
        :template-file-name="menuExcelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>

    <AssignMenuRoles
      v-model:open="assignMenuRolesVisible"
      :menu="currentAssignMenu"
      @success="handleAssignSuccess"
    />

    <TaktColumnDrawer
      entity-scope="tenant"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'menuId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted } from 'vue'
import type { TreeDataItem } from 'ant-design-vue/es/tree'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import MenuForm from './components/menu-form.vue'
import AssignMenuRoles from './components/assign-menu-roles.vue'
import { getMenuTree, getMenuById, createMenu, updateMenu, updateMenuStatus, deleteMenuById, getMenuTemplate, importMenu, exportMenu } from '@/api/identity/menu'
import type { Menu, MenuQuery, MenuTree } from '@/types/identity/menu'
import type { TreeDropPayload } from '@/components/business/takt-tree-left-table/index.vue'
import { taktExcelEntityNames } from '@/utils/naming'
import { RiEditLine, RiDeleteBinLine, RiUserSettingsLine } from '@remixicon/vue'
import { TaktMenuType } from '@/utils/common'
import {
  collectTaktTreeTableExpandableKeys,
  filterTaktTreeTableNodes,
  taktTreeTableNodeKey,
} from '@/utils/takt-tree-table'

const { t } = useI18n()
const menuExcelNames = taktExcelEntityNames('TaktMenu')

const queryKeyword = ref('')
const treeQueryKeyword = ref('')
/** 左侧树结构展开状态：true=全部展开，false=全部折叠（仅左侧工具栏控制），默认收缩 */
const treeExpanded = ref(false)
/** 左侧树当前展开的节点 key 列表（受控传给 TaktTreeTable） */
const treeExpandedKeys = ref<(string | number)[]>([])
/** 右侧树表工具栏「全部展开/收缩」 */
const tableExpanded = ref(false)
/** 右侧 a-table 树表当前展开行 key（与 row-key / menuId 一致） */
const tableExpandedRowKeys = ref<(string | number)[]>([])
const loading = ref(false)
/** 左侧导航树数据源（getMenuTree 全量，不受右侧查询影响） */
const navFullTableTree = ref<any[]>([])
/** 右侧树表数据源（与左侧共用 getMenuTree 结构，点选联动） */
const fullTableTree = ref<any[]>([])
/** 左侧树数据（由 navFullTableTree 派生） */
const menuTreeData = ref<TreeDataItem[]>([])
const selectedTreeKeys = ref<(string | number)[]>([])
const selectedRow = ref<Menu | null>(null)
const selectedRows = ref<Menu[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Menu>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
/** 高级查询（字段与 MenuQuery 一致） */
type MenuAdvancedQueryForm = Pick<MenuQuery, 'menuName' | 'menuCode' | 'menuType' | 'menuStatus'>
const emptyMenuAdvancedQueryForm = (): MenuAdvancedQueryForm => ({ menuName: '', menuCode: '' })
const advancedQueryForm = ref<MenuAdvancedQueryForm>(emptyMenuAdvancedQueryForm())
const importVisible = ref(false)
const assignMenuRolesVisible = ref(false)
const currentAssignMenu = ref<Menu | null>(null)
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
/** 开关列加载状态：key 为 `${menuId}:${field}` */
const switchLoadingMap = ref(new Map<string, boolean>())

/** 菜单类型：0=目录，1=菜单，2=按钮；TreeDataItem 排除类型为 2 的节点 */
function isMenuTypeButton(n: any): boolean {
  const typeVal = n?.menuType ?? n?.type ?? n?.MenuType
  return typeVal === TaktMenuType.Button || Number(typeVal) === TaktMenuType.Button
}

/**
 * 将接口菜单树转为右侧树表节点（保留 children，供 getSubtree 与 a-table 树形展示）
 * @param nodes 菜单树
 */
function menuTreeToTableNodes(nodes: MenuTree[]): any[] {
  if (!nodes?.length) return []
  return nodes.map((node) => {
    const childNodes = node.children?.length ? menuTreeToTableNodes(node.children) : []
    return {
      ...node,
      key: String(node.menuId),
      children: childNodes.length > 0 ? childNodes : undefined,
    }
  })
}

const getMenuId = (record: { menuId?: string; id?: string } | null | undefined): string =>
  record?.menuId != null ? String(record.menuId) : (record?.id != null ? String(record.id) : '')

/** 将 fullTableTree 转为左侧 a-tree 的 TreeDataItem（title, key, children）；排除类型为 2（按钮） */
function mapFullTableTreeToTreeData(nodes: any[]): TreeDataItem[] {
  if (!nodes?.length) return []
  return nodes
    .filter((n: any) => !isMenuTypeButton(n))
    .map((n: any): TreeDataItem => {
      const title = n.menuName ?? n.title ?? ''
      const key = String(n.menuId ?? n.key ?? n.id ?? '')
      if (!n.children?.length) {
        return { title, key }
      }
      const children = mapFullTableTreeToTreeData(n.children)
      return children.length > 0 ? { title, key, children } : { title, key }
    })
}

/**
 * 按 key 查找树节点（与左侧树、右侧表共用 fullTableTree）
 * @param nodes 树节点列表
 * @param key 节点 key（menuId）
 */
function findTreeNodeByKey(nodes: any[], key: string | number): any | null {
  const k = String(key)
  for (const node of nodes) {
    if (String(node.key ?? node.menuId ?? node.id) === k) {
      return node
    }
    if (node.children?.length) {
      const found = findTreeNodeByKey(node.children, key)
      if (found) return found
    }
  }
  return null
}

/** 从树中取以某 key 为根的子树（返回单元素数组，便于作为表格根） */
function getSubtree(nodes: any[], key: string | number): any[] {
  const node = findTreeNodeByKey(nodes, key)
  return node ? [node] : []
}

/** 按关键字过滤树：保留 title 包含关键字的节点及其祖先、子孙 */
function filterTreeByKeyword(nodes: TreeDataItem[], keyword: string): TreeDataItem[] {
  const k = (keyword ?? '').trim().toLowerCase()
  if (!k) return nodes
  function filter(nodes: TreeDataItem[]): TreeDataItem[] {
    if (!nodes?.length) return []
    return nodes
      .map(node => {
        const title = String(node.title ?? '').toLowerCase()
        const matched = title.includes(k)
        const filteredChildren = node.children?.length ? filter(node.children) : undefined
        const hasMatchInChildren = filteredChildren != null && filteredChildren.length > 0
        if (matched || hasMatchInChildren) {
          if (filteredChildren != null && filteredChildren.length > 0) {
            return { ...node, children: filteredChildren } as TreeDataItem
          }
          const { children: _omitChildren, ...rest } = node
          return rest as TreeDataItem
        }
        return null
      })
      .filter(Boolean) as TreeDataItem[]
  }
  return filter(nodes)
}

const filteredMenuTreeData = computed(() =>
  filterTreeByKeyword(menuTreeData.value, treeQueryKeyword.value)
)

/** 收集树中所有有子节点的 key（用于“全部展开”） */
function getAllParentKeys(nodes: TreeDataItem[]): (string | number)[] {
  const keys: (string | number)[] = []
  function walk(list: TreeDataItem[]) {
    if (!list?.length) return
    for (const node of list) {
      if (node.children?.length) {
        keys.push(node.key)
        walk(node.children)
      }
    }
  }
  walk(nodes)
  return keys
}

/** 工具栏「全部展开/收缩」仅切换 treeExpanded，不随关键字过滤清空用户已展开节点 */
watch(treeExpanded, (expanded) => {
  treeExpandedKeys.value = expanded ? getAllParentKeys(filteredMenuTreeData.value) : []
})

watch(filteredMenuTreeData, () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = getAllParentKeys(filteredMenuTreeData.value)
  }
})

const getMenuField = (record: any, field: string): any => record?.[field]

/** 右侧树表数据：仅当左侧选中节点时显示该节点（含全部子孙）；默认左树不选中、右表为空 */
const tableTreeData = computed(() => {
  const tree = fullTableTree.value
  if (!tree?.length) return []
  const keys = selectedTreeKeys.value
  if (keys.length === 0) return []
  const activeKey = keys[keys.length - 1]
  if (activeKey === undefined) return []
  return getSubtree(tree, activeKey)
})

/** 右侧查询条件过滤（仅影响表格展示，不替换 fullTableTree） */
function matchesMenuRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    const code = String(record.menuCode ?? '').toLowerCase()
    const name = String(record.menuName ?? '').toLowerCase()
    if (!code.includes(k) && !name.includes(k)) return false
  }
  const adv = advancedQueryForm.value
  if (adv.menuName && !String(record.menuName ?? '').includes(adv.menuName)) return false
  if (adv.menuCode && !String(record.menuCode ?? '').includes(adv.menuCode)) return false
  if (adv.menuType !== undefined && record.menuType !== adv.menuType) return false
  if (adv.menuStatus !== undefined && record.menuStatus !== adv.menuStatus) return false
  return true
}

/** 右侧过滤后的树（保留 children，供 a-table 展开/收缩） */
const tableFilteredTree = computed(() =>
  filterTaktTreeTableNodes(tableTreeData.value, matchesMenuRightQuery)
)

/**
 * 同步右侧树表全部展开/收缩
 * @returns {void}
 */
function applyMenuTableExpandState() {
  tableExpandedRowKeys.value = tableExpanded.value
    ? collectTaktTreeTableExpandableKeys(tableFilteredTree.value, (node) =>
        taktTreeTableNodeKey(node, 'menuId'),
      )
    : []
}

watch(tableExpanded, applyMenuTableExpandState)
watch(tableFilteredTree, () => {
  if (tableExpanded.value) applyMenuTableExpandState()
})

const handleTreeSelect = (selectedKeys: (string | number)[]) => {
  selectedTreeKeys.value = selectedKeys
}

/** 从树结构中查找节点 key 的父级 key 与在同级中的序号（用于 parentId / sortOrder） */
function findParentAndOrderNum(
  tree: any[],
  targetKey: string | number,
  parentKey: string = '0'
): { parentId: string; sortOrder: number } | null {
  const keyStr = String(targetKey)
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i]
    const k = String(node?.key ?? node?.menuId ?? '')
    if (k === keyStr) {
      return { parentId: parentKey, sortOrder: i }
    }
    const children = node?.children ?? []
    if (children.length) {
      const found = findParentAndOrderNum(children, targetKey, k)
      if (found) return found
    }
  }
  return null
}

const handleMenuTreeDrop = async (payload: TreeDropPayload) => {
  const { newTreeData, dragKey } = payload
  const pos = findParentAndOrderNum(newTreeData, dragKey)
  if (!pos) return
  menuTreeData.value = newTreeData as TreeDataItem[]
  try {
    loading.value = true
    const full = await getMenuById(String(dragKey))
    await updateMenu(String(dragKey), {
      ...full,
      menuId: String(full.menuId ?? dragKey),
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    })
    message.success(t('identity.menu.page.msg.orderupdated'))
    loadData()
  } catch (error: any) {
    message.error(error?.message ?? t('common.feedback.failed'))
    loadData()
  } finally {
    loading.value = false
  }
}

function setSwitchLoading(menuId: string, field: string, value: boolean) {
  const key = `${menuId}:${field}`
  const next = new Map(switchLoadingMap.value)
  if (value) next.set(key, true)
  else next.delete(key)
  switchLoadingMap.value = next
}

/** 可见/缓存/外联 开关变更：调用 update 部分更新后刷新列表 */
async function handleMenuSwitch(record: Menu, field: 'isVisible' | 'isCached' | 'isExternal', value: number) {
  const id = getMenuId(record)
  if (!id) return
  setSwitchLoading(id, field, true)
  try {
    const full = await getMenuById(id)
    await updateMenu(id, {
      ...full,
      menuId: id,
      [field]: value,
    })
    message.success(t('common.feedback.updated'))
    await loadData()
  } catch (error: any) {
    message.error(error?.message ?? t('common.feedback.failed'))
  } finally {
    setSwitchLoading(id, field, false)
  }
}

/** 状态开关变更：调用 updateStatus 后刷新列表 */
async function handleMenuStatusSwitch(record: any, menuStatus: number) {
  const id = getMenuId(record)
  if (!id) return
  setSwitchLoading(id, 'menuStatus', true)
  try {
    await updateMenuStatus({ menuId: id, menuStatus })
    message.success(t('common.feedback.updated'))
    await loadData()
  } catch (error: any) {
    message.error(error?.message ?? t('common.feedback.failed'))
  } finally {
    setSwitchLoading(id, 'menuStatus', false)
  }
}

/** 左侧树关键字搜索（客户端过滤，不重复请求列表接口） */
const handleTreeQuerySearch = () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = getAllParentKeys(filteredMenuTreeData.value)
  }
}

/**
 * 加载左侧菜单树（含禁用项，供管理页筛选）
 */
const loadMenuTree = async (): Promise<MenuTree[]> => {
  const res = await getMenuTree('0', true)
  const resAny = res as { data?: MenuTree[]; Data?: MenuTree[] }
  const trees: MenuTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const tableNodes = menuTreeToTableNodes(trees)
  navFullTableTree.value = tableNodes
  fullTableTree.value = tableNodes
  menuTreeData.value = mapFullTableTreeToTreeData(tableNodes)
  if (treeExpanded.value) {
    treeExpandedKeys.value = getAllParentKeys(filteredMenuTreeData.value)
  }
  return trees
}

onMounted(() => {
  loadData()
})

// 列顺序与 Menu 类型字段一致
const columns = computed<TableColumnsType>(() => [
  {
    title: 'ID',
    dataIndex: 'menuId',
    key: 'menuId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left'
  },
  {
    title: t('entity.menu.code'),
    dataIndex: 'menuCode',
    key: 'menuCode',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.name'),
    dataIndex: 'menuName',
    key: 'menuName',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.l10nkey'),
    dataIndex: 'i18nKey',
    key: 'i18nKey',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.icon'),
    dataIndex: 'icon',
    key: 'icon',
    width: 90,
    ellipsis: true
  },
  {
    title: t('entity.menu.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 90,
    ellipsis: true
  },
  {
    title: t('entity.menu.level'),
    dataIndex: 'level',
    key: 'level',
    width: 70
  },
  {
    title: t('entity.menu.path'),
    dataIndex: 'menuPath',
    key: 'menuPath',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.isleaf'),
    dataIndex: 'isLeaf',
    key: 'isLeaf',
    width: 90
  },
  {
    title: t('entity.menu.type'),
    dataIndex: 'menuType',
    key: 'menuType',
    width: 80
  },
  {
    title: t('entity.menu.permission'),
    dataIndex: 'permission',
    key: 'permission',
    width: 140,
    ellipsis: true
  },
  {
    title: t('entity.menu.routepath'),
    dataIndex: 'routePath',
    key: 'routePath',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.component'),
    dataIndex: 'componentPath',
    key: 'componentPath',
    width: 160,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 80
  },
  {
    title: t('entity.menu.isexternal'),
    dataIndex: 'isExternal',
    key: 'isExternal',
    width: 80
  },
  {
    title: t('entity.menu.linkurl'),
    dataIndex: 'externalUrl',
    key: 'externalUrl',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.iscached'),
    dataIndex: 'isCached',
    key: 'isCached',
    width: 70
  },
  {
    title: t('entity.menu.isvisible'),
    dataIndex: 'isVisible',
    key: 'isVisible',
    width: 70
  },
  {
    title: t('entity.menu.status'),
    dataIndex: 'menuStatus',
    key: 'menuStatus',
    width: 80
  },
  {
    title: t('entity.menu.description'),
    dataIndex: 'description',
    key: 'description',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  CreateActionColumn<Menu>({
    actions: [
      { key: 'update', label: t('common.page.button.edit'), shape: 'plain', icon: RiEditLine, permission: 'identity:menu:update', onClick: (record: Menu) => handleEdit(record) },
      { key: 'allocate-role-menu', label: t('common.page.button.allocate') + t('entity.role._self'), shape: 'plain', icon: RiUserSettingsLine, permission: 'identity:menu:update', onClick: (record: Menu) => handleAssignMenuRoles(record) },
      { key: 'delete', label: t('common.page.button.delete'), shape: 'plain', icon: RiDeleteBinLine, permission: 'identity:menu:delete', onClick: (record: Menu) => handleDeleteOne(record) }
    ]
  })
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Menu[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Menu, selected: boolean) => {
    if (selected) selectedRow.value = record
    else if (selectedRow.value && getMenuId(selectedRow.value) === getMenuId(record)) selectedRow.value = null
  },
  onSelectAll: (selected: boolean, selectedRowsData: Menu[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 初始化或增删改后刷新全量菜单树（左右共用 fullTableTree） */
const loadData = async () => {
  loading.value = true
  try {
    await loadMenuTree()
  } catch (error: unknown) {
    logger.error('[Menu] 加载数据失败', undefined, error)
    message.error((error as { message?: string })?.message || t('common.feedback.load.data.failed'))
    menuTreeData.value = []
    navFullTableTree.value = []
    fullTableTree.value = []
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 右侧查询（不影响左侧树与 fullTableTree；过滤为 computed） */
const handleSearch = () => {}

/** 右侧重置（不影响左侧树） */
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = emptyMenuAdvancedQueryForm()
}

const handleTableChange = (_pagination: any, _filters: any, sorter: any) => {
  if (sorter?.order) logger.debug('[Menu] 排序', { field: sorter.field, order: sorter.order })
}

const handleResizeColumn = (w: number, col: any) => {
  const column = columns.value.find((c: any) => {
    const colKey = col.key || col.dataIndex || col.title
    const cKey = c.key || c.dataIndex || c.title
    return colKey && cKey && String(colKey) === String(cKey)
  })
  if (column) (column as any).width = w
}

const handleCreate = () => {
  formTitle.value = t('common.page.button.create') + t('entity.menu._self')
  formData.value = {}
  formVisible.value = true
}

const handleEdit = (record: Menu) => {
  formTitle.value = t('common.page.button.edit') + t('entity.menu._self')
  formData.value = { ...record }
  formVisible.value = true
}

const handleAssignMenuRoles = (record: Menu) => {
  currentAssignMenu.value = record
  assignMenuRolesVisible.value = true
}

const handleAssignSuccess = () => {
  loadData()
}

const handleUpdate = () => {
  if (selectedRow.value) handleEdit(selectedRow.value)
  else message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.menu._self') }))
}

const handleDeleteOne = (record: Menu) => {
  const name = getMenuField(record, 'menuName') || getMenuId(record)
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.menu._self'), name }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteMenuById(getMenuId(record))
        message.success(t('common.feedback.deleted'))
        loadData()
      } catch (error: any) {
        message.error(error?.message || t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

const handleDelete = () => {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.menu._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { count: selectedRows.value.length, entity: t('entity.menu._self') }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await Promise.all(selectedRows.value.map(record => deleteMenuById(getMenuId(record))))
        message.success(t('common.feedback.deleted'))
        selectedRows.value = []
        selectedRowKeys.value = []
        selectedRow.value = null
        loadData()
      } catch (error: any) {
        message.error(error?.message || t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

const handleFormSubmit = async () => {
  try {
    if (!formRef.value) return
    await formRef.value.validate()
    const formValues = formRef.value.getValues()
    formLoading.value = true
    if (formData.value?.menuId) {
      await updateMenu(formData.value.menuId, { ...formValues, menuId: formData.value.menuId })
      message.success(t('common.feedback.updated'))
    } else {
      await createMenu(formValues)
      message.success(t('common.feedback.created'))
    }
    formRef.value?.resetFields()
    formData.value = {}
    formVisible.value = false
    loadData()
  } catch (error: any) {
    if (error?.errorFields) return
    message.error(error?.message || t('common.feedback.failed'))
  } finally {
    formLoading.value = false
  }
}

const handleFormCancel = () => {
  formVisible.value = false
  formData.value = {}
  formRef.value?.resetFields()
}

const handleImport = () => { importVisible.value = true }
const handleDownloadTemplate = async (sheetName?: string, fileName?: string) => {
  return await getMenuTemplate(sheetName, fileName)
}
const handleImportFile = async (file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> => {
  return await importMenu(file, sheetName)
}
const handleImportSuccess = (result: { success: number; fail: number; errors: string[] }) => {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}
const handleImportCancel = () => { importVisible.value = false }

const handleExport = async () => {
  try {
    loading.value = true
    const queryParams: any = {}
    if (queryKeyword.value) queryParams.KeyWords = queryKeyword.value
    if (advancedQueryForm.value.menuName) queryParams.MenuName = advancedQueryForm.value.menuName
    if (advancedQueryForm.value.menuCode) queryParams.MenuCode = advancedQueryForm.value.menuCode
    if (advancedQueryForm.value.menuType !== undefined) queryParams.MenuType = advancedQueryForm.value.menuType
    if (advancedQueryForm.value.menuStatus !== undefined) queryParams.MenuStatus = advancedQueryForm.value.menuStatus
    const blob = await exportMenu(queryParams, menuExcelNames.sheet, menuExcelNames.fileBase)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fileName = `${menuExcelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}.xlsx`
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success'))
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.export.failed'))
  } finally {
    loading.value = false
  }
}

const handleAdvancedQuery = () => { advancedQueryVisible.value = true }
const handleAdvancedQuerySubmit = () => {
  advancedQueryVisible.value = false
}
const handleAdvancedQueryReset = () => {
  advancedQueryForm.value = emptyMenuAdvancedQueryForm()
}

const handleColumnSetting = () => { columnSettingVisible.value = true }
const handleColumnKeysChange = (keys: (string | number)[]) => {
  visibleColumnKeys.value = keys.map(k => String(k))
}
const handleColumnSettingReset = () => { visibleColumnKeys.value = [] }

const handleRefresh = () => { handleSearch() }
</script>

<style scoped lang="css">
/* 边距由子组件（takt-tree-left-* / takt-tree-right-*）统一设置，本视图不重复设置 */
.identity-menu {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
  overflow: hidden;
}

.menu-query-row {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 100%;
  flex-wrap: nowrap;
  min-width: 0;
  flex-shrink: 0;
}

.menu-toolbar-row {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 100%;
  flex-wrap: nowrap;
  min-width: 0;
  flex-shrink: 0;
}

.menu-tree-table-wrap {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: row;
  min-width: 0;
  overflow: hidden;
  align-items: stretch;
}

</style>
