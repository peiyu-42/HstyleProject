<template>
    <div class="col-lg-4 py-5 px-0">
        <div class="card d-flex justify-content-center align-items-center">
            <a :href="`http://localhost:5173/product/${data.product_Id}`">
                <div class="img-sz">
                    <img :src="data.imgs[0]" class="card-img-top" alt="Product Image">
                </div>
            </a>
            <div class="card-body position-relative">
                <div class="card-title fw-bold">{{ data.product_Name }}</div>
                <span>NT$ {{ data.unitPrice.toLocaleString() }}</span>
            </div>

        </div>

    </div>
</template>
<script setup>
import { ref, onMounted } from 'vue';
const props = defineProps({
    data: Object
})

</script>
<style scoped>
.card-img-top {
    border: none;
    border-radius: 0%;
}

.fz-18 {
    font-size: 20px;
    cursor: pointer;
}

.fz-9 {
    font-size: 15px;
    color: rgb(116, 129, 143);
}

.card {
    border: none;
    border-radius: 0%;
    cursor: pointer;
}

.img-sz {
    width: 250px;
    height: 300px;
    overflow: hidden;
}

.img-sz img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: transform 1.0s ease-in-out;
}

.img-sz :hover {
    transform: scale(1.1);
    animation: rotate 1s linear infinite;

}

.img-sz :mouseout {
    opacity: 0;
    transition-delay: 1s;
    transition-timing-function: ease-out;
}

.card-body {
    width: 100%;
}
</style>