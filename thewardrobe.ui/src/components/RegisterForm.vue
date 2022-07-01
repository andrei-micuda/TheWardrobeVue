<template>
  <div class="bg-gray-800 xl:w-1/4 sm:w-96 w-11/12 rounded" id="RegisterCard">
    <RegisterSuccess v-if="isRegistered" />
    <div v-else>
      <h1 class="text-gray-100 text-xl mt-6 text-center">Register</h1>
      <a-form
        :form="form"
        @submit="handleSubmit"
        hideRequiredMark
        class="p-4 mx-auto"
      >
        <a-form-item label="Email" class="mb-3">
          <a-input
            v-decorator="[
              'email',
              {
                rules: [
                  {
                    type: 'email',
                    message: 'The input is not a valid email!',
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
                    message: 'Please input your password!',
                  },
                  {
                    min: 6,
                    message: 'Password must be at least 6 characters long!',
                  },
                  {
                    validator: validateToNextPassword,
                  },
                ],
              },
            ]"
            type="password"
          >
            <Icon slot="prefix" icon="codicon:lock" class="text-gray-100" />
          </a-input>
        </a-form-item>
        <a-form-item label="Confirm Password" class="mb-5">
          <a-input
            v-decorator="[
              'confirm',
              {
                rules: [
                  {
                    required: true,
                    message: 'Please confirm your password!',
                  },
                  {
                    validator: compareToFirstPassword,
                  },
                ],
              },
            ]"
            type="password"
            @blur="handleConfirmBlur"
          >
            <Icon slot="prefix" icon="codicon:lock" class="text-gray-100" />
          </a-input>
        </a-form-item>
        <a-button
          type="primary"
          html-type="submit"
          class="mx-auto block w-32"
          size="large"
          :disabled="isRegistering"
        >
          <Icon
            v-if="isRegistering"
            icon="gg:spinner-two-alt"
            class="animate-spin h-6 w-6 mx-auto"
          />
          <span v-else>Register</span>
        </a-button>
      </a-form>
      <p class="text-center mb-5">
        Or
        <router-link to="/signIn" class="text-green-400">sign in</router-link>
        if you already have an account.
      </p>
    </div>
  </div>
</template>

<script>
import { Icon } from "@iconify/vue2";

import api from "../api";

import RegisterSuccess from "./RegisterSuccess.vue";

export default {
  data() {
    return {
      isRegistering: false,
      isRegistered: false,
    };
  },
  beforeCreate() {
    this.form = this.$form.createForm(this, { name: "register" });
  },
  methods: {
    handleConfirmBlur(e) {
      const value = e.target.value;
      this.confirmDirty = this.confirmDirty || !!value;
    },
    compareToFirstPassword(rule, value, callback) {
      const form = this.form;
      if (value && value !== form.getFieldValue("password")) {
        callback("Passwords do not match!");
      } else {
        callback();
      }
    },
    validateToNextPassword(rule, value, callback) {
      const form = this.form;
      if (value && this.confirmDirty) {
        form.validateFields(["confirm"], { force: true });
      }
      callback();
    },
    handleSubmit(e) {
      e.preventDefault();

      this.form.validateFields(async (err, values) => {
        if (!err) {
          this.isRegistering = true;

          api
            .post("/public/api/account/register", {
              email: values.email,
              password: values.password,
              confirmPassword: values.confirm,
            })
            .then(() => {
              this.isRegistering = false;
              this.isRegistered = true;
            });
        }
      });
    },
  },
  components: {
    RegisterSuccess,
    Icon,
  },
};
</script>

<style>
h1 {
  font-family: "Josefin Sans", sans-serif;
}
</style>
