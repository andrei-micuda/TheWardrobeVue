<template>
  <div>
    <VPageHeader title="My Account" @back="$router.back()" />
  </div>
</template>

<script>
  import VPageHeader from '../components/VPageHeader.vue';

  export default {
    components: {
      VPageHeader,
    },
  }
</script>

<style lang="scss" scoped>

</style>