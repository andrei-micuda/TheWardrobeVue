<template>
  <div>
    <ItemGrid :items="items" />
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
       test: null
     }
   },
   methods: {
     fetchItems() {
       api.get('/api/itemCatalog', {
         params: {
           sellerIdExclude: store.state.id
         }
       }).then(res => {
          this.items = res.data;
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