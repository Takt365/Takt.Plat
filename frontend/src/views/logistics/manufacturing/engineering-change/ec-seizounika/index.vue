<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-seizounika -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变制造二课：页签一为采购F+仓库C003（设变+上阶物料），页签二为其它，均须填写 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex flex-col h-full min-h-0">
    <a-tabs
      v-model:active-key="activeTab"
      destroy-inactive-tab-pane
      class="flex min-h-0 flex-1 flex-col [&_.ant-tabs-content]:h-full [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-tabpane]:h-full"
    >
      <a-tab-pane :key="TAB_C003" :tab="t('logistics.manufacturing.engineering-change.ec-seizounika.page.tabs.c003')">
        <EcDeptExecLrPage
          list-permission="logistics:manufacturing:engineering:change:seizounika:list"
          update-permission="logistics:manufacturing:engineering:change:seizounika:update"
          export-permission="logistics:manufacturing:engineering:change:seizounika:export"
          menu-i18n-key="menu.logistics.manufacturing.engineering.change.seizounika"
          id-field="ecSeizounikaId"
          dept-slug="ecseizounika"
          :exec-code="TaktEcExecCodes.Pcba"
          :extra-query="{ pcbaTab: TAB_C003 }"
          :get-master-list="getEcSeizounikaMasterList"
          :get-line-list="getEcSeizounikaList"
          :update-line="updateEcSeizounika"
          :export-lines="exportEcSeizounika"
          :form-component="EcDeptViewForm"
        />
      </a-tab-pane>
      <a-tab-pane :key="TAB_OTHER" :tab="t('logistics.manufacturing.engineering-change.ec-seizounika.page.tabs.other')">
        <EcDeptExecLrPage
          list-permission="logistics:manufacturing:engineering:change:seizounika:list"
          update-permission="logistics:manufacturing:engineering:change:seizounika:update"
          export-permission="logistics:manufacturing:engineering:change:seizounika:export"
          menu-i18n-key="menu.logistics.manufacturing.engineering.change.seizounika"
          id-field="ecSeizounikaId"
          dept-slug="ecseizounika"
          :exec-code="TaktEcExecCodes.Pcba"
          :extra-query="{ pcbaTab: TAB_OTHER }"
          :get-master-list="getEcSeizounikaMasterList"
          :get-line-list="getEcSeizounikaList"
          :update-line="updateEcSeizounika"
          :export-lines="exportEcSeizounika"
          :form-component="EcDeptViewForm"
        />
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变制造二课列表页（C003 页签 / 其它页签）
 */
import { useI18n } from 'vue-i18n'
import { getEcSeizounikaList, getEcSeizounikaMasterList, updateEcSeizounika, exportEcSeizounika } from '@/api/logistics/manufacturing/engineering-change/ec-seizounika'
import { TaktEcExecCodes } from '@/constants/logistics/ec-exec-codes'
import EcDeptExecLrPage from '../components/ec-dept-exec-lr-page.vue'
import EcDeptViewForm from './components/ec-dept-view-form.vue'

const { t } = useI18n()

/** 采购 F 且仓库 C003 */
const TAB_C003 = 1
/** 其它（非 F+C003） */
const TAB_OTHER = 2
/** 当前页签 */
const activeTab = ref(TAB_C003)
</script>
