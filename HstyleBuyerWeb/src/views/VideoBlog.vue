<template>
  <div class="container">
    <div class="row border-bottom mb-5 mt-4 pb-2 d-flex justify-content-evenly">
      <link rel="preconnect" href="https://fonts.googleapis.com" />
      <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
      <link
        href="https://fonts.googleapis.com/css2?family=Bona+Nova:ital@1&display=swap"
        rel="stylesheet"
      />
      <!-- 改樣式 -->
      <h1 class="title">Video</h1>
      <div class="col-md-1">
        <router-link
          to="/Blog/EssaysBlog"
          class="nav-link targetAll btn-underline"
          >文章</router-link
        >
      </div>
      <div class="col-md-1">
        <router-link
          to="/Blog/VideoBlog"
          class="nav-link targetAll btn-underline"
          >影音</router-link
        >
      </div>
    </div>
  </div>
  <div class="searchDiv mb-4 ps-4">
    <!-- <label for="search-input" style="position: absolute; width: 1px; height: 1px; padding: 0; margin: -1px; overflow: hidden"> 搜尋： </label> -->
    <input type="text" id="search-input" v-model="keyword" placeholder="搜尋" />
    <button
      @click="searchVideosByIndex(keyword)"
      id="searchButtom"
      type="submit"
    >
      <i class="fa-solid fa-magnifying-glass"></i>
    </button>
  </div>
  <div class="">
    <div class="container">
      <div class="row">
        <VideoCard v-for="video in videos" :data="video" />
      </div>
    </div>
  </div>
</template>

<script setup>
import VideoCard from "../components/VideoCard.vue";
import axios from "axios";
import { ref, onMounted } from "vue";
import { eventBus } from "../mybus";

//const isLoaded=ref(false);
const videos = ref([]);
const Likesvideos = ref([]);
//const likeVideosId=ref([]);
//影片收藏
let likes = ref([]); //會員喜歡的影片
const likevideoId = ref([]); //會員喜歡的影片Id
const getLikesVideos = async () => {
  await axios
    .get(`https://localhost:7243/api/Video/MyLike`, { withCredentials: true })
    .then((response) => {
      if (response.data.length > 0) {
        likes.value = response.data;
        likevideoId.value = likes.value.map((v) => {
          return v.videoId;
        });
        getVideos();
        // console.log( videos.value);
      }
    })
    .catch((error) => {
      console.log(error);
    });
};

const getVideos = async () => {
  await axios
    .get(`https://localhost:7243/api/Video`)
    .then((response) => {
      response.data.map((v) => {
        v.isClicked = likevideoId.value.includes(v.id);
      });
      videos.value = response.data;
      //console.log(videos.value);
      //isLoaded.value=true;
    })
    .catch((error) => {
      console.log(error);
    });
};

// 搜尋
const keyword = ref("");
const updateSelectVideos = ref([]);
const selectedVideos = ref({});
const searchVideosByIndex = (keyword) => {
  if (keyword === "") {
    getVideos();
  } else {
    selectedVideos.value = videos.value.filter(
      (v) => v.title.includes(keyword) || v.tags.includes(keyword)
    );
    videos.value = selectedVideos.value;
  }
};

const postVideoLike = (data) => {
  axios
    .post(
      `https://localhost:7243/api/Video/Like/${props.data.id}`,
      {},
      { withCredentials: true }
    )
    .then((response) => {
      data.isClicked = !data.isClicked;
      eventBus.emit("postVideoLike");
      getVideos();
    })
    .catch((error) => {
      console.log(error);
      if (error.response.status === 401) {
        router.push("/login");
      }
    });
};

onMounted(() => {
  getVideos();
  getLikesVideos();
  searchVideosByIndex();
  window.scrollTo(0, 0);
});

eventBus.on("postVideoLike", () => {
  getLikesVideos();
});
</script>

<style scoped>
h1.title {
  margin-bottom: 20px;
  font-family: "Bona Nova", serif;
}
.col-md-1 {
  margin-top: 20px;
}

#search-input {
  width: 200px;
  padding: 10px 10px;
  border-radius: 50px;
  border: #f2f2f2;
  background-color: #f2f2f2;
  outline: none;
}

#searchButtom {
  position: relative;
  left: -40px;
  padding: 10px;
  background-color: #f2f2f2;
  border: none;
  border-radius: 50px;
  outline: none;
}

.searchDiv {
  /* left:50px; */
}
</style>
