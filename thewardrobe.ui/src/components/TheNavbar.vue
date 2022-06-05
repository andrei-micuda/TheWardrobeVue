<template>
  <!-- navbar -->
  <nav>
    <a-row class="bg-gray-800" type="flex" align="middle">
      <a-col
        :span="12"
        :offset="6"
        class="flex items-center justify-center my-2"
      >
        <!-- Logo -->
        <router-link to="/">
          <img class="sm:hidden" src="@/assets/logo-48.svg" alt="logo" />
          <img
            class="hidden sm:block"
            src="@/assets/logo-48-full.svg"
            alt="logo"
          />
        </router-link>
      </a-col>

      <a-col :span="6" class="flex items-center justify-end pr-4">
        <TheNavbarActions />
      </a-col>
    </a-row>
  </nav>
</template>

<script>
import TheNavbarActions from "./TheNavbarActions.vue";

export default {
  components: {
    TheNavbarActions,
  },
};
</script>

<style scoped>
* {
  font-family: "Josefin Sans", sans-serif;
}
</style>
