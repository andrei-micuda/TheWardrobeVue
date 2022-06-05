<template>
  <div></div>
</template>

<script>
import api from "../api";
import notifier from "../notifier";
import router from "../router";

export default {
  mounted() {
    const verificationToken = this.$route.query.token;

    api
      .post("/public/api/account/verify", {
        verificationToken,
      })
      .then(() => {
        notifier.success("Successfully verified account.");
        router.replace("/signIn");
      })
      .error((err) => {
        notifier.error(err);
      });
  },
};
</script>

<style lang="scss" scoped></style>
