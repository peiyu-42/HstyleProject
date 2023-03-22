<template>
  <div class="container my-5">
    <div class="row">
      <div class="col-md-12 text-start">
        <h5 class="fw-bold">訂單查詢</h5>
      </div>
    </div>
    <div class="order-header border-top border-bottom pt-2">
      <div class="row mb-2">
        <div class="col-1 text-center"></div>
        <div class="col-1 text-center">訂單編號</div>
        <div class="col-2 text-center">金額</div>
        <div class="col-2 text-center">日期</div>
        <div class="col-2 text-center">付款方式</div>
        <div class="col-1 text-center">狀態</div>
        <div class="col-2 text-center">幫助</div>
      </div>
    </div>
    <div class="accordion accordion-flush" id="accordionExample">
      <div
        v-for="(order, index) in orders"
        :key="order.orderId"
        class="accordion-item"
      >
        <div class="accordion-header row" :id="'heading' + index">
          <div class="col-1 text-center">
            <button
              class="accordion-button btn-order"
              type="button"
              data-bs-toggle="collapse"
              :data-bs-target="'#collapse' + index"
              :class="{ collapsed: index !== -1 }"
              aria-expanded="false"
              :aria-controls="'collapse' + index"
            ></button>
          </div>
          <div class="col-1 text-center pt-2">{{ order.orderId }}</div>
          <div class="col-2 text-center pt-2">
            NT$ {{ order.total.toLocaleString() }}
          </div>
          <div class="col-2 text-center pt-2">
            {{ order.createdTime.slice(0, 10) }}
          </div>
          <div class="col-2 text-center pt-2">{{ order.payment }}</div>
          <div class="col-1 text-center pt-2 ps-1">
            <div
              v-if="order.statusId === 1"
              @click.prevent="goToPay(order.payInfo)"
            >
              <a class="alink"
                >待付款<i
                  class="fa-solid fa-arrow-up-right-from-square fz-sm ps-2"
                ></i
              ></a>
            </div>
            <div v-else>
              {{ order.status }}
            </div>
          </div>
          <div class="col-2 pt-2 text-center mb-2">
            <a
              class="blink"
              data-bs-toggle="modal"
              data-bs-target="#MemberQModal"
              >聯絡客服</a
            >
            <br />

            <a
              v-if="order.status != '退貨處理中'"
              class="blink"
              @click="returnGoods(order.orderId)"
              >我要退貨</a
            >
          </div>
        </div>
        <div
          :id="'collapse' + index"
          class="accordion-collapse collapse"
          :aria-labelledby="'heading' + index"
          data-bs-parent="#accordionExample"
        >
          <div class="accordion-body">
            <div class="row bg-dark text-white font-weight-bold py-2">
              <div class="col-1"></div>
              <div class="col-2 text-center">品名</div>
              <div class="col-2 text-center">規格</div>
              <div class="col-2 text-center">單價</div>
              <div class="col-2 text-center">數量</div>
              <div class="col-2 text-center">小計</div>
              <div class="col-1"></div>
            </div>
            <div
              v-for="(product, index) in order.orderDetails"
              :key="index"
              class="row accordion-body border-start border-end py-3 px-0"
            >
              <div class="col-1"></div>
              <div class="col-2 text-center">{{ product.productName }}</div>
              <div class="col-2 text-center">
                {{ `${product.color}, ${product.size}` }}
              </div>
              <div class="col-2 text-center">
                {{ product.unitPrice.toLocaleString() }}
              </div>
              <div class="col-2 text-center">{{ product.quantity }}</div>
              <div class="col-2 text-center">
                {{ product.subTotal.toLocaleString() }}
              </div>
              <div class="col-1 py-0">
                <button
                  @click="
                    createCommonModal(
                      product.productId,
                      product.orderId,
                      product.productName,
                      product.color,
                      product.size
                    )
                  "
                  type="button"
                  data-bs-toggle="modal"
                  class="btn btn-link b-0 px-1"
                  data-bs-target="#ProductCommentModal"
                >
                  我要評論
                </button>
              </div>
            </div>
            <div class="row order-item-footer py-2 border">
              <div class="col-1"></div>
              <div class="col-2 text-center">
                共 {{ order.orderDetails.length }} 件
              </div>
              <div class="col-6"></div>
              <div class="col-2 text-center">H幣折抵：{{ order.discount }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="border-bottom mt-1"></div>
  </div>
  <CreatePComment :modalData="commentParams"></CreatePComment>
  <MemberQForm></MemberQForm>
</template>
  
  
<script setup>
import { onMounted, ref } from "vue";
import axios from "axios";
import MemberQForm from "./MemberQForm.vue";
import CreatePComment from "./CreatePComment.vue";

const orders = ref([]);

const getOrdersInfo = async () => {
  await axios
    .get("https://localhost:7243/api/Order", { withCredentials: true })
    .then((response) => {
      orders.value = response.data;
      //console.log(orders.value);
    })
    .catch((error) => {
      console.log(error);
    });
};

function goToPay(token) {
  // 使用 window.location.href 跳轉到 PayPal 付款頁面
  window.location.href = `https://www.sandbox.paypal.com/checkoutnow?token=${token}`;
}

const returnGoods = async (orderId) => {
  await axios
    .put(`https://localhost:7243/api/Order/${orderId}`)
    .then((response) => {
      //console.log(response.data);
      getOrdersInfo();
    })
    .catch((error) => {
      alert(error.response.data);
      console.log(error.response.data);
    });
};

const commentParams = ref({
  productId: 0,
  orderId: 0,
  productName: "",
  productSpec: "",
});
const createCommonModal = (productId, orderId, productName, color, size) => {
  commentParams.value.productId = productId;
  commentParams.value.orderId = orderId;
  commentParams.value.productName = productName;
  commentParams.value.productSpec = `${color}, ${size}`;
  //console.log(commentParams.value);
};

onMounted(() => {
  getOrdersInfo();
});
</script>

<style scoped>
.fz-sm {
  font-size: 5pt;
  color: gray;
}

.alink {
  text-decoration: none;
  padding-left: 15pt;
  color: #46a3ff;
  cursor: pointer;
}

.blink {
  text-decoration: none;
  color: #46a3ff;
  cursor: pointer;
}

.accordion-item {
  border-top: none;
  border-radius: none;
}

.accordion {
  background-color: transparent;
  --bs-accordion-border-radius: 0%;
}

.accordion-button {
  --bs-accordion-btn-icon-width: 12pt;
}

.btn-order {
  background: none;
  color: #000;
  box-shadow: none;
}

.btn-order:focus {
  outline-color: none;
  box-shadow: none;
}

.button-comment:focus,
.button-comment:active {
  box-shadow: none;
  outline: none;
}

.btn-link {
  text-decoration-line: none;
  color: #46a3ff;
}
</style>
  