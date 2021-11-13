<template>
  <div>
    <div class="w-full flex justify-center mb-4">
      <div class="space-y-2 w-full">
        <label class="block text-left uppercase text-blueGray-600 font-bold mb-2">{{label}}</label>
        <input :type="type" :placeholder="placeholder"
        class="
          border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow
          focus:outline-none focus:ring focus:ring-green-400 w-full ease-linear transition-all duration-150">
      </div>
    </div>
  </div>
</template>

<script>
  export default {
    data() {
      return {
        message: 'Welcome to Vue!',
      };
    },
    methods: {
      doSomething() {
        alert('Hello!');
      }
    },
    props: {
      label: {
        type: String,
        default: "Label"
      },
      placeholder: {
        type: String,
        default: "Placeholder"
      },
      type: {
        type: String,
        default: "text"
      }
    }
  }
</script>

<style scoped>

</style>