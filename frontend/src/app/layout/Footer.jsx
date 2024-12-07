import { Box, Container, Typography } from "@mui/material";

export default function Footer() {
  return (
    <Box
      component="footer"
      sx={(theme) => ({
        backgroundColor: theme.palette.mode === "light" ? "white" : "grey.900",
        color: theme.palette.text.secondary,
        py: 2,
        borderTop: `1px solid ${theme.palette.divider}`,
        mt: "auto",
      })}
    >
      <Container maxWidth="lg">
        <Typography variant="body2" align="center">
          © {new Date().getFullYear()} AutoPartsShop. All rights reserved.
        </Typography>
      </Container>
    </Box>
  );
}
