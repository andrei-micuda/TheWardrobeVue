<template>
  <div>
    <TheSellingModal @refreshGrid="handleRefreshGrid" />
    <!-- {{test}} -->
    <ItemGrid
      source="/api/itemCatalog"
      :params="params"
      :editable="true"
      :triggerFetch="triggerFetch" />
  </div>
</template>

<script>
  import store from '../../store';

  import TheSellingModal from './TheSellingModal.vue';
  import ItemGrid from '../Item/ItemGrid.vue';

  export default {
   components: {
     TheSellingModal,
     ItemGrid,
   },
   data() {
     return {
       params: { sellerIdInclude: store.state.id },
       triggerFetch: true
     }
   },
   methods: {
     handleRefreshGrid() {
       this.triggerFetch = !this.triggerFetch;
     }
   },
  }
</script>

<style lang="scss" scoped>

</style>