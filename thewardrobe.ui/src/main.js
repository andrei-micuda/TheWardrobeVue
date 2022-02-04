import Vue from 'vue'
import App from './App.vue'

import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/antd.css';


import './assets/tailwind.css'

import './validation'
import store from "./store"
import router from './router'

Vue.config.productionTip = false

Vue.use(Antd);

new Vue({
  router,
  store,
  beforeCreate() { this.$store.commit('initStore'); },
  render: h => h(App)
}).$mount('#app')
