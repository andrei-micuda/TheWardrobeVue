<template>
  <div class="text-white">
    <h1 class="text-2xl font-bold uppercase text-left mb-12">Sign in</h1>
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
        <ValidationProvider
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
        <VButton type="submit">Sign in</VButton>
      </form>
    </ValidationObserver>
  </div>
</template>

<script>
  import { ValidationObserver, ValidationProvider } from 'vee-validate';

  import store from '../store';
  import VInput from "./VInput.vue";
  import VButton from "./VButton.vue";

  export default {
    components: {
      VInput,
      VButton,
      ValidationObserver,
      ValidationProvider
    },
    computed: {
      user() {
        return store.state.user; 
      }
    },
    methods: {
      onSubmit() {
        console.log();
        console.log("Signing in...");
        // store.commit("signInUser", 'micu');
      }
    },
    data() {
      return {
        email: '',
        password: ''
      }
    },
  }
</script>

<style scoped>
  h1 {
    font-family: 'Josefin Sans', sans-serif;
}
</style>