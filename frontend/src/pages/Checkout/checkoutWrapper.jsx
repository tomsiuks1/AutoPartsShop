import { Elements } from "@stripe/react-stripe-js";
import CheckoutPage from ".";
import { loadStripe } from "@stripe/stripe-js";
import { useState, useEffect } from "react";
import agent from "../../app/api/agent";
import LoadingComponent from "../../components/LoadingIndicator";
import { useAppDispatch } from "../../app/store/configureStore";
import { setBasket } from "../../app/store/slices/basketSlice";

const stripePromise = loadStripe(
  "pk_test_51PI4b8EAw4YuhxY8fY4VyBihLwI84BSVlp0eSNG90tFx9zUl8MQCRSQaws4f1tmCyP6cSQcVFaR5ddgNFq08xA5b00GUOnD1fg"
);

export default function CheckoutWrapper() {
  const dispatch = useAppDispatch();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    agent.Payments.createPaymentIntent()
      .then((response) => dispatch(setBasket(response)))
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  }, [dispatch]);

  if (loading) return <LoadingComponent message="Loading checkout" />;

  return (
    <Elements stripe={stripePromise}>
      <CheckoutPage />
    </Elements>
  );
}
