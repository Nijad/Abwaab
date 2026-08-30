import HomeIcon from "../HomeIcon";

const PropertyNotFound = () => {
  return (
    <div
      id="prop-area"
      className="flex items-center justify-center w-full flex-1"
    >
      <div className="rounded-2xl border-neutral-400 bg-white text-center p-10">
        <HomeIcon />
        <h5 className="text-xl font-semibold text-navy-700 p-4 my-2">
          العقار المطلوب غير موجود!
        </h5>
      </div>
    </div>
  );
};

export default PropertyNotFound;
