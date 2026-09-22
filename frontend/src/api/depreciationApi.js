import axiosClient from "./axiosClient";

export const consultarDepreciacion = (activoId, fecha) =>
  axiosClient.post("/depreciation/calcular", {
    activoId,
    fechaConsulta: fecha,
  });
