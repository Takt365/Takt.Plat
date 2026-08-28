<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-koubai -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变采购部门：左栏设变明细主表，右栏本部门执行行 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    list-permission="logistics:manufacturing:engineering:change:koubai:list"
    update-permission="logistics:manufacturing:engineering:change:koubai:update"
    export-permission="logistics:manufacturing:engineering:change:koubai:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.koubai"
    id-field="ecKoubaiId"
    dept-slug="eckoubai"
    :exec-code="TaktEcExecCodes.Mp"
    :get-master-list="getEcKoubaiMasterList"
    :get-line-list="getEcKoubaiList"
    :update-line="updateEcKoubai"
    :export-lines="exportEcKoubai"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变采购部门列表页（左右主子表）
 */
import { getEcKoubaiList, getEcKoubaiMasterList, updateEcKoubai, exportEcKoubai } from '@/api/logistics/manufacturing/engineering-change/ec-koubai'
import { TaktEcExecCodes } from '@/constants/logistics/ec-exec-codes'
import EcDeptExecLrPage from '../components/ec-dept-exec-lr-page.vue'
import EcDeptViewForm from './components/ec-dept-view-form.vue'
</script>
