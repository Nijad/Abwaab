import { useEffect, useRef, useState } from "react";
import VerificationStatus from "../components/VerificationStatus";
import { Button } from "@mui/material";
import { useSnackbar } from "notistack";
import ChangeEmail from "../features/profile/ChangeEmail";
import StyledSwitch from "../components/StyledSwitch";
import ChangePhnoeNo from "../features/profile/ChangePhnoeNo";
import ChangePassword from "../features/profile/ChangePassword";
import VerifyChangeEmail from "../features/profile/VerifyChangeEmail";
import { profileApi } from "../api";
import useAuth from "../hooks/useAuth";
import AccountInfoItem from "../components/AccountInfoItem";
import VerifyChangePhone from "../features/profile/VerifyChangePhone";

const Profile = () => {
  const [profile, setProfile] = useState(null);
  const [notificationWays, setNotificationWays] = useState(null);
  const { logout } = useAuth();
  const [changes, setChanges] = useState({
    email: { show: false, action: "", newEmail: "" },
    phoneNo: { show: false, action: "", newPhoneNo: "" },
    password: { show: false },
    verification: { show: false, type: "" },
  });
  const { enqueueSnackbar } = useSnackbar();
  const signalRef = useRef();

  const fetchData = async () => {
    signalRef.current = new AbortController();
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
  useEffect(() => {
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
            id,
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

  const handleChangeEmail = (data, response) => {
    fetchData();
    setChanges({
      ...changes,
      email: { ...changes.email, show: false, newEmail: data },
      verification: { show: true, type: "email" },
    });
  };
  const handleChangePhone = (data, response) => {
    fetchData();
    setChanges({
      ...changes,
      phoneNo: { ...changes.phoneNo, show: false, newPhoneNo: data },
      verification: { show: true, type: "phone" },
    });
  };
  const handleChangePassword = (data) => {
    logout();
    setChanges({ ...changes, email: { ...changes.email, show: false } });
  };

  const handleVerifyChange = async (data, response) => {
    console.log(data);
  };

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
        {changes.email.show && (
          <ChangeEmail
            title={`${
              changes.email.action === "add" ? "إضافة" : "تغيير"
            } البريد الإلكتروني`}
            description={
              changes.email.action === "add"
                ? "أدخل البريد الإلكتروني الجديد. لن يتم تأكيد البريد الإلكتروني الجديد حتى يتم التحقق  بنجاح. "
                : "أدخل البريد الإلكتروني الجديد. سيبقى بريدك الحالي فعالًا ولن يتغير إلا بعد نجاح رمز التحقق."
            }
            onClose={() =>
              setChanges({
                ...changes,
                email: { ...changes.email, show: false },
              })
            }
            onSuccess={handleChangeEmail}
          />
        )}
        {changes.phoneNo.show && (
          <ChangePhnoeNo
            title={`${
              changes.phoneNo.action === "add" ? "إضافة" : "تغيير"
            } رقم الموبايل`}
            description={
              changes.phoneNo.action === "add"
                ? "أدخل الرقم الجديد. لن يتم تأكيد الرقم الجديد حتى يتم التحقق. "
                : "أدخل الرقم الجديد. سيبقى رقمك الحالي فعالًا ولن يتغير إلا بعد نجاح رمز التحقق."
            }
            onClose={() =>
              setChanges({
                ...changes,
                phoneNo: { ...changes.phoneNo, show: false },
              })
            }
            onSuccess={handleChangePhone}
          />
        )}
        {changes.password.show && (
          <ChangePassword
            onClose={() =>
              setChanges({ ...changes, password: { show: false } })
            }
            onSucces={handleChangePassword}
          />
        )}
        {changes.verification.show && changes.verification.type === "email" && (
          <VerifyChangeEmail
            newEmail={changes.email.newEmail}
            onClose={() =>
              setChanges({
                ...changes,
                verification: { show: false, type: "" },
              })
            }
            onSuccess={handleVerifyChange}
          />
        )}
        {changes.verification.show && changes.verification.type === "phone" && (
          <VerifyChangePhone
            newPhone={changes.phoneNo.newPhoneNo}
            onClose={() =>
              setChanges({
                ...changes,
                verification: { show: false, type: "" },
              })
            }
            onSuccess={handleVerifyChange}
          />
        )}
      </div>
    );
  }
};

export default Profile;
