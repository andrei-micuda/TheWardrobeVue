<template>
  <div>
    <ItemGrid source="/public/api/itemCatalog" :params="params" />
  </div>
</template>

<script>
import store from "../store";

import ItemGrid from "./Item/ItemGrid.vue";

export default {
  components: {
    ItemGrid,
  },
  data() {
    return {
      params: { sellerIdExclude: store.state.id },
    };
  },
};
</script>

<style lang="scss" scoped></style>
