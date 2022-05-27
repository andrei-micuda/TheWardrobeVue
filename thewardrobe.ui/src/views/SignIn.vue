<template>
<div class="flex justify-center items-center w-screen" :style="{ height: 'calc(100vh - 66px)' }">
    <SignInForm />
</div>
</template>

<script>
    import SignInForm from "../components/SignInForm.vue"
  export default {
    components: {
        SignInForm
    }
  }
</script>

<style scoped>
</style>