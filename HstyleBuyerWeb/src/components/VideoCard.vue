<template>
  <div class="col-lg-3">
    <div class="card card-deck">
      <router-link :to="'/VideoBlog/' + data.id" class="text-dark text-decoration-none">
        <div class="card border-0 card1">
          <div class="img-sz w-100 h200px rounded overflow-hidden position-relative">
                        <img class="card-img-top" :src="data.image" alt="VideoImage">
                        <div class="position-absolute bottom-0 end-0 pe-2 pb-1"><i
                                class="fa-solid fa-circle-play fz-10"></i>
                        </div>
                    </div>
          <div class="card-body">
            <div class="d-flex bg-white border-bottom-0">
              <div class="justify-content-start">
                <div class="badge bg-secondary opacity-50 me-1" v-for="tag in data.tags">
                  <span class="tag">#{{ tag }}</span>
                </div>
                <p class="card-title title over-text text-truncate" style="max-width: 270px">{{ data.title }}</p>
              </div>
            </div>
            <div class="container text-color">
              <div class="influence d-flex align-items-end">
                <div>
                  <!-- 宣傳 -->
                  <label><i class="fa-solid fa-eye fz-16"></i> {{ data.views }}</label>
                  <label class="ms-1"><i class="fa-solid fa-bookmark fz-16"></i> {{ data.likes }}</label>
                </div>
                <div>
                  <!-- 收藏 -->
                  <span v-if="!data.isClicked" @click.prevent @click="postVideoLike(data)"><i class="fa-regular fa-bookmark icon-hover fz-18"></i></span>
                  <span v-else @click.prevent @click="postVideoLike(data)"><i class="fa-solid fa-bookmark SolidHeart fz-18"></i></span>
                  <!-- <div @click.stop="" class="likeIcon d-flex flex-row-reverse"> -->
                </div>
              </div>
            </div>
          </div>
        </div>
      </router-link>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { ref, onMounted } from "vue";
import { eventBus } from "../mybus";
import { useRouter } from "vue-router";
const router = useRouter();
// const videos=ref([])
// const Likesvideos=ref([])
// const likes=ref([])
const props = defineProps({
  data: Object,
});

let likes = ref([]);
const likevideoId = ref([]);
const getLikesVideos = async () => {
  await axios.get(`https://localhost:7243/api/Video/MyLike`, { withCredentials: true }).then((response) => {
    if (response.data.length > 0) {
      likes.value = response.data;
      likevideoId.value = likes.value.map((v) => {
        return v.videoId;
      });
    }
  });
};
const postVideoLike = async (data) => {
  console.log(props.data.id);
  await axios
    .post(`https://localhost:7243/api/Video/Like/${props.data.id}`, {}, { withCredentials: true })
    .then((response) => {
      data.isClicked = !data.isClicked;
      eventBus.emit("postVideoLike");
    })
    .catch((error) => {
      console.log(error);
      if (error.response.status === 401) {
        router.push("/login");
      }
    });
};

onMounted(() => {});

eventBus.on("postVideoLike", () => {
  getLikesVideos();
});
</script>

<style scoped>
.card {
  height: 350px;
  border: none;
}

.likeIcon {
  /* color: darkgrey; */
  /* border: 1px; */
  /* font-size: 150%; */
}

.img {
  width: 90%;
  /* height: 100%; */
}

.img-sz {
    width: 450px;
    height: 300px;
    overflow: hidden;
}

.img-sz img {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.title {
  color: black;
  text-decoration: none;
  text-align: left;
}

a {
  text-decoration: none;
}

.tags {
  color: #6c757d;
  text-decoration: none;
  /* text-align: left; */
  /* left: 20px; */
}

.influence {
  /* color: darkgrey; */
  color: #212529;
  /* color: black; */
  font-size: 70%;
  text-decoration: none;
  text-align: left;
  display: flex; /* 使用flexbox布局 */
  justify-content: space-between; /* 内部两个<div>元素水平分布 */
}

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

.img-sz:hover i {
    transform: scale(1.05);
}

.SolidHeart {
  color: #46a3ff;
}

.fz-18 {
  font-size: 20px;
  cursor: pointer;
}

.fz-10 {
    font-size: 25px;
    color: white;
    opacity: 0.85;
}

.card-body {
    height: 150px;
}

.text-color {
  color: #212529;
  cursor: pointer;
}

.over-text {
  overflow: hidden;
  text-overflow: ellipsis;
  /* white-space: nowrap; */
  display: block;
}
</style>
