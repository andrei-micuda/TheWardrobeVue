<template>
  <a-popover>
    <template slot="content">
      <p>Content</p>
      <p>Content</p>
    </template>
    <a class="flex items-center text-gray-100 hover:text-gray-200">
      <Icon icon="eva:shopping-cart-outline" class="h-6 w-6" />
    </a>
  </a-popover>
</template>

<script>
import api from "../../api";
import store from "../../store";

import { Icon } from "@iconify/vue2";
export default {
  data() {
    return {
      items: null,
    };
  },
  mounted() {
    api.get(`/public/api/${store.state.id}/cart`).then((res) => {
      this.items = res.data;
    });
  },
  components: {
    Icon,
  },
};
</script>

<style lang="scss" scoped></style>
