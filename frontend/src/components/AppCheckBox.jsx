import { Checkbox, FormControlLabel } from "@mui/material";
import { useController } from "react-hook-form";
import PropTypes from "prop-types";

export default function AppCheckbox(props) {
  const { field } = useController({ ...props, defaultValue: false });

  return (
    <FormControlLabel
      control={
        <Checkbox
          {...field}
          checked={field.value}
          color="secondary"
          disabled={props.disabled}
        />
      }
      label={props.label}
    />
  );
}

AppCheckbox.propTypes = {
  name: PropTypes.string.isRequired,
  control: PropTypes.object.isRequired,
  label: PropTypes.string,
  disabled: PropTypes.bool,
};
