import { Grid } from "@mui/material";
import CatalogItemCard from "./catalogItemCard";
import { useAppSelector } from "../../app/store/configureStore";
import CatalogCardSkeleton from "./catalogItemCardSkeleton";

export default function CatalogList({ catalogItems }) {
  const { catalogItemsLoaded } = useAppSelector((state) => state.catalog);

  return (
    <Grid container spacing={3}> 
      {catalogItems && catalogItems?.map((catalogItem) => (
        <Grid item xs={10} md={4} key={catalogItem.id}>
          {!catalogItemsLoaded ? (
            <CatalogCardSkeleton />
          ) : (
            <CatalogItemCard catalogItem={catalogItem} />
          )}
        </Grid>
      ))}
    </Grid>
  );
}
