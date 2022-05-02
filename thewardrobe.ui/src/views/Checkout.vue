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

        <DeliveryAddressRadio @deliveryAddressChange="handleDeliveryAddressChange" />
      </a-col>
      <a-col :span="12">
        <div class="bg-gray-800 ml-10 p-4 rounded text-lg space-y-6">
          <p class="font-bold text-xl mb-6">Order Summary</p>
          
          <div>
            <p>Sold by:</p>
            <div class="flex items-center space-x-2">
              <p class="font-bold">{{seller}}</p>
              <div class="flex items-center space-x-1 bg-gray-700 px-3 py-1 rounded">
                <p id="SellerRating" class="text-sm text-center">{{sellerRating ? sellerRating.toFixed(2) : 'N/A'}}</p>
                <Icon icon="ant-design:star-filled" width="16" />
              </div>
            </div>
          </div>
          <div>
            <p>Total:</p>
            <p class="font-bold">{{total}} RON</p>
          </div>
          <div>
            <p>Delivery Address:</p>
            <div v-if="deliveryAddress">
              <p class="font-bold">{{deliveryAddress.address}}</p>
              <p>{{deliveryAddress.city}}, {{deliveryAddress.country}} {{deliveryAddress.postalCode}}</p>
            </div>
          </div>
          <a-button type="primary" size="large" @click="placeOrder">Place Order</a-button>
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
  import notifier from '../notifier';
  import store from '../store';

  export default {
    data() {
      return {
        sellerId: this.$route.params.sellerId,
        items: null,
        deliveryAddress: null
      }
    },
    computed: {
      total() {
        return this.items.reduce((acc, item) => acc + item.price, 0); 
      },
      seller() {
        return this.items[0].seller;
      },
      sellerRating() {
        return this.items[0].sellerRating;
      }
    },
    mounted () {
      this.fetchItems();
    },
    methods: {
      placeOrder() {
        api.post(`/api/${store.state.id}/order`, {
          sellerId: this.sellerId,
          deliveryAddressId: this.deliveryAddress.id,
          items: this.items.map(i => i.id)
        })
          .then(() => {
            this.$router.replace("/orders");
          });
      },
      fetchItems() {
        api.get(`/api/${store.state.id}/cart`, {params: { sellerId: this.sellerId }})
          .then(res => {
            this.items = res.data;
          });
      },
      deleteItem(itemId) {
        api.delete(`/api/${store.state.id}/cart`, {
            data: { itemId }
          })
          .then(() => {
            notifier.success("Removed item from cart.");
            this.fetchItems();
          });
      },
      handleDeliveryAddressChange(addr) {
        this.deliveryAddress = addr;
      }
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