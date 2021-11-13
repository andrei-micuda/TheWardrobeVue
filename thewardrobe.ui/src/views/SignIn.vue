<template>
  <SignInRegisterCard>
    <SignInForm />
  </SignInRegisterCard>
</template>

<script>
    import SignInRegisterCard from "../components/SignInRegisterCard.vue"
    import SignInForm from "../components/SignInForm.vue"
  export default {
    components: {
        SignInRegisterCard,
        SignInForm
    }
  }
</script>

<style scoped>
  .background
  {
    background-image: url("../assets/register-2.jpg")
  }
</style>