import axiosClient from "./axiosClient";

export const generarPdf = (datos) =>
  axiosClient.post("/reports/depreciacion/pdf", datos, {
    responseType: "blob",
  });
