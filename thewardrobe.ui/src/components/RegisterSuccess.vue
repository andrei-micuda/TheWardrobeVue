<template>
  <div>
    <Icon icon='carbon:checkmark-outline' class="w-24 h-24 text-green-400 mx-auto mb-6" />
    <h1 class="text-4xl mb-2">You have successfully registered!</h1>
    <p class="text-lg">Please verify your email in order to activate your account.</p>
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
h1 {
    font-family: 'Josefin Sans', sans-serif;
}
</style>