<template>
  <div>
    <div class="flex justify-between">
      <a-pagination
        class="mb-4 text-gray-100"
        :total="numItems"
        :page-size="itemsPerPage"
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
    <ul class="font-normal space-y-4">
      <li v-for="order in orders" :key="order.id" class="p-4 bg-gray-700 rounded">
        <div class="flex justify-between items-center">
          <div>
            <p>Order from <span class="font-bold">{{order.seller}}</span></p>
            <p>Placed at: {{order.whenPlaced.format('DD/MM/YYYY HH:mm')}} | Total: <span class="font-bold">{{order.total}} RON</span></p>
          </div>
          <div>
            <a-button @click="() => {}">details</a-button>
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

  export default {
    mounted () {
      api.get(`/api/${store.state.id}/order`)
      .then(res => {
        console.log(res.data)
        this.orders = res.data.map(order => {
          return {...order, whenPlaced: dayjs(order.whenPlaced)}
        });
      });
    },
    data() {
      return {
        orders: null,
        currentPage: 1,
        itemsPerPage: 10,
        numItems: 2,
        order: 'newest'
      }
    },
    methods: {
      handleShowSizeChange(current, pageSize) {
        this.itemsPerPage = pageSize;
        this.currentPage = current;
      },
      handlePageChange(page, pageSize) {
        this.itemsPerPage = pageSize;
        this.currentPage = page;
      },
      handleOrderChange(orderVal) {
        this.order = orderVal;
      },
    },
  }
</script>

<style lang="scss" scoped>

</style>