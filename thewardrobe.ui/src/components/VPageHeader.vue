<template>
  <div class="w-screen flex items-center p-5 justify-between">
    <div class="flex items-center">
      <button @click="$emit('back')">
        <Icon icon="eva:arrow-back-outline" width="32" height="32"
        class="mr-5 cursor-pointer rounded-full hover:bg-gray-800" />
      </button>
      <span class="inline-flex items-center text-xl relative top-0.5">{{title}}</span>
    </div>
    
    <VButton @click="$emit('apply')">Apply</VButton>
  </div>
</template>

<script>
  import { Icon } from '@iconify/vue2';
  import VButton from './VButton.vue';
  export default {
    props: {
      title: {
        type: String,
      },
    },
    components: {
      Icon,
      VButton
    },
  }
</script>

<style lang="postcss">

</style>