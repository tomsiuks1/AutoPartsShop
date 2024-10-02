import { Box, Typography, Button, Grid } from "@mui/material";
import BasketSummary from "../Basket/basketSummary";
import BasketTable from "../Basket/basketTable";
import PropTypes from "prop-types";

export default function OrderDetailed({ order, setSelectedOrder }) {
  const subtotal =
    order.orderItems.reduce(
      (sum, item) => sum + item.quantity * item.price,
      0
    ) ?? 0;
  return (
    <>
      <Box display="flex" justifyContent="space-between">
        <Typography sx={{ p: 2 }} gutterBottom variant="h4">
          Order number {order.id} - {order.orderStatus}
        </Typography>
        <Button
          onClick={() => setSelectedOrder(null)}
          sx={{ m: 2 }}
          size="large"
          variant="contained"
        >
          Back to orders
        </Button>
      </Box>
      <BasketTable items={order.orderItem} isBasket={false} />
      <Grid container>
        <Grid item xs={6} />
        <Grid item xs={6}>
          <BasketSummary subtotal={subtotal} />
        </Grid>
      </Grid>
    </>
  );
}

OrderDetailed.propTypes = {
  order: PropTypes.shape({
    id: PropTypes.number.isRequired,
    orderStatus: PropTypes.string.isRequired,
    orderItem: PropTypes.object,
    orderItems: PropTypes.arrayOf(
      PropTypes.shape({
        quantity: PropTypes.number.isRequired,
        price: PropTypes.number.isRequired,
      })
    ).isRequired,
  }).isRequired,
  setSelectedOrder: PropTypes.func.isRequired,
};
