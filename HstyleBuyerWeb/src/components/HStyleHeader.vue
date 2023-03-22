<template>
  <header class="bg-opacity-0 targetAll sticky-top" id="myHeader">
    <router-link class="logo-style targetAll" to="/">
      <link rel="preconnect" href="https://fonts.googleapis.com" />
      <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
      <link
        href="https://fonts.googleapis.com/css2?family=Roboto+Slab&display=swap"
        rel="stylesheet"
      />
      <!-- 改樣式 -->
      <h1 class="trans1">H'STYLE</h1>
    </router-link>
  </header>
  <nav class="navbar navbar-expand bg-ligt">
    <div Class="container-fluid" m-0 g-0 position-relative>
      <div class="nav inputGroup" id="app">
        <span class="icon ma-1 my-1 px-3">
          <i class="fa-solid fa-magnifying-glass search targetAll"></i>
        </span>
        <input
          class="fz14 pb-1 ps-4 textbox"
          id="search"
          type="text"
          v-model="keyword"
          @keydown.enter="onEnter"
        />
      </div>
      <div id="NavbarContent" class="nav">
        <ul class="navbar-nav">
          <li class="nav-item dropdown">
            <router-link to="/recommend" class="nav-link text-dark px-3"
              ><span class="targetAll">推薦商品</span></router-link
            >
          </li>
          <a href="#"></a>
          <li class="nav-item dropdown">
            <router-link to="/products/new" class="nav-link text-dark px-3"
              ><span class="targetAll">新品上市</span></router-link
            >
          </li>
          <li class="nav-item dropdown">
            <router-link to="/products/all" class="nav-link text-dark px-3"
              ><span class="targetAll">服飾名品</span></router-link
            >
          </li>

          <li class="nav-item dropdown">
            <a class="nav-link text-dark px-4" href="/Blog">部落格</a>
            <div class="dropdown-menu border-0 menu">
              <div class="container">
                <div class="row">
                  <!-- 文章部落格 -->
                  <div class="col-lg-12">
                    <router-link to="/Blog/EssaysBlog" class="dropdown-item"
                      >文章部落格</router-link
                    >
                  </div>
                  <!-- 影片部落格 -->
                  <div class="col-lg-12">
                    <router-link to="/Blog/VideoBlog" class="dropdown-item"
                      >影片部落格</router-link
                    >
                  </div>
                </div>
              </div>
            </div>
          </li>
        </ul>
      </div>

      <div class="nav justify-content-end ps-5">
        <div class="mx-3">
          <router-link
            to="/account/MemberCollection"
            title="喜歡"
            class="text-dark"
            ><i class="fa-regular fa-heart icon-hover fz-18"></i
          ></router-link>
        </div>
        <div class="mx-3">
          <a href="#" title="打卡" class="text-dark"
            ><i
              class="fa-regular fa-circle-check icon-hover fz-18"
              data-bs-toggle="modal"
              data-bs-target="#checkInModal"
            ></i
          ></a>
        </div>
        <div class="mx-3">
          <router-link to="/account/memberprofile" class="text-dark"
            ><i class="fa-regular fa-circle-user icon-hover"></i
          ></router-link>
        </div>
        <div class="mx-3">
          <a href="#" title="購物車" class="text-dark">
            <i
              class="fa-solid fa-cart-shopping icon-hover position-relative"
              data-bs-toggle="modal"
              data-bs-target="#cartModal"
              ><span
                class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger font-monospace px-1"
                >{{ cartItemsNum }}</span
              ></i
            >
          </a>
        </div>
      </div>
    </div>
  </nav>
  <Cart @CartCount="UpdateCartCount" />
  <CheckIn />
</template>
<script setup>
import Cart from "./Cart.vue";
import CheckIn from "./CheckIn.vue";
import { ref, onMounted, watch, watchEffect, nextTick } from "vue";
import { eventBus } from "../mybus";

const cartItemsNum = ref();
const UpdateCartCount = (CartCount) => {
  //$('#exampleModal').modal('show');
  cartItemsNum.value = CartCount;
};

const showCart = () => {
  $("#cartModal").modal("show");
};
eventBus.on("showCartEvent", showCart);

const keyword = ref(null);

const onEnter = () => {
  eventBus.emit("search", keyword);
};

onMounted(() => {
  window.addEventListener("scroll", function () {
    var header = document.querySelector("header");
    header.classList.toggle("sticky", window.scrollY > 0);
  });
  document.querySelector(".search").addEventListener("click", function () {
    document.querySelector("#search").focus();
  });
});
</script>
<style scoped>
h1.trans1 {
  font-family: "Roboto Slab", serif;
  text-align: center;
}

.badge {
  font-size: 5px;
}

.search {
  cursor: pointer;
}

.fz-18 {
  font-size: 18px;
}

.icon-hover:hover {
  color: #46a3ff;
}

.logo-style {
  text-decoration: none;
  color: black;
}

.targetAll:hover {
  color: #46a3ff;
}

.link {
  text-decoration-line: none;
}

.nav-item::after {
  content: "";
  position: absolute;
  width: 100%;
  height: 2px;
  bottom: -2px;
  left: 0;
  background-color: #46a3ff;
  visibility: hidden;
  transform: scaleX(0);
  transition: all 0.3s ease-in-out 0s;
}

.nav-item:hover::after {
  visibility: visible;
  transform: scaleX(1);
}

.textbox {
  border: none;
  border-bottom: 1px solid transparent;
  outline: none;
  font-size: 16px;
  transition: border-bottom-color 0.5s ease-in-out;
}

.textbox:focus {
  border-bottom-color: #ced4da;
}

header {
  background-color: white;
  color: rgb(12, 13, 12);
  opacity: 0.9;
  /* 內距 */
  padding-top: 5px;
  padding-bottom: 5px;
  text-align: center;
}

header:hover {
  background-color: black;
  transition: 0.5s;
}

header.sticky {
  padding: 5px 100px;
  background: #fff;
}

header .sticky ul li a {
  color: #000;
}

/* 懸停選單的樣式 */
.dropdown-menu {
  position: absolute;
  top: 100%;
  left: 0;
  z-index: 1000;
  display: none;
  min-width: 100%;
  padding: 0.5rem 0;
  margin: 0.125rem 0 0;
  font-size: 1rem;
  color: #212529;
  text-align: left;
  list-style: none;
  background-color: #fff;
  opacity: 0.85;
}

input:-webkit-autofill,
textarea:-webkit-autofill,
select:-webkit-autofill {
  -webkit-box-shadow: 0 0 0px 1000px transparent inset !important;

  background-color: transparent;

  background-image: none;

  transition: background-color 50000s ease-in-out 0s;
}

.dropdown-menu.show {
  display: block;
}

.dropdown-item {
  display: block;
  width: 100%;
  clear: both;
  font-weight: 400;
  color: #212529;
  white-space: nowrap;
  background-color: transparent;
  border: 0;
}

.dropdown-item:hover,
.dropdown-item:focus {
  color: #46a3ff;
}

.dropdown-item.active,
.dropdown-item:active {
  color: #46a3ff;
}

.nav-item:hover .dropdown-menu {
  display: block;
}

.menu {
  min-width: 100%;
  min-height: 21%;
  border: none;
  background-color: #ffffff;
  border-radius: 0%;
}
</style>