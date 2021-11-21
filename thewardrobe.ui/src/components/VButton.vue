<template>
  <button
    :type="type"
    :disabled="disabled"
    class="p-2 pt-3 px-5 bg-transparent border-2 border-green-400 text-green-400 text-lg rounded-lg transition-colors duration-400 transform hover:bg-green-400 hover:text-gray-100 focus:border-4 focus:border-green-400 align-baseline">
      <slot></slot>
    </button>
</template>

<script>
  export default {
    props: {
      type: String,
      disabled: Boolean
    },
  }
</script>

<style scoped>

</style>