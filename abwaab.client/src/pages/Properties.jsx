import React, { memo, useEffect, useRef, useState } from "react";
import { PropertyCard } from "../components/PropertyCard";
import {
  Button,
  Divider,
  Pagination,
  Slider,
  TextField,
  ToggleButton,
  Typography,
} from "@mui/material";
import { visitorApi } from "../api";
import { ORIENTATIONS } from "../dataTypes/propertis";
import { enqueueSnackbar, useSnackbar } from "notistack";
import { SEARCH_DATA, SEARCH_VALUES } from "../dataTypes/visitor";
import { useNavigate, useParams } from "react-router";

const ToggleButtonGroup = memo(
  ({ items, name, selectedId, onSelect, valueKey, labelKey }) => (
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
          onChange={() => onSelect(name, item[valueKey])}
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
    price: [null, null],
    area: [null, null],
    ...SEARCH_DATA,
  });
  const [form, setForm] = useState({ ...SEARCH_VALUES });
  const [resutls, setResults] = useState(null);
  const [loading, setLoading] = useState(false);
  const [searching, setSearching] = useState(false);
  const formSignalRef = useRef();
  const dataSignalRef = useRef();
  const getType = useParams("id");
  const { enqueueSnackbar } = useSnackbar();
  const navigate = useNavigate();

  console.log(getType);

  const fetchSearchForm = async () => {
    // setLoading(true);
    if (formSignalRef.current) {
      formSignalRef.current.abort();
    }
    try {
      formSignalRef.current = new AbortController();
      const resp = await visitorApi.getSearchForm(formSignalRef.current.signal);
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      // debugger;
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

  const fetchInitialData = async () => {
    // setLoading(true);
    if (dataSignalRef.current) {
      dataSignalRef.current.abort();
    }
    try {
      dataSignalRef.current = new AbortController();
      const resp = await visitorApi.getMostViewed(
        1,
        dataSignalRef.current.signal
      );
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      // debugger;
      setResults(resp.data);
    } catch (err) {
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
    } finally {
      // setLoading(false);
    }
  };

  const fetchPromoted = async () => {
    // setLoading(true);
    if (dataSignalRef.current) {
      dataSignalRef.current.abort();
    }
    try {
      dataSignalRef.current = new AbortController();
      const resp = await visitorApi.getPremium(1, dataSignalRef.current.signal);
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      // debugger;
      setResults(resp.data);
    } catch (err) {
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
    } finally {
      // setLoading(false);
    }
  };

  const fetchRecentlyAdded = async () => {
    // setLoading(true);
    if (dataSignalRef.current) {
      dataSignalRef.current.abort();
    }
    try {
      dataSignalRef.current = new AbortController();
      const resp = await visitorApi.getRecentlyAdded(
        1,
        dataSignalRef.current.signal
      );
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      // debugger;
      setResults(resp.data);
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
    if (formSignalRef.current) {
      formSignalRef.current.abort();
    }
    const data = {
      minArea: searchData.area[0],
      maxArea: searchData.area[1],
      minPrice: searchData.price[0],
      maxPrice: searchData.price[1],
      textSearch: searchData.textSearch,
      propertyType: searchData.propertyType,
      propertyFinishing: searchData.propertyFinishing,
      viewSides: searchData.viewSides,
      pageNo: 1,
    };
    try {
      formSignalRef.current = new AbortController();
      const resp = await visitorApi.search(data, formSignalRef.current.signal);
      setResults(resp.data);
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

  const handleToggleButtons = (name, id) => {
    const exist = searchData[name] === id;
    if (exist) {
      setSearchData({ ...searchData, [name]: null });
    } else {
      setSearchData({ ...searchData, [name]: id });
    }
  };

  useEffect(() => {
    setTimeout(() => {
      fetchSearchForm();
      switch (getType["id"]) {
        case undefined:
          fetchInitialData();
          break;
        case "recent-properties":
          fetchRecentlyAdded();
          break;
        case "promoted-properties":
          fetchPromoted();
          break;
        case "most-viewed":
          fetchInitialData();
          break;

        default:
          break;
      }
    }, 0);
  }, []);

  return (
    <div className="bg-neutral-50 flex w-full max-w-[85%] mx-auto pb-24 mt-24 gap-3">
      {/* <section className="flex flex-1 gap-6"> */}
      <main className="rounded-xl w-9/12 ">
        {!searching && (
          <h3 className="font-semibold text-[32px] text-navy-700 mb-3">
            العقارات
          </h3>
        )}
        {searching && (
          <h3 className="font-semibold text-[32px] text-navy-700 mb-3">
            تم العثور على {resutls.length} عقاراً مشابهاً
          </h3>
        )}
        <p className="text-neutral-500 text-base">
          ابحث عن العقار وفقاً لاحتياجاتك
        </p>
        {/* <p className="text-neutral-500 text-base">نتائج البحث عن "سيكب"</p> */}
        <div className="flex flex-wrap justify-start gap-5 mt-5">
          {resutls &&
            resutls.properties.map((prop) => (
              <PropertyCard
                area={prop.area}
                currency={"دولار أمريكي"}
                image={`${import.meta.env.VITE_API_BASE_URL}${prop.coverImage}`}
                key={`prop-${prop.propertyId}`}
                // orientationTag={prop.}
                price={prop.price}
                location={prop.address}
                statusTag={prop.propertyFinishing}
                title={prop.title}
                typeTag={prop.propertyType}
                onClick={() => navigate(`/properties/${prop.propertyId}`)}
              />
            ))}
        </div>
        {resutls && (
          <Pagination
            sx={{ mt: 6 }}
            count={resutls.pagesCount}
            shape="rounded"
          />
        )}
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
            fullWidth
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
            onSelect={handleToggleButtons}
            valueKey="typeId"
            labelKey="typeName"
            name="propertyType"
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
            onSelect={handleToggleButtons}
            valueKey="finishingId"
            labelKey="finishingName"
            name="propertyFinishing"
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
