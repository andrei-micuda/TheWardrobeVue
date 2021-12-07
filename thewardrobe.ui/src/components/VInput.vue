<template>
  <div>
    <div class="w-full flex justify-center mb-4">
      <div class="space-y-2 w-full">
        <label class="block text-left uppercase font-bold lg:mb-2">{{label}}</label>
        <input
          :value="value"
          v-on:input="$emit('input', $event.target.value)"
          :type="type"
          :placeholder="placeholder"
        class="
          border-0 px-3 py-3 placeholder-blueGray-300 text-gray-600 bg-white rounded text-xs lg:text-sm shadow
          focus:outline-none focus:ring focus:ring-green-400 w-full ease-linear transition-all duration-150">
          <ValidationError v-if="validationErrors">{{validationErrors[0]}}</ValidationError>
      </div>
    </div>
  </div>
</template>

<script>
  import ValidationError from "./ValidationError.vue";
  export default {
    data() {
      return {
        message: 'Welcome to Vue!'
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
      },
      value: {type:String},
      validationErrors: {
        type: Array,
      }
    },
    components: {
      ValidationError
    },
  }
</script>

<style scoped>

</style>