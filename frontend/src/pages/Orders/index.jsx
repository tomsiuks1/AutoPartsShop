import {
  TableContainer,
  Paper,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Button,
} from "@mui/material";
import { useEffect, useState } from "react";
import agent from "../../app/api/agent";
import { currencyFormat } from "../../app/util/util";
import OrderDetails from "./OrderDetails";
import LoadingIndicator from "../../components/LoadingIndicator";

export default function OrdersPage() {
  const [orders, setOrders] = useState(null);
  const [loading, setLoading] = useState(true);
  const [selectedOrderNumber, setSelectedOrderNumber] = useState(null);

  useEffect(() => {
    setLoading(true);
    agent.Orders.list()
      .then((orders) => setOrders(orders))
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <LoadingIndicator message="Loading orders..." />;

  if (selectedOrderNumber !== null && orders)
    return (
      <OrderDetails
        order={orders.find((o) => o.id === selectedOrderNumber)}
        setSelectedOrder={setSelectedOrderNumber}
      />
    );

  console.log(orders);

  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} aria-label="simple table">
        <TableHead>
          <TableRow>
            <TableCell>Order Number</TableCell>
            <TableCell align="right">Total</TableCell>
            <TableCell align="right">Order Date</TableCell>
            <TableCell align="right">Order Status</TableCell>
            <TableCell align="right"></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {orders &&
            orders?.map((order) => (
              <TableRow
                key={order.id}
                sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
              >
                <TableCell component="th" scope="row">
                  {order.id}
                </TableCell>
                <TableCell align="right">
                  {currencyFormat(order.total)}
                </TableCell>
                <TableCell align="right">
                  {order.orderDate.split("T")[0]}
                </TableCell>
                <TableCell align="right">{order.orderStatus}</TableCell>
                <TableCell align="right">
                  <Button onClick={() => setSelectedOrderNumber(order.id)}>
                    View
                  </Button>
                </TableCell>
              </TableRow>
            ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
