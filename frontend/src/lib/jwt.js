// Minimal JWT payload decoder (no signature verification — that's the
// backend's job). The API signs tokens with claims using the long .NET
// ClaimTypes URIs (e.g. ".../claims/name", ".../claims/role"), so we look
// those up by suffix instead of assuming short claim names.

export function decodeJwt(token) {
  try {
    const payload = token.split(".")[1];
    const base64 = payload.replace(/-/g, "+").replace(/_/g, "/");
    const json = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + c.charCodeAt(0).toString(16).padStart(2, "0"))
        .join("")
    );
    return JSON.parse(json);
  } catch {
    return null;
  }
}

function findClaim(payload, suffix) {
  if (!payload) return undefined;
  const key = Object.keys(payload).find((k) => k.toLowerCase().endsWith(suffix));
  return key ? payload[key] : undefined;
}

export function getUserFromToken(token) {
  const payload = decodeJwt(token);
  if (!payload) return null;

  const username = findClaim(payload, "/name") ?? payload.unique_name ?? payload.name;
  const userId = findClaim(payload, "/nameidentifier") ?? payload.nameid ?? payload.sub;
  const rawRoles = findClaim(payload, "/role") ?? payload.role;
  const roles = Array.isArray(rawRoles) ? rawRoles : rawRoles ? [rawRoles] : [];

  return { username, userId, roles, exp: payload.exp };
}

export function isTokenExpired(token) {
  const payload = decodeJwt(token);
  if (!payload?.exp) return true;
  return Date.now() >= payload.exp * 1000;
}
