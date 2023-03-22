<!-- Account.vue -->
<template>
    <div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-2">
                    <MemberSidebar></MemberSidebar>
                </div>
                <div class="col-md-10">
                    <router-view></router-view>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script setup>

import MemberSidebar from '../components/MemberSidebar.vue';

</script>

<style></style>