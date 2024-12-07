import { useState } from "react";
import PropTypes from "prop-types";
import {
  Button,
  TextField,
  FormControlLabel,
  Checkbox,
  Link,
  Box,
  Grid,
  Typography,
  Drawer,
  Divider,
} from "@mui/material";
import { useLocation, useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../store/configureStore";
import { signInUser, registerUser } from "../../store/slices/accountSlice";
import LoadingButton from "@mui/lab/LoadingButton";
import { useForm } from "react-hook-form";

function LoginDrawer({ open, onClose }) {
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useAppDispatch();

  const [isLogin, setIsLogin] = useState(true);

  const {
    register,
    handleSubmit,
    formState: { isSubmitting, errors, isValid },
  } = useForm({
    mode: "onTouched",
  });

  const submitForm = async (data) => {
    try {
      if (isLogin) {
        await dispatch(signInUser(data));
      } else {
        await dispatch(registerUser(data));
      }

      navigate(location.state?.from || "/");
      onClose();
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <Drawer anchor="right" open={open} onClose={onClose}>
      <Box
        sx={{
          p: 2,
          backgroundColor: "background.paper",
          flexGrow: 1,
        }}
      >
        <Box sx={{ display: "flex", mb: 2 }}>
          <Button
            fullWidth
            variant={isLogin ? "contained" : "outlined"}
            onClick={() => setIsLogin(true)}
            sx={{ mr: 1 }}
          >
            Login
          </Button>
          <Button
            fullWidth
            variant={!isLogin ? "contained" : "outlined"}
            onClick={() => setIsLogin(false)}
          >
            Register
          </Button>
        </Box>
        <Divider />
        <Typography component="h1" variant="h5" align="center">
          {isLogin ? "Login" : "Register"}
        </Typography>
        <Box
          component="form"
          noValidate
          onSubmit={handleSubmit(submitForm)}
          sx={{ mt: 1 }}
        >
          <TextField
            margin="normal"
            fullWidth
            label="Email"
            autoComplete="email"
            {...register("email", { required: "Email is required" })}
            error={!!errors.email}
            helperText={errors?.email?.message}
          />
          <TextField
            margin="normal"
            fullWidth
            label="Password"
            type="password"
            autoComplete="current-password"
            {...register("password", { required: "Password is required" })}
            error={!!errors.password}
            helperText={errors.password?.message}
          />
          {/* {isLogin && (
            <FormControlLabel
              control={
                <Checkbox
                  value={rememberMe}
                  color="primary"
                  onChange={(e) => setRememberMe(e.target.checked)}
                />
              }
              label="Remember me"
            />
          )} */}
          <LoadingButton
            loading={isSubmitting}
            disabled={!isValid}
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            {isLogin ? "Login" : "Register"}
          </LoadingButton>
          {/* {isLogin && (
            <Grid container>
              <Grid item xs>
                <Link href="#" variant="body2">
                  Forgot password?
                </Link>
              </Grid>
            </Grid>
          )} */}
          <Divider />
        </Box>
      </Box>
    </Drawer>
  );
}

LoginDrawer.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
};

export default LoginDrawer;
