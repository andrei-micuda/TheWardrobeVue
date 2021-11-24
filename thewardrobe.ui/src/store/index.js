import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex);

const store = new Vuex.Store({
  
  // "State" is the application data your components
  // will subscribe to
  
  state: {     
    user: null,
    jwt: null
  },
  mutations: {
    initStore(state) {
      let jwt = localStorage.getItem('jwt');
      let user = localStorage.getItem('user');
      if (jwt && user)
      {
        state.jwt = jwt;
        state.user = user;
      }
    },
    signInUser(state, { email, jwt }) {
      state.user = email;
      state.jwt = jwt;

      localStorage.setItem('jwt', jwt);
      localStorage.setItem('user', email);
    }
  }
});

export default store;