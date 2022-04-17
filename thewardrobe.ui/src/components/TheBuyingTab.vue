<template>
  <div>
    <ItemGrid :initialMinPrice="minPrice" :initialMaxPrice="maxPrice" :items="items" @filterData="fetchItems" />
  </div>
</template>

<script>
  import api from '../api';
  import store from '../store';

  import ItemGrid from './Item/ItemGrid.vue';

  export default {
   components: {
     ItemGrid,
   },
   data() {
     return {
       items: null,
       minPrice: null,
       maxPrice: null
     }
   },
   methods: {
     fetchItems(params = {}) {
       params.sellerIdExclude = store.state.id;
       api.get('/api/itemCatalog', {
         params
       }).then(res => {
          this.items = res.data.items;
          this.minPrice = res.data.minPrice;
          this.maxPrice = res.data.maxPrice;
        });
     },
    //  handleFilterData(params) {
    //     api.get('/api/itemCatalog', {
    //       params
    //     }).then(res => {this.items = res.data.items});
    //   }
   },
   mounted () {
     this.fetchItems();
   },
  }
</script>

<style lang="scss" scoped>

</style>