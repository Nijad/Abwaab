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
  Skeleton,
  IconButton,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import MapIcon from "@mui/icons-material/Map";
import { useNavigate, useParams } from "react-router";
import { useSnackbar } from "notistack";
import { propertyApi } from "../../api";
import {
  collapseTimeSlots,
  generateTimeSlots,
  timeSlots,
} from "../../utils/helpers";
import MediaUploader from "./MediaUploader";
import { LocationPicker } from "../../components/LocationPicker";
import {
  PROPERTY_GET_DATA,
  PROPERTY_GET_LISTS,
  PROPERTY_UPDATE_DATA,
  ORIENTATIONS,
  PROPERTY_MEDIA,
} from "../../dataTypes/propertis";
import { Close } from "@mui/icons-material";
import MediaDelete from "./MediaDelete";
import LabelTag from "../../components/LabelTag";
import ShowErrors from "../../components/ShowErrors";

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
  const navigate = useNavigate();
  // SPLIT STATE: Static reference data vs Dynamic form data
  const [staticData, setStaticData] = useState(PROPERTY_GET_LISTS);
  const [formData, setFormData] = useState(PROPERTY_GET_DATA);
  const [media, setMedia] = useState([PROPERTY_MEDIA]);

  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const [schedules, setSchedules] = useState(null);

  console.log(media);

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
          !ORIENTATIONS.find((b) => a.attributeId === b.attributeId)
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
      const listState = PROPERTY_GET_LISTS;
      const formState = PROPERTY_GET_DATA;
      for (const [key, value] of Object.entries(resp.data)) {
        if (key in listState) {
          listState[key] = value || [];
          continue;
        }
        if (key in formState) formState[key] = value;
      }

      // Populate split states
      setStaticData(listState);
      setFormData(formState);
      setMedia(resp.data.propertyMediaList);
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

  const updateProperty = async () => {
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
      const updateData = { ...PROPERTY_UPDATE_DATA };
      for (const [key, value] of Object.entries(formData)) {
        if (key in updateData) {
          updateData[key] = value;
        }
      }
      updateData.timeSlots = timeSlotsArr;
      updateData.propertyMediaList = [...media];
      await propertyApi.updateProperty(updateData, signalRef.current.signal);
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

  const submitProperty = async () => {
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
      const updateData = { ...PROPERTY_UPDATE_DATA };
      for (const [key, value] of Object.entries(formData)) {
        if (key in updateData) {
          updateData[key] = value;
        }
      }
      updateData.timeSlots = timeSlotsArr;
      updateData.propertyMediaList = [...media];
      await propertyApi.submitProperty(updateData, signalRef.current.signal);
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

  // const getCoverImage = useCallback(() => {
  //   var img = formData.propertyMediaList.find((m) => m.isCover === true);
  //   if (img) {
  //     return {
  //       id: img.mediaId,
  //       filePath: `${import.meta.env.VITE_API_BASE_URL}${img.filePath}`,
  //     };
  //   }
  //   return null;
  // }, [formData]);
  const findCoverImage = media.find((i) => i.isCover === true) || null;
  const getCoverImage = findCoverImage && {
    id: findCoverImage.mediaId,
    filePath: `${import.meta.env.VITE_API_BASE_URL}${findCoverImage.filePath}`,
  };
  const imagesWithoutCover = media.filter(
    (m) =>
      m.isCover === false && m.mediaTypeName.toLocaleLowerCase() === "image"
  );
  const videosWithoutCover = media.filter(
    (m) => m.mediaTypeName.toLocaleLowerCase() === "video"
  );

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

  const handleSelectLocation = (data) => {
    console.log(data);
    setFormData({ ...formData, longitude: data.lng, latitude: data.lat });
  };

  const handleUploadImage = (data) => {
    console.log("data:", data);

    const newMedia = [...media];
    newMedia.push(data);
    setMedia(newMedia);
    setFormData((f) => ({
      ...f,
      remainingImagesAllowed: f.remainingImagesAllowed - 1,
    }));
  };

  const handleDeleteImage = (id) => {
    const newMedia = media.filter((m) => m.mediaId !== id);
    setMedia(newMedia);
    setFormData((f) => ({
      ...f,
      remainingImagesAllowed: f.remainingImagesAllowed + 1,
    }));
  };

  useEffect(() => {
    setTimeout(() => fetchProperty(), 0);
  }, []);

  if (loading) {
    return (
      <div className="flex flex-nowrap w-full gap-3">
        <div className="w-full">
          <Skeleton
            variant="rounded"
            width={"100%"}
            height={"80vh"}
            sx={{ borderRadius: "18px", marginX: "5px" }}
          />
        </div>
      </div>
    );
  }

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
              <MediaUploader
                key={"cover-uploade"}
                propertyId={formData.propertyId}
                mediaInfo={getMediaTypeInfo("image")}
                image={getCoverImage}
                required={true}
                isCover={true}
                onUploaded={handleUploadImage}
                onDeleted={handleDeleteImage}
              />

              <div className="md:col-span-2 space-y-5 text-right">
                <div>
                  <Typography
                    variant="body2"
                    className="text-neutral-600 font-medium mb-2"
                  >
                    حالة العقار
                  </Typography>
                  <LabelTag
                    label={formData.propertyState}
                    classes="my-2 px-3 bg-teal-500 text-white max-h-8 mb-3"
                  />
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
                    items={ORIENTATIONS}
                    selectedIds={formData.propertyAttributesList}
                    onToggle={handleBoolAttributes}
                  />
                </div>
              </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
              <TextField
                error={errors == null ? false : errors["Title"] ? true : false}
                helperText={ShowErrors({ object: errors, key: "Title" })}
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
              />
              <TextField
                error={
                  errors == null
                    ? false
                    : errors["AreaInSquareMeter"]
                    ? true
                    : false
                }
                helperText={ShowErrors({
                  object: errors,
                  key: "AreaInSquareMeter",
                })}
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
                    [e.target.name]: parseFloat(e.target.value),
                  }))
                }
              />
              <TextField
                error={errors == null ? false : errors["Price"] ? true : false}
                helperText={ShowErrors({
                  object: errors,
                  key: "Price",
                })}
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
                    [e.target.name]: parseFloat(e.target.value),
                  }))
                }
              />
              <TextField
                error={
                  errors == null ? false : errors["Address"] ? true : false
                }
                helperText={ShowErrors({
                  object: errors,
                  key: "Address",
                })}
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
              />
            </div>
          </Box>

          {/* Section 2: Address, Map & Description */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="flex flex-col justify-between">
              <TextField
                error={
                  errors == null ? false : errors["Description"] ? true : false
                }
                helperText={ShowErrors({
                  object: errors,
                  key: "Description",
                })}
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
              {formData.latitude && formData.longitude && (
                <LocationPicker
                  onLocationSelect={handleSelectLocation}
                  lat={formData.latitude}
                  lng={formData.longitude}
                />
              )}
              {!formData.latitude && !formData.longitude && (
                <LocationPicker onLocationSelect={handleSelectLocation} />
              )}
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
                  name={att.attributeId}
                  value={getAttributeValue(att.attributeId)}
                  onChange={(e) => handleInputsAttributes(e.target.value, att)}
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
                  name={att.attributeId}
                  value={getAttributeValue(att.attributeId)}
                  onChange={(e) => handleInputsAttributes(e.target.value, att)}
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
                لديك عدد {formData.remainingImagesAllowed} صور متاحة للعقار بحسب
                الخطة المفعلة على حسابك.
              </Typography>
            </div>
            <div className="flex flex-nowrap items-center justify-start max- overflow-x-scroll gap-2">
              {imagesWithoutCover.map((m) => (
                <div
                  key={m.mediaId}
                  className="relative min-w-24 max-w-32 h-32 rounded-2xl overflow-hidden"
                >
                  <img
                    src={`${import.meta.env.VITE_API_BASE_URL}${m.filePath}`}
                    alt={m.mediaTypeName}
                    className="w-full h-full object-cover"
                  />
                  <div className="absolute top-0 left-0 p-1 -translate-x-ful">
                    <MediaDelete
                      id={m.mediaId}
                      onDeleted={handleDeleteImage}
                      key={m.mediaId}
                    />
                  </div>
                </div>
              ))}
              <div className="min-w-28">
                <MediaUploader
                  key={"add-images"}
                  propertyId={formData.propertyId}
                  mediaInfo={getMediaTypeInfo("image")}
                  // image={getCoverImage()}
                  required={false}
                  isCover={false}
                  onUploaded={handleUploadImage}
                  onDeleted={handleDeleteImage}
                />
              </div>
            </div>
          </Box>

          {/* Actions Footer */}
          <div className="flex justify-between gap-3 pt-4 border-t border-neutral-100">
            <div className="">
              <Button
                variant="contained"
                className="bg-neutral-900 hover:bg-neutral-800 text-white font-medium rounded-lg px-6 !me-3"
                onClick={updateProperty}
                disabled={loading}
                loading={loading}
                color="navy"
              >
                {loading ? "جاري الحفظ..." : "حفظ التعديلات"}
              </Button>
              <Button
                variant="outlined"
                className="border-neutral-300 text-neutral-600 hover:bg-neutral-50 font-medium rounded-lg px-6"
                color="navy"
                onClick={() => navigate("/portal/my-properties")}
              >
                إلغاء
              </Button>
            </div>
            <div className="">
              <span className="mx-3">جاهز للنشر؟</span>
              <Button
                variant="contained"
                className="bg-neutral-900 hover:bg-neutral-800 text-white font-medium rounded-lg px-6"
                onClick={submitProperty}
                disabled={loading}
                loading={loading}
                color="navy"
              >
                انشر الآن
              </Button>
            </div>
          </div>
        </Paper>
      </div>
    </div>
  );
};

export default EditPropertyById;
