<template>
  <ErrorResult
    status="403"
    code="403"
    title-key="error.page.forbidden.title"
    subtitle-key="error.page.forbidden.subtitle"
    :show-home="true"
  />
</template>

<script setup lang="ts">
import ErrorResult from './components/ErrorResult.vue'
</script>
