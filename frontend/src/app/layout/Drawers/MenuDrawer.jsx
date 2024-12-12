import PropTypes from "prop-types";
import {
  Drawer,
  Typography,
  Button,
  Divider,
  Box,
  MenuItem,
  IconButton,
  Badge,
} from "@mui/material";
import ToggleColorMode from "../ToggleColorMode";
import { MIDDLE_LINKS } from "../../../constants";
import { ShoppingCart } from "@mui/icons-material";
import { useAppSelector } from "../../store/configureStore";
import { NavLink, Link } from "react-router-dom";

function MenuDrawer({
  mode,
  handleThemeChange,
  open,
  onClose,
  toggleLoginDrawer,
}) {
  const { user } = useAppSelector((state) => state.account);
  const { basket } = useAppSelector((state) => state.basket);
  const itemCount = basket?.items?.reduce((sum, item) => sum + item.quantity, 0);

  const handleLoginDrawerToggle = () => {
    onClose();
    toggleLoginDrawer();
  };

  return (
    <Drawer anchor="right" open={open} onClose={onClose}>
      <Box
        sx={{
          minWidth: "60dvw",
          p: 2,
          backgroundColor: "background.paper",
          flexGrow: 1,
        }}
      >
        <Box
          sx={{
            display: "flex",
            flexDirection: "row",
            alignItems: "center",
            justifyContent: "space-between",
            flexGrow: 1,
          }}
        >
              {user &&
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
              }
          <ToggleColorMode mode={mode} toggleColorMode={handleThemeChange} />
        </Box>
        <Divider />
        <MenuItem
          key={"/about"}
          component={NavLink}
          to={"/about"}
        >
          <Typography variant="body2" color="text.primary">
            About
          </Typography>
        </MenuItem>
        {user && user.roles?.includes("Admin") && (
          <MenuItem
            key={"/inventory"}
            component={NavLink}
            to={"/inventory"}
          >
            <Typography variant="body2" color="text.primary">
              Inventory
            </Typography>
          </MenuItem>)}
          {user && user.roles?.length > 0 && MIDDLE_LINKS.map(({ title, path }) => (
            <MenuItem key={path} component={NavLink} to={path}>
              <Typography variant="body2" color="text.primary">
                {title}
              </Typography>
            </MenuItem>
          ))}
        {!user && (
          <>
            <Divider />
            <MenuItem>
              <Button
                color="primary"
                variant="contained"
                onClick={handleLoginDrawerToggle}
                sx={{ width: "100%" }}
              >
                Sign up
              </Button>
            </MenuItem>
          </>
        )}
      </Box>
    </Drawer>
  );
}

MenuDrawer.propTypes = {
  mode: PropTypes.oneOf(["dark", "light"]).isRequired,
  handleThemeChange: PropTypes.func.isRequired,
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  toggleLoginDrawer: PropTypes.func.isRequired,
};

export default MenuDrawer;
