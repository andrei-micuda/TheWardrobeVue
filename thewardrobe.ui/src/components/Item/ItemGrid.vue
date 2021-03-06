<template>
  <div class="text-center">
    <a-row class="w-5/6 mx-auto">
      <a-col :span="24" :xl="{ span: 6 }" class="mb-6">
        <ItemFilters
          ref="itemFilters"
          :initialMinPrice="minPrice"
          :initialMaxPrice="maxPrice"
          @filterData="fetchFilteredItems"
          @filtersCleared="fetchItems"
          @setPaging="
            (page) => {
              currentPage = page;
            }
          "
        />
      </a-col>
      <a-col :span="24" :xl="{ span: 18 }">
        <a-row
          v-if="items"
          class="w-5/6 mx-auto"
          type="flex"
          justify="space-between"
        >
          <a-col>
            <a-pagination
              class="mb-4 text-gray-100"
              size="small"
              :total="numItems"
              :page-size="itemsPerPage"
              :show-total="
                (total, range) => `${range[0]}-${range[1]} of ${total} items`
              "
              :current="currentPage"
              show-size-changer
              @change="handlePageChange"
              @showSizeChange="onShowSizeChange"
            />
          </a-col>
          <a-col>
            <a-select
              default-value="newest"
              style="width: 120px"
              @change="handleOrderChange"
            >
              <a-select-option value="newest"> Newest </a-select-option>
              <a-select-option value="priceAsc"> Price 🠕 </a-select-option>
              <a-select-option value="priceDesc"> Price 🠗 </a-select-option>
            </a-select>
          </a-col>
        </a-row>

        <VSpinner v-if="!items" class="mt-12" />

        <a-list
          v-else-if="items.length > 0"
          class="w-5/6 mx-auto mt-6"
          :grid="{ gutter: 32, xs: 1, sm: 2, lg: 3 }"
          :data-source="items"
        >
          <template #renderItem="item">
            <a-list-item>
              <ItemCard :item="item" :editable="editable" />
            </a-list-item>
          </template>
        </a-list>
        <p v-else>No items.</p>
      </a-col>
    </a-row>
  </div>
</template>

<script>
import $ from "cash-dom";

import ItemCard from "./ItemCard.vue";
import ItemFilters from "./ItemFilters.vue";
import VSpinner from "../VSpinner.vue";

import api from "../../api";
import store from "../../store";

export default {
  components: {
    ItemCard,
    ItemFilters,
    VSpinner,
  },
  props: {
    source: {
      type: String,
    },
    params: {
      type: Object,
    },
    editable: {
      type: Boolean,
      default: false,
    },
    triggerFetch: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    triggerFetch() {
      this.fetchItems();
    },
  },
  data() {
    return {
      items: null,
      minPrice: null,
      maxPrice: null,
      currentPage: 1,
      itemsPerPage: 10,
      numItems: null,
      order: "newest",
    };
  },
  methods: {
    setCurrentPage(val) {
      this.currentPage = val;
    },
    setOrderParams(params) {
      if (this.order === "newest") {
        params.orderBy = "whenAdded";
        params.order = "desc";
      } else if (this.order === "priceAsc") {
        params.orderBy = "price";
        params.order = "asc";
      } else if (this.order === "priceDesc") {
        params.orderBy = "price";
        params.order = "desc";
      }

      return params;
    },
    onShowSizeChange(current, pageSize) {
      this.itemsPerPage = pageSize;
      this.currentPage = current;

      $("#ApplyFiltersBtn").trigger("click");
    },
    handlePageChange(page, pageSize) {
      this.currentPage = page;
      this.itemsPerPage = pageSize;

      this.$refs.itemFilters.getFilteredData(page);
    },
    handleOrderChange(orderVal) {
      this.order = orderVal;

      $("#ApplyFiltersBtn").trigger("click");
    },
    fetchItems(otherParams = {}) {
      let finalParams = { ...this.params, ...otherParams };
      finalParams.requesterId = store.state.id;
      finalParams.page = this.currentPage;
      finalParams.pageSize = this.itemsPerPage;
      finalParams.onlyAvailable = true;
      finalParams = this.setOrderParams(finalParams);
      api
        .get(this.source, {
          params: finalParams,
        })
        .then((res) => {
          this.items = res.data.items;
          this.minPrice = res.data.minPrice;
          this.maxPrice = res.data.maxPrice;
          this.numItems = res.data.numItems;
        });
    },
    fetchFilteredItems(otherParams) {
      let finalParams = {
        requesterId: store.state.id,
        page: this.currentPage,
        pageSize: this.itemsPerPage,
        ...this.params,
        ...otherParams,
      };
      finalParams = this.setOrderParams(finalParams);

      api
        .get(this.source, {
          params: finalParams,
        })
        .then((res) => {
          this.items = res.data.items;
          this.numItems = res.data.numItems;
        });
    },
  },
  mounted() {
    const itemFilters = store.state.itemFilters;
    if (itemFilters === null) {
      this.fetchItems();
    } else {
      this.fetchFilteredItems(itemFilters);
      this.minPrice = itemFilters.initialMinPrice;
      this.maxPrice = itemFilters.initialMaxPrice;
    }
  },
};
</script>

<style lang="scss" scoped></style>
