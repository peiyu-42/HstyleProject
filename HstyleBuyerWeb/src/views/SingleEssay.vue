<template>
  <div class="container mb-5">
    <div class="column">
      <img :src="essays.imgs === undefined ? '' : essays.imgs[0]" alt="Image" />
    </div>
  </div>

  <div class="column">
    <div class="fashion">{{ essays.categoryName }}</div>
    <div class="text">{{ essays.etitle }}</div>
    <div class="Who">By {{ essays.influencerName }}</div>
    <!-- <div class="Who">
      By
      <router-link :to="`/Blog/EssaysBlog/${essays.influencerName}`">{{
        essays.influencerName
      }}</router-link>
    </div> -->
    <div class="Time">{{ formatDate(essays.uplodTime) }}</div>
    <hr />

    <div style="height: 100%; overflow-y: scroll">
      <!-- 內容 -->
    </div>
    <div class="row">
      <div class="col-2">
        <h5 class="recommendation">相關商品</h5>
        <div style="position: sticky; top: 2%; transform: translate(-10%, 1%)">
          <div
            class="card d-flex justify-content-center align-items-center"
            v-for="product in RecoProducts"
            :key="product.product_Id"
          >
            <a :href="`http://localhost:5173/product/${product.product_Id}`">
              <div class="img-sz">
                <img
                  :src="product.imgs[0]"
                  class="card-img-top"
                  alt="推薦商品圖片"
                />
              </div>
            </a>
            <div class="card-body position-relative">
              <div class="card-title fw-bold">
                {{ product.product_Name }}
              </div>
            </div>
            <!-- <span>$NT {{ product.unitPrice }}</span> -->
          </div>
        </div>
      </div>

      <div class="col-8">
        <div
          v-html="decodeURI(essays.econtent)"
          class="container-text mx-auto h-100"
        ></div>
      </div>
    </div>
  </div>

  <div class="comments-container">
    <form action="">
      <div class="form-group">
        <label for="comment-input" class="px-5">留言：</label>
        <input
          type="text"
          id="comment-input"
          class="form-control"
          placeholder="輸入留言..."
          v-model="comment"
        />
        <button
          class="btn btn-primary"
          type="button"
          @click.prevent="postComment()"
        >
          送出
        </button>
      </div>
    </form>
    <div class="comments-container">
      <div class="row comments">
        <div class="" v-for="comment in essayComments" :key="comment.commentId">
          <div
            class="row"
            style="background-color: #f8f8f8; padding-top: 10px; margin: 10px"
          >
            <div class="col-1">
              <img class="avatar" src="../assets/image/user.png" alt="avatar" />
            </div>
            <div class="col-7 d-flex justify-content-start">
              <div class="row">
                <div class="col-8">
                  <h5 class="comment=user">{{ comment.memberName }}</h5>
                </div>
                <div class="col-12">
                  <p class="comment=text">{{ comment.ecomment }}</p>
                </div>
              </div>
              <!-- <div class="row">
                        
                     </div> -->
            </div>
            <div class="col-4">
              <!-- <div class="comment-actions"> -->
              <i
                v-if="!comment.commentIsClicked"
                class="far fa-thumbs-up like-btn"
                @click="postCommentLike(comment.commentId)"
              ></i>
              <i
                v-else
                class="fas fa-thumbs-up like-btn"
                @click="postCommentLike(comment.commentId)"
              ></i>
              <span class="likes-count">{{ comment.likes }}</span>
              <span class="comment-date">{{ comment.etime.slice(0, 10) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRoute } from "vue-router";
import { ref, onMounted } from "vue";
// import { useRoute, useRouter } from "vue-router";
import axios from "axios";
// const router = useRouter();
const route = useRoute();
//console.log(route.params.id);
const isLoaded = ref(false);
const essays = ref({});
//得到essay
const getEssayInfo = async () => {
  await axios
    .get(`https://localhost:7243/api/Essay/${route.params.id}`)
    .then((response) => {
      essays.value = response.data;
      //console.log(essays.value);
    })
    .catch((error) => {
      console.log(error);
    });
};
const RecoProducts = ref([]);

// 喜歡的評論
let likesComments = ref([]);
const likeCommentId = ref([]);
const getCommentLikes = async () => {
  await axios
    .get(`https://localhost:7243/api/Essay/comment/Likes`, {
      withCredentials: true,
    })
    .then((response) => {
      if (response.data.length > 0) {
        likesComments.value = response.data;
        likeCommentId.value = likesComments.value.map((likes) => {
          return likes.commentId;
        });
      }
      console.log("like comment");
      console.log(likeCommentId.value);
    })
    .catch((error) => {
      console.log(error);
    });
  getComments();
};
//得到評論的值
const commentIsClicked = ref([]);
const getComments = async () => {
  await axios
    .get(`https://localhost:7243/api/Essay/Comments/${route.params.id}`)
    .then((response) => {
      essayComments.value = response.data;
      commentIsClicked.value = response.data.map((v) => {
        v.commentIsClicked = likeCommentId.value.includes(
          parseInt(v.commentId)
        );
        return v;
      });
      // essayComments.value = response.data;
      isLoaded.value = true;
      //console.log(commentIsClicked.value);
      console.log("評論");
      console.log(essayComments.value);
    })
    .catch((error) => {
      console.log(error);
    });
};

const essayComments = ref([]);

//商品推薦
const getEssayRecommenations = async () => {
  await axios
    .get(`https://localhost:7243/api/Essay/Recommenations/${route.params.id}`)
    .then((axiosResponse) => {
      RecoProducts.value = axiosResponse.data;
      //console.log();
      //isLoded.value = true;
    })
    .catch((error) => {
      console.log(error);
    });
};

//評論按贊
// const postEssayCommentLikes = async () => {
//   await axios
//     .post(`https://localhost:7243/api/Essay/comment/Likes/${commentId}`, {
//       withCredentials: true,
//     })
//     .then((response) => {
//       commentIsClicked.value = !commentIsClicked.value;
//       getCommentLikes();
//     })
//     .catch((error) => {
//       console.log(error);
//       if (error.response.status === 410) {
//         window.location = "http://localhost:5173/login";
//       }
//     });
// };

// 點擊喜歡的留言
// const commentId = ref(false);
const postCommentLike = (commentId) => {
  console.log(commentId);
  console.log(essayComments.value);
  axios
    .post(
      `https://localhost:7243/api/Essay/CommentLike/${commentId}`,
      {},
      { withCredentials: true }
    )
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
//寫留言
const comment = ref("");
const postComment = async () => {
  await axios
    .post(
      `https://localhost:7243/api/Essay/Comment/${route.params.id}`,
      { comment: comment.value },
      { withCredentials: true }
    )
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
onMounted(async () => {
  getEssayInfo();
  // getComments();
  getCommentLikes();
  getEssayRecommenations();
  window.scrollTo(0, 0);
});
const formatDate = (dateString) => {
  const date = new Date(dateString);
  const year = date.getFullYear();
  const month = date.getMonth() + 1;
  const day = date.getDate();
  return `${year}年${month}月${day}日`;
};
</script>

<style scoped>
.recommendation {
  position: sticky;
  top: 0;
  z-index: 1;
  display: flex;
  align-items: center;
  font-family: cursive;
  margin-left: -45px; /* 將margin調整到和字體左對齊 */
  padding: 10px;
}

.recommendation::before,
.recommendation::after {
  content: "";
  flex: 1;
  border-top: 1px solid #ccc;
  margin: 0 10px; /* 調整間距 */
}

.recommendation h1 {
  text-decoration: underline;
  margin: 0;
}
.h500px {
  height: 500px;
  margin-top: 50px;
  background-color: #f2f2f2;
  padding: 20px;
}

.h5000px {
  height: 500px;
  margin-top: 50px;
  padding: 20px;
}

.line {
  /* 一條直綫 */
  /* border-top: 1px solid black; */
  margin-bottom: 20px solid black;
  padding-top: 10px;
}

.line span {
  font-size: 24px;
  font-weight: bold;
}

.line {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.line:before,
.line:after {
  content: "";
  flex: 1;
  border-top: 1px solid black;
  margin: 0 20px;
}

.allComment {
  z-index: 99;
  /* 將z-index設為1 */
  position: absolute;
}

.container {
  align-items: center;
  display: flex;
  position: relative;
}

.column {
  flex: 1;
  padding: 60px;
  position: relative;
}

.column:first-child {
  flex: none;
  width: 33.33%;
}

img {
  display: block;
  width: 150%;
  margin-bottom: 10px;
}

.text {
  font-size: 25px;
  font-family: 標楷體;
  font-weight: bold;
  text-align: center;
  position: absolute;
  top: -15em;
  left: 70%;
  transform: translate(-50%, -50%);
  z-index: 1;
}

.fashion {
  font-family: cursive;
  font-size: 14px;
  font-weight: bold;
  color: #333;
  text-transform: uppercase;
  padding: 4px 8px;
  border-radius: 4px;
  cursor: pointer;
  transition: font-size 2s, text-decoration 2s;
  position: absolute;
  top: -33em;
  left: 70%;
  transform: translateX(-50%);
}

.container-text {
  width: 905px;
  height: 1212px;
  /* margin-left: 20px; */
  text-align: justify;
  /* 上下文對齊 */
}

.fashion:hover {
  /* transition: font-size 0.2s ease, text-decoration 0.2s ease; */
  font-size: 15px;
  text-decoration: underline;
}

.Who,
.Time {
  font-size: 9px;
  font-weight: bold;
  color: #333;
  font-family: cursive;
}

.Who {
  position: absolute;
  margin-top: 8px;
 
  left: 70%;
  transform: translateX(-50%); 
  top: -18em;
  font-size: 18px;
}

.Time {
  position: absolute;
  top: -16em;
  left: 70%;
  transform: translateX(-50%);
  font-size: 18px;
}

.wrapper {
  padding: 0 20%;
}

.content {
  display: flex;
  text-align: justify;
  /* 上下文對齊 */
  text-justify: inter-word;
}

#topButton {
  display: none;
  position: fixed;
  bottom: 10px;
  right: 10px;
  z-index: 99;
  background-color: grey;
  padding: 5px;
  border-radius: 50%;
}

.btn-underline::after {
  content: "";
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 2px;
  background-color: #000000;
  transform: scaleX(0);
  transition: transform 0.3s ease;
}

.btn-underline:hover::after {
  transform: scaleX(1);
}

.btn-underline {
  position: relative;
  padding: 0;
  border: none;
  background: none;
  text-decoration: none;
  font-size: 12pt;
  color: #333;
  cursor: pointer;
}

.btn-underline:hover {
  color: #000000;
}

a {
  text-decoration: none;
}

.card-img-top {
  border: none;
  border-radius: 0%;
}

.fz-18 {
  font-size: 20px;
  cursor: pointer;
}

.fz-9 {
  font-size: 15px;
  color: rgb(116, 129, 143);
}

.card {
  border: none;
  border-radius: 0%;
  cursor: pointer;
}

.img-sz {
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

.img-sz :hover {
  transform: scale(1.1);
  animation: rotate 1s linear infinite;
}

.img-sz :mouseout {
  opacity: 0;
  transition-delay: 1s;
  transition-timing-function: ease-out;
}

.card-body {
  width: 100%;
}

/* 留言 */
.comments {
  max-width: 800px;
  margin: 0 auto;
  max-height: 400px; /* 設定最大高度 */
  overflow-y: scroll;
}

.input-group {
  margin-top: 16px;
  margin-bottom: 32px;
  left: 100px;
}

.avatar {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  margin-right: 16px;
}

.comment-section {
  /* margin-bottom: 32px; */
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

/* 
.comment {
  display: flex;
  margin-bottom: 16px;
}

.comment-body {
  background-color: #f5f5f5;
  padding: 16px;
  border-radius: 8px;
}

.comment-user {
  font-size: 1.2rem;
  margin-bottom: 8px;
}

.comment-text {
  margin-bottom: 8px;
}

.comment-actions {
  display: flex;
  align-items: center;
}

.likes-count {
  margin-left: 8px;
}

.comment-date {
  margin-left: auto;
  font-size: 0.8rem;
} */

.comment {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
  margin-bottom: 20px;
  padding: 10px;
  border-radius: 5px;
  background-color: #f8f8f8;
}

.comment-body {
  margin-left: 10px;
}

.comment-user {
  margin-bottom: 5px;
  font-weight: bold;
}

.comment-text {
  margin-top: 30px;
  margin-bottom: 5px;
  margin-left: 60px;
}

.comment-actions {
  display: flex;
  align-items: center;
  position: absolute;
  /* top: 0; */
  right: 50%;
  left: 800px;
  justify-content: space-between;
}

.likes-count {
  margin-left: 5px;
  margin-right: 10px;
}

.comment-date {
  font-size: 0.8em;
  color: grey;
}
/* 留言test 2 */
.comments-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.form-group {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.form-group label {
  font-size: 1.2rem;
}

.form-control {
  flex: 1;
  margin-right: 10px;
}

.btn-primary {
  font-size: 1.2rem;
}

.comment {
  /* display: flex; */
  /* align-items: flex-start; */
  margin-bottom: 20px;
}

.avatar {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  margin-right: 10px;
}

.comment-body {
  display: flex;
  flex-direction: column;
}

.comment-user {
  font-size: 1.2rem;
  font-weight: bold;
  margin-bottom: 10px;
}

.comment-text {
  margin-bottom: 10px;
  margin-left: 60px;
}

.comment-actions {
  display: flex;
  align-items: center;
  margin-top: 5px;
}

.like-btn {
  font-size: 1.2rem;
  margin-right: 5px;
  color: grey;
  cursor: pointer;
}

.like-btn:hover {
  color: #007bff;
}

.likes-count {
  margin-right: 10px;
  color: grey;
}

.comment-date {
  font-size: 0.8rem;
  color: grey;
  margin-left: auto;
}
</style>
