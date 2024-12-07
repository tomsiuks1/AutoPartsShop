import {
  Divider,
  Grid,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  TextField,
  Typography,
  ListItem,
  List,
  Button,
  IconButton
} from "@mui/material";
import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import EditIcon from '@mui/icons-material/Edit';
import NotFound from "../NotFound";
import { LoadingButton } from "@mui/lab";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import {
  addBasketItemAsync,
  removeBasketItemAsync,
} from "../../app/store/slices/basketSlice";
import { format, parseISO } from 'date-fns';
import {
  fetchCatalogItemAsync,
  catalogSelectors,
} from "../../app/store/slices/catalogSlice";
import LoadingIndicator from "../../components/LoadingIndicator";
import agent from "../../app/api/agent";

export default function CatalogItemDetails() {
  const { id } = useParams();
  const dispatch = useAppDispatch();
  const { basket, status } = useAppSelector((state) => state.basket);
  const product = useAppSelector((state) =>
    catalogSelectors.selectById(state, id)
  );
  // const { status: productStatus } = useAppSelector((state) => state.catalog);
  const currentUser = useAppSelector((state) => state.account.user);
  
  const [quantity, setQuantity] = useState(0);
  const [isLoadingIndicatorActive, setIsLoadingIndicatorActive] = useState(false);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');
  const [editingCommentId, setEditingCommentId] = useState(null);
  const [editingContent, setEditingContent] = useState('');
  const [isPageLoading, setIsPageLoading] = useState(true);

  const handleAddComment = async () => {
    if (!newComment.trim()) return;

    setIsLoadingIndicatorActive(true);
    try {
      const comment = await agent.Comments.addComment(product.id, {
        content: newComment,
      });
      setComments([...comments, comment]);
      setNewComment('');
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoadingIndicatorActive(false);
    }
  };

  const handleEditClick = (commentId, content) => {
    setEditingCommentId(commentId);
    setEditingContent(content);
  };

  const handleSaveEdit = async () => {
    setIsLoadingIndicatorActive(true);
    try {
      await agent.Comments.updateComment(product.id, editingCommentId, { content: editingContent });
      await agent.Comments.getComments(product.id)
      .then(response => setComments(response))
      .catch(error => console.log(error));
      setEditingCommentId(null);
      setEditingContent('');
    } catch (error) {
      console.log(error);
    } finally {
      setIsLoadingIndicatorActive(false);
    }
  };

  const handleDeleteComment = async (commentId) => {
    if(!commentId) {
      return;
    }

    setIsLoadingIndicatorActive(true);
    try {
      await agent.Comments.deleteComment(product.id, commentId);
      setComments((prevComments) =>
        prevComments.filter((comment) => comment.id !== commentId)
      );
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoadingIndicatorActive(false);
    }
  };

  const handleCancelEdit = () => {
    setEditingCommentId(null);
    setEditingContent('');
  };

  const item = basket?.items.find(
    (i) => i.catalogItemId === product?.catalogItemId
  );

  useEffect(() => {
    if (item) setQuantity(item.quantity);
    if (!product && id) dispatch(fetchCatalogItemAsync(id));
  
    const fetchData = async () => {
      setIsPageLoading(true);
      try {
        if (product?.id) {
          const comments = await agent.Comments.getComments(product.id);
          setComments(comments);
        }
      } catch (error) {
        console.error("Failed to fetch comments:", error);
      } finally {
        setIsPageLoading(false);
      }
    };
  
    fetchData();
  }, [id, item, product, dispatch]);

  function handleInputChange(event) {
    if (parseInt(event.currentTarget.value) >= 0)
      setQuantity(parseInt(event.currentTarget.value));
  }

  function handleUpdateCart() {
    if (!product) return;

    if (!item || quantity > item?.quantity) {
      console.log(product);
      const updatedQuantity = item ? quantity - item.quantity : quantity;
      dispatch(
        addBasketItemAsync({
          catalogItemId: product.id,
          quantity: updatedQuantity,
        })
      );
    } else {
      const updatedQuantity = item.quantity - quantity;
      dispatch(
        removeBasketItemAsync({
          catalogItemId: product.id,
          quantity: updatedQuantity,
        })
      );
    }
  }

  if (isPageLoading || !product) {
    return <LoadingIndicator message="Loading Product..." active={true}/>;
  }

  if (!product) return <NotFound />;

  return (
    <>
    <Grid container spacing={6}>
      <Grid item xs={6}>
        <img
          src={product.pictureUrl}
          alt={product.name}
          style={{ width: "100%" }}
        />
      </Grid>
      <Grid item xs={6}>
        <Typography variant="h3">{product.name}</Typography>
        <Divider sx={{ mb: 2 }} />
        <Typography variant="h4" color="secondary">
          ${(product.price / 100).toFixed(2)}
        </Typography>
        <TableContainer>
          <Table>
            <TableBody sx={{ fontSize: "1.1em" }}>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>{product.name}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Description</TableCell>
                <TableCell>{product.description}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Type</TableCell>
                <TableCell>{product.type}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Brand</TableCell>
                <TableCell>{product.brand}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Quantity in stock</TableCell>
                <TableCell>{product.quantityInStock}</TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </TableContainer>
        <Grid container spacing={2}>
          <Grid item xs={6}>
            <TextField
              onChange={handleInputChange}
              variant={"outlined"}
              type={"number"}
              label={"Quantity in Cart"}
              fullWidth
              value={quantity}
            />
          </Grid>
          <Grid item xs={6}>
            <LoadingButton
              disabled={
                item?.quantity === quantity || (!item && quantity === 0)
              }
              loading={status.includes("pending")}
              onClick={handleUpdateCart}
              sx={{ height: "55px" }}
              color={"primary"}
              size={"large"}
              variant={"contained"}
              fullWidth
            >
              {item ? "Update Quantity" : "Add to Cart"}
            </LoadingButton>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
    <div>
      <Typography variant="h6" style={{ marginTop: '2rem' }}>
        Comments
      </Typography>
      <List>
        {comments?.map((comment) => (
          <ListItem key={comment.id} alignItems="flex-start">
            <Grid container spacing={1}>
              <Grid item xs={12}>
                {editingCommentId === comment.id ? (
                  <TextField
                    multiline
                    rows={2}
                    fullWidth
                    value={editingContent}
                    onChange={(e) => setEditingContent(e.target.value)}
                  />
                ) : (
                  <Typography variant="body1">{comment.content}</Typography>
                )}
              </Grid>
              <Grid item xs={6}>
                <Typography variant="caption" color="textSecondary">
                  {format(parseISO(comment?.createdAt), 'PPpp')}
                </Typography>
                <Typography variant="caption" color="textSecondary">
                  {` Created By: ${comment?.displayName}`}
                </Typography>
              </Grid>
              <Grid item xs={6} style={{ textAlign: 'right' }}>
                {currentUser && comment.userId === currentUser.id && (
                  <>
                    {editingCommentId === comment.id ? (
                      <>
                        {/* Save and Cancel buttons for editing */}
                        <LoadingButton
                          variant="contained"
                          color="primary"
                          size="small"
                          onClick={handleSaveEdit}
                          loading={isLoadingIndicatorActive}
                          disabled={!editingContent.trim()}
                          style={{ marginRight: '0.5rem' }}
                        >
                          Save
                        </LoadingButton>
                        <Button
                          variant="outlined"
                          color="secondary"
                          size="small"
                          onClick={handleCancelEdit}
                        >
                          Cancel
                        </Button>
                      </>
                    ) : (
                      <>
                        <IconButton
                          size="small"
                          onClick={() => handleEditClick(comment.id, comment.content)}
                        >
                          <EditIcon fontSize="small" />
                        </IconButton>
                        <LoadingButton
                          variant="contained"
                          color="primary"
                          size="small"
                          onClick={() => handleDeleteComment(comment.id)}
                          loading={isLoadingIndicatorActive}
                        >
                          Delete
                        </LoadingButton>
                      </>
                    )}
                  </>
                )}
              </Grid>
            </Grid>
          </ListItem>
        ))}
      </List>

      {/* Add New Comment */}
      <Grid container spacing={2} style={{ marginTop: '1rem' }}>
        <Grid item xs={12}>
          <TextField
            label="Add a comment"
            variant="outlined"
            multiline
            rows={3}
            value={newComment}
            onChange={(e) => setNewComment(e.target.value)}
            fullWidth
          />
        </Grid>
        <Grid item xs={12}>
          <LoadingButton
            variant="contained"
            color="primary"
            onClick={handleAddComment}
            loading={isLoadingIndicatorActive}
            disabled={!newComment.trim()}
          >
            Submit
          </LoadingButton>
        </Grid>
      </Grid>
    </div>
    </>
  );
}
