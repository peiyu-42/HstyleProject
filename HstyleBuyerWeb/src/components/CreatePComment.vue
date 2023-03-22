<template>
  <div class="modal fade" id="ProductCommentModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
    aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
      <div class="modal-content px-5">
        <div class="modal-header">
          <h5 class="modal-title mt-1" id="myModalLabel">商品評論</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <form @submit.prevent="createComment" enctype="multipart/form-data" id="PCommentForm">
          <div class="modal-body text-start">
            <div class="form-group">
              <label for="productName" class="mb-2">商品名稱:
                <span class="ms-2">{{ modalData.productName }}</span></label>
            </div>
            <div class="form-group">
              <div class="star-rating">
                <label for="productRating" class="mb-2 me-2">商品評分: </label>
                <i v-for="(star, index) in 5" :key="index" :class="starClass(index)" @click="selectRating(index)"></i>
              </div>
            </div>
            <div class="form-group">
              <label for="productSize">尺寸:<span class="ms-2">{{
                modalData.productSpec
              }}</span></label>
            </div>
            <div class="form-group form-floating my-3">
              <textarea class="form-control h200px" id="v" name="CommentContent" v-model="CommentContent"
                ref="files"></textarea>
              <label for="CommentContent">商品評論</label>
            </div>
            <div class="form-group mb-2">
              <label for="productImage" class="mb-2">上傳照片</label>
              <input type="file" multiple class="form-control" accept=".png, .jpg" id="productImage" ref="files"
                @change="handleFileUpload">
              <div class="fs14">檔案大小需為小於4MB的圖檔(限.png, .jpg),限三張照片</div>
              <div v-if="errorMessage">{{ errorMessage }}</div>
            </div>
          </div>
          <div class="modal-footer mt-0 border-0">
            <button type="submit" class="btn savebtn">
              送出
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, watch } from "vue";
import Back2Top from "./Back2Top.vue";
import axios from "axios";

const props = defineProps({
  maxRating: {
    default: 5,
  },
  initialRating: {
    default: 0,
  },
  modalData: { Object },
});

const selectedRating = ref(props.initialRating);
const stars = ref(Array(props.maxRating).fill(false));

const selectRating = (index) => {
  selectedRating.value = index + 1;
};

const starClass = (index) => {
  if (index < selectedRating.value) {
    return "fa-solid fa-star";
  } else {
    return "fa-regular fa-star";
  }
};

//檔案上傳資料綁定
const CommentContent = ref('');
const files = ref([]);
const handleFileUpload = (event) => {
  files.value = event.target.files;
};

const errorMessage = ref(null);



// 檔案
const maxFileSize = 4 * 1024 * 1024; // 4MB
const createComment = async () => {
  if (selectedRating.value == 0 || selectedRating.value == null) {
    alert("請填入評分");
    return;
  }
  if (files.value.length > 2) {
    alert("超過照片張數限制");
    return;
  }
  for (let i = 0; i < files.value.length; i++) {
    if (files.value[i].size > maxFileSize) {
      alert(`檔案 ${files.value[i].name} 過大，請選擇小於 4MB 的檔案`);
      return;
    }
  }
  if (files.value != null) {
    const form = document.forms.namedItem("PCommentForm");
    const formData = new FormData(form);
    formData.append("CommentId", 0);
    formData.append("PcommentImgs", null);
    formData.append("CreatedTime", new Date().toDateString());
    formData.append('Score', selectedRating.value);
    formData.append('CommentContent', CommentContent.value);
    for (let i = 0; i < files.value.length; i++) {
      formData.append("files", files.value[i]);
    }
    axios.post(`https://localhost:7243/api/Products/comment?orderId=${props.modalData.orderId}&productId=${props.modalData.productId}`, formData,
      {
        withCredentials: true,
      })
      .then((response) => {
        alert("發表評論成功");
        window.location = "http://localhost:5173/account/orders"
      })
      .catch((error) => {
        console.log(error.response.data);
        if (error.response.status === 401) {
          router.push("/login");
        }
      });
  }
};



onMounted(() => {
  stars.value.splice(
    props.initialRating,
    props.maxRating - props.initialRating,
    true
  );
});
</script>

<style scoped>
.h200px {
  height: 200px;
}

.savebtn:not(.nav-btns button) {
  background-color: #fff;
  color: rgb(12, 13, 12);
  padding: 10px 28px;
  border-radius: 25px;
  border: 1px solid rgb(12, 13, 12);
  transition: all 0.3s ease;
}

.savebtn:not(.nav-btns button):hover {
  background-color: #000000;
  color: #fff;
  border-color: #000000;
}

.star-rating i {
  cursor: pointer;
  font-size: 15pt;
  color: rgb(255, 208, 0);
}
</style>