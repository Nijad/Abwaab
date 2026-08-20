import { Circle } from "@mui/icons-material";
import React, { useEffect, useRef, useState } from "react";
import VerificationStatus from "../components/VerificationStatus";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Switch,
  TextField,
} from "@mui/material";
import { useSnackbar } from "notistack";
import ChangeEmail from "../features/profile/ChangeEmail";
import StyledSwitch from "../components/StyledSwitch";
import ChangePhnoeNo from "../features/profile/ChangePhnoeNo";
import ChangePassword from "../features/profile/ChancePassword";
import VerifyChange from "../features/profile/VerifyChange";
import { authApi, profileApi } from "../api";

const profile = {
  firstName: "حسام",
  lastName: "حبال",
  identifier: "+963933558117",
  accountIsVerified: true,
  email: "sdfasdfsd",
  emailIsVerified: true,
  mobileNumber: "+963933558117",
  mobileIsVerified: false,
  passwordLastModify: "اخر تعديل منذ 3 اشهر",
  emailNotificationStatus: true,
  smsNotificationStatus: false,
  pendingChanges: "لايوجد تغييرات معلقة",
};

const AccountInfoItem = ({ title, info, action, verification }) => {
  return (
    <div className="w-full border-b border-neutral-300 py-4 flex items-center last:border-b-0 justify-between">
      {/* title and info */}
      <div className={`flex-1`}>
        <h4 className="font-semibold text-sm text-black">{title}</h4>
        <p
          className="text-base text-neutral-700 text-right"
          style={{ direction: "initial" }}
        >
          {info}
        </p>
      </div>
      {/* case if only action provided */}
      {action && !verification && <div className="w-[32%]">{action}</div>}
      {/* case if action and verification provided */}
      {action && verification && (
        <div className="flex w-[32%]">
          <div className="">{action}</div>
          <div className="">{verification}</div>
        </div>
      )}
    </div>
  );
};

const Profile = () => {
  const [profile, setProfile] = useState(null);
  const [notificationWays, setNotificationWays] = useState(null);
  const [changes, setChanges] = useState({
    email: { show: false, action: "", errors: null },
    phoneNo: { show: false, action: "", errors: null },
    password: { show: false, errors: null },
    verification: { show: false, type: "email" },
    code: "",
    currentPassword: "",
    newIdentifier: "",
  });
  const { enqueueSnackbar } = useSnackbar();
  const signalRef = useRef();
  console.log(notificationWays);

  useEffect(() => {
    if (signalRef.current) {
      signalRef.current.abort();
    }
    signalRef.current = new AbortController();
    const fetchData = async () => {
      try {
        const response = await profileApi.getProfileData(
          signalRef.current.signal
        );
        const notifiWays = await profileApi.getNotificationWays(
          signalRef.current.signal
        );
        setProfile(response.data);
        setNotificationWays(notifiWays.data);
        console.log(response.data);
      } catch (error) {
        console.log(error);
      }
    };
    //   await axiosPrivate
    //     .get(`/profile/getdata`)
    //     .then((resp) => {
    //       setProfile(resp.data);
    //       console.log(resp.data);
    //     })
    //     .catch((err) => {
    //       console.log(err);
    //     });
    // };
    fetchData();
    return () => {
      signalRef.current.abort();
    };
  }, []);
  const changeNotificationWay = async (e, channel) => {
    var value = e.target.checked;
    const way =
      channel == "email"
        ? "بريد الكتروني"
        : channel == "sms"
        ? "رسائل قصيرة"
        : "";
    // debugger;
    const id = notificationWays.find((a) => a.wayName == way).id;
    if (signalRef.current) {
      signalRef.current.abort();
    }
    signalRef.current = new AbortController();
    switch (value) {
      case true:
        try {
          {
            const resp = await profileApi.subscribeNotificationWay(
              id,
              signalRef.current.signal
            );
            setProfile({ ...profile, [e.target.name]: value });
            enqueueSnackbar(resp.data.message, { variant: "success" });
          }
        } catch (error) {
          enqueueSnackbar(error.detail, { variant: "error" });
          console.log(error);
        }
        break;
      case false:
        try {
          const resp = await profileApi.unsubscribeNotificationWay(
            notificationWays["بريد الكتروني"],
            signalRef.current.signal
          );
          setProfile({ ...profile, [e.target.name]: value });
          enqueueSnackbar(resp.data.message, { variant: "success" });
        } catch (error) {
          enqueueSnackbar(error.detail, { variant: "error" });
          console.log(error);
        }
        break;
      default:
        break;
    }
  };
  const modifyEmail = async (e) => {
    e.preventDefault();
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await profileApi.initiateEmailChange(
        ...Object.values(data),
        signalRef.current.signal
      );
      setChanges({
        ...changes,
        email: { show: false, action: "" },
        verification: { show: true, type: "email" },
        newIdentifier: data.newEmail,
      });
      enqueueSnackbar(resp.data.message, { variant: "success" });
    } catch (error) {
      if (Object.hasOwn(error, "errorCode")) {
        enqueueSnackbar(error.detail, { variant: "error" });
      }
      setChanges({
        ...changes,
        email: { ...changes.email, errors: error.errors },
      });
      enqueueSnackbar(error.response.data.message, { variant: "error" });
    }
  };
  const modifyPhoneNo = async (e) => {
    e.preventDefault();
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await profileApi.initiatePhoneChange(
        ...Object.values(data),
        signalRef.current.signal
      );
      setChanges({
        ...changes,
        phoneNo: { show: false, action: "" },
        verification: { show: true, type: "phone" },
        newIdentifier: data.newEmail,
      });
      enqueueSnackbar(resp.data.message, { variant: "success" });
    } catch (error) {
      if (Object.hasOwn(error, "errorCode")) {
        enqueueSnackbar(error.detail, { variant: "error" });
      }
      setChanges({
        ...changes,
        phoneNo: { ...changes.phoneNo, errors: error.errors },
      });
      // enqueueSnackbar(error.response.data.message, { variant: "error" });
    }
  };
  const modifyPassword = async (e) => {
    e.preventDefault();
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await profileApi.changePassword(
        ...Object.values(data),
        signalRef.current.signal
      );
      setChanges({
        ...changes,
        password: { show: false },
        verification: { show: true, type: "password" },
        newIdentifier: data.newEmail,
      });
      enqueueSnackbar(resp.data.message, { variant: "success" });
    } catch (error) {
      if (Object.hasOwn(error, "errorCode")) {
        enqueueSnackbar(error.detail, { variant: "error" });
      }
      enqueueSnackbar(error.detail, { variant: "error" });
      setChanges({
        ...changes,
        password: { ...changes.password, errors: error.errors },
      });
      // enqueueSnackbar(error.response.data.message, { variant: "error" });
    }
  };
  const cancelChange = async () => {
    await axiosPrivate
      .post(`auth/VerifyAccount`, {
        identifier: changes.newIdentifier,
        code: code,
      })
      .then((resp) => {
        console.log(resp.data);
      })
      .catch((err) => {});
    console.log("canceled");
  };
  const verfiyChangeHandler = async (code) => {
    await axiosPrivate
      .post(`auth/VerifyAccount`, {
        identifier: changes.newIdentifier,
        code: code,
      })
      .then((resp) => {
        console.log(resp.data);
      })
      .catch((err) => {});
    console.log("canceled");
  };
  // if (!profile) {
  //   return <div>Loading</div>;
  // }
  if (profile) {
    return (
      <div className="flex flex-col md:px-28 w-full max-w-[1536px] mx-auto">
        <div className="mt-7">
          <h3 className="text-[32px] text-navy-700 leading-10 font-semibold mb-2">
            الملف الشخصي
          </h3>
          <p className="text-base leading-6 text-neutral-900">
            إدارة معلومات حسابك ووسائل التواصل والأمان
          </p>
        </div>
        <div className="flex-1 rounded-[20px] border border-1 border-neutral-300 my-7">
          <div className="flex flex-col p-0">
            <div className="border-b border-neutral-300 p-7">
              <div className="flex gap-5">
                <div className="w-[88px] h-[88px] rounded-full bg-teal-100 flex items-center justify-center">
                  <p className="text-teal-500 font-semibold text-2xl">{`${profile.firstName[0]} ${profile.lastName[0]}`}</p>
                </div>
                <div className="">
                  <h3 className="text-navy-800 font-semibold text-2xl leading-9">
                    {`${profile.firstName} ${profile.lastName}`}
                  </h3>
                  <p
                    className="text-neutral-800 text-base leading-6 text-end"
                    style={{ direction: "ltr" }}
                  >
                    {profile.identifier}
                  </p>
                  <VerificationStatus
                    key={"veri-1"}
                    isVerified={profile.accountIsVerified}
                    label={
                      profile.accountIsVerified
                        ? "الحساب موثّق"
                        : "الحساب غير مكتمل"
                    }
                  />
                </div>
              </div>
            </div>
            <div className="flex-1 p-7 pb-14">
              <div className="flex gap-14">
                <div className="w-1/2 ">
                  <h3 className="font-semibold text-navy-800">
                    معلومات الحساب
                  </h3>
                  <AccountInfoItem
                    key={"itm-1"}
                    title={"الإسم الأول"}
                    info={profile.firstName}
                  />
                  <AccountInfoItem
                    key={"itm-2"}
                    title={"الإسم الاخير"}
                    info={profile.lastName}
                  />
                  <AccountInfoItem
                    key={"itm-3"}
                    title={"البريد الإلكتروني"}
                    info={
                      profile.email
                        ? profile.email
                        : "لم يتم اضافة بريد إلكتروني"
                    }
                    action={
                      profile.email ? (
                        <Button
                          size="small"
                          variant="text"
                          color="inherit"
                          sx={{ marginX: 1 }}
                          onClick={() =>
                            setChanges({
                              ...changes,
                              email: { show: true, action: "edit" },
                            })
                          }
                        >
                          تغيير
                        </Button>
                      ) : (
                        <Button
                          size="small"
                          variant="text"
                          color="inherit"
                          sx={{ marginX: 1 }}
                          onClick={() =>
                            setChanges({
                              ...changes,
                              email: { show: true, action: "add" },
                            })
                          }
                        >
                          اضافة
                        </Button>
                      )
                    }
                    verification={
                      profile.email ? (
                        <VerificationStatus
                          key={"veri-2"}
                          isVerified={profile.emailIsVerified}
                          label={
                            profile.emailIsVerified ? "موثّق" : "غير موثّق"
                          }
                        />
                      ) : null
                    }
                  />
                  <AccountInfoItem
                    key={"itm-4"}
                    title={"رقم الموبايل"}
                    info={
                      profile.mobileNumber ? (
                        <span style={{ direction: "ltr" }}>
                          {profile.mobileNumber}
                        </span>
                      ) : (
                        "لم يتم اضافة رقم موبايل"
                      )
                    }
                    action={
                      profile.mobileNumber ? (
                        <Button
                          size="small"
                          variant="text"
                          color="inherit"
                          sx={{ marginX: 1 }}
                          onClick={() =>
                            setChanges({
                              ...changes,
                              phoneNo: { show: true, action: "edit" },
                            })
                          }
                        >
                          تغيير
                        </Button>
                      ) : (
                        <Button
                          size="small"
                          variant="text"
                          color="inherit"
                          sx={{ marginX: 1 }}
                          onClick={() =>
                            setChanges({
                              ...changes,
                              phoneNo: { show: true, action: "add" },
                            })
                          }
                        >
                          اضافة
                        </Button>
                      )
                    }
                    verification={
                      profile.mobileNumber ? (
                        <VerificationStatus
                          key={"veri-2"}
                          isVerified={profile.mobileIsVerified}
                          label={
                            profile.mobileIsVerified ? "موثّق" : "غير موثّق"
                          }
                        />
                      ) : null
                    }
                  />
                </div>
                <div className="w-1/2 ">
                  <h3 className="font-semibold text-navy-800">
                    الأمان والتفضيلات
                  </h3>
                  <AccountInfoItem
                    key={"itm-5"}
                    title={"كلمة المرور"}
                    info={profile.passwordLastModify}
                    action={
                      <Button
                        size="small"
                        variant="text"
                        color="inherit"
                        sx={{ marginX: 1 }}
                        onClick={() =>
                          setChanges({ ...changes, password: { show: true } })
                        }
                      >
                        تغيير
                      </Button>
                    }
                  />
                  <AccountInfoItem
                    key={"itm-6"}
                    title={"إشعارات البريد"}
                    info={
                      profile.emailNotificationStatus ? "مفعّلة" : "غير مفعّلة"
                    }
                    action={
                      // <Switch
                      // sx={{".MuiSwitch-track":{bac}}}
                      //   size="medium"
                      //   checked={profile.emailNotificationStatus}
                      //   onChange={(e) => changeNotificationWay(e, "eamil")}
                      // />
                      <StyledSwitch
                        size="medium"
                        name={"emailNotificationStatus"}
                        checked={profile.emailNotificationStatus}
                        onChange={(e) => changeNotificationWay(e, "email")}
                      />
                    }
                  />
                  <AccountInfoItem
                    key={"itm-7"}
                    title={"إشعارات الرسائل النصية"}
                    info={
                      profile.smsNotificationStatus ? "مفعّلة" : "غير مفعّلة"
                    }
                    action={
                      <StyledSwitch
                        size="medium"
                        name={"smsNotificationStatus"}
                        checked={profile.smsNotificationStatus}
                        onChange={(e) => changeNotificationWay(e, "sms")}
                      />
                      // <IosSwitch
                      //   size="medium"
                      //   checked={profile.smsNotificationStaus}
                      //   onChange={(e) => changeNotificationWay(e, "sms")}
                      // />
                    }
                  />
                  <AccountInfoItem
                    key={"itm-8"}
                    title={"تغييرات معلقة"}
                    info={profile.pendingChanges}
                  ></AccountInfoItem>
                </div>
              </div>
            </div>
          </div>
        </div>
        <ChangeEmail
          title={`${
            changes.email.action === "add" ? "إضافة" : "تغيير"
          } البريد الإلكتروني`}
          description={
            changes.email.action === "add"
              ? "أدخل البريد الإلكتروني الجديد. لن يتم تأكيد البريد الإلكتروني الجديد حتى يتم التحقق  بنجاح. "
              : "أدخل البريد الإلكتروني الجديد. سيبقى بريدك الحالي فعالًا ولن يتغير إلا بعد نجاح رمز التحقق."
          }
          open={changes.email.show}
          handleClose={() =>
            setChanges({ ...changes, email: { ...changes.email, show: false } })
          }
          handleSubmit={modifyEmail}
          errors={changes.email.errors}
        />
        <ChangePhnoeNo
          title={`${
            changes.phoneNo.action === "add" ? "إضافة" : "تغيير"
          } رقم الموبايل`}
          description={
            changes.phoneNo.action === "add"
              ? "أدخل الرقم الجديد. لن يتم تأكيد الرقم الجديد حتى يتم التحقق. "
              : "أدخل الرقم الجديد. سيبقى رقمك الحالي فعالًا ولن يتغير إلا بعد نجاح رمز التحقق."
          }
          open={changes.phoneNo.show}
          handleClose={() =>
            setChanges({
              ...changes,
              phoneNo: { ...changes.phoneNo, show: false },
            })
          }
          handleSubmit={modifyPhoneNo}
          errors={changes.phoneNo.errors}
          // errors={{ NewPhoneNo: ["message 1", "message 2"] }}
        />
        <ChangePassword
          open={changes.password.show}
          handleClose={() =>
            setChanges({ ...changes, password: { show: false } })
          }
          handleSubmit={modifyPassword}
          errors={changes.password.errors}
        />
        <VerifyChange
          title={`تحقق من ${
            changes.verification.type === "email"
              ? "البريد الإلكتروني الجديد"
              : "رقم الموبايل الجديد"
          }`}
          description={""}
          open={changes.verification.show}
          newIdentifier={changes.newIdentifier}
          handleClose={cancelChange}
          handleSubmit={verfiyChangeHandler}
        />
      </div>
    );
  }
};

export default Profile;
