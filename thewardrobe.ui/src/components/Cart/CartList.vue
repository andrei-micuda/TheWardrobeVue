<template>
  <div class="bg-gray-800 rounded p-8 mb-5">
    <div class="flex items-center space-x-2 mb-5">
      <p class="text-xl">
        Items from <span class="font-bold">{{ itemGroup[0].seller }}</span>
      </p>
      <div class="flex items-center space-x-1 bg-gray-700 px-3 py-1 rounded">
        <p id="SellerRating" class="text-sm text-center">
          {{
            itemGroup[0].sellerRating
              ? itemGroup[0].sellerRating.toFixed(2)
              : "N/A"
          }}
        </p>
        <Icon icon="ant-design:star-filled" width="16" />
      </div>
    </div>

    <ItemList
      :items="itemGroup"
      :isDeletable="true"
      @deleteItem="(itemId) => deleteItem(itemId)"
    />

    <!-- <ul class="space-y-5">
      <li v-for="item in itemGroup" :key="item.id" class="bg-gray-700">
        <a-row type="flex" class="p-2 md:p-4 items-center md:items-start">
          <a-col :span="5">
            <img :src="item.images[0]" class="w-full" />
          </a-col>
          <a-col :span="10" class="pl-4 md:p-4">
            <p class="font-semibold text-lg">{{item.productName}}</p>
          </a-col>
          <a-col :span="9" class="flex flex-col justify-between">
            <button class="self-end" @click="() => deleteItem(item.id)">
              <Icon icon="charm:cross" height="36" />
            </button>
            <p class="text-right text-xl">{{item.price}} RON</p>
          </a-col>
        </a-row>
      </li>
    </ul> -->
    <div class="flex justify-between items-center mt-10 text-xl">
      <p>
        Total: <span class="font-bold">{{ total }} RON</span>
      </p>
      <router-link
        :to="{
          name: 'checkout',
          params: { sellerId: this.itemGroup[0].sellerId },
        }"
      >
        <a-button type="primary" size="large">Go to Checkout</a-button>
      </router-link>
    </div>
  </div>
</template>

<script>
import { Icon } from "@iconify/vue2";
import ItemList from "../Item/ItemList.vue";

import api from "../../api";
import store from "../../store";

export default {
  props: {
    itemGroup: {
      type: Array,
    },
  },
  computed: {
    total() {
      return this.itemGroup.reduce((acc, item) => acc + item.price, 0);
    },
  },
  methods: {
    deleteItem(itemId) {
      api
        .delete(`/public/api/${store.state.id}/cart`, {
          data: { itemId },
        })
        .then(() => {
          this.$emit("updateItems");
        });
    },
  },
  components: {
    Icon,
    ItemList,
  },
};
</script>

<style lang="scss" scoped></style>
