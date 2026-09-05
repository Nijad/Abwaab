import { LocationOnOutlined } from "@mui/icons-material";
import { dayDT } from "../dataTypes/appointments";
import StraightenIcon from "@mui/icons-material/Straighten";
import { formatDateWithDayAr } from "../utils/helpers";
import { Button } from "@mui/material";

const AppointmentCard = ({ day = { ...dayDT }, approveButton }) => {
  return (
    <div className="w-full mx-auto p-0">
      <h5 className="">{day.appointmentDate}</h5>
      {day.appointments.map((t) => (
        <div
          key={t.appointmentId}
          className="  flex items-center gap-5 shadow-sm my-3"
        >
          {/* Time - Far Right */}
          <div className="text-gray-700 font-medium text-sm whitespace-nowrap">
            {t.fromTime}
          </div>
          <div className="bg-white border border-neutral-200 rounded-xl w-full p-5 flex gap-5">
            {/* Property Image Thumbnail */}
            <div className="w-20 h-20 rounded-lg overflow-hidden flex-shrink-0">
              <img
                src={t.coverPath}
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
            </div>

            {/* Price & Actions - Far Left */}
            <div className="flex flex-col items-end gap-2 whitespace-nowrap">
              <div className="font-bold text-navy-800 text-lg">
                {t.price} ليرة سورية
              </div>
              <div className="flex gap-3">
                {approveButton && (
                  <Button variant="contained" color="navy" className="">
                    قبول الموعد
                  </Button>
                )}
                <Button
                  variant="outlined"
                  color="navy"
                  className=""
                  disabled={!t.cancelable}
                >
                  إلغاء الموعد
                </Button>
                {!t.cancelable && (
                  <p className="text-sm text-neutral-500">
                    لايمكن الغاء الموعد الآن
                  </p>
                )}
              </div>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default AppointmentCard;
