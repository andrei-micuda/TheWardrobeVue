import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import Register from "../views/Register.vue";
import SignIn from "../views/SignIn.vue";
import ViewItem from "../views/ViewItem.vue";
import EditItem from "../views/EditItem.vue";
import Cart from "../views/Cart.vue";
import Order from "../views/Order.vue";
import Orders from "../views/Orders.vue";
import Account from "../views/Account.vue";
import VerifyAccount from "../views/VerifyAccount.vue";
import ForgotPassword from "../views/ForgotPassword.vue";
import ChangePassword from "../views/ChangePassword.vue";
import Checkout from "../views/Checkout.vue";

import store from "../store";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    alias: ["/buy", "/sell"],
    name: "Home",
    component: Home,
  },
  {
    path: "/signIn",
    name: "SignIn",
    component: SignIn,
  },
  {
    path: "/register",
    name: "Register",
    component: Register,
  },
  {
    path: "/verify-account",
    name: "verifyAccount",
    component: VerifyAccount,
  },
  {
    path: "/forgot-password",
    name: "forgotPassword",
    component: ForgotPassword,
  },
  {
    path: "/change-password",
    name: "changePassword",
    component: ChangePassword,
  },
  {
    path: "/item/:itemId",
    name: "viewItem",
    component: ViewItem,
  },
  {
    path: "/item/:itemId/edit",
    name: "editItem",
    component: EditItem,
  },
  {
    path: "/account",
    name: "account",
    component: Account,
  },
  {
    path: "/cart",
    name: "cart",
    component: Cart,
  },
  {
    path: "/checkout/:sellerId",
    name: "checkout",
    component: Checkout,
  },
  {
    path: "/orders",
    alias: ["/orders/incoming", "/orders/outgoing"],
    name: "orders",
    component: Orders,
  },
  {
    path: "/order/:orderId",
    name: "order",
    component: Order,
  },
];

const router = new VueRouter({
  routes,
});

router.beforeEach((to, from, next) => {
  // close sidebar if it was previously open
  store.commit("setIsDrawerVisible", false);

  // clear warning message if present
  if (to.path !== "/signIn") store.commit("setWarningMsg", null);

  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = [
    "/",
    "/buy",
    "/signIn",
    "/register",
    "/verify-account",
    "/forgot-password",
    "/change-password",
  ];

  const authRequired =
    !publicPages.includes(to.path) && !to.path.startsWith("/item");
  const loggedIn = localStorage.getItem("jwt");

  if (authRequired && !loggedIn) {
    console.log("Redirect to login");

    store.commit(
      "setWarningMsg",
      "The action you are trying to perform requires you to sign in first."
    );
    return next("/signIn");
  }

  next();
});

export default router;
