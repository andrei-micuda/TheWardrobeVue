<template>
  <div class="text-white flex flex-col justify-center">
    <RegisterSuccess v-if="isRegistered" />
    <div v-else>
      <h1 class="text-lg lg:text-2xl font-bold uppercase text-left mb-6 lg:mb-12">Register</h1>
      <ValidationObserver v-slot="{ handleSubmit }">
        <form @submit.prevent="handleSubmit(onSubmit)" class="space-y-4 lg:space-y-8">
          <ValidationProvider
            rules="required|email"
            v-slot="{ errors }">
            <VInput
              v-model="email"
              label="Email"
              placeholder="test@example.com"
              :validationErrors="errors"
              class="text-sm lg:text-md"  />
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
                class="text-sm lg:text-md" />
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
                class="text-sm lg:text-md" />
            </ValidationProvider>
          </ValidationObserver>
          <VButton type="submit" class="lg:h-14 w-40" :disabled="isRegistering">
            <Icon v-if="isRegistering" icon="gg:spinner-two-alt" class="animate-spin h-6 w-6 mx-auto" />
            <span v-else>Register</span>
          </VButton>
        </form>
      </ValidationObserver>

    </div>
      </div>
</template>

<script>
  import { ValidationObserver, ValidationProvider } from 'vee-validate';
  import axios from 'axios';
  import { Icon } from '@iconify/vue2';

  import VInput from "./VInput.vue";
  import VButton from "./VButton.vue";
  import RegisterSuccess from "./RegisterSuccess.vue";
  export default {
    data() {
      return {
        email: '',
        password: '',
        rePassword: '',
        isRegistering: false,
        isRegistered: false
      }
    },
    methods: {
      onSubmit() {
        console.log("Registering...");
        this.isRegistering = true;

        axios.post('/api/account/register', {
            "email": this.email,
            "password": this.password,
            "confirmPassword": this.rePassword
        }).then(res => {
          console.log("Response of Register:");
          console.log(res);
          this.isRegistering = false;
          this.isRegistered = true;
          });
      }
    },
    components: {
      VInput,
      VButton,
      RegisterSuccess,
      ValidationObserver,
      ValidationProvider,
      Icon
    },
  }
</script>

<style scoped>
  h1 {
    font-family: 'Josefin Sans', sans-serif;
}
</style>