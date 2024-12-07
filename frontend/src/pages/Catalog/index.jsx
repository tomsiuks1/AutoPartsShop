import { Grid, Paper } from "@mui/material";
import LoadingIndicator from "../../components/LoadingIndicator";
import useCatalogItems from "../../hooks/useCatalogItems";
import CatalogList from "./catalogList";
import {
  setCatalogParams,
  setPageNumber,
} from "../../app/store/slices/catalogSlice";
import CheckboxButtons from "../../components/CheckBoxButtons";
import AppPagination from "../../components/AppPagination";
import RadioButtonGroup from "../../components/RadioButtonGroup";
import { useAppSelector, useAppDispatch } from "../../app/store/configureStore";

const sortOptions = [
  { value: "name", label: "Alphabetical" },
  { value: "priceDesc", label: "Price - High to low" },
  { value: "price", label: "Price - Low to high" },
];

export default function CatalogPage() {
  const { catalogItems, filtersLoaded, brands, types, metaData } =
    useCatalogItems();
  const { catalogParams } = useAppSelector((state) => state.catalog);
  const dispatch = useAppDispatch();

    if (!filtersLoaded) return <LoadingIndicator message="Loading catalog..." active={true} />;

  return (
    <Grid container columnSpacing={5} sx={{ marginTop: "100px" }}>
      <Grid item xs={12} md={3}>
        <Paper sx={{ p: 2, mb: 2 }}>
          <RadioButtonGroup
            selectedValue={catalogParams.orderBy}
            options={sortOptions}
            onChange={(e) =>
              dispatch(setCatalogParams({ orderBy: e.target.value }))
            }
          />
        </Paper>
        <Paper sx={{ p: 2, mb: 2 }}>
          <CheckboxButtons
            items={brands}
            checked={catalogParams.brands}
            onChange={(items) => dispatch(setCatalogParams({ brands: items }))}
          />
        </Paper>
        <Paper sx={{ p: 2 }}>
          <CheckboxButtons
            items={types}
            checked={catalogParams.types}
            onChange={(items) => dispatch(setCatalogParams({ types: items }))}
          />
        </Paper>
      </Grid>
      <Grid item xs={12} md={9}>
        <CatalogList catalogItems={catalogItems} />
      </Grid>
      <Grid item xs={false} md={3} />
      <Grid item xs={12} md={9} sx={{ mb: 2 }}>
        {metaData && (
          <AppPagination
            metaData={metaData}
            onPageChange={(page) =>
              dispatch(setPageNumber({ pageNumber: page }))
            }
          />
        )}
      </Grid>
    </Grid>
  );
}
