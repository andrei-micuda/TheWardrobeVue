import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Register from '../views/Register.vue'
import SignIn from '../views/SignIn.vue'
import ViewItem from '../views/ViewItem.vue'
import EditItem from '../views/EditItem.vue'
import Cart from '../views/Cart.vue';
import Orders from '../views/Orders.vue';
import Account from '../views/Account.vue';
import Checkout from '../views/Checkout.vue';

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
    path: '/item/:itemId',
    name: 'viewItem',
    component: ViewItem
  },
  {
    path: '/item/:itemId/edit',
    name: 'editItem',
    component: EditItem
  },
  {
    path: '/account',
    name: 'account',
    component: Account
  },
  {
    path: '/cart',
    name: 'cart',
    component: Cart
  },
  {
    path: '/checkout/:sellerId',
    name: 'checkout',
    component: Checkout
  },
  {
    path: '/orders',
    alias: ['/orders/incoming', '/orders/outgoing'],
    name: 'orders',
    component: Orders
  }
];

const router = new VueRouter({
  routes
});

router.beforeEach((to, from, next) => {
  // close sidebar if it was previously open
  store.commit('setIsDrawerVisible', false);

  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/signIn', '/register'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('jwt');

  if (authRequired && !loggedIn) {
    return next('/signIn');
  }

  next();
})

export default router;
