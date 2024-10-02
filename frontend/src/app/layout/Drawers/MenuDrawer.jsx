import PropTypes from "prop-types";
import {
  Drawer,
  Typography,
  Button,
  Divider,
  Box,
  MenuItem,
} from "@mui/material";
import ToggleColorMode from "../ToggleColorMode";
import { MIDDLE_LINKS } from "../../../constants";
import { useAppSelector } from "../../store/configureStore";
import { NavLink } from "react-router-dom";

function MenuDrawer({
  mode,
  handleThemeChange,
  open,
  onClose,
  toggleLoginDrawer,
}) {
  const { user } = useAppSelector((state) => state.account);
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
            flexDirection: "column",
            alignItems: "end",
            flexGrow: 1,
          }}
        >
          <ToggleColorMode mode={mode} toggleColorMode={handleThemeChange} />
        </Box>
        {MIDDLE_LINKS.map(({ title, path }) => (
          <MenuItem
            key={path}
            component={NavLink}
            to={path}
            onClick={onClose}
            sx={{ py: "6px", px: "12px" }}
          >
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
