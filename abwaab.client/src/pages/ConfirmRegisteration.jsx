import { Button, TextField } from "@mui/material";
import React, { useEffect, useState } from "react";
import useAuth from "../hooks/useAuth";
import CountdownTimer from "../components/CountDownTimer";

const ConfirmRegisteration = () => {
  const { user, codeRemainingSeconds, setRemainingSeconds } = useAuth();
  const [code, setCode] = useState(null);
  console.log(user);

  useEffect(() => {
    //check localstorage or sessionstorage for expiry time
    const intval = setInterval(() => {
      if (codeRemainingSeconds > 0) setRemainingSeconds((prev) => prev - 1);
    }, 1000);
    return () => {
      clearInterval(intval);
    };
  }, []);

  // check in auth context first then in session storage for code expiration date
  return (
    <div>
      {codeRemainingSeconds === 0 && <p>انتهت صلاحية الرمز الخاص بك!</p>}
      {codeRemainingSeconds > 0 && (
        <div>
          {/* <p>{codeRemainingSeconds}</p> */}
          <CountdownTimer
            initialSeconds={codeRemainingSeconds}
            onComplete={() => null}
            autoStart={true}
          />
          <TextField
            label="الرمز الخاص"
            size="medium"
            variant="standard"
            name="Code"
            value={code}
            onChange={(e) => setCode(e.target.value)}
          />
        </div>
      )}
      {codeRemainingSeconds == 0 && (
        <div className="">
          <Button type="button" variant="text" size="small">
            إعادة ارسال الرمز الخاص
          </Button>
        </div>
      )}
    </div>
  );
};

export default ConfirmRegisteration;
