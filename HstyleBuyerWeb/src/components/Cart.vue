<template>
  <div
    class="modal come-from-modal fade"
    id="cartModal"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content h-100">
        <div class="modal-header">
          <h5
            class="modal-title position-absolute fw-bolder start-50 translate-middle-x"
            id="exampleModalLabel"
          >
            購物車
          </h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body mt-3">
          <div v-if="isEmpty">
            <p>您的購物車沒有商品</p>
          </div>
          <div
            v-for="item in products.cartItems"
            class="row g-2 align-items-center border-bottom pb-4 mb-3 mt-2"
          >
            <div class="col-md-4 img-sz">
              <img :src="item.image" class="img-fluid" alt="Product Image" />
            </div>
            <div class="col-md-8">
              <div class="d-flex justify-content-between align-items-center">
                <div class="mb-0 fz-15 ps-2">{{ item.productName }}</div>
              </div>
              <div class="d-flex justify-content-between align-items-center">
                <p class="text-muted mb-0 fz-15 ps-2">
                  {{ item.color + ", " + item.size }}
                </p>
              </div>
              <div
                class="d-flex justify-content-between align-items-center pt-3"
              >
                <div class="mb-0 fz-15 mx-2 pt-2">
                  <span
                    >NT$
                    {{
                      (item.unitPrice * item.quantity).toLocaleString()
                    }}</span
                  >
                </div>
                <div class="btn-group-sm" role="group">
                  <button
                    type="button"
                    class="btn btn-custom-left btn-outline-secondary btn-s"
                    @click="minusItem(item.specId)"
                  >
                    <i class="fa-solid fa-minus fz-10"></i>
                  </button>
                  <button
                    type="button"
                    class="btn border-0 border-top border-bottom border-secondary btn-outline-secondary btn-s quantity"
                  >
                    <div class="text-center mx-6">{{ item.quantity }}</div>
                  </button>
                  <button
                    type="button"
                    class="btn btn-custom-right btn-outline-secondary btn-s"
                    @click="addItem(item.specId)"
                  >
                    <i class="fa-solid fa-plus fz-10"></i>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-if="!isEmpty" class="modal-footer px-2">
          <div>
            <strong>NT$ {{ products.total.toLocaleString() }}</strong>
          </div>

          <div class="ps-2">
            <router-link to="/Checkout" class="nav-link targetAll">
              <button
                type="button"
                class="btn btn-secondary checkoutbtn"
                data-bs-dismiss="modal"
              >
                檢視購物車及付款
              </button>
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from "vue";
import axios from "axios";
import { eventBus } from "../mybus";

const products = ref([]);
const isEmpty = ref(true);
const emit = defineEmits(["CartCount"]);
const getCartInfo = async () => {
  await axios
    .get("https://localhost:7243/api/Cart", { withCredentials: true })
    .then((response) => {
      const itemsToDelete = response.data.cartItems.filter(
        (item) => item.discontinued === true
      );
      if (itemsToDelete.length > 0) {
        alert("部分款式已下架，請重新選購");
        itemsToDelete.forEach((item) => {
          minusItem(item.specId);
        });
      }
      products.value = response.data;
      if (products.value.cartItems.length > 0) {
        isEmpty.value = false;
      } else {
        isEmpty.value = true;
      }
      emit("CartCount", cartCount);
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
    })
    .catch((error) => {
      console.log(error);
    });
};

const cartCount = computed(() => {
  var sum = 0;
  products.value.cartItems.forEach(function (element) {
    sum += element.quantity;
  });
  return sum;
});

eventBus.on("addCartEvent", getCartInfo);
eventBus.on("Login", getCartInfo);

onMounted(() => {
  getCartInfo();
});
</script>
<style scoped>
.modal-dialog {
  position: fixed;
  margin: auto;
  width: 420px;
  height: 100%;
  -webkit-transform: translate3d(0%, 0, 0);
  -ms-transform: translate3d(0%, 0, 0);
  -o-transform: translate3d(0%, 0, 0);
  transform: translate3d(0%, 0, 0);
  right: 0;
}

.modal-content {
  height: 100%;
  overflow-y: auto;
  border-radius: 0px;
}

.modal-body {
  padding: 15px 15px 80px;
}

.fz-15 {
  font-size: 12pt;
}

.mx-6 {
  margin-left: 1pt;
  margin-right: 1pt;
}

.checkoutbtn:not(.nav-btns button) {
  background-color: #fff;
  color: rgb(12, 13, 12);
  padding: 10px 28px;
  border-radius: 25px;
  border: 1px solid rgb(12, 13, 12);
  transition: all 0.3s ease;
}

.checkoutbtn:not(.nav-btns button):hover {
  background-color: #000000;
  color: #fff;
  border-color: #000000;
}

.img-sz {
  width: 100px;
  height: 100px;
  overflow: hidden;
}

.img-sz img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.quantity {
  pointer-events: none;
  border-radius: 0%;
}

.modal.fade .modal-dialog {
  transform: translateX(10vh);
}

.modal.show .modal-dialog {
  transform: translateX(0);
}

.fade {
  transition: opacity 0.15s linear;
}

.btn-custom-left {
  border-radius: 5% 0 0 5%;
}

.btn-custom-right {
  border-radius: 0 5% 5% 0;
}

.fz-10 {
  font-size: 9px;
}
</style>
