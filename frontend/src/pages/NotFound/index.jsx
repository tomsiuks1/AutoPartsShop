import { Container, Paper, Typography, Divider, Button } from "@mui/material";
import { Link } from "react-router-dom";

export default function NotFound() {
  return (
    <Container component={Paper} sx={{ height: 400 }}>
      <Typography gutterBottom variant={"h3"}>
        Page does not exist
      </Typography>
      <Divider />
      <Button component={Link} to="/" fullWidth>
        Go back to home page
      </Button>
    </Container>
  );
}
