<template>
  <div id="profileMenu" class="hidden z-10 origin-top-right absolute right-0 mt-2 w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none" role="menu" aria-orientation="vertical" aria-labelledby="menu-button" tabindex="-1">
    <div v-if="user" class="py-1" role="none">
      <p class="text-gray-700 block px-4 py-2">Hello, {{user}}!</p>
      <!-- <router-link to="/register" class="text-gray-700 block px-4 py-2 text-sm">Sign out</router-link> -->
      <a @click="$store.commit('signOut')" class="text-gray-700 block px-4 py-2 text-sm">Sign out</a>
    </div>
    <div v-else>
      <router-link to="/signIn" class="text-gray-700 block px-4 py-2 text-sm">Sign in</router-link>
      <router-link to="/register" class="text-gray-700 block px-4 py-2 text-sm">Register</router-link>
    </div>
  </div>
</template>

<script>
  import store from '../store';
  export default {
    computed: {
      user() {
        return store.state.user;
      }
    },
  }
</script>

<style scoped>

</style>