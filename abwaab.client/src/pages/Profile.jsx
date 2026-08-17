import { Circle } from "@mui/icons-material";
import React, { useEffect, useState } from "react";
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
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useSnackbar } from "notistack";
import ChangeEmail from "../components/profile/ChangeEmail";
import StyledSwitch from "../components/StyledSwitch";
import ChangePhnoeNo from "../components/profile/ChangePhnoeNo";
import ChangePassword from "../components/profile/ChancePassword";
import VerifyChange from "../components/profile/VerifyChange";

const profileData = {
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
  smsNotificationStaus: false,
  pendingChanges: "لايوجد تغييرات معلقة",
};

const AccountInfoItem = ({ title, info, action, verification }) => {
  return (
    <div className="w-full border-b border-neutral-300 py-4 flex items-center last:border-b-0 justify-between">
      {/* title and info */}
      <div className="flex-1 max-w-[68%]">
        <h4 className="font-semibold text-sm text-black">{title}</h4>
        <p
          className="text-base text-neutral-700 text-right"
          style={{ direction: "initial" }}
        >
          {info}
        </p>
      </div>
      {/* case if only action provided */}
      {action && !verification && <div className="">{action}</div>}
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
  const [changes, setChanges] = useState({
    email: { show: false, action: "" },
    phoneNo: { show: false, action: "" },
    password: { show: false },
    verification: { show: true, type: "email" },
    code: "",
    currentPassword: "",
    newIdentifier: "",
    // change: { action: "", type: "" },
    // newEmail: "",
    // newPhoneNo: "",
    // code: "",
    // newPassword: "",
    // confirmPassword: "",
  });
  const axiosPrivate = useAxiosPrivate();
  const { enqueueSnackbar } = useSnackbar();

  useEffect(() => {
    const controller = new AbortController();
    const fetchData = async () => {
      await axiosPrivate
        .get(`/profile/getdata`)
        .then((resp) => {
          setProfile(resp.data);
          console.log(resp.data);
        })
        .catch((err) => {
          console.log(err);
        });
    };
    // fetchData()
    return () => {
      controller.abort;
    };
  }, []);
  const changeNotificationWay = async (e, channel) => {
    var api = "";
    var msg = "";
    var value = e.target.checked;
    switch (value) {
      case true:
        api = "profile/SubscribeNotificationWay";
        msg = `تم تفعيل إشعارات ${
          channel === "email" ? "البريد الإلكتروني" : "الرسائل النصية"
        } بنجاح!`;
        break;
      case false:
        api = "profile/UnsubscribeNotificationWay";
        msg = `تم الغاء تفعيل إشعارات ${
          channel === "email" ? "البريد الإلكتروني" : "الرسائل النصية"
        } بنجاح!`;
        break;
      default:
        break;
    }
    await axiosPrivate
      .post(api, { notifiactionWayId: profileData.notifiactionWayId })
      .then((resp) => {
        setProfile({ ...profile, [e.target.name]: value });
        enqueueSnackbar(msg, { variant: "success" });
      })
      .catch((err) => {
        enqueueSnackbar(err, { variant: "error" });
      });
  };
  const modifyEmail = async (e) => {
    e.preventDefault();
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    await axiosPrivate
      .post(`profile/initiate-email-change`, { ...data })
      .then((resp) => {
        setChanges({
          ...changes,
          verification: { show: true, type: "email" },
          newIdentifier: data.newEmail,
        });
      })
      .catch((err) => {});
  };
  const modifyPhoneNo = async (e) => {
    e.preventDefault();
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    await axiosPrivate
      .post(`profile/initiate-phone-change`, { ...data })
      .then((resp) => {
        setChanges({
          ...changes,
          verification: { show: true, type: "phone" },
          newIdentifier: data.newPhoneNo,
        });
      })
      .catch((err) => {});
  };
  const modifyPassword = async (e) => {
    e.preventDefault();
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    await axiosPrivate
      .post(`profile/ChangePassword`, { ...data })
      .then((resp) => {})
      .catch((err) => {});
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
                <p className="text-teal-500 font-semibold text-2xl">{`${profileData.firstName[0]} ${profileData.lastName[0]}`}</p>
              </div>
              <div className="">
                <h3 className="text-navy-800 font-semibold text-2xl leading-9">
                  {`${profileData.firstName} ${profileData.lastName}`}
                </h3>
                <p
                  className="text-neutral-800 text-base leading-6 text-end"
                  style={{ direction: "ltr" }}
                >
                  {profileData.identifier}
                </p>
                <VerificationStatus
                  key={"veri-1"}
                  isVerified={profileData.accountIsVerified}
                  label={
                    profileData.accountIsVerified
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
                <h3 className="font-semibold text-navy-800">معلومات الحساب</h3>
                <AccountInfoItem
                  key={"itm-1"}
                  title={"الإسم الأول"}
                  info={profileData.firstName}
                />
                <AccountInfoItem
                  key={"itm-2"}
                  title={"الإسم الاخير"}
                  info={profileData.lastName}
                />
                <AccountInfoItem
                  key={"itm-3"}
                  title={"البريد الإلكتروني"}
                  info={
                    profileData.email
                      ? profileData.email
                      : "لم يتم اضافة بريد إلكتروني"
                  }
                  action={
                    profileData.email ? (
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
                    profileData.email ? (
                      <VerificationStatus
                        key={"veri-2"}
                        isVerified={profileData.emailIsVerified}
                        label={
                          profileData.emailIsVerified ? "موثّق" : "غير موثّق"
                        }
                      />
                    ) : null
                  }
                />
                <AccountInfoItem
                  key={"itm-4"}
                  title={"رقم الموبايل"}
                  info={
                    profileData.mobileNumber ? (
                      <span style={{ direction: "ltr" }}>
                        {profileData.mobileNumber}
                      </span>
                    ) : (
                      "لم يتم اضافة رقم موبايل"
                    )
                  }
                  action={
                    profileData.mobileNumber ? (
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
                    profileData.mobileNumber ? (
                      <VerificationStatus
                        key={"veri-2"}
                        isVerified={profileData.mobileIsVerified}
                        label={
                          profileData.mobileIsVerified ? "موثّق" : "غير موثّق"
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
                  info={profileData.passwordLastModify}
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
                    profileData.emailNotificationStatus
                      ? "مفعّلة"
                      : "غير مفعّلة"
                  }
                  action={
                    // <Switch
                    // sx={{".MuiSwitch-track":{bac}}}
                    //   size="medium"
                    //   checked={profileData.emailNotificationStatus}
                    //   onChange={(e) => changeNotificationWay(e, "eamil")}
                    // />
                    <StyledSwitch
                      size="medium"
                      name={"emailNotificationStatus"}
                      checked={profileData.emailNotificationStatus}
                      onChange={(e) => changeNotificationWay(e, "eamil")}
                    />
                  }
                />
                <AccountInfoItem
                  key={"itm-7"}
                  title={"إشعارات الرسائل النصية"}
                  info={
                    profileData.smsNotificationStaus ? "مفعّلة" : "غير مفعّلة"
                  }
                  action={
                    <StyledSwitch
                      size="medium"
                      name={"smsNotificationStatus"}
                      checked={profileData.smsNotificationStaus}
                      onChange={(e) => changeNotificationWay(e, "sms")}
                    />
                    // <IosSwitch
                    //   size="medium"
                    //   checked={profileData.smsNotificationStaus}
                    //   onChange={(e) => changeNotificationWay(e, "sms")}
                    // />
                  }
                />
                <AccountInfoItem
                  key={"itm-8"}
                  title={"تغييرات معلقة"}
                  info={profileData.pendingChanges}
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
      />
      <ChangePassword
        open={changes.password.show}
        handleClose={() =>
          setChanges({ ...changes, password: { show: false } })
        }
        handleSubmit={modifyPassword}
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
};

export default Profile;
