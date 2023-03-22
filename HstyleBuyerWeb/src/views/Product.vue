<!-- Product.vue -->
<template>
   <div class="m-5 container">
      <div class="row">
         <div class="col-lg-5 border-bottom">
            <div class="d-flex justify-content-start">
               <div class="filter">
                  <div class="dropdown-toggle" id="sortDropdown" data-bs-toggle="dropdown" aria-expanded="false">排序</div>
                  <ul class="dropdown-menu menu border-0 mt-1" aria-labelledby="sortDropdown">
                     <li>
                        <a class="dropdown-item" href="#" v-for="(option, index) in sortOptions" :key="index"
                           @click="setSortOption(option)">{{ option }}
                           <i class="fa-solid fa-check ps-1 fs-7"
                              v-if="selectedSortOption === option && selectedSortOption !== null && selectedSortOption !== ''"></i></a>
                     </li>
                  </ul>
               </div>
               <div class="filter">
                  <div class="dropdown-toggle" id="categoryDropdown" data-bs-toggle="dropdown" aria-expanded="false">類別
                  </div>
                  <ul class="dropdown-menu menu border-0 mt-1" aria-labelledby="categoryDropdown">
                     <li>
                        <router-link v-for="(option, index) in categoryOptions" :key="index"
                           :to="option === '全部' ? '/products/all' : `/products/${option}`" class="dropdown-item">{{ option
                           }}<i class="fa-solid fa-check ps-1 fs-7" v-if="selectedCatoOption === option"></i></router-link>
                     </li>
                  </ul>
               </div>
               <div class="filter">
                  <div class="dropdown-toggle" id="colorDropdown" data-bs-toggle="dropdown" aria-expanded="false">顏色</div>
                  <ul class="dropdown-menu menu border-0 mt-1" aria-labelledby="colorDropdown">
                     <div class="row">
                        <li class="col-md-4">
                           <a class="dropdown-item item me-3" href="#"
                              v-for="(option, index) in colorOptions.slice(0, Math.ceil(colorOptions.length / 2))"
                              :key="index" @click="setColorOption(option)">{{ option
                              }}<i class="fa-solid fa-check ps-1 fs-7"
                                 v-if="selectedColorOption === option && selectedColorOption !== null"></i>
                           </a>
                        </li>
                        <li class="col-md-3">
                           <a class="dropdown-item item" href="#"
                              v-for="(option, index) in colorOptions.slice(Math.ceil(colorOptions.length / 2))"
                              :key="index" @click="setColorOption(option)">{{ option }}<i
                                 class="fa-solid fa-check ps-1 fs-7" v-if="selectedColorOption === option"></i>
                           </a>
                        </li>
                     </div>
                  </ul>
               </div>
               <router-link to="/products/all" class="clear">
                  <div @click="setOriginalOption()">清除篩選</div>
               </router-link>
            </div>
         </div>
      </div>
   </div>

   <div class="container">
      <div v-if="route.params.tag == 'new'" class="fs-5">| 新品 |</div>
      <div v-else-if="route.params.tag == 'all'" class="fs-5">| 全部商品 |</div>
      <div v-else class="fs-5">| {{ route.params.tag }} |</div>
      <div class="row">
         <ProductCard v-if="filteredProducts.length > 0" v-for="item in filteredProducts" :data="item" />
         <div v-else class="h-1000 col-md-12">- 無此項目的商品 -</div>
      </div>
   </div>

   <Back2Top />
</template>

<script setup>
import Back2Top from "../components/Back2Top.vue";
import ProductCard from "../components/ProductCard.vue";
import { useRoute } from "vue-router";
import axios from "axios";
import { ref, onMounted, watch, computed } from "vue";
import { eventBus } from "../mybus";
//商品預覽
const products = ref([]);
const productsResetOrder = ref([]);
const route = useRoute();
//console.log(route.params.tag);

//商品篩選排序
let selectedSortOption = ref();
let selectedColorOption = ref();
let selectedCatoOption = ref();
const categoryOptions = ref([]);
const colorOptions = ref([]);
const sortOptions = ref(["新到舊", "舊到新", "價格高到低", "價格低到高"]);
//商品收藏
const likeProductsId = ref([]);
let likes = ref([]);
const likesProducts = async () => {
   await axios
      .get("https://localhost:7243/api/Products/products/likes", {
         withCredentials: true,
      })
      .then((response) => {
         if (response.data.length > 0) {
            //console.log(response.data);
            likes.value = response.data;
            likeProductsId.value = likes.value.map((p) => {
               return p.productId;
            });
            loadProducts();
         }
         loadProducts();
      })
      .catch((error) => {
         console.log(error);
         loadProducts();
      });
};
const loadProducts = async () => {
   await axios
      .get(`https://localhost:7243/api/Products/products`, {
         withCredentials: true,
      })
      .then((response) => {
         //console.log(response.data);
         response.data.map((p) => {
            p.isClicked = likeProductsId.value.includes(p.product_Id);
         });

         //把篩選的先列出
         categoryOptions.value = ["全部", ...new Set(response.data.map((p) => p.pCategoryName))];
         colorOptions.value = Array.from(new Set(response.data.flatMap((p) => p.specs.map((s) => s.color))));

         //console.log(colorOptions.value);
         if (route.params.tag == "new") {
            products.value = response.data.filter((p) => p.tags.includes("新品"));
         } else if (route.params.tag == "all") {
            products.value = response.data;
         } else {
            products.value = response.data.filter((p) => p.pCategoryName.includes(route.params.tag));
         }
      })
      .catch((error) => {
         console.log(error);
      });
};

const categoryFilter = ref(null);
const colorFilter = ref(null);
const filteredProducts = computed(() => {
   let filtered = products.value;

   if (colorFilter.value !== null) {
      filtered = filtered.filter((p) => p.specs.some((spec) => spec.color === colorFilter.value));
   }

   return filtered;
});

//還原
const setOriginalOption = () => {
   colorFilter.value = null;
   setSortOption("新到舊");
   selectedColorOption.value = null;
   selectedSortOption.value = null;
};

//顏色
const setColorOption = (option) => {
   colorFilter.value = option;
   selectedColorOption.value = option;
};

//排序
const setSortOption = (option) => {
   // sort the products based on the selected option
   if (option === "新到舊") {
      filteredProducts.value.sort((a, b) => a.displayOrder - b.displayOrder);
   } else if (option === "舊到新") {
      filteredProducts.value.sort((a, b) => b.displayOrder - a.displayOrder);
   } else if (option === "價格高到低") {
      filteredProducts.value.sort((a, b) => b.unitPrice - a.unitPrice);
   } else if (option === "價格低到高") {
      filteredProducts.value.sort((a, b) => a.unitPrice - b.unitPrice);
   }
   selectedSortOption.value = option;
};

watch(
   () => route.params.tag,
   (newTag, oldTag) => {
      loadProducts();
   }
);

const loadProductsWithKeyWord = async (keyword) => {
   const obj = { name: keyword };
   const rawValue = obj.name._rawValue;
   await axios
      .get(`https://localhost:7243/api/Products/products?keyword=${encodeURIComponent(rawValue)}`, {
         withCredentials: true,
      })
      .then((response) => {
         //console.log(response.data);
         response.data.map((p) => {
            p.isClicked = likeProductsId.value.includes(p.product_Id);
         });

         //把篩選的先列出
         categoryOptions.value = ["全部", ...new Set(response.data.map((p) => p.pCategoryName))];
         colorOptions.value = Array.from(new Set(response.data.flatMap((p) => p.specs.map((s) => s.color))));

         //console.log(colorOptions.value);
         if (route.params.tag == "new") {
            products.value = response.data.filter((p) => p.tags.includes("新品"));
         } else if (route.params.tag == "all") {
            products.value = response.data;
         } else {
            products.value = response.data.filter((p) => p.pCategoryName.includes(route.params.tag));
         }
      })
      .catch((error) => {
         console.log(error);
      });
};

eventBus.on("addProductLike", () => {
   likesProducts();
});

eventBus.on("search", (keyword) => {
   loadProductsWithKeyWord(keyword);
});

onMounted(() => {
   likesProducts();
});
</script>
<style scoped>
.filter {
   margin-right: 15%;
   padding-bottom: 0.5%;
   font-size: 12pt;
}

.menu {
   border: none;
   background-color: #ffffff;
}

.dropdown-menu a:hover {
   background-color: transparent;
   color: #46a3ff;
}

.dropdown-toggle {
   cursor: pointer;
}

.fs-7 {
   font-size: 12px;
}

.item {
   margin-right: 10%;
}

.h-1000 {
   height: 658px;
}

.clear {
   cursor: pointer;
   text-decoration: none;
   color: black;
}
</style>
