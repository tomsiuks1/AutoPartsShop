import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import agent from "../../api/agent";
import { router } from "../../router/Routes";
import { toast } from "react-toastify";

const initialState = {
  user: null,
};

export const signInUser = createAsyncThunk(
  "account/login",
  async (data, thunkAPI) => {
    try {
      const userDto = await agent.Account.login(data);
      const { ...user } = userDto;
      router.navigate("/catalog");
      return user;
    } catch (error) {
      return thunkAPI.rejectWithValue({ error: error.data });
    }
  }
);

export const registerUser = createAsyncThunk(
  "account/register",
  async (data, thunkAPI) => {
    try {
      const userDto = await agent.Account.register(data);
      const { ...user } = userDto;
      router.navigate("/catalog");
      localStorage.setItem("user", JSON.stringify(user));
      return user;
    } catch (error) {
      return thunkAPI.rejectWithValue({ error: error.data });
    }
  }
);

export const fetchCurrentUser = createAsyncThunk(
  "account/currentUser",
  async (_, thunkAPI) => {
    try {
      const userDto = await agent.Account.currentUser();
      const { ...user } = userDto;
      return user;
    } catch (error) {
      return thunkAPI.rejectWithValue({ error: error.data });
    }
  }
);

export const accountSlice = createSlice({
  name: "account",
  initialState,
  reducers: {
    signOut: (state) => {
      state.user = null;
      localStorage.removeItem("user");
      router.navigate("/");
    },
    setUser: (state, action) => {
      const claims = JSON.parse(atob(action.payload.token.split(".")[1]));
      const roles =
        claims["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      state.user = {
        ...action.payload,
        roles: typeof roles === "string" ? [roles] : roles,
      };
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchCurrentUser.rejected, (state) => {
        state.user = null;
        localStorage.removeItem("autoPartsShopAuthorizationToken");
        toast.error("Session expired - please login again");
        router.navigate("/");
      })
      .addMatcher(
        (action) =>
          action.type === signInUser.fulfilled.type ||
          action.type === registerUser.fulfilled.type ||
          action.type === fetchCurrentUser.fulfilled.type,
        (state, action) => {
          const claims = JSON.parse(atob(action.payload.token.split(".")[1]));
          const roles =
            claims[
              "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            ];
          state.user = {
            ...action.payload,
            roles: typeof roles === "string" ? [roles] : roles,
          };
          state.user = {
            ...action.payload,
            roles: [claims.role],
          };
        }
      )
      .addMatcher(
        (action) => action.type === signInUser.rejected.type,
        (_state, action) => {
          throw action.payload;
        }
      );
  },
});

export const { signOut, setUser } = accountSlice.actions;
