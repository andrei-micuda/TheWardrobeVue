<template>
  <div class="text-center">
    <a-spin v-if="items === null" class="mt-24" />
    <a-list v-else-if="items.length > 0" class="w-5/6 mx-auto mt-10"
      :grid="{ gutter: 32, column: 4 }"
      :data-source="items">
      <template #renderItem="item">
        <a-list-item>
          <ItemCard :item="item" :editable="editable" />
        </a-list-item>
      </template>
    </a-list>
  </div>
</template>

<script>
  import ItemCard from './ItemCard.vue';
  export default {
    components: {
      ItemCard,
    },
    props: {
      editable: {
        type: Boolean,
        default: false
      },
      items: {
        type: Array,
      },
    },
  }
</script>

<style lang="scss" scoped>

</style>