import { useEffect } from "react";
import PropTypes from "prop-types";
import { Box, Paper, Typography, Grid, Button } from "@mui/material";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { LoadingButton } from "@mui/lab";
import AppTextInput from "../../../components/AppTextInput";
import AppSelectList from "../../../components/AppSelectList";
import useCatalogItems from "../../../hooks/useCatalogItems";
import { validationSchema } from "./productValidation";
import { useAppDispatch } from "../../../app/store/configureStore";
import agent from "../../../app/api/agent";
import { setProduct } from "../../../app/store/slices/catalogSlice";

export default function ProductForm({ product, cancelEdit }) {
  const {
    control,
    reset,
    handleSubmit,
    watch,
    formState: { isDirty, isSubmitting },
  } = useForm({
    mode: "onTouched",
    resolver: yupResolver(validationSchema),
  });
  const { brands, types } = useCatalogItems();
  const watchFile = watch("file", null);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (product && !watchFile && !isDirty) reset(product);
    return () => {
      if (watchFile) URL.revokeObjectURL(watchFile.preview);
    };
  }, [product, reset, watchFile, isDirty]);

  async function handleSubmitData(data) {
    try {
      let response;
      if (product) {
        response = await agent.Admin.updateProduct(product.id, data);
      } else {
        response = await agent.Admin.createProduct(data);
      }
      dispatch(setProduct(response));
      cancelEdit();
    } catch (error) {
      console.log(error);
    }
  }

  return (
    <Box component={Paper} sx={{ p: 4 }}>
      <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
        Product Details
      </Typography>
      <form onSubmit={handleSubmit(handleSubmitData)}>
        <Grid container spacing={3}>
          <Grid item xs={12} sm={12}>
            <AppTextInput control={control} name="name" label="Product name" />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppSelectList
              items={brands}
              control={control}
              name="brand"
              label="Brand"
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppSelectList
              items={types}
              control={control}
              name="type"
              label="Category"
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppTextInput
              type="number"
              control={control}
              name="price"
              label="Price"
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppTextInput
              type="number"
              control={control}
              name="quantityInStock"
              label="Quantity in Stock"
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppTextInput
              control={control}
              name="pictureUrl"
              label="Picture Url"
            />
          </Grid>
          <Grid item xs={12}>
            <div style={{ marginTop: '1rem', textAlign: 'center' }}>
              {watch('pictureUrl') ? (
                <img
                  src={watch('pictureUrl')}
                  alt="Uploaded Preview"
                  style={{
                    maxWidth: '100%',
                    maxHeight: '400px',
                    border: '1px solid #ccc',
                    borderRadius: '8px',
                  }}
                />
              ) : (
                <Typography variant="caption" color="textSecondary">
                  No image to display. Enter a valid URL.
                </Typography>
              )}
            </div>
          </Grid>
          <Grid item xs={12}>
            <AppTextInput
              multiline={true}
              rows={4}
              control={control}
              name="description"
              label="Description"
            />
          </Grid>
        </Grid>
        <Box display="flex" justifyContent="space-between" sx={{ mt: 3 }}>
          <Button onClick={cancelEdit} variant="contained" color="inherit">
            Cancel
          </Button>
          <LoadingButton
            loading={isSubmitting}
            type="submit"
            onClick={handleSubmit(handleSubmitData)}
            variant="contained"
            color="primary"
          >
            Save
          </LoadingButton>
        </Box>
      </form>
    </Box>
  );
}

ProductForm.propTypes = {
  product: PropTypes.shape({
    id: PropTypes.string,
    name: PropTypes.string.isRequired,
    brand: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
    price: PropTypes.number.isRequired,
    quantityInStock: PropTypes.number.isRequired,
    description: PropTypes.string,
    pictureUrl: PropTypes.string,
  }),
  cancelEdit: PropTypes.func.isRequired,
};
