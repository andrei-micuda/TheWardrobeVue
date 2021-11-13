<template>
  <div class="hidden xl:flex items-center space-x-5">
    <a class="hover:text-gray-200" href="#">
      <Icon :icon="icons.heart" class="h-6 w-6" />
    </a>
    <a class="flex items-center hover:text-gray-200" href="#">
        <Icon :icon="icons.cart" class="h-6 w-6" />
      <span class="flex absolute -mt-5 ml-4">
        <span class="animate-ping absolute inline-flex h-3 w-3 rounded-full bg-pink-400 opacity-75"></span>
          <span class="relative inline-flex rounded-full h-3 w-3 bg-pink-500">
          </span>
        </span>
    </a>

    <!-- Profile Button -->
    <div class="relative inline-block text-left">
      <button class="flex items-center hover:text-gray-200" @click="toggleProfileMenu">
          <Icon :icon="icons.profile" class="h-6 w-6" />
      </button>

      <div id="profileMenu" class="hidden z-10 origin-top-right absolute right-0 mt-2 w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none" role="menu" aria-orientation="vertical" aria-labelledby="menu-button" tabindex="-1">
      <div class="py-1" role="none">
          <!-- Active: "bg-gray-100 text-gray-900", Not Active: "text-gray-700" -->
          <router-link to="/signin" class="text-gray-700 block px-4 py-2 text-sm">Sign in</router-link>
          <router-link to="/register" class="text-gray-700 block px-4 py-2 text-sm">Register</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import $ from "cash-dom";
  import { Icon } from '@iconify/vue2';
  export default {
    data() {
      return {
        icons: {
          heart : "feather:heart",
          cart : "eva:shopping-cart-outline",
          profile : "gg:profile"
        }
      }
    },
    methods: {
      toggleProfileMenu() {
        const profileMenu = $("#profileMenu");
        if(profileMenu.hasClass("hidden")) {
          profileMenu.removeClass("hidden");
        }
        else { 
          profileMenu.addClass("hidden");
        }
      }
    },
    components: {
      Icon,
    },
  }
</script>

<style scoped>

</style>