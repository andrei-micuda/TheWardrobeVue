import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex);

const store = new Vuex.Store({
  
  // "State" is the application data your components
  // will subscribe to
  
  state: {     
    isSignedIn: false,
    user: null
  },
  mutations: {
    signInUser(state, user) {
      state.user = user;
      state.isSignedIn = true;
    }
  }
});

export default store;