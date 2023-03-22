<template>
   <div class="container my-5">
      <div class="row">
         <div class="col-md-12 text-start">
            <h5 class="fw-bold">客服回復</h5>
         </div>
      </div>
      <div class="order-header border-top border-bottom pt-2">
         <div class="row mb-2">
            <div class="col-1 text-center"></div>
            <div class="col-2 text-center">問題</div>
            <div class="col-4 text-center">問題描述</div>
            <div class="col-3 text-center">詢問時間</div>
            <div class="col-1 text-center"></div>
         </div>
      </div>
      <div class="accordion accordion-flush mb-2" id="accordionExample">
         <div v-for="(question, index) in memberQ" :key="question.customerQuestionId" class="accordion-item">
            <div class="accordion-header row" :id="'heading' + index">
               <div class="col-1 text-center">
                  <button class="accordion-button btn-order" type="button" data-bs-toggle="collapse"
                     :data-bs-target="'#collapse' + index" :class="{ collapsed: index !== -1 }" aria-expanded="false"
                     :aria-controls="'collapse' + index"></button>
               </div>
               <div class="col-2 text-center pt-2">{{ question.title }}</div>
               <div class="col-4 text-center pt-2">{{ question.problemDescription }}</div>
               <div class="col-3 text-center pt-2">{{ question.askTime.slice(0, 10) }}</div>
               <div class="col-1 text-center pt-2">
                  <div v-if="question.solutionDescription == null"><i class="fa-regular fa-circle"></i></div>
                  <div v-else><i class="fa-solid fa-check"></i></div>
               </div>
            </div>
            <div :id="'collapse' + index" class="accordion-collapse collapse" :aria-labelledby="'heading' + index"
               data-bs-parent="#accordionExample">
               <div class="accordion-body">
                  {{ question.solutionDescription }}
               </div>
            </div>
         </div>
      </div>
      <div class="border-bottom mt-1"></div>
   </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";
const router = useRouter();

const memberQ = ref({});
const isSolve = ref(false);
const getMemberQInfo = async () => {
   await axios
      .get(`https://localhost:7243/MemberQResponse`, { withCredentials: true })
      .then((response) => {
         memberQ.value = response.data;
         console.log(memberQ.value);
      })
      .catch((error) => {
         console.log(error.response.status);
         if (error.response.status === 401) {
            router.push("/login");
         }
      });
};

onMounted(() => {
   getMemberQInfo();
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
