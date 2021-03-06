<template>
  <section class="DeliveryAddressesSection">
    <div
      class="flex items-center cursor-pointer collapse-btn"
      :class="{ collapsed: isCollapsed }"
      @click="toggleCollapse"
    >
      <Icon
        icon="dashicons:arrow-down-alt2"
        width="20"
        class="mr-2 transition-all"
      />
      <p class="text-xl">Delivery Addresses</p>
    </div>
    <ul class="bg-gray-700 p-4" :class="{ hidden: isCollapsed }">
      <li v-for="addr in deliveryAddresses" :key="addr.id" class="p-2">
        <div class="flex justify-between items-center">
          <div>
            <p class="font-bold">{{ addr.address }}</p>
            <p>{{ addr.city }}, {{ addr.country }}, {{ addr.postalCode }}</p>
          </div>
          <div
            class="
              flex
              sm:block
              flex-col
              items-center
              justify-center
              space-y-2
              sm:space-y-0 sm:space-x-2
            "
          >
            <a-button @click="() => handleModifyAddress(addr)">modify</a-button>
            <a-button type="danger" @click="() => handleDeleteAddress(addr.id)"
              >delete</a-button
            >
          </div>
        </div>
        <a-divider class="my-2" />
      </li>
      <a-button type="primary" @click="showModal">Add address</a-button>
    </ul>
    <a-modal v-model="modalVisible" :title="modalTitle" @ok="handleOk">
      <a-form :form="form" @submit="handleSubmit" hideRequiredMark>
        <a-form-item label="Address">
          <a-input
            v-decorator="[
              'address',
              {
                rules: [
                  {
                    required: true,
                    message: 'Please provide an address!',
                  },
                ],
              },
            ]"
            placeholder="Main Street, no. 1"
          />
        </a-form-item>
        <a-form-item label="City">
          <a-input
            v-decorator="[
              'city',
              {
                rules: [
                  {
                    required: true,
                    message: 'Please provide a city!',
                  },
                ],
              },
            ]"
            placeholder="London"
          />
        </a-form-item>
        <a-form-item
          label="Country"
          class="mr-6"
          :style="{ display: 'inline-block', width: 'calc(50% - 12px)' }"
        >
          <a-input
            v-decorator="[
              'country',
              {
                rules: [
                  {
                    required: true,
                    message: 'Please provide a country!',
                  },
                ],
              },
            ]"
            placeholder="Great Britain"
          />
        </a-form-item>
        <a-form-item
          label="Postal Code"
          :style="{ display: 'inline-block', width: 'calc(50% - 12px)' }"
        >
          <a-input
            v-decorator="[
              'postalCode',
              {
                rules: [
                  {
                    required: true,
                    message: 'Please provide a postal code!',
                  },
                ],
              },
            ]"
            placeholder="123456"
          />
        </a-form-item>
        <a-button
          id="AddAddressBtn"
          class="hidden"
          type="primary"
          html-type="submit"
        ></a-button>
      </a-form>
    </a-modal>
  </section>
</template>

<script>
import $ from "cash-dom";
import { Icon } from "@iconify/vue2";

import api from "../../api";
import notifier from "../../notifier";
import store from "../../store";

export default {
  data() {
    return {
      deliveryAddresses: [],
      modalVisible: false,
      updatingAddressId: null,
      isCollapsed: false,
      modalTitle: null,
    };
  },
  methods: {
    showModal() {
      this.form.getFieldDecorator("address", { initialValue: null });
      this.form.getFieldDecorator("city", { initialValue: null });
      this.form.getFieldDecorator("country", { initialValue: null });
      this.form.getFieldDecorator("postalCode", { initialValue: null });
      this.modalTitle = "Add a new delivery address";
      this.modalVisible = true;
      this.updatingAddressId = null;
    },
    toggleCollapse() {
      this.isCollapsed = !this.isCollapsed;
    },
    handleOk() {
      $("#AddAddressBtn").trigger("click");
    },
    handleModifyAddress({ id, address, city, country, postalCode }) {
      this.form.getFieldDecorator("address", { initialValue: address });
      this.form.getFieldDecorator("city", { initialValue: city });
      this.form.getFieldDecorator("country", { initialValue: country });
      this.form.getFieldDecorator("postalCode", { initialValue: postalCode });
      this.modalTitle = "Modify delivery address";
      this.modalVisible = true;
      this.updatingAddressId = id;
    },
    handleDeleteAddress(addressId) {
      api
        .delete(`/public/api/${store.state.id}/deliveryAddress/${addressId}`)
        .then(() => {
          notifier.success("Successfully deleted address.");
          this.deliveryAddresses = this.deliveryAddresses.filter(
            (addr) => addr.id != addressId
          );
        });
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if (this.updatingAddressId != null) {
            api
              .put(
                `/public/api/${store.state.id}/deliveryAddress/${this.updatingAddressId}`,
                { ...values }
              )
              .then((res) => {
                var addrIndex = this.deliveryAddresses.findIndex(
                  (addr) => addr.id === this.updatingAddressId
                );
                this.deliveryAddresses[addrIndex] = res.data;

                notifier.success("Successfully updated address.");
                this.modalVisible = false;
              });
          } else {
            api
              .post(`/public/api/${store.state.id}/deliveryAddress`, {
                ...values,
              })
              .then((res) => {
                notifier.success("Successfully added address.");
                this.deliveryAddresses.push(res.data);
                this.modalVisible = false;
              });
          }
        }
      });
    },
  },
  beforeCreate() {
    this.form = this.$form.createForm(this, { name: "addAddress" });
  },
  mounted() {
    api.get(`/public/api/${store.state.id}/deliveryAddress`).then((res) => {
      this.deliveryAddresses = res.data;
    });
  },
  components: {
    Icon,
  },
};
</script>

<style lang="postcss">
.collapse-btn:not(.collapsed) {
  @apply mb-2;
}

.collapsed svg {
  rotate: -90deg;
}
</style>
