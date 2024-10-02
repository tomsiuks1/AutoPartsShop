import { Box, Typography, Container } from "@mui/material";

export default function AboutPage() {
  return (
    <Container maxWidth="md">
      <Box sx={{ my: 4 }}>
        <Typography variant="body1" paragraph>
          Welcome to <strong>Auto Parts shop</strong>, your one-stop destination
          for all automotive needs! Whether you are a professional mechanic, a
          DIY enthusiast, or just looking for a specific part to keep your
          vehicle running smoothly, GearUp Auto Parts has got you covered. Our
          extensive inventory includes everything from essential components like
          brakes, batteries, and filters to high-performance parts for those
          looking to enhance their ride. We pride ourselves on offering quality
          products from trusted brands, ensuring you get the best reliability
          and performance.
        </Typography>
        <Typography variant="body1" paragraph>
          At GearUp Auto Parts, our knowledgeable and friendly staff are always
          ready to assist you. Need help finding the right part? Our team can
          guide you through our wide selection and provide expert advice to make
          sure you get exactly what you need. Plus, with our convenient online
          store, you can shop from the comfort of your home and enjoy fast
          shipping directly to your door.
        </Typography>
        <Typography variant="body1" paragraph>
          We also offer a range of services, including tool rentals, battery
          testing, and oil recycling. Our goal is to make your shopping
          experience as seamless and rewarding as possible. Visit us today and
          discover why GearUp Auto Parts is the preferred choice for automotive
          parts and accessories!
        </Typography>
      </Box>
    </Container>
  );
}
