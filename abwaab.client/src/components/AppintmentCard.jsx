const AppointmentCard = ({ day }) => {
  const propertyImage =
    "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=200&h=200&fit=crop";

  return (
    <div dir="rtl" className="w-full max-w-4xl mx-auto p-4">
      <h5 className="">{day.appointmentDate.toLocaleString()} sadfsdfdsaf</h5>
      {day.appointments.map((t) => (
        <div
          key={t.appointmentId}
          className="bg-white border border-gray-200 rounded-xl p-5 flex items-center gap-5 shadow-sm"
        >
          {/* Time - Far Right */}
          <div className="text-gray-700 font-medium text-sm whitespace-nowrap">
            {t.fromTime}
          </div>

          {/* Property Image Thumbnail */}
          <div className="w-20 h-20 rounded-lg overflow-hidden flex-shrink-0">
            <img
              src={propertyImage}
              alt="شقة عصرية"
              className="w-full h-full object-cover"
            />
          </div>

          {/* Property Details - Center */}
          <div className="flex-1 space-y-2">
            <h3 className="font-bold text-gray-900 text-base">
              شقة عصرية بإطلالة مفتوحة في أبو رمانة
            </h3>
            <div className="flex items-center gap-4 text-sm text-gray-500">
              {/* Location */}
              <div className="flex items-center gap-1">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  className="w-4 h-4 text-gray-400"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                  strokeWidth={2}
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"
                  />
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"
                  />
                </svg>
                <span>دمشق – أبو رمانة</span>
              </div>

              {/* Area */}
              <div className="flex items-center gap-1">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  className="w-4 h-4 text-gray-400"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                  strokeWidth={2}
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M4 8V4m0 0h4M4 4l5 5m11-1V4m0 0h-4m4 0l-5 5M4 16v4m0 0h4m-4 0l5-5m11 5l-5-5m5 5v-4m0 4h-4"
                  />
                </svg>
                <span>180 م²</span>
              </div>
            </div>
          </div>

          {/* Price & Actions - Far Left */}
          <div className="flex flex-col items-end gap-2 whitespace-nowrap">
            <div className="font-bold text-gray-900 text-lg">245,000 دولار</div>
            <button className="border border-gray-300 text-gray-600 px-4 py-1.5 rounded-md text-sm hover:bg-gray-50 transition-colors">
              إلغاء الموعد
            </button>
            <p className="text-xs text-gray-500">
              يتبقى أقل من 4 ساعات على الموعد.
            </p>
          </div>
        </div>
      ))}
    </div>
  );
};

export default AppointmentCard;
