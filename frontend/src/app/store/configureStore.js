import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";
import { accountSlice } from "./slices/accountSlice";
import { catalogSlice } from "./slices/catalogSlice";
import { basketSlice } from "./slices/basketSlice";

export const store = configureStore({
  reducer: {
    account: accountSlice.reducer,
    catalog: catalogSlice.reducer,
    basket: basketSlice.reducer,
  },
});

export const useAppDispatch = () => useDispatch();
export const useAppSelector = useSelector;
