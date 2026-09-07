import React, { memo, useEffect, useRef, useState } from "react";
import { PropertyCard } from "../components/PropertyCard";
import { Slider, TextField, ToggleButton, Typography } from "@mui/material";
import { visitorApi } from "../api";
import { ORIENTATIONS } from "../dataTypes/propertis";
import { enqueueSnackbar } from "notistack";

const orientations = { ...ORIENTATIONS };
const ToggleButtonGroup = memo(
  ({ items, selectedId, onSelect, valueKey, labelKey }) => (
    <div className="flex flex-wrap gap-2 my-4">
      {items.map((item) => (
        <ToggleButton
          key={item[valueKey]}
          sx={{
            paddingX: "12px",
            paddingY: "2px",
            maxHeight: "32px",
            minWidth: "70px",
            "&.Mui-selected": { backgroundColor: "#169A94", color: "white" },
            "&.Mui-selected:hover": { backgroundColor: "#087A78" },
          }}
          value={item[valueKey]}
          className="!rounded-full"
          selected={item[valueKey] === selectedId}
          onChange={() => onSelect(item[valueKey])}
        >
          {item[labelKey]}
        </ToggleButton>
      ))}
    </div>
  )
);

const Properties = () => {
  const [searchData, setSearchData] = useState(null);
  const [form, setForm] = useState(null);
  const [resutls, setResults] = useState(null);
  const signalRef = useRef();

  const fetchSearchForm = async () => {
    // setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await visitorApi.getSearchForm(signalRef.current.signal);
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      setForm(resp.data);
    } catch (err) {
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
    } finally {
      // setLoading(false);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  useEffect(() => {
    setTimeout(() => {
      fetchSearchForm();
    }, 0);
  }, []);

  return (
    <div className="bg-neutral-50 flex max-w-[85%] mx-auto pb-24 mt-24 gap-3">
      {/* <section className="flex flex-1 gap-6"> */}
      <main className="rounded-xl w-9/12 ">
        <h3 className="font-semibold text-[32px] text-navy-700 mb-3">
          العقارات
        </h3>
        <h3 className="font-semibold text-[32px] text-navy-700 mb-3">
          15 عقاراً مشابهاً
        </h3>
        <p className="text-neutral-500 text-base">
          ابحث عن العقار وفقاً لاحتياجاتك
        </p>
        <p className="text-neutral-500 text-base">نتائج البحث عن "سيكب"</p>
        <div className="flex flex-wrap justify-start gap-5">
          <PropertyCard />
          <PropertyCard />
          <PropertyCard />
          <PropertyCard />
          <PropertyCard />
          <PropertyCard />
          <PropertyCard />
          <PropertyCard />
        </div>
      </main>
      <aside className="border border-neutral-200 rounded-xl w-3/12 h-fit py-3 px-6 bg-white sticky top-7">
        <h5 className="font-semibold text-lg text-navy-700">خيارات البحث</h5>
        <form method="post" onSubmit={(e) => handleSubmit(e)}>
          <TextField name="textSearch" label="نص البحث" variant="outlined" />
          <Typography
            variant="body2"
            className="text-neutral-600 font-medium mb-2"
          >
            نوع العقار
          </Typography>
          <ToggleButtonGroup
            items={form.propertyTypes}
            // selectedId={formData.propertyTypeId}
            // onSelect={(val) =>
            //   setFormData((prev) => ({ ...prev, propertyTypeId: val }))
            // }
            valueKey="typeId"
            labelKey="typeName"
          />
          <Slider
            getAriaLabel={() => "Temperature range"}
            // value={value}
            // onChange={handleChange}
            valueLabelDisplay="auto"
            // getAriaValueText={valuetext}
          />
          <Typography
            variant="body2"
            className="text-neutral-600 font-medium mb-2"
          >
            حالة الإكساء
          </Typography>
          <ToggleButtonGroup
            items={form.propertyFinishings}
            // selectedId={formData.propertyTypeId}
            // onSelect={(val) =>
            //   setFormData((prev) => ({ ...prev, propertyTypeId: val }))
            // }
            valueKey="finishingId"
            labelKey="finishingName"
          />
        </form>
      </aside>
      {/* </section> */}
    </div>
  );
};

export default Properties;
