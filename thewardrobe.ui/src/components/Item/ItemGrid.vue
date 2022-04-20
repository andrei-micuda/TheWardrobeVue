<template>
  <div class="text-center">
    <a-spin v-if="items === null" class="mt-24" />
    <a-row class="w-5/6 mx-auto">
      <a-col :span="6">
        <ItemFilters :initialMinPrice="minPrice" :initialMaxPrice="maxPrice" @filterData="fetchFilteredItems" />
      </a-col>
      <a-col v-if="items && items.length > 0" :span="18">
        <a-row class="w-5/6 mx-auto" type="flex" justify="space-between">
          <a-col>
            <a-pagination
              class="mb-4 text-gray-100"
              :total="numItems"
              :page-size="itemsPerPage"
              :show-total="(total, range) => `${range[0]}-${range[1]} of ${total} items`"
              show-size-changer
              @change="handlePageChange"
              @showSizeChange="onShowSizeChange"
            />
          </a-col>
          <a-col>
            <a-select default-value="newest" style="width: 120px" @change="handleOrderChange">
              <a-select-option value="newest">
                Newest
              </a-select-option>
              <a-select-option value="priceAsc">
                Price 🠕
              </a-select-option>
              <a-select-option value="priceDesc">
                Price 🠗
              </a-select-option>
            </a-select>
          </a-col>
        </a-row>

        <a-list class="w-5/6 mx-auto"
          :grid="{ gutter: 32, column: 3 }"
          :data-source="items">
          <template #renderItem="item">
            <a-list-item>
              <ItemCard :item="item" :editable="editable" />
            </a-list-item>
          </template>
        </a-list>
      </a-col>
    </a-row>
  </div>
</template>

<script>
  import $ from "cash-dom";

  import ItemCard from './ItemCard.vue';
  import ItemFilters from './ItemFilters.vue';

  import api from '../../api';

  export default {
    components: {
      ItemCard,
      ItemFilters,
    },
    props: {
      source: {
        type: String
      },
      params: {
        type: Object
      },
      editable: {
        type: Boolean,
        default: false
      },
      triggerFetch: {
        type: Boolean,
        default: false
      }
    },
    watch: {
      triggerFetch() {
        console.log("Triggering new fetch")
        this.fetchItems();
      }
    },
    data() {
      return {
        items: null,
        minPrice: null,
        maxPrice: null,
        currentPage: 1,
        itemsPerPage: 10,
        numItems: null,
        order: 'newest'
      }
    },
    methods: {
      setOrderParams(params) {
        if(this.order === 'newest')
        {
          params.orderBy = 'whenAdded';
          params.order = 'desc';
        }
        else if(this.order === 'priceAsc')
        {
          params.orderBy = 'price';
          params.order = 'asc';
        }
        else if(this.order === 'priceDesc')
        {
          params.orderBy = 'price';
          params.order = 'desc';
        }

        return params;
      },
      onShowSizeChange(current, pageSize) {
        console.log(current, pageSize);
      },
      handlePageChange(page, pageSize) {
        this.currentPage = page;
        this.itemsPerPage = pageSize;

        $("#ApplyFiltersBtn").trigger("click");
      },
      handleOrderChange(orderVal) {
        this.order = orderVal;

        $("#ApplyFiltersBtn").trigger("click");
      },
      fetchItems(otherParams = {}) {
        let finalParams = {...this.params, ...otherParams};
        finalParams.page = this.currentPage;
        finalParams.pageSize = this.itemsPerPage;
        finalParams = this.setOrderParams(finalParams);
        api.get(this.source, {
          params: finalParams
        }).then(res => {
          this.items = res.data.items;
          this.minPrice = res.data.minPrice;
          this.maxPrice = res.data.maxPrice;
          this.numItems = res.data.numItems;
        });
      },
      fetchFilteredItems(otherParams) {
        console.log("Getting filtered data")
        let finalParams = {...this.params, ...otherParams};
        finalParams.page = this.currentPage;
        finalParams.pageSize = this.itemsPerPage;
        finalParams = this.setOrderParams(finalParams);

        api.get(this.source, {
            params: finalParams
          }).then(res => {
            this.items = res.data.items;
            this.numItems = res.data.numItems;
          });
        },
    },
    mounted () {
      this.fetchItems();
    },
  }
</script>

<style lang="scss" scoped>

</style>