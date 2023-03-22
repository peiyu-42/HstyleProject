<!-- Recommend.vue -->
<template>
  <div class="container mt-5 mb-7">
    <div class="row">
      <div class="col-md-12 line">
        <span class="px-5 fs-5 fw-bold">今日推薦</span>
      </div>
    </div>
  </div>
  <div>
    <div id="carouselExampleIndicators" class="carousel slide mb-6 mt-2 vh-100" data-ride="carousel">
      <div class="carousel-inner">
        <div v-for="(item, index) in rec" :key="index" :class="{ active: index === 0 }" class="carousel-item"
          data-bs-interval="5000">
          <div class="row justify-content-center">
            <div class="col-md-2"></div>
            <div class="col-md-3 img-container-lg img-sz">
              <router-link :to="'/product/' + item.product_Id">
                <img :src="item.imgs[0]" alt="Large Image" />
              </router-link>
            </div>
            <div class="col-md-3 mt-5">
              <div class="row">
                <div class="col-md-6 img-container-sm1 img-sz ms-5 position-absolute top-0 start-0">
                  <router-link :to="'/product/' + item.product_Id">
                    <img :src="item.imgs[1]" alt="Small Image" />
                  </router-link>
                </div>
                <div class="col-md-12 img-container-sm2 img-sz position-absolute bottom-0 end-0 top-50">
                  <router-link :to="'/product/' + item.product_Id">
                    <img :src="item.imgs[2]" alt="Small Image" />
                  </router-link>
                </div>
                <div class="col-md-12 position-absolute bottom-0 end-0 text-end fs-5 fw-light">
                  {{ item.product_Name }}
                </div>
              </div>
            </div>
            <div class="col-md-3 mt-5">
              <div class="row">
                <div class="col-md-2">
                  <label>最高溫</label>
                  <div class="fs-1 mb-5">{{ temp[1] }}</div>
                  <label>最低溫</label>
                  <div class="fs-1">{{ temp[0] }}</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <a class="carousel-control-prev" href="#carouselExampleIndicators" role="button" data-slide="prev">
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="sr-only">Previous</span>
      </a>
      <a class="carousel-control-next" href="#carouselExampleIndicators" role="button" data-slide="next">
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="sr-only">Next</span>
      </a>
    </div>
  </div>

  <div class="container">
    <div class="row">
      <div class="h500px">
        <div class="col-md-12">
          <span v-if="orec.length > 0" class="px-5">| 會員專屬推薦 |</span>
          <span v-else class="px-5">| 新品推薦 |</span>
        </div>
        <div class="col-md-12">
          <div class="row">
            <RecommendCard v-for="item in orec" :data="item" v-if="orec.length > 0"></RecommendCard>
            <RecommendCard v-for="item in newrec" :data="item" v-else></RecommendCard>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
  
<script setup>
import { onMounted, ref, watchEffect } from "vue";
import axios from "axios";
import RecommendCard from "../components/RecommendCard.vue";

const rec = ref([]);
const isLoadEnd = ref(false);
//推薦商品
const getWeatherRecommend = async () => {
  await axios
    .get(`https://localhost:7243/api/Weather/weatherRec`)
    .then((response) => {
      rec.value = response.data;
      //console.log(rec.value);
      isLoadEnd.value = true;
    })
    .catch((error) => {
      console.log(error);
    });
};

const orec = ref([]);
//推薦商品
const getOrderRecommend = async () => {
  await axios
    .get(`https://localhost:7243/api/Products/MemberRec`, {
      withCredentials: true,
    })
    .then((response) => {
      orec.value = response.data;
    })
    .catch((error) => {
      console.log(error);
    });
};

const newrec = ref([]);
const newProductsRecommend = async () => {
  await axios
    .get(`https://localhost:7243/api/Products/NewRec`)
    .then((response) => {
      newrec.value = response.data;
    })
    .catch((error) => {
      console.log(error);
    });
};

const modules = ref([Navigation]);

const temp = ref([]);
const getWeather = async () => {
  await axios
    .get(`https://localhost:7243/api/Weather/weather`)
    .then((response) => {
      temp.value = response.data;
    })
    .catch((error) => {
      console.log(error);
    });
};

const windowscroll = () => {
  const myDiv = document.querySelector("#carouselExampleIndicators");
  let isScrollingDown = false;
  let lastScrollPosition = 0;
  let now = new Date();
  let hour = now.getHours();

  window.addEventListener("scroll", function () {
    const scrollHeight = window.scrollY;
    isScrollingDown = scrollHeight > lastScrollPosition;
    lastScrollPosition = scrollHeight;

    if (isScrollingDown && scrollHeight >= 200 && hour >= 6 && hour < 18) {
      myDiv.classList.add("bg-color-day");
    } else {
      myDiv.classList.remove("bg-color-day");
    }

    if (isScrollingDown && scrollHeight >= 200 && hour >= 18 || hour < 6) {
      myDiv.classList.add("bg-color-night");
    } else {
      myDiv.classList.remove("bg-color-night");
    }
  });
};

onMounted(() => {
  getWeatherRecommend();
  getOrderRecommend();
  newProductsRecommend();
  windowscroll();
  getWeather();
});
</script>

<style scoped>
.vh-100 {
  height: 100vh;
}

.bg-color-night {
  background-image: linear-gradient(to top, #031124 0%, #3c3969 100%);
  height: 100vh;
  transition: background-color 10s ease-in-out;
  color: white;
  background-attachment: fixed;
}

.bg-color-day {
  background-image: linear-gradient(to top, #ffffff 0%, #e9feff 100%);
  height: 100vh;
  transition: background-color 10s ease-in-out;
  color: #757474;
  background-attachment: fixed;
}

.h500px {
  height: 650px;
}

.line {
  border-top: 1px solid #dee2e6;
  text-align: center;
  line-height: 0.1em;
}

.line span {
  background: #fff;
  padding: 0 10px;
}

.fz15 {
  font-size: 15pt;
  text-align: left;
}

.mb-7 {
  margin-bottom: 5%;
}

.mb-6 {
  margin-bottom: 8%;
}

.px-10 {
  padding-left: 10%;
  padding-right: 10%;
}

.img-container-lg {
  width: 550px;
  height: 800px;
  overflow: hidden;
  margin-top: 2%;
}

.img-container-lg img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 1.0s ease-in-out;
}

.img-container-sm1 {
  width: 350px;
  height: 450px;
}

.img-container-sm1 img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 1.0s ease-in-out;
}

.img-container-sm2 {
  width: 320px;
  height: 320px;
  overflow: hidden;
  margin-top: 3%;
  margin-left: 10%;
}

.img-container-sm2 img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 1.0s ease-in-out;
}

.img-sz :hover {
  transform: scale(1.05);
  animation: rotate 1s linear infinite;

}

.img-sz :mouseout {
  opacity: 0;
  transition-delay: 1s;
  transition-timing-function: ease-out;
}


.ps-6 {
  padding-left: 10%;
}
</style>