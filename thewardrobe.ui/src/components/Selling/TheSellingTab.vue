<template>
  <div>
    <TheSellingModal @refreshGrid="fetchItems" />
    <!-- {{test}} -->
    <ItemGrid :initialMinPrice="minPrice" :initialMaxPrice="maxPrice" :editable="true" :items="items" :numItems="numItems" :itemsPerPage="itemsPerPage" />
  </div>
</template>

<script>
  import api from '../../api';
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
       items: null,
       minPrice: null,
       maxPrice: null,
       numItems: null,
       itemsPerPage: 10
     }
   },
   methods: {
     fetchItems() {
       api.get('/api/itemCatalog', {
         params: {
           sellerIdInclude: store.state.id
         }
       })
        .then(res => {
          this.items = res.data.items;
          this.minPrice = res.data.minPrice;
          this.maxPrice = res.data.maxPrice;
          this.numItems = res.data.numItems;
        });
     }
   },
   mounted () {
     this.fetchItems();
   },
  }
</script>

<style lang="scss" scoped>

</style>