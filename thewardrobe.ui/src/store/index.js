import Vue from "vue";
import Vuex from "vuex";

import api from "../api";

Vue.use(Vuex);

function setAccountDetails(state, accountId) {
  // retrieve user name and email from db
  api.get(`/public/api/${accountId}/accountDetails`).then((res) => {
    state.email = res.data.email;
    state.firstName = res.data.firstName;
    state.lastName = res.data.lastName;
  });
}

function setUserItems(state, accountId) {
  // retrieve user items from db
  api.get(`/public/api/itemCatalog/${accountId}/ids`).then((res) => {
    state.userItems = res.data;
  });
}

const getDefaultState = () => {
  return {
    items: [],
    id: null,
    email: null,
    firstName: null,
    lastName: null,
    userItems: null,
    jwt: null,
    isDrawerVisible: false,
    warningMsg: null,
    itemFilters: null,
  };
};

const store = new Vuex.Store({
  // "State" is the application data your components
  // will subscribe to

  state: getDefaultState(),
  mutations: {
    resetStore(state) {
      localStorage.removeItem("id");
      localStorage.removeItem("jwt");
      Object.assign(state, getDefaultState());
    },
    getUserData(state) {
      let id = localStorage.getItem("id");
      let jwt = localStorage.getItem("jwt");

      state.id = id;
      state.jwt = jwt;

      if (id) {
        setAccountDetails(state, id);

        setUserItems(state, id);
      }
    },
    addUserItem(state, itemId) {
      state.userItems.push(itemId);
    },
    updateAccountDetails(state, { email, firstName, lastName }) {
      state.email = email;
      state.firstName = firstName;
      state.lastName = lastName;
    },
    signInUser(state, { id, jwt }) {
      state.id = id;
      state.jwt = jwt;

      setUserItems(state, id);
      setAccountDetails(state, id);

      localStorage.setItem("id", id);
      localStorage.setItem("jwt", jwt);
    },
    // signOut(state) {
    //   console.log("Signing out...");
    //   state.id = null;
    //   state.jwt = null;

    //   localStorage.removeItem('id');
    //   localStorage.removeItem('jwt');

    //   router.replace('/signIn');
    // },
    refreshToken(state, jwt) {
      console.log("Refreshed JWT: ", jwt);
      state.jwt = jwt;

      localStorage.setItem("jwt", jwt);
    },
    setIsDrawerVisible(state, val) {
      state.isDrawerVisible = val;
    },
    setWarningMsg(state, val) {
      state.warningMsg = val;
    },
    setItemFilters(state, obj) {
      state.itemFilters = obj;
    },
  },
});

export default store;
