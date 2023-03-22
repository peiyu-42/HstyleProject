<template>
  <div class="container">
    <div class="row border-bottom mb-5 mt-4 pb-2 d-flex justify-content-evenly">
      <div class="col-md-1"><router-link to="/Blog/EssaysBlog" class="nav-link targetAll btn-underline">文章</router-link>
      </div>
      <div class="col-md-1"><router-link to="/Blog/VideoBlog" class="nav-link targetAll btn-underline">影音</router-link>
      </div>
    </div>
  </div>

  <div class="container mt-5">
    <div v-if="isLoaded" class="row">
      <div class="col-lg-8">
        <div class="row">
          <!-- plyr影片串接 -->
          <div>
            <video ref="player" id="player" class="plyr__video-embed" playsinline controls autoplay>
              <source :src="video.filePath" type="video/mp4" autoplay />
            </video>
          </div>
        </div>

        <!-- <div class="">
                    <video class="video" :src="video.filePath"></video>
                </div> -->
        <div class="row d-flex justify-content-between">
          <!-- tags -->
          <div class="col-4">
            <span class="badge bg-secondary opacity-50 me-1" v-for="tag in video.tags" :key="tag">#{{ tag }}</span>
          </div>
          <!-- 觀看收藏次數 -->
          <div class="col-4">
            <div class="videoLike">
              <label>
                <label><i class="fa-solid fa-eye fz-16"></i> {{ video.views }}</label>
                <label class="ms-1"><i class="fa-solid fa-bookmark fz-16"></i> {{ video.likes }}</label>
                <span> 收藏 </span>
                <span v-if="!isClicked" @click="postVideoLike(video.id)"><i class="fa-regular fa-bookmark icon-hover fz-18"></i></span>
                <span v-else @click="postVideoLike(video.id)"><i class="fa-solid fa-bookmark SolidHeart fz-18"></i></span>
              </label>
            </div>
          </div>
        </div>
        <div class="row">
          <!-- 影片標題 -->
          <div class="content">
            <label class="title fs-2"><strong>{{ video.title }}</strong></label>
            <p class="description fs-5">{{ video.description }}</p>
          </div>
        </div>
      </div>
      <div class="col-lg-4">
        <div class="row">
          <div class="col-lg-12">
            <form action="" class="comment">
              <div v-if="comment !== null">
                <div class="form-group commentContent" v-for="comment in videoComments" :key="comment.id">
                  <div class="row">
                    <div class="col-10">
                      <!-- 所有留言 -->
                      <div class="d-flex justify-content-start">
                        <div class="user">
                          <img src="../assets/image/user.png" alt="" />
                        </div>
                        <p>{{ comment.memberName }}說：{{ comment.comment }}</p>
                      </div>
                    </div>
                    <!-- 留言按讚 -->
                    <div class="col-1">
                      <div class="videoCommentLike">
                        <label class="">
                          <span> </span>
                          <span v-if="!comment.commentIsClicked" @click="postCommentLike(comment.id)">
                            <i class="fa-regular fa-thumbs-up fz-icon"></i>
                          </span>
                          <span v-else @click="postCommentLike(comment.id)"><i class="fa-solid fa-thumbs-up fz-icon"></i></span>
                        </label>
                      </div>
                    </div>
                  </div>
                  <div class="d-flex justify-content-end">
                    <label>{{ comment.createdTime.slice(0, 10) }}</label>
                    <!-- 按讚留言 -->
                  </div>
                </div>
              </div>
              <div v-else>目前並無留言</div>
            </form>
            <form>
              <!-- 留言板 -->
              <div class="searchDiv mb-4 mt-3">
                <!-- <label for="search-input" style="position: absolute; width: 1px; height: 1px; padding: 0; margin: -1px; overflow: hidden"> 搜尋： </label> -->
                <input type="text" id="search-input" v-model="comment" placeholder="輸入留言..." />
                <button @click.prevent="postComment()" id="searchButtom" type="submit">
                  <i class="fa-regular fa-paper-plane"></i>
                </button>
              </div>
            </form>
          </div>
          <br />
        </div>
      </div>
    </div>
  </div>
  <!-- 商品推薦 -->
  <div class="container">
    <div class="row" v-if="RecoProducts !== null">
      <div class="pt-6 pb-2">商品推薦</div>
      <div class="col-md-12 border-top">
        <div class="row h-500">
          <div class="col-lg-4 py-5 px-0" v-for="product in RecoProducts" :key="product.product_Id">
            <div class="card d-flex justify-content-center align-items-center">
              <a :href="`http://localhost:5173/product/${product.product_Id}`">
                <div class="img-sz ">
                  <img :src="product.imgs[0]" alt="Product Image" />
                </div>
              </a>
              <div class="card-body position-relative">
                <div class="card-title fw-bold">{{ product.product_Name }}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import axios from "axios";
import Plyr from "plyr";
import { slotFlagsText } from "@vue/shared";
import { eventBus } from "../mybus";

const video = ref([]);
const route = useRoute();
const RecoProducts = ref([]);
const videoComments = ref([]);

const router = useRouter();
const isLoaded = ref(false);
const isClicked = ref([]);

let likesComments = ref([]);

// const likevideoId=ref([]);

// get
//得到影片
const getVideo = async () => {
  await axios
    .get(`https://localhost:7243/api/Video/${route.params.id}`)
    .then((response) => {
      video.value = response.data;
      isClicked.value = likevideoId.value.includes(route.params.id);
      // console.log(likevideoId.value);
    })
    .catch((error) => {
      console.log(error);
    });
};

// 喜歡的評論
const likeCommentId = ref([]);
const getCommentLikes = async () => {
  await axios
    .get(`https://localhost:7243/api/Video/comment/Likes`, { withCredentials: true })
    .then((response) => {
      if (response.data.length > 0) {
        likesComments.value = response.data;
        likeCommentId.value = likesComments.value.map((likes) => {
          return likes.commentId;
        });
      }
    })
    .catch((error) => {
      console.log(error);
    });
  getComments();
};

//得到評論
const commentIsClicked = ref([]);
const getComments = async () => {
  await axios
    .get(`https://localhost:7243/api/Video/Comments/${route.params.id}`)
    .then((response) => {
      videoComments.value = response.data;
      commentIsClicked.value = response.data.map((v) => {
        v.commentIsClicked = likeCommentId.value.includes(parseInt(v.id));
        return v;
      });

      isLoaded.value = true;
    })
    .catch((error) => {
      console.log(error);
    });
};

//商品推薦
const getRecommenations = async () => {
  await axios
    .get(`https://localhost:7243/api/Video/Recommenations/${route.params.id}`)
    .then((response) => {
      RecoProducts.value = response.data;
      // isLoaded.value = true;
    })
    .catch((error) => {
      console.log(error);
    });
};

// 喜歡的video
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

// post
//點擊喜歡的影片
const postVideoLike = (videoId) => {
  axios
    .post(`https://localhost:7243/api/Video/Like/${videoId}`, {}, { withCredentials: true })
    .then((response) => {
      isClicked.value = !isClicked.value;
      eventBus.emit("postVideoLike");
      // getLikesVideos();
      // getVideo();
    })
    .catch((error) => {
      console.log(error.response.status);
      if (error.response.status === 401) {
        router.push("/login");
      }
    });
};

// 點擊喜歡的留言
// const commentId = ref(false);
const postCommentLike = (commentId) => {
  axios
    .post(`https://localhost:7243/api/Video/CommentLike/${commentId}`, {}, { withCredentials: true })
    .then((response) => {
      commentIsClicked.value = !commentIsClicked.value;
      getCommentLikes();
    })
    .catch((error) => {
      console.log(error);
      if (error.response.status === 401) {
        window.location = "http://localhost:5173/login";
      }
    });
};

const postView = () => {
  axios
    .post(`https://localhost:7243/api/Video/View/${route.params.id}`)
    .then((response) => { })
    .catch((error) => {
      console.log(error);
    });
};

// 送出留言
const comment = ref("");
const postComment = async () => {
  await axios
    .post(`https://localhost:7243/api/Video/Comment/${route.params.id}`, { comment: comment.value }, { withCredentials: true })
    .then((response) => {
      getComments();
      comment.value = "";
    })
    .catch((error) => {
      console.log(error);
      if (error.response.status === 401) {
        window.location = "http://localhost:5173/login";
      }
    });
};

onMounted(() => {
  getVideo();
  getLikesVideos();
  getCommentLikes();
  // getComments();
  getRecommenations();
  postView();
  window.scrollTo(0, 0);
});

eventBus.on("postVideoLike", () => {
  // getVideos();
  getLikesVideos();
});
</script>

<style scoped>
.video {
  width: 800px;
}

img {
  height: 140px;
}

video {
  width: 800px;
}

.pt-6 {
  padding-top: 6%;
}

.comment {
  overflow: auto;
  width: 400px;
  height: 700px;
}

.commentContent {
  border-bottom: 1px ridge;
  width: 95%;
}

.user img {
  width: 30px;
  height: 30px;
  margin-right: 5px;
}

.fz-20 {
  font-size: 20px;
}

.img-sz {
  margin-top: 5%;
  width: 250px;
  height: 300px;
  overflow: hidden;
}

.img-sz img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 1s ease-in-out;
}

img:hover {
  transform: scale(1.1);
  animation: rotate 1s linear infinite;
}


.card {
  border: none;
  border-radius: 0%;
  cursor: pointer;
}

.comment {
  height: 600px;
}

input {
  border-radius: 25px;
}

.h-500 {
  height: 600px;
}

#search-input {
  width: 400px;
  padding: 10px 10px;
  border-radius: 50px;
  border: #f2f2f2;
  background-color: #f2f2f2;
  outline: none;
}

.card-body {
  width: 100%;
}

#searchButtom {
  position: absolute;
  left: 385px;
  top: 625px;
  padding: 0px;
  background-color: #f2f2f2;
  border: none;
  border-radius: 50px;
  outline: none;
}

/* 網頁捲軸【寬度】 */
::-webkit-scrollbar {
  width: 15px;
}

/* 網頁捲軸【背景】顏色 */
::-webkit-scrollbar-track {
  /* background: #002e65; */
  background: #eeeeee;
  /* background: #000; */
}

/* 網頁捲軸【把手】顏色 */
::-webkit-scrollbar-thumb {
  background: #000;
  /* background: #eeeeee; */
}

/* 網頁捲軸【滑過時】把手的顏色 */
::-webkit-scrollbar-thumb:hover {
  /* background: #fff; */
  border-block-color: #fff;
}

/* 收藏變藍色 */
.SolidHeart {
  color: #46a3ff;
}
</style>
