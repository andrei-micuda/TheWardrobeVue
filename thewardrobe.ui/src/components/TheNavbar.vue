<template>
  <div class="flex flex-wrap place-items-center h-16 z-30">
    <section class="relative mx-auto h-full">
        <!-- navbar -->
      <nav class="flex bg-gray-800 text-white w-screen
        px-4 xl:px-12 py-3 h-full">

          <div class="w-1/3 flex items-center">
            <!-- Burger Menu -->
            <button @click="toggleSidebar">
              <Icon icon="gg:menu" class="h-6 w-6 xl:hidden" />
            </button>

          </div>

          <div class="w-1/3 flex items-center justify-center">
            <!-- Logo -->
            <a href="/">
              <img class="sm:hidden" src="@/assets/logo-48.svg" alt="logo">
              <img class="hidden sm:block" src="@/assets/logo-48-full.svg" alt="logo">
            </a>
            <!-- <p class="hidden sm:block text-2xl">TheWardrobe</p> -->
          </div>

          <div class="w-1/3 flex items-center justify-end">
            <div class="flex justify-end space-x-3">
              <!-- Search -->
              <Icon :icon="icons.search" class="h-6 w-6" />

              <!-- Shopping Cart -->
              <a class="xl:hidden flex items-center" href="#">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 hover:text-gray-200" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
                <span class="flex absolute -mt-5 ml-4">
                  <span class="animate-ping absolute inline-flex h-3 w-3 rounded-full bg-pink-400 opacity-75"></span>
                  <span class="relative inline-flex rounded-full h-3 w-3 bg-pink-500">
                  </span>
                </span>
              </a>

              <!-- Favorites -->

              <Icon :icon="icons.heart" class="h-6 w-6" />

            </div>
            <TheNavbarUserActions />
          </div>
        <!-- Responsive navbar -->
      </nav>

      <!-- sidebar -->
        <div v-if="sidebarShow" class="sidebar flex absolute h-screen w-screen top-0 z-50 bg-black bg-opacity-50">
          <div class="sidebar-menu bg-gray-800 h-full w-80 py-4">
            <ul v-if="user">
              <li>{{user}}</li>
              <li>Sign out</li>
            </ul>
            <ul v-else>
              <router-link to="signIn">Sign in</router-link>
            </ul>
          </div>
          <button @click="toggleSidebar" class="sidebar-close self-start m-2">
            <Icon :icon="icons.close" class="h-6 w-6" />
          </button>
        </div>
      
    </section>
  </div>
</template>

<script>
  import { Icon } from '@iconify/vue2';

  import store from '../store';
  import TheNavbarUserActions from './TheNavbarUserActions.vue';
  
  export default {
    data() {
      return {
        icons: {
          search: "fe:search",
          heart : "feather:heart",
          cart : "eva:shopping-cart-outline",
          close: "gg:close"
        }
      }
    },
    computed: {
      user() {
        return store.state.user;
      },
      sidebarShow() {
        return store.state.sidebarShow;
      }
    },
    components: {
      TheNavbarUserActions,
      Icon
    },
    methods: {
      toggleSidebar() {
        this.$store.commit("toggleSidebar");
      }
    },
  }
</script>

<style scoped>
* {
  font-family: 'Josefin Sans', sans-serif;
}
</style>