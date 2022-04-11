<template>
  <a-list class="w-5/6 mx-auto mt-10"
    :grid="{ gutter: 32, column: 4 }"
    :data-source="items"
    :loading="items === null">
    <template #renderItem="item">
      <a-list-item>
        <SellingItemCard :item="item" />
      </a-list-item>
    </template>
  </a-list>
</template>

<script>
  import SellingItemCard from './SellingItemCard.vue';
  export default {
    components: {
      SellingItemCard,
    },
    props: {
      items: {
        type: Array,
      },
    },
  }
</script>

<style lang="scss" scoped>

</style>