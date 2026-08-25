import React from "react";
import { useParams } from "react-router";
import EditPropertyById from "../features/properties.jsx/EditPropertyById";

const EditProperty = () => {
  // console.log(id);

  return (
    <div className="flex flex-col max-w-7xl py-5 w-full items-center mx-auto">
      <div className="my-3 w-full">
        <h4 className="py-3 font-semibold text-[32px] leading-10 text-navy-700">
          إضافة عقار جديد
        </h4>
        <p className="py-2 text-neutral-700 text-lg">
          أدخل البيانات الأساسية، حدد الموقع، ثم اضف صور العقار أو الفيديو من
          مكان واحد
        </p>
      </div>
      <EditPropertyById />
    </div>
  );
};

export default EditProperty;
