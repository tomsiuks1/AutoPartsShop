import { Remove, Add, Delete } from "@mui/icons-material";
import { LoadingButton } from "@mui/lab";
import {
  TableContainer,
  Paper,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Box,
} from "@mui/material";
import PropTypes from "prop-types";
import { useAppSelector, useAppDispatch } from "../../app/store/configureStore";
import {
  removeBasketItemAsync,
  addBasketItemAsync,
} from "../../app/store/slices/basketSlice";

export default function BasketTable({ items, isBasket = true }) {
  const { status } = useAppSelector((state) => state.basket);
  const dispatch = useAppDispatch();

  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }}>
        <TableHead>
          <TableRow>
            <TableCell>Product</TableCell>
            <TableCell align="right">Price</TableCell>
            <TableCell align="center">Quantity</TableCell>
            <TableCell align="right">Subtotal</TableCell>
            {isBasket && <TableCell align="right"></TableCell>}
          </TableRow>
        </TableHead>
        <TableBody>
          {items &&
            items.map((item) => (
              <TableRow
                key={item.catalogItemId}
                sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
              >
                <TableCell component="th" scope="row">
                  <Box display="flex" alignItems="center">
                    <img
                      src={item.pictureUrl}
                      alt={item.name}
                      style={{ height: 50, marginRight: 20 }}
                    />
                    <span>{item.name}</span>
                  </Box>
                </TableCell>
                <TableCell align="right">
                  ${(item.price / 100).toFixed(2)}
                </TableCell>
                <TableCell align="center">
                  {isBasket && (
                    <LoadingButton
                      loading={
                        status ===
                        "pendingRemoveItem" + item.catalogItemId + "rem"
                      }
                      onClick={() =>
                        dispatch(
                          removeBasketItemAsync({
                            catalogItemId: item.catalogItemId,
                            quantity: 1,
                            name: "rem",
                          })
                        )
                      }
                      color="error"
                    >
                      <Remove />
                    </LoadingButton>
                  )}
                  {item.quantity}
                  {isBasket && (
                    <LoadingButton
                      loading={status === "pendingAddItem" + item.catalogItemId}
                      onClick={() =>
                        dispatch(
                          addBasketItemAsync({
                            catalogItemId: item.catalogItemId,
                          })
                        )
                      }
                      color="secondary"
                    >
                      <Add />
                    </LoadingButton>
                  )}
                </TableCell>
                <TableCell align="right">
                  ${((item.price / 100) * item.quantity).toFixed(2)}
                </TableCell>
                {isBasket && (
                  <TableCell align="right">
                    <LoadingButton
                      loading={
                        status ===
                        "pendingRemoveItem" + item.catalogItemId + "del"
                      }
                      onClick={() =>
                        dispatch(
                          removeBasketItemAsync({
                            catalogItemId: item.catalogItemId,
                            quantity: item.quantity,
                            name: "del",
                          })
                        )
                      }
                      color="error"
                    >
                      <Delete />
                    </LoadingButton>
                  </TableCell>
                )}
              </TableRow>
            ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}

BasketTable.propTypes = {
  items: PropTypes.arrayOf(
    PropTypes.shape({
      catalogItemId: PropTypes.string.isRequired,
      name: PropTypes.string.isRequired,
      price: PropTypes.number.isRequired,
      quantity: PropTypes.number.isRequired,
      pictureUrl: PropTypes.string,
    })
  ).isRequired,
  isBasket: PropTypes.bool,
};
