<template>
  <div class="container mt-5">
    <div class="row">
      <div class="col-md-12 text-start">
        <h5 class="fw-bold">常用收件</h5>
      </div>
    </div>
  </div>

  <div class="container mb-5">
    <div class="row border pt-3 pb-5 px-4 text-start mb-4 " v-for="(addressone, index) in address">
      <div class="col-md-6 fw-bold">會員收件地址</div>
      <div class="col-md-6 text-end"><input :id="'#radio' + index" class="form-check-input" type="radio"
          name="destination" :value="addressone.destinationName"> <label :for="'#radio' + index">設為常用地址</label>
      </div>
      <div class="bg col-md-12">
        <div class="col-md-12  my-3">姓名: {{ addressone.destinationName }}</div>
        <div class="col-md-12  my-3">電話號碼: {{ addressone.destinationThe }}</div>
        <div class="col-md-12 my-3">地址: {{ addressone.destination }}</div>
      </div>
    </div>
  </div>



  <div class="mb-5">
    <button type="button" class="editbtn" data-bs-toggle="modal" data-bs-target="#exampleModal111"
      data-bs-whatever="@mdo">新增收件地址</button>
  </div>

  <div class="modal fade" id="exampleModal111" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h1 class="modal-title fs-5" id="exampleModalLabel">新增收件地址</h1>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <form>
            <div class="mb-3 form-floating">
              <input type="text" class="form-control" id="recipient-name" v-model="destinationName" require>
              <label for="recipient-name" class="col-form-label">姓名:</label>
            </div>
            <div class="mb-3 form-floating">
              <input type="text" class="form-control" id="message-text" v-model="destination" require>
              <label for="message-text" class="col-form-label">收件地:</label>
            </div>
            <div class="mb-3 form-floating">
              <input type="text" class="form-control" id="floatingPassword" v-model="destinationThe" require>
              <label for="message-text" class="col-form-label">收件電話:</label>
            </div>
            <div class="form-check text-center">
              <input class="form-check-input" type="checkbox" id="flexCheckDefault" />
              <label class="form-check-label" for="flexCheckDefault">設為常用地址</label>
            </div>
          </form>
        </div>
        <div class="modal-footer border-0">
          <button type="button" class="editbtn" @click="AddAddress">儲存</button>
        </div>
      </div>
    </div>
  </div>
</template>
    


<script setup>

import { onMounted, ref } from 'vue';

import axios from 'axios';
const address = ref([]);
const destinationName = ref("");
const destination = ref("")
const destinationThe = ref("")
const destinationCategory = ref("")


const AddAddress = () => {
  axios.post('https://localhost:7243/api/Member/AddAddress', {
    destinationName: destinationName.value,
    destination: destination.value,
    destinationThe: destinationThe.value,
    // destinationCategory: destinationCategory.value,
  }, { withCredentials: true }).then((response) => {
    alert(response.data)
    window.location = "http://localhost:5173/account/MemberAddresses";

  }).catch((err) => {
    console.log(err)
  })
};

const GetAddressInfo = async () => {
  await axios.get("https://localhost:7243/api/Member/GetAddressInfo", { withCredentials: true, })
    .then(response => {
      address.value = response.data;
      //console.log(address.value)
    })
    .catch(error => { console.log(error); });
}
onMounted(() => {
  GetAddressInfo();
});


</script>
<style scoped>
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

.pb-6 {
  padding-bottom: 5%;
}

.form-control {
  border: none;
  border-bottom: 1px solid #ccc;
  border-radius: 0;
  box-shadow: none;
}

.bg {
  background-color: #ebebeb;
  ;
}

.form-control:focus {
  border: none;
  outline: none;
  border-bottom: 1px solid #ccc;
  box-shadow: none;
}

input:-webkit-autofill,
textarea:-webkit-autofill,
select:-webkit-autofill {
  -webkit-box-shadow: 0 0 0px 1000px transparent inset !important;

  background-color: transparent;

  background-image: none;

  transition: background-color 50000s ease-in-out 0s;
}
</style>