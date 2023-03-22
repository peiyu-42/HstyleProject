<template>
   <div class="card border-0 border-bottom pb-3 mb-3 container">
      <div class="row d-flex justify-content-between mt-2">
         <div class="col-md-4">
            <div class="row text-start">
               <div class="col-md-4 mb-4">{{ data.account }}</div>
               <div class="col-md-4 star-rating"><i v-for="(star, index) in data.score" class="fa-solid fa-star"></i></div>
               <div class="fz-comment col-md-12">{{ data.commentContent }}</div>
               <div class="col-md-12 my-2"></div>
               <div class="col-md-7 mt-4">購買規格: Size {{ data.size }} | Color {{ data.color }}</div>
               <div class="col-md-5 mt-4">{{ data.createdTime }}</div>
            </div>
         </div>
         <div class="col-md-2"></div>
         <div class="col-md-3 img-comment mx-2" v-if="data.pcommentImgs && data.pcommentImgs.length === 1">
            <img :src="data.pcommentImgs[0]" alt="..." @click.prevent="showSingle()" />
            <vue-easy-lightbox :visible="visibleRef" :imgs="imgsRef" :index="indexRef" @hide="onHide"></vue-easy-lightbox>
         </div>
         <div class="col-md-4 ms-5 ps-5" v-else-if="data.pcommentImgs && data.pcommentImgs.length > 1">
            <div class="d-flex justify-content-center">
               <div class="imgs-comment mx-2" v-for="(item, index) in data.pcommentImgs" :key="index">
                  <img :src="item" alt="..." @click.prevent="showMultiple(index)" />
                  <vue-easy-lightbox :visible="visibleRef" :imgs="imgsRef" :index="indexRef" @hide="onHide"></vue-easy-lightbox>
               </div>
            </div>
         </div>
         <div class="col-md-3 text-end" v-else>
            <!-- 如果沒有照片 -->
         </div>
         <div class="col-md-1 text-end">
            <i v-if="!data.isClicked" class="fa-regular fa-thumbs-up fz-icon" @click="helpfulComment()"></i>
            <i v-else="data.isClicked" class="fa-solid fa-thumbs-up fz-icon" @click="helpfulComment()"></i>
            <span class="ps-1">{{ num }}</span>
         </div>
      </div>
   </div>
</template>
<script setup>
import { onMounted, ref } from "vue";
import axios from "axios";
import { eventBus } from "../mybus";

//照片放大套件
import VueEasyLightbox from "vue-easy-lightbox";
import "vue-easy-lightbox/external-css/vue-easy-lightbox.css";

const isClicked = ref(false);

const helpfulComment = async () => {
   await axios
      .post(`https://localhost:7243/api/Products/helpfulComment?comment_id=${props.data.commentId}`, {}, { withCredentials: true })
      .then((response) => {
         props.data.isClicked = !props.data.isClicked;
         eventBus.emit("addhelpfulComments");
      })
      .catch((error) => {
         console.log(error);
         if (error.response.status === 401) {
            window.location.href = "http://localhost:5173/login";
         }
      });
};

const props = defineProps({
   data: Object,
});

//點照片放大
const visibleRef = ref(false);
const indexRef = ref(0);
const imgsRef = ref([]);
const onShow = () => {
   visibleRef.value = true;
};
const showSingle = () => {
   imgsRef.value = props.data.pcommentImgs[0];
   onShow();
};
const showMultiple = (index) => {
   // 提取圖像 URL 並賦值給 imgsRef
   imgsRef.value = props.data.pcommentImgs.map((imgs) => imgs);
   console.log(imgsRef.value);
   indexRef.value = index; // 图片顺序索引
   onShow();
};

const onHide = () => {
   visibleRef.value = false;
};

const num = ref();
const allHelpfulComment = async () => {
   await axios
      .get(`https://localhost:7243/api/Products/AllhelpfulComments?commentId=${props.data.commentId}`, {}, { withCredentials: true })
      .then((response) => {
         num.value = response.data.length;
      })
      .catch((error) => {
         console.log(error);
      });
};

eventBus.on("addhelpfulComments", () => {
   allHelpfulComment();
});

onMounted(() => {
   allHelpfulComment();
});
</script>
<style scoped>
.ps-6 {
   padding-left: 8%;
}

.fz-comment {
   font-size: 18px;
   height: 100%;
}

.fz-icon {
   font-size: 20px;
   cursor: pointer;
}

.fz-6 {
   font-size: 15px;
}

.card-text {
   font-size: 15px;
}

.card {
   width: 100%;
}

.star-rating i {
   font-size: 12pt;
   color: rgb(255, 208, 0);
   padding-top: 5px;
}

.img-comment {
   width: 150px;
   height: 150px;
   padding: 0;
   cursor: pointer;
}

.img-comment img {
   width: 100%;
   height: 100%;
   object-fit: cover;
}

.imgs-comment {
   width: 125px;
   height: 125px;
   padding: 0;
   cursor: pointer;
}

.imgs-comment img {
   width: 100%;
   height: 100%;
   object-fit: cover;
}
</style>
