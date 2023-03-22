<template>
  <div class="col-lg-4 d-flex justify-content-around px-5 py-5">
    <div class="card d-flex justify-content-center align-items-center">
      <router-link :to="'/product/' + data.product_Id">
        <div class="img-sz">
          <img v-if="!isIn" @mouseover="isIn = true" :src="data.imgs[0]" class="card-img-top" alt="Product Image" />
          <img v-else @mouseout="isIn = false" :src="data.imgs[1]" class="card-img-top" alt="Product Image" />
        </div>
      </router-link>
      <div class="card-body position-relative">
        <div class="position-absolute top-0 end-0 me-2 mt-2" v-for="tag in data.tags">
          <label class="fz-9" v-if="tag === newProduct">{{ newProduct }}</label>
        </div>
        <div class="card-title fw-bold">{{ data.product_Name }}</div>
        <span>NT$ {{ data.unitPrice.toLocaleString() }}</span>
        <div class="card-text text-end">
          <span v-if="!data.isClicked"><i class="fa-regular fa-heart icon-hover fz-18"
              @click="likesProduct(data)"></i></span>
          <span v-else><i class="fa-solid fa-heart fz-18" @click="likesProduct(data)"></i></span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";
import { eventBus } from "../mybus";

const router = useRouter();
const props = defineProps({
  data: Object,
});

const likesProduct = async (data) => {
  await axios
    .post(
      `https://localhost:7243/api/Products/product/like?product_id=${props.data.product_Id}`,
      {},
      { withCredentials: true }
    )
    .then((response) => {
      data.isClicked = !data.isClicked;
      eventBus.emit("addProductLike");
    })
    .catch((error) => {
      console.log(error);
      if (error.response.status === 401) {
        router.push("/login");
      }
    });
};
//收藏

onMounted(() => { });

//圖片動畫
const isIn = ref(false);
//新品
const newProduct = ref("新品");
</script>

<style scoped>
.card-img-top {
  border: none;
  border-radius: 0%;
}

.fz-18 {
  font-size: 20px;
  cursor: pointer;
}

.fz-9 {
  font-size: 15px;
  color: rgb(116, 129, 143);
}

.card {
  border: none;
  border-radius: 0%;
  cursor: pointer;
}

.img-sz {
  width: 330px;
  height: 450px;
  overflow: hidden;
}

.img-sz img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 1s ease-in-out;
}

img:hover {
  transform: scale(1.1);
  animation: rotate 1s linear infinite;
}

img:mouseout {
  opacity: 0;
  transition-delay: 1s;
  transition-timing-function: ease-out;
}

.card-body {
  width: 100%;
}
</style>
