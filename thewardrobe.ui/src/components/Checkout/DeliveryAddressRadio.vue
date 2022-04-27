<template>
  <section class="bg-gray-800 rounded p-4">
    <p class="text-lg pb-4">Delivery addresses</p>
    <a-radio-group id="DeliveryAddressRadio" class="bg-gray-700 w-full p-4" v-model="deliveryAddressId">
      <a-radio v-for="addr in deliveryAddresses" :key="addr.id" :value="addr.id" class="text-gray-100">
        <div>
          <div>
            <p class="font-bold">{{addr.address}}</p>
            <p>{{addr.city}}, {{addr.country}}, {{addr.postalCode}}</p>
          </div>
        </div>
      </a-radio>
      <a-button type="primary" @click="showModal" class="mt-6">Add address</a-button>
    </a-radio-group>
    <a-modal v-model="modalVisible" title="Add new delivery address" @ok="handleOk">
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
        <a-form-item label="Country" class="mr-6" :style="{ display: 'inline-block', width: 'calc(50% - 12px)' }">
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
        <a-form-item label="Postal Code" :style="{ display: 'inline-block', width: 'calc(50% - 12px)' }">
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
        <a-button id="AddAddressBtn" class="hidden" type="primary" html-type="submit"></a-button>
      </a-form>
    </a-modal>
  </section>
</template>

<script>
  import $ from 'cash-dom';

  import api from '../../api';
  import notifier from '../../notifier';
  import store from '../../store';

  export default {
    data() {
      return {
        deliveryAddresses: [],
        modalVisible: false,
        deliveryAddressId: null
      }
    },
    methods: {
      showModal() {
        this.modalVisible = true;
      },
      handleOk() {
        $('#AddAddressBtn').trigger('click');
      },
      async handleSubmit(e) {
        e.preventDefault();
        this.form.validateFields(async (err, values) => {
          if (!err) {
            api.post(`/api/${store.state.id}/deliveryAddress`, {...values})
              .then(res => {
                notifier.success('Successfully added address.');
                this.deliveryAddresses.push(res.data);
                this.modalVisible = false;
              });
          }
        });
      }
    },
    beforeCreate () {
      this.form = this.$form.createForm(this, { name: 'addAddress' });
    },
    mounted () {
      api.get(`/api/${store.state.id}/deliveryAddress`)
        .then(res => {
          this.deliveryAddresses = res.data;
        })
    },
  }
</script>

<style lang="postcss">
#DeliveryAddressRadio {
  & .ant-radio-wrapper {
    @apply flex !important;
  }

  & .ant-radio-wrapper:not(:last-of-type) {
    @apply border-b-2 border-gray-600 mb-4 pb-4 !important;
  }

  & .ant-radio{
    @apply my-auto mr-4 !important;
  }
}
</style>