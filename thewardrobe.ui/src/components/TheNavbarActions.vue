<template>
  <div class="flex items-center space-x-5">
    <!-- <CartListOverview /> -->
    <router-link to="/cart" class="flex items-center text-gray-100 hover:text-gray-200">
      <Icon icon="eva:shopping-cart-outline" class="h-6 w-6" />
    </router-link>

    <!-- Profile Button -->
    <div class="relative inline-block text-left">
      <button class="flex items-center hover:text-gray-200" @click="() => $store.commit('setIsDrawerVisible', true)">
          <Icon :icon="icons.profile" class="h-6 w-6" />
          <!-- <span class="flex absolute -mt-5 mr-6">
            <span class="animate-ping absolute inline-flex h-2 w-2 rounded-full bg-green-400 opacity-75"></span>
            <span class="relative inline-flex rounded-full h-2 w-2 bg-green-500"></span>
          </span> -->
      </button>
      <a-drawer
      placement="right"
      :closable="true"
      :visible="$store.state.isDrawerVisible"
      @close="() => $store.commit('setIsDrawerVisible', false)"
      >
        <ul v-if="id">
          <li>
            <router-link to="/account" class="block py-2 text-sm">My Account</router-link>
          </li>
          <li>
            <router-link to="/orders" class="block py-2 text-sm">Orders</router-link>
          </li>
          <li>
            <a @click="handleSignOut" class="block py-2 text-sm">Sign out</a>
          </li>
        </ul>
        <ul v-else>
          <li>
            <router-link to="/signIn" class="block py-2 text-sm">Sign in</router-link>
          </li>
          <li>
            <router-link to="/register" class="block py-2 text-sm">Register</router-link>
          </li>
        </ul>
      </a-drawer>
    </div>
  </div>
</template>

<script>
  import { Icon } from '@iconify/vue2';

  import store from '../store';
  import router from '../router';

  // import CartListOverview from './Cart/CartListOverview.vue';

  export default {
    data() {
      return {
        icons: {
          heart : "feather:heart",
          cart : "eva:shopping-cart-outline",
          profile : "gg:profile"
        },
      }
    },
    methods: {
      handleSignOut() {
        store.commit('resetStore');
        router.replace('/signIn');
      }
    },
    computed: {
      id() {
        return store.state.id;
      }
    },
    components: {
      Icon,
      // CartListOverview
    },
  }
</script>

<style lang="postcss" scoped>
ul a {
  @apply hover:text-green-400 !important;
}
</style>