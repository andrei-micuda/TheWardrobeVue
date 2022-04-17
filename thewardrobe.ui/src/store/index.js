import Vue from 'vue'
import Vuex from 'vuex'

import api from '../api';

Vue.use(Vuex);

const store = new Vuex.Store({
  
  // "State" is the application data your components
  // will subscribe to
  
  state: {
    id: null,
    user: null,
    userItems: null,
    jwt: null,
    sidebarShow: false
  },
  mutations: {
    initStore(state) {
      let id = localStorage.getItem('id');
      let jwt = localStorage.getItem('jwt');
      let user = localStorage.getItem('user');

      state.id = id;

      // retrieve user items from db
      api.get('/api/itemCatalog', {
        params: { sellerIdInclude: id }
      })
        .then(res => {
          var itemIds = res.data.items.map(i => i.id);
          state.userItems = itemIds;
        });

      state.jwt = jwt;
      state.user = user;
    },
    signInUser(state, { id, email, jwt }) {
      state.id = id;
      state.user = email;
      state.jwt = jwt;

      localStorage.setItem('id', id);
      localStorage.setItem('jwt', jwt);
      localStorage.setItem('user', email);
    },
    signOut(state) {
      console.log("Signing out...");
      state.id = null;
      state.user = null;
      state.jwt = null;

      localStorage.removeItem('id');
      localStorage.removeItem('jwt');
      localStorage.removeItem('user');
    },
    refreshToken(state, jwt) {
      console.log("Refreshed JWT: ", jwt)
      state.jwt = jwt;

      localStorage.setItem("jwt", jwt);
    },
    toggleSidebar(state) {
      state.sidebarShow = !state.sidebarShow;
    },
    closeSidebar(state) {
      state.sidebarShow = false;
    }
  }
});

export default store;