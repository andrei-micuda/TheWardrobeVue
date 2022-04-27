<template>
  <div>
    <VPageHeader title="Checkout" @back="$router.back()" />
    <a-row class="w-3/5 mx-auto">
      <a-col :span="12" class="space-y-5">
        <div class="bg-gray-800 rounded p-4">
          <p class="text-lg pb-4">Items</p>
          <ul class="bg-gray-700 p-4">
            <li class="item" v-for="item in items" :key="item.id">
              <a-row type="flex">
                <a-col :span="5">
                  <img :src="item.images[0]" class="w-full" />
                </a-col>
                <a-col :span="10" class="p-4">
                  <p class="font-semibold text-lg">{{item.productName}}</p>
                </a-col>
                <a-col :span="9" class="flex flex-col justify-between">
                  <button class="self-end" @click="() => deleteItem(item.id)">
                    <Icon icon="charm:cross" height="36" />
                  </button>
                  <p class="text-right text-xl">{{item.price}} RON</p>
                </a-col>
              </a-row>
            </li>
          </ul>
        </div>

        <DeliveryAddressRadio />
      </a-col>
      <a-col :span="12">
        <div class="bg-gray-800 ml-10 p-4 rounded text-lg space-y-4">
          <p class="font-bold text-xl">Order Summary</p>
          <p>Total: <span class="font-bold">{{total}} RON</span></p>
          <p>Delivery Address:</p>
        </div>
      </a-col>
    </a-row>
  </div>
</template>

<script>
  import VPageHeader from '../components/VPageHeader.vue';
  import DeliveryAddressRadio from '../components/Checkout/DeliveryAddressRadio.vue';
  import { Icon } from '@iconify/vue2';

  import api from '../api';
  import store from '../store';

  export default {
    data() {
      return {
        sellerId: this.$route.params.sellerId,
        items: null
      }
    },
    computed: {
      total() {
        return this.items.reduce((acc, item) => acc + item.price, 0); 
      }
    },
    mounted () {
      api.get(`/api/${store.state.id}/cart`, {params: { sellerId: this.sellerId }})
        .then(res => {
          this.items = res.data;
        });
    },
    components: {
      VPageHeader,
      Icon,
      DeliveryAddressRadio
    },
  }
</script>

<style lang="postcss" scoped>
  .item:not(:last-of-type) {
    @apply border-b-2 border-gray-600 mb-4 pb-4 !important;
  }
</style>