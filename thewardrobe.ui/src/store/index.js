import Vue from 'vue'
import Vuex from 'vuex'

import api from '../api';
import router from '../router';

Vue.use(Vuex);

function setAccountDetails(state, accountId) {
  // retrieve user name and email from db
  api.get(`/public/api/${accountId}/accountDetails`)
    .then(res => {
      state.email = res.data.email;
      state.firstName = res.data.firstName;
      state.lastName = res.data.lastName;
    })
}

const store = new Vuex.Store({

  // "State" is the application data your components
  // will subscribe to

  state: {
    id: null,
    email: null,
    firstName: null,
    lastName: null,
    userItems: null,
    jwt: null,
    isDrawerVisible: false
  },
  mutations: {
    initStore(state) {
      let id = localStorage.getItem('id');
      let jwt = localStorage.getItem('jwt');

      state.id = id;
      state.jwt = jwt;

      if (id) {
        setAccountDetails(state, id);

        // retrieve user items from db
        api.get('/public/api/itemCatalog', {
          params: { sellerIdInclude: id }
        })
          .then(res => {
            var itemIds = res.data.items.map(i => i.id);
            state.userItems = itemIds;
          });
      }
    },
    updateAccountDetails(state, { email, firstName, lastName }) {
      console.log("updateAccountDetails")
      state.email = email;
      state.firstName = firstName;
      state.lastName = lastName;
    },
    signInUser(state, { id, jwt }) {
      state.id = id;
      state.jwt = jwt;

      setAccountDetails(state, id);

      localStorage.setItem('id', id);
      localStorage.setItem('jwt', jwt);
    },
    signOut(state) {
      console.log("Signing out...");
      state.id = null;
      state.jwt = null;

      localStorage.removeItem('id');
      localStorage.removeItem('jwt');

      router.replace('/signIn');
    },
    refreshToken(state, jwt) {
      console.log("Refreshed JWT: ", jwt)
      state.jwt = jwt;

      localStorage.setItem("jwt", jwt);
    },
    setIsDrawerVisible(state, val) {
      state.isDrawerVisible = val;
    }
  }
});

export default store;