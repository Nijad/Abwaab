import React, { useCallback, useEffect, useRef, useState } from "react";
import {
  Box,
  Typography,
  TextField,
  // Chip,
  Button,
  Checkbox,
  FormControlLabel,
  Paper,
  IconButton,
  ToggleButton,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import MapIcon from "@mui/icons-material/Map";
import { useParams } from "react-router";
import { useSnackbar } from "notistack";
import { propertyApi } from "../../api";
import {
  collapseTimeSlots,
  generateTimeSlots,
  timeSlots,
} from "../../utils/helpers";
import MediaUploader from "./MediaUploader";

const orientations = [
  {
    attributeId: "b6c2c609-8240-4a8e-a57a-01b2e2c22f89",
    attributeName: "شرقي",
  },
  {
    attributeId: "28e3819e-cfd1-404b-9fed-09264b61a9ed",
    attributeName: "جنوبي",
  },
  {
    attributeId: "4d3568ee-436b-4a53-9601-261312622055",
    attributeName: "شمالي",
  },
  {
    attributeId: "539c754b-dae7-47fc-aa13-f48817ac3475",
    attributeName: "غربي",
  },
];

const EditPropertyById = () => {
  const getId = useParams("id");
  // console.log(getId);
  const [propData, setPropData] = useState({
    propertyTypesList: [],
    propertyFinishingsList: [],
    weekDaysList: [],
    attributes: [],
    propertyId: "",
    title: null,
    description: null,
    address: null,
    areaInSquareMeter: null,
    price: null,
    latitude: null,
    longitude: null,
    propertyTypeId: null,
    propertyFinishingId: null,
    timeSlots: [],
    propertyAttributesList: [],
  });

  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  // console.log(propData);
  // const ts = collapseTimeSlots(
  //   [
  //     {
  //       timeSlotId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  //       day: 0,
  //       dayName: "string",
  //       startTime: "10:00:00",
  //       endTime: "10:30:00",
  //       notes: "string",
  //     },
  //   ],
  //   [
  //     {
  //       dayIndex: 0,
  //       dayName: "Sunday",
  //     },
  //     {
  //       dayIndex: 1,
  //       dayName: "Monday",
  //     },
  //     {
  //       dayIndex: 2,
  //       dayName: "Tuesday",
  //     },
  //     {
  //       dayIndex: 3,
  //       dayName: "Wednesday",
  //     },
  //     {
  //       dayIndex: 4,
  //       dayName: "Thursday",
  //     },
  //     {
  //       dayIndex: 5,
  //       dayName: "Friday",
  //     },
  //     {
  //       dayIndex: 6,
  //       dayName: "Saturday",
  //     },
  //   ]
  // );
  // console.log(ts);

  // 0Available Viewing Schedule State
  const [schedules, setSchedules] = useState(
    null
    // {
    // 0: { checked: false, startTime: "", to: "" },
    // 1: { checked: false, from: "", to: "" },
    // 2: { checked: false, from: "", to: "" },
    // 3: { checked: false, from: "", to: "" },
    // 4: { checked: false, from: "", to: "" },
    // 5: { checked: false, from: "", to: "" },
    // 6: { checked: false, from: "", to: "" },
    // }
  );
  // console.log(schedules);
  const getScheules = useCallback(() => {
    return Object.keys(schedules);
  }, []);

  const handleScheduleChange = (day, field, value) => {
    //handle uncheck day
    //handle change from if to is selected
    setSchedules((prev) => ({
      ...prev,
      [day]: { ...prev[day], [field]: value },
    }));
  };

  const fetchProperty = async () => {
    setLoading(true);

    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      //   debugger;
      signalRef.current = new AbortController();
      const resp = await propertyApi.getPropertyForUpdate(
        getId.id,
        signalRef.current.signal
      );
      setPropData(resp.data);
      // setSchedules(resp.data.weekDaysList.map(day=>({[day.dayIndex]:{checked:da}})))
      const schedule = collapseTimeSlots(
        resp.data.timeSlots,
        resp.data.weekDaysList
      );
      setSchedules(schedule);
      // enqueueSnackbar(resp.data.message, { variant: "success" });
      //   if (onSuccess) onSuccess(data.newEmail, resp.data);
    } catch (err) {
      //list related error codes
      setPropData(null);
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
        return;
      } else if (err.errorCode === "") {
        enqueueSnackbar(err, { variant: "error" });
      } else {
        enqueueSnackbar(err.message, { variant: "error" });
      }
    } finally {
      setLoading(false);
    }
  };

  const saveProperty = async () => {
    setLoading(true);

    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      // debugger;
      signalRef.current = new AbortController();
      const timeSlotsArr = [];
      for (const day of propData.weekDaysList) {
        const dayData = { ...schedules[day.dayIndex] };
        const tempArr = generateTimeSlots(
          day,
          dayData.startTime,
          dayData.endTime
        );
        timeSlotsArr.push(...tempArr);
      }
      const propNewData = { ...propData, timeSlots: timeSlotsArr };
      const resp = await propertyApi.updateProperty(
        propNewData,
        signalRef.current.signal
      );
      // setPropData(resp.data);
      // setSchedules(resp.data.weekDaysList.map(day=>({[day.dayIndex]:{checked:da}})))
      // const schedule = collapseTimeSlots(
      //   resp.data.timeSlots,
      //   resp.data.weekDaysList
      // );
      // setSchedules(schedule);
      // enqueueSnackbar(resp.data.message, { variant: "success" });
      //   if (onSuccess) onSuccess(data.newEmail, resp.data);
    } catch (err) {
      //list related error codes
      // setPropData(null);
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
        return;
      } else if (err.errorCode === "") {
        enqueueSnackbar(err, { variant: "error" });
      } else {
        enqueueSnackbar(err.message, { variant: "error" });
      }
    } finally {
      setLoading(false);
    }
  };

  const handleBoolAttributes = (data) => {
    const exist = propData.propertyAttributesList.find(
      (a) => a.attributeId === data.attributeId
    );
    // debugger;
    //if alredy in the list then remove it
    if (exist) {
      const newAttrbuts = propData.propertyAttributesList.filter(
        (a) => a.attributeId !== data.attributeId
      );
      setPropData({ ...propData, propertyAttributesList: newAttrbuts });
      return;
    }
    //if not, add it
    const attrb = {
      propertyAttributeId: null,
      value: "true",
      attributeId: data.attributeId,
      attributeName: data.attributeName,
      dataTypeId: data.dataTypeId,
      dataTypeDescription: data.datayTypeDescription,
    };
    setPropData({
      ...propData,
      propertyAttributesList: [...propData.propertyAttributesList, attrb],
    });
  };
  const getAttributeValue = (id) => {
    const att = propData.propertyAttributesList.find(
      (a) => a.attributeId === id
    );
    if (att) {
      return att.value;
    }
    return "";
  };

  const handleInputsAttributes = (value, data) => {
    const exist = propData.propertyAttributesList.find(
      (a) => a.attributeId === data.attributeId
    );
    // debugger;
    //if alredy in the list then modify it
    if (exist) {
      const newAttrbuts = propData.propertyAttributesList.map((a) => {
        if (a.attributeId === data.attributeId) {
          return { ...a, value: value };
        } else return a;
      });
      setPropData({ ...propData, propertyAttributesList: newAttrbuts });
      return;
    }
    //if not, add it
    const attrb = {
      propertyAttributeId: null,
      value: value,
      attributeId: data.attributeId,
      attributeName: data.attributeName,
      dataTypeId: data.dataTypeId,
      dataTypeDescription: data.datayTypeDescription,
    };
    setPropData({
      ...propData,
      propertyAttributesList: [...propData.propertyAttributesList, attrb],
    });
  };

  const GetBoolAttributesNoOrientation = () => {
    return propData.attributes.filter(
      (a) =>
        a.datayTypeDescription === "boolean" &&
        orientations.find((b) => a.attributeId == b.attributeId) == undefined
    );
  };

  useEffect(() => {
    setTimeout(() => {
      fetchProperty();
    }, 0);
  }, []);

  // if (!propData) {
  //   return <></>;
  // }

  return (
    <div className="min-h-screen  mt-5 flex justify-center  w-full">
      <div className="w-full ">
        {/* Top Header outside paper */}

        {/* Main Card */}
        <Paper className="p-6 md:p-8 !rounded-2xl bg-white shadow-xl space-y-8">
          {/* Section 1: Basic Information & Cover Upload */}
          <Box>
            <Typography
              variant="h6"
              className="font-bold text-neutral-800 mb-6 text-right"
            >
              المعلومات الأساسية
            </Typography>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-6 items-start ">
              {/* Left Column: Cover Image Upload Dropzone */}
              <div className="md:col-span-1 border-2 border-dashed border-sky-300 rounded-2xl p-6 bg-neutral-50 flex flex-col items-center justify-center text-center cursor-pointer hover:bg-neutral-100 transition-colors h-full min-h-[300px] w-full">
                <div className="w-10 h-10 rounded-full bg-white shadow flex items-center justify-center mb-3">
                  <AddIcon className="text-neutral-600" />
                </div>
                <Typography className="font-bold text-neutral-800 mb-1">
                  إضافة صورة الغلاف
                </Typography>
                <Typography variant="caption" className="text-neutral-400">
                  مطلوبة
                </Typography>
              </div>

              {/* Right Column: Chips Selection */}
              <div className="md:col-span-2 space-y-5 text-right">
                {/* Property Type */}
                <div>
                  <Typography
                    variant="body2"
                    className="text-neutral-600 font-medium mb-2"
                  >
                    نوع العقار
                  </Typography>
                  <div className="flex flex-wrap gap-2 my-4">
                    {propData.propertyTypesList.map((type) => (
                      <ToggleButton
                        key={type.propertyTypeId}
                        sx={{
                          paddingX: "12px",
                          paddingY: "2px",
                          maxHeight: "32px",
                          "&.Mui-selected": {
                            backgroundColor: "#169A94",
                            color: "white",
                          },
                          "&.Mui-selected:hover": {
                            backgroundColor: "#087A78",
                          },
                          minWidth: "70px",
                        }}
                        value={type.typeId}
                        className="!rounded-full"
                        selected={type.typeId === propData.propertyTypeId}
                        onChange={() =>
                          setPropData({
                            ...propData,
                            propertyTypeId: type.typeId,
                          })
                        }
                      >
                        {type.typeName}
                      </ToggleButton>
                    ))}
                  </div>
                </div>

                {/* Finish Type */}
                <div>
                  <Typography
                    variant="body2"
                    className="text-neutral-600 font-medium mb-2"
                  >
                    حالة الإكساء
                  </Typography>
                  <div className="flex flex-wrap gap-2 my-4">
                    {propData.propertyFinishingsList.map((type) => (
                      <ToggleButton
                        key={type.propertyFinishingId}
                        sx={{
                          paddingX: "12px",
                          paddingY: "2px",
                          maxHeight: "32px",
                          "&.Mui-selected": {
                            backgroundColor: "#169A94",
                            color: "white",
                          },
                          "&.Mui-selected:hover": {
                            backgroundColor: "#087A78",
                          },
                          minWidth: "70px",
                        }}
                        value={type.finishingId}
                        className="!rounded-full"
                        selected={
                          type.finishingId === propData.propertyFinishingId
                        }
                        onChange={() =>
                          setPropData({
                            ...propData,
                            propertyFinishingId: type.finishingId,
                          })
                        }
                      >
                        {type.finishingName}
                      </ToggleButton>
                    ))}
                  </div>
                </div>
                {/* Orientations */}
                <div>
                  <Typography
                    variant="body2"
                    className="text-neutral-600 font-medium mb-2"
                  >
                    اتجاهات العقار (اختياري)
                  </Typography>
                  <div className="flex flex-wrap gap-2 my-4">
                    {orientations.map((side) => (
                      <ToggleButton
                        key={side.attributeId}
                        sx={{
                          paddingX: "12px",
                          paddingY: "2px",
                          maxHeight: "32px",
                          "&.Mui-selected": {
                            backgroundColor: "#169A94",
                            color: "white",
                          },
                          "&.Mui-selected:hover": {
                            backgroundColor: "#087A78",
                          },
                          minWidth: "70px",
                        }}
                        value={side.attributeId}
                        className="!rounded-full"
                        selected={
                          propData.propertyAttributesList.find(
                            (a) => a.attributeId === side.attributeId
                          ) !== undefined
                            ? true
                            : false
                        }
                        onChange={() => handleBoolAttributes(side)}
                      >
                        {side.attributeName}
                      </ToggleButton>
                    ))}
                  </div>
                </div>
              </div>
            </div>

            {/* Inputs Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
              <TextField
                placeholder="وصف مختصر"
                label="وصف مختصر"
                variant="filled"
                fullWidth
                value={propData.title}
                name="title"
                onChange={(e) =>
                  setPropData({ ...propData, [e.target.name]: e.target.value })
                }
                InputProps={{
                  disableUnderline: true,
                  className: "rounded-lg bg-neutral-100/70",
                }}
              />
              <TextField
                placeholder="المساحة بالمتر المربع"
                label="المساحة بالمتر المربع"
                variant="filled"
                fullWidth
                type="number"
                value={propData.areaInSquareMeter}
                name="areaInSquareMeter"
                onChange={(e) =>
                  setPropData({ ...propData, [e.target.name]: e.target.value })
                }
                InputProps={{
                  disableUnderline: true,
                  className: "rounded-lg bg-neutral-100/70",
                }}
              />
              <TextField
                placeholder="السعر"
                label="السعر"
                variant="filled"
                fullWidth
                type="number"
                value={propData.price}
                name="price"
                onChange={(e) =>
                  setPropData({ ...propData, [e.target.name]: e.target.value })
                }
                InputProps={{
                  disableUnderline: true,
                  className: "rounded-lg bg-neutral-100/70",
                }}
              />
              <TextField
                placeholder="العنوان"
                label="العنوان"
                variant="filled"
                fullWidth
                value={propData.address}
                name="address"
                onChange={(e) =>
                  setPropData({ ...propData, [e.target.name]: e.target.value })
                }
                InputProps={{
                  disableUnderline: true,
                  className: "rounded-lg bg-neutral-100/70",
                }}
              />
            </div>
          </Box>

          {/* Section 2: Address, Map & Description */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Left Side: Description */}
            <div className="flex flex-col justify-between">
              <TextField
                placeholder="تفاصيل العقار"
                multiline
                rows={7}
                variant="outlined"
                fullWidth
                value={propData.description}
                label="تفاصيل العقار"
                name="description"
                onChange={(e) =>
                  setPropData({ ...propData, [e.target.name]: e.target.value })
                }
                className="bg-white rounded-lg"
              />
              <Typography
                variant="caption"
                className="text-neutral-400 text-right mt-1"
              >
                اذكر التفاصيل التي تساعد المستخدم على اتخاذ قرار مناسب.
              </Typography>
            </div>
            {/* Right Side: Map & Address */}
            <div className="space-y-4">
              {/* <TextField
                placeholder="عنوان العقار"
                variant="outlined"
                fullWidth
                value={address}
                onChange={(e) => setAddress(e.target.value)}
                className="bg-white rounded-lg"
              /> */}
              <div className="bg-neutral-100 rounded-lg h-full border border-neutral-200 relative flex items-center justify-center">
                <span className="absolute top-2 left-2 bg-white/80 px-2 py-0.5 rounded text-[10px] font-sans text-neutral-500">
                  Google Maps
                </span>
                <div className="text-center text-neutral-500 flex flex-col items-center gap-1">
                  <MapIcon className="text-neutral-400" />
                  <Typography
                    variant="body2"
                    className="text-neutral-500 text-xs"
                  >
                    لم يتم تحديد الموقع
                  </Typography>
                </div>
              </div>
            </div>
          </div>

          {/* Section 3: Extra Attributes */}
          <Box className="space-y-3">
            <Typography
              variant="h6"
              className="font-bold text-neutral-800 text-right"
            >
              المميزات الإضافية
            </Typography>
            {/* number attributes */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
              {propData.attributes
                .filter((a) => a.datayTypeDescription === "number")
                .map((att) => (
                  <TextField
                    placeholder={att.attributeName}
                    type="number"
                    variant="filled"
                    fullWidth
                    value={getAttributeValue(att.attributeId)}
                    onChange={(e) =>
                      handleInputsAttributes(e.target.value, att)
                    }
                    InputProps={{
                      disableUnderline: true,
                      className: "rounded-lg bg-neutral-100/70",
                    }}
                  />
                ))}
            </div>
            {/* string attributes */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
              {propData.attributes
                .filter((a) => a.datayTypeDescription === "string")
                .map((att) => (
                  <TextField
                    placeholder={att.attributeName}
                    type="text"
                    variant="filled"
                    fullWidth
                    value={getAttributeValue(att.attributeId)}
                    onChange={(e) => handleInputsAttributes(e.target.value)}
                    InputProps={{
                      disableUnderline: true,
                      className: "rounded-lg bg-neutral-100/70",
                    }}
                  />
                ))}
            </div>
            {/* bool attributes */}
            <div className="flex items-center gap-3 mt-6">
              {GetBoolAttributesNoOrientation().map((att) => (
                <ToggleButton
                  key={att.attributeId}
                  sx={{
                    paddingX: "12px",
                    paddingY: "2px",
                    maxHeight: "32px",
                    "&.Mui-selected": {
                      backgroundColor: "#169A94",
                      color: "white",
                    },
                    "&.Mui-selected:hover": {
                      backgroundColor: "#087A78",
                    },
                    minWidth: "70px",
                  }}
                  value={att.attributeId}
                  className="!rounded-full"
                  selected={
                    propData.propertyAttributesList.find(
                      (a) => a.attributeId === att.attributeId
                    ) !== undefined
                      ? true
                      : false
                  }
                  onChange={() => handleBoolAttributes(att)}
                >
                  {att.attributeName}
                </ToggleButton>
              ))}
            </div>
          </Box>
          {/* Section 4: Available Viewing Times */}
          <Box className="space-y-3">
            <Typography
              variant="h6"
              className="font-bold text-neutral-800 text-right"
            >
              أوقات المعاينة المتاحة
            </Typography>
            <Typography
              variant="body2"
              className="text-neutral-400 text-right text-xs mb-4"
            >
              اختر الأيام المناسبة وقدّر وقت البداية والنهاية. يُتاح وقت النهاية
              بعد اختيار البداية، والحد الأدنى للفترة 30 دقيقة.
            </Typography>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-x-6 gap-y-3">
              {schedules &&
                propData.weekDaysList.map((day) => (
                  <div
                    key={day.dayIndex}
                    className="flex items-center justify-between gap-2 border-b border-neutral-100 pb-2"
                  >
                    <FormControlLabel
                      control={
                        <Checkbox
                          checked={schedules[day.dayIndex].checked}
                          onChange={(e) =>
                            handleScheduleChange(
                              day.dayIndex,
                              "checked",
                              e.target.checked
                            )
                          }
                          size="small"
                        />
                      }
                      label={schedules[day.dayIndex].name}
                      className="min-w-[90px] text-neutral-700"
                    />
                    <div className="flex items-center gap-2">
                      <FormControl sx={{ m: 1, minWidth: 120 }}>
                        <InputLabel id="demo-simple-select-autowidth-label">
                          من
                        </InputLabel>
                        <Select
                          labelId="demo-simple-select-autowidth-label"
                          id="demo-simple-select-autowidth"
                          value={schedules[day.dayIndex].startTime}
                          onChange={(e) =>
                            handleScheduleChange(
                              day.dayIndex,
                              "startTime",
                              e.target.value
                            )
                          }
                          autoWidth
                          disabled={!schedules[day.dayIndex].checked}
                          label="من"
                          size="small"
                        >
                          <MenuItem value="">
                            <span>اختر فترة</span>
                          </MenuItem>
                          {timeSlots().map((t) => (
                            <MenuItem value={t}>{t}</MenuItem>
                          ))}
                        </Select>
                      </FormControl>
                      <FormControl sx={{ m: 1, minWidth: 120 }}>
                        <InputLabel id="demo-simple-select-autowidth-label">
                          إلى
                        </InputLabel>
                        <Select
                          labelId="demo-simple-select-autowidth-label"
                          id="demo-simple-select-autowidth"
                          value={schedules[day.dayIndex].endTime}
                          onChange={(e) =>
                            handleScheduleChange(
                              day.dayIndex,
                              "endTime",
                              e.target.value
                            )
                          }
                          disabled={
                            !schedules[day.dayIndex].startTime ||
                            !schedules[day.dayIndex].checked
                          }
                          autoWidth
                          label="Age"
                          size="small"
                        >
                          <MenuItem value="">
                            <span>اختر فترة</span>
                          </MenuItem>
                          {timeSlots(
                            null,
                            schedules[day.dayIndex].startTime
                          ).map((t) => (
                            <MenuItem value={t}>{t}</MenuItem>
                          ))}
                        </Select>
                      </FormControl>
                    </div>
                  </div>
                ))}
            </div>
          </Box>

          {/* Section 4: Property Photos & Videos */}
          <Box className="space-y-4 pt-2">
            <div className="text-right">
              <Typography variant="h6" className="font-bold text-neutral-800">
                صور وفيديو العقار
              </Typography>
              <Typography variant="body2" className="text-neutral-400 text-xs">
                يمكنك إضافة حتى 10 صور للعقار، ويمكن إرفاق فيديو من نافذة الرفع.
              </Typography>
            </div>

            <Button
              variant="contained"
              className="bg-neutral-900 hover:bg-neutral-800 text-white font-medium capitalize rounded-lg px-6 py-2"
            >
              إضافة صور للعقار
            </Button>
          </Box>

          {/* Actions Footer */}
          <div className="flex justify-start gap-3 pt-4 border-t border-neutral-100">
            <Button
              variant="contained"
              className="bg-neutral-900 hover:bg-neutral-800 text-white font-medium rounded-lg px-6"
              onClick={() => saveProperty()}
            >
              إضافة عقار
            </Button>
            <Button
              variant="outlined"
              className="border-neutral-300 text-neutral-600 hover:bg-neutral-50 font-medium rounded-lg px-6"
              //   onClick={onCancel}
            >
              إلغاء
            </Button>
          </div>
        </Paper>
      </div>
    </div>
  );
};

export default EditPropertyById;
