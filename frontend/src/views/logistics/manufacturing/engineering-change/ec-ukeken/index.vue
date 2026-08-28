<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-ukeken -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变受检部门：左栏设变明细主表，右栏本部门执行行 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    list-permission="logistics:manufacturing:engineering:change:ukeken:list"
    update-permission="logistics:manufacturing:engineering:change:ukeken:update"
    export-permission="logistics:manufacturing:engineering:change:ukeken:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.ukeken"
    id-field="ecUkekenId"
    dept-slug="ecukeken"
    :exec-code="TaktEcExecCodes.Iqc"
    :get-master-list="getEcUkekenMasterList"
    :get-line-list="getEcUkekenList"
    :update-line="updateEcUkeken"
    :export-lines="exportEcUkeken"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变受检部门列表页（左右主子表）
 */
import { getEcUkekenList, getEcUkekenMasterList, updateEcUkeken, exportEcUkeken } from '@/api/logistics/manufacturing/engineering-change/ec-ukeken'
import { TaktEcExecCodes } from '@/constants/logistics/ec-exec-codes'
import EcDeptExecLrPage from '../components/ec-dept-exec-lr-page.vue'
import EcDeptViewForm from './components/ec-dept-view-form.vue'
</script>
