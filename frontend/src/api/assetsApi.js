import axiosClient from "./axiosClient";

export const getActivos = () => axiosClient.get("/assets");
export const getCategorias = () => axiosClient.get("/assets/categorias");
export const crearActivo = (activo) => axiosClient.post("/assets", activo);
