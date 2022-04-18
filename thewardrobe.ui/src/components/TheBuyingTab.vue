<template>
  <div>
    <ItemGrid
      :initialMinPrice="minPrice"
      :initialMaxPrice="maxPrice"
      :items="items"
      :numItems="numItems"
      :currentPage="currentPage"
      :itemsPerPage="itemsPerPage"
      @change="handlePageChange"
      @filterData="fetchFilteredItems" />
  </div>
</template>

<script>
  import $ from 'cash-dom';

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
       maxPrice: null,
       currentPage: 1,
       itemsPerPage: 10,
       numItems: null,
     }
   },
   methods: {
     fetchItems(params = {}) {
      params.sellerIdExclude = store.state.id;
      params.page = this.currentPage;
      params.pageSize = this.itemsPerPage;
      api.get('/api/itemCatalog', {
        params
      }).then(res => {
        this.items = res.data.items;
        this.minPrice = res.data.minPrice;
        this.maxPrice = res.data.maxPrice;
        this.numItems = res.data.numItems;
      });
     },
     fetchFilteredItems(params) {
      params.sellerIdExclude = store.state.id;
      params.page = this.currentPage;
      params.pageSize = this.itemsPerPage;
        api.get('/api/itemCatalog', {
          params
        }).then(res => {
          this.items = res.data.items;
          this.numItems = res.data.numItems;
        });
      },
      handlePageChange(page, pageSize) {
        this.currentPage = page;
        this.itemsPerPage = pageSize;

        $("#ApplyFiltersBtn").trigger("click");
      }
   },
   mounted () {
     this.fetchItems();
   },
  }
</script>

<style lang="scss" scoped>

</style>