import { Backdrop, Box, CircularProgress, Typography } from "@mui/material";
import PropTypes from "prop-types";

export default function LoadingIndicator({ message = "Loading...", active }) {
  return (
    <Backdrop open={active}>
      <Box
        alignItems="center"
        display="flex"
        justifyContent="center"
        height="100vh"
        width="100vw"
        position="relative"
      >
        <CircularProgress size={100} color="primary" />
        <Typography
          variant="h4"
          sx={{ justifyContent: "center", position: "fixed", top: "60%" }}
        >
          {message}
        </Typography>
      </Box>
    </Backdrop>
  );
}

LoadingIndicator.propTypes = {
  message: PropTypes.string,
  active: PropTypes.bool,
};
