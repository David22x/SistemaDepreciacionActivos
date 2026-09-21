import axiosClient from "./axiosClient";

export const consultarDepreciacion = (activoId, fecha) =>
  axiosClient.get(`/depreciation/activo/${activoId}?fecha=${fecha}`);
