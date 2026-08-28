<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-seizouikka -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变制造一课：左栏设变明细主表，右栏本部门执行行 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    list-permission="logistics:manufacturing:engineering:change:seizouikka:list"
    update-permission="logistics:manufacturing:engineering:change:seizouikka:update"
    export-permission="logistics:manufacturing:engineering:change:seizouikka:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.seizouikka"
    id-field="ecSeizouikkaId"
    dept-slug="ecseizouikka"
    :exec-code="TaktEcExecCodes.Assy"
    :get-master-list="getEcSeizouikkaMasterList"
    :get-line-list="getEcSeizouikkaList"
    :update-line="updateEcSeizouikka"
    :export-lines="exportEcSeizouikka"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变制造一课列表页（左右主子表）
 */
import { getEcSeizouikkaList, getEcSeizouikkaMasterList, updateEcSeizouikka, exportEcSeizouikka } from '@/api/logistics/manufacturing/engineering-change/ec-seizouikka'
import { TaktEcExecCodes } from '@/constants/logistics/ec-exec-codes'
import EcDeptExecLrPage from '../components/ec-dept-exec-lr-page.vue'
import EcDeptViewForm from './components/ec-dept-view-form.vue'
</script>
