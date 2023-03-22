<template>
  <div class="container m-5">
    <div class="row d-flex justify-content-center">
      <div class="col-md-12 ps-5 mb-5">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="http://localhost:5173/">Home</a></li>
            <li class="breadcrumb-item click text-secondary" @click="returnToCategory()">{{ product.pCategoryName }}</li>
            <li class="breadcrumb-item active">{{ product.product_Name }}</li>
          </ol>
        </nav>
      </div>
      <div class="col-md-7 ps-7 mb-5">
        <swiper :direction="'vertical'" :pagination="{
          clickable: true,
        }" :modules="modules" class="MySwiper">
          <swiper-slide v-for="(image, index) in product.imgs" :key="index"><img :src="image"></swiper-slide>
        </swiper>
      </div>
      <div class="col-md-5 ps-6">
        <div class="row m-0">
          <div class="col-md-12 pt-6"></div>
          <div class="col-md-12 d-flex justify-content-between p-0">
            <div v-for="(image, index) in product.imgs" :key="index" class="thumb" @click="showMultiple(index)">
              <img :src="image" class="p-0 pe-2">
            </div>
            <vue-easy-lightbox :visible="visibleRef" :imgs="imgsRef" :index="indexRef" @hide="onHide"></vue-easy-lightbox>
          </div>
          <div class="col-md-12 text-start">
            <h5 class="py-5">{{ product.product_Name }}</h5>
          </div>
          <div class="col-md-12 mb-5">
            <div>
              <div class="product-options text-start">
                <label>規格:</label>
                <select v-model="SelectSpecId" class="mx-2">
                  <option v-for="spec in product.specs" :value="spec.specId">
                    {{ `${spec.color}, ${spec.size}` }}
                  </option>
                </select>
              </div>
            </div>
          </div>
          <div class="col-md-12 pt-5"></div>
          <div class="col-md-12 text-start mt-2">
            <button @click="addItem()" class="add-to-cart"> NT$ {{ product.unitPrice > 0 ?
              product.unitPrice.toLocaleString() : 0 }}<span class="border-start  ms-2" data-bs-target="#exampleModal"><span
                  class="ps-2">加入購物車</span></span></button>
            <span class=" pl-2" v-if="!isClicked" @click="likesProduct()"><i
                class="fa-regular fa-heart icon-hover fz-18"></i></span>
            <span class="pl-2" v-else @click="likesProduct()"><i class="fa-solid fa-heart fz-18"></i></span>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="container">
    <div class="row">
      <div class="col-md-6 pb-2">
        <button class="btn-underline" @click="showComment = false" :class="{ active: !showComment }">商品描述</button>
      </div>
      <div class="col-md-6">
        <button class="btn-underline" @click="showComment = true" :class="{ active: showComment }">商品評論</button>
      </div>
      <div v-if="showComment" class="col-md-12 border-top pt-3 mb-big">
        <PComment v-if="comment.length > 0" v-for="item in comment" :data="item"></PComment>
        <div v-else class="pt-4">- 此商品無評論 -</div>
      </div>
      <div v-else class="col-md-12 border-top pt-5 px-6  mb-big">
        {{ product.description }}
      </div>
      <div class="h500px">
        <div class="col-md-12 line">
          <span class="px-5">猜你喜歡</span>
        </div>
        <div class="col-md-12">
          <div class="row">
            <RecommendCard v-for="item in rec" :data="item"></RecommendCard>
          </div>
        </div>
      </div>

    </div>
  </div>
  <Back2Top />
</template>

<script setup>
import { onMounted, ref, watch } from "vue";
import { useRoute } from "vue-router";
import axios from "axios";
import { Swiper, SwiperSlide } from "swiper/vue";
import Back2Top from "../components/Back2Top.vue";
import PComment from "../components/PComment.vue";
import RecommendCard from "../components/RecommendCard.vue";
import { eventBus } from "../mybus";

//照片放大套件
import VueEasyLightbox from 'vue-easy-lightbox'
import 'vue-easy-lightbox/external-css/vue-easy-lightbox.css'

// Import Swiper styles 輪播套件
import "swiper/css";

import "swiper/css/pagination";
// import required modules
import { Pagination } from "swiper";
import { useRouter } from "vue-router";
const router = useRouter();

//路由
const route = useRoute();
const product = ref([]);

const isClicked = ref(false);
const showComment = ref(false);

//商品呈現
const getProduct = async () => {
  await axios
    .get(`https://localhost:7243/api/Products/${route.params.id}`)
    .then((response) => {
      product.value = response.data;
      SelectSpecId.value = product.value.specs[0].specId;
      isClicked.value = likeProductsId.value.includes(
        parseInt(route.params.id)
      );

    })
    .catch((error) => {
      console.log(error);
    });
};

//照片輪播
const modules = ref([Pagination]);

//點照片放大
const visibleRef = ref(false)
const indexRef = ref(0)
const imgsRef = ref([]);
const onShow = () => {
  visibleRef.value = true
}
const showMultiple = (index) => {
  // 提取圖像 URL 並賦值給 imgsRef
  imgsRef.value = product.value.imgs.map(imgs => imgs);
  indexRef.value = index // 图片顺序索引
  onShow();
}

const onHide = () => {
  visibleRef.value = false
}
//加入購物車

const SelectSpecId = ref(0);
const addItem = async () => {
  await axios
    .post(
      `https://localhost:7243/api/Cart/${SelectSpecId.value}`,
      {},
      { withCredentials: true }
    )
    .then((response) => {
      eventBus.emit("addCartEvent");
      eventBus.emit("showCartEvent");
    })
    .catch((error) => {
      console.log(error);
      router.push("/login");
    });
};

//收藏
let likes = ref([]);
const likeProductsId = ref([]);

const likesProducts = async () => {
  await axios
    .get("https://localhost:7243/api/Products/products/likes", {
      withCredentials: true,
    })
    .then((response) => {
      if (response.data.length > 0) {
        likes.value = response.data;
        likeProductsId.value = likes.value.map((p) => {
          return p.productId;
        });
      }
    })
    .catch((error) => {
      console.log(error);
    });
};

const likesProduct = async () => {
  await axios
    .post(
      `https://localhost:7243/api/Products/product/like?product_id=${route.params.id}`,
      {},
      {
        withCredentials: true,
      }
    )
    .then((response) => {
      isClicked.value = !isClicked.value;
    })
    .catch((error) => {
      console.log(error);
      if (error.response.status === 401) {
        router.push("/login");
      }
    });
};

const rec = ref([]);
//推薦商品
const getRecommend = async () => {
  await axios
    .get(`https://localhost:7243/api/Products/ProdRec/${route.params.id}`)
    .then((response) => {
      rec.value = response.data.splice(0, 3);
      //console.log(rec.value);
    })
    .catch((error) => {
      console.log(error);
    });
};

const comment = ref([]);
//評論呈現
const getComment = async () => {
  await axios
    .get(`https://localhost:7243/api/Products/comments/${route.params.id}`)
    .then((response) => {
      comment.value = response.data;
      response.data.map((p) => {
        p.isClicked = helpfulCommentsId.value.includes(p.commentId);
      });

    })
    .catch((error) => {
      console.log(error);
    });
};
//評論點讚
let isLoaded = ref(false);
let helpfulComments = ref([]);
let helpfulCommentsId = ref([]);
const loadHelpfulComments = async () => {
  await axios.get(`https://localhost:7243/api/Products/helpfulComment/member`,
    { withCredentials: true })
    .then(response => {
      helpfulComments.value = response.data;
      helpfulCommentsId.value = helpfulComments.value.map((c) => { return c.commentId })
    })
    .catch(error => { console.log(error); });

  getComment();
}

eventBus.on("addhelpfulComments", () => {
  loadHelpfulComments();
});

const returnToCategory = () => {
  window.location = `http://localhost:5173/products/${product.value.pCategoryName}`
}

onMounted(() => {
  likesProducts();
  getProduct();
  getRecommend();
  loadHelpfulComments();
  window.scrollTo(0, 0);
});
</script>

<style scoped>
.line {
  border-top: 1px solid #dee2e6;
  text-align: center;
  line-height: 0.1em;
}

.line span {
  background: #fff;
  padding: 0 10px;
}

.px-6 {
  padding-left: 15%;
  padding-right: 15%;
}

.mb-big {
  margin-bottom: 20%;
}

.h200px {
  height: 200px;
}

.btn-underline {
  position: relative;
  padding: 0;
  border: none;
  background: none;
  text-decoration: none;
  font-size: 12pt;
  color: #333;
  cursor: pointer;
}

.pt-6 {
  padding-top: 30%;
}

.btn-underline:hover {
  color: #000000;
}

.btn-underline::after {
  content: "";
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 2px;
  background-color: #000000;
  transform: scaleX(0);
  transition: transform 0.3s ease;
}

.btn-underline:hover::after {
  transform: scaleX(1);
}

.thumb {
  width: 100px;
  height: 100px;
  margin-bottom: 10px;
  cursor: pointer;
}

.ps-8 {
  padding-right: 15%;
}

.thumb img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.ps-6 {
  padding-left: 17%;
  margin-right: 0%;
}

.MySwiper {
  width: 480px;
  height: 600px;
  overflow: hidden;
}

.MySwiper .swiper-slide img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.fz-18 {
  font-size: 20px;
  cursor: pointer;
}

.ps-7 {
  padding-left: 30%;
}

.add-to-cart {
  background-color: #fff;
  color: rgb(12, 13, 12);
  padding: 10px 28px;
  border-radius: 25px;
  border: 1px solid rgb(12, 13, 12);
  transition: all 0.3s ease;
}

.add-to-cart:hover {
  background-color: #000;
  color: #fff;
}

.h500px {
  height: 500px;
}

.product-options label {
  font-weight: bold;
  margin-right: 10px;
}

.product-options select {
  outline: none;
  padding: 5px;
  border: 1px solid #ccc;
  border-radius: 5%;
  font-size: 15px;
}

.click {
  cursor: pointer;
}

a {
  text-decoration-line: none;
  color: #000;
}
</style>
<style>
.swiper-pagination-bullet {
  background: var(--bs-gray-500) !important;
}
</style>