<script setup>
import HStyleHeader from './components/HStyleHeader.vue'
import HStyleFooter from './components/HStyleFooter.vue'
import { ref } from 'vue'


</script>

<template>
  <div id="app">
    <HStyleHeader />
    <router-view />
    <HStyleFooter />
  </div>
</template>

<style scoped>
#app {
  text-align: center;
  color: #2c3e50;

}
</style>
