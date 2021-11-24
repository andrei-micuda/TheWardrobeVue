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
      <VButton type="submit" class="h-14 w-40" :disabled="isAuthenticating">
        <Icon v-if="isAuthenticating" icon="gg:spinner-two-alt" class="animate-spin h-6 w-6 mx-auto" />
        <span v-else>Sign in</span>
      </VButton>
    </form>
    </ValidationObserver>
  </div>
</template>

<script>
  import { ValidationObserver, ValidationProvider } from 'vee-validate';
  import axios from 'axios';
  import { Icon } from '@iconify/vue2';

  import store from '../store';
  import router from '../router';

  import VInput from './VInput.vue';
  import VButton from './VButton.vue';

  export default {
    components: {
      VInput,
      VButton,
      ValidationObserver,
      ValidationProvider,
      Icon

    },
    computed: {
      user() {
        return store.state.user; 
      }
    },
    methods: {
      onSubmit() {
        console.log("Authenticating...");
        this.isAuthenticating = true;

        axios.post('/accounts/authenticate', {
            "email": this.email,
            "password": this.password,
        }).then(res => {
          store.commit('signInUser', {email: res.data.email, jwt: res.data.jwt});

          this.isAuthenticating = false;
          this.isAuthenticated = true;

          router.push('/');
          });
      }
    },
    data() {
      return {
        email: '',
        password: '',
        isAuthenticating: false,
        isAuthenticated: false
      }
    },
  }
</script>

<style scoped>
  h1 {
    font-family: 'Josefin Sans', sans-serif;
}
</style>