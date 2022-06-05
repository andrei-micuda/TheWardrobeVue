<template>
  <div
    class="flex justify-center items-center w-screen"
    :style="{ height: 'calc(100vh - 66px)' }"
  >
    <a-alert
      v-if="warningMsg"
      :message="warningMsg"
      class="absolute top-24"
      banner
    />
    <SignInForm />
  </div>
</template>

<script>
import SignInForm from "../components/SignInForm.vue";
import store from "../store";

export default {
  computed: {
    warningMsg() {
      return store.state.warningMsg;
    },
  },
  components: {
    SignInForm,
  },
};
</script>

<style scoped></style>
