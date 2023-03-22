<template>
   <!-- Modal -->
   <div class="modal fade" id="CustomerQModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog">
         <div class="modal-content">
            <div class="modal-header">
               <h5 class="modal-title" id="exampleModalLabel">提交</h5>
               <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
               <div class="mb-4">抱歉該篇文章沒能解決您的問題，如果您願意，請與我們分享缺乏的部分，我們將努力使說明內容更為完善。</div>
               <form @submit.prevent="postCustomerQ">
                  <div class="mb-3 text-start">
                     <label for="Qcategory" class="form-label">問題類別</label>
                     <select id="Qcategory" v-model="qcategoryId" class="form-select  border-bottom"
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
                     <textarea class="form-control" id="Qcontent" v-model="problemDescription" rows="3"
                        placeholder="請輸入問題內容..." required></textarea>
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
import { ref, onMounted } from "vue";
import axios from "axios";
import AlertModal from "../components/AlertModal.vue";

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

const qcategoryId = ref([]);
const title = ref([]);
const problemDescription = ref([]);
const askTime = ref([]);
const postCustomerQ = async () => {
   await axios
      .post("https://localhost:7243/CustomerQ", {
         memberId: null,
         qcategoryId: qcategoryId.value,
         title: title.value,
         problemDescription: problemDescription.value,
         filePath: null,
         file: null,
         askTime: new Date(),
      })
      .then((response) => {
         // console.log(response.data);
         document.getElementById("AlertModal").click();
         // window.location = "http://localhost:5173/Questions";
      })
      .catch((error) => {
         console.log(error);
      });
};

onMounted(() => {
   getQCategoryInfo();
});
</script>

<style scoped>
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
