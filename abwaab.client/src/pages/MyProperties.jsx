import { Button } from "@mui/material";
import React from "react";
import { Outlet, useNavigate } from "react-router";

const MyProperties = () => {
  const navigate = useNavigate();
  return (
    <div>
      <Button onClick={() => navigate("add")}>Add property</Button>
    </div>
  );
};

export default MyProperties;
