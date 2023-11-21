import axios from 'axios'

export const configApi = axios.create({
  baseURL: "https://localhost:7055/api",
});
