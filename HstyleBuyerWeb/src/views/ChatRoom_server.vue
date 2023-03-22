<template>
   <div>
      <h1>Server Chat Room</h1>
      <ul>
         <li v-for="(message, index) in messages" :key="index">{{ message }}</li>
      </ul>
      <form @submit.prevent="handleSubmit">
         <input v-model="message" type="text" placeholder="Type your message" />
         <button type="submit">Send</button>
      </form>
   </div>
</template>

<script>
// const WebSocket = require("ws");
// const wss = new WebSocket.Server({ port: 7243 });

// wss.on("connection", function connection(ws) {
//    console.log("新的客戶端已連接");
//    // 監聽客戶端發送的消息
//    ws.on("message", function incoming(data) {
//       console.log("接收到客戶端的消息: %s", data);
//       // 广播消息给其他客户端
//       wss.clients.forEach(function each(client) {
//          if (client !== ws && client.readyState === WebSocket.OPEN) {
//             client.send(data);
//          }
//       });
//    });
// });
</script>
