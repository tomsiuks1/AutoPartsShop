import { Edit, Delete } from "@mui/icons-material";
import {
  Box,
  Typography,
  Button,
  TableContainer,
  Paper,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
} from "@mui/material";
import { currencyFormat } from "../../../app/util/util";
import useCatalogItems from "../../../hooks/useCatalogItems";
import AppPagination from "../../../components/AppPagination";
import { useAppDispatch } from "../../../app/store/configureStore";
import {
  removeProduct,
  setPageNumber,
} from "../../../app/store/slices/catalogSlice";
import { useState } from "react";
import ProductForm from "./productForm";
import { LoadingButton } from "@mui/lab";
import agent from "../../../app/api/agent";

export default function Inventory() {
  const { products, metaData } = useCatalogItems();
  const [editMode, setEditMode] = useState(false);
  const dispatch = useAppDispatch();
  const [selectedProduct, setSelectedProduct] = useState(undefined);
  const [loading, setLoading] = useState(false);
  const [target, setTarget] = useState(0);

  function handleSelectProduct(product) {
    setSelectedProduct(product);
    setEditMode(true);
  }

  function cancelEdit() {
    if (selectedProduct) setSelectedProduct(undefined);
    setEditMode(false);
  }

  function handleDeleteProduct(id) {
    setLoading(true);
    setTarget(id);
    agent.Admin.deleteProduct(id)
      .then(() => dispatch(removeProduct(id)))
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  }

  if (editMode)
    return <ProductForm cancelEdit={cancelEdit} product={selectedProduct} />;

  return (
    <>
      <Box display="flex" justifyContent="space-between">
        <Typography sx={{ p: 2 }} variant="h4">
          Inventory
        </Typography>
        <Button
          sx={{ m: 2 }}
          size="large"
          variant="contained"
          onClick={() => setEditMode(true)}
        >
          Create
        </Button>
      </Box>
      <TableContainer component={Paper}>
        <Table sx={{ minWidth: 650 }} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell>#</TableCell>
              <TableCell align="left">Product</TableCell>
              <TableCell align="right">Price</TableCell>
              <TableCell align="center">Type</TableCell>
              <TableCell align="center">Brand</TableCell>
              <TableCell align="center">Quantity</TableCell>
              <TableCell align="right"></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {products &&
              products.map((product) => (
                <TableRow
                  key={product.id}
                  sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                >
                  <TableCell component="th" scope="row">
                    {product.id}
                  </TableCell>
                  <TableCell align="left">
                    <Box display="flex" alignItems="center">
                      <img
                        src={product.pictureUrl}
                        alt={product.name}
                        style={{ height: 50, marginRight: 20 }}
                      />
                      <span>{product.name}</span>
                    </Box>
                  </TableCell>
                  <TableCell align="right">
                    {currencyFormat(product.price)}
                  </TableCell>
                  <TableCell align="center">{product.type}</TableCell>
                  <TableCell align="center">{product.brand}</TableCell>
                  <TableCell align="center">
                    {product.quantityInStock}
                  </TableCell>
                  <TableCell align="right">
                    <Button
                      startIcon={<Edit />}
                      onClick={() => handleSelectProduct(product)}
                    />
                    <LoadingButton
                      loading={loading && target === product.id}
                      startIcon={<Delete />}
                      color="error"
                      onClick={() => handleDeleteProduct(product.id)}
                    />
                  </TableCell>
                </TableRow>
              ))}
          </TableBody>
        </Table>
      </TableContainer>
      {metaData && (
        <Box sx={{ pt: 2 }}>
          <AppPagination
            metaData={metaData}
            onPageChange={(page) =>
              dispatch(setPageNumber({ pageNumber: page }))
            }
          />
        </Box>
      )}
    </>
  );
}
