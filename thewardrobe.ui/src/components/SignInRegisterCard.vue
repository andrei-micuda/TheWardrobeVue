<template>
  <div class="background bg-cover flex-grow flex flex-col justify-center">
      <div class="absolute bg-gradient-to-b from-gray-300 to-gray-400 opacity-40 inset-0 z-0"></div>
      <!-- <div class="absolute bg-gradient-to-b from-green-500 to-green-400 opacity-40 inset-0 z-0"></div> -->
      <div class="p-8 bg-gray-700 rounded-2xl w-1/2 z-10 self-end mr-5">
          <slot></slot>
      </div>
  </div>
</template>

<script>
  export default {
    
  }
</script>

<style scoped>

</style>