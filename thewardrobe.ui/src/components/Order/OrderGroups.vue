<template>
  <div class="bg-gray-800 p-8 w-3/5 mx-auto rounded">
    <a-collapse v-model="activeKey">
      <a-collapse-panel key="pending" header="Pending" class="font-bold text-lg">
        <p>Pending Orders</p>
      </a-collapse-panel>
      <a-collapse-panel key="inProgress" header="In Progress" class="font-bold text-lg">
        <OrderList />
      </a-collapse-panel>
      <a-collapse-panel key="finished" header="Finished Orders" class="font-bold text-lg">
        <p>Finished Orders</p>
      </a-collapse-panel>
    </a-collapse>
  </div>
</template>

<script>
  import OrderList from './OrderList.vue';

  export default {
    data() {
      return {
        activeKey: "inProgress"
      }
    },
    components: {
      OrderList,
    },
  }
</script>

<style lang="scss" scoped>

</style>