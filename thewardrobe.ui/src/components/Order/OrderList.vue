<template>
  <div>
    <div class="flex justify-between">
      <a-pagination
        class="mb-4 text-gray-100"
        size="small"
        :total="numItems"
        :page-size="itemsPerPage"
        :pageSizeOptions="['5', '10']"
        :show-total="(total, range) => `${range[0]}-${range[1]} of ${total} items`"
        show-size-changer
        @change="handlePageChange"
        @showSizeChange="handleShowSizeChange"
      />
      <a-select default-value="newest" style="width: 120px" @change="handleOrderChange">
        <a-select-option value="newest">
          Newest
        </a-select-option>
        <a-select-option value="oldest">
          Oldest
        </a-select-option>
      </a-select>
    </div>
    <VSpinner v-if="orders === null" />
    <p v-else-if="orders.length === 0" class="text-center">No data.</p>
    <ul v-else class="font-normal space-y-4">
      <li v-for="order in orders" :key="order.id" class="p-4 bg-gray-700 rounded">
        <div class="flex justify-between items-center">
          <div>
            <p v-if="isIncoming">Order from <span class="font-bold">{{order.seller}}</span></p>
            <p v-else>Order to <span class="font-bold">{{order.buyer}}</span></p>
            <p>Placed at: {{order.whenPlaced.format('DD/MM/YYYY HH:mm')}} | Total: <span class="font-bold">{{order.total}} RON</span></p>
          </div>
          <div>
            <router-link :to="{name: 'order', params: {orderId: order.id}}">
              <a-button>details</a-button>
            </router-link>
          </div>
        </div>
      </li>
    </ul>
  </div>
</template>

<script>
  import dayjs from 'dayjs';

  import api from '../../api';
  import store from '../../store';

  import VSpinner from '../VSpinner.vue';

  export default {
    mounted () {
      this.fetchData();
    },
    props: {
      isIncoming: {
        type: Boolean,
        default: true
      },
      orderStatus: {
        type: Number
      }
    },
    watch: {
      isIncoming() {
        this.fetchData();
      }
    },
    data() {
      return {
        orders: null,
        currentPage: 1,
        itemsPerPage: 5,
        numItems: 2,
        order: 'newest'
      }
    },
    computed: {
      params() {
        return {
          type: this.isIncoming ? 0 : 1,
          page: this.currentPage,
          pageSize: this.itemsPerPage,
          orderBy: 'whenPlaced',
          order: this.order === 'newest' ? 'desc' : 'asc'
        };
      }
    },
    methods: {
      fetchData() {
        console.log(this.params)
        api.get(`/public/api/${store.state.id}/order`, {
        params: {
          ...this.params,
          status: this.orderStatus
        }
      })
      .then(res => {
        this.orders = res.data.orders.map(order => {
          return {...order, whenPlaced: dayjs(order.whenPlaced)}
        });

        this.numItems = res.data.numOrders;
      });
      },
      handleShowSizeChange(current, pageSize) {
        this.itemsPerPage = pageSize;
        this.currentPage = current;

        this.fetchData();
      },
      handlePageChange(page, pageSize) {
        this.itemsPerPage = pageSize;
        this.currentPage = page;

        this.fetchData();

      },
      handleOrderChange(orderVal) {
        this.order = orderVal;

        this.fetchData();
      },
    },
    components: {
      VSpinner,
    },
  }
</script>

<style lang="scss" scoped>

</style>