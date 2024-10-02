import {
  Avatar,
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  CardMedia,
  Typography,
} from "@mui/material";
import { Link } from "react-router-dom";
import { currencyFormat } from "../../app/util/util";
import { LoadingButton } from "@mui/lab";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { addBasketItemAsync } from "../../app/store/slices/basketSlice";
import PropTypes from "prop-types";

export default function CatalogItemCard({ catalogItem }) {
  const { status } = useAppSelector((state) => state.basket);
  const dispatch = useAppDispatch();

  return (
    <Card>
      <CardHeader
        avatar={
          <Avatar sx={{ bgcolor: "secondary.main" }}>
            {catalogItem.name.charAt(0).toUpperCase()}
          </Avatar>
        }
        title={catalogItem.name}
        titleTypographyProps={{
          sx: { fontWeight: "bold", color: "primary.main" },
        }}
      />
      <CardMedia
        sx={{
          height: 140,
          backgroundSize: "contain",
          bgcolor: "primary.light",
        }}
        image={catalogItem.pictureUrl}
        title={catalogItem.name}
      />
      <CardContent>
        <Typography gutterBottom color="secondary" variant="h5" component="div">
          {currencyFormat(catalogItem.price)}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {catalogItem.brand} / {catalogItem.type}
        </Typography>
      </CardContent>
      <CardActions>
        <LoadingButton
          loading={status === "pendingAddItem" + catalogItem.id}
          onClick={() =>
            dispatch(addBasketItemAsync({ catalogItemId: catalogItem.id }))
          }
          size="small"
        >
          Add to Cart
        </LoadingButton>
        <Button component={Link} to={`/catalog/${catalogItem.id}`} size="small">
          View
        </Button>
      </CardActions>
    </Card>
  );
}

CatalogItemCard.propTypes = {
  catalogItem: PropTypes.shape({
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    price: PropTypes.number.isRequired,
    pictureUrl: PropTypes.string.isRequired,
    brand: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
  }).isRequired,
};
