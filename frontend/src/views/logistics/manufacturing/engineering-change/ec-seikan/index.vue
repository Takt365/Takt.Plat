<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-seikan -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变生管部门：左栏设变明细主表，右栏本部门执行行 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    list-permission="logistics:manufacturing:engineering:change:seikan:list"
    update-permission="logistics:manufacturing:engineering:change:seikan:update"
    export-permission="logistics:manufacturing:engineering:change:seikan:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.seikan"
    id-field="ecSeikanId"
    dept-slug="ecseikan"
    :exec-code="TaktEcExecCodes.Pmc"
    :get-master-list="getEcSeikanMasterList"
    :get-line-list="getEcSeikanList"
    :update-line="updateEcSeikan"
    :export-lines="exportEcSeikan"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变生管部门列表页（左右主子表）
 */
import { getEcSeikanList, getEcSeikanMasterList, updateEcSeikan, exportEcSeikan } from '@/api/logistics/manufacturing/engineering-change/ec-seikan'
import { TaktEcExecCodes } from '@/constants/logistics/ec-exec-codes'
import EcDeptExecLrPage from '../components/ec-dept-exec-lr-page.vue'
import EcDeptViewForm from './components/ec-dept-view-form.vue'
</script>
