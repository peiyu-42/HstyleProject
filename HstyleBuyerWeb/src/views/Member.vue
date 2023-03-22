<!-- Product.vue -->
<template>
    <div>
      <h1>Welcome to the member page!</h1>
      <p>This is the product page of our app.</p>
    </div>

    <div>
    <form>
      <input type="text" onchange="" v-model="username" placeholder="username">
      <input type="password" v-model="password" placeholder="password">
      <button type="button" @click="login">Login</button>
      <button type="button" @click="Logout">Logout</button>

    </form>
  </div>

<div>
  <input type="text" onchange="" v-model="name" placeholder="name">
  <input type="email" onchange="" v-model="Email" placeholder="Email">
  <input type="text" onchange="" v-model="Account" placeholder="Account">
  <input type="number" onchange="" v-model="PhoneNumber" placeholder="PhoneNumber">
  <input type="text" onchange="" v-model="Address" placeholder="Address">
  <div>
    <input type="radio" id="male" value="male" v-model="gender" placeholder="gender">
    <label for="male">Male</label>

    <input type="radio" id="female" value="female" v-model="gender" placeholder="gender">
    <label for="female">Female</label>

    <p>Selected gender: {{ gender }}</p>
  </div>
  <input type="date" id="date" onchange="" v-model="Birthday" placeholder="Birthday">
  <input type="password" onchange="" v-model="EncryptedPassword" placeholder="password">
  <button type="button" @click="register">register</button>

</div>

  
<div id="g_id_onload"
     data-client_id="826430034566-tvq18h6l8duepd5chu8rc1gekodbc4a5.apps.googleusercontent.com"
     data-login_uri="http://localhost:5173/member"
     data-auto_prompt="false">
</div>
<div class="g_id_signin"
     data-type="standard"
     data-size="large"
     data-theme="outline"
     data-text="sign_in_with"
     data-shape="rectangular"
     data-logo_alignment="left">
</div>

<div>
    <p>會員ID：{{ memberId }}</p>
</div>

</template>


<script>
  

//以上測試
import axios from 'axios';
axios.defaults.withCredentials = true;
import { reactive } from 'vue'

const state = reactive({
  token: ''
})

const signIn = async () => {
  try {
    await googleAuth.signIn()
    const googleUser = googleAuth.getAuthUser()
    const idToken = googleUser.getAuthResponse().id_token
    state.token = idToken

    const response = await fetch('https://your-api-endpoint.com/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ idToken })
    })
    if (!response.ok) {
      throw new Error('Login failed')
    }
  } catch (err) {
    console.error(err)
  }
}

export default {
  signIn,
  state,
    data(){
        return{
            username: "",
            password: "",
            name:"",
            Email:"",
            Account:"",
            PhoneNumber:"",
            Address:"",
            gender:"",
            Birthday:"",
            EncryptedPassword:"",
            MailCode:1,
            isSignedIn: false,
            googleUser: null,
            memberId: null,
        }
    },

  methods: {
    login(e) {
      e.preventDefault();
      fetch('https://localhost:7243/api/Member/LogIn', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          "Accept": "application/json",
        },
        credentials: 'include',

        body: JSON.stringify({
          'account': this.username,
          'password': this.password
        }),
      })
        .then((response) => {
          console.log(response.data)
        })
        .catch(error => console.error(error));
    },

    Logout(e){
        //e.preventDefault()
        fetch('https://localhost:7243/api/Member/LogOut', {
        method: 'POST',
        headers: {     
            'Content-Type': 'application/json',       
            "Accept": "application/json",
        },
        credentials: 'include',
        })
        .then((response) => {
            return response.json();
        })
        .then(response => {
            console.log(response)
        }).catch(error => {
            console.error(error)// 網路錯誤或其他錯誤處理
        })
    },
    register(e) {
      e.preventDefault();
      fetch('https://localhost:7243/api/Member/Register', {
        method: 'POST',
        headers: {
          // Authorization: Bearer <your_access_token>,
          'Authorization': `Bearer ${access_token}`,
          'Content-Type': 'application/json',
          "Accept": "application/json",
        },
        body: JSON.stringify({
          'name': this.name,
          'email': this.Email,
          'password': this.EncryptedPassword,
          'account': this.Account,
          'phone_Number': this.PhoneNumber,
          'address': this.Address,
          'gender': this.Gender,
          'birthday': this.Birthday,
          'mailCode':this.MailCode,
        }),
        credentials: 'same-origin'
      })
        .then((response) => {
            // return response.json();
            if (response.ok) {
            return response.json();
          } else {
            throw new Error('Something went wrong');
          }
        })
        .then( (response) => {
            console.log(response);
        })
        .catch(error => console.error(error));
    },
  }
}
</script>
  <style></style>