import axios from 'axios';
import notifier from "../notifier";

let api = axios.create();

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

api.interceptors.response.use((response) => response, (error) => {
  console.log("Intercepting error");
  console.error(error);
  // whatever you want to do with the error
  error.handleGlobally = errorComposer(error);

  return Promise.reject(error);
});

export default api;