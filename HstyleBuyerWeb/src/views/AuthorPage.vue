<template><p>this is auther page</p></template>

<script setup>
import { useRoute } from "vue-router";
const route = useRoute();
alert(route.params.authorName);
</script>

<style></style>
