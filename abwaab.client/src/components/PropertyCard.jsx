import React from "react";
import LocationOnOutlinedIcon from "@mui/icons-material/LocationOnOutlined";
import StraightenIcon from "@mui/icons-material/Straighten";

export const PropertyCard = ({
  image = "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?auto=format&fit=crop&w=800&q=80",
  title = "شقة عصرية بإطلالة مفتوحة في أبو رمانة",
  location = "دمشق — أبو رمانة",
  price = "245,000",
  currency = "دولار",
  area = "180",
  typeTag = "شقة",
  statusTag = "سوبر ديلوكس",
  orientationTag = "غربي",
  onClick,
}) => {
  return (
    <div
      dir="rtl"
      onClick={onClick}
      className="max-w-sm rounded-3xl overflow-hidden bg-white shadow-sm border border-neutral-100 hover:shadow-md transition-shadow duration-300 cursor-pointer font-sans"
    >
      {/* Property Image Header */}
      <div className="h-56 w-full overflow-hidden">
        <img src={image} alt={title} className="w-full h-full object-cover" />
      </div>

      {/* Property Details Content */}
      <div className="p-5 flex flex-col gap-4 text-right">
        {/* Top Chips (Type & Status) */}
        <div className="flex items-center gap-2 justify-start">
          {typeTag && (
            <span className="px-3 py-1 text-xs font-semibold text-slate-800 bg-sky-50/60 border border-sky-100/80 rounded-xl">
              {typeTag}
            </span>
          )}
          {statusTag && (
            <span className="px-3 py-1 text-xs font-semibold text-slate-800 bg-slate-50 border border-slate-200/80 rounded-xl">
              {statusTag}
            </span>
          )}
        </div>

        {/* Title */}
        <h3 className="text-xl font-extrabold text-slate-900 leading-snug">
          {title}
        </h3>

        {/* Location */}
        <div className="flex items-center gap-1 text-slate-500 text-sm font-medium">
          <LocationOnOutlinedIcon fontSize="small" className="text-slate-500" />
          <span>{location}</span>
        </div>

        {/* Price */}
        <div className="text-2xl font-bold text-slate-900 flex items-center gap-1.5 pt-1">
          <span>{price}</span>
          <span className="text-xl font-bold text-slate-900">{currency}</span>
        </div>

        {/* Bottom Metadata (Area & Orientation) */}
        <div className="flex items-center flex-row-reverse justify-between pt-1">
          {/* Orientation Badge (Bottom Left in RTL) */}
          {orientationTag ? (
            <span className="px-3.5 py-1 text-xs font-semibold text-slate-700 bg-slate-50 border border-slate-200/80 rounded-xl">
              {orientationTag}
            </span>
          ) : (
            <div />
          )}

          {/* Area */}
          <div className="flex items-center gap-1.5 text-slate-800 font-bold text-base">
            <span>{area} م²</span>
            <StraightenIcon
              className="text-slate-800 transform rotate-90"
              fontSize="small"
            />
          </div>
        </div>
      </div>
    </div>
  );
};
