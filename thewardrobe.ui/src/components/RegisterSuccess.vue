<template>
  <div class="my-12">
    <Icon icon='carbon:checkmark-outline' class="w-24 h-24 text-green-400 mx-auto mb-6" />
    <h1 class="text-2xl mb-2 text-gray-100 text-center">You have successfully registered!</h1>
    <p class="text-lg text-center">Please verify your email in order to verify your account.</p>
  </div>
</template>

<script>
  import { Icon } from '@iconify/vue2';
  export default {
    components: {
      Icon,
    },
  }
</script>

<style scoped>
</style>