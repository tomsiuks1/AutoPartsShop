export function getCookie(key) {
  const b = document.cookie.match("(^|;)\\s*" + key + "\\s*=\\s*([^;]+)");
  return b ? b.pop() : "";
}

export function currencyFormat(amount) {
  return "$" + (amount / 100).toFixed(2);
}

export function getAuthenticationToken() {
  return localStorage.getItem("autoPartsShopAuthorizationToken");
}
