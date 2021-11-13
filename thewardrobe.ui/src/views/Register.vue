<template>
  <LoginRegisterCard>
    <RegisterForm />
  </LoginRegisterCard>
</template>

<script>
    import LoginRegisterCard from "../components/SignInRegisterCard.vue"
    import RegisterForm from "../components/RegisterForm.vue"
  export default {
    components: {
        LoginRegisterCard,
        RegisterForm
    }
  }
</script>

<style scoped>
  .background
  {
    background-image: url("../assets/register-2.jpg")
  }
</style>