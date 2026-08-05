import React, { useState } from "react";
import axios from "../services/axios";
import { Button } from "@mui/material";
import useAuth from "../hooks/useAuth";
import { useNavigate } from "react-router";

const Login = () => {
  // const axiosPrivate = useAxiosPrivate();
  const { login } = useAuth();
  const navigate = useNavigate();
  const [fdata, setFdata] = useState({ identifier: "", password: "" });
  const handleSubmit = async (e) => {
    e.preventDefault();
    const controller = new AbortController();
    var isAdmin;
    await axios
      .post(
        "/auth/loginuser",
        { ...fdata },
        {
          signal: controller.signal,
        }
      )
      .then((resp) => {
        isAdmin = resp.data.isAdmin;
        login(resp.data);
      })
      .catch((e) => console.log(e))
      .finally(() => {
        if (isAdmin) redirect("Admin");
        else redirect("");
      });

    // return <Navigate to="/" replace />;
    // const token = parseJwt(response.data.accessToken);
    // console.log(token);

    // const response = await fetch(`${import.meta.}/api/auth/login` {
    //   body: JSON.stringify({ ...fdata }),
    //   headers: {
    //     "Content-Type": "Application/json",
    //   },
    //   mode: "cors",
    //   method: "Post",
    //   signal: controller.signal,
    // });
    // console.log(response);
  };
  console.log(fdata);
  const redirect = (to) => {
    navigate(`/${to}`);
  };

  return (
    <div>
      <form method="post" onSubmit={handleSubmit}>
        <div className="">
          <label>Email / Mobile No.</label>
          <input
            type="text"
            name="Identifier"
            value={fdata.identifier}
            onChange={(e) => setFdata({ ...fdata, identifier: e.target.value })}
          />
        </div>
        <div className="">
          <label>Password</label>
          <input
            type="password"
            name="Password"
            value={fdata.password}
            onChange={(e) => setFdata({ ...fdata, password: e.target.value })}
          />
        </div>
        {/* <input type="submit" /> */}
        <Button type="submit" variant="outlined" color="secondary">
          Submit
        </Button>
      </form>
    </div>
  );
};

export default Login;
