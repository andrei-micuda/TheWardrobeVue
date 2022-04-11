<template>
  <div>
    <TheSellingModal @refreshGrid="fetchItems" />
    <!-- {{test}} -->
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
       items: null,
       test: null
     }
   },
   methods: {
     fetchItems() {
       api.get(`/api/${store.state.id}/itemCatalog`)
        .then(res => {
          this.items = res.data;
        });
     }
   },
   mounted () {
     this.fetchItems();
     api.get("/api/accounts/hello").then((res) => this.test = res.data);
   },
  }
</script>

<style lang="scss" scoped>

</style>