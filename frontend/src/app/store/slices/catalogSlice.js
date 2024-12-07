import {
  createAsyncThunk,
  createEntityAdapter,
  createSlice,
} from "@reduxjs/toolkit";
import agent from "../../api/agent";

const catalogAdapter = createEntityAdapter();

function getAxiosParams(catalogParams) {
  const params = new URLSearchParams();
  params.append("pageNumber", catalogParams.pageNumber.toString());
  params.append("pageSize", catalogParams.pageSize.toString());
  params.append("orderBy", catalogParams.orderBy);
  if (catalogParams.searchTerm)
    params.append("searchTerm", catalogParams.searchTerm);
  if (catalogParams.types.length > 0)
    params.append("types", catalogParams.types.toString());
  if (catalogParams.brands.length > 0)
    params.append("brands", catalogParams.brands.toString());
  return params;
}

export const fetchCatalogItemsAsync = createAsyncThunk(
  "products/fetchProductsAsync",
  async (_, thunkAPI) => {
    const params = getAxiosParams(thunkAPI.getState().catalog.catalogParams);
    try {
      const response = await agent.Catalog.getCatalog(params);

      thunkAPI.dispatch(setMetaData(response.metaData));
      return response.items;
    } catch (error) {
      return thunkAPI.rejectWithValue({ error: error.data });
    }
  }
);

export const fetchCatalogItemAsync = createAsyncThunk(
  "products/fetchProductAsync",
  async (catalogItemId, thunkAPI) => {
    try {
      const catalogItem = await agent.Catalog.details(catalogItemId);
      return catalogItem;
    } catch (error) {
      return thunkAPI.rejectWithValue({ error: error.data });
    }
  }
);

export const fetchFilters = createAsyncThunk(
  "products/fetchFilters",
  async (_, thunkAPI) => {
    try {
      return agent.Catalog.fetchFilters();
    } catch (error) {
      return thunkAPI.rejectWithValue({ error: error.message });
    }
  }
);

function initParams() {
  return {
    pageNumber: 1,
    pageSize: 6,
    orderBy: "name",
    brands: [],
    types: [],
  };
}

export const catalogSlice = createSlice({
  name: "catalog",
  initialState: catalogAdapter.getInitialState({
    catalogItemsLoaded: false,
    filtersLoaded: false,
    status: "idle",
    brands: [],
    types: [],
    catalogParams: initParams(),
    metaData: null,
  }),
  reducers: {
    setCatalogParams: (state, action) => {
      state.catalogItemsLoaded = false;
      state.catalogParams = {
        ...state.catalogParams,
        ...action.payload,
        pageNumber: 1,
      };
    },
    setPageNumber: (state, action) => {
      state.catalogItemsLoaded = false;
      state.catalogParams = { ...state.catalogParams, ...action.payload };
    },
    setMetaData: (state, action) => {
      state.metaData = action.payload;
    },
    resetProductParams: (state) => {
      state.catalogParams = initParams();
    },
    setProduct: (state, action) => {
      catalogAdapter.upsertOne(state, action.payload);
      state.catalogItemsLoaded = false;
    },
    removeProduct: (state, action) => {
      catalogAdapter.removeOne(state, action.payload);
      state.catalogItemsLoaded = false;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchCatalogItemsAsync.pending, (state) => {
      state.status = "pendingFetchCatalogItems";
    });
    builder.addCase(fetchCatalogItemsAsync.fulfilled, (state, action) => {
      catalogAdapter.setAll(state, action.payload);
      (state.status = "idle"), (state.catalogItemsLoaded = true);
    });
    builder.addCase(fetchCatalogItemsAsync.rejected, (state, action) => {
      console.log(action.payload);
      state.status = "idle";
    });
    builder.addCase(fetchCatalogItemAsync.pending, (state) => {
      state.status = "pendingFetchCatalogItem";
    });
    builder.addCase(fetchCatalogItemAsync.fulfilled, (state, action) => {
      catalogAdapter.upsertOne(state, action.payload);
      state.status = "idle";
    });
    builder.addCase(fetchCatalogItemAsync.rejected, (state, action) => {
      console.log(action);
      state.status = "idle";
    });
    builder.addCase(fetchFilters.pending, (state) => {
      state.status = "pendingFetchFilters";
    });
    builder.addCase(fetchFilters.fulfilled, (state, action) => {
      state.brands = action.payload.brands;
      state.types = action.payload.types;
      state.status = "idle";
      state.filtersLoaded = true;
    });
    builder.addCase(fetchFilters.rejected, (state) => {
      state.status = "idle";
    });
  },
});

export const {
  setCatalogParams,
  resetProductParams,
  setMetaData,
  setPageNumber,
  setProduct,
  removeProduct,
} = catalogSlice.actions;

export const catalogSelectors = catalogAdapter.getSelectors(
  (state) => state.catalog
);
