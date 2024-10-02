import { TextField } from "@mui/material";
import { useController } from "react-hook-form";
import PropTypes from "prop-types";

export default function AppTextInput(props) {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });

  return (
    <TextField
      {...props}
      {...field}
      multiline={props.multiline}
      rows={props.rows}
      type={props.type}
      fullWidth
      variant="outlined"
      error={!!fieldState.error}
      helperText={fieldState.error?.message}
    />
  );
}

AppTextInput.propTypes = {
  name: PropTypes.string.isRequired,
  label: PropTypes.string,
  control: PropTypes.object.isRequired,
  multiline: PropTypes.bool,
  rows: PropTypes.number,
  type: PropTypes.string,
};
