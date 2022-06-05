<template>
  <div
    class="flex justify-center items-center w-screen"
    :style="{ height: 'calc(100vh - 66px)' }"
  >
    <RegisterForm />
  </div>
</template>

<script>
import RegisterForm from "../components/RegisterForm.vue";
export default {
  components: {
    RegisterForm,
  },
};
</script>

<style scoped></style>
