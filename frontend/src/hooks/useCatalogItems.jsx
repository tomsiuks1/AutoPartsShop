import { useEffect } from "react";
import {
  catalogSelectors,
  fetchCatalogItemsAsync,
  fetchFilters,
} from "../app/store/slices/catalogSlice";
import { useAppSelector, useAppDispatch } from "../app/store/configureStore";

export default function useCatalogItems() {
  const catalogItems = useAppSelector(catalogSelectors.selectAll);
  const { catalogItemsLoaded, filtersLoaded, brands, types, metaData } =
    useAppSelector((state) => state.catalog);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (!catalogItemsLoaded) dispatch(fetchCatalogItemsAsync());
  }, [catalogItemsLoaded, dispatch]);

  useEffect(() => {
    if (!filtersLoaded) dispatch(fetchFilters());
  }, [dispatch, filtersLoaded]);

  return {
    catalogItems,
    catalogItemsLoaded,
    filtersLoaded,
    brands,
    types,
    metaData,
  };
}
