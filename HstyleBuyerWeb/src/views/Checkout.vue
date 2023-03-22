<!-- checkout.vue -->
<template>
  <div class="container mt-3">
    <div class="row">
      <div class="col-md-12 mb-3 cartContent">
        <div class="text-start border-bottom">
          <h5 class="fw-bold">購物車</h5>
        </div>
        <div v-for="item in products.cartItems" class="py-3 border-bottom row">
          <div class="col-md-12 d-flex align-items-center">
            <div class="">
              <div class="custom">
                <img :src="item.image" alt="圖片已失效" />
              </div>
            </div>
            <div class="ms-5">
              <span class="card-title"
                >{{ item.productName }}
                <span class="border-start ps-2"
                  >規格: {{ item.color + ", " + item.size }}</span
                >
              </span>
            </div>
            <div class="flex-grow-1 text-end pe-6">
              <div class="btn-group" role="group">
                <button
                  type="button"
                  class="btn btn-custom btn-outline-secondary btn-s"
                  @click="minusItem(item.specId)"
                >
                  <i class="fa-solid fa-minus fa-xs"></i>
                </button>
                <button
                  type="button"
                  class="btn btn-custom btn-outline-secondary btn-s quantity"
                >
                  {{ item.quantity }}
                </button>
                <button
                  type="button"
                  class="btn btn-custom btn-outline-secondary btn-s"
                  @click="addItem(item.specId)"
                >
                  <i class="fa-solid fa-plus fa-xs"></i>
                </button>
              </div>
            </div>
            <div class="ms-auto">
              <span
                >NT$
                {{ (item.unitPrice * item.quantity).toLocaleString() }}</span
              >
            </div>
          </div>
        </div>
      </div>
      <!-- <form @submit.prevent="checkout"> -->
      <div class="col-md-7 mt-5">
        <div class="fz-12 fw-bold text-start border-bottom pb-1 mb-2">
          收件與付款資訊
        </div>
        <div class="my-4 d-flex justify-content-between position-relative">
          <label for="name" class="form-label fw-bold pe-3">姓名</label>
          <input
            v-model="shipName"
            type="text"
            class="textbox_ship flex-grow-1"
            id="name"
            placeholder="請輸入姓名"
          />
          <div
            v-if="error.name"
            class="text-danger fz-smer position-absolute top-50 start-50 translate-middle pe-7"
          >
            {{ error.name }}
          </div>
          <label for="phone" class="form-label fw-bold pe-3">電話</label>
          <input
            v-model="shipPhone"
            type="text"
            class="textbox_ship flex-grow-1"
            id="phone"
            placeholder="請輸入電話號碼"
          />
          <div
            v-if="error.phone"
            class="text-danger fz-smer position-absolute top-50 pb1 end-0 translate-middle-y"
          >
            {{ error.phone }}
          </div>
        </div>
        <div class="mb-4 d-flex justify-content-between position-relative">
          <label for="address" class="form-label fw-bold pe-3">地址</label>
          <input
            v-model="shipAddress"
            type="text"
            class="textbox_ship flex-grow-1"
            id="address"
            placeholder="請輸入地址"
          />
          <div
            v-if="error.address"
            class="text-danger fz-smer position-absolute pb1 top-50 end-0 translate-middle-y"
          >
            {{ error.address }}
          </div>
        </div>
        <div class="mb-3 d-flex justify-content-between position-relative">
          <label for="payment" class="form-label fw-bold pe-3">付款方式</label>
          <div class="flex-grow-1">
            <select
              v-model="payment"
              aria-label="Select payment method"
              id="payment"
            >
              <option value="">選擇付款方式</option>
              <option value="信用卡">信用卡</option>
              <option value="Paypal">PayPal</option>
            </select>
            <div
              v-if="error.payment"
              class="text-danger fz-smer pb-3 position-absolute top-50 end-0 translate-middle-y"
            >
              {{ error.payment }}
            </div>
          </div>
        </div>
        <div class="text-end">
          <button
            @click="demo1"
            class="btn btn-link text-dark text-decoration-none"
          >
            demo
          </button>
        </div>
      </div>
      <div class="col-md-1"></div>
      <div class="col-md-4 mt-5">
        <div class="fz-12 fw-bold text-start border-bottom pb-1 mb-2">
          訂單摘要
        </div>
        <div class="d-flex justify-content-between pb-3">
          <div class="fw-bold">商品金額</div>
          <div class="pe-1">
            NT$ {{ products.total > 0 ? products.total.toLocaleString() : 0 }}
          </div>
        </div>
        <div class="d-flex justify-content-between pb-3">
          <div class="fw-bold">運費</div>
          <div class="pe-1">NT$ 0</div>
        </div>
        <div class="d-flex justify-content-between pb-2">
          <div class="fw-bold">使用H幣</div>
          <div v-if="error.discount" class="text-danger pe-5 pt-1 fz-smer">
            {{ error.discount }}
          </div>
          <input
            v-model="discount"
            type="number"
            class="textbox form-floating no-spin"
            name=""
            id=""
            placeholder="請輸入數量"
          />
        </div>
        <div class="fz-sm text-end pb-4">
          目前有{{ H_Coin }}枚，最高可使用{{ coinUseLimit }}枚
        </div>
        <div class="d-flex justify-content-between border-top py-4">
          <div class="fw-bold">總金額</div>
          <div>NT$ {{ totalIncludeHcoin.toLocaleString() }}</div>
        </div>
        <div class="text-end mt-1 mb-5">
          <button
            :disabled="progressing"
            @click="checkout"
            type="button"
            class="btn checkoutbtn"
          >
            提交訂單
          </button>
          <div id="result"></div>
        </div>
      </div>
      <!-- </form> -->
    </div>
  </div>
  <div v-if="progressing" id="modal" class="modal">
    <div class="modal-content">
      <img
        class="loading"
        src="../assets/progressGif/loading.gif"
        alt="Loading..."
      />
    </div>
  </div>

  <!-- 用表單送給藍新 -->
  <form
    name="Newebpay"
    method="post"
    action="https://ccore.newebpay.com/MPG/mpg_gateway"
  >
    <!-- 設定 hidden 可以隱藏不用給使用者看的資訊 -->
    <!-- 藍新金流商店代號 -->
    <input
      type="hidden"
      id="MerchantID"
      name="MerchantID"
      value="MS148335860"
    />
    <!-- 交易資料透過 Key 及 IV 進行 AES 加密 -->
    <input
      type="hidden"
      id="TradeInfo"
      name="TradeInfo"
      value="5b7c2d1eed7e527931ad7ca846c28f06e8ea19aa7a059c61993cff60a8208ee86fd411d603ed6735805efd8abf9deb039c92241fe3eb00e8168c4c7d04faf7966925631087933e10f3b7c98618f66f93f196c3842ecb4a3d537b88877ca49be7235e916e9660b66842e759f141c18f4b0086c8c9375ad1526ff20bff34041a66abff65bc8d4574be69e160c7f384f820a535b0fdadf07d8a9ec5dc5479ff387de922be749bd88eb30a5e7d4d613ba255d51b144debeb4a86bfb5ed2e4974cb54a8eaa62dc609e35b6d543ed064f8cac9ab827a0650d3ff1fed30d9c848b306a63b9cc5680a125d1e9e469461c97e5bc6b783cc693fbae708f09d485cb47b1ea56d675e5f4be2b06570c556670349ff0a31e4b42f98fda6ec28ff8d5cf675855a2e354e511fb7c2517751a3a8d7dce848"
    />
    <!-- 經過上述 AES 加密過的字串，透過商店 Key 及 IV 進行 SHA256 加密 -->
    <input
      type="hidden"
      id="TradeSha"
      name="TradeSha"
      value="3E7A356A3852638C4A2573AD5D2F1CF709720F40165CF90AAEB82E4AA8FCBE66"
    />
    <!-- 串接程式版本 -->
    <input type="hidden" id="Version" name="Version" value="2.0" />
    <!-- 直接執行送出 -->
    <!-- <input type="submit" value="前往付款" /> -->
  </form>
</template>
  
<script setup>
import { ref, onMounted, computed, onBeforeMount } from "vue";
import axios from "axios";
import { eventBus } from "../mybus";

//呈現購物車
const products = ref([]);
const getCartInfo = async () => {
  await axios
    .get("https://localhost:7243/api/Cart", { withCredentials: true })
    .then((response) => {
      products.value = response.data;
      if (response.data.cartItems.length === 0) {
        window.location = "http://localhost:5173/product";
      }
    })
    .catch((error) => {
      console.log(error);
    });
};

const addItem = async (specId) => {
  await axios
    .post(
      `https://localhost:7243/api/Cart/${specId}`,
      {},
      { withCredentials: true }
    )
    .then((response) => {
      getCartInfo();
      eventBus.emit("addCartEvent");
    })
    .catch((error) => {
      console.log(error);
    });
};
const minusItem = async (specId) => {
  await axios
    .delete(`https://localhost:7243/api/Cart/${specId}`, {
      withCredentials: true,
    })
    .then((response) => {
      getCartInfo();
      eventBus.emit("addCartEvent");
    })
    .catch((error) => {
      console.log(error);
    });
  console.log(products.value);
};

//綁定收件、收費資料
const shipName = ref("");
const shipAddress = ref("");
const shipPhone = ref("");
const payment = ref("");
const discount = ref("");
const totalIncludeHcoin = computed(() => {
  return products.value.total - discount.value;
});

//結帳
const progressing = ref(false);
const H_Coin = ref(0);
const getCoin = async () => {
  await axios
    .get("https://localhost:7243/api/HCoin/TotalHCoin", {
      withCredentials: true,
    })
    .then((response) => {
      H_Coin.value = response.data;
    })
    .catch((error) => {
      console.log(error);
    });
};
const coinUseLimit = computed(() => {
  const twentyPercent = parseInt(products.value.total * 0.2);
  return H_Coin.value < twentyPercent ? H_Coin : twentyPercent;
});
const checkout = async () => {
  if (!validateForm()) {
    return;
  }
  progressing.value = true;
  await axios
    .post(
      `https://localhost:7243/api/Cart/Checkout`,
      {
        payment: payment.value,
        shipVia: "黑貓", //todo。運送方式改成後台管理頁面填寫
        freight: 0, //todo，考慮運費拿掉全部免運
        discount: discount.value,
        shipName: shipName.value,
        shipAddress: shipAddress.value,
        shipPhone: shipPhone.value,
      },
      { withCredentials: true }
    )
    .then((response) => {
      axios
        .post(
          `https://localhost:7243/api/Paypal/${response.data}`,
          {},
          {
            withCredentials: true,
          }
        )
        .then((response) => {
          window.location = response.data.paypalLink;
        })
        .catch((error) => {
          console.log(error);
          progressing.value = false;
        });
    })
    .catch((error) => {
      console.log(error);
      progressing.value = false;
    });
};

//todo...信用卡支付
const payByCredic = () => {
  axios
    .post(`https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5`, {
      ashKey: "pwFHCqoQZGmho4w6", //ECPay提供的Hash Key
      HashIV: "EkRm7iFT261dpevs", //ECPay提供的Hash IV
      MerchantID: "3002607", //ECPay提供的特店編號
      send: {
        ReturnURL: "http://example.com", //付款完成通知回傳的網址
        ClientBackURL: "http://www.google.com./", //瀏覽器端返回的廠商網址
        OrderResultURL: "https://www.youtube.com/", //瀏覽器端回傳付款結果網址
        MerchantTradeNo: "ECPay" + new Random().Next(0, 99999).ToString(), //廠商的交易編號
        MerchantTradeDate: DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), //廠商的交易時間
        TotalAmount: Decimal.Parse("3280"), //交易總金額
        TradeDesc: "交易描述", //交易描述
        ChoosePayment: PaymentMethod.Credit, //使用的付款方式
        Remark: "", //備註欄位
        ChooseSubPayment: PaymentMethodItem.None, //使用的付款子項目
        NeedExtraPaidInfo: ExtraPaymentInfo.Yes, //是否需要額外的付款資訊
        DeviceSource: DeviceType.PC, //來源裝置
        IgnorePayment: "", //不顯示的付款方式
        PlatformID: "", //特約合作平台商代號
        CustomField1: "",
        CustomField2: "",
        CustomField3: "",
        CustomField4: "",
        EncryptType: 1,
        Items: {
          Name: "HStyleStore", //商品名稱
          Price: Decimal.Parse("3280"), //商品單價
          Currency: "新台幣", //幣別單位
          Quantity: Int32.Parse("1"), //購買數量
          URL: "http://google.com", //商品的說明網址
        },
      },
    })
    .then((response) => {
      console.log(response.data);
    })
    .catch((error) => {
      console.log(error);
      progressing.value = false;
    });
};

const error = ref({});
const validateForm = () => {
  error.value = {};

  if (!shipName.value) {
    error.value.name = "收件姓名必填";
  } else if (shipName.value.length < 2) {
    error.value.name = "姓名長度太短";
  }
  const phonePattern = /^09\d{8}$/;
  if (!shipPhone.value) {
    error.value.phone = "電話欄位必填";
  } else if (!phonePattern.test(shipPhone.value)) {
    error.value.phone = "電話格式錯誤";
  }

  const addressPattern =
    /^(台灣省|台灣|臺灣省|臺灣)?([^\s]+?[市|縣|州])([^\s]+?[區|鄉|鎮])([^\s]+?(?:街|路|巷))([^\s]+?(?:號))?$/;
  if (!shipAddress.value) {
    error.value.address = "地址欄位必填";
  } else if (!addressPattern.test(shipAddress.value)) {
    error.value.address = "地址格式錯誤";
  }
  if (discount.value < 0 || discount.value > coinUseLimit.value.value) {
    error.value.discount = "輸入值超出範圍";
  }
  if (payment.value === "") {
    error.value.payment = "必須選擇一種付款方式";
  }
  console.log(error.value);
  // 如果有錯誤，顯示錯誤信息，並返回 false
  if (Object.keys(error.value).length > 0) {
    return false;
  }

  // 所有驗證都通過，返回 true
  return true;
};

const demo1 = () => {
  shipName.value = "黃石峰";
  shipAddress.value = "台北市中正區復興路232號";
  shipPhone.value = "0987652312";
  discount.value = 1000;
};

onMounted(() => {
  getCartInfo();
  getCoin();
});
</script>
<style scoped>
.custom {
  width: 200px;
  height: 200px;
  overflow: hidden;
}

.pe-6 {
  padding-right: 10%;
}

.pe-7 {
  padding-right: 15%;
  padding-bottom: 1%;
}

.pb1 {
  padding-bottom: 1%;
  z-index: 9999;
}

.custom img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.fz-12 {
  font-size: 13pt;
}

.btn-custom {
  border-radius: 5%;
}

.fz-sm {
  font-size: 12px;
}

.fz-10 {
  font-size: 15px;
}

.fz-smer {
  font-size: 14px;
}

.checkoutbtn:not(.nav-btns button) {
  background-color: #fff;
  color: rgb(12, 13, 12);
  padding: 10px 28px;
  border-radius: 25px;
  border: 1px solid rgb(12, 13, 12);
}

.quantity {
  pointer-events: none;
  border-radius: 0%;
}

.checkoutbtn:not(.nav-btns button):hover {
  background-color: #000000;
  color: #fff;
  border-color: #000000;
}

.textbox {
  border: none;
  border-bottom: 1px solid transparent;
  outline: none;
  font-size: 16px;
  transition: border-bottom-color 0.2s ease-in-out;
  text-align: right;
  padding-right: 4px;
}

select {
  appearance: none;
  border: none;
  border-bottom: 1px solid #ccc;
  width: 100%;
  color: #757575;
}

select:focus {
  outline: none;
}

.textbox_ship {
  border: none;
  border-bottom: 1px solid #ccc;
  outline: none;
  font-size: 16px;
  transition: border-bottom-color 0.2s ease-in-out;
}

.textbox:focus {
  border-bottom-color: #ccc;
}

.cartContent {
  min-height: 200px;
}

.modal {
  display: block;
  position: fixed;
  z-index: 9999;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(255, 255, 255, 0.5);
}

.modal-content {
  position: absolute;
  border: none;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 100px;
  /* 設置寬度為 200px */
  height: 100px;
  /* 設置高度為 200px */
  margin: auto;
  /* 將margin設置為auto使其垂直和水平居中 */
}

.no-spin::-webkit-inner-spin-button,
.no-spin::-webkit-outer-spin-button {
  -webkit-appearance: none;
  margin: 0;
}
</style>