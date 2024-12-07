import {
  Container,
  CssBaseline,
  ThemeProvider,
  createTheme,
} from "@mui/material";
import "./App.css";
import { useState, useEffect, useCallback } from "react";
import Header from "./layout/Header";
import { Outlet } from "react-router-dom";
import LoadingIndicator from "../components/LoadingIndicator";
import { ToastContainer } from "react-toastify";
import { useAppDispatch } from "./store/configureStore";
import { fetchCurrentUser } from "./store/slices/accountSlice";
import { fetchBasketAsync } from "./store/slices/basketSlice";
import "react-toastify/dist/ReactToastify.css";
import ErrorBoundary from "./ErrorBoundry";
import Footer from "./layout/Footer";

function App() {
  const dispatch = useAppDispatch();
  const [mode, setMode] = useState("light");
  const [isLoading, setIsLoading] = useState(true);
  const [isLoginModalOpen, setIsLoginModalOpen] = useState(false);

  const initApp = useCallback(async () => {
    try {
      await dispatch(fetchCurrentUser());
      await dispatch(fetchBasketAsync());
    } catch (error) {
      console.log(error);
    }
  }, [dispatch]);

  useEffect(() => {
    initApp().then(() => setIsLoading(false));
  }, [initApp]);

  const defaultTheme = createTheme({ palette: { mode } });

  const messageDuration = 5000;

  const handleThemeChange = () => {
    setMode((prev) => (prev === "dark" ? "light" : "dark"));
  };

  return (
    <ThemeProvider theme={defaultTheme}>
      <CssBaseline />
      <ErrorBoundary>
        <Header
          mode={mode}
          handleThemeChange={handleThemeChange}
          setIsLoginModalOpen={() => setIsLoginModalOpen(!isLoginModalOpen)}
        />
        <Container>
          <Outlet />
        </Container>
        <Footer />
      </ErrorBoundary>
      <LoadingIndicator active={isLoading} />
      <ToastContainer theme={mode} autoClose={messageDuration} />
    </ThemeProvider>
  );
}

export default App;
