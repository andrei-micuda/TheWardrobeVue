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

      <TheNavbarProfileMenu />
    </div>
  </div>
</template>

<script>
  import $ from "cash-dom";
  import { Icon } from '@iconify/vue2';
  import TheNavbarProfileMenu from "./TheNavbarProfileMenu.vue";

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
      TheNavbarProfileMenu
    },
  }
</script>

<style scoped>

</style>