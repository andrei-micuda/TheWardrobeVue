<template>
  <div>
    <VPageHeader title="Order Details" @back="$router.back()" />
    <a-row class="w-3/5 mx-auto">
      <a-col :span="16" class="space-y-5">
        <div class="bg-gray-800 rounded p-4">
            <a-steps direction="vertical" :current="1">
              <a-step title="Pending" description="The seller needs to confirm your order." />
              <a-step title="Delivering" description="This is a description." />
              <a-step title="Completed" description="This is a description." />
            </a-steps>
        </div>
      </a-col>
      <a-col :span="8">
        <div class="bg-gray-800 ml-10 p-4 rounded space-y-6">
          <div class="space-y-1">
            <p class="text-lg">Order from:</p>
            <p class="font-bold">{{order.seller}}</p>
          </div>

          <a-divider />

          <div class="space-y-1">
            <p class="text-lg">Items:</p>
            <ul class="space-y-2 pb-4">
              <li v-for="item in order.orderItems" :key="item.id" class="flex justify-between items-center">
                <img :src="item.images[0]" alt="" class="w-16">
                <span>{{item.price}} RON</span>
              </li>
            </ul>
            <div class="flex justify-between">
              <span>Total:</span>
              <span class="font-bold">{{total}} RON</span>
            </div>
          </div>

          <a-divider />

          <div class="space-y-1">
            <p class="text-lg">Delivery Address:</p>
            <p class="font-bold">{{order.deliveryAddress.address}}</p>
            <p>{{order.deliveryAddress.city}}, {{order.deliveryAddress.country}} {{order.deliveryAddress.postalCode}}</p>
          </div>
        </div>
      </a-col>
    </a-row>
  </div>
</template>

<script>
  import VPageHeader from '../components/VPageHeader.vue';

  import api from '../api';
  import store from '../store';

  export default {
    data() {
      return {
        order: null
      }
    },
    computed: {
      total() {
        return this.order.orderItems.reduce((acc, item) => acc + item.price, 0); 
      }
    },
    mounted () {
      api.get(`/api/${store.state.id}/order/${this.$route.params.orderId}`)
      .then(res => {
        this.order = res.data;
      });
    },
    components: {
      VPageHeader
    }
  }
</script>

<style lang="scss" scoped>

</style>