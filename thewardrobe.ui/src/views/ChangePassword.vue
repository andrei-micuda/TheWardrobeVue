<template>
  <div
    class="flex justify-center items-center w-screen"
    :style="{ height: 'calc(100vh - 66px)' }"
  >
    <div class="bg-gray-800 xl:w-1/4 sm:w-96 w-11/12 rounded">
      <div>
        <h1 class="text-gray-100 text-xl my-6 text-center">Change password</h1>
        <p class="mx-4">Please confirm your new password below.</p>
        <a-form
          :form="form"
          @submit="handleSubmit"
          hideRequiredMark
          class="p-4 mx-auto"
        >
          <a-form-item label="Password" class="mb-3">
            <a-input
              v-decorator="[
                'password',
                {
                  rules: [
                    {
                      required: true,
                      message: 'Please input your Password!',
                    },
                    {
                      min: 6,
                      message:
                        'Your password should be at least 6 characters long.',
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
                      min: 6,
                      message:
                        'Your password should be at least 6 characters long.',
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
            class="mx-auto block w-48"
            size="large"
            :disabled="isChanging"
          >
            <Icon
              v-if="isChanging"
              icon="gg:spinner-two-alt"
              class="animate-spin h-6 w-6 mx-auto"
            />
            <span v-else>Change Password</span>
          </a-button>
        </a-form>
      </div>
    </div>
  </div>
</template>

<script>
import { Icon } from "@iconify/vue2";

import api from "../api";
import notifier from "../notifier";
import router from "../router";

export default {
  components: {
    Icon,
  },
  beforeCreate() {
    this.form = this.$form.createForm(this, { name: "reset-password" });
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
          console.log(values);
          this.isChanging = true;

          api
            .post("/public/api/account/change-password", {
              token: this.$route.query.token,
              password: values.password,
              confirmPassword: values.confirm,
            })
            .then(() => {
              notifier.success("Password successfully changed.");
              router.replace("/signIn");
            })
            .catch((error) => {
              error.handleGlobally && error.handleGlobally();
            })
            .finally(() => {
              this.isChanging = false;
            });
        }
      });
    },
  },
  data() {
    return {
      isChanging: false,
    };
  },
};
</script>

<style scoped>
h1 {
  font-family: "Josefin Sans", sans-serif;
}
</style>
