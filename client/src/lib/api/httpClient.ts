import axios from 'axios';

const sleep = (delay: number) =>
  new Promise((resolve) => {
    setTimeout(resolve, delay);
  });

const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

httpClient.interceptors.response.use(async (response) => {
  try {
    await sleep(1000);

    return response;
  } catch (error) {
    console.log(error);
    return Promise.reject(error);
  }
});

export default httpClient;
