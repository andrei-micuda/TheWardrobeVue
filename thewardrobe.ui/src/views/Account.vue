<template>
  <div>
    <VPageHeader title="My Account" @back="$router.back()" />
    <a-row class="w-3/5 mx-auto">
      <a-col :span="6">
        <div class="rounded border-gray-500 w-4/5 mx-auto">
          <img :src="profilePicture" alt="Account Profile Picture" class="rounded-full mx-auto">
        </div>
      </a-col>
      <a-col :span="18" class="p-4 bg-gray-800 rounded">
        <AccountDetails />
        <DeliveryAddressList />
      </a-col>
    </a-row>
  </div>
</template>

<script>
  import VPageHeader from '../components/VPageHeader.vue';
  import AccountDetails from '../components/Account/AccountDetails.vue';
  import DeliveryAddressList from '../components/Account/DeliveryAddressList.vue';

  import { mapState } from 'vuex';

  export default {
    components: {
      VPageHeader,
      AccountDetails,
      DeliveryAddressList
    },
    computed: {
      ...mapState(['email', 'firstName', 'lastName']),
      profilePicture() {
        return `https://ui-avatars.com/api/?name=${this.name}&size=128&format=svg`;
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