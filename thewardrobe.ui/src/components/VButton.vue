<template>
  <button class="p-2 pt-3 px-5 bg-transparent border-2 border-green-400 text-green-400 text-lg rounded-lg transition-colors duration-400 transform hover:bg-green-400 hover:text-gray-100 focus:border-4 focus:border-green-400 align-baseline">{{text}}</button>
</template>

<script>
  export default {
    props: {
      text: {
        type: String,
        default: "Button"
      },
    },
  }
</script>

<style scoped>

</style>