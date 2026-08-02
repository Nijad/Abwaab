import React, { useState } from "react";
import axios from "../services/axios";

const Login = () => {
  // const axiosPrivate = useAxiosPrivate();
  const [fdata, setFdata] = useState({ identifier: "", password: "" });
  const handleSubmit = async (e) => {
    e.preventDefault();
    const controller = new AbortController();
    const response = await axios.post("/auth/loginuser", {
      data: { ...fdata },
      signal: controller.signal,
    });
    // const response = await fetch(`${import.meta.}/api/auth/login` {
    //   body: JSON.stringify({ ...fdata }),
    //   headers: {
    //     "Content-Type": "Application/json",
    //   },
    //   mode: "cors",
    //   method: "Post",
    //   signal: controller.signal,
    // });
    console.log(response);
  };
  console.log(fdata);

  return (
    <div>
      <form method="post" onSubmit={handleSubmit}>
        <div className="">
          <label>Email / Mobile No.</label>
          <input
            type="text"
            name="identifier"
            value={fdata.identifier}
            onChange={(e) =>
              setFdata({ ...fdata, [e.target.name]: e.target.value })
            }
          />
        </div>
        <div className="">
          <label>Password</label>
          <input
            type="password"
            name="password"
            value={fdata.password}
            onChange={(e) =>
              setFdata({ ...fdata, [e.target.name]: e.target.value })
            }
          />
        </div>
        <input type="submit" />
      </form>
    </div>
  );
};

export default Login;
