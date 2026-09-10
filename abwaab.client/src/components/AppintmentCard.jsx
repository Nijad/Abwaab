import { LocationOnOutlined } from "@mui/icons-material";
import { dayDT } from "../dataTypes/appointments";
import StraightenIcon from "@mui/icons-material/Straighten";
import { formatDateWithDayAr } from "../utils/helpers";
import { Button } from "@mui/material";
import LabelTag from "./LabelTag";

const AppointmentCard = ({ day, type, onAccept, onReject, onCancel }) => {
  return (
    <div className="w-full mx-auto p-0">
      <h5 className="">{formatDateWithDayAr(day?.appointmentDate)}</h5>
      {day &&
        day.appointments.map((t) => (
          <div key={t.appointmentId} className="  flex items-center gap-5 my-3">
            {/* Time - Far Right */}
            <div className="text-gray-700 font-medium text-sm whitespace-nowrap">
              {/* <p className="text-center text-neutral-600">from</p> */}
              <p className="">{t.fromTime}</p>
              <p className="text-center text-neutral-600">-</p>
              <p className="">{t.endTime}</p>
            </div>
            <div className="bg-white border border-neutral-200 rounded-xl w-full p-5 flex gap-5">
              {/* Property Image Thumbnail */}
              <div className="w-20 h-20 rounded-lg overflow-hidden flex-shrink-0">
                <img
                  src={`${import.meta.env.VITE_API_BASE_URL}${t?.coverPath}`}
                  alt={t.propertyTitle}
                  className="w-full h-full object-cover"
                />
              </div>

              {/* Property Details - Center */}
              <div className="flex-1 space-y-2">
                <h3 className="font-bold text-navy-800 text-lg">
                  {t.propertyTitle}
                </h3>
                <div className="flex items-center gap-4 text-sm text-gray-500">
                  {/* Location */}
                  <div className="flex items-center gap-1">
                    <LocationOnOutlined />
                    <span>{t.address}</span>
                  </div>

                  {/* Area */}
                  <div className="flex items-center gap-1">
                    <StraightenIcon />
                    <span>{t.area} م²</span>
                  </div>
                </div>
                <LabelTag
                  className="rounded-2xl bg-navy-300 px-3"
                  label={t.arbicAppointmentState}
                  key={t.appointmentId}
                />
              </div>

              {/* Price & Actions - Far Left */}
              <div className="flex flex-col items-end gap-2 whitespace-nowrap">
                <div className="font-bold text-navy-800 text-lg">
                  {t.price?.toLocaleString()} دولار امريكي
                </div>
                <div className="flex gap-3">
                  {type === "received" && t.appointmentState === "Pending" && (
                    <>
                      <Button
                        variant="contained"
                        color="navy"
                        onClick={() => onAccept && onAccept(t.appointmentId)}
                      >
                        قبول الموعد
                      </Button>
                      <Button
                        variant="outlined"
                        color="navy"
                        disabled={!t.cancelable}
                        onClick={() => onReject && onReject(t.appointmentId)}
                      >
                        رفض الموعد
                      </Button>
                    </>
                  )}
                  {type === "received" && t.appointmentState === "Approved" && (
                    <>
                      <Button
                        variant="contained"
                        color="navy"
                        onClick={() => onCancel && onCancel(t.appointmentId)}
                      >
                        الغاء الموعد
                      </Button>
                      <div className="">
                        {!t.cancelable && (
                          <p className="text-sm text-neutral-500 ">
                            لايمكن الغاء الموعد الآن
                          </p>
                        )}
                      </div>
                    </>
                  )}
                  {(type === "received" && t.appointmentState === "Approved") ||
                    (type === "requested" && (
                      <>
                        <Button
                          variant="contained"
                          color="navy"
                          onClick={() => onCancel && onCancel(t.appointmentId)}
                        >
                          الغاء الموعد
                        </Button>
                        <div className="">
                          {!t.cancelable && (
                            <p className="text-sm text-neutral-500 ">
                              لايمكن الغاء الموعد الآن
                            </p>
                          )}
                        </div>
                      </>
                    ))}
                </div>
              </div>
            </div>
          </div>
        ))}
    </div>
  );
};

export default AppointmentCard;
