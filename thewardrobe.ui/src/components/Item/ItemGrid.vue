<template>
  <div class="text-center">
    <a-spin v-if="items === null" class="mt-24" />
    <a-row class="w-5/6 mx-auto">
      <a-col :span="6">
        <ItemFilters :initialMinPrice="initialMinPrice" :initialMaxPrice="initialMaxPrice" @filterData="params => $emit('filterData', params)" />
      </a-col>
      <a-col :span="18">
        <a-list v-if="items.length > 0" class="w-5/6 mx-auto"
          :grid="{ gutter: 32, column: 3 }"
          :data-source="items">
          <template #renderItem="item">
            <a-list-item>
              <ItemCard :item="item" :editable="editable" />
            </a-list-item>
          </template>
        </a-list>

        <a-pagination
          :total="numItems"
          :page-size="itemsPerPage"
          show-size-changer
          @change="(page, pageSize) => $emit('change', page, pageSize)"
          @showSizeChange="onShowSizeChange"
        />
      </a-col>
    </a-row>
  </div>
</template>

<script>
  import ItemCard from './ItemCard.vue';
  import ItemFilters from './ItemFilters.vue';

  export default {
    components: {
      ItemCard,
      ItemFilters,
    },
    props: {
      currentPage: {
        type: Number,
        default: 1
      },
      editable: {
        type: Boolean,
        default: false
      },
      items: {
        type: Array,
      },
      numItems: {
        type: Number
      },
      itemsPerPage: {
        type: Number
      },
      initialMinPrice: {
        type: Number
      },
      initialMaxPrice: {
        type: Number
      },
    },
    methods: {
      onShowSizeChange(current, pageSize) {
        console.log(current, pageSize);
      },
    },
  }
</script>

<style lang="scss" scoped>

</style>