export function formatMoney(value) {
  const numero = Number(value ?? 0);
  return numero.toLocaleString('es-EC', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2,
  });
}

export function formatDate(value) {
  if (!value) return 'No disponible';
  const fecha = new Date(value);
  if (Number.isNaN(fecha.getTime())) return 'No disponible';
  return fecha.toLocaleDateString('es-EC', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    timeZone: 'UTC',
  });
}
