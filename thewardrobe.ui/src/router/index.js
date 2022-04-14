import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Register from '../views/Register.vue'
import SignIn from '../views/SignIn.vue'
import EditItem from '../views/EditItem.vue'

import store from "../store";

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    alias: ['/buy', "/sell"],
    name: 'Home',
    component: Home
  },
  {
    path: '/signIn',
    name: 'SignIn',
    component: SignIn
  },
  {
    path: '/register',
    name: 'Register',
    component: Register
  },
  {
    path: '/:itemId/edit',
    name: 'editItem',
    component: EditItem
  }
];

const router = new VueRouter({
  routes
});

router.beforeEach((to, from, next) => {
  // close sidebar if it was previously open
  store.commit("closeSidebar");

  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/signIn', '/register'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('jwt');

  console.log(loggedIn)

  if (authRequired && !loggedIn) {
    return next('/signIn');
  }

  next();
})

export default router;
