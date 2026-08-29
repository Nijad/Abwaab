import React, { memo, useEffect, useRef, useState } from "react";
import { useSnackbar } from "notistack";
import { propertyApi } from "../../api";
import { NavLink, useParams } from "react-router";
import { LocationPicker } from "../../components/LocationPicker";
import LabelTag from "../../components/LabelTag";
import VisitReservationButton from "./VisitReservationButton";
import PropertyDetailsLoading from "../../components/properties/PropertyDetailsLoading";
import PropertyNotFound from "../../components/properties/PropertyNotFound";
import { ORIENTATIONS, PROPERTY_DETAILS } from "../../dataTypes/propertis";
import RemoveRedEyeOutlinedIcon from "@mui/icons-material/RemoveRedEyeOutlined";
import DateRangeOutlinedIcon from "@mui/icons-material/DateRangeOutlined";
import { PropertyMediaGalleryModal } from "../../components/PropertyMediaGalleryModal";
import StarBorderOutlinedIcon from "@mui/icons-material/StarBorderOutlined";
import ApartmentRoundedIcon from "@mui/icons-material/ApartmentRounded";
import StraightenRoundedIcon from "@mui/icons-material/StraightenRounded";
import ImagesearchRollerOutlinedIcon from "@mui/icons-material/ImagesearchRollerOutlined";

const CustomAttributes = memo(({ attributes }) => {
  return (
    <React.Fragment>
      {attributes.length > 0 &&
        attributes.map((attribute) => {
          const exist = ORIENTATIONS.find(
            (o) => o.attributeId === attribute.attributeId
          );
          if (!exist) {
            return (
              <div className="flex items-center justify-start bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
                <StarBorderOutlinedIcon
                  sx={{ marginInlineStart: "8px", marginInlineEnd: "15px" }}
                />
                <div className="">
                  <p className="text-navy-700 text-sm my-2">
                    {attribute.attributeName}
                  </p>
                  <p className="text-navy-700 text-xl font-semibold my-2">
                    {attribute.dataTypeDescription === "boolean"
                      ? "متوفر"
                      : attribute.value}
                  </p>
                </div>
              </div>
            );
          }
        })}
    </React.Fragment>
  );
});
const Orientations = memo(({ attributes }) => {
  return (
    <React.Fragment>
      {ORIENTATIONS.map((oreint) => {
        const exist = attributes.find(
          (att) => att.attributeId === oreint.attributeId
        );
        if (exist) {
          return (
            <LabelTag
              label={oreint.attributeName}
              classes="px-3 rounded-full bg-teal-50 border border-teal-500 inline-block me-3"
            />
          );
        }
      })}
    </React.Fragment>
  );
});

const ViewPropertyDetails = () => {
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState(PROPERTY_DETAILS);
  const { enqueueSnackbar } = useSnackbar();
  const signalRef = useRef();
  const getId = useParams("id");

  const fetchPreperty = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.propertyDetails(
        getId.id,
        signalRef.current.signal
      );
      setData(resp.data);
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      //   if (onSuccess) onSuccess(data.newEmail, resp.data);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err.detail, { variant: "error" });
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        return;
      } else if (err.errorCode === "") {
        enqueueSnackbar(err.detail, { variant: "error" });
      }
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setTimeout(() => {
      fetchPreperty();
    }, 0);
  }, []);

  if (loading) {
    return <PropertyDetailsLoading />;
  } else if (!data) {
    return (
      <div className="mt-24">
        <PropertyNotFound />
      </div>
    );
  } else {
    return (
      <div className="bg-neutral-50 flex flex-col max-w-7x px-28 mx-auto pb-24">
        {/*Top bar*/}
        <section className="flex items-center content-between w-full my-4">
          <div className="p-5 flex-1 flex">
            <NavLink to={"/"}>الرئيسية</NavLink>
            <p className="mx-2">/</p>
            <NavLink to={"/properties"}>العقارات</NavLink>
            <p className="mx-2">/</p>
            {data.title}
          </div>
          <div className="flex items-center">
            <div className="flex items-center">
              <DateRangeOutlinedIcon />
              <p className="text-sm mx-2">اضيف في 15 آب 2026</p>
            </div>
            <div className="flex items-center">
              <RemoveRedEyeOutlinedIcon />
              <p className="text-sm mx-2">{data.viewsNumber} مشاهدة</p>
            </div>
          </div>
        </section>
        <section className="flex flex-1 gap-6">
          <main className="rounded-xl w-4/6">
            <PropertyMediaGalleryModal />
            {/* Info Section */}
            <div className="my-6 flex flex-wrap p-4 bg-white gap-3  justify-start rounded-xl border border-neutral-200">
              <div className="flex items-center justify-start bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
                <ApartmentRoundedIcon
                  sx={{ marginInlineStart: "8px", marginInlineEnd: "15px" }}
                />
                <div className="">
                  <p className="text-navy-700 text-sm my-2">نوع العقار</p>
                  <p className="text-navy-700 text-xl font-semibold my-2">
                    {data.propertyType}
                  </p>
                </div>
              </div>
              <div className="flex items-center justify-start bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
                <ImagesearchRollerOutlinedIcon
                  sx={{ marginInlineStart: "8px", marginInlineEnd: "15px" }}
                />
                <div className="">
                  <p className="text-navy-700 text-sm my-2">حالة الإكساء</p>
                  <p className="text-navy-700 text-xl font-semibold my-2">
                    {data.propertyFinishing}
                  </p>
                </div>
              </div>
              <div className="flex items-center justify-start bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
                <StraightenRoundedIcon
                  sx={{ marginInlineStart: "8px", marginInlineEnd: "15px" }}
                />
                <div className="">
                  <p className="text-navy-700 text-sm my-2">المساحة</p>
                  <p className="text-navy-700 text-xl font-semibold my-2">
                    {data.areaInSquareMeter} م<sup>2</sup>
                  </p>
                </div>
              </div>
              {/* Loop through remaining attributes */}
              <CustomAttributes attributes={data.propertyAttributesList} />
            </div>
            {/* Description Section */}
            <div className="my-6 rounded-xl bg-white p-5 min-h-32">
              <h6 className="text-navy-700 text-xl font-semibold my-2">وصف</h6>
              <p className="text-neutral-700 text-base my-2">
                {data.description}
              </p>
            </div>
          </main>
          <aside className="border border-neutral-200 rounded-xl w-2/6 h-fit py-3 px-6 bg-white">
            <div className="border-b border-b-neutral-200">
              <p className="text-neutral-600 text-lg my-4">السعر</p>
              <p className="text-navy-700 text-2xl font-semibold my-4">
                {data.price?.toLocaleString()} ليرة سورية
              </p>
            </div>
            <div className="border-b border-b-neutral-200">
              <p className="text-navy-700 text-2xl font-semibold my-4">
                اتجاهات العقار
              </p>
              <Orientations attributes={data.propertyAttributesList} />
              <p className="text-neutral-600 text-lg my-4"></p>
            </div>
            <div className="mb-3">
              <p className="text-navy-700 text-2xl font-semibold my-4">
                موقع العقار
              </p>
              <p className="text-neutral-600 text-lg my-4">{data.address}</p>
              <div className="mb-3">
                {data.latitude && data.latitude && (
                  <LocationPicker
                    lat={data.latitude}
                    lng={data.longitude}
                    readOnly
                  />
                )}
              </div>
              <VisitReservationButton />
            </div>
          </aside>
        </section>
      </div>
    );
  }
};
export default ViewPropertyDetails;
