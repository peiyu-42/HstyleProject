<template>
  <div class="container mt-5">
    <div class="row">
      <div class="col-md-12 border-bottom text-start">
        <h5 class="fw-bold">會員資料</h5>
      </div>
      <div class="col-md-12 text-end">
        <button class="btn btn-link text-dark text-decoration-none fw-bold" type="button" data-bs-toggle="collapse"
          data-bs-target="#collapseExample999" aria-expanded="false" aria-controls="collapseExample">
          變更密碼<i class="fa-regular fa-pen-to-square ps-2"></i>
        </button>
        <div class="collapse text-start mb-5" id="collapseExample999">
          <div class="card card-body">
            <form>
              <div class="mb-3 d-flex justify-content-between w-100">
                <div class="col-md-4"> <label for="exampleInputEmail1" class="form-label fw-bold">* 原始密碼：</label>
                  <input type="account" class="textbox_member pe-6" placeholder="請輸入原始密碼" v-model="oldPassword" required>
                </div>
                <div class="col-md-4"><label for="exampleInputPassword1" class="form-label fw-bold">* 新密碼：</label>
                  <input type="password" class="textbox_member " placeholder="請輸入新密碼" v-model="newPassword" required>
                </div>
                <div class="col-md-4">
                  <label for="exampleInputPassword1" class="form-label fw-bold">* 確認新密碼：</label>
                  <input type="password" class="textbox_member" placeholder="請確認新密碼" v-model="newPassword2" required>
                </div>
              </div>
              <div class="text-center mt-4"><button type="button" class="editbtn" @click="ResetPassword1">更新密碼</button>
              </div>

            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="container mt-3 h-500">
    <div class="row">
      <div class="col-md-12 text-start ">
        <label for="validationDefault01" class="form-label fw-bold">* 姓名：</label>
        <input type="text" class="textbox_member pe-6" id="validationDefault01" v-model="member.name" required>
        <label for="validationDefault02" class="form-label fw-bold">* 手機號碼：</label>
        <input type="text" class="textbox_member pe-6" id="validationDefault02" v-model="member.phoneNumber" required>
        <div class="mt-5 text-start textbox">
          <label for="validationDefaultUsername" class="form-label fw-bold">* 地址：</label>
          <input type="text" class="textbox_member pe-12" id="validationDefaultUsername" v-model="member.address"
            required>
        </div>
        <div class="mt-5 text-start ms-1">
          <label for="validationDefault03" class="form-label fw-bold">郵箱：
            <input type="text" class="textbox_member pe-3" v-model="member.email" readonly required></label>
        </div>
        <div class="mt-5 text-start  ms-1">
          <label for="validationDefault03" class="form-label fw-bold ps-2">生日：
            <input type="text" class="textbox_member pe-3 me-5" v-model="member.birthday" readonly required></label>
          <label for="validationDefault03" class="form-label fw-bold ps-2">性別：
            <input type="text" class="textbox_member pe-2" v-model="member.gender" readonly required></label>
        </div>
        <div class="mt-5 text-start  ms-1">
          <img src="../assets/image/Hcoin.png" class="sz-10" />
          <label for="validationDefault03" class="form-label fw-bold"> H幣的總額：</label>
          <input type="text" class="textbox_readonly" v-model="member.totalH" readonly required>
        </div>
      </div>
      <div class="col-12 mt-5">
        <div class="form-check my-3">
          <input class="form-check-input" type="checkbox" value="" id="invalidCheck2" required>
          <label class="form-check-label" for="invalidCheck2">
            同意條款和條件
          </label>
        </div>
        <div class="col-12">
          <button class="editbtn" type="button" @click="EditMember">更新會員資料</button>
        </div>
      </div>
    </div>
  </div>
</template>
    
<script setup>

import { onMounted, ref } from 'vue';
import axios from 'axios';
import { eventBus } from "../mybus";

const isClicked = false;



const member = ref({});
const name = ref("");
const phoneNumber = ref("");
const address = ref("");
const gender = ref("");
const birthday = ref("");
const Resetaccount = ref("");
const password = ref("");
const EncryptedPassword = ref("");
const oldPassword = ref("");
const newPassword = ref("");
const newPassword2 = ref("");
const getMemberInfo = async () => {
  await axios.get("https://localhost:7243/api/Member", { withCredentials: true, })
    .then(response => {
      member.value = response.data;
      //console.log(member.value)
    })
    .catch(error => { console.log(error); });
}

eventBus.on("ResetPassword", () => {
  getMemberInfo();

});

const EditMember = () => {
  console.log(member.value)
  axios.post('https://localhost:7243/api/Member/EditMember', {
    name: member.value.name,
    phoneNumber: member.value.phoneNumber,
    address: member.value.address,
    // gender: member.value.gender,
  }, { withCredentials: true })
    .then((response) => {
      console.log(response.data)
      alert("更新成功")
      if (!response.ok) {
        alert(err.data)
      } else {
        console.log(response.data)
        window.location = "http://localhost:5173/account/MemberProfile";
      }

    }).catch((err) => {
      console.log(err)
    })
};


const ResetPassword1 = () => {
  axios.post(`https://localhost:7243/api/Member/ResetPassword?oldPassword=${oldPassword.value}&newPassword=${newPassword.value}&newPassword2=${newPassword2.value}`, {},
    { withCredentials: true })
    .then((response) => {
      alert(response.data)

      if (response.data === "修改成功") {
        eventBus.emit("ResetPassword");
        window.location = "http://localhost:5173/"
      }
    }).catch((err) => {

    })
};

onMounted(() => {
  getMemberInfo();
});


</script>
<style scoped>
h1 {
  text-align: left;
  text-indent: 20px
}

.w-100 input {
  width: 100%;
}

.editbtn:not(.nav-btns button) {
  background-color: #fff;
  color: rgb(12, 13, 12);
  padding: 10px 28px;
  border-radius: 25px;
  border: 1px solid rgb(12, 13, 12);
  transition: all 0.3s ease;
}

.editbtn:not(.nav-btns button):hover {
  background-color: #000000;
  color: #fff;
  border-color: #000000;

}

.sz-10 {
  width: auto;
  height: 25px;
  padding-bottom: 5px;
}

.textbox_member {
  border: none;
  border-bottom: 1px solid #ccc;
  outline: none;
  font-size: 16px;
  transition: border-bottom-color 0.2s ease-in-out;
}

.pe-6 {
  padding-right: 10%;
  margin-right: 5%;
}

.h-500 {
  height: 700px;
}

.textbox_readonly {
  border: none;
  outline: none;
  font-size: 16px;
}

.textbox input {
  width: 60%;
}


input:-webkit-autofill {
  -webkit-box-shadow: 0 0 0 30px white inset;
  background-color: white !important;
}
</style>