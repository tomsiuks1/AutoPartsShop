import axios from "axios";
import { toast } from "react-toastify";
import { router } from "../router/Routes";
import { store } from "../store/configureStore";

const sleep = () => new Promise((resolve) => setTimeout(resolve, 500));

axios.defaults.baseURL = "http://localhost:5241/api";
axios.defaults.withCredentials = true;

const responseBody = (response) => response.data;

axios.interceptors.request.use((config) => {
  const token = store.getState().account.user?.token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

axios.interceptors.response.use(
  async (response) => {
    if (import.meta.env.DEV) await sleep();
    const pagination = response.headers["pagination"];
    if (pagination) {
      response.data = {
        items: response.data,
        metaData: JSON.parse(pagination),
      };
      return response;
    }
    return response;
  },
  (error) => {
    const { data, status } = error.response;
    switch (status) {
      case 400:
        if (data.errors) {
          const modelStateErrors = [];
          for (const key in data.errors) {
            if (data.errors[key]) {
              modelStateErrors.push(data.errors[key]);
            }
          }
          throw modelStateErrors.flat();
        }
        toast.error(data.title);
        break;
      case 401:
        toast.error(data.title);
        break;
      case 403:
        toast.error("You are not allowed to do that!");
        break;
      case 500:
        router.navigate("/server-error", { state: { error: data } });
        break;
      default:
        break;
    }
    return Promise.reject(error.response);
  }
);

const request = {
  get: (url, params) => axios.get(url, { params }).then(responseBody),
  post: (url, data) => axios.post(url, data).then(responseBody),
  put: (url, data) => axios.put(url, data).then(responseBody),
  delete: (url) => axios.delete(url).then(responseBody),
  postForm: (url, data) =>
    axios
      .post(url, data, {
        headers: { "Content-type": "multipart/form-data" },
      })
      .then(responseBody),
  putForm: (url, data) =>
    axios
      .put(url, data, {
        headers: { "Content-type": "multipart/form-data" },
      })
      .then(responseBody),
};

const Catalog = {
  getCarMakers: () => request.get("/catalog/car/makers"),
  getCarModels: () => request.get("/catalog/car/models"),
  getCatalog: (params) => request.get("/catalog", params),
  details: (id) => request.get(`/catalog/${id}`),
  fetchFilters: () => request.get("/catalog/filters"),
};

const Basket = {
  get: () => request.get("basket"),
  addItem: (catalogItemId, quantity = 1) =>
    request.post(
      `basket?catalogItemId=${catalogItemId}&quantity=${quantity}`,
      {}
    ),
  removeItem: (catalogItemId, quantity = 1) =>
    request.delete(
      `basket?catalogItemId=${catalogItemId}&quantity=${quantity}`
    ),
};

const Orders = {
  list: () => request.get("orders"),
  fetch: (id) => request.get(`orders/${id}`),
  create: (values) => request.post("orders", values),
};

const Account = {
  login: (values) => request.post("/account/login", values),
  register: (values) => request.post("/account/register", values),
  currentUser: () => request.get("/account/currentUser"),
  fetchAddress: () => request.get("account/savedAddress"),
};

function createFormData(item) {
  const formData = new FormData();
  // eslint-disable-next-line no-debugger
  debugger;
  for (const key in item) {
    formData.append(key, item[key]);
  }
  return formData;
}

const Admin = {
  createProduct: (product) =>
    request.postForm("catalog", createFormData(product)),
  updateProduct: (product) =>
    request.putForm("catalog", createFormData(product)),
  deleteProduct: (id) => request.delete(`catalog/${id}`),
};

const Payments = {
  createPaymentIntent: () => request.post("payments", {}),
};

const agent = {
  Catalog,
  Account,
  Basket,
  Payments,
  Orders,
  Admin,
};

export default agent;
