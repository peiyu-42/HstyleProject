<template>
   <div>
      <h6>Chat Room</h6>
      <p v-for="(message, index) in messages" :key="index">{{ message.substring(3, 100) }}</p>
      <!-- <ul>
         <p v-for="(message, index) in messages" :key="index">{{ message.substring(3,3+messageLength) }}</p>
         <p v-for="(message, index) in messages" :key="index">{{ message.slice(7+messageLength, 100) }}</p>
      </ul> -->
      <!-- <div v-for="(message, index) in messages" :key="index">
         <p>{{ message.substring(3, 4+messageLength) }}</p>
         <p>{{ message.slice(7+messageLength, 100) }}</p>
      </div> -->
      <form @submit.prevent="handleSubmit">
         <input v-model="message" type="text" class="form-control" name="messageContent" placeholder="Type your message" />
         <button class="btn mt-3" type="submit">Send</button>
      </form>
   </div>

   <!-- <router-link to="/ServerChatRoom" class="nav-link text-secondary"><span class="txt-hover">客服回覆</span></router-link> -->
</template>

<script>
import { ref, onUnmounted } from "vue";
export default {
   name: "ChatRoom",
   setup() {
      const message = ref("");
      const messages = ref([]);
      const messageLength = ref([]);
      const ws = new WebSocket("wss://localhost:7243/ws");

      function handleSubmit() {
         const data = {
            // userName: Math.random().toString(36).substring(7),
            message: message.value,
         };
         // messages.messageLength.value.push = message.value.length;
         // console.log(messages.messageLength.value);
         ws.send(JSON.stringify(data));
         message.value = "";
         console.log(message.value);
      }

      ws.onopen = () => {
         console.log("WebSocket已連接");
      };

      ws.onmessage = (event) => {
         const data = JSON.parse(event.data);
         messages.value.push(`${data.userName}: ${data.message}`);
      };

      ws.onclose = () => {
         console.log("WebSocket已關閉");
      };

      ws.onerror = (error) => {
         console.log(`WebSocket錯誤: ${error}`);
      };

      onUnmounted(() => {
         ws.close();
         console.log("聊天室已關閉");
      });

      return {
         message,
         messages,
         handleSubmit,
         messageLength,
      };
   },
};
</script>
<style scoped>
/* 按鈕 */
.btn:not(.nav-btns button) {
   background-color: #fff;
   color: rgb(12, 13, 12);
   padding: 6px 15px;
   margin: 5px;
   border-radius: 25px;
   border: 1px solid rgb(12, 13, 12);
}

.btn:not(.nav-btns button):hover {
   background-color: #000;
   color: #fff;
   border-color: #000;
}

input:-webkit-autofill,
textarea:-webkit-autofill,
select:-webkit-autofill {
   -webkit-box-shadow: 0 0 0px 1000px transparent inset !important;

   background-color: transparent;

   background-image: none;

   transition: background-color 50000s ease-in-out 0s;
}

.form-control:focus,
.form-control:active {
   box-shadow: none;
   outline: none;
   color: #000;
}
</style>
