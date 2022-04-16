import axios from 'axios';
import notifier from "../notifier";
import store from "../store";

let api = axios.create();

const refreshAccessToken = async () => {
  var res = await api.post("/api/accounts/refresh-token");
  var data = res.data;
  console.log(data);
  store.commit("refreshToken", data.jwt);
  return data.jwt;
}

// errorComposer will compose a handleGlobally function
const errorComposer = (error) => {
    return () => {
      if (error.response) {
        notifier.error(error.response.data.message); 
      }
      else {
        notifier.error(error.message);
      }
    }
}

// add stored jwt to every request
api.interceptors.request.use(
  async config => {
    config.headers = { 
      'Authorization': `Bearer ${localStorage.getItem('jwt')}`,
      'Content-Type': 'application/json'
    }
    return config;
  },
  error => {
    Promise.reject(error)
  });

// handle jwt expiration
api.interceptors.response.use(response => response, async function (error) {
  const originalRequest = error.config;
  if (error.response.status === 401 && !originalRequest._retry) {
    originalRequest._retry = true;
    const access_token = await refreshAccessToken();            
    axios.defaults.headers.common['Authorization'] = 'Bearer ' + access_token;
    return api(originalRequest);
  }
  return Promise.reject(error);
});

api.interceptors.response.use((response) => response, (error) => {
  console.log("Intercepting error");
  console.error(error);
  // whatever you want to do with the error
  error.handleGlobally = errorComposer(error);

  return Promise.reject(error);
});

export default api;