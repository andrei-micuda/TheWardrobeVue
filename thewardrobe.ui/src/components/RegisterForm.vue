<template>
  <div class="text-white">
    <h1 class="text-2xl font-bold uppercase text-left mb-12">Register</h1>
    <ValidationObserver v-slot="{ handleSubmit }">
      <form @submit.prevent="handleSubmit(onSubmit)" class="space-y-8">
        <ValidationProvider
          rules="required|email"
          v-slot="{ errors }">
          <VInput
            v-model="email"
            label="Email"
            placeholder="test@example.com"
            :validationErrors="errors"
            class="text-md"  />
        </ValidationProvider>
        <ValidationObserver>
          <ValidationProvider
            name="password"
            rules="required"
            v-slot="{ errors }">
            <VInput
              v-model="password"
              label="Password"
              placeholder="Your Password"
              type="password"
              :validationErrors="errors"
              class="text-md" />
          </ValidationProvider>
          <ValidationProvider
            rules="required|passwordMatch:@password"
            v-slot="{ errors }">
            <VInput
              v-model="rePassword"
              label="Confirm Password"
              placeholder="Your Password"
              type="password"
              :validationErrors="errors"
              class="text-md" />
          </ValidationProvider>
        </ValidationObserver>
        <VButton text="Register" type="submit"/>
      </form>
    </ValidationObserver>
  </div>
</template>

<script>
  import { ValidationObserver, ValidationProvider } from 'vee-validate';

  import VInput from "./VInput.vue";
  import VButton from "./VButton.vue";
  export default {
    data() {
      return {
        email: '',
        password: '',
        rePassword: ''
      }
    },
    methods: {
      onSubmit() {
        console.log("Registering...");
      }
    },
    components: {
      VInput,
      VButton,
      ValidationObserver,
      ValidationProvider
    },
  }
</script>

<style scoped>
  h1 {
    font-family: 'Josefin Sans', sans-serif;
}
</style>