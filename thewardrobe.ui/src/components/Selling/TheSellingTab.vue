<template>
  <div>
    <TheSellingModal @refreshGrid="fetchItems" />
    <!-- {{test}} -->
    <ItemGrid :editable="true" :items="items" />
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
       test: null
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