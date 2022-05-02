<template>
  <div>
    <VPageHeader title="Order Details" @back="$router.back()" />
    <a-row v-if="order" class="w-3/5 mx-auto">
      <a-col :span="16" class="space-y-5">
        <div v-if="order.status === orderStatuses.cancelled || order.status === orderStatuses.declined" class="bg-gray-800 rounded p-4">
          <a-steps id="CancelledSteps" :current="-1" class="mb-4">
            <a-step title="Pending" />
            <a-step title="Delivering" />
            <a-step
              :title="order.status === orderStatuses.cancelled ? 'Cancelled' : 'Declined'"
              status="error" />
          </a-steps>

          <p v-if="order.status === orderStatuses.cancelled">The order has been cancelled by the buyer.</p>
          <p v-else>The order has been declined by the seller.</p>
        </div>
        <div v-else class="bg-gray-800 rounded p-4">
          <a-steps :current="order.status" class="mb-4">
            <a-step :title="stepOneTitle" />
            <a-step :title="stepTwoTitle" />
            <a-step title="Completed" :status="order.status == orderStatuses.completed ? 'finish' : 'wait'" />
          </a-steps>

          <div v-if="order.status === orderStatuses.pending">
            <div v-if="isBuyer" class="space-y-1">
              <p>Waiting for the seller to accept your order.</p>
              <a-button type="danger" @click="() => handleOrderStatusChange(orderStatuses.cancelled)">Cancel Order</a-button>
            </div>

            <div v-else class="space-x-4">
              <a-button type="primary" @click="() => handleOrderStatusChange(orderStatuses.inProgress)">Accept Order</a-button>
              <a-button type="danger" @click="() => handleOrderStatusChange(orderStatuses.declined)">Decline Order</a-button>
            </div>
          </div>
          <div v-else-if="order.status === orderStatuses.inProgress" class="space-y-2">
            <p>The order is on its way.</p>
            <div v-if="isBuyer" class="space-y-1">
              <p>Please mark the order as completed once you have received it.</p>
              <a-button type="primary" @click="() => handleOrderStatusChange(orderStatuses.completed)">Mark as received</a-button>
            </div>
          </div>
          <div v-else-if="order.status === orderStatuses.completed" class="space-y-4">
            <p>The order has been completed successfully.</p>

            <div class="bg-gray-700 rounded p-4">
              <p>Please rate your experience with this {{isBuyer ? 'seller' : 'buyer'}}:</p>
              <a-rate v-model="orderRating" @change="handleRatingChange" class="text-yellow-300" />
            </div>
          </div>
        </div>
      </a-col>
      <a-col :span="8">
        <div class="bg-gray-800 ml-10 p-4 rounded space-y-6">
          <div class="space-y-1">
            <p class="text-lg">Order {{isBuyer ? 'from': 'to'}}:</p>
            <div class="flex items-center space-x-2">
              <p class="font-bold">{{isBuyer ? order.seller : order.buyer}}</p>
              <div class="flex items-center space-x-1 bg-gray-700 px-3 py-1 rounded">
                <p id="SellerRating" class="text-sm text-center">{{rating}}</p>
                <Icon icon="ant-design:star-filled" width="16" />
              </div>
            </div>
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
  import { Icon } from "@iconify/vue2";

  import api from '../api';
  import notifier from '../notifier';
  import store from '../store';

  export default {
    data() {
      return {
        account: store.state.id,
        order: null,
        orderRating: 0,
        orderStatuses: {
          pending: 0,
          inProgress: 1,
          completed: 2,
          cancelled: 3,
          declined: 4
        }
      }
    },
    computed: {
      total() {
        return this.order.orderItems.reduce((acc, item) => acc + item.price, 0); 
      },
      stepOneTitle() {
        if(this.order.status == this.orderStatuses.pending)
          return "Pending";
        return "Accepted";
      },
      stepTwoTitle() {
        if(this.order.status == this.orderStatuses.inProgress)
          return "Delivering";
        return "Delivered";
      },
      isBuyer() {
        return this.account === this.order.buyerId;
      },
      rating() {
        const buyerRating = this.order.buyerRating;
        const sellerRating = this.order.sellerRating;

        if(this.isBuyer)
          return sellerRating ? sellerRating.toFixed(2) : 'N/A';
        
        return buyerRating ? buyerRating.toFixed(2) : 'N/A';
      }
    },
    mounted () {
      this.fetchOrderData();
    },
    methods: {
      fetchOrderData() {
        api.get(`/api/${this.account}/order/${this.$route.params.orderId}`)
        .then(res => {
          this.order = res.data;
          if(this.isBuyer)
            this.orderRating = this.order.orderSellerRating;
          else
            this.orderRating = this.order.orderBuyerRating;
        });
      },
      handleOrderStatusChange(status) {
        api.patch(`/api/${this.account}/order/${this.order.id}`, { status })
          .then(() => {
            this.order.status = status;
          })
          .catch((err) => {
            console.error(err);
            this.fetchOrderData();
          });
      },
      handleRatingChange(orderRating) {
        api.patch(`/api/${this.account}/order/${this.order.id}/review`, { rating: orderRating })
          .then(() => {
            notifier.success("Successfully reviewed order!");
          });
      }
    },
    components: {
      VPageHeader,
      Icon
    }
  }
</script>

<style lang="postcss">
#CancelledSteps {
  & .ant-steps-item-wait {
    & .ant-steps-item-tail::after {
      @apply bg-gray-500 !important;
    }

    & .ant-steps-item-icon {
      @apply bg-transparent border-gray-500 !important;
    }
  }
}
</style>