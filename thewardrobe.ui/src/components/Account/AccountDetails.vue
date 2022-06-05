<template>
  <section id="AccountDetailsSection">
    <div
      class="flex items-center cursor-pointer mb-2"
      :class="{ collapsed: isCollapsed }"
      @click="toggleCollapse"
    >
      <Icon
        icon="dashicons:arrow-down-alt2"
        width="20"
        class="mr-2 transition-all"
      />
      <p class="text-xl">Account Details</p>
    </div>
    <a-form
      :form="form"
      @submit="handleSubmit"
      hideRequiredMark
      class="bg-gray-700 p-4 mb-5"
      :class="{ hidden: isCollapsed }"
    >
      <a-form-item
        label="First Name"
        class="mr-6"
        :style="{ display: 'inline-block', width: 'calc(50% - 12px)' }"
      >
        <a-input
          v-decorator="[
            'firstName',
            {
              rules: [],
            },
          ]"
          placeholder="John"
        />
      </a-form-item>
      <a-form-item
        label="Last Name"
        :style="{ display: 'inline-block', width: 'calc(50% - 12px)' }"
      >
        <a-input
          v-decorator="[
            'lastName',
            {
              rules: [],
            },
          ]"
          placeholder="Doe"
        />
      </a-form-item>
      <a-form-item
        label="Email"
        class="mr-6"
        :style="{ display: 'inline-block', width: 'calc(50% - 12px)' }"
      >
        <a-input
          v-decorator="[
            'email',
            {
              rules: [
                {
                  required: true,
                  type: 'email',
                  message: 'Please provide a valid email address!',
                },
              ],
            },
          ]"
          placeholder="john.doe@example.com"
        />
      </a-form-item>
      <a-form-item
        label="Phone Number"
        :style="{ display: 'inline-block', width: 'calc(50% - 12px)' }"
      >
        <a-input v-decorator="['phoneNumber']" placeholder="0712345678" />
      </a-form-item>
      <a-button type="primary" html-type="submit">Update Details</a-button>
    </a-form>
  </section>
</template>

<script>
import { Icon } from "@iconify/vue2";

import api from "../../api";
import store from "../../store";

export default {
  data() {
    return {
      accountDetails: null,
      isCollapsed: false,
    };
  },
  beforeCreate() {
    this.form = this.$form.createForm(this, { name: "updateDetails" });
  },
  mounted() {
    api.get(`/public/api/${store.state.id}/accountDetails`).then((res) => {
      this.accountDetails = res.data;
      this.form.setFieldsValue({
        firstName: this.accountDetails.firstName,
        lastName: this.accountDetails.lastName,
        email: this.accountDetails.email,
        phoneNumber: this.accountDetails.phoneNumber,
      });
    });
  },
  methods: {
    toggleCollapse() {
      this.isCollapsed = !this.isCollapsed;
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.form.validateFields(async (err, values) => {
        if (!err) {
          api
            .put(`/public/api/${store.state.id}/accountDetails`, { ...values })
            .then((res) => {
              this.accountDetails = res.data;
              store.commit("updateAccountDetails", res.data);
              this.form.setFieldsValue({
                firstName: this.accountDetails.firstName,
                lastName: this.accountDetails.lastName,
                email: this.accountDetails.email,
                phoneNumber: this.accountDetails.phoneNumber,
              });
            });
        }
      });
    },
  },
  components: {
    Icon,
  },
};
</script>

<style lang="postcss">
.collapsed svg {
  rotate: -90deg;
}
</style>
