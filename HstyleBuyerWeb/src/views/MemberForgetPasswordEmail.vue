<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-6">
        <div class="card mt-6">
          <div class="card-header text-center fw-bold">信箱驗證成功</div>
          <div class="card-body text-center mb-4"></div>
          <div class="text-center mx-5">
            <router-link class="returnbtn" to="/login">點此繼續使用登入</router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { onMounted } from "vue";

const vaildate=()=>{
  var url=window.location.href 
  var lastSlashIndex=url.lastIndexOf('/')
  const confirmCode= url.substring(lastSlashIndex + 1)
  console.log(confirmCode);
  url = url.slice(0, lastSlashIndex);
  lastSlashIndex = url.lastIndexOf('/')
  const account=url.substring(lastSlashIndex + 1)
  console.log(account);
 axios.get(`https://localhost:7243/api/Member/EmailConfirm?account=${account}&confirmCode=${confirmCode}`, { withCredentials: true, })
 .then(response => {
  // if(response.data==="驗證成功"){
  //       eventBus.emit("ResetPassword"); 
  //       window.location="http://localhost:5173/"}
  alert(response.data)
        })
        .catch(error => { console.log(error); });

}

onMounted(()=>{
  vaildate();
});
</script>

<style scoped>
.container {
  min-height: 550px;
}

.card {
  border: none;
}

.card-header {
  border-radius: 0%;
  background-color: transparent;
  font-size: 15pt;
}

.a {
  text-decoration: none;
  color: #000000;
}

.returnbtn:not(.nav-btns button) {
  background-color: #fff;
  color: rgb(12, 13, 12);
  padding: 10px 28px;
  border-radius: 25px;
  border: 1px solid rgb(12, 13, 12);
  transition: all 0.3s ease;
  text-decoration: none;
}

.returnbtn:not(.nav-btns button):hover {
  background-color: #000000;
  color: #fff;
  border-color: #000000;
  text-decoration: none;
}

.mt-6 {
  margin-top: 20%;
}
</style>