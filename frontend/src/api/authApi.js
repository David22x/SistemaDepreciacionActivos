import axiosClient from "./axiosClient";

export const login = (credenciales) =>
  axiosClient.post("/auth/login", credenciales);

export const register = (datos) => axiosClient.post("/auth/register", datos);
