<!-- Home.vue -->
<template>
  <Back2Top></Back2Top>
  <div id="area1" class="area area1">
    <div class="hero-text nav-btns">
      <h1 class="text-captialize">Welcome to H'style</h1>
      <p>
        Discover the latest collections and experience the unique world of
        H'style
      </p>
      <router-link to="/products/new" class="btn mt-3 text-uppercasre"
        >shop now</router-link
      >
    </div>
  </div>

  <div id="area2" class="area area2">
    <div id="section1" class="text-center">
      <div style="z-index: 0">
        <div class="box"></div>
        <h1 class="text-captialize text-white">Welcome to H'style</h1>
        <p class="text-white">
          Discover the latest collections and experience the unique world of
          H'style
        </p>
        <router-link to="/products/針織衫" class="btn mt-3 text-uppercasre"
          >shop now</router-link
        >
      </div>
    </div>
  </div>

  <div id="area3" class="area area3">
    <div id="section1" class="text-center">
      <div class="box"></div>
      <h1 class="text-captialize">Welcome to H'style</h1>
      <p class="text-white">
        Discover the latest collections and experience the unique world of
        H'style
      </p>
      <router-link to="/products/連身裙" class="btn mt-3 text-uppercasre"
        >shop now</router-link
      >
    </div>
  </div>

  <div id="area4" class="area area4">
    <div id="section1" class="text-center">
      <div class="box"></div>
      <h1 class="text-captialize">Welcome to H'style</h1>
      <p class="text-white">
        Discover the latest collections and experience the unique world of
        H'style
      </p>
      <router-link to="/products/外套" class="btn mt-3 text-uppercasre"
        >shop now</router-link
      >
    </div>
  </div>

  <div id="area5" class="area area5">
    <div id="section1" class="text-center">
      <div class="box"></div>
      <h1 class="text-captialize">Welcome to H'style</h1>
      <p class="text-white">
        Discover the latest collections and experience the unique world of
        H'style
      </p>
      <router-link to="/products/all" class="btn mt-3 text-uppercasre"
        >shop now</router-link
      >
    </div>
  </div>
</template>
  
<script setup>
import Back2Top from "../components/Back2Top.vue";
</script>

<style scoped>
.box {
  width: 500px;
  height: 700px;
}

.area {
  width: 100%;
  height: 100vh;
  background-size: cover;
  background-position: center center;
  overflow-y: hidden;
}

.area1 {
  background-image: url(../assets/image/chanel1.jpg);
  background-attachment: fixed;
}

.area2 {
  background-image: url(../assets/image/chanel2.jpg);
  background-attachment: fixed;
}

.area3 {
  background-image: url(../assets/image/chanel3.jpg);
  background-attachment: fixed;
}

.area4 {
  background-image: url(../assets/image/chanel4.jpg);
  background-attachment: fixed;
}

.area5 {
  background-image: url(../assets/image/chanel5.jpg);
  background-attachment: fixed;
}

.myList .list-group-item {
  position: relative;
  padding: 0;
  margin: 0;
  width: 20px;
  height: 20px;
  background-color: #ffffff;
  border-radius: 50%;
  border: none;
  outline: none;
  margin-bottom: 5px;
  opacity: 0.5;
}

.myList .active {
  background-color: #1b9dc4;
  opacity: 0.7;
}

.myList .list-group-item:hover::after {
  position: absolute;
  top: -2px;
  left: -60px;
  background-color: rgb(141, 218, 241);
  content: attr(option-text);
  width: 50px;
  text-align: center;
  border-radius: 10px;
}

.title {
  text-align: center;
}

/* 文字下移 */

.hero-text {
  text-align: center;
  color: #fff;
  position: absolute;
  top: 100%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.trans1 {
  text-transform: uppercase;
  padding-top: 5px;
}

.section1 {
  text-align: center;
  color: #fff;
  position: absolute;
  top: 100%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.btn:not(.nav-btns button) {
  background-color: #fff;
  color: rgb(12, 13, 12);
  padding: 10px 28px;
  border-radius: 25px;
  border: 1px solid rgb(12, 13, 12);
  transition: all 0.3s ease-in-out 0s;
}

.btn:not(.nav-btns button):hover {
  background-color: #46a3ff;
  color: #fff;
  border-color: #46a3ff;
}
</style>