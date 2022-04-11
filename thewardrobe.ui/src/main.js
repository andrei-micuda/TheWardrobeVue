import Vue from 'vue'
import App from './App.vue'

import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/antd.css';


import './assets/tailwind.css'

import './validation'
import store from "./store"
import router from './router'

Vue.config.productionTip = false
Vue.config.errorHandler = function (err, vm, info) {
   // handle error
   // `info` is a Vue-specific error info, e.g. which lifecycle hook
   // the error was found in. Only available in 2.2.0+
  console.log(err)
  console.log(vm)
  console.log(info)
 }

Vue.use(Antd);

new Vue({
  router,
  store,
  beforeCreate() { this.$store.commit('initStore'); },
  render: h => h(App)
}).$mount('#app')
