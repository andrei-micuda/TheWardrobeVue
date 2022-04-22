<template>
  <div>
    <VPageHeader title="My Account" @back="$router.back()" />
    <a-row class="w-3/5 mx-auto">
      <a-col :span="6">
        <div class="border rounded border-gray-500 w-4/5 mx-auto h-32"></div>
      </a-col>
      <a-col :span="18" class="p-4 bg-gray-800 rounded">
        <DeliveryAddressList />
      </a-col>
    </a-row>
  </div>
</template>

<script>
  import VPageHeader from '../components/VPageHeader.vue';
  import DeliveryAddressList from '../components/Account/DeliveryAddressList.vue';

  export default {
    components: {
      VPageHeader,
      DeliveryAddressList
    },
  }
</script>

<style lang="scss" scoped>

</style>