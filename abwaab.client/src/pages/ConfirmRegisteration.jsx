import { Button, TextField } from "@mui/material";
import React, { useEffect, useRef, useState } from "react";
import useAuth from "../hooks/useAuth";
import CountdownTimer from "../components/CountDownTimer";
import axios from "../services/axios";
import { useNavigate } from "react-router";
import { useSnackbar } from "notistack";

const ConfirmRegisteration = () => {
  const { user, codeRemainingSeconds, setRemainingSeconds, login } = useAuth();
  const [fdata, setFdata] = useState({
    Identifier: user.identifier,
    Code: "",
  });
  console.log(fdata);
  const navigate = useNavigate();
  const { enqueueSnackbar } = useSnackbar();

  const resendCode = async () => {
    try {
      await axios
        .post("/auth/resendcode", { Identifier: user.identifier })
        .then((resp) => {
          setRemainingSeconds(resp.data.codeTimeOutInMinuts * 60);
          enqueueSnackbar(resp.data.message, { variant: "success" });
          console.log(resp.data);
        })
        .catch((err) => {});
    } catch (error) {}
  };
  const handleFormSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios
        .post("/auth/verifyaccount", { ...fdata })
        .then((resp) => {
          console.log(resp.data);
          // login(resp.data);
          navigate("/", { replace: true });
          enqueueSnackbar("الرمز المدخل صحيح", { variant: "success" });
        })
        .catch((err) => {});
    } catch (error) {}
  };

  const handleCountdownEnd = () => {
    setRemainingSeconds(0);
  };

  // check in auth context first then in session storage for code expiration date
  return (
    <div>
      {codeRemainingSeconds === 0 && <p>انتهت صلاحية الرمز الخاص بك!</p>}
      {codeRemainingSeconds > 0 && (
        <div>
          {/* <p>{codeRemainingSeconds}</p> */}
          <CountdownTimer
            initialSeconds={codeRemainingSeconds}
            onComplete={() => handleCountdownEnd()}
            autoStart={true}
          />
          <form method="post" onSubmit={(e) => handleFormSubmit(e)}>
            <TextField
              label="الرمز الخاص"
              size="medium"
              variant="standard"
              name="Code"
              value={fdata.Code}
              onChange={(e) =>
                setFdata({ ...fdata, [e.target.name]: e.target.value })
              }
            />
          </form>
        </div>
      )}
      {codeRemainingSeconds == 0 && (
        <div className="">
          <Button
            type="button"
            variant="text"
            size="small"
            onClick={() => resendCode()}
          >
            إعادة ارسال الرمز الخاص
          </Button>
        </div>
      )}
    </div>
  );
};

export default ConfirmRegisteration;
