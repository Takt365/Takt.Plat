'use strict';

const fs = require('fs');
const path = require('path');

const ROOT = path.join(__dirname, '..', 'frontend', 'src');
const GIJUTSU = path.join(ROOT, 'views/logistics/manufacturing/engineering-change/ec-gijutsu');
const ATT_SRC = path.join(ROOT, 'views/logistics/manufacturing/engineering-change/ec-attachment/components');

function copyAttachmentComponents() {
  for (const name of ['ec-attachment-panel.vue', 'ec-attachment-form.vue']) {
    const src = path.join(ATT_SRC, name);
    const dest = path.join(GIJUTSU, 'components', name);
    if (!fs.existsSync(src)) {
      console.warn('skip missing', src);
      continue;
    }
    let text = fs.readFileSync(src, 'utf8');
    text = text.replace(/engineering-change\/ec-attachment/g, 'engineering-change/ec-gijutsu');
    fs.writeFileSync(dest, text);
  }
}

function writeSubPanels() {
  const content = `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：ec-sub-panels.vue -->
<!-- 功能描述：设变技术部门右栏子表面板（附件 TaktEcAttachment + 明细 TaktEcDetail） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-sub-panels flex h-full min-h-0 flex-col overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-sub-panels-tabs flex min-h-0 flex-1 flex-col"
    >
      <a-tab-pane
        key="attachment"
        :tab="t('entity.ecattachment._self')"
        force-render
      >
        <EcAttachmentPanel
          ref="attachmentPanelRef"
          class="h-full min-h-0"
        />
      </a-tab-pane>
      <a-tab-pane
        key="detail"
        :tab="t('entity.ecdetail._self')"
        force-render
      >
        <EcDetailPanel
          ref="detailPanelRef"
          class="h-full min-h-0"
        />
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变技术部门右栏：附件 + 明细子表 Tab
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'
import EcAttachmentPanel from './ec-attachment-panel.vue'
import EcDetailPanel from './ec-detail-panel.vue'

const { t } = useI18n()
/** 当前激活子表 Tab（流程：主表 → 附件 → 明细） */
const activeTab = ref('attachment')
/** 附件子表面板 ref */
const attachmentPanelRef = ref<InstanceType<typeof EcAttachmentPanel> | null>(null)
/** 明细子表面板 ref */
const detailPanelRef = ref<InstanceType<typeof EcDetailPanel> | null>(null)

/** 主表选中变更后刷新两个子表 */
function reload() {
  attachmentPanelRef.value?.reload?.()
  detailPanelRef.value?.reload?.()
}

defineExpose({ reload })
</script>

<style scoped lang="css">
:deep(.ec-sub-panels-tabs .ant-tabs-content-holder) {
  flex: 1;
  min-height: 0;
  overflow: hidden;
}

:deep(.ec-sub-panels-tabs .ant-tabs-content) {
  height: 100%;
}

:deep(.ec-sub-panels-tabs .ant-tabs-tabpane) {
  height: 100%;
  min-height: 0;
}
</style>
`;
  fs.writeFileSync(path.join(GIJUTSU, 'components/ec-sub-panels.vue'), content);
}

function patchGijutsuIndex() {
  const indexPath = path.join(GIJUTSU, 'index.vue');
  let text = fs.readFileSync(indexPath, 'utf8');
  text = text.replace(
    "import EcDetailPanel from './components/ec-detail-panel.vue'",
    "import EcSubPanels from './components/ec-sub-panels.vue'",
  );
  text = text.replace(
    `<EcDetailPanel
          ref="ecDetailPanelRef"
          class="h-full min-h-0 flex-1"
        />`,
    `<EcSubPanels
          ref="ecSubPanelsRef"
          class="h-full min-h-0 flex-1"
        />`,
  );
  text = text.replace(
    'const ecDetailPanelRef = ref<InstanceType<typeof EcDetailPanel> | null>(null)',
    'const ecSubPanelsRef = ref<InstanceType<typeof EcSubPanels> | null>(null)',
  );
  text = text.replace(
    'ecDetailPanelRef.value?.reload?.()',
    'ecSubPanelsRef.value?.reload?.()',
  );
  fs.writeFileSync(indexPath, text);
}

function patchDetailPanel() {
  const panelPath = path.join(GIJUTSU, 'components/ec-detail-panel.vue');
  let text = fs.readFileSync(panelPath, 'utf8');
  text = text.replace(/EcGijutsuDetail/g, 'EcDetail');
  fs.writeFileSync(panelPath, text);
}

function rmDir(rel) {
  const full = path.join(ROOT, rel);
  if (fs.existsSync(full)) {
    fs.rmSync(full, { recursive: true, force: true });
    console.log('removed', rel);
  }
}

copyAttachmentComponents();
writeSubPanels();
patchGijutsuIndex();
patchDetailPanel();

for (const rel of [
  'views/logistics/manufacturing/engineering-change/ec-detail',
  'views/logistics/manufacturing/engineering-change/ec-attachment',
  'views/logistics/manufacturing/engineering-change/gijutsu',
  'views/logistics/manufacturing/engineering-change/ec-gijutsu',
  'views/logistics/manufacturing/engineering-change/ec',
]) {
  rmDir(rel);
}

console.log('consolidate ec-gijutsu master-detail done');
