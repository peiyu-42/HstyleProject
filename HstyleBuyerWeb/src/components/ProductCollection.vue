<template>
    <div class="card  col-md-4 mx-3 my-3">
        <router-link :to="'/product/' + data.productId" class="card">
            <img :src="data.images" alt="Card image">
            <div class="card-overlay">
                <h3>{{ data.productName }}</h3>
            </div>
        </router-link>
    </div>
</template>
<script setup>
const props = defineProps({
    data: Object,
});

</script>
<style scoped>
.card {
    position: relative;
    width: 350px;
    height: 450px;
    overflow: hidden;
    border: none;
    padding-bottom: 2%;
    border-radius: 0%;
}

.card img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: all 0.3s ease;
}

.card:hover img {
    filter: brightness(70%);
}

.card-overlay {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    text-align: center;
    color: #fff;
    opacity: 0;
    transition: all 0.3s ease;
}

.card:hover .card-overlay {
    opacity: 1;
}

.card-overlay h3 {
    font-size: 1.5rem;
    margin-bottom: 0.5rem;
}

.card-overlay p {
    font-size: 1rem;
    margin: 0;
}
</style>