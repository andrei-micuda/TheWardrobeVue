import Vue from 'vue'
import App from './App.vue'

import './assets/tailwind.css'

import './validation'
import store from "./store"
import router from './router'

Vue.config.productionTip = false

new Vue({
  router,
  store,
  beforeCreate() { this.$store.commit('initStore'); },
  render: h => h(App)
}).$mount('#app')
