<template>
  <div>
    <VPageHeader title="My Account" @back="$router.back()" />
    <a-row class="lg:w-3/5 md:w-5/6 w-11/12 mx-auto">
      <a-col :span="24" :md="{ span: 6 }">
        <div class="rounded border-gray-500 md:w-4/5 mb-6 md:mb-0 flex md:block items-center justify-between sm:justify-center mx-auto">
          <img :src="profilePicture" alt="Account Profile Picture" class="rounded-full mr-6 md:mr-0 md:mx-auto">

          <div class="bg-gray-800 md:mt-4 rounded p-4 space-y-2 w-64 sm:w-auto">
            <p class="text-center font-bold">Ratings</p>
            <div class="flex items-center space-x-2 justify-center">
              <p>Buyer:</p>
              <div class="flex items-center space-x-1 bg-gray-700 px-3 py-1 rounded">
                <p class="text-sm text-center">{{buyerRating ? buyerRating.toFixed(2) : 'N/A'}}</p>
                <Icon icon="ant-design:star-filled" width="16" />
              </div>
            </div>
            <div class="flex items-center space-x-2 justify-center">
              <p>Seller:</p>
              <div class="flex items-center space-x-1 bg-gray-700 px-3 py-1 rounded">
                <p class="text-sm text-center">{{sellerRating ? sellerRating.toFixed(2) : 'N/A'}}</p>
                <Icon icon="ant-design:star-filled" width="16" />
              </div>
            </div>
          </div>
        </div>
      </a-col>
      <a-col :span="24" :md="{ span: 18 }" class="p-4 bg-gray-800 rounded">
        <AccountDetails />
        <DeliveryAddressList />
      </a-col>
    </a-row>
  </div>
</template>

<script>
  import { Icon } from "@iconify/vue2";
  import VPageHeader from '../components/VPageHeader.vue';
  import AccountDetails from '../components/Account/AccountDetails.vue';
  import DeliveryAddressList from '../components/Account/DeliveryAddressList.vue';

  import { mapState } from 'vuex';

  import api from '../api';
  import store from '../store';

  export default {
    data() {
      return {
        buyerRating: null,
        sellerRating: null,
      }
    },
    mounted () {
      api.get(`/public/api/${store.state.id}/order/ratings`)
      .then(res => {
        this.buyerRating = res.data.buyerRating;
        this.sellerRating = res.data.sellerRating;
      });
    },
    components: {
      Icon,
      VPageHeader,
      AccountDetails,
      DeliveryAddressList
    },
    computed: {
      ...mapState(['email', 'firstName', 'lastName']),
      profilePicture() {
        return `https://eu.ui-avatars.com/api/?name=${this.name}&size=128&format=svg`;
      },
      name() {
        if(this.firstName && this.lastName)
          return `${this.firstName}+${this.lastName}`;
        else if(this.firstName)
          return this.firstName;
        else if(this.lastName)
          return this.lastName;
        else
          return this.email;
      }
    },
  }
</script>

<style lang="postcss">
</style>