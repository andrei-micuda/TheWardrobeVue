<template>
  <div>
    <TheSellingModal @refreshGrid="fetchItems" />
    <TheSellingGrid :items="items" />
  </div>
</template>

<script>
  import api from '../../api';
  import store from '../../store';

  import TheSellingModal from './TheSellingModal.vue';
  import TheSellingGrid from './TheSellingGrid.vue';

  export default {
   components: {
     TheSellingModal,
     TheSellingGrid,
   },
   data() {
     return {
       items: []
     }
   },
   methods: {
     fetchItems() {
       api.get("/api/itemCatalog/" + store.state.id)
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