<template>
  <a-carousel arrows dots-class="slick-dots slick-thumb">
    <template #customPaging="props">
      <a>
        <img :src="images[props.i].url" />
      </a>
    </template>
    <div v-for="image in images" :key="image">
      <img :src="image.url" />
    </div>
  </a-carousel>
</template>

<script>
  export default {
    props: {
      images: {
        type: Array,
      },
    },
  }
</script>

<style lang="scss" scoped>

</style>