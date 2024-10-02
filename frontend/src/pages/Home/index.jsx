import { useState, useEffect } from "react";
import { Typography } from "@mui/material";
import agent from "../../app/api/agent";

export default function HomePage() {
  const [makers, setMakers] = useState();

  const get = async () => {
    const response = await agent.Catalog.getCarMakers();
    setMakers(response);

    console.log(response);
  };

  // useEffect(() => {
  //   get();
  // }, []);
  
  return (
    <div>
      {/* {makers &&
        makers.map((x) => <Typography key={x.name}>{x.name}</Typography>)} */}
    </div>
  );
}
