<template>
   <div class="modal fade" id="checkInModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog">
         <div class="modal-content">
            <div class="modal-header">
               <h1 class="modal-title fs-5" id="exampleModalLabel">每日簽到</h1>
               <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
               <div>
                  <img id="Check" v-for="i in checkIns.checkInTimes" :key="i" :src="`../src/assets/image/Checked.jpg`" alt="Image" />
                  <img id="unCheck" v-for="i in leftTimes" :key="i" :src="`../src/assets/image/Hcoin.png`" alt="Image" />
               </div>
               <div id="alertLogin" class="fs-2" style="display: none">請先登入會員</div>
            </div>
            <div id="checkButton" class="modal-footer">
               <button type="button" class="btn btn-primary" @click="checkIn()">簽到</button>
            </div>
         </div>
      </div>
   </div>

   <!-- Button trigger modal -->
   <button type="button" id="checkInAlert" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#successCheckModal" style="display: none">
      Alert 打卡成功
   </button>
   <!-- Modal -->
   <div class="modal fade" id="successCheckModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
         <div class="modal-content position-relative">
            <svg xmlns="http://www.w3.org/2000/svg" style="display: none">
               <symbol id="check-circle-fill" fill="currentColor" viewBox="0 0 16 16">
                  <path
                     d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"
                  />
               </symbol>
            </svg>
            <div class="alert alert-success d-flex align-items-center modal-body position-absolute top-50 start-50 translate-middle" role="alert">
               <svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Success:">
                  <use xlink:href="#check-circle-fill" />
               </svg>
               <div>打卡成功</div>
            </div>
         </div>
      </div>
   </div>

   <!-- Button trigger modal -->
   <button type="button" id="checkedAlert" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#CheckedModal" style="display: none">
      Alert 已打過卡
   </button>
   <!-- Modal -->
   <div class="modal fade" id="CheckedModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
         <div class="modal-content position-relative">
            <svg xmlns="http://www.w3.org/2000/svg" style="display: none">
               <symbol id="info-fill" fill="currentColor" viewBox="0 0 16 16">
                  <path
                     d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"
                  />
               </symbol>
            </svg>
            <div class="alert alert-primary d-flex align-items-center modal-body position-absolute top-50 start-50 translate-middle" role="alert">
               <svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Info:"><use xlink:href="#info-fill" /></svg>
               <div>今天打過卡囉</div>
            </div>
         </div>
      </div>
   </div>
</template>

<script setup>
// 點擊之後再做判斷
import { ref, onMounted } from "vue";
import axios from "axios";
import { eventBus } from "../mybus";
const checkIns = ref([]);
let leftTimes = 0;
eventBus.on("Login", () => {
   getCheckInfo();
});

const getCheckInfo = async () => {
   await axios
      .get(`https://localhost:7243/api/HCoin/CheckIn`, { withCredentials: true })
      .then((response) => {
         checkIns.value = response.data;
         leftTimes = 7 - checkIns.value.checkInTimes;
         document.getElementById("alertLogin").style.display = "none";
         document.getElementById("checkButton").style.display = "block";
         //  console.log(checkIns.value);
      })
      .catch((error) => {
         console.log(error.response.status);
         document.getElementById("alertLogin").style.display = "block";
         document.getElementById("checkButton").style.display = "none";
         // if (error.response.status === 401) {
         //     window.location = "http://localhost:5173/login"
         // }
      });
};

var lastTime = new Date();
const checkIn = async () => {
   lastTime = new Date(checkIns.value.lastTime).toLocaleDateString();
   let today = new Date().toLocaleDateString();
   if (lastTime === today) {
      document.getElementById("checkedAlert").click();
      // alert("今天打卡過囉!!");
   } else {
      await axios
         .put(`https://localhost:7243/api/HCoin/CheckIn`, {}, { withCredentials: true })
         .then((response) => {
            getCheckInfo();
            document.getElementById("checkInAlert").click();
            // alert("打卡成功!!!");
         })
         .catch((error) => {
            console.log(error);
         });
   }
};

onMounted(() => {
   getCheckInfo();
});
</script>

<style scoped>
/* 組件的 CSS 样式 */
#Check,
#unCheck {
   width: 14%;
}
#Check {
   width: 11%;
   margin: 5px;
}
/* 按鈕 */
.btn:not(.nav-btns button) {
   background-color: #fff;
   color: rgb(12, 13, 12);
   padding: 10px 28px;
   border-radius: 25px;
   border: 1px solid rgb(12, 13, 12);
}
.btn:not(.nav-btns button):hover {
   background-color: #46a3ff;
   color: #fff;
   border-color: #46a3ff;
}
</style>
