<template>
<div class="bg-gray-800 w-1/4 rounded">
    <h1 class="text-gray-100 text-xl mt-6 text-center">Sign in</h1>
    <a-form :form="form" @submit="handleSubmit" hideRequiredMark class="p-4 mx-auto">
        <a-form-item label="Email" class="mb-3">
            <a-input
            v-decorator="[
                'email',
                {
                rules: [
                {
                    type: 'email',
                    message: 'The input is not valid email!',
                },
                {
                    required: true,
                    message: 'Please input your email!',
                },
                ],
            },
            ]"
            >
                <Icon slot="prefix" icon="codicon:mail" class="text-gray-100" />
            </a-input>
        </a-form-item>
        <a-form-item label="Password" class="mb-3">
            <a-input
            v-decorator="[
                'password',
                {
                    rules: [
                        {
                            required: true,
                            message: 'Please input your Password!'
                        },
                    ]
                },
            ]"
            type="password"
            >
                <Icon slot="prefix" icon="codicon:lock" class="text-gray-100" />
            </a-input>
        </a-form-item>
        <a-button type="primary" html-type="submit" class="mx-auto block w-1/4" size="large" :disabled="isAuthenticating">
            <Icon v-if="isAuthenticating" icon="gg:spinner-two-alt" class="animate-spin h-6 w-6 mx-auto" />
            <span v-else>Sign in</span>
        </a-button>
    </a-form>
    <p class="text-center mb-2">Don't have an account? <router-link to='/register' class="text-green-400">Register here</router-link></p>
    <p class="text-center mb-5">Forgot your password? <router-link to='/forgot-password' class="text-green-400">Reset it here</router-link></p>
</div>
</template>

<script>
  import { Icon } from '@iconify/vue2';

  import api from '../api';
  import store from '../store';
  import router from '../router';

  export default {
    components: {
      Icon

    },
    beforeCreate () {
      this.form = this.$form.createForm(this, { name: 'register' });
    },
    methods: {
        handleSubmit(e) {
            e.preventDefault();

            this.form.validateFields(async (err, values) => {
                if (!err) {
                    console.log("Authenticating...");
                    this.isAuthenticating = true;

                    api.post('/public/api/account/authenticate', {
                        "email": values.email,
                        "password": values.password,
                    }).then(res => {
                        store.commit('signInUser', {id: res.data.id, email: res.data.email, jwt: res.data.jwt});

                        router.replace('/');
                    }).catch(error => {
                        error.handleGlobally && error.handleGlobally();
                    }).finally(() => {
                        this.isAuthenticating = false;
                    });
                }
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