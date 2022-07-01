import axios from "axios";
import qs from "qs";
import notifier from "../notifier";
import store from "../store";

let api = axios.create();

const refreshAccessToken = async () => {
  var jwt = null;

  await axios
    .post("/public/api/account/refresh-token")
    .then((res) => {
      var data = res.data;
      jwt = data.jwt;
      store.commit("refreshToken", jwt);
    })
    .catch(() => {
      store.commit("resetStore");
    });

  return jwt;
};

// errorComposer will compose a handleGlobally function
const errorComposer = (error) => {
  return () => {
    if (error.response) {
      notifier.error(error.response.data.message);
    } else {
      notifier.error(error.message);
    }
  };
};

api.defaults.paramsSerializer = function (params) {
  return qs.stringify(params, { indices: false }); // param=value1&param=value2
};

// add stored jwt to every request
api.interceptors.request.use(
  async (config) => {
    config.headers = {
      Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      "Content-Type": "application/json",
    };
    return config;
  },
  (error) => {
    Promise.reject(error);
  }
);

// handle jwt expiration
api.interceptors.response.use(
  (response) => response,
  async function (error) {
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const access_token = await refreshAccessToken();
      axios.defaults.headers.common["Authorization"] = "Bearer " + access_token;
      return api(originalRequest);
    }
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => response,
  (error) => {
    console.log("Intercepting error");
    // console.error(error);
    // whatever you want to do with the error
    error.handleGlobally = errorComposer(error);

    return Promise.reject(error);
  }
);

export default api;
