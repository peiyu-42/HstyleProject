<template>
  <div class="d-inline-flex p-2 bd-highlight">
    <div class="card bg-dark text-white cardImage">
      <router-link :to="'/EssaysBlog/' + data.essayId" class="card">
        <img :src="data.imgs" alt="Essay Card" />
        <!-- <div class="card-overlay">
          <h3>{{ data.etitle }}</h3>
        </div> -->
        <div class="details">
          <div class="title">{{ data.etitle }}</div>
        </div>
      </router-link>
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  data: Object,
});
</script>

<style scoped>
.details {
  position: relative;
}
.details div {
  position: absolute;
  bottom: 0;
  font-weight: bold;
  color: white;
}
.title {
  font-family: 標楷體;
  font-size: large;
  left: 50%;
  transform: translateX(-50%);
  /* padding:60px; */
  height: 300px;
}
/* overflow: hidden; */
 .card {
  position: relative;
  width: 250px;
  height: 250px;
  
  border: none;
  padding-bottom: 2%;
}

/*.card img {
  width: 100%;
  height: 150%;
  object-fit: cover;
  transition: all 0.3s ease;
} */

.card:hover img {
  filter: brightness(70%);
}
/* 元素上方創建一個半透明層 */
.card-overlay {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  text-align: center;
  color: #fff;
  opacity: 0;
  transition: all 0.3s ease;
}

.card:hover .card-overlay {
  opacity: 1;
}

/* .card-overlay h3 {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
} */
.card-overlay h3 {
  font-size: 1.2rem;
  margin-bottom: 0.5rem;
}

.card-overlay p {
  font-size: 1rem;
  margin: 0;
}
.details {
  position: relative;
}
.details div {
  position: absolute;
  bottom: 0;
  font-weight: bold;
  color: white;
}
/* .category {
  right: 180px;
  left: 190px;
  font-family: cursive;
  font-weight: bold;
  padding: 0px;
  height: 320px;
}
.title {
  font-family: 標楷體;
  font-size: large;
  left: 50%;
  transform: translateX(-50%);
  height: 300px;
}
.influencer {
  font-family: cursive;
  padding: 20px;
  height: 180px;
  left: 140px;
  right: 150px;
}
.formatDate {
  left: 80px;
  right: 85px;
  font-family: cursive;
  font-weight: bold;
  padding: 30px;
  height: 160px;
} */

/* .card {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
} */

.card img {
  width: 100%;
  max-width: 350px;
  height: auto;
  object-fit: cover;
}

.card .details {
  margin-top: 10px;
}

.card .details .title {
  font-size: 1rem; /* 调整字体大小 */
  line-height: 1.0; /* 调整行高 */
  margin-bottom: -220px;
  width:230px;
}
</style>
