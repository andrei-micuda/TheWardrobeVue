<template>
<div class="flex justify-center items-center w-screen" :style="{ height: 'calc(100vh - 66px)' }">
  <div class="bg-gray-800 w-1/4 rounded">
    <div class="my-12 mx-6" v-if="isSent">
      <Icon icon='carbon:checkmark-outline' class="w-24 h-24 text-green-400 mx-auto mb-6" />
      <h1 class="text-2xl mb-2 text-gray-100 text-center">Reset email sent!</h1>
      <p class="text-lg text-center">Please verify your inbox and follow the instructions in order reset your password.</p>
    </div>
    <div v-else>
      <h1 class="text-gray-100 text-xl my-6 text-center">Forgot password</h1>
      <p class="mx-4">Please enter your account email below in order to reset your password.</p>
      <a-form :form="form" @submit="handleSubmit" hideRequiredMark class="p-4 mx-auto">
        <a-form-item class="mb-3">
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
        <a-button type="primary" html-type="submit" class="mx-auto block w-1/4" size="large" :disabled="isSending">
            <Icon v-if="isSending" icon="gg:spinner-two-alt" class="animate-spin h-6 w-6 mx-auto" />
            <span v-else>Reset</span>
        </a-button>
      </a-form>
    </div>
  </div>
</div>
</template>

<script>
 import { Icon } from '@iconify/vue2';

  import api from '../api';

  export default {
    components: {
      Icon
    },
    beforeCreate () {
      this.form = this.$form.createForm(this, { name: 'forgot-password' });
    },
    methods: {
        handleSubmit(e) {
            e.preventDefault();

            this.form.validateFields(async (err, values) => {
                if (!err) {
                    this.isSending = true;

                    api.post('/public/api/account/forgot-password', {
                        "email": values.email,
                    }).then(() => {
                        this.isSent = true;
                    }).catch(error => {
                        error.handleGlobally && error.handleGlobally();
                    }).finally(() => {
                        this.isSending = false;
                    });
                }
            });
        }
    },
    data() {
      return {
        isSending: false,
        isSent: false
      }
    },
  }
</script>

<style scoped>
  h1 {
    font-family: 'Josefin Sans', sans-serif;
}
</style>