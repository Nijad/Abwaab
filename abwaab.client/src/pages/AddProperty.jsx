import React from "react";

const AddProperty = () => {
  return (
    <div className="flex flex-col px-28 py-5">
      <div className="my-3">
        <h4 className="py-3 font-semibold text-[32px] leading-10 text-navy-700">
          إضافة عقار جديد
        </h4>
        <p className="py-2 text-neutral-700 text-lg">
          أدخل البيانات الأساسية، حدد الموقع، ثم اضف صور العقار أو الفيديو من
          مكان واحد
        </p>
      </div>
      <div className="flex flex-col rounded-3xl bg-white p-3 flex-1">
        <h6 className="">Basic Information</h6>
        <div className="flex items-center content-start gap-3">
          <div className="w-1/4">Image</div>
          <div className="">Info</div>
        </div>
      </div>
    </div>
  );
};

export default AddProperty;
