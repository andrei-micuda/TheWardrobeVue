<template>
  <div class="text-white">
    <h1 class="text-2xl font-bold uppercase text-left mb-12">Sign in</h1>
    <div class="space-y-8">
      <VInput class="text-md" label="Email" placeholder="test@example.com" />
      <VInput class="text-md" label="Password" placeholder="Your Password" type="password" />
      <VButton text="Sign in" />
    </div>
  </div>
</template>

<script>
  import VInput from "./VInput.vue";
  import VButton from "./VButton.vue";
  export default {
    components: {
      VInput,
      VButton
    },
  }
</script>

<style scoped>
  h1 {
    font-family: 'Josefin Sans', sans-serif;
}
</style>