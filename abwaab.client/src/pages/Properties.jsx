import React, { memo, useEffect, useRef, useState } from "react";
import { PropertyCard } from "../components/PropertyCard";
import {
  Button,
  Divider,
  Slider,
  TextField,
  ToggleButton,
  Typography,
} from "@mui/material";
import { visitorApi } from "../api";
import { ORIENTATIONS } from "../dataTypes/propertis";
import { enqueueSnackbar, useSnackbar } from "notistack";
import { SEARCH_DATA, SEARCH_VALUES } from "../dataTypes/visitor";

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
const BoolToggleButtonGroup = memo(({ items, selectedIds, onToggle }) => (
  <div className="flex items-center gap-3 mt-6 flex-wrap">
    {items.map((att) => {
      const isSelected = selectedIds.some((a) => a === att.attributeId);
      return (
        <ToggleButton
          key={att.attributeId}
          sx={{
            paddingX: "12px",
            paddingY: "2px",
            maxHeight: "32px",
            minWidth: "70px",
            "&.Mui-selected": { backgroundColor: "#169A94", color: "white" },
            "&.Mui-selected:hover": { backgroundColor: "#087A78" },
          }}
          value={att.attributeId}
          className="!rounded-full"
          selected={isSelected}
          onChange={() => onToggle(att)}
        >
          {att.attributeName}
        </ToggleButton>
      );
    })}
  </div>
));

const Properties = () => {
  const [searchData, setSearchData] = useState({
    price: [0, 0],
    area: [0, 0],
    ...SEARCH_DATA,
  });
  const [form, setForm] = useState({ ...SEARCH_VALUES });
  const [resutls, setResults] = useState(null);
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  console.log(searchData);

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
      setSearchData({
        ...searchData,
        price: [resp.data.minPrice, resp.data.maxPrice],
        area: [resp.data.minArea, resp.data.maxArea],
      });
    } catch (err) {
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
    } finally {
      // setLoading(false);
    }
  };

  const handleChange = (event, newValue) => {
    console.log(event);

    setSearchData({ ...searchData, [event.target.name]: newValue });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const data = {
      minArea: searchData.area[0],
      maxArea: searchData.area[1],
      minPrice: searchData.area[0],
      maxPrice: searchData.area[1],
      textSearch: searchData.textSearch,
      propertyType: searchData.propertyType,
      propertyFinishing: searchData.propertyFinishing,
      viewSides: searchData.viewSides,
    };
    try {
      signalRef.current = new AbortController();
      const resp = await visitorApi.search(data, signalRef.current.signal);
      // enqueueSnackbar(resp.data.message, { variant: "success" });
      // if (onSuccess) onSuccess(data, resp.data);
    } catch (err) {
      //list related error codes
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
    } finally {
      setLoading(false);
    }
  };
  const handleBoolAttributes = (data) => {
    // debugger;
    setSearchData((prev) => {
      const exist = prev.viewSides.includes(data.attributeId);
      if (!exist) {
        const result = [...prev.viewSides.filter((v) => v !== "")];
        result.push(data.attributeId);
        return { ...prev, viewSides: result };
      } else {
        const result = [
          ...prev.viewSides.filter((v) => v !== "" && v !== data.attributeId),
        ];
        return { ...prev, viewSides: result };
      }
    });
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
      <aside className="border border-neutral-200 rounded-xl w-3/12 h-fit py-3 px-6 bg-white sticky top-7 overflow-hidden">
        <h5 className="font-semibold text-lg text-navy-700">خيارات البحث</h5>
        <form method="post" onSubmit={(e) => handleSubmit(e)}>
          <TextField
            name="textSearch"
            label="نص البحث"
            variant="outlined"
            size="small"
            margin="normal"
            value={searchData.textSearch}
            onChange={(e) =>
              setSearchData({ ...searchData, [e.target.name]: e.target.value })
            }
          />
          <Divider className="!my-3" />
          <Typography
            variant="body2"
            className="text-neutral-600 font-medium mb-2"
          >
            نوع العقار
          </Typography>
          <ToggleButtonGroup
            items={form.propertyTypes}
            selectedId={searchData.propertyType}
            onSelect={(val) =>
              setSearchData((prev) => ({ ...prev, propertyType: val }))
            }
            valueKey="typeId"
            labelKey="typeName"
          />
          <Typography
            variant="body2"
            className="text-neutral-600 font-medium mb-2"
          >
            نطاق السعر
          </Typography>
          <Slider
            getAriaLabel={() => ""}
            name="price"
            value={searchData.price}
            onChange={handleChange}
            valueLabelDisplay="auto"
            // getAriaValueText={valuetext}
            min={form.minPrice}
            max={form.maxPrice}
          />
          <Typography
            variant="body2"
            className="text-neutral-600 font-medium mb-2"
          >
            نطاق المساحة
          </Typography>
          <Slider
            getAriaLabel={() => ""}
            name="area"
            value={searchData.area}
            onChange={handleChange}
            valueLabelDisplay="auto"
            // getAriaValueText={valuetext}
            min={form.minArea}
            max={form.maxArea}
          />
          <Typography
            variant="body2"
            className="text-neutral-600 font-medium mb-2"
          >
            حالة الإكساء
          </Typography>
          <ToggleButtonGroup
            items={form.propertyFinishings}
            selectedId={searchData.propertyFinishing}
            onSelect={(val) =>
              setSearchData((prev) => ({ ...prev, propertyFinishing: val }))
            }
            valueKey="finishingId"
            labelKey="finishingName"
          />
          <Typography
            variant="body2"
            className="text-neutral-600 font-medium mb-2"
          >
            اتجاهات العقار
          </Typography>
          <BoolToggleButtonGroup
            items={ORIENTATIONS}
            selectedIds={searchData.viewSides}
            onToggle={handleBoolAttributes}
          />
          <Divider className="!my-2" />
          <Button
            size="medium"
            variant="contained"
            color="navy"
            type="submit"
            fullWidth
            className="!my-2"
            loading={loading}
          >
            تنفيذ البحث
          </Button>
        </form>
      </aside>
      {/* </section> */}
    </div>
  );
};

export default Properties;
