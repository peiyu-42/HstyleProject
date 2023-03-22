<template>
  <ul class="nav nav-tabs mt-5">
    <li class="nav-item">
      <a class="nav-link" :class="{ active: currentTab === 'Product' }" aria-current="page" href="#"
        @click.prevent="currentTab = 'Product'">商品收藏</a>
    </li>
    <li class="nav-item">
      <a class="nav-link" :class="{ active: currentTab === 'Essay' }" href="#"
        @click.prevent="currentTab = 'Essay'">文章收藏</a>
    </li>
    <li class="nav-item">
      <a class="nav-link" :class="{ active: currentTab === 'Video' }" href="#"
        @click.prevent="currentTab = 'Video'">影片收藏</a>
    </li>
  </ul>
  <div v-if="currentTab === 'Product'" class="mt-4 container-fluid">
    <div class="row">
      <ProductCollection v-for="item in likes" :data="item" />
    </div>
  </div>
  <div v-if="currentTab === 'Essay'">
    <EssayCollection v-for="item in essaylikes" :data="item" />
  </div>
  <div v-if="currentTab === 'Video'">
    <VideoCollection v-for="item in vlikes" :data="item" />
  </div>
  <Back2Top />
</template>

<script setup>
import ProductCollection from "./ProductCollection.vue";
import VideoCollection from "./VideoCollection.vue";
import Back2Top from "./Back2Top.vue";
import axios from "axios";
import { ref, onMounted, watch } from "vue";
import { eventBus } from "../mybus";
import EssayCollection from "./EssayCollection.vue";

const currentTab = ref("Product");
//收藏
let likes = ref([]);
const likesProducts = async () => {
  await axios
    .get("https://localhost:7243/api/Products/products/likes", {
      withCredentials: true,
    })
    .then((response) => {
      likes.value = response.data;
      //console.log(response.data);
    })
    .catch((error) => {
      console.log(error);
    });
};

const essaylikes = ref([]);
const likesEssay = async () => {
  await axios
    .get("https://localhost:7243/api/Essay/Elike", { withCredentials: true })
    .then((response) => {
      essaylikes.value = response.data;

      //console.log(essaylikes.value);
    })
    .catch((error) => {
      console.log(error);
    });
};

// 影片
let vlikes = ref([]);
const likesVideos = async () => {
  await axios
    .get("https://localhost:7243/api/Video/MyLike", {
      withCredentials: true,
    })
    .then((response) => {
      vlikes.value = response.data;
      //console.log(response.data);
    })
    .catch((error) => {
      console.log(error);
    });
};

onMounted(() => {
  likesProducts();
  likesEssay();
  likesVideos();
});
</script>
<style scoped>
.nav-link {
  color: darkgrey;
}
</style>
