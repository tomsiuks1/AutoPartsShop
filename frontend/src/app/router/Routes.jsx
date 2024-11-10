import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../App";
import HomePage from "../../pages/Home";
import AboutPage from "../../pages/About";
import CatalogPage from "../../pages/Catalog";
import NotFound from "../../pages/NotFound";
import BasketPage from "../../pages/Basket";
import RequireAuth from "./RequireAuth";
import CatalogItemDetails from "../../pages/Catalog/catalogItemDetails";
import CheckoutWrapper from "../../pages/Checkout/checkoutWrapper";
import OrdersPage from "../../pages/Orders";
import Inventory from "../../pages/Admin/Inventory";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <HomePage /> },
      {
        element: <RequireAuth roles={["User", "Member", "Admin"]} />,
        children: [
          { path: "catalog/:id", element: <CatalogItemDetails /> },
          { path: "/checkout", element: <CheckoutWrapper /> },
          { path: "/orders", element: <OrdersPage /> },
        ],
      },
      {
        element: <RequireAuth roles={["Admin"]} />,
        children: [{ path: "/inventory", element: <Inventory /> }],
      },
      { path: "/about", element: <AboutPage /> },
      { path: "/catalog", element: <CatalogPage /> },
      { path: "/basket", element: <BasketPage /> },
      { path: "/not-found", element: <NotFound /> },
      { path: "*", element: <Navigate replace to="/not-found" /> },
    ],
  },
]);
