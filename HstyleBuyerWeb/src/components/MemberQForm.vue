<template>
   <!-- Modal -->
   <div class="modal fade" id="MemberQModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog">
         <div class="modal-content">
            <div class="modal-header">
               <h5 class="modal-title" id="exampleModalLabel">聯絡客服</h5>
               <button type="button" id="closeModal" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
               <div class="mb-2">- 請提供以下資料，我們的客服將盡快回復 -</div>
               <form @submit.prevent="postMemberQ" enctype="multipart/form-data" id="userForm">
                  <input type="hidden" name="customerQuestionId" class="form-control" value="0" />
                  <div class="mb-3 text-start">
                     <label for="Qcategory" class="form-label">問題類別</label>
                     <select id="Qcategory" v-model="qcategoryId" class="form-select border-bottom"
                        aria-label="Default select example" required>
                        <option v-for="(option, index) in categoryQ" :key="index" :value="option.qcategoryId">
                           {{ option.categoryName }}
                        </option>
                     </select>
                  </div>
                  <div class="mb-3 text-start">
                     <label for="Qtitle" class="form-label">提問題目</label>
                     <input type="text" id="Qtitle" v-model="title" class="form-control" placeholder="請輸入想要問的題目"
                        required />
                  </div>
                  <div class="mb-3 text-start">
                     <label for="Qcontent">提問內容</label>
                     <textarea class="form-control" id="Qcontent" v-model="problemDescription"
                        placeholder="請協助加以描述您遇到的問題..." rows="3" required></textarea>
                  </div>
                  <div class="mb-3 text-start">
                     <label for="imageFile" class="form-label">圖片上傳</label>
                     <input class="form-control" type="file" id="imageFile" accept=".png, .jpg" @change="handleUpload" />
                     <div class="fs14 mt-2">只可上傳一個檔案，且大小需小於2MB的圖檔，如果檔案大大或格式限制無法順利上傳，建議改以連結方式提供。</div>
                     <div v-if="errorMessage">{{ errorMessage }}</div>
                  </div>
                  <button type="submit" class="btn-submit">送出</button>
               </form>
               <button type="buttom" @click="putData()" class="btn btn-link text-dark text-decoration-none">填入資料</button>
            </div>
         </div>
      </div>
   </div>
   <button id="AlertModal" type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#ThanksModal"
      style="display: none">
      alertThanks
   </button>
   <AlertModal />
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import axios from "axios";
import AlertModal from "../components/AlertModal.vue";
import { useRouter } from "vue-router";
const router = useRouter();

const putData = () => {
   title.value = "訂製商品";
   problemDescription.value = "我想訂製商品可以嗎?";
};

const categoryQ = ref([]);
const getQCategoryInfo = async () => {
   await axios
      .get("https://localhost:7243/CommonQCategory")
      .then((response) => {
         categoryQ.value = response.data;
      })
      .catch((error) => {
         console.log(error);
      });
};

// 檔案
const file = ref(null);
const errorMessage = ref(null);
const handleUpload = (event) => {
   file.value = event.target.files[0];
};
watch(file, (newFile, oldFile) => {
   if (newFile) {
      const formData = new FormData();
      formData.append("file", newFile);
      const fileSize = newFile.size / 1024 / 1024; // 轉換為 MB
      if (fileSize > 4) {
         // 限制檔案大小為 4 MB
         errorMessage.value = "檔案大小超過限制";
         file.value = null;
      } else {
         errorMessage.value = null;
      }
   }
});

// 送出表單
const qcategoryId = ref([]);
const title = ref([]);
const problemDescription = ref([]);
const askTime = ref([]);
const postMemberQ = async () => {
   if (file.value) {
      const form = document.forms.namedItem("userForm");
      const formData = new FormData(form);
      formData.append("memberId", 0);
      formData.append("filePath", null);
      formData.append("qcategoryId", qcategoryId.value);
      formData.append("title", title.value);
      formData.append("problemDescription", problemDescription.value);
      formData.append("askTime", new Date().toDateString());
      formData.append("file", file.value);
      console.log(formData.get("file"));

      await axios
         .post("https://localhost:7243/MemberQ", formData, {
            // headers: {
            //    "Content-Type": "multipart/form-data",
            // },
            withCredentials: true,
         })
         .then((response) => {
            console.log("檔案上傳");
            document.getElementById("AlertModal").click();
         })
         .catch((error) => {
            console.log(error.response.status);
            // document.getElementById("#MemberQModal").closest();
            if (error.response.status === 401) {
               router.push("/login");
            }
         });
   } else {
      const form = document.forms.namedItem("userForm");
      const formData = new FormData(form);
      formData.append("memberId", 0);
      formData.append("filePath", null);
      formData.append("qcategoryId", qcategoryId.value);
      formData.append("title", title.value);
      formData.append("problemDescription", problemDescription.value);
      formData.append("askTime", new Date().toDateString());
      formData.append("file", null);
      await axios
         .post("https://localhost:7243/MemberQ", formData, { withCredentials: true })
         .then((response) => {
            console.log("資料上傳");
            document.getElementById("AlertModal").click();
         })
         .catch((error) => {
            console.log(error.response.status);
            if (error.response.status === 401) {
               router.push("/login");
               // window.location = "http://localhost:5173/login";
            }
         });
   }
};

onMounted(() => {
   getQCategoryInfo();
});
</script>

<style scoped>
.fs14 {
   font-size: 14px;
}

.btn-submit {
   background-color: #fff;
   color: rgb(12, 13, 12);
   padding: 10px 28px;
   border-radius: 25px;
   border: 1px solid rgb(12, 13, 12);
   transition: all 0.3s ease;
}

.btn-submit:hover {
   background-color: #000;
   color: #fff;
}

.form-select {
   border: none;
}

input:-webkit-autofill,
textarea:-webkit-autofill,
select:-webkit-autofill {
   -webkit-box-shadow: 0 0 0px 1000px transparent inset !important;

   background-color: transparent;

   background-image: none;

   transition: background-color 50000s ease-in-out 0s;
}

input:focus {
   box-shadow: none;
}

textarea:focus {
   box-shadow: none;
}

select:focus {
   box-shadow: none;
}
</style>
