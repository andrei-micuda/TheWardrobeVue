<template>
  <div id="app" class="min-h-screen flex flex-col bg-gray-700">
    <TheNavbar />
    <router-view/>
  </div>
</template>

<script>
import TheNavbar from "./components/TheNavbar.vue"
export default {
  components: {
        TheNavbar
    }
}
</script>

<style lang="postcss">
@import url('https://fonts.googleapis.com/css2?family=Josefin+Sans:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;1,100;1,200;1,300;1,400;1,500;1,600;1,700&family=Libre+Franklin:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&display=swap');
* {
  box-sizing: border-box;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  /* text-align: center; */
  color: #fff;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}

.ant-btn {
  @apply bg-gray-600 border-gray-600 text-gray-100 hover:border-green-400 !important;
}

.ant-btn-primary {
  @apply bg-gray-500 border-green-400 text-green-400 hover:bg-green-400 hover:text-gray-100 !important;
}

.ant-message {
  @apply mt-14 !important;

  & .anticon {
    top: -2px !important;
  }
}

.ant-spin-dot-item {
  @apply bg-green-400 !important;
}

.ant-radio-group-solid .ant-radio-button-wrapper-checked:not(.ant-radio-button-wrapper-disabled) {
  @apply bg-green-400 border-green-400 !important;

  &::before {
    @apply bg-green-400 !important;
  }
}

.ant-radio-button-wrapper:hover {
  @apply text-green-400 !important;
}

.ant-form,
.ant-form-item-label > label {
  @apply text-gray-100 !important;
}

.ant-upload-list-item-card-actions .anticon,
.ant-upload-list-item-info {
  @apply text-gray-100 !important;
}

a {
  @apply hover:text-green-500 !important;
}
</style>
