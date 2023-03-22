<template>
  <div class="d-inline-flex p-2 bd-highlight">
    <div class="card bg-dark text-white cardImage">
      <router-link :to="'/VideoBlog/' + data.videoId" class="card">
        <img :src="data.image" alt="VideoImage" />
        <div class="card-overlay">
          <h5 class="card-title">{{ data.title }}</h5>
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
.cardImage {
  position: relative;
  width: 350px;
  height: 250px;
  overflow: hidden;
  border: none;
  padding-bottom: 2%;
}
.cardImage img {
  width: 100%;
  height: 120%;
  object-fit: cover;
  transition: all 0.3s ease;
}
.cardImage:hover img {
  filter: brightness(70%);
}
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

.card-overlay h3 {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
}

.card-overlay p {
  font-size: 1rem;
  margin: 0;
}
</style>
