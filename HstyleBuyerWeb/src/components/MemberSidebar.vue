<template>
    <div>

    </div>
    <div class="sidebar mt-5">
        <ul class="list-unstyled">
            <li>
                <router-link to="/account/MemberProfile" class="text-decoration-none text-dark">會員資料</router-link>
            </li>
            <li>
                <router-link to="/account/MemberAddresses" class="text-decoration-none text-dark">常用收件</router-link>
            </li>
            <li>
                <router-link to="/account/orders" class="text-decoration-none text-dark">訂單查詢</router-link>
            </li>
            <li>
                <router-link to="/account/MemberCollection" class="text-decoration-none text-dark">收藏清單</router-link>
            </li>
            <li>
                <router-link to="/account/responses" class="text-decoration-none text-dark">客服回覆</router-link>
            </li>
        </ul>
        <button type="button" @click="logout" class="btn btn-link text-dark text-decoration-none">登出</button>
    </div>
</template>
  
<script setup>
import axios from 'axios';
import { eventBus } from "../mybus";

const logout = () => {
    axios.post('https://localhost:7243/api/Member/LogOut', {}, {
        withCredentials: true,
    })
        .then((response) => {
            console.log(response.data);
            window.location = "http://localhost:5173";
        })
        .catch((error) => {
            console.error(error);
        });
};

eventBus.on("ResetPassword", () => {
    logout();
});
</script>
  
<style scoped>
.sidebar {
    background-color: #f5f5f5;
    padding: 10px;
    height: 100%;
    width: 300px;
    min-height: 650px;
}

ul {
    margin: 0;
    padding: 0;
}

li {
    margin-bottom: 10px;
}

i {
    font-size: 1.2rem;
}
</style>
  