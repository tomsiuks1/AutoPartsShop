import { useState } from "react";
import {
  AppBar,
  Box,
  Toolbar,
  Typography,
  MenuItem,
  Container,
  Button,
  Badge,
  IconButton,
} from "@mui/material";
import PropTypes from "prop-types";
import MenuIcon from "@mui/icons-material/Menu";
import ToggleColorMode from "./ToggleColorMode";
import { Link, NavLink } from "react-router-dom";
import MenuDrawer from "./Drawers/MenuDrawer";
import LoginDrawer from "./Drawers/LoginDrawer";
import { useAppSelector } from "../store/configureStore";
import SignedInMenu from "./SignedInMenu";
import { MIDDLE_LINKS } from "../../constants";
import "./styles.css";
import { ShoppingCart } from "@mui/icons-material";

const logoStyle = {
  width: "140px",
  height: "auto",
  cursor: "pointer",
};

function Header({ mode, handleThemeChange }) {
  const { user } = useAppSelector((state) => state.account);
  const { basket } = useAppSelector((state) => state.basket);
  const itemCount = basket?.items.reduce((sum, item) => sum + item.quantity, 0);

  const [openMenuDrawer, setOpenMenuDrawer] = useState(false);
  const [openLoginDrawer, setOpenLoginDrawer] = useState(false);

  const toggleMenuDrawer = (newOpen) => () => {
    setOpenMenuDrawer(newOpen);
  };

  const toggleLoginDrawer = (newOpen) => () => {
    setOpenLoginDrawer(newOpen);
  };

  return (
    <div>
      <AppBar
        position="fixed"
        sx={{
          boxShadow: 0,
          bgcolor: "transparent",
          backgroundImage: "none",
          mt: 2,
        }}
      >
        <Container maxWidth="lg">
          <Toolbar
            variant="regular"
            sx={(theme) => ({
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              flexShrink: 0,
              borderRadius: "999px",
              bgcolor:
                theme.palette.mode === "light"
                  ? "rgba(255, 255, 255, 0.4)"
                  : "rgba(0, 0, 0, 0.4)",
              backdropFilter: "blur(24px)",
              maxHeight: 40,
              border: "1px solid",
              borderColor: "divider",
              boxShadow:
                theme.palette.mode === "light"
                  ? `0 0 1px rgba(85, 166, 246, 0.1), 1px 1.5px 2px -1px rgba(85, 166, 246, 0.15), 4px 4px 12px -2.5px rgba(85, 166, 246, 0.15)`
                  : "0 0 1px rgba(2, 31, 59, 0.7), 1px 1.5px 2px -1px rgba(2, 31, 59, 0.65), 4px 4px 12px -2.5px rgba(2, 31, 59, 0.65)",
            })}
          >
            <Box
              sx={{
                flexGrow: 1,
                display: "flex",
                alignItems: "center",
                ml: "-18px",
                px: 0,
              }}
            >
              <Box component={NavLink} to="/">
                <img
                  src="https://assets-global.website-files.com/61ed56ae9da9fd7e0ef0a967/61f12e6faf73568658154dae_SitemarkDefault.svg"
                  style={logoStyle}
                  alt="logo of sitemark"
                />
              </Box>
              <Box sx={{ display: { xs: "none", md: "flex" } }}>
                {user && user.roles?.includes("Admin") && (
                  <MenuItem
                    key={"/inventory"}
                    component={NavLink}
                    to={"/inventory"}
                  >
                    <Typography variant="body2" color="text.primary">
                      Inventory
                    </Typography>
                  </MenuItem>
                )}
                {MIDDLE_LINKS.map(({ title, path }) => (
                  <MenuItem key={path} component={NavLink} to={path}>
                    <Typography variant="body2" color="text.primary">
                      {title}
                    </Typography>
                  </MenuItem>
                ))}
              </Box>
            </Box>
            <Box
              sx={{
                display: { xs: "none", md: "flex" },
                gap: 0.5,
                alignItems: "center",
              }}
            >
              <ToggleColorMode
                mode={mode}
                toggleColorMode={handleThemeChange}
              />
              <IconButton
                component={Link}
                to="/basket"
                size="large"
                edge="start"
                color="primary"
                sx={{ mr: 2 }}
              >
                <Badge badgeContent={itemCount} color="secondary">
                  <ShoppingCart />
                </Badge>
              </IconButton>
              {!user ? (
                <Box sx={{ display: { sm: "" } }}>
                  <Button
                    color="primary"
                    variant="contained"
                    size="small"
                    onClick={toggleLoginDrawer(true)}
                    sx={{ minWidth: "30px", p: "5px" }}
                  >
                    Sign up
                  </Button>
                  <LoginDrawer
                    mode={mode}
                    handleThemeChange={handleThemeChange}
                    open={openLoginDrawer}
                    onClose={toggleLoginDrawer(false)}
                  />
                </Box>
              ) : (
                <SignedInMenu />
              )}
            </Box>
            <Box sx={{ display: { sm: "", md: "none" } }}>
              <Button
                variant="text"
                color="primary"
                aria-label="menu"
                onClick={toggleMenuDrawer(true)}
                sx={{ minWidth: "30px", p: "4px" }}
              >
                <MenuIcon />
              </Button>
              <MenuDrawer
                mode={mode}
                handleThemeChange={handleThemeChange}
                toggleLoginDrawer={toggleLoginDrawer(true)}
                open={openMenuDrawer}
                onClose={toggleMenuDrawer(false)}
              />
            </Box>
          </Toolbar>
        </Container>
      </AppBar>
    </div>
  );
}

Header.propTypes = {
  mode: PropTypes.oneOf(["dark", "light"]).isRequired,
  handleThemeChange: PropTypes.func.isRequired,
};

export default Header;
