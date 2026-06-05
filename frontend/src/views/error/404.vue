<template>
  <ErrorResult
    status="404"
    code="404"
    title-key="error.page.notfound.title"
    subtitle-key="error.page.notfound.subtitle"
    :show-back="true"
    :show-home="true"
  />
</template>

<script setup lang="ts">
import ErrorResult from './components/ErrorResult.vue'
</script>
