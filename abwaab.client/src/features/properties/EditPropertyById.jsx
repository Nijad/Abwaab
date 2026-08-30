import React, {
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
  memo,
} from "react";
import {
  Box,
  Typography,
  TextField,
  Button,
  Checkbox,
  FormControlLabel,
  Paper,
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

// ==========================================
// 1. MEMOIZED COMPONENTS (Prevents Re-renders)
// ==========================================

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
      const isSelected = selectedIds.some(
        (a) => a.attributeId === att.attributeId
      );
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

const ScheduleGrid = memo(({ weekDaysList, schedules, onScheduleChange }) => {
  if (!schedules) return null;
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-x-6 gap-y-3">
      {weekDaysList.map((day) => (
        <div
          key={day.dayIndex}
          className="flex items-center justify-between gap-2 border-b border-neutral-100 pb-2"
        >
          <FormControlLabel
            control={
              <Checkbox
                checked={schedules[day.dayIndex]?.checked || false}
                onChange={(e) =>
                  onScheduleChange(day.dayIndex, "checked", e.target.checked)
                }
                size="small"
              />
            }
            label={schedules[day.dayIndex]?.name}
            className="min-w-[90px] text-neutral-700"
          />
          <div className="flex items-center gap-2">
            <FormControl sx={{ m: 1, minWidth: 120 }}>
              <InputLabel id={`start-${day.dayIndex}`}>من</InputLabel>
              <Select
                labelId={`start-${day.dayIndex}`}
                value={schedules[day.dayIndex]?.startTime || ""}
                onChange={(e) =>
                  onScheduleChange(day.dayIndex, "startTime", e.target.value)
                }
                autoWidth
                disabled={!schedules[day.dayIndex]?.checked}
                label="من"
                size="small"
              >
                <MenuItem value="">
                  <span>اختر فترة</span>
                </MenuItem>
                {timeSlots().map((t) => (
                  <MenuItem key={t} value={t}>
                    {t}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <FormControl sx={{ m: 1, minWidth: 120 }}>
              <InputLabel id={`end-${day.dayIndex}`}>إلى</InputLabel>
              <Select
                labelId={`end-${day.dayIndex}`}
                value={schedules[day.dayIndex]?.endTime || ""}
                onChange={(e) =>
                  onScheduleChange(day.dayIndex, "endTime", e.target.value)
                }
                disabled={
                  !schedules[day.dayIndex]?.startTime ||
                  !schedules[day.dayIndex]?.checked
                }
                autoWidth
                label="إلى"
                size="small"
              >
                <MenuItem value="">
                  <span>اختر فترة</span>
                </MenuItem>
                {timeSlots(null, schedules[day.dayIndex]?.startTime).map(
                  (t) => (
                    <MenuItem key={t} value={t}>
                      {t}
                    </MenuItem>
                  )
                )}
              </Select>
            </FormControl>
          </div>
        </div>
      ))}
    </div>
  );
});

// ==========================================
// 2. MAIN COMPONENT
// ==========================================

const EditPropertyById = () => {
  const { id } = useParams();

  // SPLIT STATE: Static reference data vs Dynamic form data
  const [staticData, setStaticData] = useState({
    propertyTypesList: [],
    propertyFinishingsList: [],
    weekDaysList: [],
    attributes: [],
    mediaTypes: [],
  });

  const [formData, setFormData] = useState({
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
    propertyAttributesList: [],
  });

  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const [schedules, setSchedules] = useState(null);

  // MEMOIZE EXPENSIVE COMPUTATIONS
  const numberAttributes = useMemo(
    () =>
      staticData.attributes.filter((a) => a.datayTypeDescription === "number"),
    [staticData.attributes]
  );

  const stringAttributes = useMemo(
    () =>
      staticData.attributes.filter((a) => a.datayTypeDescription === "string"),
    [staticData.attributes]
  );

  const boolAttributesNoOrientation = useMemo(
    () =>
      staticData.attributes.filter(
        (a) =>
          a.datayTypeDescription === "boolean" &&
          !orientations.find((b) => a.attributeId === b.attributeId)
      ),
    [staticData.attributes]
  );

  const handleScheduleChange = useCallback((day, field, value) => {
    setSchedules((prev) => ({
      ...prev,
      [day]: { ...prev[day], [field]: value },
    }));
  }, []);

  const fetchProperty = async () => {
    setLoading(true);
    if (signalRef.current) signalRef.current.abort();
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.getPropertyForUpdate(
        id,
        signalRef.current.signal
      );

      // Populate split states
      setStaticData({
        propertyTypesList: resp.data.propertyTypesList || [],
        propertyFinishingsList: resp.data.propertyFinishingsList || [],
        weekDaysList: resp.data.weekDaysList || [],
        attributes: resp.data.attributes || [],
        mediaTypes: resp.data.mediaTypes || [],
      });

      setFormData({
        propertyId: resp.data.propertyId || "",
        title: resp.data.title,
        description: resp.data.description,
        address: resp.data.address,
        areaInSquareMeter: resp.data.areaInSquareMeter,
        price: resp.data.price,
        latitude: resp.data.latitude,
        longitude: resp.data.longitude,
        propertyTypeId: resp.data.propertyTypeId,
        propertyFinishingId: resp.data.propertyFinishingId,
        propertyAttributesList: resp.data.propertyAttributesList || [],
      });

      setSchedules(
        collapseTimeSlots(resp.data.timeSlots, resp.data.weekDaysList)
      );
    } catch (err) {
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
      } else {
        enqueueSnackbar(err.message || err, { variant: "error" });
      }
    } finally {
      setLoading(false);
    }
  };

  const saveProperty = async () => {
    setLoading(true);
    if (signalRef.current) signalRef.current.abort();
    try {
      signalRef.current = new AbortController();
      const timeSlotsArr = [];
      for (const day of staticData.weekDaysList) {
        const dayData = { ...schedules[day.dayIndex] };
        if (dayData?.checked) {
          const tempArr = generateTimeSlots(
            day,
            dayData.startTime,
            dayData.endTime
          );
          timeSlotsArr.push(...tempArr);
        }
      }
      // Merge for API payload
      const propNewData = {
        ...staticData,
        ...formData,
        timeSlots: timeSlotsArr,
      };
      await propertyApi.updateProperty(propNewData, signalRef.current.signal);
      enqueueSnackbar("تم حفظ التعديلات بنجاح", { variant: "success" });
    } catch (err) {
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
      } else {
        enqueueSnackbar(err.message || err, { variant: "error" });
      }
    } finally {
      setLoading(false);
    }
  };

  // STABLE CALLBACKS USING FUNCTIONAL UPDATES
  const handleBoolAttributes = useCallback((data) => {
    setFormData((prev) => {
      const exist = prev.propertyAttributesList.find(
        (a) => a.attributeId === data.attributeId
      );
      if (exist) {
        return {
          ...prev,
          propertyAttributesList: prev.propertyAttributesList.filter(
            (a) => a.attributeId !== data.attributeId
          ),
        };
      }
      const attrb = {
        propertyAttributeId: null,
        value: "true",
        attributeId: data.attributeId,
        attributeName: data.attributeName,
        dataTypeId: data.dataTypeId,
        dataTypeDescription: data.datayTypeDescription,
      };
      return {
        ...prev,
        propertyAttributesList: [...prev.propertyAttributesList, attrb],
      };
    });
  }, []);

  const getAttributeValue = useCallback(
    (id) => {
      const att = formData.propertyAttributesList.find(
        (a) => a.attributeId === id
      );
      return att ? att.value : "";
    },
    [formData.propertyAttributesList]
  );

  const handleInputsAttributes = useCallback((value, data) => {
    setFormData((prev) => {
      const exist = prev.propertyAttributesList.find(
        (a) => a.attributeId === data.attributeId
      );
      if (exist) {
        return {
          ...prev,
          propertyAttributesList: prev.propertyAttributesList.map((a) =>
            a.attributeId === data.attributeId ? { ...a, value: value } : a
          ),
        };
      }
      const attrb = {
        propertyAttributeId: null,
        value: value,
        attributeId: data.attributeId,
        attributeName: data.attributeName,
        dataTypeId: data.dataTypeId,
        dataTypeDescription: data.datayTypeDescription,
      };
      return {
        ...prev,
        propertyAttributesList: [...prev.propertyAttributesList, attrb],
      };
    });
  }, []);

  const getMediaTypeInfo = useCallback(
    (type = "") => {
      if (staticData.mediaTypes.length > 0) {
        const mtype = staticData.mediaTypes.find(
          (t) => t.mediaTypeName.toLowerCase() === type.toLowerCase()
        );
        if (mtype) {
          return {
            mediaTypeId: mtype.mediaTypeId,
            mediaTypeName: mtype.mediaTypeName,
          };
        }
      }
      return { mediaTypeId: "", mediaTypeName: "" };
    },
    [staticData]
  );

  useEffect(() => {
    setTimeout(() => fetchProperty(), 0);
  }, []);

  return (
    <div className="min-h-screen mt-5 flex justify-center w-full">
      <div className="w-full">
        <Paper className="p-6 md:p-8 !rounded-2xl bg-white shadow-xl space-y-8">
          {/* Section 1: Basic Information */}
          <Box>
            <Typography
              variant="h6"
              className="font-bold text-neutral-800 mb-6 text-right"
            >
              المعلومات الأساسية
            </Typography>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6 items-start">
              {/* <div className="md:col-span-1 border-2 border-dashed border-sky-300 rounded-2xl p-6 bg-neutral-50 flex flex-col items-center justify-center text-center cursor-pointer hover:bg-neutral-100 transition-colors h-full min-h-[300px] w-full">
                <div className="w-10 h-10 rounded-full bg-white shadow flex items-center justify-center mb-3">
                  <AddIcon className="text-neutral-600" />
                </div>
                <Typography className="font-bold text-neutral-800 mb-1">
                  إضافة صورة الغلاف
                </Typography>
                <Typography variant="caption" className="text-neutral-400">
                  مطلوبة
                </Typography>
              </div> */}
              <MediaUploader
                propertyId={formData.propertyId}
                mediaInfo={getMediaTypeInfo("image")}
              />

              <div className="md:col-span-2 space-y-5 text-right">
                <div>
                  <Typography
                    variant="body2"
                    className="text-neutral-600 font-medium mb-2"
                  >
                    نوع العقار
                  </Typography>
                  <ToggleButtonGroup
                    items={staticData.propertyTypesList}
                    selectedId={formData.propertyTypeId}
                    onSelect={(val) =>
                      setFormData((prev) => ({ ...prev, propertyTypeId: val }))
                    }
                    valueKey="typeId"
                    labelKey="typeName"
                  />
                </div>
                <div>
                  <Typography
                    variant="body2"
                    className="text-neutral-600 font-medium mb-2"
                  >
                    حالة الإكساء
                  </Typography>
                  <ToggleButtonGroup
                    items={staticData.propertyFinishingsList}
                    selectedId={formData.propertyFinishingId}
                    onSelect={(val) =>
                      setFormData((prev) => ({
                        ...prev,
                        propertyFinishingId: val,
                      }))
                    }
                    valueKey="finishingId"
                    labelKey="finishingName"
                  />
                </div>
                <div>
                  <Typography
                    variant="body2"
                    className="text-neutral-600 font-medium mb-2"
                  >
                    اتجاهات العقار (اختياري)
                  </Typography>
                  <BoolToggleButtonGroup
                    items={orientations}
                    selectedIds={formData.propertyAttributesList}
                    onToggle={handleBoolAttributes}
                  />
                </div>
              </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
              <TextField
                placeholder="وصف مختصر"
                label="وصف مختصر"
                variant="filled"
                fullWidth
                value={formData.title || ""}
                name="title"
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    [e.target.name]: e.target.value,
                  }))
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
                value={formData.areaInSquareMeter || ""}
                name="areaInSquareMeter"
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    [e.target.name]: e.target.value,
                  }))
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
                value={formData.price || ""}
                name="price"
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    [e.target.name]: e.target.value,
                  }))
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
                value={formData.address || ""}
                name="address"
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    [e.target.name]: e.target.value,
                  }))
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
            <div className="flex flex-col justify-between">
              <TextField
                placeholder="تفاصيل العقار"
                multiline
                rows={7}
                variant="outlined"
                fullWidth
                value={formData.description || ""}
                label="تفاصيل العقار"
                name="description"
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    [e.target.name]: e.target.value,
                  }))
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
            <div className="space-y-4">
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
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
              {numberAttributes.map((att) => (
                <TextField
                  key={att.attributeId}
                  placeholder={att.attributeName}
                  type="number"
                  variant="filled"
                  fullWidth
                  value={getAttributeValue(att.attributeId)}
                  onChange={(e) => handleInputsAttributes(e.target.value, att)}
                  InputProps={{
                    disableUnderline: true,
                    className: "rounded-lg bg-neutral-100/70",
                  }}
                />
              ))}
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
              {stringAttributes.map((att) => (
                <TextField
                  key={att.attributeId}
                  placeholder={att.attributeName}
                  type="text"
                  variant="filled"
                  fullWidth
                  value={getAttributeValue(att.attributeId)}
                  onChange={(e) => handleInputsAttributes(e.target.value, att)}
                  InputProps={{
                    disableUnderline: true,
                    className: "rounded-lg bg-neutral-100/70",
                  }}
                />
              ))}
            </div>
            <BoolToggleButtonGroup
              items={boolAttributesNoOrientation}
              selectedIds={formData.propertyAttributesList}
              onToggle={handleBoolAttributes}
            />
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
            <ScheduleGrid
              weekDaysList={staticData.weekDaysList}
              schedules={schedules}
              onScheduleChange={handleScheduleChange}
            />
          </Box>

          {/* Section 5: Property Photos & Videos */}
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
              onClick={saveProperty}
              disabled={loading}
              loading={loading}
            >
              {loading ? "جاري الحفظ..." : "حفظ التعديلات"}
            </Button>
            <Button
              variant="outlined"
              className="border-neutral-300 text-neutral-600 hover:bg-neutral-50 font-medium rounded-lg px-6"
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
