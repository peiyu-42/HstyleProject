<template>
  <!-- 内文解碼 -->

  <div class="col-md-3 mb-4">
    <router-link :to="'/EssaysBlog/' + data.essayId" class="text-dark text-decoration-none">
      <div class="card border-0 card1">
        <div class="card-img w-100 h200px rounded overflow-hidden">
          <img :src="data.imgs[0]" class="card-img-top" alt="Essays Image" />
        </div>

        <div class="card-header d-flex bg-white border-bottom-0">
          <span class="badge bg-secondary opacity-50 me-1" v-for="tag in data.tags">{{ tag }}</span>
        </div>
        <div class="card-body text-truncate" style="max-width: 320px">
          {{ data.etitle }}
        </div>
        <div class="card-footer bg-white border-top-0 d-flex">
          <span class="me-auto">{{ data.uplodTime.slice(0, 10) }}</span>

          <!-- <span><i class="fa-regular fa-bookmark"></i></span> -->
          <div @click.stop class="card-text text-end">
            <span v-if="!data.isClicked" @click.prevent @click="postEssayLike(data)"><i class="fa-regular fa-bookmark icon-hover fz-18"></i></span>
            <span v-else @click.prevent @click="postEssayLike(data)"><i class="fa-solid fa-bookmark SolidHeart fz-18"></i></span>
          </div>
        </div>
      </div>
    </router-link>
  </div>
</template>
<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { useRoute, useRouter } from "vue-router";
// import { eventBus } from "../mybus";
const props = defineProps({
  data: Object,
});

const route = useRoute();
const router = useRouter();

//收藏
let likes = ref([]);
const Essays = ref([]);

// const likesEssay = async () => {
//   await axios
//     .get("https://localhost:7243/api/Essay/Elike", { withCredentials: true })
//     .then((response) => {
//       if (response.data.length > 0) {
//         likes.value = response.data;
//         console.log(likes.value);
//         Essays.value = likes.value.map((e) => {
//           return e.essayId;
//         });
//       }
//     })
//     .catch((error) => {
//       console.log(error);
//     });
// };

const postEssayLike = async (data) => {
  await axios
    .post(`https://localhost:7243/api/Essay/Elike /${props.data.essayId}`, {}, { withCredentials: true })
    .then((response) => {
      data.isClicked = !data.isClicked;
    })
    .catch((error) => {
      console.log(error);
      if (error.response.status === 401) {
        router.push("/login");
      }
    });
};

onMounted(async () => {
  // getEssayInfo();
  //await likesEssay();
});
</script>
<style scoped>
.h200px {
  height: 200px;
}

.object-fit-cover {
  object-fit: cover;
}

.card1 img {
  transition: all 0.2s;
}

.card1:hover img {
  transform: scale(1.1);
}

.SolidHeart {
  color: #46a3ff;
}

.fz-18 {
  font-size: 20px;
  cursor: pointer;
}
</style>
