import React from "react";
import MyAppointmnets from "../features/MyAppointments";

const Appointments = () => {
  return (
    <div className="flex flex-col items-start py-8 px-28 w-full max-w-7xl mx-auto">
      <h3 className="text-[32px] text-navy-800 font-semibold">مواعيدي</h3>
      <p className="text-base text-neutral-600 ">
        يمكنك معاينة المواعيد التي قمت بحجزها لمعاينة عقارات اخرى، كما يمكنك
        معاينة المواعيد المطلوبة لمعاينة عقاراتك
      </p>
      <MyAppointmnets />
    </div>
  );
};

export default Appointments;
