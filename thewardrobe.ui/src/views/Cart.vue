<template>
  <div>
    <VPageHeader title="My Cart" @back="$router.back()" />
    <VSpinner v-if="!items" />
    <ul
      v-else-if="items && items.length > 0"
      class="w-11/12 md:w-3/5 mx-auto space-y-10"
    >
      <li v-for="group in itemGroups.keys()" :key="group">
        <CartList
          :itemGroup="itemGroups.get(group)"
          @updateItems="fetchItems"
        />
      </li>
    </ul>
    <p v-else class="text-xl text-center mt-6">No items in cart.</p>
  </div>
</template>

<script>
import VPageHeader from "../components/VPageHeader.vue";
import CartList from "../components/Cart/CartList.vue";
import VSpinner from "../components/VSpinner.vue";

import api from "../api";
import store from "../store";
import { groupBy } from "../helpers/arrayHelpers";

export default {
  data() {
    return {
      items: null,
    };
  },
  computed: {
    itemGroups() {
      return groupBy(this.items, (i) => i.sellerId);
    },
  },
  methods: {
    fetchItems() {
      api.get(`/public/api/${store.state.id}/cart`).then((res) => {
        this.items = res.data;
      });
    },
  },
  mounted() {
    this.fetchItems();
  },
  components: {
    VPageHeader,
    CartList,
    VSpinner,
  },
};
</script>

<style lang="scss" scoped></style>
