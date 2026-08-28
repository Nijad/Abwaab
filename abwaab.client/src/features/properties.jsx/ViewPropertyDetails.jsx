import { Button } from "@mui/material";
import { useRef, useState } from "react";
import { useSnackbar } from "notistack";
import { propertyApi } from "../../api";
import { useNavigate, useParams } from "react-router";
import { InsertComment, Preview } from "@mui/icons-material";
import { LocationPicker } from "../../components/LocationPicker";
import LabelTag from "../../components/LabelTag";
import VisitReservationButton from "./VisitReservationButton";

const ViewPropertyDetails = ({ title, description, onClose, onSuccess }) => {
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [open, setOpen] = useState(false);
  const { enqueueSnackbar } = useSnackbar();
  const navigate = useNavigate();
  const signalRef = useRef();
  const getId = useParams("id");

  const fetchPreperty = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.getPropertyForUpdate(
        getId.id,
        signalRef.current.signal
      );
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
  return (
    <div className="bg-neutral-50 flex flex-col max-w-7x px-28 mx-auto pb-24">
      {/*Top bar*/}
      <section className="flex items-center content-between w-full my-4">
        <div className="p-5 flex-1">الرئيسية/العقارات/شقة للبيع في المزة</div>
        <div className="flex items-center">
          <div className="flex items-center">
            <InsertComment />
            <p className="text-sm mx-2">اضيف في 15 آب 2026</p>
          </div>
          <div className="flex items-center">
            <Preview />
            <p className="text-sm mx-2">523 مشاهدة</p>
          </div>
        </div>
      </section>
      <section className="flex flex-1 gap-6">
        <main className="rounded-xl w-4/6">
          {/* Cover Image Section */}
          <div className="w-full object-fill">
            <img
              src="https://media.rightmove.co.uk/property-photo/7a7a38e41/172696301/7a7a38e41f373b665c85fa12b1c0064f.jpeg"
              alt="sd"
              className="w-full rounded-xl"
            />
          </div>
          {/* Images Section */}
          <div className="my-3 flex flex-nowrap gap-3 w-full">
            <div className=" max-w-1/4 object-fill">
              <img
                src="https://media.rightmove.co.uk/property-photo/7a7a38e41/172696301/7a7a38e41f373b665c85fa12b1c0064f.jpeg"
                alt="sd"
                className="w-full rounded-xl"
              />
            </div>{" "}
            <div className=" max-w-1/4 object-fill">
              <img
                src="https://media.rightmove.co.uk/property-photo/7a7a38e41/172696301/7a7a38e41f373b665c85fa12b1c0064f.jpeg"
                alt="sd"
                className="w-full rounded-xl"
              />
            </div>{" "}
            <div className=" max-w-1/4 object-fill">
              <img
                src="https://media.rightmove.co.uk/property-photo/7a7a38e41/172696301/7a7a38e41f373b665c85fa12b1c0064f.jpeg"
                alt="sd"
                className="w-full rounded-xl"
              />
            </div>{" "}
            <div className=" max-w-1/4 object-fill">
              <img
                src="https://media.rightmove.co.uk/property-photo/7a7a38e41/172696301/7a7a38e41f373b665c85fa12b1c0064f.jpeg"
                alt="sd"
                className="w-full rounded-xl"
              />
            </div>
          </div>
          {/* Info Section */}
          <div className="my-6 flex flex-wrap p-4 bg-white gap-3  justify-between rounded-xl border border-neutral-200">
            <div className="bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
              info area
            </div>
            <div className="bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
              info area
            </div>
            <div className="bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
              info area
            </div>
            <div className="bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
              info area
            </div>
            <div className="bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
              info area
            </div>
            <div className="bg-neutral-50 text-navy-700 p-4 rounded-xl min-w-[32%]">
              info area
            </div>
          </div>
          {/* Description Section */}
          <div className="my-6 rounded-xl bg-white p-5 min-h-32">
            description
          </div>
        </main>
        <aside className="border border-neutral-200 rounded-xl w-2/6 h-fit py-3 px-6 bg-white">
          <div className="border-b border-b-neutral-200">
            <p className="text-neutral-600 text-lg my-4">السعر</p>
            <p className="text-navy-700 text-2xl font-semibold my-4">
              254,545,000 ليرة سورية
            </p>
          </div>
          <div className="border-b border-b-neutral-200">
            <p className="text-navy-700 text-2xl font-semibold my-4">
              اتجاهات العقار
            </p>
            <LabelTag
              label={"asldfjalsk"}
              classes="px-3 rounded-full bg-teal-50 border border-teal-500"
            />
            <p className="text-neutral-600 text-lg my-4"></p>
          </div>
          <div className="mb-3">
            <p className="text-navy-700 text-2xl font-semibold my-4">
              موقع العقار
            </p>
            <p className="text-neutral-600 text-lg my-4">
              دمشق المزة شارع الجلاء
            </p>
            <LocationPicker />
            <VisitReservationButton />
          </div>
        </aside>
      </section>
    </div>
  );
};

export default ViewPropertyDetails;
