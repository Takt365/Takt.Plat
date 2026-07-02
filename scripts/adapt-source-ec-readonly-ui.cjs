/**
 * 一次性脚本：设变来源页只读 UI（主表：查询+导入+导出+详情；子表：查询+导入+导出）
 */
const fs = require('fs')

const indexPath = 'g:/AppDevelop/VS2026/Takt.Plat/frontend/src/views/logistics/manufacturing/engineering-change/source-ec/index.vue'
const panelPath = 'g:/AppDevelop/VS2026/Takt.Plat/frontend/src/views/logistics/manufacturing/engineering-change/source-ec/components/source-ec-detail-panel.vue'

function trimIndexTemplate(c) {
  c = c.replace(
    /    <!-- 工具栏 -->[\s\S]*?    \/>/,
    `    <!-- 工具栏 -->
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="false"
      :left-actions="toolbarLeftActions"
    />`
  )
  c = c.replace(/\s*:master-visible-column-keys="visibleColumnKeys"\n/, '\n')
  c = c.replace(
    /    <!-- 新增\/编辑对话框 -->[\s\S]*?    <\/TaktModal>\n/,
    `    <!-- 详情对话框 -->
    <TaktModal
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: t('entity.sourceec._self') })"
      width="1100px"
      wrap-class-name="takt-form-modal-resizable"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <SourceEcForm
          v-if="detailData"
          :key="detailData?.sourceEcId ?? 'detail'"
          :form-data="detailData"
          :loading="true"
        />
      </a-spin>
    </TaktModal>
`
  )
  c = c.replace(/    <!-- 高级查询抽屉 -->[\s\S]*?    <\/TaktQueryDrawer>\n\n/, '')
  c = c.replace(/    <!-- 列设置抽屉 -->[\s\S]*?    <\/TaktColumnDrawer>\n/, '')
  return c
}

function trimIndexScript(c) {
  c = c.replace(
    /import \{ message, Modal \} from 'ant-design-vue'/,
    "import { message } from 'ant-design-vue'"
  )
  c = c.replace(
    /import \{ CreateActionColumn \} from '@\/components\/business\/takt-action-column\/index'\n/,
    ''
  )
  c = c.replace(
    /import \{ getSourceEcList, getSourceEcById, createSourceEc, updateSourceEc, deleteSourceEcById, deleteSourceEcBatch, getSourceEcTemplate, importSourceEc, exportSourceEc, updateSourceEcStatus \}/,
    'import { getSourceEcList, getSourceEcById, getSourceEcTemplate, importSourceEc, exportSourceEc }'
  )
  c = c.replace(
    /import type \{ SourceEc, SourceEcQuery \}/,
    "import type { ToolBarAction } from '@/components/business/takt-tools-bar/index'\nimport type { SourceEc, SourceEcQuery }"
  )
  c = c.replace(
    /import \{ RiEditLine, RiDeleteBinLine, RiQuestionLine \} from '@remixicon\/vue'/,
    "import { RiEyeLine, RiImportLine, RiExportLine } from '@remixicon/vue'"
  )
  c = c.replace(
    /\/\*\* 新增\/编辑弹窗是否打开 \*\/[\s\S]*?const formRef = ref\(\)\n\n/,
    `/** 详情弹窗是否打开 */
const detailVisible = ref(false)
/** 详情加载中 */
const detailLoading = ref(false)
/** 详情数据（含子表） */
const detailData = ref<Partial<SourceEc> | null>(null)

`
  )
  c = c.replace(
    /\/\*\* 高级查询抽屉是否打开 \*\/[\s\S]*?const visibleColumnKeys = ref<string\[\]>\(\[\]\)\n/,
    ''
  )
  c = c.replace(
    /\/\*\* 工具栏「编辑」是否禁用[\s\S]*?const deleteDisabled = computed\(\(\) => selectedRows\.value\.length === 0\)\n\n/,
    `/** 工具栏「详情」是否禁用（须恰好选中一行） */
const detailDisabled = computed(() => selectedRows.value.length !== 1)
/** 主表工具栏：导入、导出、详情 */
const toolbarLeftActions = computed<ToolBarAction[]>(() => [
  {
    key: 'import',
    label: t('common.page.button.import'),
    icon: RiImportLine,
    permission: 'logistics:manufacturing:engineering:change:source:ec:import',
    buttonClass: 'takt-button-import',
    loading: loading.value,
    onClick: () => handleImport(),
  },
  {
    key: 'export',
    label: t('common.page.button.export'),
    icon: RiExportLine,
    permission: 'logistics:manufacturing:engineering:change:source:ec:export',
    buttonClass: 'takt-button-export',
    loading: loading.value,
    onClick: () => void handleExport(),
  },
  {
    key: 'detail',
    label: t('common.page.button.detail'),
    icon: RiEyeLine,
    permission: 'logistics:manufacturing:engineering:change:source:ec:query',
    buttonClass: 'takt-button-detail',
    disabled: detailDisabled.value,
    loading: detailLoading.value,
    onClick: () => {
      if (selectedRow.value) {
        void handleShowDetail(selectedRow.value)
      } else {
        message.warning(t('common.tip.select.to.action', {
          action: t('common.page.button.detail'),
          entity: t('entity.sourceec._self'),
        }))
      }
    },
  },
])

`
  )
  c = c.replace(
    /function buildListQuery\(overrides\?: Partial<SourceEcQuery>\): SourceEcQuery \{[\s\S]*?  return query\n\}/,
    `function buildListQuery(overrides?: Partial<SourceEcQuery>): SourceEcQuery {
  const kw = (queryKeyword.value ?? '').trim()
  const query: SourceEcQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  return query
}`
  )
  c = c.replace(
    /,\n  CreateActionColumn\(\{[\s\S]*?\}\)\n\]\)/,
    ',\n])'
  )
  c = c.replace(
    /\/\*\* 打开新增弹窗 \*\/[\s\S]*?\/\*\* 打开导入对话框 \*\//,
    `/**
 * 打开主表详情（只读表单，含子表）
 * @param record 主表行
 */
async function handleShowDetail(record: SourceEc) {
  const id = getSourceEcId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = null
  try {
    detailData.value = await getSourceEcById(id)
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    detailVisible.value = false
  } finally {
    detailLoading.value = false
  }
}

/** 关闭详情弹窗 */
function handleDetailClose() {
  detailVisible.value = false
  detailData.value = null
}

/** 打开导入对话框 */`
  )
  c = c.replace(
    /\/\*\* 删除单行 \*\/[\s\S]*?\/\*\* 打开高级查询抽屉 \*\//,
    '/** 表格 change 占位 */\nfunction handleTableChange() {}\n/** 列宽拖拽回调占位 */\nfunction handleResizeColumn() {}\n</script>\nPLACEHOLDER_END'
  )
  c = c.replace(/\/\*\* 打开高级查询抽屉 \*\/[\s\S]*?<\/script>\nPLACEHOLDER_END[\s\S]*?<\/script>\n?/, '')
  c = c.replace(/PLACEHOLDER_END[\s\S]*?<\/script>\n?/, '</script>\n')
  c = c.replace(
    /function handleReset\(\) \{\n  queryKeyword\.value = ''\n  advancedQueryForm\.value = \{[\s\S]*?  loadData\(\)\n\}/,
    `function handleReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}`
  )
  return c
}

function trimPanelTemplate(c) {
  c = c.replace(
    /    <TaktToolsBar[\s\S]*?    \/>/,
    `    <TaktToolsBar
      import-permission="logistics:manufacturing:engineering:change:source:ec:import"
      export-permission="logistics:manufacturing:engineering:change:source:ec:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-expand="false"
      :show-refresh="false"
      :show-import="true"
      :show-export="true"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
    />`
  )
  c = c.replace(/\s*:visible-column-keys="visibleColumnKeys"\n/, '\n')
  c = c.replace(/    <TaktModal[\s\S]*?<!-- 导入对话框 -->/, '    <!-- 导入对话框 -->')
  c = c.replace(/    <TaktQueryDrawer[\s\S]*?<!-- 导入对话框 -->/, '    <!-- 导入对话框 -->')
  c = c.replace(/    <TaktColumnDrawer[\s\S]*?    <\/TaktColumnDrawer>\n/, '')
  return c
}

function trimPanelScript(c) {
  c = c.replace(/import \{ message, Modal \} from 'ant-design-vue'/, "import { message } from 'ant-design-vue'")
  c = c.replace(/import \{ CreateActionColumn \} from '@\/components\/business\/takt-action-column\/index'\n/, '')
  c = c.replace(
    /import \{ RiEditLine, RiDeleteBinLine, RiQuestionLine \} from '@remixicon\/vue'\n/,
    ''
  )
  c = c.replace(/import SourceEcDetailForm from '\.\/source-ec-detail-form\.vue'\n/, '')
  c = c.replace(
    /  getSourceEcDetailList,\n  getSourceEcDetailById,\n  createSourceEcDetail,\n  updateSourceEcDetail,\n  deleteSourceEcDetailById,\n  deleteSourceEcDetailBatch,\n  getSourceEcDetailTemplate,/,
    '  getSourceEcDetailList,\n  getSourceEcDetailTemplate,'
  )
  c = c.replace(/const formVisible = ref\(false\)[\s\S]*?const formRef = ref\(\)\n\n/, '')
  c = c.replace(/const advancedQueryVisible = ref\(false\)[\s\S]*?const importVisible = ref\(false\)\n/, 'const importVisible = ref(false)\n')
  c = c.replace(/const updateDisabled = computed[\s\S]*?const deleteDisabled = computed[\s\S]*?\n\n/, '')
  c = c.replace(/,\n  CreateActionColumn\(\{[\s\S]*?\}\),\n\]\)/, ',\n])')
  c = c.replace(
    /function buildListQuery\(overrides\?: Partial<SourceEcDetailQuery>\): SourceEcDetailQuery \{[\s\S]*?  return query\n\}/,
    `function buildListQuery(overrides?: Partial<SourceEcDetailQuery>): SourceEcDetailQuery {
  const kw = (queryKeyword.value ?? '').trim()
  const query: SourceEcDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    sourceEcId: masterSourceEcId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  return query
}`
  )
  c = c.replace(/function handleCreate\(\) \{[\s\S]*?function handleRefresh\(\) \{\n  void loadData\(\)\n\}\n\n/, '')
  return c
}

let index = fs.readFileSync(indexPath, 'utf8')
index = trimIndexTemplate(index)
index = trimIndexScript(index)
fs.writeFileSync(indexPath, index)

let panel = fs.readFileSync(panelPath, 'utf8')
panel = trimPanelTemplate(panel)
panel = trimPanelScript(panel)
fs.writeFileSync(panelPath, panel)

console.log('adapt-source-ec-readonly-ui done')
